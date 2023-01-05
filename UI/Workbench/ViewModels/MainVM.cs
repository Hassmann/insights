using SLD.Insights;
using SLD.Insights.UI.ViewModels;

namespace Workbench.ViewModels
{
	using Sources;

	public class MainVM : ViewModel
	{
		private static InsightsSource Insights = new InsightsSource("Main");

		public PeriodicSource Periodic { get; } = new PeriodicSource();

		public LogVM Log { get; } = new LogVM();
	}
}