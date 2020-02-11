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
	public sealed class ColorMultiValueConverter : global::Avalonia.Data.Converters.IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			Control framework = values[0] as Control;
			AvaloniaProperty dp = parameter as AvaloniaProperty;

			if (values.Count() > 1 && framework != null && values[1] is Color && dp != null)
			{
				return framework.UpdateDependencyColor(dp, (Color)values[1]);
			}
			return Color.Transparent.ToBrush();
		}

		public object Convert(IList<object> values, Type targetType, object parameter, CultureInfo culture)
		{
			return Convert(values.ToArray(), targetType, parameter, culture);
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

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
