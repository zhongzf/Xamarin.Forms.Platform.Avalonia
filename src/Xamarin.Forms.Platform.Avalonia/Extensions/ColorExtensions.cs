using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Media;

namespace Xamarin.Forms.Platform.Avalonia
{ 
	public static class ColorExtensions
	{
		public static global::Avalonia.Media.Color GetContrastingColor(this global::Avalonia.Media.Color color)
		{
			var nThreshold = 105;
			int bgLuminance = Convert.ToInt32(color.R * 0.2 + color.G * 0.7 + color.B * 0.1);

			var contrastingColor = 255 - bgLuminance < nThreshold ? Colors.Black : Colors.White;
			return contrastingColor;
		}

		public static Brush ToBrush(this Color color)
		{
			return new SolidColorBrush(color.ToNativeColor());
		}

		public static global::Avalonia.Media.Color ToNativeColor(this Color color)
		{
			return global::Avalonia.Media.Color.FromArgb((byte)(color.A * 255), (byte)(color.R * 255), (byte)(color.G * 255), (byte)(color.B * 255));
		}

		public static Color ToFormsColor(this global::Avalonia.Media.Color color)
		{
			return Color.FromRgba(color.R, color.G, color.B, color.A);
		}

		public static Color ToFormsColor(this SolidColorBrush solidColorBrush)
		{
			return solidColorBrush.Color.ToFormsColor();
		}
	}
}
