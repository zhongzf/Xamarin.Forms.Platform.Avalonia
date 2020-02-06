using Avalonia.Controls;
using Avalonia.Forms.Controls;
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
