using SLD.Insights;
using System.Diagnostics;
using System.Windows;
using Workbench.ViewModels;

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

		public MainVM ViewModel { get; }

		public MainWindow()
		{
			InitializeComponent();

			ViewModel = new MainVM();
		}
	}
}
