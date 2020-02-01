using Avalonia;
using System;
using System.Collections.Generic;
using System.Text;

namespace Xamarin.Forms.Platform.Avalonia.Interfaces
{
    public interface IDataTemplateSelector
    {
        global::Avalonia.Markup.Xaml.Templates.DataTemplate SelectTemplate(object item, AvaloniaObject container);
    }
}
