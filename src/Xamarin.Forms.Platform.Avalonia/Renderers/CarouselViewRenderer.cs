using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms.Platform.Avalonia.Controls;

namespace Xamarin.Forms.Platform.Avalonia
{
    // TODO:
    public class CarouselViewRenderer : ItemsViewRenderer<CarouselView, FormsCarouselView>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<CarouselView> e)
        {
            if (e.NewElement != null)
            {
                if (Control == null) // construct and SetNativeControl and suscribe control event
                {
                    SetNativeControl(new FormsCarouselView() { ContentLoader = new FormsContentLoader() });
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
