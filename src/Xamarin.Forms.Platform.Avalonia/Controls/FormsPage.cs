using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms.Platform.Avalonia.Helpers;
using Xamarin.Forms.Platform.Avalonia.Interfaces;

namespace Xamarin.Forms.Platform.Avalonia.Controls
{
    public class FormsPage : UserControl
    {
		public static readonly StyledProperty<string> TitleProperty = AvaloniaProperty.Register<FormsPage, string>(nameof(Title));
		public static readonly StyledProperty<string> BackButtonTitleProperty = AvaloniaProperty.Register<FormsPage, string>(nameof(BackButtonTitle));
		public static readonly StyledProperty<bool> HasNavigationBarProperty = AvaloniaProperty.Register<FormsPage, bool>(nameof(HasNavigationBar), true);
		public static readonly StyledProperty<bool> HasBackButtonProperty = AvaloniaProperty.Register<FormsPage, bool>(nameof(HasBackButton), true);

		public static readonly StyledProperty<ObservableCollection<Control>> PrimaryTopBarCommandsProperty = AvaloniaProperty.Register<FormsPage, ObservableCollection<Control>>(nameof(PrimaryTopBarCommands));
		public static readonly StyledProperty<ObservableCollection<Control>> SecondaryTopBarCommandsProperty = AvaloniaProperty.Register<FormsPage, ObservableCollection<Control>>(nameof(SecondaryTopBarCommands));
		public static readonly StyledProperty<ObservableCollection<Control>> PrimaryBottomBarCommandsProperty = AvaloniaProperty.Register<FormsPage, ObservableCollection<Control>>(nameof(PrimaryBottomBarCommands));
		public static readonly StyledProperty<ObservableCollection<Control>> SecondaryBottomBarCommandsProperty = AvaloniaProperty.Register<FormsPage, ObservableCollection<Control>>(nameof(SecondaryBottomBarCommands));
		public static readonly StyledProperty<ObservableCollection<Control>> ContentBottomBarProperty = AvaloniaProperty.Register<FormsPage, ObservableCollection<Control>>(nameof(ContentBottomBar));

		public static readonly StyledProperty<Brush> TitleBarBackgroundColorProperty = AvaloniaProperty.Register<FormsPage, Brush>(nameof(TitleBarBackgroundColor));
		public static readonly StyledProperty<Brush> TitleBarTextColorProperty = AvaloniaProperty.Register<FormsPage, Brush>(nameof(TitleBarTextColor));

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

		public IFormsNavigation Navigation
		{
			get
			{
				IFormsNavigation nav = this.TryFindParent<FormsNavigationPage>();
				return nav ?? new DefaultNavigation();
			}
		}

		public FormsWindow ParentWindow
		{
			get
			{
				if (global::Avalonia.Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
				{
					if (desktop.MainWindow is FormsWindow parentWindow)
					{
						return parentWindow;
					}
				}
				return null;
			}
		}

		public FormsPage()
		{
			this.SetValue(FormsPage.PrimaryTopBarCommandsProperty, new ObservableCollection<Control>());
			this.SetValue(FormsPage.SecondaryTopBarCommandsProperty, new ObservableCollection<Control>());
			this.SetValue(FormsPage.PrimaryBottomBarCommandsProperty, new ObservableCollection<Control>());
			this.SetValue(FormsPage.SecondaryBottomBarCommandsProperty, new ObservableCollection<Control>());

			//this.Loaded += (sender, e) => Appearing();
			AttachedToVisualTree += (sender, e) => Appearing();
			//this.Unloaded += (sender, e) => Disappearing();
			DetachedFromVisualTree += (sender, e) => Disappearing();
		}

		protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if(e.Property == TitleProperty || e.Property == HasBackButtonProperty ||e.Property == HasNavigationBarProperty || e.Property == TitleBarBackgroundColorProperty || e.Property == TitleBarTextColorProperty)
			{
				ParentWindow?.SynchronizeAppBar();
			}
		}

		private void Commands_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			ParentWindow?.SynchronizeToolbarCommands();
		}

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
