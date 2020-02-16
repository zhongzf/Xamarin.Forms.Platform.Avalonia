using System;
using System.Collections.Generic;
using System.Text;

namespace AvaloniaForms.Controls
{
    public partial class ComboBox : Avalonia.Controls.ComboBox
    {
		internal bool IsClosingAnimated { get; private set; }

		internal bool IsOpeningAnimated { get; private set; }


		protected virtual void OnClosedAnimationStarted()
		{
			ClosedAnimationStarted?.Invoke(this, EventArgs.Empty);
		}

		protected virtual void OnOpenAnimationCompleted()
		{
			OpenAnimationCompleted?.Invoke(this, EventArgs.Empty);
		}

		internal event EventHandler<EventArgs> ClosedAnimationStarted;

		internal event EventHandler<EventArgs> OpenAnimationCompleted;
	}
}
