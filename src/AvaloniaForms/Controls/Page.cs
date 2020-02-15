using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Styling;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvaloniaForms.Controls
{
    internal interface IPageOverrides
    {
        void OnNavigatedFrom(object e);
        void OnNavigatedTo(object e);
        void OnNavigatingFrom(object e);
    }

    //
    // Summary:
    //     Specifies caching characteristics for a page involved in a navigation.
    public enum NavigationCacheMode
    {
        //
        // Summary:
        //     The page is never cached and a new instance of the page is created on each visit.
        Disabled = 0,
        //
        // Summary:
        //     The page is cached and the cached instance is reused for every visit regardless
        //     of the cache size for the frame.
        Required = 1,
        //
        // Summary:
        //     The page is cached, but the cached instance is discarded when the size of the
        //     cache for the frame is exceeded.
        Enabled = 2
    }

    internal interface IPage
    {
        AppBar BottomAppBar { get; set; }
        Frame Frame { get; }
        NavigationCacheMode NavigationCacheMode { get; set; }
        AppBar TopAppBar { get; set; }
    }

    public partial class Page : UserControl, IPage, IPageOverrides, IStyleable
    {
        Type IStyleable.StyleKey => typeof(UserControl);

        public AppBar BottomAppBar { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Frame Frame => throw new NotImplementedException();

        public NavigationCacheMode NavigationCacheMode { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public AppBar TopAppBar { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void OnNavigatedFrom(object e)
        {
            throw new NotImplementedException();
        }

        public void OnNavigatedTo(object e)
        {
            throw new NotImplementedException();
        }

        public void OnNavigatingFrom(object e)
        {
            throw new NotImplementedException();
        }

        public Page()
        {
            LayoutUpdated += OnLayoutUpdated;
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
