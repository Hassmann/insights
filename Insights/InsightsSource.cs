using System;
using System.Diagnostics;

namespace SLD.Insights
{
	public partial class InsightsSource : DiagnosticListener
	{
		private const string InsightName = null;

		public InsightsSource(string area) : base(area.Trim())
		{
		}

		public InsightsSource(object source) : this(source.ToString())
		{
		}

		public void Trace(string text, params object[] payload)
			=> Log(text, TraceLevel.Verbose, null, payload);

		public void Info(string text, params object[] payload)
			=> Log(text, TraceLevel.Info, null, payload);

		public void Warning(string text, params object[] payload)
			=> Log(text, TraceLevel.Warning, null, payload);

		public void Error(string text, Exception e = null, params object[] payload)
			=> Log(text, TraceLevel.Error, exception: e, payload);

		public void Log(string text, TraceLevel level = Insight.DefaultLevel, Exception exception = null, params object[] payload)
			=> Log(() => new Insight(level)
			{
				Text = text,
				Payload = payload,
				Exception = exception

			}, level);

		public void Log(Func<Insight> createInsight, TraceLevel level = Insight.DefaultLevel)
		{
			if (IsEnabled(null, level))
			{
				try
				{
					var insight = createInsight();

					insight.Source = Name;

					Write(InsightName, insight);
				}
				catch (Exception e)
				{
					Error("Could not create Insight", e);
				}
			}
		}
	}
}