using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;

namespace Xamarin.Forms.Platform.Avalonia.Converters
{
	public sealed class HeightConverter : global::Avalonia.Data.Converters.IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			double def = 50;
			var ps = parameter as string;
			// TODO:
			//if (string.IsNullOrWhiteSpace(ps) || !double.TryParse(ps, out def))
			//{
			//	def = double.NaN;
			//}
			var val = (double)value;
			return val > 0 ? val : def;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
