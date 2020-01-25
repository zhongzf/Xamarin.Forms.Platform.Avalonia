using Avalonia;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Text;

namespace Xamarin.Forms.Platform.Avalonia.Controls
{
	public class FormsTabbedPage : FormsMultiPage
	{
		public static readonly StyledProperty<Brush> BarBackgroundColorProperty = AvaloniaProperty.Register<FormsTabbedPage, Brush>(nameof(BarBackgroundColor));
		public static readonly StyledProperty<Brush> BarTextColorProperty = AvaloniaProperty.Register<FormsTabbedPage, Brush>(nameof(BarTextColor));

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

		public FormsTabbedPage()
		{
		}
	}
}
