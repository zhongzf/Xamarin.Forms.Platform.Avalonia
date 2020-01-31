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
        public override Size PixelScreenSize => throw new NotImplementedException();

        public override Size ScaledScreenSize => (global::Avalonia.Application.Current as IClassicDesktopStyleApplicationLifetime)?.MainWindow.Screens.Primary.WorkingArea.ToRect(1.0).Size.ToSize() ?? Size.Zero;

        public override double ScalingFactor => throw new NotImplementedException();
    }
}
