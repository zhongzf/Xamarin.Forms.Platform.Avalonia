using Avalonia;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.Forms.Platform.Avalonia
{
	public class SliderRenderer : ViewRenderer<Slider, global::Avalonia.Controls.Slider>
	{
		protected override void OnElementChanged(ElementChangedEventArgs<Slider> e)
		{
			if (e.NewElement != null)
			{
				if (Control == null) // construct and SetNativeControl and suscribe control event
				{
					SetNativeControl(new global::Avalonia.Controls.Slider());
					Control.PropertyChanged += Control_PropertyChanged;
				}

				// Update control property 
				UpdateMinimum();
				UpdateMaximum();
				UpdateValue();
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

			if (e.PropertyName == Slider.MinimumProperty.PropertyName)
				UpdateMinimum();
			else if (e.PropertyName == Slider.MaximumProperty.PropertyName)
				UpdateMaximum();
			else if (e.PropertyName == Slider.ValueProperty.PropertyName)
				UpdateValue();
		}
		
		void UpdateMinimum()
		{
			Control.Minimum = Element.Minimum;
		}

		void UpdateMaximum()
		{
			Control.Maximum = Element.Maximum;
		}

		void UpdateValue()
		{
			if (Control.Value != Element.Value)
				Control.Value = Element.Value;
		}

		void HandleValueChanged(object sender, AvaloniaPropertyChangedEventArgs e)
		{
			((IElementController)Element).SetValueFromRenderer(Slider.ValueProperty, Control.Value);
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