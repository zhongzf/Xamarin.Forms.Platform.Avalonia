using Avalonia.Controls;
using Avalonia.Input;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms.Internals;

namespace Xamarin.Forms.Platform.Avalonia
{
	public class TextCellRenderer : ICellRenderer
	{
		public virtual global::Avalonia.Markup.Xaml.Templates.DataTemplate GetTemplate(Cell cell)
		{
			var textCell = (global::Avalonia.Markup.Xaml.Templates.DataTemplate)global::Avalonia.Application.Current.Resources["TextCell"];
			if (cell.RealParent is ListView)
			{
				if (cell.GetIsGroupHeader<ItemsView<Cell>, Cell>())
				{
					return (global::Avalonia.Markup.Xaml.Templates.DataTemplate)global::Avalonia.Application.Current.Resources["ListViewHeaderTextCell"];
				}

				if (global::Avalonia.Application.Current.Resources.ContainsKey("ListViewTextCell"))
				{
					return (global::Avalonia.Markup.Xaml.Templates.DataTemplate)global::Avalonia.Application.Current.Resources["ListViewTextCell"];
				}
			}

			return textCell;
		}
	}

	public class EntryCellRendererCompleted : ICommand
	{
		public bool CanExecute(object parameter)
		{
			return true;
		}

		public event EventHandler CanExecuteChanged;

		public void Execute(object parameter)
		{
			var entryCell = (IEntryCellController)parameter;
			entryCell.SendCompleted();
		}

		protected virtual void OnCanExecuteChanged()
		{
			CanExecuteChanged?.Invoke(this, EventArgs.Empty);
		}
	}

	public class EntryCellPhoneTextBox : TextBox
	{
		public event EventHandler KeyboardReturnPressed;

		protected override void OnKeyUp(KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				EventHandler handler = KeyboardReturnPressed;
				if (handler != null)
					handler(this, EventArgs.Empty);
			}
			base.OnKeyUp(e);
		}
	}

	public class EntryCellRenderer : ICellRenderer
	{
		public virtual global::Avalonia.Markup.Xaml.Templates.DataTemplate GetTemplate(Cell cell)
		{
			return (global::Avalonia.Markup.Xaml.Templates.DataTemplate)global::Avalonia.Application.Current.Resources["EntryCell"];
		}
	}

	public class ViewCellRenderer : ICellRenderer
	{
		public virtual global::Avalonia.Markup.Xaml.Templates.DataTemplate GetTemplate(Cell cell)
		{
			return (global::Avalonia.Markup.Xaml.Templates.DataTemplate)global::Avalonia.Application.Current.Resources["ViewCell"];
		}
	}

	public class SwitchCellRenderer : ICellRenderer
	{
		public virtual global::Avalonia.Markup.Xaml.Templates.DataTemplate GetTemplate(Cell cell)
		{
			return (global::Avalonia.Markup.Xaml.Templates.DataTemplate)global::Avalonia.Application.Current.Resources["SwitchCell"];
		}
	}

	public class ImageCellRenderer : ICellRenderer
	{
		public virtual global::Avalonia.Markup.Xaml.Templates.DataTemplate GetTemplate(Cell cell)
		{
			return (global::Avalonia.Markup.Xaml.Templates.DataTemplate)global::Avalonia.Application.Current.Resources["ImageCell"];
		}
	}
}
