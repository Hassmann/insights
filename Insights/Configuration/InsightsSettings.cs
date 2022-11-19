namespace SLD.Insights.Configuration
{
	public class InsightsSettings
	{
		public SourceSettings[] Sources { get; set; }

		public Dictionary<string, TraceLevel> Levels { get; set; }

		public bool DumpExceptions { get; set; } = true;

		public bool HasSources
			=> (Levels is not null && Levels.Any())
			|| (Sources is not null && Sources.Any());
	}
}