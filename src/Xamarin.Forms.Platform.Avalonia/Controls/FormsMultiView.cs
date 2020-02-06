using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using AvaloniaForms.Controls;
using AvaloniaForms.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using ASelectionChangedEventArgs = AvaloniaForms.Controls.SelectionChangedEventArgs;

namespace Xamarin.Forms.Platform.Avalonia.Controls
{
    public class FormsMultiView : UserControl
    {
        public TransitioningContentControl ContentControl { get; private set; }

        public event EventHandler<ASelectionChangedEventArgs> SelectionChanged;

        public static readonly StyledProperty<IContentLoader> ContentLoaderProperty = AvaloniaProperty.Register<FormsMultiView, IContentLoader>(nameof(ContentLoader), new DefaultContentLoader());
        public static readonly StyledProperty<ObservableCollection<object>> ItemsSourceProperty = AvaloniaProperty.Register<FormsMultiView, ObservableCollection<object>>(nameof(ItemsSource));
        public static readonly StyledProperty<object> SelectedItemProperty = AvaloniaProperty.Register<FormsMultiView, object>(nameof(SelectedItem));
        public static readonly StyledProperty<int> SelectedIndexProperty = AvaloniaProperty.Register<FormsMultiView, int>(nameof(SelectedIndex), 0);

        public IContentLoader ContentLoader
        {
            get { return (IContentLoader)GetValue(ContentLoaderProperty); }
            set { SetValue(ContentLoaderProperty, value); }
        }

        public ObservableCollection<object> ItemsSource
        {
            get { return (ObservableCollection<object>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public object SelectedItem
        {
            get { return (object)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        public FormsMultiView()
        {
            this.LayoutUpdated += OnLayoutUpdated;

            AttachedToVisualTree += (sender, e) => Appearing();
            DetachedFromVisualTree += (sender, e) => Disappearing();
            
            SelectedItemProperty.Changed.AddClassHandler<FormsMultiView>((x, e) => x.OnSelectedItemChanged(e));
            SetValue(FormsMultiView.ItemsSourceProperty, new ObservableCollection<object>());
        }

        protected virtual void OnLayoutUpdated(object sender, EventArgs e)
        {
            OnContentLoaderLayoutUpdated(this, e);
        }

        protected virtual void Appearing()
        {
        }

        protected virtual void Disappearing()
        {
        }

        protected virtual void OnSelectedItemChanged(AvaloniaPropertyChangedEventArgs e)
        {
            if (e.OldValue == e.NewValue) return;
            OnSelectedItemChanged(e.OldValue, e.NewValue);
            OnContentLoaderLayoutUpdated(this, e);
        }

        private void OnSelectedItemChanged(object oldValue, object newValue)
        {
            if (ItemsSource == null) return;
            SelectedIndex = ItemsSource.Cast<object>().ToList().IndexOf(newValue);
            SelectionChanged?.Invoke(this, new ASelectionChangedEventArgs(oldValue, newValue));
        }

        protected virtual void OnContentLoaderLayoutUpdated(object sender, EventArgs e)
        {
        }

        protected override void OnTemplateApplied(TemplateAppliedEventArgs e)
        {
            base.OnTemplateApplied(e);

            ContentControl = e.NameScope.Find<TransitioningContentControl>("PART_Multi_Content");
        }
    }
}
