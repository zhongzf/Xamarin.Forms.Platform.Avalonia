using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Internals;

namespace Xamarin.Forms.Platform.Avalonia
{
    internal class AvaloniaTicker : Ticker
    {
        [ThreadStatic]
        static Ticker s_ticker;

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

        protected override Ticker GetTickerInstance()
        {
            if((global::Avalonia.Application.Current as IClassicDesktopStyleApplicationLifetime).MainWindow != null)
            {
                // We've got multiple windows open, we'll need to use the local ThreadStatic Ticker instead of the 
                // singleton in the base class 
                if (s_ticker == null)
                {
                    s_ticker = new AvaloniaTicker();
                }

                return s_ticker;
            }

            return base.GetTickerInstance();
        }
    }
}
