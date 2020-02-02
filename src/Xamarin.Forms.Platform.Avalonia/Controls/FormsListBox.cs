﻿using Avalonia;
using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Platform.Avalonia.Interfaces;

namespace Xamarin.Forms.Platform.Avalonia.Controls
{
    public class FormsListBox : ListBox
    {
        public static readonly StyledProperty<object> ItemTemplateSelectorProperty = AvaloniaProperty.Register<FormsListBox, object>(nameof(ItemTemplateSelector));

        public object ItemTemplateSelector
        {
            get { return (object)GetValue(ItemTemplateSelectorProperty); }
            set { SetValue(ItemTemplateSelectorProperty, value); }
        }

        public FormsListBox()
        {
            ItemTemplateSelectorProperty.Changed.AddClassHandler<FormsListBox>((x, e) =>
            {
                var itemTemplateSelector = e.NewValue as IDataTemplateSelector;
                if (itemTemplateSelector != null)
                {
                    ItemTemplate = itemTemplateSelector.SelectTemplate(null, this);
                }
            });
        }
    }
}