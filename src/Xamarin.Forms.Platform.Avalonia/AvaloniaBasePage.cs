using System;
using System.Collections.Generic;
using System.Text;

namespace Xamarin.Forms.Platform.Avalonia
{
	public abstract partial class AvaloniaBasePage : AvaloniaForms.Controls.Page
	{
		Application _application;

		public AvaloniaBasePage()
		{
		}

		public void LoadApplication(Application application)
		{
			if (application == null)
				throw new ArgumentNullException("application");

			_application = application;
			Application.SetCurrentApplication(application);

			// TODO: Resister MainPage.

			_application.SendStart();
		}	
	}
}
