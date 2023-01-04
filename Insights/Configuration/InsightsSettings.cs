namespace SLD.Insights.Configuration
{
	public class InsightsSettings
	{
		public SourceSettings[] Sources { get; set; }

		public Dictionary<string, TraceLevel> Levels { get; set; }

		public bool DumpExceptions { get; set; } = true;

		public TraceLevel DefaultLevel { get; set; } = TraceLevel.Warning;

		public TimeSpan IdleThreshold { get; set; } = TimeSpan.FromSeconds(3);

		public bool HasSources
			=> (Levels is not null && Levels.Any())
			|| (Sources is not null && Sources.Any());

		public IEnumerable<string> ConfiguredSources
			=> Enumerable.Concat(
				Sources.IfAny().Select(source => source.Name),
				Levels.IfAny().Select(level => level.Key)
				)
			.Distinct();
	}
}