using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Styling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AvaloniaForms.Controls
{
	public partial class CommandBar : AppBar, IStyleable
	{
		public static readonly StyledProperty<IEnumerable<Control>> PrimaryCommandsProperty = AvaloniaProperty.Register<CommandBar, IEnumerable<Control>>(nameof(PrimaryCommands));
		public static readonly StyledProperty<IEnumerable<Control>> SecondaryCommandsProperty = AvaloniaProperty.Register<CommandBar, IEnumerable<Control>>(nameof(SecondaryCommands));
		public static readonly StyledProperty<bool> IsDynamicOverflowEnabledProperty = AvaloniaProperty.Register<CommandBar, bool>(nameof(IsDynamicOverflowEnabled));

		static CommandBar()
		{
		}

		Type IStyleable.StyleKey => typeof(CommandBar);

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

		public bool IsDynamicOverflowEnabled
		{
			get { return GetValue(IsDynamicOverflowEnabledProperty); }
			set { SetValue(IsDynamicOverflowEnabledProperty, value); }
		}

		Button _moreButton;
		ItemsControl _primaryItemsControl;
		bool _isInValidLocation;

		// Set by the container if the container is a valid place to show a toolbar.
		// This exists to provide consistency with the other platforms; we've got 
		// rules in place that limit toolbars to Navigation Page and to Tabbed 
		// and Master-Detail Pages when they're currently displaying a Navigation Page
		public bool IsInValidLocation
		{
			get { return _isInValidLocation; }
			set
			{
				_isInValidLocation = value;
				UpdateVisibility();
			}
		}


		public CommandBar()
		{
			//UpdateVisibility();
		}

		protected override void OnTemplateApplied(TemplateAppliedEventArgs e)
		{
			base.OnTemplateApplied(e);

			_moreButton = e.NameScope.Find<Button>("MoreButton");
			_primaryItemsControl = e.NameScope.Find<ItemsControl>("PrimaryItemsControl");
		}

		public void Reset()
		{
		}

		void UpdateVisibility()
		{
			// Determine whether we have a title (or some other content) inside this command bar
			var frameworkElement = Content as Control;

			// Apply the rules for consistency with other platforms

			// Not in one of the acceptable toolbar locations from the other platforms
			if (!IsInValidLocation)
			{
				// If there's no title to display (e.g., toolbarplacement is set to bottom)
				// or the title is collapsed (e.g., because it's empty)
				if (frameworkElement == null || !frameworkElement.IsVisible)
				{
					// Just collapse the whole thing
					IsVisible = false;
					return;
				}

				// The title needs to be visible, but we're not allowed to show a toolbar
				// So we need to hide the toolbar items

				IsVisible = true;

				if (_moreButton != null)
				{
					_moreButton.IsVisible = false;
				}

				if (_primaryItemsControl != null)
				{
					_primaryItemsControl.IsVisible = false;
				}

				return;
			}

			// We're in one of the acceptable toolbar locations from the other platforms so the normal rules apply

			if (_primaryItemsControl != null)
			{
				// This is normally visible by default, but it might have been collapsed by the toolbar consistency rules above
				_primaryItemsControl.IsVisible = true;
			}

			// Are there any commands to display?
			var visibility = PrimaryCommands.Count() + SecondaryCommands.Count() > 0;

			if (_moreButton != null)
			{
				// The "..." button should only be visible if we have commands to display
				_moreButton.IsVisible = visibility;

				// There *is* an OverflowButtonVisibility property that does more or less the same thing, 
				// but it became available in 10.0.14393.0 and we have to support 10.0.10240
			}

			if (frameworkElement != null && frameworkElement.IsVisible)
			{
				// If there's a title to display, we have to be visible whether or not we have commands
				IsVisible = true;
			}
			else
			{
				// Otherwise, visibility depends on whether we have commands
				IsVisible = visibility;
			}
		}
	}
}
