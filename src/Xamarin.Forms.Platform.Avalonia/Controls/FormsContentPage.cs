using Avalonia.Controls;
using AvaloniaForms.Controls;
using Avalonia.Styling;
using System;

namespace Xamarin.Forms.Platform.Avalonia.Controls
{
	public class FormsContentPage : DynamicContentPage, IStyleable
	{
		Type IStyleable.StyleKey => typeof(ContentControl);

		public FormsContentPage()
		{
		}
	}
}
