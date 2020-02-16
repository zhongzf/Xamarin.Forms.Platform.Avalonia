using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Text;

namespace Xamarin.Forms.Platform.Avalonia
{
    public class DefaultViewRenderer : ViewRenderer<View, Control>
    {
    }

    public class ViewRenderer<TElement, TNativeElement> : VisualElementRenderer<TElement, TNativeElement>
        where TElement : View
        where TNativeElement : Control
    {
        protected virtual void UpdateBackground()
        {
        }
    }
}
