using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
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

        public static T GetDefaultValue<T>(this StyledPropertyMetadata<T> propertyMetadata)
        {
            return default(T);
        }

        public static T Find<T>(this Control control, string name, TemplateAppliedEventArgs e = null) where T : class, IControl
        {
            if (e != null)
            {
                var child = global::Avalonia.Controls.NameScopeExtensions.Find<T>(e.NameScope, name);
                if (child == null)
                {
                    child = control.FindControl<T>(name);
                }
                return child;
            }
            return control.FindControl<T>(name);
        }
    }
}
