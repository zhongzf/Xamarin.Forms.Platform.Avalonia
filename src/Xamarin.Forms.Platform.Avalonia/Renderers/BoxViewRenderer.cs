using Avalonia.Controls;
using Avalonia.Media;
using System;
using System.ComponentModel;
using Xamarin.Forms.Platform.Avalonia.Extensions;
using ARectangle = Avalonia.Controls.Shapes.Rectangle;

namespace Xamarin.Forms.Platform.Avalonia
{
	public class BoxViewRenderer : ViewRenderer<BoxView, ARectangle>
	{
		Border _border;

		protected override void OnElementChanged(ElementChangedEventArgs<BoxView> e)
		{
			if (e.NewElement != null)
			{
				if (Control == null) // Construct and SetNativeControl and suscribe control event
				{
					ARectangle rectangle = new ARectangle();

					_border = new Border();

					VisualBrush visualBrush = new VisualBrush
					{
						Visual = _border
					};

					rectangle.Fill = visualBrush;

					SetNativeControl(rectangle);
				}

				UpdateBackground();
				UpdateCornerRadius();
			}

			base.OnElementChanged(e);
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == BoxView.ColorProperty.PropertyName)
				UpdateBackground();
			else if (e.PropertyName == BoxView.CornerRadiusProperty.PropertyName)
				UpdateCornerRadius();
		}

		protected override void UpdateNativeWidget()
		{
			base.UpdateNativeWidget();

			UpdateSize();
		}

		protected override void UpdateBackground()
		{
			Color color = Element.Color != Color.Default ? Element.Color : Element.BackgroundColor;
			_border.UpdateDependencyColor(Border.BackgroundProperty, color);
		}

		void UpdateCornerRadius()
		{
			var cornerRadius = Element.CornerRadius;
			_border.CornerRadius = new global::Avalonia.CornerRadius(cornerRadius.TopLeft, cornerRadius.TopRight, cornerRadius.BottomRight, cornerRadius.BottomLeft);
		}

		void UpdateSize()
		{
			_border.Height = Element.Height > 0 ? Element.Height : Double.NaN;
			_border.Width = Element.Width > 0 ? Element.Width : Double.NaN;
		}
	}
}