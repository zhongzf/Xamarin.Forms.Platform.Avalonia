using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Styling;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms.Internals;

namespace Xamarin.Forms.Platform.Avalonia.Controls
{
    public class CellControl : AvaloniaForms.Controls.ContentControl
    {
        public static readonly StyledProperty<object> CellProperty = AvaloniaProperty.Register<CellControl, object>(nameof(Cell));
        public static readonly StyledProperty<bool> IsGroupHeaderProperty = AvaloniaProperty.Register<CellControl, bool>(nameof(IsGroupHeader));

        static CellControl()
        {
            CellProperty.Changed.AddClassHandler<CellControl>((x, e) => x.SetSource(e.OldValue as Cell, e.NewValue as Cell));
        }

        internal static readonly BindableProperty MeasuredEstimateProperty = BindableProperty.Create("MeasuredEstimate", typeof(double), typeof(ListView), -1d);
        readonly Lazy<ListView> _listView;
        readonly PropertyChangedEventHandler _propertyChangedHandler;
        Brush _defaultOnColor;

        IList<MenuItem> _contextActions;
        global::Avalonia.Markup.Xaml.Templates.DataTemplate _currentTemplate;
        bool _isListViewRealized;
        object _newValue;

        public Cell Cell
        {
            get { return (Cell)GetValue(CellProperty); }
            set { SetValue(CellProperty, value); }
        }

        public bool IsGroupHeader
        {
            get { return (bool)GetValue(IsGroupHeaderProperty); }
            set { SetValue(IsGroupHeaderProperty, value); }
        }

        protected Control CellContent
        {
            get { return (Control)Content; }
        }

        public CellControl()
        {
            _listView = new Lazy<ListView>(GetListView);

            DataContextChanged += OnDataContextChanged;

            Unloaded += (sender, args) =>
            {
                Cell?.SendDisappearing();
            };

            _propertyChangedHandler = OnCellPropertyChanged;
        }

        void SetSource(Cell oldCell, Cell newCell)
        {
            if (oldCell != null)
            {
                oldCell.PropertyChanged -= _propertyChangedHandler;
                oldCell.SendDisappearing();
            }

            if (newCell != null)
            {
                newCell.SendAppearing();

                UpdateContent(newCell);

                newCell.PropertyChanged += _propertyChangedHandler;
            }
        }

        global::Avalonia.Markup.Xaml.Templates.DataTemplate GetTemplate(Cell cell)
        {
            var renderer = Registrar.Registered.GetHandlerForObject<ICellRenderer>(cell);
            return renderer.GetTemplate(cell);
        }
        
        void UpdateContent(Cell newCell)
        {
            var template = GetTemplate(newCell);
            if (template != _currentTemplate || Content == null)
            {
                _currentTemplate = template;
                ContentTemplate = template;
                Content = newCell;
            }
        }

        ListView GetListView()
        {
            //var parent = VisualTreeHelper.GetParent(this);
            //while (parent != null)
            //{
            //    var lv = parent as ListViewRenderer;
            //    if (lv != null)
            //    {
            //        _isListViewRealized = true;
            //        return lv.Element;
            //    }

            //    parent = VisualTreeHelper.GetParent(parent);
            //}

            return null;
        }

        void OnDataContextChanged(object sender, EventArgs args)
        {
            var newValue = DataContext;
            if (newValue == null)
                return;

            // We don't want to set the Cell until the ListView is realized, just in case the 
            // Cell has an ItemTemplate. Instead, we'll store the new data item, and it will be
            // set on MeasureOverrideDelegate. However, if the parent is a TableView, we'll already 
            // have a complete Cell object to work with, so we can move ahead.
            if (_isListViewRealized || newValue is Cell)
            {
                SetCell(newValue);
            }
            else if (newValue != null)
            {
                _newValue = newValue;
            }
        }

        void SetCell(object newContext)
        {
            var cell = newContext as Cell;

            if (cell != null)
            {
                Cell = cell;
                return;
            }

            if (ReferenceEquals(Cell?.BindingContext, newContext))
                return;

            Cell = cell;
        }

        void OnCellPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
        }

        protected override global::Avalonia.Size MeasureOverride(global::Avalonia.Size availableSize)
        {
            ListView lv = _listView.Value;

            // set the Cell now that we have a reference to the ListView, since it will have been skipped
            // on DataContextChanged.
            if (_newValue != null)
            {
                SetCell(_newValue);
                _newValue = null;
            }

            if (Content == null)
            {
                if (lv != null)
                {
                    if (lv.HasUnevenRows)
                    {
                        var estimate = (double)lv.GetValue(MeasuredEstimateProperty);
                        if (estimate > -1)
                            return new global::Avalonia.Size(availableSize.Width, estimate);
                    }
                    else
                    {
                        double rowHeight = lv.RowHeight;
                        if (rowHeight > -1)
                            return new global::Avalonia.Size(availableSize.Width, rowHeight);
                    }
                }

                // This needs to return a size with a non-zero height; 
                // otherwise, it kills virtualization.
                return new global::Avalonia.Size(0, Cell.DefaultCellHeight);
            }

            // Children still need measure called on them
            var result = base.MeasureOverride(availableSize);

            if (lv != null)
            {
                lv.SetValue(MeasuredEstimateProperty, result.Height);
            }

            return result;
        }
    }
}
