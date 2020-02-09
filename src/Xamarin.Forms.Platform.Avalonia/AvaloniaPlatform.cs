using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms.Internals;

namespace Xamarin.Forms.Platform.Avalonia
{
    public class AvaloniaPlatform : Platform
    {
        public AvaloniaPlatform(global::Avalonia.Controls.ContentControl page)
            : base(page)
        {
        }
    }
}
