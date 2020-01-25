using Avalonia;
using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Xamarin.Forms.Platform.Avalonia.Extensions;

namespace Xamarin.Forms.Platform.Avalonia.Converters
{
	public sealed class ColorConverter : global::Avalonia.Data.Converters.IMultiValueConverter
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
}
