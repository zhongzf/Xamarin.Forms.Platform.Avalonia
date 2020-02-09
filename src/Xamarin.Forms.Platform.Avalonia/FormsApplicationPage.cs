using Avalonia;
using Avalonia.Markup.Xaml.Styling;
using AvaloniaForms.Controls;
using AvaloniaForms.Themes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms.Platform.Avalonia.Controls;

namespace Xamarin.Forms.Platform.Avalonia
{
    public class FormsApplicationPage : ApplicationWindow
    {
        protected Application Application { get; private set; }
        protected Platform Platform { get; private set; }

        public FormsApplicationPage(Application application = null)
        {
            IncludeDefaultTheme();

            this.DataContext = this;
            //System.Windows.Application.Current.Startup += OnLaunching;
            //System.Windows.Application.Current.Exit += OnClosing;

            this.ContentLoader = new FormsContentLoader();

            if (application != null)
            {
                LoadApplication(application);
            }
            else if (Application.Current != null)
            {
                LoadApplication(Application.Current);
            }
        }

        public static bool DefaultThemeIncluded { get; private set; } = false;

        private static void IncludeDefaultTheme()
        {
            if (!DefaultThemeIncluded)
            {
                var baseUri = new Uri(string.Format("avares://{0}/{1}.xaml", Assembly.GetEntryAssembly().GetName().Name, global::Avalonia.Application.Current.GetType().Name));
                var sourceUri = new Uri(string.Format("resm:{1}.DefaultTheme.xaml?assembly={0}", typeof(DefaultTheme).Assembly.GetName().Name, typeof(DefaultTheme).Namespace));
                global::Avalonia.Application.Current.Styles.Add(new StyleInclude(baseUri) { Source = sourceUri });
                DefaultThemeIncluded = true;
            }
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
            {
                Platform = new AvaloniaPlatform(this);
            }
            if (Application == null)
            {
                Application = Application.Current;
            }

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
