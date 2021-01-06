using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace CakeShop.CustomView
{
	/// <summary>
	/// Interaction logic for CarouselDialog.xaml
	/// </summary>
	public partial class CarouselDialog : UserControl
	{
		private bool _hideRequest = false;
		private UIElement _parent;
		private List<Cake_Image> _cakeImages;

		public delegate void CloseCarouselDialogHandler();
		public event CloseCarouselDialogHandler CloseCarouselDialog;
		public CarouselDialog()
		{
			InitializeComponent();
			Visibility = Visibility.Collapsed;
		}

		public void SetParent(UIElement parent)
		{
			_parent = parent;
		}

		public void ShowDialog(List<Cake_Image> cakeImages, int selectedIndex)
		{
			_cakeImages = cakeImages;
			carouselRecipeImages.ItemsSource = _cakeImages;
			carouselRecipeImages.SelectedItem = _cakeImages[selectedIndex];
			currentImagePosTextBlock.Text = $"{selectedIndex + 1} of {_cakeImages.Count}";

			_parent.IsEnabled = false;
			_hideRequest = false;

			Visibility = Visibility.Visible;

			while (!_hideRequest)
			{
				//Stop if app close
				if (this.Dispatcher.HasShutdownStarted || this.Dispatcher.HasShutdownFinished)
				{
					break;
				}

				this.Dispatcher.Invoke(DispatcherPriority.Background, new ThreadStart(delegate { }));
				Thread.Sleep(20);
			}

		}

		public void HideDialog()
		{
			_hideRequest = true;
			Visibility = Visibility.Collapsed;
			_parent.IsEnabled = true;
		}

		private void closeWindowButton_Click(object sender, RoutedEventArgs e)
		{
			CloseCarouselDialog?.Invoke();
			HideDialog();
		}

		private void nextImageButton_Click(object sender, RoutedEventArgs e)
		{
			carouselRecipeImages.RotateLeft();
		}

		private void prevImageButton_Click(object sender, RoutedEventArgs e)
		{
			carouselRecipeImages.RotateRight();
		}

		private void carouselRecipeImages_SelectionChanged(FrameworkElement selectedElement)
		{
			currentImagePosTextBlock.Text = $"{_cakeImages.IndexOf((Cake_Image)carouselRecipeImages.SelectedItem) + 1} of {_cakeImages.Count}";
		}
	}
}
