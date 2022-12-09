## Why Insights?

__To understand what's happening in your software while you develop it__

>This single goal, the commitment to simplicity, and not to interfere with code and performance drive the design of SLD.Insights.

### Features

- Read-friendly Traces to the Output Window
- Configurable in Code and Settings
- Minimal impact on Code and Performance

### Quickstart
1. Get the **Nuget** package ```SLD.Insights```

2. Consider this code

```csharp
using SLD.Insights;
using System.Diagnostics;

// Start by placing an instance of an InsightsSource where you can access it
InsightsSource Insights = new("Plain App")
{
    // in code here, but usually configured in appsettings etc.
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


// High-performance version, Insight will only be constructed when listeners are present
Insights.Log(() => "Expensive string creation", TraceLevel.Info);


// Helper: Throw the nested exception above
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

  00:00.422                      Plain App | Expensive string creation
```
>Actually, it will look like this, when you have right-clicked the Output and unchecked all the clutter except "Program Output" and optionally "Exceptions".
### Configuration

>The Output above is still too verbose, let's assume we don't want to see any of the "Insights" infrastructure stuff and only real errors from the App.

Most of the time, you would not configure the displayed level in code like above, but:
```csharp
InsightsSource Insights = new("Plain App");
```
You can configure displayed levels in your ```appsettings.json```
```json
{
  "Insights": {
    "Levels": {
      "Insights" : "Off",
      "Plain App": "Error"
    }
  }
}
```
The Output after a run will look like 
```
  00:00.004                       Insights | Initialize
E 00:00.262                      Plain App | Test Error
E 00:00.304                      Plain App | Test Exception
...
```
If you don't use appsettings, a text file by the name of ```Insights.Levels``` in the directory of the executable can also be used to configure with a simple syntax:
```
Insights  : Off
Plain App : Error
```
This can be useful in large projects with dozens of insight sources to stay organized, since only lines with the pattern ```[Name]:[TraceLevel]``` are being evaluated.
```
This is a perfectly valid configuration file.

Main ________________________________________
Plain App : Error

Infrastructure ______________________________
Insights  : Off
```
### Many InsightsSources

>Insights are most useful when you 
>- Can just start to type ```Insights.``` anywhere in your code and leave the trace you have in mind
>- The Output Window is tuned to your current work with the right granularity

There are several patterns of creating insights sources, examples:

#### ... per class
```csharp
class Application
{
    static InsightsSource Insights = new(nameof(Application));
}
```

#### ... per instance
```csharp
abstract class Module
{
    protected InsightsSource Insights;
    
    protected Module(string name)
    {
    	Insights = new($"Module {name}");
    }
}
```

#### ... as global
```csharp
static class Insights
{
    internal static InsightsSource Infrastructure = new(nameof(Infrastructure));
    internal static InsightsSource Storage = new(nameof(Storage));
}
```
#### ... ad libitum
Just make sure you have a useful collection of topics to fade in and out, according to the problem you are working on.


