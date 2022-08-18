using System.Diagnostics;

namespace SLD.Insights.Configuration
{
	public class SourceSettings
	{
		public string Name { get; set; }
		public TraceLevel Level { get; set; } = Insight.DefaultLevel;
	}
}