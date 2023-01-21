using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLD.Insights.UI.ViewModels
{
	public class InsightVM : ViewModel
	{
		private Insight _insight;

		public InsightVM(Insight insight)
		{
			_insight = insight;
		}
	}
}
