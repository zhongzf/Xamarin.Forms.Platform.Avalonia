using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Styling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AvaloniaForms.Controls
{
    public class RoundButton : Button, IStyleable
    {
		public static readonly StyledProperty<int> CornerRadiusProperty = AvaloniaProperty.Register<RoundButton, int>(nameof(CornerRadius));

		static RoundButton()
		{
			CornerRadiusProperty.Changed.AddClassHandler<RoundButton>((x, e) => x.OnCornerRadiusPropertyChanged(e));
		}

		Type IStyleable.StyleKey => typeof(Button);

		public int CornerRadius
		{
			get
			{
				return (int)GetValue(CornerRadiusProperty);
			}
			set
			{
				SetValue(CornerRadiusProperty, value);
			}
		}

		public RoundButton()
		{
			LayoutUpdated += OnLayoutUpdated;
		}

		#region Loaded & Unloaded
		public event EventHandler<RoutedEventArgs> Loaded;
		public event EventHandler<RoutedEventArgs> Unloaded;

		protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
		{
			OnLoaded(new RoutedEventArgs());
			Appearing();
		}

		protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
		{
			OnUnloaded(new RoutedEventArgs());
			Disappearing();
		}

		protected virtual void OnLoaded(RoutedEventArgs e) { Loaded?.Invoke(this, e); }
		protected virtual void OnUnloaded(RoutedEventArgs e) { Unloaded?.Invoke(this, e); }
		#endregion

		#region Appearing & Disappearing
		protected virtual void Appearing()
		{
		}

		protected virtual void Disappearing()
		{
		}
		#endregion

		#region LayoutUpdated & SizeChanged
		public event EventHandler<EventArgs> SizeChanged;
		protected virtual void OnSizeChanged(EventArgs e) { SizeChanged?.Invoke(this, e); }

		protected virtual void OnLayoutUpdated(object sender, EventArgs e)
		{
			OnSizeChanged(e);
		}
		#endregion

		ContentPresenter _contentPresenter;

		protected override void OnTemplateApplied(TemplateAppliedEventArgs e)
		{
			base.OnTemplateApplied(e);
			_contentPresenter = VisualChildren.OfType<ContentPresenter>().FirstOrDefault();
			UpdateCornerRadius();
		}

		private void OnCornerRadiusPropertyChanged(AvaloniaPropertyChangedEventArgs e)
		{
			UpdateCornerRadius();
		}

		void UpdateCornerRadius()
		{
			if (_contentPresenter != null)
			{
				_contentPresenter.CornerRadius = new CornerRadius(CornerRadius);
			}
		}
	}
}
