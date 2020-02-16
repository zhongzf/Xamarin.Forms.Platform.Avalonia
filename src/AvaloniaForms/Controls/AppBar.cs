using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Styling;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvaloniaForms.Controls
{
    internal interface IAppBar
    {
        bool IsOpen { get; set; }
        bool IsSticky { get; set; }

        event EventHandler<object> Closed;
        event EventHandler<object> Opened;
    }

    public class AppBar : ContentControl, IAppBar
    {
        public bool IsOpen { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool IsSticky { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public event EventHandler<object> Closing;
        public event EventHandler<object> Closed;
        public event EventHandler<object> Opened;
        public event EventHandler<object> Opening;

		protected virtual void OnClosed(object e) { Closed?.Invoke(this, e); }
        protected virtual void OnClosing(object e) { Closing?.Invoke(this, e); }
        protected virtual void OnOpened(object e) { Opened?.Invoke(this, e); }
        protected virtual void OnOpening(object e) { Opening?.Invoke(this, e); }
    }
}
