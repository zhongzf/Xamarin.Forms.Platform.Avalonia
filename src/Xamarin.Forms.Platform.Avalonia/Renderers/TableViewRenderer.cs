using System;
using System.Collections.Generic;
using System.Text;

namespace Xamarin.Forms.Platform.Avalonia
{
    public class TableViewRenderer : ViewRenderer<TableView, AvaloniaForms.Controls.ListView>
    {
		bool _ignoreSelectionEvent;
		bool _disposed;

		public override SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
		{
			SizeRequest result = base.GetDesiredSize(widthConstraint, heightConstraint);
			result.Minimum = new Size(40, 40);
			return result;
		}

		protected override void OnElementChanged(ElementChangedEventArgs<TableView> e)
		{
			if (e.OldElement != null)
			{
				e.OldElement.ModelChanged -= OnModelChanged;
			}

			if (e.NewElement != null)
			{
				if (Control == null)
				{
					SetNativeControl(new AvaloniaForms.Controls.ListView
					{
						ItemTemplate = (global::Avalonia.Markup.Xaml.Templates.DataTemplate)global::Avalonia.Application.Current.Resources["CellTemplate"],
					});

					Control.SelectionChanged += OnSelectionChanged;
				}

				e.NewElement.ModelChanged += OnModelChanged;
				OnModelChanged(e.NewElement, EventArgs.Empty);
			}

			base.OnElementChanged(e);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && !_disposed)
			{
				_disposed = true;
				if (Control != null)
				{
					Control.SelectionChanged -= OnSelectionChanged;
				}
			}
			base.Dispose(disposing);
		}

		void OnModelChanged(object sender, EventArgs e)
		{
			// This auto-selects the first item in the new DataContext, so we just null it and ignore the selection
			// as this selection isn't driven by user input
			_ignoreSelectionEvent = true;
			Control.DataContext = Element.Root;
			_ignoreSelectionEvent = false;
		}

		void OnSelectionChanged(object sender, global::Avalonia.Controls.SelectionChangedEventArgs e)
		{
			if (!_ignoreSelectionEvent)
			{
				foreach (object item in e.AddedItems)
				{
					if (item is Cell cell)
					{
						if (cell.IsEnabled)
							Element.Model.RowSelected(cell);
						break;
					}
				}
			}

			if (Control == null)
				return;

			Control.SelectedItem = null;
		}
	}
}
