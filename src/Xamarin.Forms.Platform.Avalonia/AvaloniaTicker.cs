using Avalonia.Threading;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Internals;

namespace Xamarin.Forms.Platform.Avalonia
{
    internal class AvaloniaTicker : Ticker
    {
        readonly DispatcherTimer _timer;

		public AvaloniaTicker()
		{
			_timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(15) };
			_timer.Tick += (sender, args) => SendSignals();
		}

		protected override void DisableTimer()
		{
			_timer.Stop();
		}

		protected override void EnableTimer()
		{
			_timer.Start();
		}
	}
}
