using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Internals;

namespace Xamarin.Forms.Platform.Avalonia
{
    internal class AvaloniaDeviceInfo : DeviceInfo
    {
        public override Size PixelScreenSize => throw new NotImplementedException();

        public override Size ScaledScreenSize => throw new NotImplementedException();

        public override double ScalingFactor => throw new NotImplementedException();
    }
}
