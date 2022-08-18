using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;


namespace SLD.Insights
{

	using Configuration;
	using Output;
	using System.Diagnostics;

	public partial class InsightsSource
	{
		const string ConfigurationFileName = "appsettings.json";

		static readonly DateTime _startTime = DateTime.Now;

		// Settings for each configured source
		static readonly Dictionary<string, SourceSettings> _sources = new Dictionary<string, SourceSettings>();

		// Global output of insights
		static readonly Subject<Insight> _output = new Subject<Insight>();

		// Trace Insights itself?
		static TraceLevel _insightsLevel = TraceLevel.Info;

		static InsightsSource()
		{
			TraceSelf("Initialize");

			InsightsSettings settings = null;

			// Configuration
			IConfigurationRoot configuration = GetConfiguration();

			if (configuration != null)
			{
				IConfigurationSection section = configuration.GetSection("Insights");

				if (section != null)
				{
					settings = section.Get<InsightsSettings>();

					if (settings != null && settings.Sources != null)
					{
						ApplySettings(settings);

						TraceSelf($"Configured Sources: {string.Join(", ", settings.Sources.Select(source => source.Name))}");
					}
					else
					{
						TraceSelf("No Sources configured", TraceLevel.Warning);
					}
				}
				else
				{
					TraceSelf("No 'Insights' section in configuration", TraceLevel.Warning);
				}
			}
			else
			{
				TraceSelf("No configuration file found", TraceLevel.Warning);
			}

			// Infos
			TraceSelf($"BasePath: {BasePath}");

			// Listen to insights
			_output.Subscribe(new TraceObserver(settings));

			// Listen to source registrations
			DiagnosticListener.AllListeners.Subscribe(OnSourceRegistered);
		}

		public static IObservable<Insight> Insights
			=> _output;

		public static readonly string BasePath
			= AppDomain.CurrentDomain.BaseDirectory;

		static IConfigurationRoot GetConfiguration()
		{


			if (File.Exists(Path.Combine(BasePath, ConfigurationFileName)))
			{
				return new ConfigurationBuilder()
					.SetBasePath(BasePath)
					.AddJsonFile(ConfigurationFileName)
					.Build();
			}

			return null;
		}

		static void ApplySettings(InsightsSettings settings)
		{
			foreach (SourceSettings source in settings.Sources)
			{
				if (source.Name == "Insights")
				{
					_insightsLevel = source.Level;
				}
				else
				{
					_sources.Add(source.Name, source);
				}
			}
		}

		static void OnSourceRegistered(DiagnosticListener listener)
		{
			TraceSelf($"Available: {listener.Name}", TraceLevel.Verbose);

			if (_sources.TryGetValue(listener.Name, out SourceSettings settings) && settings.Level != TraceLevel.Off)
			{
				listener
					.Select(pair => (Insight)pair.Value)
					.Where(insight => settings.Level >= insight.Level)
					.Subscribe(insight => OnInsightReceived(listener, insight));
			}
		}

		static void OnInsightReceived(DiagnosticListener listener, Insight insight)
		{
			// Never trace in here!
			insight.Source = listener.Name;
			insight.Time = DateTime.Now - _startTime;

			_output.OnNext(insight);
		}

		static void TraceSelf(string text, TraceLevel level = TraceLevel.Info)
		{
			if (_insightsLevel >= level)
			{
				TraceOutput.Write(new Insight
				{
					Source = "Insights",
					Text = text,
					Level = level,
					Time = DateTime.Now - _startTime
				});
			}
		}
	}
}