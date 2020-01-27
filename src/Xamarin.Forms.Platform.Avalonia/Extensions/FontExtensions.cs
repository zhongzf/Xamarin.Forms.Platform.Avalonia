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

                //if (!string.IsNullOrEmpty(font.FontFamily))
                //    self.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), font.FontFamily);
                //else
                //    self.FontFamily = (FontFamily)System.Windows.Application.Current.Resources["FontFamilySemiBold"];

                if (font.FontAttributes.HasFlag(FontAttributes.Italic))
                    self.FontStyle = FontStyle.Italic;
                else
                    self.FontStyle = FontStyle.Normal;

                if (font.FontAttributes.HasFlag(FontAttributes.Bold))
                    self.FontWeight = FontWeight.Bold;
                else
                    self.FontWeight = FontWeight.Normal;
            }
        }

        public static void ApplyFont(this TextBlock self, Font font)
        {
            self.FontSize = font.UseNamedSize ? GetFontSize(font.NamedSize) : font.FontSize;

            //if (!string.IsNullOrEmpty(font.FontFamily))
            //{
            //    self.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), font.FontFamily);
            //}
            //else
            //{
            //    self.FontFamily = (FontFamily)System.Windows.Application.Current.Resources["FontFamilyNormal"];
            //}

            if (font.FontAttributes.HasFlag(FontAttributes.Italic))
                self.FontStyle = FontStyle.Italic;
            else
                self.FontStyle = FontStyle.Normal;

            if (font.FontAttributes.HasFlag(FontAttributes.Bold))
                self.FontWeight = FontWeight.Bold;
            else
                self.FontWeight = FontWeight.Normal;
        }


        internal static void ApplyFont(this Control control, IFontElement element)
        {
            if (control is TemplatedControl self)
            {
                self.FontSize = element.FontSize;

                //if (!string.IsNullOrEmpty(element.FontFamily))
                //    self.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), element.FontFamily);
                //else
                //    self.FontFamily = (FontFamily)System.Windows.Application.Current.Resources["FontFamilySemiBold"];

                if (element.FontAttributes.HasFlag(FontAttributes.Italic))
                    self.FontStyle = FontStyle.Italic;
                else
                    self.FontStyle = FontStyle.Normal;

                if (element.FontAttributes.HasFlag(FontAttributes.Bold))
                    self.FontWeight = FontWeight.Bold;
                else
                    self.FontWeight = FontWeight.Normal;
            }
        }

        internal static double GetFontSize(this NamedSize size)
        {
            switch (size)
            {
                case NamedSize.Default:
                case NamedSize.Micro:
                case NamedSize.Small:
                case NamedSize.Medium:
                // use normal instead of medium as this is the default
                case NamedSize.Large:
                    {
                        // TODO:
                        return 20;
                    }
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
