﻿using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalonia.Forms.Controls
{
    public class RoundButton : Avalonia.Controls.Button
    {
		public static readonly StyledProperty<int> CornerRadiusProperty = AvaloniaProperty.Register<RoundButton, int>(nameof(CornerRadius));

		static RoundButton()
		{
			CornerRadiusProperty.Changed.AddClassHandler<RoundButton>((x, e) => x.OnCornerRadiusPropertyChanged(e));
		}

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