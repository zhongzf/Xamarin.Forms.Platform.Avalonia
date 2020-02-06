using Avalonia;
using Avalonia.Controls;
using AvaloniaForms.Interfaces;
using Avalonia.Styling;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AvaloniaForms.Controls
{
    public class DynamicContentControl : ContentControl, IStyleable
    {        
        public static readonly StyledProperty<object> SourceProperty = AvaloniaProperty.Register<DynamicContentControl, object>(nameof(Source));
        public static readonly StyledProperty<IContentLoader> ContentLoaderProperty = AvaloniaProperty.Register<DynamicContentControl, IContentLoader>(nameof(ContentLoader), new DefaultContentLoader());
        
        static DynamicContentControl()
        {
            SourceProperty.Changed.AddClassHandler<DynamicContentControl>((x, e) => x.OnSourcePropertyChanged(e));
            ContentLoaderProperty.Changed.AddClassHandler<DynamicContentControl>((x, e) => x.OnContentLoaderPropertyChanged(e));
        }

        Type IStyleable.StyleKey => typeof(ContentControl);

        private CancellationTokenSource tokenSource;

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

        public DynamicContentControl()
        {
            LayoutUpdated += OnLayoutUpdated;
        }

        protected virtual void OnLayoutUpdated(object sender, EventArgs e)
        {
            ContentLoader.OnSizeContentChanged(this, Source);
        }

        private void OnSourcePropertyChanged(AvaloniaPropertyChangedEventArgs e)
        {
            OnSourceChanged(e.OldValue, e.NewValue);
        }

        private void OnContentLoaderPropertyChanged(AvaloniaPropertyChangedEventArgs e)
        {
            if (e.NewValue == null)
            {
                throw new ArgumentNullException("ContentLoader");
            }
        }

        protected virtual void OnSourceChanged(object oldValue, object newValue)
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
