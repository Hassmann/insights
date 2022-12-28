using System;
using System.Windows.Controls;

namespace SLD.Insights.UI
{
	using Model;

	/// <summary>
	/// Interaction logic for InsightsWindowControl.
	/// </summary>
	public partial class InsightsWindowControl : UserControl
	{
		readonly Log _log = new Log();

		/// <summary>
		/// Initializes a new instance of the <see cref="InsightsWindowControl"/> class.
		/// </summary>
		public InsightsWindowControl()
		{
			this.InitializeComponent();
		}

		public IObserver<Insight> Log
			=> _log;
	}
}