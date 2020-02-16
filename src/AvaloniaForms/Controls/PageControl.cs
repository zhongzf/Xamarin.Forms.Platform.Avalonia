using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Styling;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvaloniaForms.Controls
{
    public class PageControl : ContentControl, IStyleable
    {
        Type IStyleable.StyleKey => typeof(PageControl);

        ContentPresenter _presenter;

        public double ContentHeight
        {
            get { return _presenter != null ? _presenter.Bounds.Height : 0; }
        }

        public double ContentWidth
        {
            get { return _presenter != null ? _presenter.Bounds.Width : 0; }
        }


        public PageControl()
        {
        }

        protected override void OnTemplateApplied(TemplateAppliedEventArgs e)
        {
            base.OnTemplateApplied(e);

            _presenter = e.NameScope.Find<ContentPresenter>("presenter");
        }
    }
}
