using Avalonia;
using System.ComponentModel;
using System.Windows;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform.Avalonia.Extensions;
using AProgressBar = Avalonia.Controls.ProgressBar;

namespace Xamarin.Forms.Platform.Avalonia
{
	public class ProgressBarRenderer : ViewRenderer<ProgressBar, AProgressBar>
	{
		protected override void OnElementChanged(ElementChangedEventArgs<ProgressBar> e)
		{
			if (e.NewElement != null)
			{
				if (Control == null) // construct and SetNativeControl and suscribe control event
				{
					SetNativeControl(new AProgressBar { Minimum = 0, Maximum = 1 });
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
			if(e.Property == global::Avalonia.Controls.Primitives.RangeBase.ValueProperty)
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
			Control.UpdateDependencyColor(AProgressBar.ForegroundProperty, Element.ProgressColor.IsDefault ? Color.DeepSkyBlue : Element.ProgressColor);
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
	}
}