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
using CakeShop.Utilities;
using CakeShop.Converter;
namespace CakeShop.Pages
{
	/// <summary>
	/// Interaction logic for CakeDetailPage.xaml
	/// </summary>
	public partial class CakeDetailPage : Page
	{
		public delegate void UpdateCakeHandler(int cakeID);
		public event UpdateCakeHandler UpdateCake;

		private DatabaseUtilities _databaseUtilities = DatabaseUtilities.GetDatabaseInstance();
		private ApplicationUtilities _applicationUtilities = ApplicationUtilities.GetAppInstance();
		private AbsolutePathConverter _absolutePathConverter = new AbsolutePathConverter();

		private Cake _cake = new Cake();

		public CakeDetailPage()
		{
			InitializeComponent();
		}

		public CakeDetailPage(int cakeID)
		{
			_cake = _databaseUtilities.getCakeById(cakeID);
			DataContext = this._cake;

			InitializeComponent();

		}

		private void updateCakeButton_Click(object sender, RoutedEventArgs e)
		{
			UpdateCake?.Invoke(_cake.ID_Cake);
		}

        private void addToOrderButton_Click(object sender, RoutedEventArgs e)
        {
			_cake.Order_Quantity = 1;
			_cake.Total_Price = _cake.SELLING_PRICE_INT_FOR_BINDING * _cake.Order_Quantity;
			_cake.Total_Price_FOR_BINDING = _applicationUtilities.GetMoneyForBinding(_cake.Total_Price);
			Global.Global.cakesOrder.Add(_cake);

			foreach(var cake in Global.Global.cakesOrder)
            {
				Debug.WriteLine(cake.ID_Cake);
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {


		}
    }
}
