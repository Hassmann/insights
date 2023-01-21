﻿namespace SLD.Insights
{
	public class Insight
	{
		internal const TraceLevel DefaultLevel = TraceLevel.Info;

		public Insight(TraceLevel level = DefaultLevel)
		{
			Level = level;
		}

		public TraceLevel Level { get; }
		public DateTimeOffset TimeStamp { get; set; } = DateTimeOffset.Now;
		public bool IsHighlight { get; set; }

		public string Source { get; set; }
		public string Text { get; set; }
		public object[] Payload { get; set; }

		public Exception Exception { get; set; }

		public TimeSpan Time
			=> TimeStamp - InsightsSource.StartTime;

		public bool IsError
			=> Level == TraceLevel.Error;

		public override string ToString()
			=> $"{Source}: {Text}";
	}
}