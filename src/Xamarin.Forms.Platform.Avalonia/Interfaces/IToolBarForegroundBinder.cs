using AvaloniaForms.Controls;

namespace Xamarin.Forms.Platform.Avalonia
{
	internal interface IToolBarForegroundBinder
	{
		void BindForegroundColor(CommandBar appBar);
		void BindForegroundColor(AppBarButton button);
	}
}