using System;
using System.Diagnostics;

namespace SLD.Insights
{

	public class Insight
	{
		internal const TraceLevel DefaultLevel = TraceLevel.Info;

		public string Source { get; set; }
		public string Text { get; set; }
		public object Payload { get; set; }

		public TraceLevel Level { get; set; } = DefaultLevel;

		public Exception Exception { get; set; }

		public TimeSpan Time { get; set; }

		public bool IsError
			=> Exception != null;
	}
}