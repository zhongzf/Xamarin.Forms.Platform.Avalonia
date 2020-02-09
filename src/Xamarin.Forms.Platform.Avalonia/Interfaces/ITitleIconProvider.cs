using Avalonia.Media.Imaging;

namespace Xamarin.Forms.Platform.Avalonia
{
	internal interface ITitleIconProvider
	{
		Bitmap TitleIcon { get; set; }
	}
}