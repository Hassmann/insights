using System;

namespace SLD.Insights.Output
{

	using Configuration;

	class TraceObserver : IObserver<Insight>
	{
		readonly InsightsSettings _settings;

		public TraceObserver(InsightsSettings settings)
		{
			_settings = settings;
		}

		public void OnCompleted() => throw new NotImplementedException();

		public void OnError(Exception error) => throw new NotImplementedException();

		public void OnNext(Insight insight)
		{
			TraceOutput.Write(insight);

			if (insight.Exception != null && _settings.DumpExceptions)
			{
				TraceOutput.Dump(insight.Exception);
			}
		}
	}
}