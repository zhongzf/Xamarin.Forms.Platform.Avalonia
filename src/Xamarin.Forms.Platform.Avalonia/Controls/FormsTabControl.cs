using System;
using System.Collections.Generic;
using System.Text;

namespace Xamarin.Forms.Platform.Avalonia.Controls
{
    public class FormsTabControl : AvaloniaForms.Controls.TabControl
    {
        public FormsTabControl()
        {
            var containedPageTemplate = (global::Avalonia.Markup.Xaml.Templates.DataTemplate)global::Avalonia.Application.Current.Resources["ContainedPageTemplate"];
            ContentTemplate = containedPageTemplate;

            var tabbedPageHeaderTemplate = (global::Avalonia.Markup.Xaml.Templates.DataTemplate)global::Avalonia.Application.Current.Resources["TabbedPageHeaderTemplate"];
            ItemTemplate = tabbedPageHeaderTemplate;
        }
    }
}
