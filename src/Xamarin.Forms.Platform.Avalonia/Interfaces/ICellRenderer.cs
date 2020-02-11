using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.Forms.Platform.Avalonia
{
	public interface ICellRenderer : IRegisterable
	{
		global::Avalonia.Markup.Xaml.Templates.DataTemplate GetTemplate(Cell cell);
	}
}
