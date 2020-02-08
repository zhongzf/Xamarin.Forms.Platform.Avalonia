namespace Xamarin.Forms.Platform.Avalonia.Controls
{
    public class FormsNavigationPage : AvaloniaForms.Controls.NavigationPage
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
