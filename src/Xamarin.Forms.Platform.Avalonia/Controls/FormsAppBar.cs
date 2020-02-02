using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using System.Collections.Generic;
using System.Windows;
using Xamarin.Forms.Platform.Avalonia.Extensions;

namespace Xamarin.Forms.Platform.Avalonia.Controls
{
	public class FormsAppBar : ContentControl
	{
		ToggleButton btnMore;

		public static readonly StyledProperty<IEnumerable<Control>> PrimaryCommandsProperty = AvaloniaProperty.Register<FormsAppBar, IEnumerable<Control>>(nameof(PrimaryCommands));
		public static readonly StyledProperty<IEnumerable<Control>> SecondaryCommandsProperty = AvaloniaProperty.Register<FormsAppBar, IEnumerable<Control>>(nameof(SecondaryCommands));

		public IEnumerable<Control> PrimaryCommands
		{
			get { return (IEnumerable<Control>)GetValue(PrimaryCommandsProperty); }
			set { SetValue(PrimaryCommandsProperty, value); }
		}

		public IEnumerable<Control> SecondaryCommands
		{
			get { return (IEnumerable<Control>)GetValue(SecondaryCommandsProperty); }
			set { SetValue(SecondaryCommandsProperty, value); }
		}

		public FormsAppBar()
		{
			//this.DefaultStyleKey = typeof(FormsAppBar);
		}

		protected override void OnTemplateApplied(TemplateAppliedEventArgs e)
		{
			base.OnTemplateApplied(e);

			btnMore = this.Find<ToggleButton>("PART_More", e);
		}

		public void Reset()
		{
			if (btnMore != null)
			{
				btnMore.IsChecked = false;
			}
		}
	}
}
