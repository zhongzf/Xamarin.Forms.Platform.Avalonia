using Avalonia.Controls.ApplicationLifetimes;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Internals;


namespace Xamarin.Forms.Platform.Avalonia
{
    internal class AvaloniaDeviceInfo : DeviceInfo
    {
		public override Size PixelScreenSize
		{
			get
			{
				double scaling = ScalingFactor;
				Size scaled = ScaledScreenSize;
				double width = Math.Round(scaled.Width * scaling);
				double height = Math.Round(scaled.Height * scaling);

				return new Size(width, height);
			}
		}

		public override Size ScaledScreenSize => (global::Avalonia.Application.Current as IClassicDesktopStyleApplicationLifetime)?.MainWindow.Screens.Primary.WorkingArea.ToRect(ScalingFactor).Size.ToSize() ?? Size.Zero;

        public override double ScalingFactor => 1.0;

        public AvaloniaDeviceInfo()
        {
		}
    }
}
