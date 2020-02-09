using AvaloniaForms.Controls;
using System.Threading.Tasks;

namespace Xamarin.Forms.Platform.Avalonia
{
	internal interface IToolbarProvider
	{
		Task<CommandBar> GetCommandBarAsync();
	}
}