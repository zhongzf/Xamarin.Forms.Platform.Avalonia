using Avalonia.Controls;
using AvaloniaForms.Controls;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvaloniaForms
{
    public partial class TitleViewManager
    {
        readonly ITitleViewRendererController _titleViewRendererController;

        object TitleView => _titleViewRendererController.TitleViewContent;
        CommandBar CommandBar => _titleViewRendererController.CommandBar;
        Control TitleViewPresenter => _titleViewRendererController.TitleViewPresenter;

        public TitleViewManager(ITitleViewRendererController titleViewRendererController)
        {
            _titleViewRendererController = titleViewRendererController;
            _titleViewRendererController.TitleViewPresenter.AttachedToVisualTree += OnTitleViewPresenterLoaded;

            CommandBar.LayoutUpdated += commandLayoutUpdated;
            CommandBar.Unloaded += commandBarUnloaded;
        }

        internal void OnTitleViewPropertyChanged()
        {
            UpdateTitleViewWidth();
        }

        void OnTitleViewPresenterLoaded(object sender, EventArgs e)
        {
            UpdateTitleViewWidth();
            TitleViewPresenter.AttachedToVisualTree -= OnTitleViewPresenterLoaded;
        }

        void commandBarUnloaded(object sender, EventArgs e)
        {
            CommandBar.LayoutUpdated -= commandLayoutUpdated;
            CommandBar.Unloaded -= commandBarUnloaded;
        }

        void commandLayoutUpdated(object sender, object e)
        {
            UpdateTitleViewWidth();
        }

        void UpdateTitleViewWidth()
        {
            if (TitleView == null || TitleViewPresenter == null || CommandBar == null)
                return;

            if (CommandBar.Width <= 0) return;

            double buttonWidth = 0;
            foreach (var item in CommandBar.GetDescendantsByName<Button>("MoreButton"))
                if (item.IsVisible)
                    buttonWidth += item.Width;

            if (!CommandBar.IsDynamicOverflowEnabled)
                foreach (var item in CommandBar.GetDescendantsByName<ItemsControl>("PrimaryItemsControl"))
                    buttonWidth += item.Width;

            TitleViewPresenter.Width = CommandBar.Width - buttonWidth;
            UpdateVisibility();
        }

        void UpdateVisibility()
        {
            if (TitleView == null)
                _titleViewRendererController.TitleViewVisibility = false;
            else
                _titleViewRendererController.TitleViewVisibility = true;
        }
    }
}
