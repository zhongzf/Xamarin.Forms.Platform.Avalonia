# Xamarin.Forms.Platform.Avalonia
Xamarin Forms platform implemented with Avalonia (A multi-platform .NET UI framework)


## Xamarin.Forms

Xamarin.Forms provides a way to quickly build native apps for iOS, Android, Windows and macOS, completely in C#.

Read more about the platform at https://www.xamarin.com/forms.

## Avalonia

Avalonia is a WPF-inspired cross-platform XAML-based UI framework providing a flexible styling system and supporting a wide range of OSs: Windows (.NET Framework, .NET Core), Linux (GTK), MacOS, Android and iOS.

Read more about the framework at https://avaloniaui.net.


## Getting Started

1. Creating a new Avalonia project, read more about it at http://avaloniaui.net/docs/quickstart/create-new-project.

2. You can find the packages here [NuGet](https://www.nuget.org/packages/Xamarin.Forms.Platform.Avalonia/) and install the package like this:
```
Install-Package Xamarin.Forms.Platform.Avalonia
```
3. Build a Xamarin.Forms App, read more about it at https://docs.microsoft.com/en-us/xamarin/get-started/first-app.
4. Add the new created Xamarin.Forms App project as project reference to your new created Avalonia project.
5. Edit MainWindow.xaml.cs, add using:
```cs
using Xamarin.Forms.Platform.Avalonia;
```
6. Change the base class of MainWindow to **FormsApplicationPage**, and add **Forms.Init()** and **LoadApplication()** method call, '**FormsGallery.App**' in the code should be the name of your Xamarin.Forms App:
```cs
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
```

## Screenshot

<img width='300' src='https://raw.githubusercontent.com/zhongzf/Xamarin.Forms.Platform.Avalonia/develop/doc/images/screenshot.png'>