using Avalonia;
using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xamarin.Forms.Platform.Avalonia.Controls
{
    public class FormsCollectionView : FormsMultiView
    {
        public FormsCollectionView()
        {
        }

        protected override void Appearing()
        {
            base.Appearing();

            UpdateCurrentSelectedIndex(0);
        }

        private void UpdateCurrentSelectedIndex(object newValue)
        {
            if (this.ItemsSource == null) return;
            var items = this.ItemsSource.Cast<object>();

            if ((int)newValue >= 0 && (int)newValue < items.Count())
            {
                this.SelectedItem = items.ElementAt((int)newValue);
            }
        }
    }
}
