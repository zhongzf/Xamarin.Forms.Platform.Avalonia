using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.Styling;
using AvaloniaForms.Themes;
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

        static bool FlagsSet { get; set; }

        static IReadOnlyList<string> s_flags;
        public static IReadOnlyList<string> Flags => s_flags ?? (s_flags = new List<string>().AsReadOnly());

        public static void Init<T>(IEnumerable<Assembly> rendererAssemblies = null) where T : Application, new()
        {
            Init(null, rendererAssemblies);
            Application.Current = new T();
        }

        public static void Init(Type applicationType = null, IEnumerable<Assembly> rendererAssemblies = null)
        {
            if (IsInitialized)
                return;

            Log.Listeners.Add(new DelegateLogListener((c, m) => Console.WriteLine("[{0}] {1}", m, c)));
            Registrar.ExtraAssemblies = rendererAssemblies?.ToArray();

            Device.SetTargetIdiom(TargetIdiom.Desktop);
            Device.PlatformServices = new AvaloniaPlatformServices();
            Device.Info = new AvaloniaDeviceInfo();
            ExpressionSearch.Default = new AvaloniaExpressionSearch();

            Registrar.RegisterAll(new[] { typeof(ExportRendererAttribute), typeof(ExportCellAttribute), typeof(ExportImageSourceHandlerAttribute) });

            Ticker.SetDefault(new AvaloniaTicker());
            Device.SetIdiom(TargetIdiom.Desktop);
            Device.SetFlags(s_flags);

            IsInitialized = true;

            if (applicationType != null)
            {
                Application.Current = Activator.CreateInstance(applicationType) as Application;
            }
        }

        public static void SetFlags(params string[] flags)
        {
            if (FlagsSet)
            {
                return;
            }

            if (IsInitialized)
            {
                throw new InvalidOperationException($"{nameof(SetFlags)} must be called before {nameof(Init)}");
            }

            s_flags = flags.ToList().AsReadOnly();
            FlagsSet = true;
        }
    }
}
