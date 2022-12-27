using SLD.Insights;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;

namespace SLD.Insights.UI
{
	using Model;

	/// <summary>
	/// Interaction logic for InsightsWindowControl.
	/// </summary>
	public partial class InsightsWindowControl : UserControl
	{
		Log _log = new Log();

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