﻿using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.Forms.Platform.Avalonia
{
	public class StepperRenderer : ViewRenderer<Stepper, Border>
	{
		readonly StackPanel _panel = new StackPanel();
		global::Avalonia.Controls.Button _downButton;
		global::Avalonia.Controls.Button _upButton;

		protected override void OnElementChanged(ElementChangedEventArgs<Stepper> e)
		{
			if (e.NewElement != null)
			{
				if (Control == null) // construct and SetNativeControl and suscribe control event
				{
					SetNativeControl(CreateControl());
					_upButton.Click += UpButtonOnClick;
					_downButton.Click += DownButtonOnClick;
				}

				// Update control property 
				UpdateButtons();
			}

			base.OnElementChanged(e);
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if(e.PropertyName == Stepper.MinimumProperty.PropertyName ||e.PropertyName == Stepper.MaximumProperty.PropertyName ||
				e.PropertyName == Stepper.ValueProperty.PropertyName)
				UpdateButtons();
		}

		Border CreateControl()
		{
			var border = new Border() { Child = _panel };
			_panel.HorizontalAlignment = HorizontalAlignment.Right;
			_panel.Orientation = Orientation.Horizontal;

			_upButton = new global::Avalonia.Controls.Button { Content = "+", Width = 100 };
			_downButton = new global::Avalonia.Controls.Button { Content = "-", Width = 100 };

			_panel.Children.Add(_downButton);
			_panel.Children.Add(_upButton);
			return border;
		}

		void DownButtonOnClick(object sender, RoutedEventArgs routedEventArgs)
		{
			((IElementController)Element).SetValueFromRenderer(Stepper.ValueProperty, Math.Max(Element.Minimum, Element.Value - Element.Increment));
		}

		void UpButtonOnClick(object sender, RoutedEventArgs routedEventArgs)
		{
			((IElementController)Element).SetValueFromRenderer(Stepper.ValueProperty, Math.Min(Element.Maximum, Element.Value + Element.Increment));
		}

		void UpdateButtons()
		{
			_upButton.IsEnabled = Element.Value < Element.Maximum;
			_downButton.IsEnabled = Element.Value > Element.Minimum;
		}

		protected override void UpdateEnabled()
		{
			_panel.IsEnabled = Element.IsEnabled;
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
					_upButton.Click -= UpButtonOnClick;
					_downButton.Click -= DownButtonOnClick;
				}
			}

			_isDisposed = true;
			base.Dispose(disposing);
		}
	}
}
