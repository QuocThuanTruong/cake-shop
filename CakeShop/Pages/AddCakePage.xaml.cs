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
using CakeShop.Utilities;
using System.Windows.Forms;
using System.Diagnostics;

namespace CakeShop.Pages
{
	/// <summary>
	/// Interaction logic for AddCakePage.xaml
	/// </summary>
	public partial class AddCakePage : Page
	{
		private bool isUpdate = false;

		private DatabaseUtilities _databaseUtilities = DatabaseUtilities.GetDatabaseInstance();
		private Cake _cake = new Cake();

		public List<Cake_Image> Images_For_Binding;

		private int _ordinal_number_image = 0;

		public AddCakePage()
		{
			InitializeComponent();
			updateTextBlock.Visibility = Visibility.Collapsed;
			this.isUpdate = false;
		}

		public AddCakePage(int cakeID)
		{
			InitializeComponent();

			updateTextBlock.Visibility = Visibility.Visible;
			this.isUpdate = true;

			_cake = _databaseUtilities.getCakeById(cakeID);

			DataContext = this._cake;
		}

		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
			switch (_cake.Type_Cake)
            {
				case "Topping":
					cakeCategoryComboBox.SelectedIndex = 0;
					break;
				case "Glazed":
					cakeCategoryComboBox.SelectedIndex = 1;
					break;
				case "Filling":
					cakeCategoryComboBox.SelectedIndex = 2;
					break;

			}

			Images_For_Binding = new List<Cake_Image>(_cake.CAKE_IMAGE_FOR_BINDING);

			if (Images_For_Binding.Count > 0)
			{
				addCakeImagesOption1Button.Visibility = Visibility.Collapsed;
				addCakeImagesOption2Button.Visibility = Visibility.Visible;
				cakeImageListView.Visibility = Visibility.Visible;
			}

			_ordinal_number_image = _databaseUtilities.GetMaxOrdinalNumberImage(_cake.ID_Cake);
		}

		private void addCakeImagesButton_Click(object sender, RoutedEventArgs e)
		{
			using (OpenFileDialog openFileDialog = new OpenFileDialog())
			{
				openFileDialog.Multiselect = true;
				openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
				openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg;*.ico)|*.png;*.jpeg;*.jpg;*.ico";

				if (openFileDialog.ShowDialog() == DialogResult.OK)
				{
                    var imageIdx = 0;

                    foreach (var fileName in openFileDialog.FileNames)
                    {
                        Cake_Image image = new Cake_Image();

                        image.Link_Image = fileName;
                        image.ImageIndex = Images_For_Binding.Count + 1;
                        image.Is_Active = 1;
                        image.Ordinal_Number = ++_ordinal_number_image;

                        Images_For_Binding.Add(image);
                    }

                    addCakeImagesOption1Button.Visibility = Visibility.Collapsed;
                    addCakeImagesOption2Button.Visibility = Visibility.Visible;
                    cakeImageListView.Visibility = Visibility.Visible;
                    cakeImageListView.ItemsSource = null;
                    cakeImageListView.ItemsSource = Images_For_Binding;
                }
			}
		}

        private void avatarPickerFrameButton_Click(object sender, RoutedEventArgs e)
        {
			using (OpenFileDialog openFileDialog = new OpenFileDialog())
			{
				openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
				openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg;*.ico)|*.png;*.jpeg;*.jpg;*.ico";

				if (openFileDialog.ShowDialog() == DialogResult.OK)
				{
					BitmapImage bitmap = new BitmapImage();

					bitmap.BeginInit();
					bitmap.CacheOption = BitmapCacheOption.OnLoad;
					bitmap.UriSource = new Uri(openFileDialog.FileName, UriKind.Relative);
					bitmap.EndInit();

					avatarImage.Source = bitmap;
				}
			}
		}

        private void deleteRelativeImageInListButton_Click(object sender, RoutedEventArgs e)
        {
			var clickedButton = (System.Windows.Controls.Button)sender;

			Debug.WriteLine(clickedButton.Tag);

			Images_For_Binding[int.Parse(clickedButton.Tag.ToString()) - 1].Is_Active = 0;

			updateRelativeImageIndex();
		}
		private void updateRelativeImageIndex()
		{
			var total_collapsed_image = 0;

			foreach (var image in Images_For_Binding)
			{
				if (image.Is_Active == 0)
				{
					++total_collapsed_image;

				}
			}

			if (Images_For_Binding.Count == total_collapsed_image)
			{
				addCakeImagesOption1Button.Visibility = Visibility.Visible;
				addCakeImagesOption2Button.Visibility = Visibility.Collapsed;
				cakeImageListView.Visibility = Visibility.Collapsed;
			}
			else
			{
				cakeImageListView.ItemsSource = null;
				cakeImageListView.ItemsSource = Images_For_Binding;
			}
		}

        private void saveCakeButton_Click(object sender, RoutedEventArgs e)
        {
			_cake.Name_Cake = cakeNameTextBox.Text;
			if (_cake.Name_Cake.Length == 0)
			{
				//notiMessageSnackbar.MessageQueue.Enqueue($"Không được bỏ trống tên chuyến đi", "OK", () => { });
				return;
			}

			string[] typeCakes = { "Topping", "Glazed", "Filling" };
			_cake.Type_Cake = typeCakes[cakeCategoryComboBox.SelectedIndex];


		}
    }
}
