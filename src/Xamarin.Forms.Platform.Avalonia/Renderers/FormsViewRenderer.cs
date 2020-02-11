using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Xamarin.Forms.Platform.Avalonia
{
    //public class DefaultViewRenderer : FormsViewRenderer<View, UserControl>
    //{
    //    protected override void OnElementChanged(ElementChangedEventArgs<View> e)
    //    {
    //        base.OnElementChanged(e);
    //        SetNativeControl(new UserControl());
    //    }
    //}

    public class FormsViewRenderer<TElement, TNativeElement> : VisualElementRenderer<TElement, TNativeElement> where TElement : View where TNativeElement : Control
    {
        protected override void OnElementChanged(ElementChangedEventArgs<TElement> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                UpdateBackgroundColor();
            }
        }

        internal virtual void OnModelFocusChangeRequested(object sender, VisualElement.FocusRequestArgs args)
        {
            if (Control == null)
                return;

            if (args.Focus)
            {
                Control.Focus();
                args.Result = Control.IsFocused;
            }
            else
            {
                UnfocusControl(Control);
                args.Result = true;
            }
        }

        internal void UnfocusControl(Control control)
        {
            if (control == null || !control.IsEnabled)
                return;
            // TODO:
            //control.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
        }

        protected virtual void UpdateBackground()
        {
            if (Control is global::Avalonia.Controls.Primitives.TemplatedControl templatedControl)
            {
                templatedControl?.UpdateDependencyColor(global::Avalonia.Controls.Primitives.TemplatedControl.BackgroundProperty, Element.BackgroundColor);
            }
        }

        protected virtual void UpdateHeight()
        {
            if (Control == null || Element == null)
                return;

            Control.Height = Element.Height > 0 ? Element.Height : Element.HeightRequest;
        }

        protected virtual void UpdateWidth()
        {
            if (Control == null || Element == null)
                return;

            Control.Width = Element.Width > 0 ? Element.Width : Element.WidthRequest;
        }

        protected virtual void UpdateNativeWidget()
        {
        }

        protected virtual void UpdateEnabled()
        {
            if (Control != null)
            {
                Control.IsEnabled = Element.IsEnabled;
            }
        }
    }
}
