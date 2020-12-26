using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
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

namespace CakeShop.Pages
{
	/// <summary>
	/// Interaction logic for DashBoardPage.xaml
	/// </summary>
	public partial class DashBoardPage : Page
	{
		private string[] _labels { get; set; } = new[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" };

		public DashBoardPage()
		{
			InitializeComponent();

			DataContext = this;
		}

		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
			SeriesCollection revenueByMonth = new SeriesCollection
			{
				new ColumnSeries
				{
					Values = new ChartValues<double> { 10, 50, 39, 50 , 60, 10, 20, 30, 40, 50, 60, 70}
				}

			};


			revenueByMonthChart.Series = revenueByMonth;
			revenueByMonthChartAxisX.Labels = _labels;


		}

		private void yearCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{

		}
	}
}
