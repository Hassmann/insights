using System.Collections.Generic;
using System.Diagnostics;

namespace SLD.Insights.Configuration
{
	public class InsightsSettings
	{
		public SourceSettings[] Sources { get; set; }

		public Dictionary<string, TraceLevel> Levels { get; set; }

		public bool DumpExceptions { get; set; } = true;
	}
}