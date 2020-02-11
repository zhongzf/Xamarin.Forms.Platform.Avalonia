using Avalonia.Controls;
using AvaloniaForms.Controls;
using System;
using System.Collections.Generic;

namespace Xamarin.Forms.Platform.Avalonia
{
	public static class PageExtensions
	{
		public static Control ToNativeElement(this Page view)
		{
			if (!Forms.IsInitialized)
				throw new InvalidOperationException("Call Forms.Init() before this");

			if (!(view.RealParent is Application))
			{
				Application app = new DefaultApplication
				{
					MainPage = view
				};

				var formsApplicationPage = new FormsApplicationPage();
				formsApplicationPage.LoadApplication(app);
				var platform = new AvaloniaPlatform(formsApplicationPage);
				platform.SetPage(view);
			}

			IVisualElementRenderer renderer = Platform.GetOrCreateRenderer(view);

			if (renderer == null)
			{
				throw new InvalidOperationException($"Could not find or create a renderer for {view}");
			}

			var frameworkElement = renderer.GetNativeElement();

			if (frameworkElement != null)
			{
				frameworkElement.Initialized += (sender, args) =>
				{
					view.Layout(new Rectangle(0, 0, frameworkElement.Bounds.Width, frameworkElement.Bounds.Height));
				};
				frameworkElement.LayoutUpdated += (sender, args) =>
				{
					view.Layout(new Rectangle(0, 0, frameworkElement.Bounds.Width, frameworkElement.Bounds.Height));
				};
			}

			return frameworkElement;
		}

		public static Control CreateFrameworkElement(this ContentPage contentPage)
		{
			return contentPage.ToNativeElement();
		}

		internal static Control ToFrameworkElement(this VisualElement visualElement)
		{
			if (!Forms.IsInitialized)
			{
				throw new InvalidOperationException("call Forms.Init() before this");
			}

			var root = new ApplicationWindow();

			// Yes, this looks awkward. But the page needs to be Platformed or several things won't work
			new AvaloniaPlatform(root);

			var renderer = visualElement.GetOrCreateRenderer();

			if (renderer == null)
			{
				throw new InvalidOperationException($"Could not find or create a renderer for {visualElement}");
			}

			var frameworkElement = renderer.ContainerElement;

			void UpdateLayout()
				=> visualElement.Layout(new Rectangle(0, 0, frameworkElement.Width, frameworkElement.Height));

			frameworkElement.AttachedToVisualTree += (sender, args) => UpdateLayout();

			// Workaround for Uno's Loaded event being raised before
			// ActualWidth and ActualHeight are being set.
			// This is required to get the X.F Shell to work properly.
			//
			// See https://github.com/unoplatform/uno/issues/1763 for more details.
			frameworkElement.LayoutUpdated += (s, e) => UpdateLayout();

			return frameworkElement;
		}

		//public static bool Navigate(this Windows.UI.Xaml.Controls.Frame frame, ContentPage page)
		//{
		//	return Navigate(frame, page, null);
		//}

		//internal static bool Navigate(this Windows.UI.Xaml.Controls.Frame frame, ContentPage page, Windows.UI.Xaml.Media.Animation.NavigationTransitionInfo infoOverride)
		//{

		//	if (page == null)
		//	{
		//		throw new ArgumentNullException(nameof(page));
		//	}

		//	Guid id = Guid.NewGuid();

		//	FormsEmbeddedPageWrapper.Pages.Add(id, page);
		//	if (infoOverride != null)
		//		return frame.Navigate(typeof(FormsEmbeddedPageWrapper), id, infoOverride);

		//	return frame.Navigate(typeof(FormsEmbeddedPageWrapper), id);
		//}
	}

	class DefaultApplication : Application
	{
	}

	public sealed partial class FormsEmbeddedPageWrapper :  EmbeddedPage
	{
		internal static Dictionary<Guid, ContentPage> Pages = new Dictionary<Guid, ContentPage>();

		public FormsEmbeddedPageWrapper()
		{
			//InitializeComponent();
		}

		//protected override void OnNavigatedTo(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
		//{
		//	base.OnNavigatedTo(e);

		//	if (e.Parameter == null)
		//	{
		//		throw new InvalidOperationException($"Cannot navigate to {nameof(FormsEmbeddedPageWrapper)} without "
		//			+ $"providing a {nameof(Xamarin.Forms.Page)} identifier.");
		//	}

		//	// Find the page instance in the dictionary and then discard it so we don't prevent it from being collected
		//	var key = (Guid)e.Parameter;
		//	var page = Pages[key];
		//	Pages.Remove(key);

		//	// Convert that page into a FrameWorkElement we can display in the ContentPresenter
		//	var frameworkElement = page.CreateFrameworkElement();

		//	if (frameworkElement == null)
		//	{
		//		throw new InvalidOperationException($"Could not find or create a renderer for the Page {page}");
		//	}

		//	Root.Content = frameworkElement;
		//}
	}
}
