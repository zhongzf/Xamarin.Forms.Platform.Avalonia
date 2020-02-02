using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Platform.Avalonia.Extensions;
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

		protected override void OnTemplateApplied(TemplateAppliedEventArgs e)
		{
			base.OnTemplateApplied(e);

			close = this.Find<global::Avalonia.Controls.Button>("PART_Close", e);
			if (close != null)
			{
				close.Click += CloseClick;
			}

			max = this.Find<global::Avalonia.Controls.Button>("PART_Max", e);
			if (max != null)
			{
				max.Click += MaximizeClick;
			}

			min = this.Find<global::Avalonia.Controls.Button>("PART_Min", e);
			if (min != null)
			{
				min.Click += MinimizeClick;
			}
			this.ParentWindow = this.TryFindParent<FormsWindow>();
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
