using SLD.Insights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualStudio.Model
{
	class Log : IObserver<Insight>
	{
		public void OnNext(Insight value) => throw new NotImplementedException();
		public void OnError(Exception error) => throw new NotImplementedException();
		public void OnCompleted() => throw new NotImplementedException();
	}
}
