using Avalonia.Media;
using System;
using System.Globalization;
using System.IO;
using Xamarin.Forms.Platform.Avalonia.Controls;
using Xamarin.Forms.Platform.Avalonia.Enums;

namespace Xamarin.Forms.Platform.Avalonia.Converters
{
	public class IconConveter : global::Avalonia.Data.Converters.IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is FileImageSource imageSource)
			{
				if (Enum.TryParse(imageSource.File, true, out Symbol symbol))
					return new FormsSymbolIcon() { Symbol = symbol };
				else if (TryParseGeometry(imageSource.File, out Geometry geometry))
					return new FormsPathIcon() { Data = geometry };
				else if (Path.GetExtension(imageSource.File) != null)
					return new FormsBitmapIcon() { UriSource = new Uri(imageSource.File, UriKind.RelativeOrAbsolute) };
			}

			return null;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		private bool TryParseGeometry(string value, out Geometry geometry)
		{
			geometry = null;
			try
			{
				geometry = Geometry.Parse(value);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}
	}
}
