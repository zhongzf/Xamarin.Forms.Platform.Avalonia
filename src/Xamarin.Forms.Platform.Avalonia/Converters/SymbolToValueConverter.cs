using System;
using System.Globalization;
using Xamarin.Forms.Platform.Avalonia.Enums;

namespace Xamarin.Forms.Platform.Avalonia.Converters
{
	public class SymbolToValueConverter : global::Avalonia.Data.Converters.IValueConverter
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
