using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Styling;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Avalonia.Controls.Presenters;

namespace AvaloniaForms.Controls
{
    public class ApplicationWindow : Window, IStyleable
    {
        public static readonly StyledProperty<object> StartupPageProperty = AvaloniaProperty.Register<ApplicationWindow, object>(nameof(StartupPage));

        public static readonly StyledProperty<object> CurrentModalPageProperty = AvaloniaProperty.Register<ApplicationWindow, object>(nameof(CurrentModalPage));
        public static readonly StyledProperty<IContentLoader> ContentLoaderProperty = AvaloniaProperty.Register<ApplicationWindow, IContentLoader>(nameof(ContentLoader));
        public static readonly StyledProperty<string> CurrentTitleProperty = AvaloniaProperty.Register<ApplicationWindow, string>(nameof(CurrentTitle));
        public static readonly StyledProperty<bool> HasBackButtonProperty = AvaloniaProperty.Register<ApplicationWindow, bool>(nameof(HasBackButton));
        public static readonly StyledProperty<bool> HasBackButtonModalProperty = AvaloniaProperty.Register<ApplicationWindow, bool>(nameof(HasBackButtonModal));
        public static readonly StyledProperty<bool> HasNavigationBarProperty = AvaloniaProperty.Register<ApplicationWindow, bool>(nameof(HasNavigationBar));
        public static readonly StyledProperty<string> BackButtonTitleProperty = AvaloniaProperty.Register<ApplicationWindow, string>(nameof(BackButtonTitle));

        public static readonly StyledProperty<Brush> TitleBarBackgroundColorProperty = AvaloniaProperty.Register<ApplicationWindow, Brush>(nameof(TitleBarBackgroundColor));
        public static readonly StyledProperty<Brush> TitleBarTextColorProperty = AvaloniaProperty.Register<ApplicationWindow, Brush>(nameof(TitleBarTextColor));

        public static readonly StyledProperty<bool> HasContentDialogProperty = AvaloniaProperty.Register<ApplicationWindow, bool>(nameof(HasContentDialog));
        public static readonly StyledProperty<bool> HasModalPageProperty = AvaloniaProperty.Register<ApplicationWindow, bool>(nameof(HasModalPage));

        public static readonly StyledProperty<bool> HasTopAppBarProperty = AvaloniaProperty.Register<ApplicationWindow, bool>(nameof(HasTopAppBar));
        public static readonly StyledProperty<bool> HasBottomAppBarProperty = AvaloniaProperty.Register<ApplicationWindow, bool>(nameof(HasBottomAppBar));

        public static readonly StyledProperty<NavigationPage> CurrentNavigationPageProperty = AvaloniaProperty.Register<ApplicationWindow, NavigationPage>(nameof(CurrentNavigationPage));
        public static readonly StyledProperty<MasterDetailPage> CurrentMasterDetailPageProperty = AvaloniaProperty.Register<ApplicationWindow, MasterDetailPage>(nameof(CurrentMasterDetailPage));
        public static readonly StyledProperty<ContentDialog> CurrentContentDialogProperty = AvaloniaProperty.Register<ApplicationWindow, ContentDialog>(nameof(CurrentContentDialog));

        static ApplicationWindow()
        {
        }

        Type IStyleable.StyleKey => typeof(ApplicationWindow);

        CommandBar topAppBar;
        CommandBar bottomAppBar;

        Button previousButton;
        Button previousModalButton;
        Button hamburgerButton;

        public ContentPresenter ContentDialogContentPresenter { get; private set; }

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

        public object StartupPage
        {
            get { return (object)GetValue(StartupPageProperty); }
            set { SetValue(StartupPageProperty, value); }
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

        public object CurrentModalPage
        {
            get { return (object)GetValue(CurrentModalPageProperty); }
            private set { SetValue(CurrentModalPageProperty, value); }
        }

        public IContentLoader ContentLoader
        {
            get { return (IContentLoader)GetValue(ContentLoaderProperty); }
            set { SetValue(ContentLoaderProperty, value); }
        }

        public bool HasContentDialog
        {
            get { return (bool)GetValue(HasContentDialogProperty); }
            private set { SetValue(HasContentDialogProperty, value); }
        }

        public bool HasModalPage
        {
            get { return (bool)GetValue(HasModalPageProperty); }
            private set { SetValue(HasModalPageProperty, value); }
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


        public ContentDialog CurrentContentDialog
        {
            get { return GetValue(CurrentContentDialogProperty); }
            set { SetValue(CurrentContentDialogProperty, value); }
        }

        public NavigationPage CurrentNavigationPage
        {
            get { return GetValue(CurrentNavigationPageProperty); }
            private set { SetValue(CurrentNavigationPageProperty, value); }
        }

        public MasterDetailPage CurrentMasterDetailPage
        {
            get { return GetValue(CurrentMasterDetailPageProperty); }
            private set { SetValue(CurrentMasterDetailPageProperty, value); }
        }


        public ApplicationWindow()
        {
            this.Opened += (sender, e) => Appearing();
            this.Closed += (sender, e) => Disappearing();
        }

        public void SetStartupPage(object page)
        {
            this.StartupPage = page;
        }

        #region Loaded & Unloaded
        public event EventHandler<RoutedEventArgs> Loaded;
        public event EventHandler<RoutedEventArgs> Unloaded;

        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            OnLoaded(new RoutedEventArgs());
            //Appearing();
        }

        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
            OnUnloaded(new RoutedEventArgs());
            //Disappearing();
        }

