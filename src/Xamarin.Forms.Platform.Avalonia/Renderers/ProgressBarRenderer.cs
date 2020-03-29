using Avalonia;
using System;
using System.ComponentModel;
using System.Windows;
using Xamarin.Forms.Internals;

namespace Xamarin.Forms.Platform.Avalonia
{
    public class ProgressBarRenderer : ViewRenderer<ProgressBar, AvaloniaForms.Controls.ProgressBar>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<ProgressBar> e)
        {
            if (e.NewElement != null)
            {
                if (Control == null) // construct and SetNativeControl and suscribe control event
                {
                    SetNativeControl(new AvaloniaForms.Controls.ProgressBar { Minimum = 0, Maximum = 1 });
                    Control.PropertyChanged += Control_PropertyChanged;
                }

                // Update control property 
                UpdateProgress();
                UpdateProgressColor();
            }

            base.OnElementChanged(e);
        }

        private void Control_PropertyChanged(object sender, global::Avalonia.AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Property == global::Avalonia.Controls.Primitives.RangeBase.ValueProperty)
            {
                HandleValueChanged(sender, e);
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == ProgressBar.ProgressProperty.PropertyName)
                UpdateProgress();
            else if (e.PropertyName == ProgressBar.ProgressColorProperty.PropertyName)
                UpdateProgressColor();
        }

        void UpdateProgressColor()
        {
            Control.UpdateDependencyColor(AvaloniaForms.Controls.ProgressBar.ForegroundProperty, Element.ProgressColor.IsDefault ? Color.DeepSkyBlue : Element.ProgressColor);
        }

        void UpdateProgress()
        {
            Control.Value = Element.Progress;
        }

        void HandleValueChanged(object sender, AvaloniaPropertyChangedEventArgs e)
        {
            ((IVisualElementController)Element)?.InvalidateMeasure(InvalidationTrigger.MeasureChanged);
        }

        bool _isDisposed;

        protected override void Dispose(bool disposing)
        {
            if (_isDisposed)
                return;

            if (disposing)
            {
                if (Control != null)
                {
                    Control.PropertyChanged -= Control_PropertyChanged;
                }
            }

            _isDisposed = true;
            base.Dispose(disposing);
        }

        public override SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
        {
            if (Children.Count == 0 || Control == null)
                return new SizeRequest();

            var constraint = new global::Avalonia.Size(widthConstraint, heightConstraint);
            var child = Control;

            child.Measure(constraint);
            var result = new Size(Math.Ceiling(child.DesiredSize.Width), Math.Max(Math.Ceiling(child.DesiredSize.Height), 10));

            return new SizeRequest(result);
        }
    }
}