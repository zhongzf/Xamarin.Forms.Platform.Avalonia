using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Xamarin.Forms.Platform.Avalonia.Converters
{
    public class PageToRenderedElementConverter : global::Avalonia.Data.Converters.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var page = value as Page;
            if (page == null)
                return null;

            IVisualElementRenderer renderer = page.GetOrCreateRenderer();
            if (renderer == null)
                return null;

            return renderer.ContainerElement;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
