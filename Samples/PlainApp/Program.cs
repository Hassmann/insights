using SLD.Insights;
using SLD.Insights.Output;
using System.Diagnostics;

// BTW: There is a global outlet for insights, based on the Reactive pattern
InsightsSource.Insights
	.Subscribe(new ConsoleObserver());

// Start by placing an instance of InsightsSource where you can access it
InsightsSource Insights = new InsightsSource("Plain App")
{
	// in code here, but also configurable in appsettings etc.
	DisplayLevel = TraceLevel.Verbose
};

// Send traces at different trace levels
Insights.Trace("Executing");

Insights.Warning("Test Warning");

Insights.Error("Test Error");

try
{
	ThrowException();
}
catch (Exception e)
{
	Insights.Error("Test Exception", e);
}

// High-performance version 
Insights.Log(() => "Deferred", TraceLevel.Info);


// Helper: Will throw a nested exception
static void ThrowException()
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
