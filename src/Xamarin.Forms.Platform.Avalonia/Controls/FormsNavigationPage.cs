using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Forms.Controls;
using Avalonia.Styling;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Xamarin.Forms.Platform.Avalonia.Extensions;

namespace Xamarin.Forms.Platform.Avalonia.Controls
{
    public class FormsNavigationPage : global::Avalonia.Forms.Controls.NavigationPage
    {
        NavigationPage NavigationPage;

        public FormsNavigationPage(NavigationPage navigationPage)
        {
            ContentLoader = new FormsContentLoader();
            NavigationPage = navigationPage;
        }

        public override void OnBackButtonPressed()
        {
            if (!NavigationPage.CurrentPage?.SendBackButtonPressed() ?? false)
            {
                NavigationPage.PopAsync();
            }
        }        
    }
}
