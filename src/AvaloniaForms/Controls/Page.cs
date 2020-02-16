using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Media;
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
        AppBar TopAppBar { get; }
        AppBar BottomAppBar { get; }
        Frame Frame { get; }
        NavigationCacheMode NavigationCacheMode { get; set; }
    }

    public partial class Page : UserControl, IPage, IPageOverrides, IStyleable
    {
        public static readonly StyledProperty<string> CurrentTitleProperty = AvaloniaProperty.Register<ApplicationWindow, string>(nameof(CurrentTitle));
        public static readonly StyledProperty<bool> HasBackButtonProperty = AvaloniaProperty.Register<ApplicationWindow, bool>(nameof(HasBackButton));
        public static readonly StyledProperty<bool> HasBackButtonModalProperty = AvaloniaProperty.Register<ApplicationWindow, bool>(nameof(HasBackButtonModal));
        public static readonly StyledProperty<bool> HasNavigationBarProperty = AvaloniaProperty.Register<ApplicationWindow, bool>(nameof(HasNavigationBar));
        public static readonly StyledProperty<string> BackButtonTitleProperty = AvaloniaProperty.Register<ApplicationWindow, string>(nameof(BackButtonTitle));

        public static readonly StyledProperty<Brush> TitleBarBackgroundColorProperty = AvaloniaProperty.Register<ApplicationWindow, Brush>(nameof(TitleBarBackgroundColor));
        public static readonly StyledProperty<Brush> TitleBarTextColorProperty = AvaloniaProperty.Register<ApplicationWindow, Brush>(nameof(TitleBarTextColor));

        public static readonly StyledProperty<bool> HasTopAppBarProperty = AvaloniaProperty.Register<ApplicationWindow, bool>(nameof(HasTopAppBar));
        public static readonly StyledProperty<bool> HasBottomAppBarProperty = AvaloniaProperty.Register<ApplicationWindow, bool>(nameof(HasBottomAppBar));

        Type IStyleable.StyleKey => typeof(Page);

        public AppBar TopAppBar => topAppBar;
        public AppBar BottomAppBar => bottomAppBar;

        public Frame Frame => throw new NotImplementedException();

        public NavigationCacheMode NavigationCacheMode { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

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

            this.HasTopAppBar = true;
            this.HasNavigationBar = true;
            this.HasBackButton = true;
            this.HasBottomAppBar = false;
            this.HasBackButtonModal = false;
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

        public Brush TitleBarBackgroundColor
        {
            get { return (Brush)GetValue(TitleBarBackgroundColorProperty); }
            private set { SetValue(TitleBarBackgroundColorProperty, value); }
        }

        public Brush TitleBarTextColor
        {
            get { return (Brush)GetValue(TitleBarTextColorProperty); }
            private set { SetValue(TitleBarTextColorProperty, value); }
        }

        public string CurrentTitle
        {
            get { return (string)GetValue(CurrentTitleProperty); }
            private set { SetValue(CurrentTitleProperty, value); }
        }

        public bool HasBackButton
        {
            get { return (bool)GetValue(HasBackButtonProperty); }
            private set { SetValue(HasBackButtonProperty, value); }
        }

        public bool HasBackButtonModal
        {
            get { return (bool)GetValue(HasBackButtonModalProperty); }
            private set { SetValue(HasBackButtonModalProperty, value); }
        }

        public bool HasNavigationBar
        {
            get { return (bool)GetValue(HasNavigationBarProperty); }
            private set { SetValue(HasNavigationBarProperty, value); }
        }

        public string BackButtonTitle
        {
            get { return (string)GetValue(BackButtonTitleProperty); }
            private set { SetValue(BackButtonTitleProperty, value); }
        }

        public bool HasTopAppBar
        {
            get { return (bool)GetValue(HasTopAppBarProperty); }
            private set { SetValue(HasTopAppBarProperty, value); }
        }

        public bool HasBottomAppBar
        {
            get { return (bool)GetValue(HasBottomAppBarProperty); }
            private set { SetValue(HasBottomAppBarProperty, value); }
        }


        CommandBar topAppBar;
        CommandBar bottomAppBar;

        Button previousButton;
        Button previousModalButton;
        Button hamburgerButton;

        protected override void OnTemplateApplied(TemplateAppliedEventArgs e)
        {
            base.OnTemplateApplied(e);

            topAppBar = e.NameScope.Find<CommandBar>("PART_TopAppBar");
            bottomAppBar = e.NameScope.Find<CommandBar>("PART_BottomAppBar");

            previousButton = e.NameScope.Find<Button>("PART_Previous");
            if (previousButton != null)
            {
                previousButton.Click += OnPreviousButtonClick;
            }
            previousModalButton = e.NameScope.Find<Button>("PART_Previous_Modal");
            if (previousButton != null)
            {
                previousModalButton.Click += OnPreviousModalButtonClick;
            }
            hamburgerButton = e.NameScope.Find<Button>("PART_Hamburger");
            if (hamburgerButton != null)
            {
                hamburgerButton.Click += OmHamburgerButtonClick;
            }
        }

        protected virtual void OmHamburgerButtonClick(object sender, RoutedEventArgs e)
        {
            //if (CurrentMasterDetailPage != null)
            //{
            //    CurrentMasterDetailPage.IsPresented = !CurrentMasterDetailPage.IsPresented;
            //}
        }

        protected virtual void OnPreviousModalButtonClick(object sender, RoutedEventArgs e)
        {
            OnBackSystemButtonPressed();
        }

        protected virtual void OnPreviousButtonClick(object sender, RoutedEventArgs e)
        {
            //if (CurrentNavigationPage != null && CurrentNavigationPage.StackDepth > 1)
            //{
            //    CurrentNavigationPage.OnBackButtonPressed();
            //}
        }

        public virtual void OnBackSystemButtonPressed()
        {
            //PopModal();
        }
    }
}
