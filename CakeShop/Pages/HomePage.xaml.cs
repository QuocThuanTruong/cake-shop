using System;
using System.Collections.Generic;
using System.Configuration;
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

namespace CakeShop.Pages
{
	/// <summary>
	/// Interaction logic for HomePage.xaml
	/// </summary>
	public partial class HomePage : Page
	{
		private List<Tuple<Button, TextBlock>> cakeGroupButton;

		public delegate void ShowCakeDetailPageHandler(int cakeID);
		public event ShowCakeDetailPageHandler ShowCakeDetailPage;

		private Configuration _configuration;

		private DatabaseUtilities _databaseUtilities = DatabaseUtilities.GetDatabaseInstance();

		private int _sortedBy = 0;
		private int _currentPage = 1;
		private int totalCakePerPage = 9;
		private bool _isSearching = false;
		private string _prevCondition = "";
		private int _maxPage = 0;
		private (string column, string type)[] _conditionSortedBy = {("Current_Quantity", "DESC"), ("Current_Quantity", "ASC"),
																	 ("Name_Cake", "ASC"), ("Name_Cake", "DESC"),
																	 ("Selling_Price", "DESC"), ("Selling_Price", "ASC") };
		public HomePage()
		{
			InitializeComponent();

			cakeGroupButton = new List<Tuple<Button, TextBlock>>()
			{
				new Tuple<Button, TextBlock>(toppingGroupButton, toppingTextBlock),
				new Tuple<Button, TextBlock>(glazedGroupButton, glazedTextBlock),
				new Tuple<Button, TextBlock>(fillingGroupButton, fillingTextBlock)
			};

			_configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

			_sortedBy = int.Parse(ConfigurationManager.AppSettings["SortedByHomePage"]);
			sortTypeComboBox.SelectedIndex = _sortedBy;

			loadCakes();
		}

		private void groupButton_Click(object sender, RoutedEventArgs e)
		{
			//Convert current clicked button to list item
			var clickedButton = ((Button)sender);
			var clickedItemIdx = int.Parse(clickedButton.Tag.ToString());
			var clickedItem = cakeGroupListBox.Items.GetItemAt(clickedItemIdx);

			//Add this converted item if new else remove it
			if (cakeGroupListBox.SelectedItems.Contains(clickedItem))
			{
				cakeGroupListBox.SelectedItems.Remove(clickedItem);

				cakeGroupButton[clickedItemIdx].Item1.Background = (SolidColorBrush)FindResource("MyLightGray");
				cakeGroupButton[clickedItemIdx].Item2.Foreground = (SolidColorBrush)FindResource("MyBlack");
			}
			else
			{
				cakeGroupListBox.SelectedItems.Add(clickedItem);
			}


			Debug.WriteLine(((Button)sender).Tag.ToString());

			foreach (var item in cakeGroupListBox.SelectedItems)
			{

				Debug.WriteLine(((Grid)item).Tag);
			}

			if (this.IsLoaded)
			{
				if (_isSearching)
				{
					//loadRecipesSearch();
				}
				else
				{
					loadCakes();
				}
			}
		}

		private void cakeGroupListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			foreach (var item in cakeGroupListBox.SelectedItems)
			{
				var selectedIndex = int.Parse(((Grid)item).Tag.ToString());

				cakeGroupButton[selectedIndex].Item1.Background = (SolidColorBrush)FindResource("MyLightBrown");
				cakeGroupButton[selectedIndex].Item2.Foreground = Brushes.White;
			}
        }

		private void eraserAllFilterButton_Click(object sender, RoutedEventArgs e)
		{
			foreach (var item in cakeGroupListBox.SelectedItems)
			{
				var selectedIndex = int.Parse(((Grid)item).Tag.ToString());

				cakeGroupButton[selectedIndex].Item1.Background = (SolidColorBrush)FindResource("MyLightGray");
				cakeGroupButton[selectedIndex].Item2.Foreground = (SolidColorBrush)FindResource("MyBlack");
			}

			cakeGroupListBox.SelectedItems.Clear();

			if (this.IsLoaded)
			{
				if (_isSearching)
				{
					//loadRecipesSearch();
				}
				else
				{
					loadCakes();
				}
			}
		}

		private void gridViewButton_Click(object sender, RoutedEventArgs e)
		{
			if (largeCakesListView.Visibility == Visibility.Visible)
			{
				largeCakesListView.Visibility = Visibility.Collapsed;
				smallCakesListView.Visibility = Visibility.Visible;

				totalCakePerPage = 6;
			} 
			else
			{
				largeCakesListView.Visibility = Visibility.Visible;
				smallCakesListView.Visibility = Visibility.Collapsed;

				totalCakePerPage = 9;
			}

			if (_isSearching)
			{
				//loadRecipesSearch();
			}
			else
			{
				loadCakes();
			}
		}

