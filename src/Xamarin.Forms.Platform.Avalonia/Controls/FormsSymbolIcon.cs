using Avalonia;
using System.Windows;
using Xamarin.Forms.Platform.Avalonia.Enums;

namespace Xamarin.Forms.Platform.Avalonia.Controls
{
	public class FormsSymbolIcon : FormsElementIcon
	{
		public static readonly StyledProperty<Symbol> SymbolProperty = AvaloniaProperty.Register<FormsSymbolIcon, Symbol>(nameof(Symbol));

		public Symbol Symbol
		{
			get { return (Symbol)GetValue(SymbolProperty); }
			set { SetValue(SymbolProperty, value); }
		}

		public FormsSymbolIcon()
		{
		}
	}
}
