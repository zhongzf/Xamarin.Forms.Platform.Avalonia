using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using System;
using System.ComponentModel;
using System.Windows.Input;

namespace AvaloniaForms
{
	internal class MenuItemCommand : ICommand
	{
		readonly MenuItem _menuItem;

		public MenuItemCommand(MenuItem item)
		{
			_menuItem = item;
			_menuItem.PropertyChanged += OnElementPropertyChanged;
		}

		public virtual bool CanExecute(object parameter)
		{
			return _menuItem.IsEnabled;
		}

		public event EventHandler CanExecuteChanged;

		public void Execute(object parameter)
		{
			//((IMenuItemController)_menuItem).Activate();
		}

		void OnCanExecuteChanged()
		{
			EventHandler changed = CanExecuteChanged;
			if (changed != null)
				changed(this, EventArgs.Empty);
		}

		void OnElementPropertyChanged(object sender, AvaloniaPropertyChangedEventArgs e)
		{
			if (e.Property.Name == InputElement.IsEnabledProperty.Name)
				OnCanExecuteChanged();
		}
	}
}