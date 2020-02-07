using Avalonia;
using Avalonia.Media;
using Avalonia.Styling;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvaloniaForms.Controls
{
    public class TabbedPage : MultiContentPage, IStyleable
	{
		public static readonly StyledProperty<Brush> BarBackgroundColorProperty = AvaloniaProperty.Register<TabbedPage, Brush>(nameof(BarBackgroundColor));
		public static readonly StyledProperty<Brush> BarTextColorProperty = AvaloniaProperty.Register<TabbedPage, Brush>(nameof(BarTextColor));

		static TabbedPage()
		{
		}

		Type IStyleable.StyleKey => typeof(TabbedPage);

		public Brush BarBackgroundColor
		{
			get { return (Brush)GetValue(BarBackgroundColorProperty); }
			set { SetValue(BarBackgroundColorProperty, value); }
		}

		public Brush BarTextColor
		{
			get { return (Brush)GetValue(BarTextColorProperty); }
			set { SetValue(BarTextColorProperty, value); }
		}
	}
}
