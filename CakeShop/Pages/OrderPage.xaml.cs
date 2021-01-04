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
using CakeShop.Global;
using CakeShop.Utilities;

namespace CakeShop.Pages
{
	/// <summary>
	/// Interaction logic for OrderPage.xaml
	/// </summary>
	public partial class OrderPage : Page
	{
		//Custom lại constructor
		public delegate void CreateNewOrderHandler();
		public event CreateNewOrderHandler CreateNewOrder;

		public delegate void CreateOrderWithCurrentHandler(Object dummy);
		public event CreateOrderWithCurrentHandler CreateOrderWithCurrent;

		private DatabaseUtilities _databaseUtilities = DatabaseUtilities.GetDatabaseInstance();
		private ApplicationUtilities _applicationUtilities = ApplicationUtilities.GetAppInstance();

		private List<Invoice> _invoices = new List<Invoice>();
		public OrderPage()
		{
			InitializeComponent();

		}

		private void Page_Loaded(object sender, RoutedEventArgs e)
        {
			_invoices = _databaseUtilities.GetAllInvoice();

			currentOrdersListView.ItemsSource = Global.Global.cakesOrder;
			ordersListView.ItemsSource = _invoices;
		}

		private void createNewOrderButton_Click(object sender, RoutedEventArgs e)
		{
			CreateNewOrder?.Invoke();
		}

		private void createOrderWithCurrentButton_Click(object sender, RoutedEventArgs e)
		{
			CreateOrderWithCurrent?.Invoke(new Object());
		}

        private void clearCurrentOrderButton_Click(object sender, RoutedEventArgs e)
        {
			Global.Global.clearGlobal();
			currentOrdersListView.ItemsSource = null;

		}
    }
}
