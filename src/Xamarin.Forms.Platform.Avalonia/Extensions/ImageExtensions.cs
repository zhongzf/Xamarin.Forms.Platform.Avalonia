using Avalonia.Media;
using Avalonia.Media.Imaging;
using AvaloniaForms.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms.Internals;

namespace Xamarin.Forms.Platform.Avalonia
{
	public static class ImageExtensions
	{
		public static Stretch ToStretch(this Aspect aspect)
		{
			switch (aspect)
			{
				case Aspect.Fill:
					return Stretch.Fill;
				case Aspect.AspectFill:
					return Stretch.UniformToFill;
				default:
				case Aspect.AspectFit:
					return Stretch.Uniform;
			}
		}

		public static Size GetImageSourceSize(this Bitmap source)
		{
			if (source is null)
			{
				return Size.Zero;
			}
			else if (source is Bitmap bitmap)
			{
				return new Size
				{
					Width = bitmap.PixelSize.Width,
					Height = bitmap.PixelSize.Height
				};
			}

			throw new InvalidCastException($"\"{source.GetType().FullName}\" is not supported.");
		}

		public static IconElement ToNativeIconElement(this ImageSource source)
		{
			return source.ToNativeIconElementAsync().GetAwaiter().GetResult();
		}

		public static async Task<IconElement> ToNativeIconElementAsync(this ImageSource source, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (source == null || source.IsEmpty)
				return null;

			var handler = Internals.Registrar.Registered.GetHandlerForObject<IIconElementHandler>(source);
			if (handler == null)
				return null;

			try
			{
				return await handler.LoadIconElementAsync(source, cancellationToken);
			}
			catch (OperationCanceledException)
			{
				// no-op
			}

			return null;
		}

		public static Bitmap ToWindowsImageSource(this ImageSource source)
		{
			return source.ToNativeImageSourceAsync().GetAwaiter().GetResult();
		}

		public static async Task<Bitmap> ToNativeImageSourceAsync(this ImageSource source, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (source == null || source.IsEmpty)
				return null;

			var handler = Registrar.Registered.GetHandlerForObject<IImageSourceHandler>(source);
			if (handler == null)
				return null;

			try
			{
				return await handler.LoadImageAsync(source, cancellationToken);
			}
			catch (OperationCanceledException)
			{
				Log.Warning("Image loading", "Image load cancelled");
			}
			catch (Exception ex)
			{
				Log.Warning("Image loading", $"Image load failed: {ex}");
			}

			return null;
		}
	}
}
