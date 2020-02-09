using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AvaloniaForms
{
    public interface IContentLoader
    {
        Task<object> LoadContentAsync(Control parent, object oldContent, object newContent, CancellationToken cancellationToken);
		
		// TODO: 
		void OnSizeContentChanged(Control parent, object content);
	}

	public class DefaultContentLoader : IContentLoader
	{
		public Task<object> LoadContentAsync(Control parent, object oldContent, object newContent, CancellationToken cancellationToken)
		{
			if (!Application.Current.CheckAccess())
			{
				throw new InvalidOperationException("UIThreadRequired");
			}

			var scheduler = TaskScheduler.FromCurrentSynchronizationContext();
			return Task.Factory.StartNew(() => LoadContent(newContent), cancellationToken, TaskCreationOptions.None, scheduler);
		}

		protected virtual object LoadContent(object content)
		{
			if (content is Control)
			{
				return content;
			}

			var loader = new AvaloniaXamlLoader();
			var uri = content as Uri;
			if (content is string)
			{
				Uri.TryCreate(content as string, UriKind.RelativeOrAbsolute, out uri);
			}
			if (uri != null)
			{
				return loader.Load(uri);
			}
			return null;
		}

		public void OnSizeContentChanged(Control parent, object content)
		{
		}
	}
}
