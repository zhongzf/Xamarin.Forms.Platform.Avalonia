using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Xamarin.Forms.Platform.Avalonia
{
    public interface IVisualNativeElementRenderer : IVisualElementRenderer
    {
        event EventHandler<PropertyChangedEventArgs> ElementPropertyChanged;
        event EventHandler ControlChanging;
        event EventHandler ControlChanged;
    }
}
