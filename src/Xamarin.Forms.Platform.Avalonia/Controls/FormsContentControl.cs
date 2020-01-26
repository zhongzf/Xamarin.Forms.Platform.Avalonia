using Avalonia;
using Avalonia.Controls;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms.Platform.Avalonia.Interfaces;

namespace Xamarin.Forms.Platform.Avalonia.Controls
{
    public class FormsContentControl : ContentControl
    {
        private CancellationTokenSource tokenSource;

        public static readonly StyledProperty<object> SourceProperty = AvaloniaProperty.Register<FormsContentControl, object>(nameof(Source));
        public static readonly StyledProperty<IContentLoader> ContentLoaderProperty = AvaloniaProperty.Register<FormsContentControl, IContentLoader>(nameof(ContentLoader), new DefaultContentLoader());

        public object Source
        {
            get { return (object)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public IContentLoader ContentLoader
        {
            get { return (IContentLoader)GetValue(ContentLoaderProperty); }
            set { SetValue(ContentLoaderProperty, value); }
        }

        public FormsContentControl()
        {
            //this.DefaultStyleKey = typeof(FormsContentControl);
            this.LayoutUpdated += FormsContentControl_LayoutUpdated;
            SourceProperty.Changed.AddClassHandler<FormsContentControl>((x, e) => x.OnSourceChanged(e));
            ContentLoaderProperty.Changed.AddClassHandler<FormsContentControl>((x, e) => x.OnContentLoaderChanged(e));
        }

        private void FormsContentControl_LayoutUpdated(object sender, EventArgs e)
        {
            this.ContentLoader.OnSizeContentChanged(this, Source);
        }

        private void OnContentLoaderChanged(AvaloniaPropertyChangedEventArgs e)
        {
            if (e.NewValue == null)
            {
                throw new ArgumentNullException("ContentLoader");
            }
        }

        private void OnSourceChanged(AvaloniaPropertyChangedEventArgs e)
        {
            OnSourceChanged(e.OldValue, e.NewValue);
        }

        private void OnSourceChanged(object oldValue, object newValue)
        {
            if (newValue != null && newValue.Equals(oldValue)) return;

            var localTokenSource = new CancellationTokenSource();
            this.tokenSource = localTokenSource;

            var scheduler = TaskScheduler.FromCurrentSynchronizationContext();
            var task = this.ContentLoader.LoadContentAsync(this, oldValue, newValue, this.tokenSource.Token);

            task.ContinueWith(t =>
            {
                try
                {
                    if (t.IsFaulted || t.IsCanceled || localTokenSource.IsCancellationRequested)
                    {
                        this.Content = null;
                    }
                    else
                    {
                        this.Content = t.Result;

                    }
                }
                finally
                {
                    if (this.tokenSource == localTokenSource)
                    {
                        this.tokenSource = null;
                    }
                    localTokenSource.Dispose();
                }
            }, scheduler);
            return;
        }
    }
}
