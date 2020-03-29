﻿using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Generators;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvaloniaForms.Controls.Generators
{
    public class ListViewItemContainerGenerator<T> : ItemContainerGenerator<T> where T : class, IControl, new()
    {
        /// <summary>
        /// Gets the container's ContentTemplate property.
        /// </summary>
        protected AvaloniaProperty DataTemplateSelectorProperty { get; }

        public ListViewItemContainerGenerator(IControl owner, AvaloniaProperty contentProperty, AvaloniaProperty contentTemplateProperty, AvaloniaProperty dataTemplateSelectorProperty)
            : base(owner, contentProperty, contentTemplateProperty)
        {
            DataTemplateSelectorProperty = dataTemplateSelectorProperty;
        }

        protected override IControl CreateContainer(object item)
        {
            var result = base.CreateContainer(item);
            var listViewItem = result as T;
            if (listViewItem != null)
            {
                var dataTemplateSelector = Owner.GetValue(DataTemplateSelectorProperty) as IDataTemplateSelector;
                if (dataTemplateSelector != null)
                {
                    var itemTemplate = dataTemplateSelector.SelectTemplate(item, result);
                    if (itemTemplate != null)
                    {
                        result.SetValue(ContentTemplateProperty, itemTemplate, BindingPriority.Style);
                    }
                }
            }
            return result;
        }

        public override bool TryRecycle(int oldIndex, int newIndex, object item) => false;
    }
}
