using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms.Platform.Avalonia.Extensions;
using Xamarin.Forms.Platform.Avalonia.Helpers;
using Xamarin.Forms.Platform.Avalonia.Interfaces;

namespace Xamarin.Forms.Platform.Avalonia.Controls
{
	public class FormsWindow : Window
	{
		FormsAppBar topAppBar;
		FormsAppBar bottomAppBar;
		global::Avalonia.Controls.Button previousButton;
		global::Avalonia.Controls.Button previousModalButton;
		global::Avalonia.Controls.Button hamburgerButton;

		public static readonly StyledProperty<object> StartupPageProperty = AvaloniaProperty.Register<FormsWindow, object>(nameof(StartupPage));
		public static readonly StyledProperty<object> CurrentModalPageProperty = AvaloniaProperty.Register<FormsWindow, object>(nameof(CurrentModalPage));
		public static readonly StyledProperty<IContentLoader> ContentLoaderProperty = AvaloniaProperty.Register<FormsWindow, IContentLoader>(nameof(ContentLoader));
		public static readonly StyledProperty<string> CurrentTitleProperty = AvaloniaProperty.Register<FormsWindow, string>(nameof(CurrentTitle));
		public static readonly StyledProperty<bool> HasBackButtonProperty = AvaloniaProperty.Register<FormsWindow, bool>(nameof(HasBackButton));
		public static readonly StyledProperty<bool> HasBackButtonModalProperty = AvaloniaProperty.Register<FormsWindow, bool>(nameof(HasBackButtonModal));
		public static readonly StyledProperty<bool> HasNavigationBarProperty = AvaloniaProperty.Register<FormsWindow, bool>(nameof(HasNavigationBar));
		public static readonly StyledProperty<string> BackButtonTitleProperty = AvaloniaProperty.Register<FormsWindow, string>(nameof(BackButtonTitle));
		public static readonly StyledProperty<FormsNavigationPage> CurrentNavigationPageProperty = AvaloniaProperty.Register<FormsWindow, FormsNavigationPage>(nameof(CurrentNavigationPage));
		public static readonly StyledProperty<FormsMasterDetailPage> CurrentMasterDetailPageProperty = AvaloniaProperty.Register<FormsWindow, FormsMasterDetailPage>(nameof(CurrentMasterDetailPage));
		public static readonly StyledProperty<FormsContentDialog> CurrentContentDialogProperty = AvaloniaProperty.Register<FormsWindow, FormsContentDialog>(nameof(CurrentContentDialog));

		public static readonly StyledProperty<Brush> TitleBarBackgroundColorProperty = AvaloniaProperty.Register<FormsWindow, Brush>(nameof(TitleBarBackgroundColor));
		public static readonly StyledProperty<Brush> TitleBarTextColorProperty = AvaloniaProperty.Register<FormsWindow, Brush>(nameof(TitleBarTextColor));

		public static readonly StyledProperty<bool> HasContentDialogProperty = AvaloniaProperty.Register<FormsWindow, bool>(nameof(HasContentDialog));
		public static readonly StyledProperty<bool> HasModalPageProperty = AvaloniaProperty.Register<FormsWindow, bool>(nameof(HasModalPage));

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

