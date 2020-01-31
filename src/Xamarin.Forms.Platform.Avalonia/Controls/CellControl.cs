using Avalonia;
using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Internals;

namespace Xamarin.Forms.Platform.Avalonia.Controls
{
	public class CellControl : ContentControl
	{
		public static readonly StyledProperty<object> CellProperty = AvaloniaProperty.Register<CellControl, object>(nameof(Cell));
		public static readonly StyledProperty<bool> ShowContextActionsProperty = AvaloniaProperty.Register<CellControl, bool>(nameof(ShowContextActions), true);

		readonly PropertyChangedEventHandler _propertyChangedHandler;

		public CellControl()
		{
			CellProperty.Changed.AddClassHandler<CellControl>((x, e) => x.SetSource(e.OldValue, e.NewValue));

			DetachedFromVisualTree += (sender, args) =>
			{
				ICellController cell = DataContext as ICellController;
				if (cell != null)
					cell.SendDisappearing();
			};

			LayoutUpdated += CellControl_LayoutUpdated;

			_propertyChangedHandler = OnCellPropertyChanged;
		}

		private void CellControl_LayoutUpdated(object sender, EventArgs e)
		{
			OnRenderSizeChanged(this.Bounds.Size);
		}

		public Cell Cell
		{
			get { return (Cell)GetValue(CellProperty); }
			set { SetValue(CellProperty, value); }
		}

		public bool ShowContextActions
		{
			get { return (bool)GetValue(ShowContextActionsProperty); }
			set { SetValue(ShowContextActionsProperty, value); }
		}

		global::Avalonia.Markup.Xaml.Templates.DataTemplate GetTemplate(Cell cell)
		{
			var renderer = Registrar.Registered.GetHandlerForObject<ICellRenderer>(cell);
			return renderer.GetTemplate(cell);
		}

		void OnCellPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "HasContextActions")
				SetupContextMenu();
		}

		void SetSource(object oldCellObj, object newCellObj)
		{
			var oldCell = oldCellObj as Cell;
			var newCell = newCellObj as Cell;

			if (oldCell != null)
			{
				oldCell.PropertyChanged -= _propertyChangedHandler;
				((ICellController)oldCell).SendDisappearing();
			}

			if (newCell != null)
			{
				((ICellController)newCell).SendAppearing();

				if (oldCell == null || oldCell.GetType() != newCell.GetType())
					ContentTemplate = GetTemplate(newCell);

				Content = newCell;

				SetupContextMenu();

				newCell.PropertyChanged += _propertyChangedHandler;
			}
			else
				Content = null;
		}

		protected virtual void OnRenderSizeChanged(global::Avalonia.Size newSize)
		{
			if (Content is ViewCell vc)
			{
				if (vc.LogicalChildren != null && vc.LogicalChildren.Any())
				{
					foreach (var child in vc.LogicalChildren)
					{
						if (child is Layout layout)
						{
							if (layout.HorizontalOptions.Expands)
							{
								layout.Layout(new Rectangle(layout.X, layout.Y, newSize.Width, newSize.Height));
							}
						}
					}
				}
			}
		}

		void SetupContextMenu()
		{
			if (Content == null || !ShowContextActions)
				return;

			//if (!Cell.HasContextActions)
			//{
			//	ContextMenuService.SetContextMenu(this, null);
			//	return;
			//}

			ApplyTemplate();

			//ContextMenu menu = new CustomContextMenu();
			//menu.SetBinding(ItemsControl.ItemsSourceProperty, new System.Windows.Data.Binding("ContextActions"));

			//ContextMenuService.SetContextMenu(this, menu);
		}
	}
}