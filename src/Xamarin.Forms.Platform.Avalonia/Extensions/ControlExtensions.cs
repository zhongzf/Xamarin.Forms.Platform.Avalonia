using Avalonia;
using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Text;

namespace Xamarin.Forms.Platform.Avalonia.Extensions
{
    public static class ControlExtensions
    {
        public static object UpdateDependencyColor(this AvaloniaObject depo, AvaloniaProperty dp, Color newColor)
        {
            if (!newColor.IsDefault)
                depo.SetValue(dp, newColor.ToBrush());
            else
                depo.ClearValue(dp);

            return depo.GetValue(dp);
        }

        internal static IEnumerable<T> GetChildren<T>(this AvaloniaObject parent) where T : AvaloniaObject
        {
            var visual = parent as global::Avalonia.Visual;
            if (visual != null)
            {
                foreach (var child in (visual as global::Avalonia.VisualTree.IVisual).VisualChildren)
                {
                    if (child is T)
                    {
                        yield return child as T;
                    }
                    else
                    {
                        foreach (var subChild in (child as AvaloniaObject).GetChildren<T>())
                        {
                            yield return subChild;
                        }
                    }
                }
            }
        }
    }
}
