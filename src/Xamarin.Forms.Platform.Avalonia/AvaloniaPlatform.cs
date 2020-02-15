using System;
using System.Collections.Generic;
using System.Text;

namespace Xamarin.Forms.Platform.Avalonia
{
    public class AvaloniaPlatform : Platform
    {
        public AvaloniaPlatform(AvaloniaForms.Controls.Page page)
            : base(page)
        {
        }
    }
}