		public FormsContentDialog CurrentContentDialog
		{
			get { return (FormsContentDialog)GetValue(CurrentContentDialogProperty); }
			set { SetValue(CurrentContentDialogProperty, value); }
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

		public FormsNavigationPage CurrentNavigationPage
		{
			get { return (FormsNavigationPage)GetValue(CurrentNavigationPageProperty); }
			private set { SetValue(CurrentNavigationPageProperty, value); }
		}

		public FormsMasterDetailPage CurrentMasterDetailPage
		{
			get { return (FormsMasterDetailPage)GetValue(CurrentMasterDetailPageProperty); }
			private set { SetValue(CurrentMasterDetailPageProperty, value); }
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

		public FormsWindow()
		{
			this.Opened += (sender, e) => Appearing();
			this.Closed += (sender, e) => Disappearing();
		}

		protected virtual void Appearing()
		{

		}

		protected virtual void Disappearing()
		{

		}

		protected override void OnTemplateApplied(TemplateAppliedEventArgs e)
		{
			base.OnTemplateApplied(e);

			topAppBar = this.Find<FormsAppBar>("PART_TopAppBar", e);
			bottomAppBar = this.Find<FormsAppBar>("PART_BottomAppBar", e);

			previousButton = this.Find<global::Avalonia.Controls.Button>("PART_Previous", e);
			if (previousButton != null)
			{
				previousButton.Click += PreviousButton_Click;
			}
			previousModalButton = this.Find<global::Avalonia.Controls.Button>("PART_Previous_Modal", e);
			if (previousButton != null)
			{
				previousModalButton.Click += PreviousModalButton_Click;
			}
			hamburgerButton = this.Find<global::Avalonia.Controls.Button>("PART_Hamburger", e);
			if (hamburgerButton != null)
			{
				hamburgerButton.Click += HamburgerButton_Click;
			}
		}

		private void PreviousModalButton_Click(object sender, RoutedEventArgs e)
		{
			OnBackSystemButtonPressed();
		}

		private void HamburgerButton_Click(object sender, RoutedEventArgs e)
		{
			if (CurrentMasterDetailPage != null)
			{
				CurrentMasterDetailPage.IsPresented = !CurrentMasterDetailPage.IsPresented;
			}
		}

		private void PreviousButton_Click(object sender, RoutedEventArgs e)
		{
			if (CurrentNavigationPage != null && CurrentNavigationPage.StackDepth > 1)
			{
				CurrentNavigationPage.OnBackButtonPressed();
			}
		}

		private static void OnContentLoaderChanged(AvaloniaObject o, AvaloniaPropertyChangedEventArgs e)
		{
			if (e.NewValue == null)
				throw new ArgumentNullException("ContentLoader");
		}

		public void SynchronizeAppBar()
		{
			IEnumerable<FormsPage> childrens = this.FindVisualChildren<FormsPage>();

			CurrentTitle = childrens.FirstOrDefault()?.GetTitle();
			HasNavigationBar = childrens.FirstOrDefault()?.GetHasNavigationBar() ?? false;
			CurrentNavigationPage = childrens.OfType<FormsNavigationPage>()?.FirstOrDefault();
			CurrentMasterDetailPage = childrens.OfType<FormsMasterDetailPage>()?.FirstOrDefault();
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
			IEnumerable<FormsPage> childrens = this.FindVisualChildren<FormsPage>();

			var page = childrens.FirstOrDefault();
			if (page == null) return;

			if (topAppBar != null)
			{
				topAppBar.PrimaryCommands = page.GetPrimaryTopBarCommands();
				topAppBar.SecondaryCommands = page.GetSecondaryTopBarCommands();
				topAppBar.Reset();
			}

			if (bottomAppBar != null)
			{
				bottomAppBar.PrimaryCommands = page.GetPrimaryBottomBarCommands();
				bottomAppBar.SecondaryCommands = page.GetSecondaryBottomBarCommands();
				bottomAppBar.Content = childrens.LastOrDefault(x => x.ContentBottomBar != null)?.ContentBottomBar;
				bottomAppBar.Reset();
			}
		}

		public void ShowContentDialog(FormsContentDialog contentDialog)
		{
			this.CurrentContentDialog = contentDialog;
			this.HasContentDialog = true;
		}

		public void HideContentDialog()
		{
			this.CurrentContentDialog = null;
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
				return null;

			var modal = InternalChildren.Last();

			if (InternalChildren.Remove(modal))
			{
				/*if (LightContentControl != null)
				{
					LightContentControl.Transition = animated ? TransitionType.Right : TransitionType.Normal;
				}*/
				CurrentModalPage = InternalChildren.LastOrDefault();
			}

			this.HasBackButtonModal = InternalChildren.Count >= 1;
			this.HasModalPage = InternalChildren.Count >= 1;

			return modal;
		}

		public virtual void OnBackSystemButtonPressed()
		{
			PopModal();
		}
	}
}
