using System;
using System.Diagnostics;

namespace SLD.Insights.Output
{
	public class ConsoleObserver : IObserver<Insight>
	{
		private bool _dumpExceptions;

		public ConsoleObserver(bool dumpExceptions = true)
		{
			_dumpExceptions = dumpExceptions;
		}

		public void OnCompleted()
			=> OnNext(new Insight(TraceLevel.Info)
			{
				Source = nameof(TraceObserver),
				Text = "Completed - no more traces"
			});

		public void OnError(Exception error)
			=> OnNext(new Insight(TraceLevel.Error)
			{
				Source = nameof(TraceObserver),
				Exception = error,
				Text = "Trace stream failed"
			});

		public void OnNext(Insight insight)
		{
			var old = Console.ForegroundColor;

			switch (insight.Level)
			{
				case TraceLevel.Error:
					Console.ForegroundColor = ConsoleColor.Red;
					break;
				case TraceLevel.Warning:
					Console.ForegroundColor = ConsoleColor.DarkYellow;
					break;
				default:
					break;
			}

			Console.WriteLine(insight.ToTrace());

			if (insight.Exception != null && _dumpExceptions)
			{
				Console.WriteLine(insight.Exception.ToTrace());
			}

			Console.ForegroundColor = old;
		}
	}
}