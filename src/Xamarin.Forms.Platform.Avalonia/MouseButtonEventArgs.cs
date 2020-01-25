using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Text;

namespace Xamarin.Forms.Platform.Avalonia
{
    public class MouseButtonEventArgs
    {
        private readonly global::Avalonia.Input.PointerReleasedEventArgs _e;

        public MouseButtonEventArgs(global::Avalonia.Input.PointerReleasedEventArgs e)
        {
            _e = e;
        }

        public bool Handled
        {
            set
            {
                _e.Handled = value;
            }
        }

        public int ClickCount
        {
            get
            {
                // TODO:
                return 1;
            }
        }

        public global::Avalonia.Point GetPosition(Control fe)
        {
            return _e.GetPosition(fe);
        }
    }
}
