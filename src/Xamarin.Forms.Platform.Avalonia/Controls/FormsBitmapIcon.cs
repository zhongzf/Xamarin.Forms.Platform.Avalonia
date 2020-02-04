using Avalonia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Xamarin.Forms.Platform.Avalonia.Controls
{
	public class FormsBitmapIcon : FormsElementIcon
	{
		public static readonly StyledProperty<Uri> UriSourceProperty = AvaloniaProperty.Register<FormsBitmapIcon, Uri>(nameof(UriSource));

		public Uri UriSource
		{
			get { return (Uri)GetValue(UriSourceProperty); }
			set { SetValue(UriSourceProperty, value); }
		}

		public FormsBitmapIcon()
		{
		}

		protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if (e.Property == UriSourceProperty)
			{
				OnSourceChanged(e);
			}
		}

		private void OnSourceChanged(AvaloniaPropertyChangedEventArgs e)
		{
			OnSourceChanged(e.OldValue, e.NewValue);
		}

		private void OnSourceChanged(object oldValue, object newValue)
		{
			if (newValue is Uri uri && !uri.IsAbsoluteUri)
			{
				var name = Assembly.GetEntryAssembly().GetName().Name;
				UriSource = new Uri(string.Format("pack://application:,,,/{0};component/{1}", name, uri.OriginalString));
			}
		}
	}
}
