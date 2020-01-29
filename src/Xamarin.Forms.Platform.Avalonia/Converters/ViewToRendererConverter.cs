using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.Forms.Platform.Avalonia.Converters
{
	public sealed class ViewToRendererConverter : global::Avalonia.Data.Converters.IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			VisualElement visualElement = value as VisualElement;
			if (visualElement != null)
			{
				var frameworkElement = Platform.GetOrCreateRenderer(visualElement)?.GetNativeElement();

				if(frameworkElement != null)
				{
					frameworkElement.Initialized += (sender, args) =>
					{
						visualElement.Layout(new Rectangle(0, 0, frameworkElement.Bounds.Width, frameworkElement.Bounds.Height));
					};

					frameworkElement.LayoutUpdated += (sender, args) =>
					{
						visualElement.Layout(new Rectangle(0, 0, frameworkElement.Bounds.Width, frameworkElement.Bounds.Height));
					};

					return frameworkElement;
				}
			}
			return null;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
