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
				var renderer = Platform.GetOrCreateRenderer(visualElement);
				var nativeElement = renderer?.GetNativeElement();
				if(nativeElement != null)
				{
					nativeElement.Initialized += (sender, args) =>
					{
						visualElement.Layout(new Rectangle(0, 0, nativeElement.Bounds.Width, nativeElement.Bounds.Height));
					};

					nativeElement.LayoutUpdated += (sender, args) =>
					{
						visualElement.Layout(new Rectangle(0, 0, nativeElement.Bounds.Width, nativeElement.Bounds.Height));
					};

					return nativeElement;
				}

				var containerElement = renderer?.ContainerElement;
				if (containerElement != null)
				{
					containerElement.Initialized += (sender, args) =>
					{
						visualElement.Layout(new Rectangle(0, 0, containerElement.Bounds.Width, containerElement.Bounds.Height));
					};

					containerElement.LayoutUpdated += (sender, args) =>
					{
						visualElement.Layout(new Rectangle(0, 0, containerElement.Bounds.Width, containerElement.Bounds.Height));
					};

					return containerElement;
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
