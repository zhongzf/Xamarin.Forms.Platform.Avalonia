using Avalonia.Markup.Xaml.Styling;
using AvaloniaForms;
using AvaloniaForms.Themes;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Xamarin.Forms.Platform.Avalonia
{
    public class FormsApplicationPage : ApplicationWindow
    {
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

        public AvaloniaPage ApplicationPage { get; private set; }

        public FormsApplicationPage()
        {
            IncludeDefaultTheme();
        }

        protected void LoadApplication(Application application)
        {
            ApplicationPage = new AvaloniaPage();
            this.Content = ApplicationPage;
            ApplicationPage.LoadApplication(application);
        }
    }
}
