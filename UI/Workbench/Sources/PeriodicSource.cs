using SLD.Insights;
using System;
using System.Reactive;
using System.Reactive.Linq;

namespace Workbench.Sources
{
	public class PeriodicSource
	{
		IDisposable _timer;
		InsightsSource Insights = new InsightsSource("Periodic");

		public PeriodicSource()
		{
			_timer = Observable
			.Interval(TimeSpan.FromSeconds(1))
			.Subscribe(number => Insights.Info($"Periodic #{number}"));
		}
	}
}
