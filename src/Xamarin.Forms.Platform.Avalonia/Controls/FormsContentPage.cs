using Avalonia.Controls;
using Avalonia.Styling;
using System;

namespace Xamarin.Forms.Platform.Avalonia.Controls
{
	public class FormsContentPage : FormsPage, IStyleable
	{
		Type IStyleable.StyleKey => typeof(ContentControl);

		public FormsContentPage()
		{
		}
	}
}
