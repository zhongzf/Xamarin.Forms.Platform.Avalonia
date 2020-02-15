using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms.Platform.Avalonia.Controls;

namespace Xamarin.Forms.Platform.Avalonia
{
    public class NavigationPageRenderer : IVisualElementRenderer
    {
        FormsPageControl _container;
        Page _currentPage;
        Page _previousPage;

        public Control ContainerElement => _container;

        VisualElement IVisualElementRenderer.Element => Element;

        public event EventHandler<VisualElementChangedEventArgs> ElementChanged;

		public void Dispose()
		{
			Dispose(true);
		}

		bool _disposed;

		protected void Dispose(bool disposing)
		{
			if (_disposed || !disposing)
			{
				return;
			}

			Element?.SendDisappearing();
			_disposed = true;

			SetElement(null);
			SetPage(null, false, true);
			_previousPage = null;
		}


		public NavigationPage Element { get; private set; }

        public SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
        {
            var constraint = new global::Avalonia.Size(widthConstraint, heightConstraint);
            IVisualElementRenderer childRenderer = Platform.GetRenderer(Element.CurrentPage);
            Control child = childRenderer.ContainerElement;

            double oldWidth = child.Width;
            double oldHeight = child.Height;

            child.Height = double.NaN;
            child.Width = double.NaN;

            child.Measure(constraint);
            var result = new Size(Math.Ceiling(child.DesiredSize.Width), Math.Ceiling(child.DesiredSize.Height));

            child.Width = oldWidth;
            child.Height = oldHeight;

            return new SizeRequest(result);
        }

        public InputElement GetNativeElement()
        {
            throw new NotImplementedException();
        }

		public void SetElement(VisualElement element)
		{
			if (element != null && !(element is NavigationPage))
				throw new ArgumentException("Element must be a Page", nameof(element));

			NavigationPage oldElement = Element;
			Element = (NavigationPage)element;

			if (Element != null && Element.CurrentPage is null)
				throw new InvalidOperationException(
					"NavigationPage must have a root Page before being used. Either call PushAsync with a valid Page, or pass a Page to the constructor before usage.");

			if (oldElement != null)
			{
				// TODO:
			}

			if (element != null)
			{
				if (_container == null)
				{
					_container = new FormsPageControl();
					_container.SizeChanged += OnNativeSizeChanged;

					SetPage(Element.CurrentPage, false, false);

					_container.Loaded += OnLoaded;
					_container.Unloaded += OnUnloaded;
				}

				_container.DataContext = Element.CurrentPage;

				// TODO:


				PushExistingNavigationStack();
			}

			OnElementChanged(new VisualElementChangedEventArgs(oldElement, element));
		}

		void PushExistingNavigationStack()
		{
			foreach (var page in Element.Pages)
			{
				SetPage(page, false, false);
			}
		}

		void SetPage(Page page, bool isAnimated, bool isPopping)
		{
			if (_currentPage != null)
			{
				if (isPopping)
					_currentPage.Cleanup();

				_container.Content = null;
				_currentPage.PropertyChanged -= OnCurrentPagePropertyChanged;
			}

			if (!isPopping)
				_previousPage = _currentPage;

			_currentPage = page;

			if (page == null)
				return;


			page.PropertyChanged += OnCurrentPagePropertyChanged;

			IVisualElementRenderer renderer = page.GetOrCreateRenderer();

			_container.Content = renderer.ContainerElement;
			_container.DataContext = page;
		}

		void OnNativeSizeChanged(object sender, EventArgs e)
		{
			UpdateContainerArea();
		}

		void UpdateContainerArea()
		{
			Element.ContainerArea = new Rectangle(0, 0, _container.ContentWidth, _container.ContentHeight);
		}

		void OnLoaded(object sender, RoutedEventArgs args)
		{
			if (Element == null)
				return;

			Element.SendAppearing();
		}

		void OnUnloaded(object sender, RoutedEventArgs args)
		{
			Element?.SendDisappearing();
		}

		void OnCurrentPagePropertyChanged(object sender, PropertyChangedEventArgs e)
		{
		}

		protected void OnElementChanged(VisualElementChangedEventArgs e)
		{
			ElementChanged?.Invoke(this, e);
		}
	}
}
