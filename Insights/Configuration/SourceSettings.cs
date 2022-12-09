namespace SLD.Insights.Configuration
{
	public class SourceSettings
	{
		public string Name { get; set; }
		public TraceLevel Level { get; set; } = Insight.DefaultLevel;

		public override string ToString()
			=> $"{Name}: {Level}";
	}
}