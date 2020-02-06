using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Styling;
using System;
using System.Collections.Generic;
using System.Text;

namespace Avalonia.Forms.Controls
{
    public class AppBar : ContentControl
	{
		public static readonly StyledProperty<IEnumerable<Control>> PrimaryCommandsProperty = AvaloniaProperty.Register<AppBar, IEnumerable<Control>>(nameof(PrimaryCommands));
		public static readonly StyledProperty<IEnumerable<Control>> SecondaryCommandsProperty = AvaloniaProperty.Register<AppBar, IEnumerable<Control>>(nameof(SecondaryCommands));

		static AppBar()
		{
		}

		ToggleButton btnMore;

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

		public AppBar()
		{
		}

		protected override void OnTemplateApplied(TemplateAppliedEventArgs e)
		{
			base.OnTemplateApplied(e);

			btnMore = e.NameScope.Find<ToggleButton>("PART_More");
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
