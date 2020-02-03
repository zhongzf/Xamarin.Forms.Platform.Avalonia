using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms.Platform.Avalonia.Controls;

namespace Xamarin.Forms.Platform.Avalonia
{
    public class CollectionViewRenderer : ItemsViewRenderer<CollectionView, FormsCollectionView>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<CollectionView> e)
        {
            if (e.NewElement != null)
            {
                if (Control == null) // construct and SetNativeControl and suscribe control event
                {
                    SetNativeControl(new FormsCollectionView() { ContentLoader = new FormsContentLoader() });
                }

                // TODO:
            }

            base.OnElementChanged(e);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            // TODO:
        }
    }
}
