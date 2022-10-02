using System.Diagnostics;

namespace SLD.Insights.Configuration
{
	public class SourceSettings
	{
		public string Name { get; set; }
		public TraceLevel Level { get; set; } = Insight.DefaultLevel;

		internal bool IsEnabled(string name, object level, object ignored)
			=> Level >= (TraceLevel)level;

		public override string ToString()
			=> $"{Name}: {Level}";
	}
}