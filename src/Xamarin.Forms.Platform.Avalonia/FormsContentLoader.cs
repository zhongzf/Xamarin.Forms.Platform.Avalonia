using Avalonia;
using Avalonia.Controls;
using Avalonia.Forms.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Xamarin.Forms.Platform.Avalonia.Extensions;

namespace Xamarin.Forms.Platform.Avalonia
{
    public class FormsContentLoader : IContentLoader
    {
        public Task<object> LoadContentAsync(Control parent, object oldContent, object newContent, CancellationToken cancellationToken)
        {
            VisualElement element = oldContent as VisualElement;
            if (element != null)
            {
                element.Cleanup(); // Cleanup old content
            }

            if (!global::Avalonia.Application.Current.CheckAccess())
                throw new InvalidOperationException("UIThreadRequired");

            var scheduler = TaskScheduler.FromCurrentSynchronizationContext();
            return Task.Factory.StartNew(() => LoadContent(parent, newContent), cancellationToken, TaskCreationOptions.None, scheduler);
        }

        protected virtual object LoadContent(Control parent, object page)
        {
            VisualElement visualElement = page as VisualElement;
            if (visualElement != null)
            {
                var renderer = CreateOrResizeContent(parent, visualElement);
                return renderer;
            }
            return null;
        }

        public void OnSizeContentChanged(Control parent, object page)
        {
            VisualElement visualElement = page as VisualElement;
            if (visualElement != null)
            {
                CreateOrResizeContent(parent, visualElement);
            }
        }

        public void OnSizeContentChanged(Rect parentBounds, object page)
        {
            VisualElement visualElement = page as VisualElement;
            if (visualElement != null)
            {
                CreateOrResizeContent(parentBounds, visualElement);
            }
        }

        private object CreateOrResizeContent(Control parent, VisualElement visualElement)
        {
            //if (Debugger.IsAttached)
            //	Console.WriteLine("Page type : " + visualElement.GetType() + " (" + (visualElement as Page).Title + ") -- Parent type : " + visualElement.Parent.GetType() + " -- " + parent.ActualHeight + "H*" + parent.ActualWidth + "W");
            var parentBounds = parent.Bounds;
            return CreateOrResizeContent(parentBounds, visualElement);
        }

        private object CreateOrResizeContent(Rect parentBounds, VisualElement visualElement)
        {
            var renderer = Platform.GetOrCreateRenderer(visualElement);
            var actualRect = new Rectangle(0, 0, parentBounds.Width, parentBounds.Height);
            visualElement.Layout(actualRect);

            // ControlTemplate adds an additional layer through which to send sizing changes.
            var contentPage = visualElement as ContentPage;
            if (contentPage?.Content != null)
            {
                contentPage.Content?.Layout(actualRect);
            }
            else
            {
                var contentView = visualElement as ContentView;
                if (contentView?.Content != null)
                {
                    contentView.Content?.Layout(actualRect);
                }
            }

            IPageController pageController = visualElement.RealParent as IPageController;
            if (pageController != null)
            {
                pageController.ContainerArea = actualRect;
            }

            return renderer.GetNativeElement();
        }
    }
}
