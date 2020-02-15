using Avalonia.Interactivity;
using AvaloniaForms.Controls;
using System;
using System.Collections.Generic;
using System.Text;

namespace Xamarin.Forms.Platform.Avalonia.Controls
{
    public class FormsPresenter : PageContentPresenter
    {
        public FormsPresenter()
        {
			Loaded += FormsPresenter_Loaded;
			Unloaded += FormsPresenter_Unloaded;
			SizeChanged += (s, e) =>
			{
				if (Width > 0 && Height > 0 && DataContext is Page page)
				{
					((Page)page.RealParent).ContainerArea = new Rectangle(0, 0, Width, Height);
				}
			};
		}

		void FormsPresenter_Loaded(object sender, RoutedEventArgs e)
			=> (DataContext as Page)?.SendAppearing();

		void FormsPresenter_Unloaded(object sender, RoutedEventArgs e)
			=> (DataContext as Page)?.SendDisappearing();
	}
}
