using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Media;
using AvaloniaForms.Helpers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AvaloniaForms.Extensions
{
    public static class ControlExtensions
    {
        static readonly Lazy<ConcurrentDictionary<Type, AvaloniaProperty>> ForegroundProperties =
           new Lazy<ConcurrentDictionary<Type, AvaloniaProperty>>(() => new ConcurrentDictionary<Type, AvaloniaProperty>());

        public static Brush GetForeground(this Control element)
        {
            if (element == null)
                throw new ArgumentNullException("element");

            return (Brush)element.GetValue(GetForegroundProperty(element));
        }

        public static Binding GetForegroundBinding(this Control element)
        {
            var expr = new Binding(GetForegroundProperty(element).Name) { Source = element };
            return expr;
        }

        public static object GetForegroundCache(this Control element)
        {
            var binding = GetForegroundBinding(element);
            if (binding != null)
                return binding;

            return GetForeground(element);
        }

        public static void RestoreForegroundCache(this Control element, object cache)
        {
            var binding = cache as Binding;
            if (binding != null)
                SetForeground(element, binding);
            else
                SetForeground(element, (Brush)cache);
        }

        public static void SetForeground(this Control element, Brush foregroundBrush)
        {
            if (element == null)
                throw new ArgumentNullException("element");

            element.SetValue(GetForegroundProperty(element), foregroundBrush);
        }

        public static void SetForeground(this Control element, Binding binding)
        {
            if (element == null)
                throw new ArgumentNullException("element");

            element.Bind(GetForegroundProperty(element), binding);
        }

        internal static IEnumerable<T> GetDescendantsByName<T>(this AvaloniaObject parent, string elementName)
            where T : AvaloniaObject
        {
            int myChildrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < myChildrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i) as AvaloniaObject;
                var controlName = child.GetValue(Control.NameProperty) as string;
                if (controlName == elementName && child is T)
                    yield return child as T;
                else
                {
                    foreach (var subChild in child.GetDescendantsByName<T>(elementName))
                        yield return subChild;
                }
            }
        }

        internal static T GetFirstDescendant<T>(this AvaloniaObject element) where T : Control
        {
            int count = VisualTreeHelper.GetChildrenCount(element);
            for (var i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(element, i) as AvaloniaObject;

                T target = child as T ?? GetFirstDescendant<T>(child);
                if (target != null)
                    return target;
            }

            return null;
        }

        static AvaloniaProperty GetForegroundProperty(Control element)
        {
            if (element is TemplatedControl)
                return TemplatedControl.ForegroundProperty;
            if (element is TextBlock)
                return TextBlock.ForegroundProperty;

            Type type = element.GetType();

            AvaloniaProperty foregroundProperty;
            if (!ForegroundProperties.Value.TryGetValue(type, out foregroundProperty))
            {
                var field = type?.GetFields().FirstOrDefault(f => f.Name == "ForegroundProperty");
                if (field == null)
                    throw new ArgumentException("type is not a Foregroundable type");

                var property = (AvaloniaProperty)field.GetValue(null);
                ForegroundProperties.Value.TryAdd(type, property);

                return property;
            }

            return foregroundProperty;
        }

        internal static IEnumerable<T> GetChildren<T>(this AvaloniaObject parent)
            where T : AvaloniaObject

        {
            int myChildrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < myChildrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i) as AvaloniaObject;
                if (child is T)
                    yield return child as T;
                else
                {
                    foreach (var subChild in child.GetChildren<T>())
                        yield return subChild;
                }
            }
        }
    }
}
