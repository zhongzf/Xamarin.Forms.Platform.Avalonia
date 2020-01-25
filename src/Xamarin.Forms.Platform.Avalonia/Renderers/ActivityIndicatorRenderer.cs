using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Platform.Avalonia.Extensions;
using AProgressBar = Avalonia.Controls.ProgressBar;

namespace Xamarin.Forms.Platform.Avalonia
{
	public class ActivityIndicatorRenderer : ViewRenderer<ActivityIndicator, AProgressBar>
	{
		protected override void OnElementChanged(ElementChangedEventArgs<ActivityIndicator> e)
		{
			if (e.NewElement != null)
			{
				if (Control == null) // construct and SetNativeControl and suscribe control event
				{
					SetNativeControl(new AProgressBar());
				}

				UpdateIsIndeterminate();
				UpdateColor();
			}

			base.OnElementChanged(e);
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == ActivityIndicator.IsRunningProperty.PropertyName)
				UpdateIsIndeterminate();
			else if (e.PropertyName == ActivityIndicator.ColorProperty.PropertyName)
				UpdateColor();
		}

		void UpdateColor()
		{
			Control.UpdateDependencyColor(AProgressBar.ForegroundProperty, Element.Color);
		}

		void UpdateIsIndeterminate()
		{
			Control.IsIndeterminate = Element.IsRunning;
		}
	}
}
