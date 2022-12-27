using SLD.Insights;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Workbench
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
		static InsightsSource Insights = new InsightsSource("MainWindow")
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
