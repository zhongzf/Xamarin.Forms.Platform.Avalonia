using Avalonia.Media.Imaging;
using System;
using System.Globalization;
using Xamarin.Forms.Internals;

namespace Xamarin.Forms.Platform.Avalonia.Converters
{
	public sealed class ImageConverter : global::Avalonia.Data.Converters.IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var task = (value as ImageSource)?.ToNativeImageSourceAsync();
			return task?.AsAsyncValue() ?? AsyncValue<Bitmap>.Null;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
