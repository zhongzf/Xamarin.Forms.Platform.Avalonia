using Avalonia;
using System.Windows;

namespace Xamarin.Forms.Platform.Avalonia.Controls
{
	public class FormsAppBarButton : global::Avalonia.Controls.Button
	{
		public static readonly StyledProperty<FormsElementIcon> IconProperty = AvaloniaProperty.Register<FormsAppBarButton, FormsElementIcon>(nameof(Icon));
		public static readonly StyledProperty<string> LabelProperty = AvaloniaProperty.Register<FormsAppBarButton, string>(nameof(Label));

		public FormsElementIcon Icon
		{
			get { return (FormsElementIcon)GetValue(IconProperty); }
			set { SetValue(IconProperty, value); }
		}

		public string Label
		{
			get { return (string)GetValue(LabelProperty); }
			set { SetValue(LabelProperty, value); }
		}

		public FormsAppBarButton()
		{
			//this.DefaultStyleKey = typeof(FormsAppBarButton);
		}
	}
}
