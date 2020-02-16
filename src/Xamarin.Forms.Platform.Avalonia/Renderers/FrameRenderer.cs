using Avalonia.Controls;
using Avalonia.Media;
using System.ComponentModel;

namespace Xamarin.Forms.Platform.Avalonia
{
	public class FrameRenderer : ViewRenderer<Frame, Border>
	{
		public FrameRenderer()
		{
			AutoPackage = false;
		}

		protected override void OnElementChanged(ElementChangedEventArgs<Frame> e)
		{
			base.OnElementChanged(e);

			if (e.NewElement != null)
			{
				if (Control == null)
					SetNativeControl(new Border());

				PackChild();
				UpdateBorder();
				UpdateCornerRadius();
			}
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == "Content")
			{
				PackChild();
			}
			else if (e.PropertyName == Frame.BorderColorProperty.PropertyName || e.PropertyName == Frame.HasShadowProperty.PropertyName)
			{
				UpdateBorder();
			}
			else if (e.PropertyName == Frame.CornerRadiusProperty.PropertyName)
			{
				UpdateCornerRadius();
			}
		}

		protected override void UpdateBackgroundColor()
		{
			//background color change must be handled separately
			//because the background would protrude through the border if the corners are rounded
			//as the background would be applied to the renderer's FrameworkElement
			Color backgroundColor = Element.BackgroundColor;
			if (Control != null)
			{
				Control.Background = backgroundColor.IsDefault ? null : backgroundColor.ToBrush();
			}
		}

		void PackChild()
		{
			if (Element.Content == null)
				return;

			IVisualElementRenderer renderer = Element.Content.GetOrCreateRenderer();
			Control.Child = renderer.ContainerElement;
		}

		void UpdateBorder()
		{
			if (Element.BorderColor != Color.Default)
			{
				Control.BorderBrush = Element.BorderColor.ToBrush();
				Control.BorderThickness = new global::Avalonia.Thickness(1);
			}
			else
			{
				Control.BorderBrush = new Color(0, 0, 0, 0).ToBrush();
			}
		}

		void UpdateCornerRadius()
		{
			float cornerRadius = Element.CornerRadius;

			if (cornerRadius == -1f)
				cornerRadius = 5f; // default corner radius

			Control.CornerRadius = new global::Avalonia.CornerRadius(cornerRadius);
		}
	}

}
