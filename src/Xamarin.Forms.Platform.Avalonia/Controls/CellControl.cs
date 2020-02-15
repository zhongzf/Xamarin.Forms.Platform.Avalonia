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
    public class CellControl : ContentControl, IStyleable
    {
        public static readonly StyledProperty<object> CellProperty = AvaloniaProperty.Register<CellControl, object>(nameof(Cell));
        public static readonly StyledProperty<bool> IsGroupHeaderProperty = AvaloniaProperty.Register<CellControl, bool>(nameof(IsGroupHeader));

        static CellControl()
        {
            CellProperty.Changed.AddClassHandler<CellControl>((x, e) => x.SetSource(e.OldValue as Cell, e.NewValue as Cell));
        }

        Type IStyleable.StyleKey => typeof(ContentControl);

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
                Content = template.Content;
            }
            ((Control)Content).DataContext = newCell;
        }
    }
}