		private void smallCakesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{

		}

		private void CakesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ListView listView = smallCakesListView;

			if (largeCakesListView.Visibility == Visibility.Visible)
			{
				listView = largeCakesListView;
			}
			else
			{
				listView = smallCakesListView;
			}

			var selectedItemIndex = listView.SelectedIndex;
			int selectedID = -1;

			if (selectedItemIndex != -1)
			{
				//selectedID = ((Recipe)recipesListView.SelectedItem).ID_RECIPE;
				selectedID = int.Parse(((Grid)listView.SelectedItem).Tag.ToString());
				Debug.WriteLine(selectedID);

				ShowCakeDetailPage?.Invoke(selectedID);
			}
		}

		private void sortTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (this.IsLoaded)
			{
				_sortedBy = sortTypeComboBox.SelectedIndex;

				_configuration.AppSettings.Settings["SortedByHomePage"].Value = _sortedBy.ToString();
				_configuration.Save(ConfigurationSaveMode.Modified);

				if (_isSearching)
				{
					//loadJourneySearch();
				}
				else
				{
					loadCakes();
				}
			}
		}

		private string getConditionInQuery()
		{
			string result = "";

			if (cakeGroupListBox.SelectedItems.Count > 0)
			{
				result += "(";
			}

			//Select * from [dbo].[Cake] where Type_Cake = N'Topping' OR
			string[] typeCakes = { "Topping", "Glazed", "Filling"};
			foreach (var cakeListBoxSelectedItem in cakeGroupListBox.SelectedItems)
			{
				var selectedButton = ((Grid)cakeListBoxSelectedItem);
				int index = int.Parse(selectedButton.Tag.ToString());
				result += $" Type_Cake = N\'{typeCakes[index]}\' OR";
			}

			if (result.Length > 0)
			{
				result = result.Substring(0, result.Length - 3);

				if (cakeGroupListBox.SelectedItems.Count > 0)
				{
					result += ")";
				}
			}

			return result;
		}

		private int getMaxPage(int totalResult)
		{
			var result = Math.Ceiling((double)totalResult / totalCakePerPage);

			return (int)result;
		}

		private void loadCakes()
        {
			if (!_isSearching)
			{
				string condition = getConditionInQuery();

				if (_prevCondition != condition)
				{
					_currentPage = 1;
					_prevCondition = condition;
				}
				else
				{
					//Do Nothing
				}

				(List<Cake> cakes, int totalCakeResult) resultQuery = _databaseUtilities.ExecQureyToGetCakes(condition, _conditionSortedBy[_sortedBy], _currentPage, totalCakePerPage);
				List<Cake> cakes = resultQuery.cakes;

				_maxPage = getMaxPage(resultQuery.totalCakeResult);


				if (_maxPage == 0)
				{
					_maxPage = 1;
					_currentPage = 1;
				}

				currentPageTextBlock.Text = $"{_currentPage} of {(_maxPage)}";


				currentResultTextBlock.Text = $"Hiển thị {cakes.Count} Trong tổng số {resultQuery.totalCakeResult} chuyến đi";

				largeCakesListView.ItemsSource = cakes;
				smallCakesListView.ItemsSource = cakes;
			}
			else
			{

			}
		}

        private void firstPageButton_Click(object sender, RoutedEventArgs e)
        {
			_currentPage = 1;

			if (_isSearching)
			{
				//loadRecipesSearch();
			}
			else
			{
				loadCakes();
			}
		}

        private void prevPageButton_Click(object sender, RoutedEventArgs e)
        {
			if (_currentPage > 1)
			{
				--_currentPage;
			}

			if (_isSearching)
			{
				//loadRecipesSearch();
			}
			else
			{
				loadCakes();
			}
		}

        private void nextPageButton_Click(object sender, RoutedEventArgs e)
        {
			if (_currentPage < (int)_maxPage)
			{
				++_currentPage;
			}

			if (_isSearching)
			{
				//loadRecipesSearch();
			}
			else
			{
				loadCakes();
			}
		}

        private void lastPageButton_Click(object sender, RoutedEventArgs e)
        {
			_currentPage = _maxPage;

			if (_isSearching)
			{
				//loadRecipesSearch();
			}
			else
			{
				loadCakes();
			}
		}
    }
}
