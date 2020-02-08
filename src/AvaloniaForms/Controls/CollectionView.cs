using Avalonia;
using Avalonia.Controls;
using Avalonia.Styling;
using AvaloniaForms.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvaloniaForms.Controls
{
    public class CollectionView : ItemsRepeater, IStyleable
    {
        public static readonly StyledProperty<IContentLoader> ContentLoaderProperty = AvaloniaProperty.Register<CollectionView, IContentLoader>(nameof(ContentLoader), new DefaultContentLoader());

        static CollectionView()
        {
        }

        Type IStyleable.StyleKey => typeof(Panel);

        public IContentLoader ContentLoader
        {
            get { return (IContentLoader)GetValue(ContentLoaderProperty); }
            set { SetValue(ContentLoaderProperty, value); }
        }
    }
}
