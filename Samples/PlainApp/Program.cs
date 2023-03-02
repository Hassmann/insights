using SLD.Insights;
using SLD.Insights.Output;
using System.Diagnostics;

// BTW: There is a global outlet for insights, based on the Reactive pattern
InsightsSource.Insights
	.Subscribe(new ConsoleObserver());

// Start by placing an instance of an InsightsSource where you can access it
InsightsSource Insights = new("Plain App");

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

// An idle marker will be inserted after some time without insights.
// Default is 3 seconds
Thread.Sleep(3100);

// High-performance version, Insight will only be constructed when listeners are present
Insights.Log(() => "Expensive string creation", TraceLevel.Info);

//////////////////////////////////////////////////////////////////////////////

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