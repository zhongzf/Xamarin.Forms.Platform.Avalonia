using System;
using System.Collections.Generic;
using System.Text;

namespace Xamarin.Forms.Platform.Avalonia
{
	public abstract partial class AvaloniaBasePage : AvaloniaForms.Controls.Page
	{
		Application _application;

		internal Platform Platform { get; private set; }

		public AvaloniaBasePage()
		{
		}

		public void LoadApplication(Application application)
		{
			if (application == null)
				throw new ArgumentNullException("application");

			_application = application;
			Application.SetCurrentApplication(application);

			if(_application.MainPage != null)
			{
				RegisterWindow(_application.MainPage);
			}

			_application.SendStart();
		}	

		protected abstract Platform CreatePlatform();

		protected virtual void RegisterWindow(Page page)
		{
			if (page == null)
				throw new ArgumentNullException("page");

			Platform = CreatePlatform();
			Platform.SetPage(page);
		}
	}
}
