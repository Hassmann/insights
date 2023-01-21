using ReactiveUI;
using System;

namespace SLD.Insights.UI.ViewModels
{
	public class LogVM : ViewModel
	{
		private static InsightsSource Insights = new InsightsSource("LogVM");

		private LogFrameVM _currentFrame = new LogFrameVM();

		public LogVM()
		{
			// Pipe all insights into the log
			InsightsSource.Insights
				.Subscribe(
					OnNext,
					OnError,
					OnCompleted
				);

			Insights.Info("Insights subscribed to display");
		}

		public LogFrameVM CurrentFrame
		{
			get => _currentFrame;
			set => this.RaiseAndSetIfChanged(ref _currentFrame, value);
		}

		private void OnCompleted() => throw new NotImplementedException();

		private void OnError(Exception error) => throw new NotImplementedException();

		private void OnNext(Insight insight)
		{
			var item = new InsightVM(insight);


		}
	}
}