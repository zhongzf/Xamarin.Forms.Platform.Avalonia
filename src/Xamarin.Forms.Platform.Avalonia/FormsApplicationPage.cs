using AvaloniaForms;
using System;
using System.Collections.Generic;
using System.Text;

namespace Xamarin.Forms.Platform.Avalonia
{
    public class FormsApplicationPage : ApplicationWindow
    {
        public AvaloniaPage ApplicationPage { get; private set; }

        public FormsApplicationPage()
        {            
        }
        
        protected void LoadApplication(Application application)
        {
            ApplicationPage = new AvaloniaPage();
            this.Content = ApplicationPage;
            ApplicationPage.LoadApplication(application);
        }
    }
}
