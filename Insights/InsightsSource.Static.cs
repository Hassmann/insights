﻿using System.Reactive.Subjects;

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

		// Default Level for Source, if not configured
		private static TraceLevel _defaultSourceLevel;

		static InsightsSource()
		{
			TraceSelf("Initialize");

			InsightsSettings settings = FindAnySettings();

			if (settings.HasSources)
			{
				ApplySettings(settings);
				TraceSelf($"Configured Sources: {string.Join(", ", settings.ConfiguredSources)}", isHighlight: true);
			}
			else
			{
				TraceSelf("No Sources configured", TraceLevel.Warning);
			}

			// Infos
			TraceSelf($"BasePath: {BasePath}");

			SubscribeToOutput(settings);

			// Listen to source registrations
			AllListeners.Subscribe(OnSourceRegistered);
		}

		public static IObservable<Insight> Insights
			=> _sink.Select(pair => pair.Value as Insight);

		public static void ApplySettings(InsightsSettings settings)
		{
			_sources.Clear();
			_defaultSourceLevel = settings.DefaultLevel;

			// Start with deprecated format
			foreach (SourceSettings source in settings.Sources.IfAny())
			{
				ApplySourceLevel(source.Name, source.Level);
			}

			// Overwrite with Dictionary style
			foreach (var pair in settings.Levels.IfAny())
			{
				ApplySourceLevel(pair.Key, pair.Value);
			}
		}

		private static void SubscribeToOutput(InsightsSettings settings)
		{
			var output = new TraceObserver(settings);

			Insights
				.Subscribe(output);
		}

		private static void SetDisplayLevel(InsightsSource source, TraceLevel level)
		{
			TraceSelf($"{source.Name}: {level}");

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

				source.Subscribe(_sink, (string name, object level, object ignored) => IsSourceEnabled(source, (TraceLevel)level));
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

		private static bool IsSourceEnabled(InsightsSource source, TraceLevel level)
		{
			if (_sources.TryGetValue(source.Name, out var found))
			{
				return found.Level >= level;
			}

			return _defaultSourceLevel >= level;
		}

		private static void ApplySourceLevel(string source, TraceLevel level)
		{
			if (source == "Insights")
			{
				_insightsLevel = level;
			}
			else
			{
				if (_sources.ContainsKey(source))
				{
					TraceSelf($"Defined more than once: {source}", TraceLevel.Warning);
				}

				_sources[source] = new SourceSettings
				{
					Level = level,
					Name = source
				};
			}
		}

		private static void OnSourceRegistered(DiagnosticListener listener)
		{
			if (listener is InsightsSource insights)
			{
				SourceSettings settings = null;

				if (_sources.TryGetValue(insights.Name, out settings) && settings.Level != TraceLevel.Off)
				{
					insights.Subscribe(_sink, (string name, object level, object ignored) => IsSourceEnabled(insights, (TraceLevel)level));
				}

				if (settings is null)
				{
					WriteHighlight($"{listener.Name}: Unconfigured", TraceLevel.Info);
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