using Avalonia;
using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvaloniaForms.Controls
{
    public class AppBarButton : Button
    {
		public static readonly StyledProperty<ElementIcon> IconProperty = AvaloniaProperty.Register<AppBarButton, ElementIcon>(nameof(Icon));
		public static readonly StyledProperty<string> LabelProperty = AvaloniaProperty.Register<AppBarButton, string>(nameof(Label));

		public ElementIcon Icon
		{
			get { return GetValue(IconProperty); }
			set { SetValue(IconProperty, value); }
		}

		public string Label
		{
			get { return (string)GetValue(LabelProperty); }
			set { SetValue(LabelProperty, value); }
		}
	}
}
