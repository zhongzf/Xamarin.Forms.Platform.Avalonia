using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Collections.ObjectModel;

namespace Xamarin.Forms.Platform.Avalonia.Experiments
{
    public partial class PageRenderer : VisualElementRenderer<Page, Control>
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
                    AttachedToVisualTree += OnAttachedToVisualTree;
                    Tracker = new BackgroundTracker<Control>(BackgroundProperty);
                }

                if (_loaded)
                    e.NewElement.SendAppearing();
            }
        }

        private void OnAttachedToVisualTree(object sender, global::Avalonia.VisualTreeAttachmentEventArgs e)
        {
            OnLoaded(sender, new RoutedEventArgs());
        }

        void OnLoaded(object sender, RoutedEventArgs args)
        {
            var carouselPage = Element?.Parent as CarouselPage;
            if (carouselPage != null && carouselPage.Children[0] != Element)
            {
                return;
            }
            _loaded = true;
            DetachedFromVisualTree += OnDetachedFromVisualTree;
            Element?.SendAppearing();
        }

        private void OnDetachedFromVisualTree(object sender, global::Avalonia.VisualTreeAttachmentEventArgs e)
        {
            OnUnloaded(sender, new RoutedEventArgs());
        }

        void OnUnloaded(object sender, RoutedEventArgs args)
        {
            DetachedFromVisualTree -= OnDetachedFromVisualTree;
            _loaded = false;
            Element?.SendDisappearing();
        }
    }
}