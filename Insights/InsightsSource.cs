using System;
using System.Diagnostics;

namespace SLD.Insights
{
	public partial class InsightsSource
	{
		readonly DiagnosticSource _source;

		public InsightsSource(string area)
		{
			_source = new DiagnosticListener(area);
		}

		public InsightsSource(object source) : this(source.ToString())
		{
		}

		public void Trace(string text, object payload = null)
			=> Write(text, TraceLevel.Verbose, payload);

		public void Info(string text, object payload = null)
			=> Write(text, TraceLevel.Info, payload);

		public void Warning(string text, object payload = null)
			=> Write(text, TraceLevel.Warning, payload);

		public void Error(string text, Exception e = null)
			=> Write(text, TraceLevel.Error, exception: e);

		public void Write(string text, TraceLevel level = Insight.DefaultLevel, object payload = null, Exception exception = null)
			=> WriteTo(_source, text, level, payload, exception);

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