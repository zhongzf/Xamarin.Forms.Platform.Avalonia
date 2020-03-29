using Avalonia.Controls;
using Avalonia.Styling;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Xamarin.Forms.Platform.Avalonia
{
    public class PageRenderer : VisualElementRenderer<Page, Control>
    {
		bool _disposed;

		bool _loaded;

		protected override void Dispose(bool disposing)
		{
			if (!disposing || _disposed)
				return;

			_disposed = true;

			if (Element != null)
			{
				ReadOnlyCollection<Element> children = ((IElementController)Element).LogicalChildren;
				for (var i = 0; i < children.Count; i++)
				{
					var visualChild = children[i] as VisualElement;
					visualChild?.Cleanup();
				}
				Element?.SendDisappearing();
			}

			base.Dispose();
		}

		protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
		{
			base.OnElementChanged(e);

			e.OldElement?.SendDisappearing();

			if (e.NewElement != null)
			{
				if (e.OldElement == null)
				{
					Loaded += OnLoaded;
					// TODO:
					//Tracker = new BackgroundTracker<Control>(BackgroundProperty);
				}

				if (_loaded)
					e.NewElement.SendAppearing();
			}
		}

		void OnLoaded(object sender, EventArgs args)
		{
			var carouselPage = Element?.Parent as CarouselPage;
			if (carouselPage != null && carouselPage.Children[0] != Element)
			{
				return;
			}
			_loaded = true;
			Unloaded += OnUnloaded;
			Element?.SendAppearing();
		}

		void OnUnloaded(object sender, EventArgs args)
		{
			Unloaded -= OnUnloaded;
			_loaded = false;
			Element?.SendDisappearing();
		}
	}
}
