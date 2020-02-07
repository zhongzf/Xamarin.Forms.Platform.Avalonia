using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaForms.Converters
{
	public sealed class CaseConverter : global::Avalonia.Data.Converters.IValueConverter
	{
		public bool ConvertToUpper { get; set; }

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
				return null;

			var v = (string)value;
			return ConvertToUpper ? v.ToUpper() : v.ToLower();
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
