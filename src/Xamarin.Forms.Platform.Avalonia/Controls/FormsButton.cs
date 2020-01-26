using Avalonia;
using Avalonia.Controls;
using System.Linq;
using AButton = Avalonia.Controls.Button;

namespace Xamarin.Forms.Platform.Avalonia.Controls
{
	public class FormsButton : AButton
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

		public FormsButton()
		{
			CornerRadiusProperty.Changed.AddClassHandler<FormsButton>((x, e) => x.OnCornerRadiusChanged(e));
		}

		protected override void OnMeasureInvalidated()
		{
			base.OnMeasureInvalidated();
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
