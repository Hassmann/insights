namespace SLD.Insights
{
	public static class InsightsExtensions
	{
		private class TraceObservable<T> : IObservable<T>
		{
			private readonly IObservable<T> _source;
			private readonly string _name;
			private readonly InsightsSource _insights;

			public TraceObservable(IObservable<T> source, InsightsSource insights, string name)
			{
				_source = source;
				_insights = insights;
				_name = name;
			}

			IDisposable IObservable<T>.Subscribe(IObserver<T> observer)
				=> _source.Subscribe(
					item =>
					{
						_insights.Trace($"{_name} | → {item}");
						observer.OnNext(item);
					},
					e =>
					{
						_insights.Error($"{_name} | ERROR | {e}", e);
						observer.OnError(e);
					},
					() =>
					{
						_insights.Trace($"{_name} | Completed");
						observer.OnCompleted();
					});
		}

		public static IObservable<T> Trace<T>(this IObservable<T> source, InsightsSource insights, string name)
			=> new TraceObservable<T>(source, insights, name);
	}
}