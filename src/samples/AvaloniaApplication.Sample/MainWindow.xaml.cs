using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Xamarin.Forms.Platform.Avalonia;

namespace AvaloniaApplication.Sample
{
    public class MainWindow : FormsApplicationPage
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif

            Xamarin.Forms.Forms.Init();
            LoadApplication(new FormsGallery.App());
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
