using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CakeShop.Converter
{
	class AbsolutePathConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string relativePath = (string)value;

			var pattern = "([A-Z]:\\.*)";
			var regex = new Regex(pattern);

			if (regex.IsMatch(relativePath))
			{
				return relativePath;
			}

			string directory = AppDomain.CurrentDomain.BaseDirectory;
			string absolutePath = $"{directory}{relativePath}";

			Debug.WriteLine(absolutePath);
			return absolutePath;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
