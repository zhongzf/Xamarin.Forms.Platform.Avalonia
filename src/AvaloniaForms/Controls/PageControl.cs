using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using AvaloniaForms.Helpers;
using AvaloniaForms.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaForms.Controls
{
    public partial class PageControl : ContentControl, IToolbarProvider, ITitleViewRendererController
    {
        public static readonly StyledProperty<bool> TitleVisibilityProperty = AvaloniaProperty.Register<PageControl, bool>(nameof(TitleVisibility));
        public static readonly StyledProperty<Brush> ToolbarBackgroundProperty = AvaloniaProperty.Register<PageControl, Brush>(nameof(ToolbarBackground));
        public static readonly StyledProperty<string> BackButtonTitleProperty = AvaloniaProperty.Register<PageControl, string>(nameof(BackButtonTitle));
        public static readonly StyledProperty<Thickness> ContentMarginProperty = AvaloniaProperty.Register<PageControl, Thickness>(nameof(ContentMargin));
        public static readonly StyledProperty<Bitmap> TitleIconProperty = AvaloniaProperty.Register<PageControl, Bitmap>(nameof(TitleIcon));
        public static readonly StyledProperty<object> TitleViewContentProperty = AvaloniaProperty.Register<PageControl, object>(nameof(TitleViewContent));
        public static readonly StyledProperty<bool> TitleViewVisibilityProperty = AvaloniaProperty.Register<PageControl, bool>(nameof(TitleViewVisibility));
        public static readonly StyledProperty<double> TitleInsetProperty = AvaloniaProperty.Register<PageControl, double>(nameof(TitleInset));
        public static readonly StyledProperty<Brush> TitleBrushProperty = AvaloniaProperty.Register<PageControl, Brush>(nameof(TitleBrush));

        static PageControl()
        {
        }

        protected CommandBar _commandBar;
        public CommandBar CommandBar => _commandBar;

        protected Control _titleViewPresenter;
        public Control TitleViewPresenter => _titleViewPresenter;

        ToolbarPlacement _toolbarPlacement;
        bool _toolbarDynamicOverflowEnabled = true;
        readonly ToolbarPlacementHelper _toolbarPlacementHelper = new ToolbarPlacementHelper();

        public bool ShouldShowToolbar
        {
            get { return _toolbarPlacementHelper.ShouldShowToolBar; }
            set { _toolbarPlacementHelper.ShouldShowToolBar = value; }
        }

        public Bitmap TitleIcon
        {
            get { return GetValue(TitleIconProperty); }
            set { SetValue(TitleIconProperty, value); }
        }

        public object TitleViewContent
        {
            get { return GetValue(TitleViewContentProperty); }
            set { SetValue(TitleViewContentProperty, value); }
        }

        TaskCompletionSource<CommandBar> _commandBarTcs;
        ContentPresenter _presenter;
        TitleViewManager _titleViewManager;

        public PageControl()
        {
            LayoutUpdated += OnLayoutUpdated;
        }

        #region Loaded & Unloaded
        public event EventHandler<EventArgs> Loaded;
        public event EventHandler<EventArgs> Unloaded;

        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            OnLoaded(e);
            Appearing();
        }

        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
            OnUnloaded(e);
            Disappearing();
        }

        protected virtual void OnLoaded(EventArgs e) { Loaded?.Invoke(this, e); }
        protected virtual void OnUnloaded(EventArgs e) { Unloaded?.Invoke(this, e); }
        #endregion

        #region Appearing & Disappearing
        protected virtual void Appearing()
        {
        }

        protected virtual void Disappearing()
        {
        }
        #endregion

        #region LayoutUpdated & SizeChanged
        public event EventHandler<EventArgs> SizeChanged;
        protected virtual void OnSizeChanged(EventArgs e) { SizeChanged?.Invoke(this, e); }

        protected virtual void OnLayoutUpdated(object sender, EventArgs e)
        {
            OnSizeChanged(e);
        }
        #endregion

        public string BackButtonTitle
        {
            get { return (string)GetValue(BackButtonTitleProperty); }
            set { SetValue(BackButtonTitleProperty, value); }
        }

        public double ContentHeight
        {
            get { return _presenter != null ? _presenter.Height : 0; }
        }

        public Thickness ContentMargin
        {
            get { return GetValue(ContentMarginProperty); }
            set { SetValue(ContentMarginProperty, value); }
        }

        public double ContentWidth
        {
            get { return _presenter != null ? _presenter.Width : 0; }
        }

        public Brush ToolbarBackground
        {
            get { return (Brush)GetValue(ToolbarBackgroundProperty); }
            set { SetValue(ToolbarBackgroundProperty, value); }
        }

        public ToolbarPlacement ToolbarPlacement
        {
            get { return _toolbarPlacement; }
            set
            {
                _toolbarPlacement = value;
                _toolbarPlacementHelper.UpdateToolbarPlacement();
            }
        }

        public bool ToolbarDynamicOverflowEnabled
        {
            get { return _toolbarDynamicOverflowEnabled; }
            set
            {
                _toolbarDynamicOverflowEnabled = value;
                UpdateToolbarDynamicOverflowEnabled();
            }
        }

        public bool TitleVisibility
        {
            get { return GetValue(TitleVisibilityProperty); }
            set { SetValue(TitleVisibilityProperty, value); }
        }

        public bool TitleViewVisibility
        {
            get { return GetValue(TitleViewVisibilityProperty); }
            set { SetValue(TitleViewVisibilityProperty, value); }
        }

        public Brush TitleBrush
        {
            get { return (Brush)GetValue(TitleBrushProperty); }
            set { SetValue(TitleBrushProperty, value); }
        }

        public double TitleInset
        {
            get { return (double)GetValue(TitleInsetProperty); }
            set { SetValue(TitleInsetProperty, value); }
        }

        protected override void OnTemplateApplied(TemplateAppliedEventArgs e)
        {
            base.OnTemplateApplied(e);

            _presenter = e.NameScope.Find<ContentPresenter>("presenter");

            _titleViewPresenter = e.NameScope.Find<Control>("TitleViewPresenter");

            _commandBar = e.NameScope.Find<CommandBar>("CommandBar");

            _titleViewManager = new TitleViewManager(this);

            _toolbarPlacementHelper.Initialize(_commandBar, () => ToolbarPlacement, name => e.NameScope.Find(name) as AvaloniaObject);
            UpdateToolbarDynamicOverflowEnabled();

            TaskCompletionSource<CommandBar> tcs = _commandBarTcs;
            tcs?.SetResult(_commandBar);
        }

        void UpdateToolbarDynamicOverflowEnabled()
        {
            if (_commandBar != null)
            {
                _commandBar.IsDynamicOverflowEnabled = ToolbarDynamicOverflowEnabled;
            }
        }

        public Task<CommandBar> GetCommandBarAsync()
        {
            if (_commandBar != null)
                return Task.FromResult(_commandBar);

            _commandBarTcs = new TaskCompletionSource<CommandBar>();
            ApplyTemplate();
            return _commandBarTcs.Task;
        }
    }
}
