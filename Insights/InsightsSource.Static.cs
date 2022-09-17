﻿using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

		private static readonly DateTime _startTime = DateTime.Now;

		// Settings for each configured source
		private static readonly Dictionary<string, SourceSettings> _sources = new Dictionary<string, SourceSettings>();

		// Global output of insights
		private static readonly Subject<Insight> _output = new Subject<Insight>();

		// Trace Insights itself?
		private static TraceLevel _insightsLevel = TraceLevel.Info;

		static InsightsSource()
		{
			TraceSelf("Initialize");

			InsightsSettings settings = FindAnySettings();

			if (settings.Sources != null && settings.Sources.Any())
			{
				WriteHighlight($"Configured Sources: {string.Join(", ", settings.Sources.Select(source => source.Name))}");
				ApplySettings(settings);
			}
			else
			{
				TraceSelf("No Sources configured", TraceLevel.Warning);
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
					_sources.Add(source.Name, source);
				}
			}
		}

		private static void OnSourceRegistered(DiagnosticListener listener)
		{
			SourceSettings settings = null;

			if (_sources.TryGetValue(listener.Name, out settings) && settings.Level != TraceLevel.Off)
			{
				listener
					.Select(pair => pair.Value)
					.OfType<Insight>()
					.Where(insight => settings.Level >= insight.Level)
					.Subscribe(insight => OnInsightReceived(listener, insight));
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

		private static void OnInsightReceived(DiagnosticListener listener, Insight insight)
		{
			// Never trace in here!
			insight.Source = listener.Name;
			insight.Time = DateTime.Now - _startTime;

			_output.OnNext(insight);
		}

		private static void WriteHighlight(string text, TraceLevel level = TraceLevel.Info)
			=> TraceSelf(text, level, true);

		private static void TraceSelf(string text, TraceLevel level = TraceLevel.Info, bool isHighlight = false)
		{
			if (_insightsLevel >= level)
			{
				TraceOutput.Write(new Insight
				{
					Source = "Insights",
					Text = text,
					Level = level,
					Time = DateTime.Now - _startTime,
					IsHighlight = isHighlight
				});
			}
		}
	}
}