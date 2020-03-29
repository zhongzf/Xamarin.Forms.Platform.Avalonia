using Avalonia.Media;
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

            if (global::Avalonia.Application.Current.Resources.ContainsKey("SystemColorControlAccentBrush"))
            {
                var accent = (SolidColorBrush)global::Avalonia.Application.Current.Resources["SystemColorControlAccentBrush"];
                Color.SetAccent(accent.ToFormsColor());
            }

            Registrar.ExtraAssemblies = rendererAssemblies?.ToArray();

            Device.SetTargetIdiom(TargetIdiom.Desktop);
            Device.PlatformServices = new AvaloniaPlatformServices();
            Device.Info = new AvaloniaDeviceInfo();
            ExpressionSearch.Default = new AvaloniaExpressionSearch();

            Registrar.RegisterAll(new[] { typeof(ExportRendererAttribute), typeof(ExportCellAttribute), typeof(ExportImageSourceHandlerAttribute) });
            
            Ticker.SetDefault(new AvaloniaTicker());
            Device.SetIdiom(TargetIdiom.Desktop);

            IsInitialized = true;
        }
    }
}
