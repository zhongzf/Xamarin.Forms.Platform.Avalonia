using Avalonia;
using System;
using System.ComponentModel;
using Xamarin.Forms.Platform.Avalonia.Controls;

namespace Xamarin.Forms.Platform.Avalonia
{
	public class MasterDetailPageRenderer : VisualPageRenderer<MasterDetailPage, FormsMasterDetailPage>
	{
		protected override void OnElementChanged(ElementChangedEventArgs<MasterDetailPage> e)
		{
			if (e.NewElement != null)
			{
				if (Control == null) // construct and SetNativeControl and suscribe control event
				{
					SetNativeControl(new FormsMasterDetailPage() { ContentLoader = new FormsContentLoader() });

					Control.PropertyChanged += Control_PropertyChanged;
				}
			}

			base.OnElementChanged(e);
		}

		private void Control_PropertyChanged(object sender, AvaloniaPropertyChangedEventArgs e)
		{
			if(e.Property == FormsMasterDetailPage.IsPresentedProperty)
			{
				OnIsPresentedChanged(e);
			}
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == MasterDetailPage.IsPresentedProperty.PropertyName) // || e.PropertyName == MasterDetailPage.MasterBehaviorProperty.PropertyName)
				UpdateIsPresented();
			else if (e.PropertyName == "Master")
				UpdateMasterPage();
			else if (e.PropertyName == "Detail")
				UpdateDetailPage();
		}

		protected override void Appearing()
		{
			base.Appearing();
			UpdateIsPresented();
			UpdateMasterPage();
			UpdateDetailPage();
		}

		void UpdateIsPresented()
		{
			Control.IsPresented = Element.IsPresented;
		}

		void UpdateMasterPage()
		{
			Control.MasterPage = Element.Master;
		}

		void UpdateDetailPage()
		{
			Control.DetailPage = Element.Detail;
		}
		
		private void OnIsPresentedChanged(AvaloniaPropertyChangedEventArgs e)
		{
			((IElementController)Element).SetValueFromRenderer(MasterDetailPage.IsPresentedProperty, Control.IsPresented);
		}

		bool _isDisposed;

		protected override void Dispose(bool disposing)
		{
			if (_isDisposed)
				return;

			if (disposing)
			{
				if (Control != null)
				{
					Control.PropertyChanged -= Control_PropertyChanged;
				}
			}

			_isDisposed = true;
			base.Dispose(disposing);
		}
	}
}
