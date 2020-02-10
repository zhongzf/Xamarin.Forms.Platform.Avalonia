using Avalonia;
using AvaloniaForms.Controls;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Platform.Avalonia.Converters;

namespace Xamarin.Forms.Platform.Avalonia.Controls
{
    public partial class FormsPageControl : PageControl
    {
        public static readonly StyledProperty<object> TitleViewProperty = AvaloniaProperty.Register<PageControl, object>(nameof(TitleView));

        public View TitleView
        {
            get { return (View)GetValue(TitleViewProperty); }
            set { SetValue(TitleViewProperty, value); }
        }

        public FormsPageControl()
        {
            this.Bind(PageControl.TitleViewContentProperty, new global::Avalonia.Data.Binding(nameof(TitleView)) { Converter = new ViewToRendererConverter() });
        }
    }
}
