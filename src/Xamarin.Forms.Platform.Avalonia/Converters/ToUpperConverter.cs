using System;

namespace Xamarin.Forms.Platform.Avalonia.Converters
{
	public class ToUpperConverter : global::Avalonia.Data.Converters.IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (value != null)
			{
				var strValue = value.ToString();

				return strValue.ToUpperInvariant();
			}
			return null;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotSupportedException();
		}

	}
}
