using System;
using System.Collections.Generic;

namespace SLD.Insights.UI.Model
{
	class Log : IObserver<Insight>
	{
		readonly List<Insight> _insights = new List<Insight>();

		public void OnNext(Insight insight)
		{
			_insights.Add(insight);
		}

		public void OnError(Exception error) => throw new NotImplementedException();
		public void OnCompleted() => throw new NotImplementedException();
	}
}
