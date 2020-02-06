using Avalonia.Data.Converters;
using Avalonia.Forms.Controls;
using System;
using System.Globalization;

namespace Avalonia.Forms.Converters
{
	public class SymbolToValueConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is Symbol symbol)
				return Char.ConvertFromUtf32((int)symbol);

			return null;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return null;
		}
	}
}
