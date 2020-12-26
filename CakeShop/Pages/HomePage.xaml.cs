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

namespace CakeShop.Pages
{
	/// <summary>
	/// Interaction logic for HomePage.xaml
	/// </summary>
	public partial class HomePage : Page
	{
		private List<Tuple<Button, TextBlock>> cakeGroupButton;

		public HomePage()
		{
			InitializeComponent();

			cakeGroupButton = new List<Tuple<Button, TextBlock>>()
			{
				new Tuple<Button, TextBlock>(toppingGroupButton, toppingTextBlock),
				new Tuple<Button, TextBlock>(glazedGroupButton, glazedTextBlock),
				new Tuple<Button, TextBlock>(fillingGroupButton, fillingTextBlock)
			};
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
		}

		private void gridViewButton_Click(object sender, RoutedEventArgs e)
		{
			if (largeCakesListView.Visibility == Visibility.Visible)
			{
				largeCakesListView.Visibility = Visibility.Collapsed;
				smallCakesListView.Visibility = Visibility.Visible;
			} 
			else
			{
				largeCakesListView.Visibility = Visibility.Visible;
				smallCakesListView.Visibility = Visibility.Collapsed;
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
			}
		}
	}
}
