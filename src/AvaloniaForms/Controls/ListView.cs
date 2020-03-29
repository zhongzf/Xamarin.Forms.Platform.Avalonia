using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Generators;
using AvaloniaForms.Controls;
using Avalonia.Styling;
using System;
using System.Collections.Generic;
using System.Text;
using AvaloniaForms.Controls.Generators;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Presenters;

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
            VirtualizationMode = ItemVirtualizationMode.None;
        }

        protected override IItemContainerGenerator CreateItemContainerGenerator()
        {
            return new ListViewItemContainerGenerator<ListViewItem>(this, ListViewItem.ContentProperty, ListViewItem.ContentTemplateProperty, ListView.ItemTemplateSelectorProperty);
        }

        public ItemsPresenter ItemsPresenterPart { get; set; }

        protected override void OnTemplateApplied(TemplateAppliedEventArgs e)
        {
            base.OnTemplateApplied(e);

            ItemsPresenterPart = e.NameScope.Get<ItemsPresenter>("PART_ItemsPresenter");
        }
    }
}
