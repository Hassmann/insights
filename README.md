# Why Insights?

__To understand what's happening in your software while you develop it__

>This single goal and the commitment not to interfere with code and performance drive the design decisions behind SLD.Insights.

## SLD.Insights

- Readable Traces to the Output Window
- Configurable in Code and Settings
- Minimal impact on code performance

## Quickstart

Consider this code

```csharp
using SLD.Insights;
using System.Diagnostics;

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

```

Run and you will see this Output Window
```
  00:00.003                       Insights | Initialize
I 00:00.050                       Insights | Configured Sources: Insights, Plain App
  00:00.053                       Insights | BasePath: C:\Work\SLD\Insights\Samples\PlainApp\bin\Debug\net6.0\
I 00:00.088                       Insights | Plain App: Verbose
  00:00.285                       Insights | Plain App: Verbose
  00:00.287                      Plain App | Executing
W 00:00.303                      Plain App | Test Warning
E 00:00.305                      Plain App | Test Error
E 00:00.398                      Plain App | Test Exception

  InvalidOperationException ________________________________________________________________________

  Outer

  Assembly: PlainApp
  File    : Program.cs (Line 45)
  Method  : Program.<<Main>$>g__ThrowException|0_1

     -----------------------------------------------------------------------------------------------

     IndexOutOfRangeException ______________________________________________________________________

     Inner

     Assembly: PlainApp
     File    : Program.cs (Line 41)
     Method  : Program.<<Main>$>g__ThrowException|0_1

____________________________________________________________________________________________________

  00:00.422                      Plain App | Deferred
```

## Configuration