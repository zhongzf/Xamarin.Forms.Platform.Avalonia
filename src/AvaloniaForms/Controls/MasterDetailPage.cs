using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Avalonia.Styling;

namespace AvaloniaForms.Controls
{
    public class MasterDetailPage : DynamicContentPage, IStyleable
    {
        public static readonly StyledProperty<object> MasterPageProperty = AvaloniaProperty.Register<MasterDetailPage, object>(nameof(MasterPage));
        public static readonly StyledProperty<object> DetailPageProperty = AvaloniaProperty.Register<MasterDetailPage, object>(nameof(DetailPage));
        public static readonly StyledProperty<bool> IsPresentedProperty = AvaloniaProperty.Register<MasterDetailPage, bool>(nameof(IsPresented));

        static MasterDetailPage()
        {
            IsPresentedProperty.Changed.AddClassHandler<MasterDetailPage>((x, e) => x.OnIsPresentedPropertyChanged(e));
        }

        Type IStyleable.StyleKey => typeof(MasterDetailPage);

        public object MasterPage
        {
            get { return (object)GetValue(MasterPageProperty); }
            set { SetValue(MasterPageProperty, value); }
        }

        public object DetailPage
        {
            get { return (object)GetValue(DetailPageProperty); }
            set { SetValue(DetailPageProperty, value); }
        }

        public bool IsPresented
        {
            get { return (bool)GetValue(IsPresentedProperty); }
            set { SetValue(IsPresentedProperty, value); }
        }

        public DynamicContentControl MasterContentControl { get; private set; }
        public DynamicContentControl DetailContentControl  { get; private set; }
        public Grid GridContainer { get; private set; }

        public double? MasterColumnWidth { get; private set; }

        public MasterDetailPage()
        {
        }

        private void OnIsPresentedPropertyChanged(AvaloniaPropertyChangedEventArgs e)
        {
            if (GridContainer != null)
            {
                if ((bool)e.NewValue)
                {
                    if ((GridContainer.ColumnDefinitions[0].Width.Value <= 0) && (MasterColumnWidth ?? 0) > 0)
                    {
                        GridContainer.ColumnDefinitions[0].Width = new GridLength((double)MasterColumnWidth);
                    }
                }
                else
                {
                    var value = GridContainer.ColumnDefinitions[0]?.Width.Value;
                    if (value > 0)
                    {
                        MasterColumnWidth = value;
                    }
                    GridContainer.ColumnDefinitions[0].Width = new GridLength(0);
                }
            }
        }

        protected override void OnTemplateApplied(TemplateAppliedEventArgs e)
        {
            base.OnTemplateApplied(e);

            MasterContentControl = e.NameScope.Find<DynamicContentControl>("PART_Master");
            DetailContentControl = e.NameScope.Find<DynamicContentControl>("PART_Detail_Content");

            GridContainer = e.NameScope.Find<Grid>("PART_Container");

            MasterColumnWidth = GridContainer?.ColumnDefinitions[0]?.Width.Value;
            if (!this.IsPresented)
            {
                GridContainer.ColumnDefinitions[0].Width = new GridLength(0);
            }
        }


        public override string GetTitle()
        {
            if (DetailContentControl?.Content is DynamicContentPage page)
            {
                return page.GetTitle();
            }
            return this.Title;
        }

        public override Brush GetTitleBarBackgroundColor()
        {
            if (DetailContentControl?.Content is DynamicContentPage page)
            {
                return page.GetTitleBarBackgroundColor();
            }
            return this.TitleBarBackgroundColor;
        }

        public override Brush GetTitleBarTextColor()
        {
            if (DetailContentControl?.Content is DynamicContentPage page)
            {
                return page.GetTitleBarTextColor();
            }
            return this.TitleBarTextColor;
        }

        public override IEnumerable<Control> GetPrimaryTopBarCommands()
        {
            return PrimaryTopBarCommands.Merge(MasterContentControl, page => page.GetPrimaryTopBarCommands()).Merge(DetailContentControl, page => page.GetPrimaryTopBarCommands());
        }

        public override IEnumerable<Control> GetSecondaryTopBarCommands()
        {
            return SecondaryTopBarCommands.Merge(MasterContentControl, page => page.GetSecondaryTopBarCommands()).Merge(DetailContentControl, page => page.GetSecondaryTopBarCommands());
        }

        public override IEnumerable<Control> GetPrimaryBottomBarCommands()
        {
            return PrimaryBottomBarCommands.Merge(MasterContentControl, page => page.GetPrimaryBottomBarCommands()).Merge(DetailContentControl, page => page.GetPrimaryBottomBarCommands());
        }

        public override IEnumerable<Control> GetSecondaryBottomBarCommands()
        {
            return SecondaryBottomBarCommands.Merge(MasterContentControl, page => page.GetSecondaryBottomBarCommands()).Merge(DetailContentControl, page => page.GetSecondaryBottomBarCommands());
        }
    }
}
