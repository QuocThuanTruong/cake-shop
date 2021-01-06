using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CakeShop.Converter;
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
		private AbsolutePathConverter _absolutePathConverter = new AbsolutePathConverter();

		private Timer _loadingTmer;
		private int _timeCounter = 0;

		private const int TIME_LOAD_UNIT = 100;
		private const int TOTAL_TIME_LOAD_IN_SECOND = 5;

		private int _sortedBy = 5;
		private int _currentPage = 1;
		private int totalCakePerPage = 9;
		private bool _isSearching = false;
		private bool _canSearchRequest = false;
		private int _maxPage = 0;
		private string _search_text = "";
		private string _condition = "";
		private bool _isFirstSearch = true;
		private string _prevCondition = "init";
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

			_loadingTmer = new Timer(TIME_LOAD_UNIT);
			_loadingTmer.Elapsed += LoadingTmer_Elapsed;
			_loadingTmer.Start();

			_configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

			_sortedBy = int.Parse(ConfigurationManager.AppSettings["SortedByHomePage"]);
			sortTypeComboBox.SelectedIndex = _sortedBy;

			loadCakes();
		}

		private void LoadingTmer_Elapsed(object sender, ElapsedEventArgs e)
		{
			Dispatcher.Invoke(() =>
			{
				if (_isSearching)
				{
					++_timeCounter;

					if (_timeCounter % TOTAL_TIME_LOAD_IN_SECOND == 0 && _canSearchRequest)
					{
						_timeCounter = 0;

						loadCakesSearch();
					}
				}
				else
				{
					_timeCounter = 0;
				}

			});
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
					loadCakesSearch();
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
					loadCakesSearch();
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
				loadCakesSearch();
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
				//selectedID = ((Cake)CakesListView.SelectedItem).ID_Cake;
				//selectedID = int.Parse(((Grid)listView.SelectedItem).Tag.ToString());
				//Debug.WriteLine(selectedID);

				//ShowCakeDetailPage?.Invoke(selectedID);

				selectedID = ((Cake)listView.SelectedItem).ID_Cake;
			}

			ShowCakeDetailPage?.Invoke(selectedID);
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

				currentResultTextBlock.Text = $"Hiển thị {cakes.Count} Trong tổng số {resultQuery.totalCakeResult} kết quả";

				for (int i = 0; i < cakes.Count; ++i)
				{
					cakes[i].Name_In_Small_Grid = ApplicationUtilities.GetAppInstance().getStandardName(cakes[i].Name_Cake, 18);
					cakes[i].Name_In_Large_Grid = ApplicationUtilities.GetAppInstance().getStandardName(cakes[i].Name_Cake, 26);
				}

				largeCakesListView.ItemsSource = null;
				smallCakesListView.ItemsSource = null;

				largeCakesListView.ItemsSource = cakes;
				smallCakesListView.ItemsSource = cakes;
			}
			else
			{
				searchTextBox_TextChanged(null, null);
			}
		}

		private void loadCakesSearch()
		{
			(List<Cake> Cakes, int totalCakeResult) CakesSearchResults = _databaseUtilities.GetCakesSearchResult(_search_text, _condition, _conditionSortedBy[_sortedBy], _currentPage, totalCakePerPage);

			_maxPage = getMaxPage(CakesSearchResults.totalCakeResult);
			if (_maxPage == 0)
			{
				_maxPage = 1;
				_currentPage = 1;

				messageNotFoundContainer.Visibility = Visibility.Visible;
			}
			else
			{
				messageNotFoundContainer.Visibility = Visibility.Hidden;
			}

			currentPageTextBlock.Text = $"{_currentPage} of {(_maxPage)}";


			List<Cake> Cakes = CakesSearchResults.Cakes;

			largeCakesListView.ItemsSource = null;
			smallCakesListView.ItemsSource = null;

			if (Cakes.Count > 0)
			{
				largeCakesListView.ItemsSource = Cakes;
				smallCakesListView.ItemsSource = Cakes;

				currentResultTextBlock.Text = $"Hiển thị {Cakes.Count} trong tổng số {CakesSearchResults.totalCakeResult} kết quả";

				for (int i = 0; i < Cakes.Count; ++i)
				{
					Cakes[i].Name_In_Small_Grid = ApplicationUtilities.GetAppInstance().getStandardName(Cakes[i].Name_Cake, 18);
					Cakes[i].Name_In_Large_Grid = ApplicationUtilities.GetAppInstance().getStandardName(Cakes[i].Name_Cake, 26);
				}
			}
			else
			{
				largeCakesListView.ItemsSource = null;
				smallCakesListView.ItemsSource = null;

				currentResultTextBlock.Text = "Không tìm thấy bánh thỏa yêu cầu";
			}

			_canSearchRequest = false;
		}

		private void firstPageButton_Click(object sender, RoutedEventArgs e)
        {
			_currentPage = 1;

			if (_isSearching)
			{
				loadCakesSearch();
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
				loadCakesSearch();
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
				loadCakesSearch();
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
				loadCakesSearch();
			}
			else
			{
				loadCakes();
			}
		}

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
			string search_text = searchTextBox.Text;

			if (search_text.Length != 0)
			{
				_isSearching = true;

				if (_isFirstSearch)
				{
					_currentPage = 1;
					_isFirstSearch = false;
				}

				string condition = getConditionInQuery();

				if (_search_text != search_text || _condition != condition)
				{
					_search_text = search_text;
					_condition = condition;

					_canSearchRequest = true;
				}

				_condition = condition;

				if (_prevCondition != condition)
				{
					_currentPage = 1;
					_prevCondition = condition;
				}
				else
				{
					//Do Nothing
				}
			}
			else
			{
				_isSearching = false;

				loadCakes();
				messageNotFoundContainer.Visibility = Visibility.Collapsed;
			}
		}
    }
}
