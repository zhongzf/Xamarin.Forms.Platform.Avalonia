using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Generators;
using AvaloniaForms.Controls;
using AvaloniaForms.Generators;
using AvaloniaForms.Interfaces;
using Avalonia.Styling;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvaloniaForms.Controls
{
    public class ListView : ListBox, IStyleable
    {
        public static readonly StyledProperty<object> ItemTemplateSelectorProperty = AvaloniaProperty.Register<ListView, object>(nameof(ItemTemplateSelector));

        static ListView()
        {
        }

        Type IStyleable.StyleKey => typeof(ListBox);

        public object ItemTemplateSelector
        {
            get { return GetValue(ItemTemplateSelectorProperty); }
            set { SetValue(ItemTemplateSelectorProperty, value); }
        }

        public ListView()
        {
        }

        protected override IItemContainerGenerator CreateItemContainerGenerator()
        {
            return new ListViewItemContainerGenerator<ListViewItem>(this, ListViewItem.ContentProperty, ListViewItem.ContentTemplateProperty, ListView.ItemTemplateSelectorProperty);
        }
    }
}
