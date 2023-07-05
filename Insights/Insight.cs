namespace SLD.Insights
{
	[DebuggerDisplay("{Source, nq} | {Text, nq}")]
	public class Insight
	{
		internal const TraceLevel DefaultLevel = TraceLevel.Info;

		public Insight(TraceLevel level = DefaultLevel)
		{
			Level = level;
		}

		public TraceLevel Level { get; }
		public DateTimeOffset TimeStamp { get; } = DateTimeOffset.Now;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsHighlight { get; internal set; }

		public string Source { get; internal set; }
		public string Text { get; set; }
		public object[] Payload { get; set; }

		public Exception Exception { get; set; }

		public TimeSpan Time
			=> TimeStamp - InsightsSource.StartTime;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsError
			=> Level == TraceLevel.Error;
	}
}