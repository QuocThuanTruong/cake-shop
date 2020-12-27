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
	/// Interaction logic for CakeDetailPage.xaml
	/// </summary>
	public partial class CakeDetailPage : Page
	{
		public delegate void UpdateCakeHandler(int cakeID);
		public event UpdateCakeHandler UpdateCake;

		public CakeDetailPage()
		{
			InitializeComponent();
		}

		public CakeDetailPage(int cakeID)
		{
			InitializeComponent();
		}

		private void updateCakeButton_Click(object sender, RoutedEventArgs e)
		{
			UpdateCake?.Invoke(1);
		}
	}
}
