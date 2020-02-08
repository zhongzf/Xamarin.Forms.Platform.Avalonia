using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Styling;
using System;
using Xamarin.Forms.Internals;

namespace Xamarin.Forms.Platform.Avalonia
{
    public class ItemContentControl : ContentControl
    {
        public static readonly StyledProperty<DataTemplate> FormsDataTemplateProperty = AvaloniaProperty.Register<ItemContentControl, DataTemplate>(nameof(FormsDataTemplate));
        public static readonly StyledProperty<object> FormsDataContextProperty = AvaloniaProperty.Register<ItemContentControl, object>(nameof(FormsDataContext));
        public static readonly StyledProperty<BindableObject> FormsContainerProperty = AvaloniaProperty.Register<ItemContentControl, BindableObject>(nameof(FormsContainer));
        public static readonly StyledProperty<double> ItemHeightProperty = AvaloniaProperty.Register<ItemContentControl, double>(nameof(ItemHeight));
        public static readonly StyledProperty<double> ItemWidthProperty = AvaloniaProperty.Register<ItemContentControl, double>(nameof(ItemWidth));
        public static readonly StyledProperty<double> ItemSpacingProperty = AvaloniaProperty.Register<ItemContentControl, double>(nameof(ItemSpacing));

        static ItemContentControl()
        {
            FormsDataTemplateProperty.Changed.AddClassHandler<ItemContentControl>((x, e) => x.FormsDataTemplateChanged(e));
            FormsDataContextProperty.Changed.AddClassHandler<ItemContentControl>((x, e) => x.FormsDataContextChanged(e));
            FormsContainerProperty.Changed.AddClassHandler<ItemContentControl>((x, e) => x.FormsContainerChanged(e));
        }

        public DataTemplate FormsDataTemplate
        {
            get => (DataTemplate)GetValue(FormsDataTemplateProperty);
            set => SetValue(FormsDataTemplateProperty, value);
        }

        public object FormsDataContext
        {
            get => GetValue(FormsDataContextProperty);
            set => SetValue(FormsDataContextProperty, value);
        }

        public BindableObject FormsContainer
        {
            get => (BindableObject)GetValue(FormsContainerProperty);
            set => SetValue(FormsContainerProperty, value);
        }

        public double ItemHeight
        {
            get => (double)GetValue(ItemHeightProperty);
            set => SetValue(ItemHeightProperty, value);
        }

        public double ItemWidth
        {
            get => (double)GetValue(ItemWidthProperty);
            set => SetValue(ItemWidthProperty, value);
        }

        public Thickness ItemSpacing
        {
            get => (Thickness)GetValue(ItemSpacingProperty);
            set => SetValue(ItemSpacingProperty, value);
        }


        IVisualElementRenderer _renderer;

        public ItemContentControl()
        {
        }


        private void FormsDataTemplateChanged(AvaloniaPropertyChangedEventArgs e)
        {
            if (e.NewValue == null)
            {
                return;
            }
            Realize();
        }

        private void FormsDataContextChanged(AvaloniaPropertyChangedEventArgs e)
        {
            Realize();
        }

        private void FormsContainerChanged(AvaloniaPropertyChangedEventArgs e)
        {
            Realize();
        }

        internal void Realize()
        {
            var dataContext = FormsDataContext;
            var formsTemplate = FormsDataTemplate;
            var container = FormsContainer;

            var itemsView = container as ItemsView;

            if (itemsView != null && _renderer?.Element != null)
            {
                itemsView.RemoveLogicalChild(_renderer.Element);
            }

            if (dataContext == null || formsTemplate == null || container == null)
            {
                return;
            }

            var view = FormsDataTemplate.CreateContent(dataContext, container) as View;
            view.BindingContext = dataContext;
            _renderer = Platform.CreateRenderer(view);
            Platform.SetRenderer(view, _renderer);

            Content = _renderer.ContainerElement;

            itemsView?.AddLogicalChild(view);
        }

        protected override global::Avalonia.Size MeasureOverride(global::Avalonia.Size availableSize)
        {
            if (_renderer == null)
            {
                return base.MeasureOverride(availableSize);
            }

            var frameworkElement = Content as Control;

            var formsElement = _renderer.Element;
            if (ItemHeight != default || ItemWidth != default)
            {
                formsElement.Layout(new Rectangle(0, 0, ItemWidth, ItemHeight));

                var wsize = new global::Avalonia.Size(ItemWidth, ItemHeight);

                frameworkElement.Margin = new global::Avalonia.Thickness(ItemSpacing.Left, ItemSpacing.Top, ItemSpacing.Right, ItemSpacing.Bottom);

                frameworkElement.Measure(wsize);

                return base.MeasureOverride(wsize);
            }
            else
            {
                var (width, height) = formsElement.Measure(availableSize.Width, availableSize.Height,
                    MeasureFlags.IncludeMargins).Request;

                width = Max(width, availableSize.Width);
                height = Max(height, availableSize.Height);

                formsElement.Layout(new Rectangle(0, 0, width, height));

                var wsize = new global::Avalonia.Size(width, height);

                frameworkElement.Measure(wsize);

                return base.MeasureOverride(wsize);
            }
        }

        double Max(double requested, double available)
        {
            return Math.Max(requested, ClampInfinity(available));
        }

        double ClampInfinity(double value)
        {
            return double.IsInfinity(value) ? 0 : value;
        }
    }
}