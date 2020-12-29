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
	/// Interaction logic for CreateOrderPage.xaml
	/// </summary>
	public partial class CreateOrderPage : Page
	{
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
	}
}
