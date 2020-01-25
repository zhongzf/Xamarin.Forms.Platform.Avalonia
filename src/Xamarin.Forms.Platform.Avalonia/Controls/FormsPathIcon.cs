using Avalonia;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Text;

namespace Xamarin.Forms.Platform.Avalonia.Controls
{
	public class FormsPathIcon : FormsElementIcon
	{
		public static readonly StyledProperty<Geometry> DataProperty = AvaloniaProperty.Register<FormsPathIcon, Geometry>(nameof(Data));

		public Geometry Data
		{
			get { return (Geometry)GetValue(DataProperty); }
			set { SetValue(DataProperty, value); }
		}

		public FormsPathIcon()
		{
			//this.DefaultStyleKey = typeof(FormsPathIcon);
		}
	}
}
