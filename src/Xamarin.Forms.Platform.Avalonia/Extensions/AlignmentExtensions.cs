using Avalonia.Layout;
using System;
using System.Windows;

namespace Xamarin.Forms.Platform.Avalonia
{
	internal static class AlignmentExtensions
	{
		internal static global::Avalonia.Media.TextAlignment ToNativeTextAlignment(this TextAlignment alignment)
		{
			switch (alignment)
			{
				case TextAlignment.Center:
					return global::Avalonia.Media.TextAlignment.Center;
				case TextAlignment.End:
					return global::Avalonia.Media.TextAlignment.Right;
				default:
					return global::Avalonia.Media.TextAlignment.Left;
			}
		}

		internal static VerticalAlignment ToNativeVerticalAlignment(this TextAlignment alignment)
		{
			switch (alignment)
			{
				case TextAlignment.Start:
					return VerticalAlignment.Top;
				case TextAlignment.Center:
					return VerticalAlignment.Center;
				case TextAlignment.End:
					return VerticalAlignment.Bottom;
				default:
					return VerticalAlignment.Top;
			}
		}

		internal static VerticalAlignment ToNativeVerticalAlignment(this LayoutOptions alignment)
		{
			switch (alignment.Alignment)
			{
				case LayoutAlignment.Start:
					return VerticalAlignment.Top;
				case LayoutAlignment.Center:
					return VerticalAlignment.Center;
				case LayoutAlignment.End:
					return VerticalAlignment.Bottom;
				case LayoutAlignment.Fill:
					return VerticalAlignment.Stretch;
				default:
					return VerticalAlignment.Stretch;
			}
		}

		internal static HorizontalAlignment ToNativeHorizontalAlignment(this LayoutOptions alignment)
		{
			switch (alignment.Alignment)
			{
				case LayoutAlignment.Start:
					return HorizontalAlignment.Left;
				case LayoutAlignment.Center:
					return HorizontalAlignment.Center;
				case LayoutAlignment.End:
					return HorizontalAlignment.Right;
				case LayoutAlignment.Fill:
					return HorizontalAlignment.Stretch;
				default:
					return HorizontalAlignment.Stretch;
			}
		}
	}
}
