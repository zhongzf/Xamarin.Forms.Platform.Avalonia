using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Text;

namespace Xamarin.Forms.Platform.Avalonia
{
	public interface IVisualElementRenderer : IRegisterable, IDisposable
	{
		Control GetNativeElement();

		VisualElement Element { get; }

		event EventHandler<VisualElementChangedEventArgs> ElementChanged;

		SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint);

		void SetElement(VisualElement element);
	}
}
