using Avalonia;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms.Platform.Avalonia.Controls;

namespace Xamarin.Forms.Platform.Avalonia
{
    public class FormsApplicationPage : FormsWindow
    {
        protected Application Application { get; private set; }
        protected Platform Platform { get; private set; }

        public FormsApplicationPage()
        {
            //System.Windows.Application.Current.Startup += OnLaunching;
            //System.Windows.Application.Current.Exit += OnClosing;

            //MessagingCenter.Send(this, WPFDeviceInfo.BWPorientationChangedName, this.ToDeviceOrientation());
            this.LayoutUpdated += FormsApplicationPage_LayoutUpdated;

			this.ContentLoader = new FormsContentLoader();
        }

        private void FormsApplicationPage_LayoutUpdated(object sender, EventArgs e)
        {
            //this.ContentLoader.OnSizeContentChanged(this, StartupPage);
        }

        public void LoadApplication(Application application)
        {
            Application.Current = application;
            application.PropertyChanged += ApplicationOnPropertyChanged;
            Application = application;

            // Hack around the fact that OnLaunching will have already happened by this point, sad but needed.
            application.SendStart();
        }

        void ApplicationOnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
			if (args.PropertyName == "MainPage" && IsInitialized)
			{
				SetMainPage();
			}
        }

        void SetMainPage()
        {
            if (Platform == null)
                Platform = new Platform(this);

            if (Application != null && Application.MainPage != null)
            {
                Platform.SetPage(Application.MainPage);
            }
        }

		void OnActivated(object sender, System.EventArgs e)
		{
			// TODO : figure out consistency of get this to fire
			// Check whether tombstoned (terminated, but OS retains information about navigation state and state dictionarys) or dormant
			Application.SendResume();
		}

		// when app gets tombstoned, user press back past first page
		//void OnClosing(object sender, ExitEventArgs e)
		//{
		//	Application.SendSleep();
		//}

		void OnDeactivated(object sender, System.EventArgs e)
		{
			Application.SendSleep();
		}

		//void OnLaunching(object sender, StartupEventArgs e)
		//{
		//	Application.SendStart();
		//}

		//void OnOrientationChanged(object sender, SizeChangedEventArgs e)
		//{
		//	MessagingCenter.Send(this, WPFDeviceInfo.BWPorientationChangedName, this.ToDeviceOrientation());
		//}

		protected override void Appearing()
		{
			base.Appearing();
			SetMainPage();
		}

		//public override void OnBackSystemButtonPressed()
		//{
		//	Application.NavigationProxy.PopModalAsync();
		//}
	}
}
