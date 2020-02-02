using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Platform.Avalonia.Controls;
using Avalonia.Controls;

namespace Xamarin.Forms.Platform.Avalonia
{
    public static class SystemCommands
    {
        internal static void MinimizeWindow(FormsWindow parentWindow)
        {
            parentWindow.WindowState = WindowState.Minimized;
        }

        internal static void RestoreWindow(FormsWindow parentWindow)
        {
            parentWindow.WindowState = WindowState.Normal;
        }

        internal static void MaximizeWindow(FormsWindow parentWindow)
        {
            parentWindow.WindowState = WindowState.Maximized;
        }
    }
}
