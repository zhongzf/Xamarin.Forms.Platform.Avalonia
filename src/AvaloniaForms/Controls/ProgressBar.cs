using Avalonia;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvaloniaForms.Controls
{
    public partial class ProgressBar : Avalonia.Controls.ProgressBar
    {
		public static readonly StyledProperty<double> ElementOpacityProperty = AvaloniaProperty.Register<ProgressBar, double>(nameof(ElementOpacity));

		public double ElementOpacity
		{
			get { return (double)GetValue(ElementOpacityProperty); }
			set { SetValue(ElementOpacityProperty, value); }
		}
	}
}
