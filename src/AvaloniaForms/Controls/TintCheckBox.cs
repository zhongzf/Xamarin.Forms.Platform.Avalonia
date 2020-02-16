using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Styling;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvaloniaForms.Controls
{
    public class TintCheckBox : CheckBox, IStyleable
    {
        public static readonly StyledProperty<Brush> TintBrushProperty = AvaloniaProperty.Register<TintCheckBox, Brush>(nameof(TintBrush));

        static TintCheckBox()
        {
            TintBrushProperty.Changed.AddClassHandler<TintCheckBox>((x, e) => x.OnTintBrushPropertyChanged(e));
        }

        Type IStyleable.StyleKey => typeof(CheckBox);

        public Brush TintBrush
        {
            get { return (Brush)GetValue(TintBrushProperty); }
            set { SetValue(TintBrushProperty, value); }
        }

        public TintCheckBox()
        {
            BorderBrush = new SolidColorBrush(Colors.Black);
        }

        protected void OnTintBrushPropertyChanged(AvaloniaPropertyChangedEventArgs e)
        {
            var checkBox = this;

            if (e.NewValue is SolidColorBrush solidBrush && solidBrush.Color.A == 0)
            {
                checkBox.BorderBrush = new SolidColorBrush(Colors.Black);
            }
            else if (e.NewValue is SolidColorBrush b)
            {
                checkBox.BorderBrush = b;
            }
        }
    }
}
