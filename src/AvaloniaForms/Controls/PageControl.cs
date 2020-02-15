using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Styling;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvaloniaForms.Controls
{
    public class PageControl : ContentControl, IStyleable
    {
        Type IStyleable.StyleKey => typeof(PageControl);

        ContentPresenter _presenter;

        public double ContentHeight
        {
            get { return _presenter != null ? _presenter.Height : 0; }
        }

        public double ContentWidth
        {
            get { return _presenter != null ? _presenter.Width : 0; }
        }


        public PageControl()
        {
            LayoutUpdated += OnLayoutUpdated;
        }

        protected override void OnTemplateApplied(TemplateAppliedEventArgs e)
        {
            base.OnTemplateApplied(e);

            _presenter = e.NameScope.Find<ContentPresenter>("presenter");
        }

        #region Loaded & Unloaded
        public event EventHandler<RoutedEventArgs> Loaded;
        public event EventHandler<RoutedEventArgs> Unloaded;

        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            OnLoaded(new RoutedEventArgs());
            Appearing();
        }

        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
            OnUnloaded(new RoutedEventArgs());
            Disappearing();
        }

        protected virtual void OnLoaded(RoutedEventArgs e) { Loaded?.Invoke(this, e); }
        protected virtual void OnUnloaded(RoutedEventArgs e) { Unloaded?.Invoke(this, e); }
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
    }
}
