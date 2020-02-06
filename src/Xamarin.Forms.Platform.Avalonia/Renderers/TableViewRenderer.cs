using Avalonia;
using Avalonia.Controls.Templates;
using AvaloniaForms.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Platform.Avalonia.Controls;
using AList = Avalonia.Controls.ListBox;
using ASelectionChangedEventArgs = Avalonia.Controls.SelectionChangedEventArgs;

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
				return (global::Avalonia.Markup.Xaml.Templates.DataTemplate)global::Avalonia.Application.Current.Resources["TableSectionHeader"];
			}
		}
	}

	public class TableViewRenderer : ViewRenderer<TableView, FormsListBox>
	{
		public override SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
		{
			SizeRequest result = base.GetDesiredSize(widthConstraint, heightConstraint);
			result.Minimum = new Size(40, 40);
			return result;
		}


		protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.TableView> e)
		{
			if (e.OldElement != null)
			{
				Element.ModelChanged -= OnModelChanged;
			}

			if (e.NewElement != null)
			{
				if (Control == null) // construct and SetNativeControl and suscribe control event
				{
					var listView = new FormsListBox
					{
						ItemTemplateSelector = new TableViewDataTemplateSelector(),
						//Style = (System.Windows.Style)System.Windows.Application.Current.Resources["TableViewTemplate"],
					};
					
					SetNativeControl(listView);
					Control.SelectionChanged += Control_SelectionChanged;
				}

				// Update control property 
				Control.Items = GetTableViewRow();

				// Element event
				Element.ModelChanged += OnModelChanged;
			}

			base.OnElementChanged(e);
		}

	
		private void Control_SelectionChanged(object sender, ASelectionChangedEventArgs e)
		{
			foreach (object item in e.AddedItems)
			{
				Cell cell = item as Cell;
				if (cell != null)
				{
					if (cell.IsEnabled)
						Element.Model.RowSelected(cell);
					break;
				}
			}

			Control.SelectedItem = null;
		}

		void OnModelChanged(object sender, EventArgs eventArgs)
		{
			Control.Items = GetTableViewRow();
		}

		public IList<object> GetTableViewRow()
		{
			List<object> result = new List<object>();
			
			foreach (var item in Element.Root)
			{
				if (!string.IsNullOrWhiteSpace(item.Title))
					result.Add(item);
				
				result.AddRange(item);
			}
			return result;
		}

		bool _isDisposed;

		protected override void Dispose(bool disposing)
		{
			if (_isDisposed)
				return;

			if (disposing)
			{
				if (Control != null)
				{
					Control.SelectionChanged -= Control_SelectionChanged;
				}

				if(Element != null)
				{
					Element.ModelChanged -= OnModelChanged;
				}
			}

			_isDisposed = true;
			base.Dispose(disposing);
		}
	}
}
