using CakeShop.Pages;
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

namespace CakeShop
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainScreen : Window
	{

		private List<Tuple<Button, Image, string, string, TextBlock>> _mainScreenButtons;

		public MainScreen()
		{
			InitializeComponent();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			DashBoardPage dashBoardPage = new DashBoardPage();
			
			pageNavigation.NavigationService.Navigate(dashBoardPage);

			_mainScreenButtons = new List<Tuple<Button, Image, string, string, TextBlock>>()
			{
				new Tuple<Button, Image, string, string, TextBlock>(dashboardPageButton, iconDashboardPage, "IconBrownDashboard", "IconWhiteDashboard", dashboardPageName),
				new Tuple<Button, Image, string, string, TextBlock>(homePageButton, iconHomePage, "IconBrownHome", "IconWhiteHome", homePageName),
				new Tuple<Button, Image, string, string, TextBlock>(addCakePageButton, iconAddCakePage, "IconBrownAdd", "IconWhiteAdd", addCakePageName),
				new Tuple<Button, Image, string, string, TextBlock>(orderPageButton, iconOrderPage, "IconBrownOrder", "IconWhiteOrder", orderPageName),
				new Tuple<Button, Image, string, string, TextBlock>(helpPageButton, iconHelpPage, "IconBrownHelp", "IconWhiteHelp", helpPageName),
				new Tuple<Button, Image, string, string, TextBlock>(aboutPageButton, iconAboutPage, "IconBrownAbout", "IconWhiteAbout", aboutPageName)
			};

			//Default load page is home page
			DrawerButton_Click(dashboardPageButton, e);
		}

		private void DrawerButton_Click(object sender, RoutedEventArgs e)
		{
			/** Highlight selected button**/
			var selectedButton = (Button)sender;

			/** Default property of button
			 * <Setter Property="Background" Value="Transparent"/>
			 * <Setter Property="BorderThickness" Value="1"/>**/

			foreach (var button in _mainScreenButtons)
			{
				if (button.Item1.Name != selectedButton.Name)
				{
					button.Item1.Background = Brushes.Transparent;
					button.Item1.IsEnabled = true;

					button.Item2.Source = (ImageSource)FindResource(button.Item3);
					button.Item5.Foreground = (Brush)FindResource("MyBrown");
				}
			}

			//Highlight
			selectedButton.Background = (Brush)FindResource("MyPinkGradient");
			selectedButton.IsEnabled = false;
			/****/

			/** Navigating page **/
			pageNavigation.NavigationService.Navigate(getPageFromButton(selectedButton));
		}

		private Page getPageFromButton(Button selectedButton)
		{
			Page result = new HomePage();

			if (selectedButton.Name == dashboardPageButton.Name)
			{
				iconDashboardPage.Source = (ImageSource)FindResource(_mainScreenButtons[0].Item4);
				dashboardPageName.Foreground = Brushes.White;
				result = new DashBoardPage();
			}
			else if (selectedButton.Name == homePageButton.Name)
			{
				iconHomePage.Source = (ImageSource)FindResource(_mainScreenButtons[1].Item4);
				homePageName.Foreground = Brushes.White;
				result = new HomePage();
				((HomePage)result).ShowCakeDetailPage += MainScreen_ShowCakeDetailPage;
				
			}
			else if (selectedButton.Name == addCakePageButton.Name)
			{
				iconAddCakePage.Source = (ImageSource)FindResource(_mainScreenButtons[2].Item4);
				addCakePageName.Foreground = Brushes.White;
				result = new AddCakePage();

			}
			else if (selectedButton.Name == orderPageButton.Name)
			{
				iconOrderPage.Source = (ImageSource)FindResource(_mainScreenButtons[3].Item4);
				orderPageName.Foreground = Brushes.White;
				result = new OrderPage();
				((OrderPage)result).CreateNewOrder += MainScreen_CreateNewOrder;
				((OrderPage)result).CreateOrderWithCurrent += MainScreen_CreateOrderWithCurrent;
			}
			else if (selectedButton.Name == helpPageButton.Name)
			{
				iconHelpPage.Source = (ImageSource)FindResource(_mainScreenButtons[4].Item4);
				helpPageName.Foreground = Brushes.White;
				result = new HelpPage();
			}
			else if (selectedButton.Name == aboutPageButton.Name)
			{
				iconAboutPage.Source = (ImageSource)FindResource(_mainScreenButtons[5].Item4);
				aboutPageName.Foreground = Brushes.White;
				result = new AboutPage();
			}

			return result;
		}

		private void MainScreen_CreateOrderWithCurrent(Object dummy)
		{
			CreateOrderPage createOrderPage = new CreateOrderPage(dummy);
			pageNavigation.NavigationService.Navigate(createOrderPage);

			cleaerDrawerButton();
		}

		private void MainScreen_CreateNewOrder()
		{
			CreateOrderPage createOrderPage = new CreateOrderPage();
			pageNavigation.NavigationService.Navigate(createOrderPage);

			cleaerDrawerButton();
		}

		private void MainScreen_ShowCakeDetailPage(int cakeID)
		{
			CakeDetailPage cakeDetailPage = new CakeDetailPage(cakeID);
			cakeDetailPage.UpdateCake += CakeDetailPage_UpdateCake;
			pageNavigation.NavigationService.Navigate(cakeDetailPage);
			cleaerDrawerButton();
		}

		private void CakeDetailPage_UpdateCake(int cakeID)
		{
			AddCakePage addCakePage = new AddCakePage(cakeID);
			pageNavigation.NavigationService.Navigate(addCakePage);
			cleaerDrawerButton();
		}

		private void cleaerDrawerButton()
		{
			foreach (var button in _mainScreenButtons)
			{
				
					button.Item1.Background = Brushes.Transparent;
					button.Item1.IsEnabled = true;

					button.Item2.Source = (ImageSource)FindResource(button.Item3);
					button.Item5.Foreground = (Brush)FindResource("MyBrown");
				
			}
		}

		private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
		{
			this.DragMove();
		}

		private void closeWindowButton_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private void minimizeWindowButton_Click(object sender, RoutedEventArgs e)
		{
			this.WindowState = WindowState.Minimized;
		}
	}
}
