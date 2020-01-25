using Avalonia.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.Forms.Platform.Avalonia.Helpers
{
	static class UiHelper
	{
		public static void ExecuteInUiThread(Action action)
		{
			if (global::Avalonia.Application.Current.CheckAccess())
			{
				action?.Invoke();
			}
			else
			{
				Dispatcher.UIThread.InvokeAsync(action);
			}
		}
	}
}
