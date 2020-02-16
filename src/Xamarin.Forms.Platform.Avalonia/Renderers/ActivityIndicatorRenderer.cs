using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.Forms.Platform.Avalonia
{
	public class ActivityIndicatorRenderer : ViewRenderer<ActivityIndicator, AvaloniaForms.Controls.ProgressBar>
	{
		protected override void OnElementChanged(ElementChangedEventArgs<ActivityIndicator> e)
		{
			if (e.NewElement != null)
			{
				if (Control == null) // construct and SetNativeControl and suscribe control event
				{
					SetNativeControl(new AvaloniaForms.Controls.ProgressBar());
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
			Control.UpdateDependencyColor(AvaloniaForms.Controls.ProgressBar.ForegroundProperty, Element.Color);
		}

		void UpdateIsIndeterminate()
		{
			Control.IsIndeterminate = Element.IsRunning;
		}
	}
}
