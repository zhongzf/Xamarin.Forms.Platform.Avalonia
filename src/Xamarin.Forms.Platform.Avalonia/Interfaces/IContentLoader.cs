using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Xamarin.Forms.Platform.Avalonia.Interfaces
{
    public interface IContentLoader
    {
		Task<object> LoadContentAsync(Control parent, object oldContent, object newContent, CancellationToken cancellationToken);

		void OnSizeContentChanged(Control parent, object content);
		void OnSizeContentChanged(Rect parentBounds, object content);
	}

	public class DefaultContentLoader : IContentLoader
	{
		public Task<object> LoadContentAsync(Control parent, object oldContent, object newContent, CancellationToken cancellationToken)
		{
			if (!global::Avalonia.Application.Current.CheckAccess())
				throw new InvalidOperationException("UIThreadRequired");

			var scheduler = TaskScheduler.FromCurrentSynchronizationContext();
			return Task.Factory.StartNew(() => LoadContent(newContent), cancellationToken, TaskCreationOptions.None, scheduler);
		}

		protected virtual object LoadContent(object content)
		{
			if (content is Control)
				return content;

			var loader = new AvaloniaXamlLoader();
			if (content is Uri)
				return loader.Load(content as Uri);

			if (content is string)
			{
				if (Uri.TryCreate(content as string, UriKind.RelativeOrAbsolute, out Uri uri))
				{
					return loader.Load(uri);
				}
			}

			return null;
		}

		public void OnSizeContentChanged(Control parent, object page)
		{
		}

		public void OnSizeContentChanged(Rect parentBounds, object content)
		{
		}
	}
}
