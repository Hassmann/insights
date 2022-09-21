using SLD.Insights;
using SLD.Insights.Output;
using System;
using System.Threading.Tasks;

namespace PlainApp;

internal class Program
{
	private static InsightsSource Insights = new InsightsSource("Plain App");

	private static void RaiseException()
	{
		try
		{
			throw new IndexOutOfRangeException("Inner");
		}
		catch (Exception e)
		{
			throw new InvalidOperationException("Outer", e);
		}
	}

	private static async Task Main(string[] args)
	{
		InsightsSource.Insights
			.Subscribe(new ConsoleObserver());

		Insights.Info("Executing");

		Insights.Warning("Test Warning");

		await Task.Delay(TimeSpan.FromSeconds(1));

		Insights.Error("Test Error");

		try
		{
			RaiseException();
		}
		catch (Exception e)
		{
			Insights.Error("Test Exception", e);
		}

		Insights.Log(() => "Deferred");

		Insights.Info("Executed");
	}
}