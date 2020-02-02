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

        public static T GetDefaultValue<T>(this StyledPropertyMetadata<T> propertyMetadata)
        {
            return default(T);
        }
    }
}
