using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace Xamarin.Forms.Platform.Avalonia.Converters
{
	public sealed class ColorConverter : global::Avalonia.Data.Converters.IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var color = (Color)value;
			var defaultColorKey = (string)parameter;

			Brush defaultBrush = defaultColorKey != null && global::Avalonia.Application.Current.Resources.ContainsKey(defaultColorKey) ? (Brush)global::Avalonia.Application.Current.Resources[defaultColorKey] : new SolidColorBrush(Colors.Transparent);
			return color == Color.Default ? defaultBrush : color.ToBrush();
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

}
