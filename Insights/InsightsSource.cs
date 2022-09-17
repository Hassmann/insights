using System;
using System.Diagnostics;

namespace SLD.Insights
{
	public partial class InsightsSource : DiagnosticListener
	{
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
		{
			if(IsEnabled(null, level))
			{
				WriteTo(this, text, level, payload, exception);
			}
		}

		static void WriteTo(DiagnosticSource source, string text, TraceLevel level = Insight.DefaultLevel, object payload = null, Exception exception = null)
			=> source.Write(text, new Insight
			{
				Text = text,
				Level = level,
				Payload = payload,
				Exception = exception
			});

	}
}