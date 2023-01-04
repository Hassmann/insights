namespace SLD.Insights.Output
{
	using Configuration;

	public class TraceObserver : IObserver<Insight>
	{
		private readonly InsightsSettings _settings;

		private DateTimeOffset _lastTimestamp = DateTimeOffset.MaxValue;

		public TraceObserver(InsightsSettings settings)
		{
			_settings = settings;
		}

		public void OnCompleted()
			=> TraceOutput.Write(new Insight(TraceLevel.Info)
			{
				Source = nameof(TraceObserver),
				Text = "Completed - no more traces"
			});

		public void OnError(Exception error)
			=> TraceOutput.Write(new Insight(TraceLevel.Error)
			{
				Source = nameof(TraceObserver),
				Exception = error,
				Text = "Failure: " + error.ToTrace()
			});

		public void OnNext(Insight insight)
		{
			if (insight.TimeStamp - _lastTimestamp > _settings.IdleThreshold)
			{
				TraceOutput.Write("---");
			}

			_lastTimestamp = insight.TimeStamp;

			TraceOutput.Write(insight);

			if (insight.Exception != null && _settings.DumpExceptions)
			{
				TraceOutput.Dump(insight.Exception);
			}
		}
	}
}