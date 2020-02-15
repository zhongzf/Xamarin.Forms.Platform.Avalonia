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
    public class TabbedPageRenderer : IVisualElementRenderer
    {
		public Control ContainerElement => Control;

        public FormsTabControl Control { get; private set; }

        public TabbedPage Element { get; private set; }

		VisualElement IVisualElementRenderer.Element => Element;

		public event EventHandler<VisualElementChangedEventArgs> ElementChanged;

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
        {
            var constraint = new global::Avalonia.Size(widthConstraint, heightConstraint);

            double oldWidth = Control.Width;
            double oldHeight = Control.Height;

            Control.Height = double.NaN;
            Control.Width = double.NaN;

            Control.Measure(constraint);
            var result = new Size(Math.Ceiling(Control.DesiredSize.Width), Math.Ceiling(Control.DesiredSize.Height));

            Control.Width = oldWidth;
            Control.Height = oldHeight;

            return new SizeRequest(result);
        }

        public InputElement GetNativeElement()
        {
            return Control;
        }

		public void SetElement(VisualElement element)
		{
			if (element != null && !(element is TabbedPage))
				throw new ArgumentException("Element must be a TabbedPage", "element");

			TabbedPage oldElement = Element;
			Element = (TabbedPage)element;

			if (oldElement != null)
			{
				oldElement.PropertyChanged -= OnElementPropertyChanged;
			}

			if (element != null)
			{
				if (Control == null)
				{
					Control = new FormsTabControl();

					Control.SelectionChanged += OnSelectionChanged;

					Control.Loaded += OnLoaded;
					Control.Unloaded += OnUnloaded;
				}

				Control.DataContext = Element;
			}

			OnElementChanged(new VisualElementChangedEventArgs(oldElement, element));
		}

		protected virtual void OnElementChanged(VisualElementChangedEventArgs e)
		{
			ElementChanged?.Invoke(this, e);
		}

		void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
		}

		void OnLoaded(object sender, RoutedEventArgs args)
		{
			Element?.SendAppearing();
		}

		void OnSelectionChanged(object sender, global::Avalonia.Controls.SelectionChangedEventArgs e)
		{
			if (Element == null)
				return;

			Page page = e.AddedItems.Count > 0 ? (Page)e.AddedItems[0] : null;
			Page currentPage = Element.CurrentPage;
			if (currentPage == page)
				return;
			currentPage?.SendDisappearing();
			Element.CurrentPage = page;

			page?.SendAppearing();
		}

		void OnUnloaded(object sender, RoutedEventArgs args)
		{
			Element?.SendDisappearing();
		}
	}
}
