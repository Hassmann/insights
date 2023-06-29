using Microsoft.Extensions.Logging;
using System.Reactive.Disposables;

namespace SLD.Insights
{
	internal class LoggerAdapter : ILogger
	{
		private InsightsSource _source;

		public LoggerAdapter(InsightsSource source)
		{
			_source = source;
		}

		public void Log<TState>(
			LogLevel logLevel, 
			EventId eventId, 
			TState state, 
			Exception exception, 
			Func<TState, Exception, string> formatter
		)
		{
			var traceLevel = logLevel switch
			{
				LogLevel.Trace or LogLevel.Debug => TraceLevel.Verbose,
				LogLevel.Information => TraceLevel.Info,
				LogLevel.Warning => TraceLevel.Warning,
				LogLevel.Error or LogLevel.Critical => TraceLevel.Error,
				_ => TraceLevel.Off
			};

			_source.Log(
				$"{state}",
				traceLevel,
				exception
				);
		}

		public bool IsEnabled(LogLevel logLevel)
			=> _source.DisplayLevel switch
			{
				TraceLevel.Error => logLevel >= LogLevel.Error,
				TraceLevel.Warning => logLevel >= LogLevel.Warning,
				TraceLevel.Info => logLevel >= LogLevel.Information,
				TraceLevel.Verbose => logLevel >= LogLevel.Trace,

				_ => false
			};

		public IDisposable BeginScope<TState>(TState state) where TState : notnull
		{
			_source.Trace($"Begin Scope: {state}");

			return Disposable.Create(() => _source.Trace($"End Scope: {state}"));
		}
	}
}