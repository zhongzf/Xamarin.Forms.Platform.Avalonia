using Avalonia;
using Avalonia.Styling;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvaloniaForms.Controls
{
	public partial class ProgressBar : Avalonia.Controls.ProgressBar, IStyleable
	{
		public static readonly StyledProperty<double> ElementOpacityProperty = AvaloniaProperty.Register<ProgressBar, double>(nameof(ElementOpacity));

		public double ElementOpacity
		{
			get { return (double)GetValue(ElementOpacityProperty); }
			set { SetValue(ElementOpacityProperty, value); }
		}

		Type IStyleable.StyleKey => typeof(Avalonia.Controls.ProgressBar);
	}
}
