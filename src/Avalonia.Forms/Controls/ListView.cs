using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Generators;
using Avalonia.Forms.Controls;
using Avalonia.Forms.Generators;
using Avalonia.Forms.Interfaces;
using Avalonia.Styling;
using System;
using System.Collections.Generic;
using System.Text;

namespace Avalonia.Forms.Controls
{
    public class ListView : ListBox, IStyleable
    {
        Type IStyleable.StyleKey => typeof(ListBox);

        public static readonly StyledProperty<object> ItemTemplateSelectorProperty = AvaloniaProperty.Register<ListView, object>(nameof(ItemTemplateSelector));

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