        protected virtual void OnLoaded(RoutedEventArgs e) { Loaded?.Invoke(this, e); }
        protected virtual void OnUnloaded(RoutedEventArgs e) { Unloaded?.Invoke(this, e); }
        #endregion

        protected virtual void Appearing()
        {
        }

        protected virtual void Disappearing()
        {
        }

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

            ContentDialogContentPresenter = e.NameScope.Find<ContentPresenter>("PART_ContentDialog_ContentPresenter");
        }


        protected virtual void OmHamburgerButtonClick(object sender, RoutedEventArgs e)
        {
            if (CurrentMasterDetailPage != null)
            {
                CurrentMasterDetailPage.IsPresented = !CurrentMasterDetailPage.IsPresented;
            }
        }

        protected virtual void OnPreviousModalButtonClick(object sender, RoutedEventArgs e)
        {
            OnBackSystemButtonPressed();
        }

        protected virtual void OnPreviousButtonClick(object sender, RoutedEventArgs e)
        {
            if (CurrentNavigationPage != null && CurrentNavigationPage.StackDepth > 1)
            {
                CurrentNavigationPage.OnBackButtonPressed();
            }
        }

        public virtual void OnBackSystemButtonPressed()
        {
            PopModal();
        }

        public void ShowContentDialog(ContentDialog contentDialog)
        {
            this.CurrentContentDialog = contentDialog;
            this.HasContentDialog = true;
            if (ContentDialogContentPresenter.Content == null)
            {
                ContentDialogContentPresenter.Content = contentDialog;
            }
        }

        public void HideContentDialog()
        {
            this.CurrentContentDialog = null;
            if (ContentDialogContentPresenter.Content != null)
            {
                ContentDialogContentPresenter.Content = null;
            }
            this.HasContentDialog = false;
        }

        public ObservableCollection<object> InternalChildren { get; } = new ObservableCollection<object>();

        public void PushModal(object page)
        {
            PushModal(page, true);
        }

        public void PushModal(object page, bool animated)
        {
            InternalChildren.Add(page);
            this.CurrentModalPage = InternalChildren.Last();
            this.HasModalPage = true;
            this.HasBackButtonModal = true;
        }

        public object PopModal()
        {
            return PopModal(true);
        }

        public object PopModal(bool animated)
        {
            if (InternalChildren.Count < 1)
            {
                return null;
            }

            var modal = InternalChildren.Last();

            if (InternalChildren.Remove(modal))
            {
                CurrentModalPage = InternalChildren.LastOrDefault();
            }
            this.HasBackButtonModal = InternalChildren.Count >= 1;
            this.HasModalPage = InternalChildren.Count >= 1;

            return modal;
        }

        public void SynchronizeAppBar()
        {
            IEnumerable<DynamicContentPage> childrens = this.FindVisualChildren<DynamicContentPage>();

            CurrentTitle = childrens.FirstOrDefault()?.GetTitle();
            HasNavigationBar = childrens.FirstOrDefault()?.GetHasNavigationBar() ?? false;
            CurrentNavigationPage = childrens.OfType<NavigationPage>()?.FirstOrDefault();
            CurrentMasterDetailPage = childrens.OfType<MasterDetailPage>()?.FirstOrDefault();
            var page = childrens.FirstOrDefault();
            if (page != null)
            {
                TitleBarBackgroundColor = page.GetTitleBarBackgroundColor();
                TitleBarTextColor = page.GetTitleBarTextColor();
            }
            else
            {
                ClearValue(TitleBarBackgroundColorProperty);
                ClearValue(TitleBarTextColorProperty);
            }

            if (hamburgerButton != null)
            {
                hamburgerButton.IsVisible = CurrentMasterDetailPage != null;
            }

            if (CurrentNavigationPage != null)
            {
                HasBackButton = CurrentNavigationPage.GetHasBackButton();
                BackButtonTitle = CurrentNavigationPage.GetBackButtonTitle();

            }
            else
            {
                HasBackButton = false;
                BackButtonTitle = "";
            }
        }

        public void SynchronizeToolbarCommands()
        {
            IEnumerable<DynamicContentPage> childrens = this.FindVisualChildren<DynamicContentPage>();

            var page = childrens.FirstOrDefault();
            if (page == null) return;

            if (topAppBar != null)
            {
                topAppBar.PrimaryCommands = page.GetPrimaryTopBarCommands();
                topAppBar.SecondaryCommands = page.GetSecondaryTopBarCommands();
                topAppBar.Reset();

                // TODO:
                HasTopAppBar = false;
            }

            if (bottomAppBar != null)
            {
                bottomAppBar.PrimaryCommands = page.GetPrimaryBottomBarCommands();
                bottomAppBar.SecondaryCommands = page.GetSecondaryBottomBarCommands();
                bottomAppBar.Content = childrens.LastOrDefault(x => x.ContentBottomBar != null)?.ContentBottomBar;
                bottomAppBar.Reset();

                // TODO:
                HasBottomAppBar = bottomAppBar.PrimaryCommands.Count() > 0 || bottomAppBar.SecondaryCommands.Count() > 0 || bottomAppBar.Content != null;
            }
        }
    }
}
