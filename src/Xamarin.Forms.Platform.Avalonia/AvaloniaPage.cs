﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Xamarin.Forms.Platform.Avalonia
{
    public class AvaloniaPage : AvaloniaBasePage
    {
        protected override Platform CreatePlatform()
        {
            return new AvaloniaPlatform(this);
        }
    }
}