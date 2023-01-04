using SLD.Insights;
using SLD.Insights.UI.Model;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workbench.ViewModels
{
	public class MainVM : ViewModel
	{
		static InsightsSource Insights = new InsightsSource("Main");

		public MainVM()
		{
			InsightsSource.Insights.Subscribe(Log);

			Insights.Info("Insights subscribed to display");

		}

		public Log Log { get; } = new Log();
	}
}
