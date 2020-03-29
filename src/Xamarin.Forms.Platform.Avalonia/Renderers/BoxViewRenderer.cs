using Avalonia.Controls;
using Avalonia.Media;
using System;
using System.ComponentModel;
using Xamarin.Forms.Platform.Avalonia.Converters;

namespace Xamarin.Forms.Platform.Avalonia
{
	public class BoxViewRenderer : ViewRenderer<BoxView, AvaloniaForms.Controls.Border>
	{
		protected override void OnElementChanged(ElementChangedEventArgs<BoxView> e)
		{
			if (e.NewElement != null)
			{
				if (Control == null) // Construct and SetNativeControl and suscribe control event
				{
					var border = new AvaloniaForms.Controls.Border
					{
						DataContext = Element
					};

					border.SetBinding(AvaloniaForms.Controls.Border.BackgroundProperty, "Color", new ColorConverter());

					SetNativeControl(border);
				}

				UpdateCornerRadius();
			}

			base.OnElementChanged(e);
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == BoxView.CornerRadiusProperty.PropertyName)
				UpdateCornerRadius();
		}

		void UpdateCornerRadius()
		{
			var cornerRadius = Element.CornerRadius;
			Control.CornerRadius = new global::Avalonia.CornerRadius(cornerRadius.TopLeft, cornerRadius.TopRight, cornerRadius.BottomRight, cornerRadius.BottomLeft);
		}

		void UpdateSize()
		{
			Control.Height = Element.Height > 0 ? Element.Height : Double.NaN;
			Control.Width = Element.Width > 0 ? Element.Width : Double.NaN;
		}
	}
}