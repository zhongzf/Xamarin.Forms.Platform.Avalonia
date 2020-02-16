using Avalonia.Interactivity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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

            if (_application.MainPage != null)
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

        protected override void OnPreviousButtonClick(object sender, RoutedEventArgs e)
        {
            if (Platform.CurrentPage is NavigationPage navigationPage)
            {
                navigationPage.Navigation.PopAsync().GetAwaiter().GetResult();
            }
        }
    }
}
