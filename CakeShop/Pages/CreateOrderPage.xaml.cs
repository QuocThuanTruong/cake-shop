using CakeShop.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
	/// Interaction logic for CreateOrderPage.xaml
	/// </summary>
	public partial class CreateOrderPage : Page
	{
		private DatabaseUtilities _databaseUtilities = DatabaseUtilities.GetDatabaseInstance();
		private ApplicationUtilities _applicationUtilities = ApplicationUtilities.GetAppInstance();
		private List<Cake> cakes = new List<Cake>();
		private int _index;
		private int _totalCost = 0;
		private int _shippingFee = 0;
		public CreateOrderPage()
		{
			InitializeComponent();
		}

		public CreateOrderPage(Object dummy)
		{
			InitializeComponent();
		}

		private void shipToggleButton_Click(object sender, RoutedEventArgs e)
		{
			if (addressTextBox.IsEnabled == false)
			{
				addressTextBox.IsEnabled = true;
			} 
			else
			{
				addressTextBox.IsEnabled = false;
			}

			if (phoneTextBox.IsEnabled == false)
			{
				phoneTextBox.IsEnabled = true;
			}
			else
			{
				phoneTextBox.IsEnabled = false;
			}

			if (shipipngFeeTextBox.IsEnabled == false)
			{
				shipipngFeeTextBox.IsEnabled = true;
			}
			else
			{
				shipipngFeeTextBox.IsEnabled = false;
			}

			if (shippingFreeGrid.Visibility == Visibility.Visible)
			{
				shippingFreeGrid.Visibility = Visibility.Collapsed;
			}
			else
			{
				shippingFreeGrid.Visibility = Visibility.Visible;
			}
		}

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
			orderedCakeListView.ItemsSource = Global.Global.cakesOrder;
			orderPreviewListView.ItemsSource = Global.Global.cakesOrder;

			cakes = _databaseUtilities.GetAllCake();

			cakeComboBox.ItemsSource = cakes;

			Global.Global.calcTotalCost();
			totalCakePriceTextBlock.Text = _applicationUtilities.GetMoneyForBinding(Global.Global.totalCost);
			_totalCost = Global.Global.totalCost + _shippingFee;
			totalPriceTextBlock.Text = _applicationUtilities.GetMoneyForBinding(_totalCost);
		}

        private void cakeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
			Cake cakeSelection = (Cake)cakeComboBox.SelectedItem;

			for (int i = 0; i < Global.Global.cakesOrder.Count; ++i)
            {
				if (cakeSelection.ID_Cake == Global.Global.cakesOrder[i].ID_Cake)
                {
					importQuantityTextBox.Text = $"{Global.Global.cakesOrder[i].Order_Quantity}";
					_index = i;
					break;
                }
            }
        }

        private void addCakeButton_Click(object sender, RoutedEventArgs e)
        {
			if (importQuantityTextBox.Text.Length == 0)
            {
				//Khong duoc bo trong
				return;
            }

			Cake cake = (Cake)cakeComboBox.SelectedItem;
			cake.Order_Quantity = int.Parse(importQuantityTextBox.Text);

			if (cake.Order_Quantity > cake.Current_Quantity)
            {
				//Chi con toi da cake.Current_Quantity
				return;
            }

			cake.Current_Quantity -= cake.Order_Quantity;
			cake.Total_Price = cake.SELLING_PRICE_INT_FOR_BINDING * cake.Order_Quantity;
			cake.Total_Price_FOR_BINDING = _applicationUtilities.GetMoneyForBinding(cake.Total_Price);

			Global.Global.cakesOrder.Add(cake);
			Global.Global.calcTotalCost();

			totalCakePriceTextBlock.Text = _applicationUtilities.GetMoneyForBinding(Global.Global.totalCost);
			_totalCost = Global.Global.totalCost + _shippingFee;
			totalPriceTextBlock.Text = _applicationUtilities.GetMoneyForBinding(_totalCost);

			orderedCakeListView.ItemsSource = null;
			orderPreviewListView.ItemsSource = null;

			orderedCakeListView.ItemsSource = Global.Global.cakesOrder;
			orderPreviewListView.ItemsSource = Global.Global.cakesOrder;

			//cakeComboBox.SelectedIndex = -1;
			importQuantityTextBox.Text = "";
		}

        private void importQuantityTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
			//Regex meaning: not match any number digit zero or many times
			var pattern = "[^0-9]+";
			var regex = new Regex(pattern);

			//if true -> input event has handled (skiped this character)
			e.Handled = regex.IsMatch(e.Text);
		}

        private void finishOrderButton_Click(object sender, RoutedEventArgs e)
        {
			if (customerTextBox.Text.Length == 0)
            {
				//Khong duoc bo trong
				return;
            }

			string customerName = customerTextBox.Text;
			string customerAddress = "";
			string customerPhone = "";

            if (shipToggleButton.IsChecked ?? false)
            {
                if (addressTextBox.Text.Length == 0)
                {
                    //Khong duoc bo trong
                    return;
                }
                customerAddress = addressTextBox.Text;

                if (phoneTextBox.Text.Length == 0)
                {
                    return;
                }
                customerPhone = phoneTextBox.Text;

                if (shipipngFeeTextBox.Text.Length == 0)
                {
                    return;
                }
                _shippingFee = int.Parse(shipipngFeeTextBox.Text);
                //shippingFeeTextBlock.Text = _applicationUtilities.GetMoneyForBinding(_shippingFee);
            }

            Global.Global.calcTotalCost();
            _totalCost = Global.Global.totalCost + _shippingFee;
            totalPriceTextBlock.Text = _applicationUtilities.GetMoneyForBinding(_totalCost);

            int ID_Invoice = _databaseUtilities.GetMaxIDInvoice() + 1;
			var today = DateTime.Now;

            _databaseUtilities.AddInvoice(
                ID_Invoice,
                today,
                customerName,
                customerAddress,
                customerPhone,
                decimal.Parse($"{_shippingFee}", NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowCurrencySymbol | NumberStyles.Currency, new CultureInfo("en-US")),
                decimal.Parse($"{_totalCost}", NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowCurrencySymbol | NumberStyles.Currency, new CultureInfo("en-US"))
            );

            for (int i = 0; i < Global.Global.cakesOrder.Count; ++i)
            {
                Cake cake = Global.Global.cakesOrder[i];
                _databaseUtilities.AddInvoiceDetail(ID_Invoice, i + 1, cake.ID_Cake, cake.Order_Quantity);
				_databaseUtilities.UpdateCake(cake.ID_Cake, cake.Name_Cake, cake.Description, cake.Type_Cake, cake.Original_Price, cake.Selling_Price, cake.Current_Quantity, cake.Link_Avt);
				
            }

			cancelOrderButton_Click(null, null);
		}

        private void phoneTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
			//Regex meaning: not match any number digit zero or many times
			var pattern = "[^0-9]+";
			var regex = new Regex(pattern);

			//if true -> input event has handled (skiped this character)
			e.Handled = regex.IsMatch(e.Text);
		}

        private void shipipngFeeTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (shipipngFeeTextBox.Text.Length == 0)
            {
                return;
            }
            _shippingFee = int.Parse(shipipngFeeTextBox.Text);
            shippingFeeTextBlock.Text = _applicationUtilities.GetMoneyForBinding(_shippingFee);

            _totalCost = Global.Global.totalCost + _shippingFee;
            totalPriceTextBlock.Text = _applicationUtilities.GetMoneyForBinding(_totalCost);
        }

        private void cancelOrderButton_Click(object sender, RoutedEventArgs e)
        {
			//Reset
			_totalCost = 0;
			_shippingFee = 0;
			customerTextBox.Text = "";

			if (shipToggleButton.IsChecked ?? false)
			{
				shipToggleButton.IsChecked = false;
				shipToggleButton_Click(null, null);

				addressTextBox.Text = "";
				phoneTextBox.Text = "";
				shipipngFeeTextBox.Text = "";
			}

			totalCakePriceTextBlock.Text = _applicationUtilities.GetMoneyForBinding(0);
			totalPriceTextBlock.Text = _applicationUtilities.GetMoneyForBinding(0);
			shippingFeeTextBlock.Text = _applicationUtilities.GetMoneyForBinding(0);

			Global.Global.cakesOrder.Clear();
			Global.Global.totalCost = 0;

			orderedCakeListView.ItemsSource = null;
			orderPreviewListView.ItemsSource = null;

			orderedCakeListView.ItemsSource = Global.Global.cakesOrder;
			orderPreviewListView.ItemsSource = Global.Global.cakesOrder;

			cakeComboBox.ItemsSource = null;

			cakes = _databaseUtilities.GetAllCake();
			cakeComboBox.ItemsSource = cakes;

			cakeComboBox.SelectedIndex = -1;
		}
    }
}
