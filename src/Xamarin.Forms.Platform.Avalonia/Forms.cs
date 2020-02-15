using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform.Avalonia;

namespace Xamarin.Forms
{
    public static class Forms
    {
        public static bool IsInitialized { get; private set; }

        public static void Init(Type applicationType = null, IEnumerable<Assembly> rendererAssemblies = null)
        {
            if (IsInitialized)
                return;

            Registrar.ExtraAssemblies = rendererAssemblies?.ToArray();

            Device.SetTargetIdiom(TargetIdiom.Desktop);
            Device.PlatformServices = new AvaloniaPlatformServices();


            Device.SetIdiom(TargetIdiom.Desktop);

            IsInitialized = true;
        }
    }
}
