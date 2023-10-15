using Microsoft.Extensions.Logging;

namespace SLD.Insights
{
	public class LoggerProvider : ILoggerProvider
	{
		public ILogger CreateLogger(string categoryName)
			=> new LoggerAdapter(new InsightsSource(categoryName.Split('.').Last()));

		public void Dispose()
		{ }
	}
}