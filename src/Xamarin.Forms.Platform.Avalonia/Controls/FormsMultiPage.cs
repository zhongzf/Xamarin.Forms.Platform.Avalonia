using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Xamarin.Forms.Platform.Avalonia.Interfaces;

namespace Xamarin.Forms.Platform.Avalonia.Controls
{
    public class SelectionChangedEventArgs : EventArgs
    {
        public SelectionChangedEventArgs(object oldElement, object newElement)
        {
            OldElement = oldElement;
            NewElement = newElement;
        }

        public object NewElement { get; private set; }

        public object OldElement { get; private set; }
    }

    public class FormsMultiPage : FormsPage
    {
        public FormsTransitioningContentControl FormsContentControl { get; private set; }

        public event EventHandler<SelectionChangedEventArgs> SelectionChanged;

        public static readonly StyledProperty<IContentLoader> ContentLoaderProperty = AvaloniaProperty.Register<FormsMultiPage, IContentLoader>(nameof(ContentLoader), new DefaultContentLoader());
        public static readonly StyledProperty<ObservableCollection<object>> ItemsSourceProperty = AvaloniaProperty.Register<FormsMultiPage, ObservableCollection<object>>(nameof(ItemsSource));
        public static readonly StyledProperty<object> SelectedItemProperty = AvaloniaProperty.Register<FormsMultiPage, object>(nameof(SelectedItem));
        public static readonly StyledProperty<int> SelectedIndexProperty = AvaloniaProperty.Register<FormsMultiPage, int>(nameof(SelectedIndex), 0);

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

        public FormsMultiPage()
        {
            this.LayoutUpdated += OnLayoutUpdated;

            SelectedItemProperty.Changed.AddClassHandler<FormsMultiPage>((x, e) => x.OnSelectedItemChanged(e));
            SetValue(FormsMultiPage.ItemsSourceProperty, new ObservableCollection<object>());
        }

        protected virtual void OnLayoutUpdated(object sender, EventArgs e)
        {
            OnContentLoaderLayoutUpdated(this, e);
        }

        private void OnSelectedItemChanged(AvaloniaPropertyChangedEventArgs e)
        {
            if (e.OldValue == e.NewValue) return;
            OnSelectedItemChanged(e.OldValue, e.NewValue);
            OnContentLoaderLayoutUpdated(this, e);
        }

        private void OnSelectedItemChanged(object oldValue, object newValue)
        {
            if (ItemsSource == null) return;
            SelectedIndex = ItemsSource.Cast<object>().ToList().IndexOf(newValue);
            SelectionChanged?.Invoke(this, new SelectionChangedEventArgs(oldValue, newValue));
        }

        protected virtual void OnContentLoaderLayoutUpdated(object sender, EventArgs e)
        {
        }

        protected override void OnTemplateApplied(TemplateAppliedEventArgs e)
        {
            base.OnTemplateApplied(e);
            // TODO:
            FormsContentControl = this.FindControl<FormsTransitioningContentControl>("PART_Multi_Content") as FormsTransitioningContentControl;
        }

        public override bool GetHasNavigationBar()
        {
            if (FormsContentControl != null && FormsContentControl.Content is FormsPage page)
            {
                return page.GetHasNavigationBar();
            }
            return false;
        }

        //public override IEnumerable<FrameworkElement> GetPrimaryTopBarCommands()
        //{
        //	List<FrameworkElement> frameworkElements = new List<FrameworkElement>();
        //	frameworkElements.AddRange(this.PrimaryTopBarCommands);

        //	if (FormsContentControl != null && FormsContentControl.Content is FormsPage page)
        //	{
        //		frameworkElements.AddRange(page.GetPrimaryTopBarCommands());
        //	}

        //	return frameworkElements;
        //}

        //public override IEnumerable<FrameworkElement> GetSecondaryTopBarCommands()
        //{
        //	List<FrameworkElement> frameworkElements = new List<FrameworkElement>();
        //	frameworkElements.AddRange(this.SecondaryTopBarCommands);

        //	if (FormsContentControl != null && FormsContentControl.Content is FormsPage page)
        //	{
        //		frameworkElements.AddRange(page.GetSecondaryTopBarCommands());
        //	}

        //	return frameworkElements;
        //}

        //public override IEnumerable<FrameworkElement> GetPrimaryBottomBarCommands()
        //{
        //	List<FrameworkElement> frameworkElements = new List<FrameworkElement>();
        //	frameworkElements.AddRange(this.PrimaryBottomBarCommands);

        //	if (FormsContentControl != null && FormsContentControl.Content is FormsPage page)
        //	{
        //		frameworkElements.AddRange(page.GetPrimaryBottomBarCommands());
        //	}

        //	return frameworkElements;
        //}

        //public override IEnumerable<FrameworkElement> GetSecondaryBottomBarCommands()
        //{
        //	List<FrameworkElement> frameworkElements = new List<FrameworkElement>();
        //	frameworkElements.AddRange(this.SecondaryBottomBarCommands);

        //	if (FormsContentControl != null && FormsContentControl.Content is FormsPage page)
        //	{
        //		frameworkElements.AddRange(page.GetSecondaryBottomBarCommands());
        //	}

        //	return frameworkElements;
        //}
    }
}
