using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace AvaloniaForms.Controls
{
    public class IconElement : Image
    {
    }

    public class ElementIcon : TemplatedControl
    {
    }

    public class BitmapIcon : ElementIcon
    {
        public static readonly StyledProperty<Uri> UriSourceProperty = AvaloniaProperty.Register<BitmapIcon, Uri>(nameof(UriSource));

        static BitmapIcon()
        {
            UriSourceProperty.Changed.AddClassHandler<BitmapIcon>((x, e) => x.OnUriSourcePropertyChanged(e));
        }

        public Uri UriSource
        {
            get { return (Uri)GetValue(UriSourceProperty); }
            set { SetValue(UriSourceProperty, value); }
        }

        public BitmapIcon()
        {
        }

        private void OnUriSourcePropertyChanged(AvaloniaPropertyChangedEventArgs e)
        {
            var newValue = e.NewValue;
            if (newValue is Uri uri && !uri.IsAbsoluteUri)
            {
                var name = Assembly.GetEntryAssembly().GetName().Name;
                UriSource = new Uri(string.Format("pack://application:,,,/{0};component/{1}", name, uri.OriginalString));
            }
        }
    }

    public class PathIcon : ElementIcon
    {
        public static readonly StyledProperty<Geometry> DataProperty = AvaloniaProperty.Register<PathIcon, Geometry>(nameof(Data));

        public Geometry Data
        {
            get { return (Geometry)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }
    }

    public class SymbolIcon : ElementIcon
    {
        public static readonly StyledProperty<Symbol> SymbolProperty = AvaloniaProperty.Register<SymbolIcon, Symbol>(nameof(Symbol));

        public Symbol Symbol
        {
            get { return (Symbol)GetValue(SymbolProperty); }
            set { SetValue(SymbolProperty, value); }
        }
    }
}
