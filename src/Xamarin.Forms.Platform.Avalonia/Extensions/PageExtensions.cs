using Avalonia.Controls;
using System;

namespace Xamarin.Forms.Platform.Avalonia
{
	public static class PageExtensions
	{
		public static Control ToFrameworkElement(this Page view)
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
	}

	class DefaultApplication : Application
	{
	}
}
