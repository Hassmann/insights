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

		public void Trace(string text, object payload = null)
			=> Send(text, TraceLevel.Verbose, payload);

		public void Info(string text, object payload = null)
			=> Send(text, TraceLevel.Info, payload);

		public void Warning(string text, object payload = null)
			=> Send(text, TraceLevel.Warning, payload);

		public void Error(string text, Exception e = null)
			=> Send(text, TraceLevel.Error, exception: e);

		public void Send(string text, TraceLevel level = Insight.DefaultLevel, object payload = null, Exception exception = null)
			=> Send(() => new Insight(level)
			{
				Text = text,
				Payload = payload,
				Exception = exception

			}, level);

		public void Send(Func<Insight> createInsight, TraceLevel level = Insight.DefaultLevel)
		{
			if (IsEnabled(null, level))
			{
				try
				{
					var insight = createInsight();

					insight.Source = Name;

					Write(InsightName, createInsight());
				}
				catch (Exception e)
				{
					Error("Could not create Insight", e);
				}
			}
		}
	}
}