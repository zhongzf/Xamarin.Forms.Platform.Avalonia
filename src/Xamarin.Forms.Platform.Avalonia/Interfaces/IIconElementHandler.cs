using AvaloniaForms.Controls;
using System.Threading;
using System.Threading.Tasks;

namespace Xamarin.Forms.Platform.Avalonia
{
	public interface IIconElementHandler : IRegisterable
	{
		Task<IconElement> LoadIconElementAsync(ImageSource imagesource, CancellationToken cancellationToken = default(CancellationToken));
	}
}
