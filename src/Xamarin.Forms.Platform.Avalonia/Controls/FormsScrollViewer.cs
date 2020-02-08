using System;
using Avalonia.Controls;
using Avalonia.Styling;

namespace Xamarin.Forms.Platform.Avalonia.Controls
{
    public class FormsScrollViewer : ScrollViewer, IStyleable
    {
        Type IStyleable.StyleKey => typeof(ScrollViewer);
    }
}
