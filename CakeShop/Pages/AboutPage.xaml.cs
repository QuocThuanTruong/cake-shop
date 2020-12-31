using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
	/// Interaction logic for AboutPage.xaml
	/// </summary>
	public partial class AboutPage : Page
	{
		private string[] _memberUrls = { "https://www.facebook.com/th.coconut", "https://www.facebook.com/tranght111", "https://https://www.facebook.com/nhattuan.le.33" };
		private ObservableCollection<Tuple<string, string, string, string>> _memberDetails = new ObservableCollection<Tuple<string, string, string, string>>();
		public AboutPage()
		{
			InitializeComponent();

			_memberDetails.Add(new Tuple<string, string, string, string>("0", "QT", Properties.Resources.text_name_qt, Properties.Resources.text_mssv_qt));
			_memberDetails.Add(new Tuple<string, string, string, string>("1", "HT", Properties.Resources.text_name_ht, Properties.Resources.text_mssv_ht));
			_memberDetails.Add(new Tuple<string, string, string, string>("2", "NT", Properties.Resources.text_name_nt, Properties.Resources.text_mssv_nt));

			membersListview.ItemsSource = _memberDetails;
		}


		private void memberAvatar_Click(object sender, RoutedEventArgs e)
		{
			var selectedButton = (Button)sender;
			System.Diagnostics.Process.Start(_memberUrls[int.Parse(selectedButton.Tag.ToString())]);
		}

		private void openSourceDetailTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
		{
			System.Diagnostics.Process.Start(Properties.Resources.text_github);
		}
	}
}
