using Avalonia.Forms.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Avalonia.Forms.Controls
{
    public class MasterDetailPage : DynamicContentPage
    {
        public static readonly StyledProperty<object> MasterPageProperty = AvaloniaProperty.Register<MasterDetailPage, object>(nameof(MasterPage));
        public static readonly StyledProperty<object> DetailPageProperty = AvaloniaProperty.Register<MasterDetailPage, object>(nameof(DetailPage));
        public static readonly StyledProperty<bool> IsPresentedProperty = AvaloniaProperty.Register<MasterDetailPage, bool>(nameof(IsPresented));

        public object MasterPage
        {
            get { return (object)GetValue(MasterPageProperty); }
            set { SetValue(MasterPageProperty, value); }
        }

        public object DetailPage
        {
            get { return (object)GetValue(DetailPageProperty); }
            set { SetValue(DetailPageProperty, value); }
        }

        public bool IsPresented
        {
            get { return (bool)GetValue(IsPresentedProperty); }
            set { SetValue(IsPresentedProperty, value); }
        }
    }
}
