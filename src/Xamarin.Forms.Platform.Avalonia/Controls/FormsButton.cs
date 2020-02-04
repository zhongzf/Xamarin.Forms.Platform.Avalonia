using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Styling;
using System;
using System.Linq;
using AButton = Avalonia.Controls.Button;

namespace Xamarin.Forms.Platform.Avalonia.Controls
{
	public class FormsButton : AButton, IStyleable
	{
		public static readonly StyledProperty<int> CornerRadiusProperty = AvaloniaProperty.Register<FormsButton, int>(nameof(CornerRadius));

		global::Avalonia.Controls.Presenters.ContentPresenter _contentPresenter;
		
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

		Type IStyleable.StyleKey => typeof(AButton);

		public FormsButton()
		{
			CornerRadiusProperty.Changed.AddClassHandler<FormsButton>((x, e) => x.OnCornerRadiusChanged(e));
		}

		protected override void OnTemplateApplied(TemplateAppliedEventArgs e)
		{
			base.OnTemplateApplied(e);
			_contentPresenter = VisualChildren.OfType<global::Avalonia.Controls.Presenters.ContentPresenter>().FirstOrDefault();
			UpdateCornerRadius();
		}

		void OnCornerRadiusChanged(AvaloniaPropertyChangedEventArgs e)
		{
			UpdateCornerRadius();
		}

		void UpdateCornerRadius()
		{
			if (_contentPresenter != null)
			{
				_contentPresenter.CornerRadius = new global::Avalonia.CornerRadius(CornerRadius);
			}
		}
	}
}
