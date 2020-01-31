using Avalonia.Controls.ApplicationLifetimes;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform.Avalonia.Extensions;

namespace Xamarin.Forms.Platform.Avalonia
{
    internal class AvaloniaDeviceInfo : DeviceInfo
    {
        public override Size PixelScreenSize => ScaledScreenSize;

        public override Size ScaledScreenSize => (global::Avalonia.Application.Current as IClassicDesktopStyleApplicationLifetime)?.MainWindow.Screens.Primary.WorkingArea.ToRect(ScalingFactor).Size.ToSize() ?? Size.Zero;

        public override double ScalingFactor => 1.0;
    }
}
