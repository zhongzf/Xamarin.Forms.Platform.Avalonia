using Avalonia.Media;

namespace Xamarin.Forms.Platform.Avalonia
{
	internal interface ITitleProvider
	{
		Brush BarBackgroundBrush { set; }

		Brush BarForegroundBrush { set; }

		bool ShowTitle { get; set; }

		string Title { get; set; }
	}
}