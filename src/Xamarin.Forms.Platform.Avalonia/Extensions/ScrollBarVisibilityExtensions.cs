using AScrollBarVisibility = Avalonia.Controls.Primitives.ScrollBarVisibility;

namespace Xamarin.Forms.Platform.Avalonia.Extensions
{
	static class ScrollBarVisibilityExtensions
	{
		internal static AScrollBarVisibility ToNativeScrollBarVisibility(this ScrollBarVisibility visibility)
		{
			switch (visibility)
			{
				case ScrollBarVisibility.Always:
					return AScrollBarVisibility.Visible;
				case ScrollBarVisibility.Default:
					return AScrollBarVisibility.Auto;
				case ScrollBarVisibility.Never:
					return AScrollBarVisibility.Hidden;
				default:
					return AScrollBarVisibility.Auto;
			}
		}
	}
}