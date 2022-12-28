using SLD.Insights;
using System.Diagnostics;
using System.Windows;

namespace Workbench
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		static readonly InsightsSource Insights = new InsightsSource("MainWindow")
		{
			DisplayLevel = TraceLevel.Verbose
		};

		public MainWindow()
		{
			InitializeComponent();

			InsightsSource.Insights.Subscribe(_display.Log);

			Insights.Info("Insights subscribed to display");
		}
	}
}
