﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace SLD.Insights
{
	using Configuration;
	using Output;

	public partial class InsightsSource
	{
		public static readonly string BasePath
			= AppDomain.CurrentDomain.BaseDirectory;

		internal static readonly DateTime StartTime = DateTime.Now;

		// Settings for each configured source
		private static readonly Dictionary<string, SourceSettings> _sources = new Dictionary<string, SourceSettings>();

		// Global output of insights
		private static readonly Subject<KeyValuePair<string, object>> _sink = new Subject<KeyValuePair<string, object>>();

		// Trace Insights itself?
		private static TraceLevel _insightsLevel = TraceLevel.Info;

		static InsightsSource()
		{
			TraceSelf("Initialize");

			InsightsSettings settings = FindAnySettings();

			if (settings.Sources != null && settings.Sources.Any())
			{
				WriteHighlight($"Configured Sources: {string.Join(", ", settings.Sources?.Select(source => source.Name))}");
				ApplySettings(settings);
			}
			else
			{
				TraceSelf("No Sources configured", TraceLevel.Warning);
			}

			// Infos
			TraceSelf($"BasePath: {BasePath}");

			// Listen to insights
			Insights.Subscribe(new TraceObserver(settings));

			// Listen to source registrations
			AllListeners.Subscribe(OnSourceRegistered);
		}

		public static IObservable<Insight> Insights
			=> _sink.Select(pair => pair.Value as Insight);

		private static void SetDisplayLevel(InsightsSource source, TraceLevel level)
		{
			if (_sources.TryGetValue(source.Name, out var found))
			{
				found.Level = level;
			}
			else
			{
				var settings = new SourceSettings
				{
					Name = source.Name,
					Level = level
				};

				_sources[source.Name] = settings;

				source.Subscribe(_sink, settings.IsEnabled);
			}
		}

		private static TraceLevel GetDisplayLevel(InsightsSource source)
		{
			if (_sources.TryGetValue(source.Name, out var found))
			{
				return found.Level;
			}

			return Insight.DefaultLevel;
		}

		private static void ApplySettings(InsightsSettings settings)
		{
			foreach (SourceSettings source in settings.Sources)
			{
				if (source.Name == "Insights")
				{
					_insightsLevel = source.Level;
				}
				else
				{
					if (_sources.ContainsKey(source.Name))
					{
						TraceSelf($"Defined more than once: {source.Name}", TraceLevel.Warning);
					}

					_sources[source.Name] = source;
				}
			}
		}

		private static void OnSourceRegistered(DiagnosticListener listener)
		{
			if (listener is InsightsSource insights)
			{
				SourceSettings settings = null;

				if (_sources.TryGetValue(insights.Name, out settings) && settings.Level != TraceLevel.Off)
				{
					insights.Subscribe(_sink, settings.IsEnabled);
				}

				if (settings is null)
				{
					WriteHighlight($"{listener.Name}: Unconfigured");
				}
				else
				{
					WriteHighlight($"{listener.Name}: {settings.Level}", TraceLevel.Verbose);
				}
			}
		}

		private static void WriteHighlight(string text, TraceLevel level = TraceLevel.Info)
			=> TraceSelf(text, level, true);

		private static void TraceSelf(string text, TraceLevel level = TraceLevel.Info, bool isHighlight = false)
		{
			if (_insightsLevel >= level)
			{
				TraceOutput.Write(new Insight(level)
				{
					Source = "Insights",
					Text = text,
					IsHighlight = isHighlight
				});
			}
		}
	}
}