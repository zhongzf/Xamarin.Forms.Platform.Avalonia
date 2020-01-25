using Avalonia.Layout;
using Avalonia.Media;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform.Avalonia.Extensions;
using AButton = Avalonia.Controls.Button;
using AImage = Avalonia.Controls.Image;
using AThickness = Avalonia.Thickness;

namespace Xamarin.Forms.Platform.Avalonia
{
	public class ImageButtonRenderer : ViewRenderer<ImageButton, AButton>, IVisualElementRenderer
	{
		bool _disposed;
		AButton _button;
		AImage _image;

		public ImageButtonRenderer() : base() { }

		protected async override void OnElementChanged(ElementChangedEventArgs<ImageButton> e)
		{
			base.OnElementChanged(e);

			if (e.NewElement != null)
			{
				if (Control == null)
				{
					_image = new AImage
					{
						VerticalAlignment = VerticalAlignment.Center,
						HorizontalAlignment = HorizontalAlignment.Center,
						Stretch = Stretch.Uniform
					};

					//_image.ImageFailed += OnImageFailed;

					_button = new AButton
					{
						Padding = new AThickness(0),
						BorderThickness = new AThickness(0),
						Background = null,

						Content = _image
					};

					//_button.Click += OnButtonClick;

					SetNativeControl(_button);
				}

				if (Element.BorderColor != Color.Default)
					UpdateBorderColor();

				if (Element.BorderWidth != 0)
					UpdateBorderWidth();

				await TryUpdateSource().ConfigureAwait(false);
				UpdateAspect();

				if (Element.IsSet(Button.PaddingProperty))
					UpdatePadding();
			}
		}

		protected async override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == ImageButton.SourceProperty.PropertyName)
				await TryUpdateSource().ConfigureAwait(false);
			else if (e.PropertyName == ImageButton.BorderColorProperty.PropertyName)
				UpdateBorderColor();
			else if (e.PropertyName == ImageButton.BorderWidthProperty.PropertyName)
			{
				UpdateBorderWidth();
				UpdatePadding();
			}
			else if (e.PropertyName == ImageButton.AspectProperty.PropertyName)
				UpdateAspect();
			else if (e.PropertyName == Button.PaddingProperty.PropertyName)
				UpdatePadding();
		}

		protected override void Dispose(bool disposing)
		{
			if (_disposed)
				return;

			if (disposing)
			{
				if (_image != null)
				{
					//_image.ImageFailed -= OnImageFailed;
				}

				if (_button != null)
				{
					//_button.Click -= OnButtonClick;
				}
			}

			_disposed = true;
			base.Dispose(disposing);
		}

		protected virtual async Task TryUpdateSource()
		{
			try
			{
				await UpdateSource().ConfigureAwait(false);
			}
			catch (Exception ex)
			{
				Log.Warning(nameof(ImageButtonRenderer), "Error loading image: {0}", ex);
			}
			finally
			{
				Element.SetIsLoading(false);
			}
		}

		protected async Task UpdateSource()
		{
			if (Element == null || Control == null)
				return;

			Element.SetIsLoading(true);

			ImageSource source = Element.Source;
			IImageSourceHandler handler;
			if (source != null && (handler = Registrar.Registered.GetHandlerForObject<IImageSourceHandler>(source)) != null)
			{
				global::Avalonia.Media.Imaging.Bitmap imageSource;

				try
				{
					imageSource = await handler.LoadImageAsync(source);
				}
				catch (OperationCanceledException)
				{
					imageSource = null;
				}

				// In the time it takes to await the imagesource, some zippy little app
				// might have disposed of this Image already.
				if (_image != null)
					_image.Source = imageSource;

				RefreshImage();
			}
			else
			{
				_image.Source = null;
				Element.SetIsLoading(false);
			}
		}

		void RefreshImage()
		{
			((IVisualElementController)Element)?.InvalidateMeasure(InvalidationTrigger.RendererReady);
		}

		//void OnButtonClick(object sender, System.Windows.RoutedEventArgs e)
		//{
		//	((IButtonController)Element)?.SendReleased();
		//	((IButtonController)Element)?.SendClicked();
		//}

		//void OnImageFailed(object sender, System.Windows.ExceptionRoutedEventArgs e)
		//{
		//	Log.Warning("Image loading", $"Image failed to load: {e.ErrorException.Message}");
		//	Element?.SetIsLoading(false);
		//}

		void UpdateBorderColor()
		{
			Control.UpdateDependencyColor(AButton.BorderBrushProperty, Element.BorderColor);
		}

		void UpdateBorderWidth()
		{
			Control.BorderThickness =
				Element.BorderWidth <= 0d
					? new AThickness(1)
					: new AThickness(Element.BorderWidth);
		}

		void UpdateAspect()
		{
			_image.Stretch = Element.Aspect.ToStretch();
			if (Element.Aspect == Aspect.Fill)
			{
				Control.HorizontalAlignment = HorizontalAlignment.Center;
				Control.VerticalAlignment = VerticalAlignment.Center;
			}
			else
			{
				Control.HorizontalAlignment = HorizontalAlignment.Left;
				Control.VerticalAlignment = VerticalAlignment.Top;
			}
		}

		void UpdatePadding()
		{
			Control.Padding = new AThickness(
				Element.Padding.Left,
				Element.Padding.Top,
				Element.Padding.Right,
				Element.Padding.Bottom
			);
		}
	}
}
