using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls.Templates;
using AvaloniaForms;
using System;
using System.Collections.Generic;
using System.Text;

namespace Xamarin.Forms.Platform.Avalonia
{
	public class TableViewDataTemplateSelector : IDataTemplateSelector
	{
		public IDataTemplate SelectTemplate(object item, object container)
		{
			if (item is Cell)
			{
				return (global::Avalonia.Markup.Xaml.Templates.DataTemplate)global::Avalonia.Application.Current.Resources["CellTemplate"];
			}
			else
			{
				return (global::Avalonia.Markup.Xaml.Templates.DataTemplate)global::Avalonia.Application.Current.Resources["TableSection"];
			}
		}
	}

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
						ItemTemplateSelector = new TableViewDataTemplateSelector()
					});

					Control.Bind(AvaloniaForms.Controls.ListView.ItemsProperty, new global::Avalonia.Data.Binding { Path = "" });
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
