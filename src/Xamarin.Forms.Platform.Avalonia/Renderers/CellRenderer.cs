using Avalonia.Controls;
using Avalonia.Input;
using System;
using System.Windows.Input;
using Xamarin.Forms.Internals;

namespace Xamarin.Forms.Platform.Avalonia
{
	public class TextCellRenderer : ICellRenderer
	{
		public virtual global::Avalonia.Markup.Xaml.Templates.DataTemplate GetTemplate(Cell cell)
		{
			/*if (cell.RealParent is ListView)
			{
				if (cell.GetIsGroupHeader<ItemsView<Cell>, Cell>())
					return (System.Windows.DataTemplate)System.Windows.Application.Current.Resources["ListViewHeaderTextCell"];

				return (System.Windows.DataTemplate)System.Windows.Application.Current.Resources["ListViewTextCell"];
			}*/

			//return (global::Avalonia.Markup.Xaml.Templates.DataTemplate)System.Windows.Application.Current.Resources["TextCell"];
			// TODO:
			return null;
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
			//return (System.Windows.DataTemplate)System.Windows.Application.Current.Resources["EntryCell"];
			// TODO:
			return null;
		}
	}

	public class ViewCellRenderer : ICellRenderer
	{
		public virtual global::Avalonia.Markup.Xaml.Templates.DataTemplate GetTemplate(Cell cell)
		{
			//return (System.Windows.DataTemplate)System.Windows.Application.Current.Resources["ViewCell"];
			// TODO:
			return null;
		}
	}

	public class SwitchCellRenderer : ICellRenderer
	{
		public virtual global::Avalonia.Markup.Xaml.Templates.DataTemplate GetTemplate(Cell cell)
		{
			//return (System.Windows.DataTemplate)System.Windows.Application.Current.Resources["SwitchCell"];
			// TODO:
			return null;
		}
	}

	public class ImageCellRenderer : ICellRenderer
	{
		public virtual global::Avalonia.Markup.Xaml.Templates.DataTemplate GetTemplate(Cell cell)
		{
			//return (System.Windows.DataTemplate)System.Windows.Application.Current.Resources["ImageCell"];
			// TODO:
			return null;
		}
	}
}