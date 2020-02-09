using Avalonia;
using Avalonia.Controls;
using Avalonia.Styling;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvaloniaForms.Controls
{
    public class CollectionView : ItemsRepeater, IStyleable
    {
        public static readonly StyledProperty<IContentLoader> ContentLoaderProperty = AvaloniaProperty.Register<CollectionView, IContentLoader>(nameof(ContentLoader), new DefaultContentLoader());

        static CollectionView()
        {
        }

        Type IStyleable.StyleKey => typeof(Panel);

        public IContentLoader ContentLoader
        {
            get { return (IContentLoader)GetValue(ContentLoaderProperty); }
            set { SetValue(ContentLoaderProperty, value); }
        }

        public CollectionView()
        {
			LayoutUpdated += OnLayoutUpdated;
		}

		#region Loaded & Unloaded
		public event EventHandler<EventArgs> Loaded;
		public event EventHandler<EventArgs> Unloaded;

		protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
		{
			OnLoaded(e);
			Appearing();
		}

		protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
		{
			OnUnloaded(e);
			Disappearing();
		}

		protected virtual void OnLoaded(EventArgs e) { Loaded?.Invoke(this, e); }
		protected virtual void OnUnloaded(EventArgs e) { Unloaded?.Invoke(this, e); }
		#endregion

		#region Appearing & Disappearing
		protected virtual void Appearing()
		{
		}

		protected virtual void Disappearing()
		{
		}
		#endregion

		#region LayoutUpdated & SizeChanged
		public event EventHandler<EventArgs> SizeChanged;
		protected virtual void OnSizeChanged(EventArgs e) { SizeChanged?.Invoke(this, e); }

		protected virtual void OnLayoutUpdated(object sender, EventArgs e)
		{
			OnSizeChanged(e);
		}
		#endregion
	}
}
