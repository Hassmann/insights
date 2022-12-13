// See https://aka.ms/new-console-template for more information
using SLD.Insights;
using System.Diagnostics;

const string Topic = "Interesting";

// Setup an Emitter
var emitter = InsightsEmitter
	.CreateLocal();

// Pipe all insights into emitter
InsightsSource.Insights
	.Subscribe(emitter);

// Setup a Receiver
InsightsReceiver
	.ConnectLocal()
	.Subscribe(insight => Trace.WriteLine($"Received: {insight}"));

// Send insight
InsightsSource Insights = new(Topic)
{
	DisplayLevel = TraceLevel.Info,
};

for (int i = 0; i < 10; i++)
{
	Insights.Info($"Demo Trace {i}");

	Thread.Sleep(1000);
}


Console.ReadLine();



