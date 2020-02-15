using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms.Internals;
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
                ((INotifyCollectionChanged)oldElement.Children).CollectionChanged -= OnPagesChanged;
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
                OnPagesChanged(Element.Children, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));

                ((INotifyCollectionChanged)Element.Children).CollectionChanged += OnPagesChanged;
                element.PropertyChanged += OnElementPropertyChanged;
            }

            OnElementChanged(new VisualElementChangedEventArgs(oldElement, element));
        }

        public void Dispose()
        {
            Dispose(true);
        }

        bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing || _disposed)
                return;

            _disposed = true;
            Element?.SendDisappearing();
            SetElement(null);
        }

        protected virtual void OnElementChanged(VisualElementChangedEventArgs e)
        {
            ElementChanged?.Invoke(this, e);
        }

        void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(TabbedPage.CurrentPage))
            {
                UpdateCurrentPage();
            }
        }

        void OnLoaded(object sender, RoutedEventArgs args)
        {
            Element?.SendAppearing();
        }

        void OnPagesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var items = new List<object>();
            e.Apply(Element.Children, items);
            Control.Items = items;

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                case NotifyCollectionChangedAction.Remove:
                case NotifyCollectionChangedAction.Replace:
                    if (e.NewItems != null)
                        for (int i = 0; i < e.NewItems.Count; i++)
                            ((Page)e.NewItems[i]).PropertyChanged += OnChildPagePropertyChanged;
                    if (e.OldItems != null)
                        for (int i = 0; i < e.OldItems.Count; i++)
                            ((Page)e.OldItems[i]).PropertyChanged -= OnChildPagePropertyChanged;
                    break;
                case NotifyCollectionChangedAction.Reset:
                    foreach (var page in Element.Children)
                        page.PropertyChanged += OnChildPagePropertyChanged;
                    break;
            }

            Control.InvalidateArrange();
        }

        void OnChildPagePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var page = sender as Page;
            if (page != null)
            {
                // If AccessKeys properties are updated on a child (tab) we want to
                // update the access key on the native control.
            }
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

        void UpdateCurrentPage()
        {
            Page page = Element.CurrentPage;

            if (page == null)
                return;

            Control.SelectedItem = page;
        }

    }
}
