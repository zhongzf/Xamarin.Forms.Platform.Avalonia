using Avalonia;
using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvaloniaForms.Controls
{
    public partial class Slider : Avalonia.Controls.Slider
    {
		public static readonly StyledProperty<Bitmap> ElementOpacityProperty = AvaloniaProperty.Register<Slider, Bitmap>(nameof(ElementOpacity));

		public Bitmap ElementOpacity
		{
			get { return GetValue(ElementOpacityProperty); }
			set { SetValue(ElementOpacityProperty, value); }
		}
	}
}
