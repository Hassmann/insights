using SLD.Application;
using System;
using System.Threading.Tasks;

namespace PlainApp;

class Program : ConsoleApplication
{
	protected override async Task Execute()
	{
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

		Insights.Info("Executed");
	}

	void RaiseException()
	{
		try
		{
			throw new Exception("Inner");
		}
		catch (Exception e)
		{
			throw new Exception("Outer", e);
		}
	}


	static Task Main(string[] args)
		=> Run<Program>("PlainApp");
}
