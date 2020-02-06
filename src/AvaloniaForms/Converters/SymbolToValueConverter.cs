using Avalonia.Data.Converters;
using AvaloniaForms.Controls;
using System;
using System.Globalization;

namespace AvaloniaForms.Converters
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
