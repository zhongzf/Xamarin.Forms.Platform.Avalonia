using Avalonia;
using Avalonia.Media;
using Avalonia.Styling;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Platform.Avalonia.Extensions;
using ACheckBox = Avalonia.Controls.CheckBox;

namespace Xamarin.Forms.Platform.Avalonia.Controls
{
	public class FormsCheckBox : ACheckBox, IStyleable
	{
		public static readonly StyledProperty<Brush> TintBrushProperty = AvaloniaProperty.Register<FormsPage, Brush>(nameof(TintBrush));

		static FormsCheckBox()
		{
			TintBrushProperty.Changed.AddClassHandler<FormsCheckBox>((x, e) => x.OnTintBrushPropertyChanged(e));
		}

		protected void OnTintBrushPropertyChanged(AvaloniaPropertyChangedEventArgs e)
		{
			var checkBox = this;

			if (e.NewValue is SolidColorBrush solidBrush && solidBrush.Color.A == 0)
			{
				checkBox.BorderBrush = Color.Black.ToBrush();
			}
			else if (e.NewValue is SolidColorBrush b)
			{
				checkBox.BorderBrush = b;
			}
		}

		Type IStyleable.StyleKey => typeof(ACheckBox);

		public FormsCheckBox()
		{
			BorderBrush = Color.Black.ToBrush();
		}

		public Brush TintBrush
		{
			get { return (Brush)GetValue(TintBrushProperty); }
			set { SetValue(TintBrushProperty, value); }
		}
	}
}
