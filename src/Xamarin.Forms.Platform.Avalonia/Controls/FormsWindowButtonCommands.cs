using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Platform.Avalonia.Helpers;

namespace Xamarin.Forms.Platform.Avalonia.Controls
{
	public class FormsWindowButtonCommands : ContentControl
	{
		private global::Avalonia.Controls.Button min;
		private global::Avalonia.Controls.Button max;
		private global::Avalonia.Controls.Button close;

		private FormsWindow _parentWindow;

		public FormsWindow ParentWindow
		{
			get { return _parentWindow; }
			set
			{
				_parentWindow = value;
			}
		}

		public FormsWindowButtonCommands()
		{
			//this.DefaultStyleKey = typeof(FormsWindowButtonCommands);
		}

		//public override void OnApplyTemplate()
		//{
		//	base.OnApplyTemplate();

		//	close = Template.FindName("PART_Close", this) as System.Windows.Controls.Button;
		//	if (close != null)
		//	{
		//		close.Click += CloseClick;
		//	}

		//	max = Template.FindName("PART_Max", this) as System.Windows.Controls.Button;
		//	if (max != null)
		//	{
		//		max.Click += MaximizeClick;
		//	}

		//	min = Template.FindName("PART_Min", this) as System.Windows.Controls.Button;
		//	if (min != null)
		//	{
		//		min.Click += MinimizeClick;
		//	}
		//	this.ParentWindow = this.TryFindParent<FormsWindow>();
		//}

		//private void MinimizeClick(object sender, RoutedEventArgs e)
		//{
		//	if (null == this.ParentWindow) return;
		//	Microsoft.Windows.Shell.SystemCommands.MinimizeWindow(this.ParentWindow);
		//}

		//private void MaximizeClick(object sender, RoutedEventArgs e)
		//{
		//	if (null == this.ParentWindow) return;
		//	if (this.ParentWindow.WindowState == WindowState.Maximized)
		//	{
		//		Microsoft.Windows.Shell.SystemCommands.RestoreWindow(this.ParentWindow);
		//	}
		//	else
		//	{
		//		Microsoft.Windows.Shell.SystemCommands.MaximizeWindow(this.ParentWindow);
		//	}
		//}

		//private void CloseClick(object sender, RoutedEventArgs e)
		//{
		//	if (null == this.ParentWindow) return;
		//	this.ParentWindow.Close();
		//}
	}
}
