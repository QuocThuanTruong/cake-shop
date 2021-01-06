using CakeShop.Utilities;
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
		public string[] Labels { get; set; } = new[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" };
		private List<int> _years = new List<int>();
		private int _year = 2020;
		private DatabaseUtilities _databaseUtilities = DatabaseUtilities.GetDatabaseInstance();
		private ApplicationUtilities _applicationUtilities = ApplicationUtilities.GetAppInstance();

		public DashBoardPage()
		{
			InitializeComponent();
		}

		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
			_years = _databaseUtilities.GetAllYear();
			_years.Reverse();
			yearCombobox.ItemsSource = _years;
			yearCombobox.SelectedIndex = 0;
			loadDashboard();
			DataContext = this;
		}

		private void yearCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
			
			if (this.IsLoaded)
            {
				
				loadDashboard();
			}
    
        }

		private void loadDashboard()
        {
			if (_years.Count > 0)
			{
				_year = _years[yearCombobox.SelectedIndex];
			}
			else
			{
				_year = DateTime.Now.Year;
			}

			var result = _databaseUtilities.StatisticByYear(_year);

			if (_year == DateTime.Now.Year)
            {
				result.SumStockReceiving = _databaseUtilities.CalcTotalCurrentCake();
				totalInventoryCard.Visibility = Visibility.Collapsed;
				currentCakeCard.Visibility = Visibility.Visible;
				SumCurrentStockReceiving.Text = result.SumStockReceiving.ToString();
			} 
			else
			{
				totalInventoryCard.Visibility = Visibility.Visible;
				currentCakeCard.Visibility = Visibility.Collapsed;
			}

			SumStockReceiving.Text = result.SumStockReceiving.ToString();
			
			SumInvoices.Text = result.SumInvoices.ToString();
			SumStockReveivingMoney.Text = result.SumStockReveivingMoney_FOR_BINDING;
			SumRevenue.Text = result.SumRevenue_FOR_BINDING;
			SumProfit.Text = result.SumProfit_FOR_BINDING;

			//var revenueByMonthInYearCollection = new SeriesCollection();
			//for (int i = 1; i <= 12; ++i)
			//{
			//	revenueByMonthInYearCollection.Add(new ColumnSeries
			//	{
			//		Title = $"Tháng {i}",
			//		Values = new ChartValues<double> { _databaseUtilities.GetRevenueByMonthInYear(i, _year) }
			//	});
			//}

			//revenueByMonthChart.Series = revenueByMonthInYearCollection;

			var revenues = new ChartValues<double>();
			for (int i = 1; i <= 12; i++)
			{
				revenues.Add(_databaseUtilities.GetRevenueByMonthInYear(i, _year));
			}

			var revenueByMonthInYearCollection = new SeriesCollection()
			{
				new ColumnSeries
				{
					Title = "",
					Values = revenues
				}
			};

			revenueByMonthChart.Series = revenueByMonthInYearCollection;

			var revenueByTypeOfCakeResult = _databaseUtilities.statisticRevenueByTypeOfCakeInYear_Results(_year);
			var revenueByTypeOfCakeInYearCollection = new SeriesCollection();
			foreach (var revenue in revenueByTypeOfCakeResult)
			{
				revenueByTypeOfCakeInYearCollection.Add(new PieSeries
				{
					Title = revenue.TypeCake,
					Values = new ChartValues<double> { decimal.ToDouble(revenue.SumRevenue ?? 0) }
				});
			}

			revenueByCategoryChart.Series = revenueByTypeOfCakeInYearCollection;
		}
	}
}
