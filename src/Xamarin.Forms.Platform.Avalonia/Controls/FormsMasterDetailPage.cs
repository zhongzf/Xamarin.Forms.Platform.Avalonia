using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Platform.Avalonia.Extensions;
using Xamarin.Forms.Platform.Avalonia.Interfaces;
using System.Linq;

namespace Xamarin.Forms.Platform.Avalonia.Controls
{
    public class FormsMasterDetailPage : FormsPage
    {
        FormsContentControl lightMasterContentControl;
        FormsContentControl lightDetailContentControl;

        global::Avalonia.Controls.Grid container;

        public static readonly StyledProperty<object> MasterPageProperty = AvaloniaProperty.Register<FormsMasterDetailPage, object>(nameof(MasterPage));
        public static readonly StyledProperty<object> DetailPageProperty = AvaloniaProperty.Register<FormsMasterDetailPage, object>(nameof(DetailPage));
        public static readonly StyledProperty<IContentLoader> ContentLoaderProperty = AvaloniaProperty.Register<FormsMasterDetailPage, IContentLoader>(nameof(ContentLoader));
        public static readonly StyledProperty<bool> IsPresentedProperty = AvaloniaProperty.Register<FormsMasterDetailPage, bool>(nameof(IsPresented));

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

        public IContentLoader ContentLoader
        {
            get { return (IContentLoader)GetValue(ContentLoaderProperty); }
            set { SetValue(ContentLoaderProperty, value); }
        }

        public FormsMasterDetailPage()
        {
            IsPresentedProperty.Changed.AddClassHandler<FormsMasterDetailPage>((x, e) => x.OnIsPresentedChanged(e));
        }

        protected double? oldColumnDefinitions;

        protected virtual void OnIsPresentedChanged(AvaloniaPropertyChangedEventArgs e)
        {
            if (this.container != null)
            {
                if ((bool)e.NewValue)
                {
                    if ((this.container.ColumnDefinitions[0].Width.Value <= 0) && (this.oldColumnDefinitions ?? 0) > 0)
                    {
                        this.container.ColumnDefinitions[0].Width = new global::Avalonia.Controls.GridLength((double)this.oldColumnDefinitions);
                    }
                }
                else
                {
                    var oldWidth = this.container.ColumnDefinitions.FirstOrDefault().Width.Value;
                    if(oldWidth > 0)
                    {
                        this.oldColumnDefinitions = oldWidth;
                    }
                    this.container.ColumnDefinitions[0].Width = new global::Avalonia.Controls.GridLength(0);
                }
            }
        }

        protected override void OnTemplateApplied(TemplateAppliedEventArgs e)
        {
            base.OnTemplateApplied(e);

            lightMasterContentControl = e.NameScope.Find<FormsContentControl>("PART_Master") as FormsContentControl;
            lightDetailContentControl = e.NameScope.Find<FormsContentControl>("PART_Detail_Content") as FormsContentControl;

            container = this.Find<global::Avalonia.Controls.Grid>("PART_Container", e);
            this.oldColumnDefinitions = this.container.ColumnDefinitions.FirstOrDefault().Width.Value;
            if (!this.IsPresented)
            {
                this.container.ColumnDefinitions[0].Width = new global::Avalonia.Controls.GridLength(0);
            }
        }

        public override string GetTitle()
        {
            if (lightDetailContentControl != null && lightDetailContentControl.Content is FormsPage page)
            {
                return page.GetTitle();
            }
            return this.Title;
        }

        public override Brush GetTitleBarBackgroundColor()
        {
            if (lightDetailContentControl != null && lightDetailContentControl.Content is FormsPage page)
            {
                return page.GetTitleBarBackgroundColor();
            }
            return this.TitleBarBackgroundColor;
        }

        public override Brush GetTitleBarTextColor()
        {
            if (lightDetailContentControl != null && lightDetailContentControl.Content is FormsPage page)
            {
                return page.GetTitleBarTextColor();
            }
            return this.TitleBarTextColor;
        }

        public override IEnumerable<Control> GetPrimaryTopBarCommands()
        {
            List<Control> frameworkElements = new List<Control>();
            frameworkElements.AddRange(this.PrimaryTopBarCommands);

            if (lightMasterContentControl != null && lightMasterContentControl.Content is FormsPage masterPage)
            {
                frameworkElements.AddRange(masterPage.GetPrimaryTopBarCommands());
            }

            if (lightDetailContentControl != null && lightDetailContentControl.Content is FormsPage page)
            {
                frameworkElements.AddRange(page.GetPrimaryTopBarCommands());
            }
            return frameworkElements;
        }

        public override IEnumerable<Control> GetSecondaryTopBarCommands()
        {
            List<Control> frameworkElements = new List<Control>();
            frameworkElements.AddRange(this.SecondaryTopBarCommands);

            if (lightMasterContentControl != null && lightMasterContentControl.Content is FormsPage masterPage)
            {
                frameworkElements.AddRange(masterPage.GetSecondaryTopBarCommands());
            }

            if (lightDetailContentControl != null && lightDetailContentControl.Content is FormsPage page)
            {
                frameworkElements.AddRange(page.GetSecondaryTopBarCommands());
            }

            return frameworkElements;
        }

        public override IEnumerable<Control> GetPrimaryBottomBarCommands()
        {
            List<Control> frameworkElements = new List<Control>();
            frameworkElements.AddRange(this.PrimaryBottomBarCommands);

            if (lightMasterContentControl != null && lightMasterContentControl.Content is FormsPage masterPage)
            {
                frameworkElements.AddRange(masterPage.GetPrimaryBottomBarCommands());
            }

            if (lightDetailContentControl != null && lightDetailContentControl.Content is FormsPage page)
            {
                frameworkElements.AddRange(page.GetPrimaryBottomBarCommands());
            }

            return frameworkElements;
        }

        public override IEnumerable<Control> GetSecondaryBottomBarCommands()
        {
            List<Control> frameworkElements = new List<Control>();
            frameworkElements.AddRange(this.SecondaryBottomBarCommands);

            if (lightMasterContentControl != null && lightMasterContentControl.Content is FormsPage masterPage)
            {
                frameworkElements.AddRange(masterPage.GetSecondaryBottomBarCommands());
            }

            if (lightDetailContentControl != null && lightDetailContentControl.Content is FormsPage page)
            {
                frameworkElements.AddRange(page.GetSecondaryBottomBarCommands());
            }

            return frameworkElements;
        }
    }
}
