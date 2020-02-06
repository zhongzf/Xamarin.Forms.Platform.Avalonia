using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Forms.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Avalonia.Forms.Controls
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

    public class MultiContentPage : DynamicContentPage
    {
        public static readonly StyledProperty<ObservableCollection<object>> ItemsSourceProperty = AvaloniaProperty.Register<MultiContentPage, ObservableCollection<object>>(nameof(ItemsSource));
        public static readonly StyledProperty<object> SelectedItemProperty = AvaloniaProperty.Register<MultiContentPage, object>(nameof(SelectedItem));
        public static readonly StyledProperty<int> SelectedIndexProperty = AvaloniaProperty.Register<MultiContentPage, int>(nameof(SelectedIndex), 0);

        static MultiContentPage()
        {

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

        public TransitioningContentControl ContentControl { get; private set; }

        public event EventHandler<SelectionChangedEventArgs> SelectionChanged;

        public MultiContentPage()
        {
            SetValue(MultiContentPage.ItemsSourceProperty, new ObservableCollection<object>());
        }

        protected override void OnTemplateApplied(TemplateAppliedEventArgs e)
        {
            base.OnTemplateApplied(e);

            ContentControl = e.NameScope.Find<TransitioningContentControl>("PART_Multi_Content");
        }

        public override bool GetHasNavigationBar()
        {
            if (ContentControl != null && ContentControl.Content is DynamicContentPage page)
            {
                return page.GetHasNavigationBar();
            }
            return false;
        }

        public override IEnumerable<Control> GetPrimaryTopBarCommands()
        {
            List<Control> frameworkElements = new List<Control>();
            frameworkElements.AddRange(this.PrimaryTopBarCommands);

            if (ContentControl != null && ContentControl.Content is DynamicContentPage page)
            {
                frameworkElements.AddRange(page.GetPrimaryTopBarCommands());
            }

            return frameworkElements;
        }

        public override IEnumerable<Control> GetSecondaryTopBarCommands()
        {
            List<Control> frameworkElements = new List<Control>();
            frameworkElements.AddRange(this.SecondaryTopBarCommands);

            if (ContentControl != null && ContentControl.Content is DynamicContentPage page)
            {
                frameworkElements.AddRange(page.GetSecondaryTopBarCommands());
            }

            return frameworkElements;
        }

        public override IEnumerable<Control> GetPrimaryBottomBarCommands()
        {
            List<Control> frameworkElements = new List<Control>();
            frameworkElements.AddRange(this.PrimaryBottomBarCommands);

            if (ContentControl != null && ContentControl.Content is DynamicContentPage page)
            {
                frameworkElements.AddRange(page.GetPrimaryBottomBarCommands());
            }

            return frameworkElements;
        }

        public override IEnumerable<Control> GetSecondaryBottomBarCommands()
        {
            List<Control> frameworkElements = new List<Control>();
            frameworkElements.AddRange(this.SecondaryBottomBarCommands);

            if (ContentControl != null && ContentControl.Content is DynamicContentPage page)
            {
                frameworkElements.AddRange(page.GetSecondaryBottomBarCommands());
            }

            return frameworkElements;
        }

    }
}
