using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Styling;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvaloniaForms.Controls
{
    public class AppBar : ContentControl
    {
        public event EventHandler<object> Closing;
        public event EventHandler<object> Closed;
        public event EventHandler<object> Opened;
        public event EventHandler<object> Opening;

        public AppBar()
        {
			LayoutUpdated += OnLayoutUpdated;
		}

		#region Loaded & Unloaded
		public event EventHandler<EventArgs> Loaded;
		public event EventHandler<EventArgs> Unloaded;

		protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
		{
			OnLoaded(e);
			Appearing();
		}

		protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
		{
			OnUnloaded(e);
			Disappearing();
		}

		protected virtual void OnLoaded(EventArgs e) { Loaded?.Invoke(this, e); }
		protected virtual void OnUnloaded(EventArgs e) { Unloaded?.Invoke(this, e); }
		#endregion

		#region Appearing & Disappearing
		protected virtual void Appearing()
		{
		}

		protected virtual void Disappearing()
		{
		}
        #endregion

        #region LayoutUpdated & SizeChanged
        public event EventHandler<EventArgs> SizeChanged;
		protected virtual void OnSizeChanged(EventArgs e) { SizeChanged?.Invoke(this, e); }

		protected virtual void OnLayoutUpdated(object sender, EventArgs e)
		{
			OnSizeChanged(e);
		}
		#endregion

		protected virtual void OnClosed(object e) { Closed?.Invoke(this, e); }
        protected virtual void OnClosing(object e) { Closing?.Invoke(this, e); }
        protected virtual void OnOpened(object e) { Opened?.Invoke(this, e); }
        protected virtual void OnOpening(object e) { Opening?.Invoke(this, e); }
    }
}
