using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Styling;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace AvaloniaForms.Controls
{
    public class DynamicContentPage : UserControl
    {
		public static readonly StyledProperty<IContentLoader> ContentLoaderProperty = AvaloniaProperty.Register<DynamicContentPage, IContentLoader>(nameof(ContentLoader), new DefaultContentLoader());

		public static readonly StyledProperty<string> TitleProperty = AvaloniaProperty.Register<DynamicContentPage, string>(nameof(Title));
		public static readonly StyledProperty<string> BackButtonTitleProperty = AvaloniaProperty.Register<DynamicContentPage, string>(nameof(BackButtonTitle));
		public static readonly StyledProperty<bool> HasNavigationBarProperty = AvaloniaProperty.Register<DynamicContentPage, bool>(nameof(HasNavigationBar), true);
		public static readonly StyledProperty<bool> HasBackButtonProperty = AvaloniaProperty.Register<DynamicContentPage, bool>(nameof(HasBackButton), true);

		public static readonly StyledProperty<ObservableCollection<Control>> PrimaryTopBarCommandsProperty = AvaloniaProperty.Register<DynamicContentPage, ObservableCollection<Control>>(nameof(PrimaryTopBarCommands));
		public static readonly StyledProperty<ObservableCollection<Control>> SecondaryTopBarCommandsProperty = AvaloniaProperty.Register<DynamicContentPage, ObservableCollection<Control>>(nameof(SecondaryTopBarCommands));
		public static readonly StyledProperty<ObservableCollection<Control>> PrimaryBottomBarCommandsProperty = AvaloniaProperty.Register<DynamicContentPage, ObservableCollection<Control>>(nameof(PrimaryBottomBarCommands));
		public static readonly StyledProperty<ObservableCollection<Control>> SecondaryBottomBarCommandsProperty = AvaloniaProperty.Register<DynamicContentPage, ObservableCollection<Control>>(nameof(SecondaryBottomBarCommands));
		public static readonly StyledProperty<ObservableCollection<Control>> ContentBottomBarProperty = AvaloniaProperty.Register<DynamicContentPage, ObservableCollection<Control>>(nameof(ContentBottomBar));

		public static readonly StyledProperty<Brush> TitleBarBackgroundColorProperty = AvaloniaProperty.Register<DynamicContentPage, Brush>(nameof(TitleBarBackgroundColor));
		public static readonly StyledProperty<Brush> TitleBarTextColorProperty = AvaloniaProperty.Register<DynamicContentPage, Brush>(nameof(TitleBarTextColor));

		static DynamicContentPage()
		{
		}

		public IContentLoader ContentLoader
		{
			get { return (IContentLoader)GetValue(ContentLoaderProperty); }
			set { SetValue(ContentLoaderProperty, value); }
		}

		public Brush TitleBarBackgroundColor
		{
			get { return (Brush)GetValue(TitleBarBackgroundColorProperty); }
			set { SetValue(TitleBarBackgroundColorProperty, value); }
		}

		public Brush TitleBarTextColor
		{
			get { return (Brush)GetValue(TitleBarTextColorProperty); }
			set { SetValue(TitleBarTextColorProperty, value); }
		}

		public string Title
		{
			get { return (string)GetValue(TitleProperty); }
			set { SetValue(TitleProperty, value); }
		}

		public string BackButtonTitle
		{
			get { return (string)GetValue(BackButtonTitleProperty); }
			set { SetValue(BackButtonTitleProperty, value); }
		}

		public bool HasNavigationBar
		{
			get { return (bool)GetValue(HasNavigationBarProperty); }
			set { SetValue(HasNavigationBarProperty, value); }
		}

		public bool HasBackButton
		{
			get { return (bool)GetValue(HasBackButtonProperty); }
			set { SetValue(HasBackButtonProperty, value); }
		}

		public ObservableCollection<Control> PrimaryTopBarCommands
		{
			get { return (ObservableCollection<Control>)GetValue(PrimaryTopBarCommandsProperty); }
			set { SetValue(PrimaryTopBarCommandsProperty, value); }
		}

		public ObservableCollection<Control> SecondaryTopBarCommands
		{
			get { return (ObservableCollection<Control>)GetValue(SecondaryTopBarCommandsProperty); }
			set { SetValue(SecondaryTopBarCommandsProperty, value); }
		}

		public ObservableCollection<Control> PrimaryBottomBarCommands
		{
			get { return (ObservableCollection<Control>)GetValue(PrimaryBottomBarCommandsProperty); }
			set { SetValue(PrimaryBottomBarCommandsProperty, value); }
		}

		public ObservableCollection<Control> SecondaryBottomBarCommands
		{
			get { return (ObservableCollection<Control>)GetValue(SecondaryBottomBarCommandsProperty); }
			set { SetValue(SecondaryBottomBarCommandsProperty, value); }
		}

		public object ContentBottomBar
		{
			get { return (object)GetValue(ContentBottomBarProperty); }
			set { SetValue(ContentBottomBarProperty, value); }
		}

		public INavigation Navigation
		{
			get
			{
				INavigation nav = this.TryFindParent<NavigationPage>();
				return nav ?? new DefaultNavigation();
			}
		}

		public ApplicationWindow ParentWindow => this.GetParentWindow() as ApplicationWindow;

		public DynamicContentPage()
		{
			this.SetValue(DynamicContentPage.PrimaryTopBarCommandsProperty, new ObservableCollection<Control>());
			this.SetValue(DynamicContentPage.SecondaryTopBarCommandsProperty, new ObservableCollection<Control>());
			this.SetValue(DynamicContentPage.PrimaryBottomBarCommandsProperty, new ObservableCollection<Control>());
			this.SetValue(DynamicContentPage.SecondaryBottomBarCommandsProperty, new ObservableCollection<Control>());

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
			this.PrimaryTopBarCommands.CollectionChanged += Commands_CollectionChanged;
			this.SecondaryTopBarCommands.CollectionChanged += Commands_CollectionChanged;
			this.PrimaryBottomBarCommands.CollectionChanged += Commands_CollectionChanged;
			this.SecondaryBottomBarCommands.CollectionChanged += Commands_CollectionChanged;
			ParentWindow?.SynchronizeToolbarCommands();
			ParentWindow?.SynchronizeAppBar();
		}

		protected virtual void Disappearing()
		{
			this.PrimaryTopBarCommands.CollectionChanged -= Commands_CollectionChanged;
			this.SecondaryTopBarCommands.CollectionChanged -= Commands_CollectionChanged;
			this.PrimaryBottomBarCommands.CollectionChanged -= Commands_CollectionChanged;
			this.SecondaryBottomBarCommands.CollectionChanged -= Commands_CollectionChanged;
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

		protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if (e.Property == TitleProperty || e.Property == HasBackButtonProperty || e.Property == HasNavigationBarProperty || e.Property == TitleBarBackgroundColorProperty || e.Property == TitleBarTextColorProperty)
			{
				ParentWindow?.SynchronizeAppBar();
			}
		}


		private void Commands_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			ParentWindow?.SynchronizeToolbarCommands();
		}

		public virtual string GetTitle()
		{
			return this.Title;
		}

		public virtual bool GetHasNavigationBar()
		{
			return this.HasNavigationBar;
		}

		public virtual Brush GetTitleBarBackgroundColor()
		{
			return this.TitleBarBackgroundColor;
		}

		public virtual Brush GetTitleBarTextColor()
		{
			return this.TitleBarTextColor;
		}

		public virtual IEnumerable<Control> GetPrimaryTopBarCommands()
		{
			return this.PrimaryTopBarCommands;
		}

		public virtual IEnumerable<Control> GetSecondaryTopBarCommands()
		{
			return this.SecondaryTopBarCommands;
		}

		public virtual IEnumerable<Control> GetPrimaryBottomBarCommands()
		{
			return this.PrimaryBottomBarCommands;
		}

		public virtual IEnumerable<Control> GetSecondaryBottomBarCommands()
		{
			return this.SecondaryBottomBarCommands;
		}
	}
}
