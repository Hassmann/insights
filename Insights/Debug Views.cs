namespace SLD.Insights
{
	[DebuggerDisplay("{Name} ({DisplayLevel,nq})")]
	[DebuggerTypeProxy(typeof(DebugView))]
	public partial class InsightsSource
	{
		private class DebugView
		{
			InsightsSource the;

			public DebugView(InsightsSource insights)
			{
				the = insights;
			}

			public string Name => the.Name;
			public TraceLevel DisplayLevel => the.DisplayLevel;

			public static DateTime StartTime => InsightsSource.StartTime;
		}
	}
}
