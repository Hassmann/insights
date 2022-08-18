namespace SLD.Insights.Configuration
{
	public class InsightsSettings
	{
		public SourceSettings[] Sources { get; set; }

		public bool DumpExceptions { get; set; } = true;
	}
}