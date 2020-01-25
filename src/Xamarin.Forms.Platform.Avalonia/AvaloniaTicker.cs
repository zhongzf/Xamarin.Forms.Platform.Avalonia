using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Internals;

namespace Xamarin.Forms.Platform.Avalonia
{
    internal class AvaloniaTicker : Ticker
    {
        protected override void DisableTimer()
        {
            throw new NotImplementedException();
        }

        protected override void EnableTimer()
        {
            throw new NotImplementedException();
        }
    }
}
