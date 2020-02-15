using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Internals;

namespace Xamarin.Forms.Platform.Avalonia
{
    public static class FontExtensions
    {
        public static void ApplyFont(this Control control, Font font)
        {
            if (control is TemplatedControl self)
            {
                self.FontSize = font.UseNamedSize ? GetFontSize(font.NamedSize) : font.FontSize;
                //self.FontFamily = !string.IsNullOrEmpty(font.FontFamily) ? new FontFamily(font.FontFamily) : (FontFamily)WApplication.Current.Resources["ContentControlThemeFontFamily"];
                self.FontStyle = font.FontAttributes.HasFlag(FontAttributes.Italic) ? FontStyle.Italic : FontStyle.Normal;
                self.FontWeight = font.FontAttributes.HasFlag(FontAttributes.Bold) ? FontWeight.Bold : FontWeight.Normal;
            }
        }

        public static void ApplyFont(this TextBlock self, Font font)
        {
            self.FontSize = font.UseNamedSize ? GetFontSize(font.NamedSize) : font.FontSize;
            //self.FontFamily = !string.IsNullOrEmpty(font.FontFamily) ? new FontFamily(font.FontFamily) : (FontFamily)WApplication.Current.Resources["ContentControlThemeFontFamily"];
            self.FontStyle = font.FontAttributes.HasFlag(FontAttributes.Italic) ? FontStyle.Italic : FontStyle.Normal;
            self.FontWeight = font.FontAttributes.HasFlag(FontAttributes.Bold) ? FontWeight.Bold : FontWeight.Normal;
        }


        internal static void ApplyFont(this Control control, IFontElement element)
        {
            if (control is TemplatedControl self)
            {
                self.FontSize = element.FontSize;
                //self.FontFamily = !string.IsNullOrEmpty(element.FontFamily) ? new FontFamily(font.FontFamily) : (FontFamily)WApplication.Current.Resources["ContentControlThemeFontFamily"];
                self.FontStyle = element.FontAttributes.HasFlag(FontAttributes.Italic) ? FontStyle.Italic : FontStyle.Normal;
                self.FontWeight = element.FontAttributes.HasFlag(FontAttributes.Bold) ? FontWeight.Bold : FontWeight.Normal;
            }
        }

        internal static double GetFontSize(this NamedSize size)
        {
            // These are values pulled from the mapped sizes on Windows Phone, WinRT has no equivalent sizes, only intents.
            switch (size)
            {
                case NamedSize.Default:
                    //return (double)WApplication.Current.Resources["ControlContentThemeFontSize"];
                    return 20;
                case NamedSize.Micro:
                    return 15.667;
                case NamedSize.Small:
                    return 18.667;
                case NamedSize.Medium:
                    return 22.667;
                case NamedSize.Large:
                    return 32;
                case NamedSize.Body:
                    return 14;
                case NamedSize.Caption:
                    return 12;
                case NamedSize.Header:
                    return 46;
                case NamedSize.Subtitle:
                    return 20;
                case NamedSize.Title:
                    return 24;
                default:
                    throw new ArgumentOutOfRangeException("size");
            }
        }

        internal static bool IsDefault(this IFontElement self)
        {
            return self.FontFamily == null && self.FontSize == Device.GetNamedSize(NamedSize.Default, typeof(Label), true) && self.FontAttributes == FontAttributes.None;
        }
    }
}
