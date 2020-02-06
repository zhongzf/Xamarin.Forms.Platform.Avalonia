using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using AvaloniaForms.Helpers;
using Avalonia.Interactivity;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvaloniaForms.Controls
{
    public class WindowButtons : ContentControl
    {
		private Button min;
		private Button max;
		private Button close;

		private ApplicationWindow _parentWindow;
		public ApplicationWindow ParentWindow
		{
			get { return _parentWindow; }
			set
			{
				_parentWindow = value;
			}
		}
		
		protected override void OnTemplateApplied(TemplateAppliedEventArgs e)
		{
			base.OnTemplateApplied(e);

			close = e.NameScope.Find<Button>("PART_Close");
			if (close != null)
			{
				close.Click += CloseClick;
			}

			max = e.NameScope.Find<Button>("PART_Max");
			if (max != null)
			{
				max.Click += MaximizeClick;
			}

			min = e.NameScope.Find<Button>("PART_Min");
			if (min != null)
			{
				min.Click += MinimizeClick;
			}
			this.ParentWindow = this.TryFindParent<ApplicationWindow>();
		}

		private void MinimizeClick(object sender, RoutedEventArgs e)
		{
			if (null == this.ParentWindow) return;
			SystemCommands.MinimizeWindow(this.ParentWindow);
		}

		private void MaximizeClick(object sender, RoutedEventArgs e)
		{
			if (null == this.ParentWindow) return;
			if (this.ParentWindow.WindowState == WindowState.Maximized)
			{
				SystemCommands.RestoreWindow(this.ParentWindow);
			}
			else
			{
				SystemCommands.MaximizeWindow(this.ParentWindow);
			}
		}

		private void CloseClick(object sender, RoutedEventArgs e)
		{
			if (null == this.ParentWindow) return;
			this.ParentWindow.Close();
		}
	}
}
