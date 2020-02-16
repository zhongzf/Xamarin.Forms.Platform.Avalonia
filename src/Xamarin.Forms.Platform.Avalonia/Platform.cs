using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Internals;

namespace Xamarin.Forms.Platform.Avalonia
{
    public abstract class Platform : INavigation
#pragma warning disable CS0618 // Type or member is obsolete
		, IPlatform
#pragma warning restore CS0618 // Type or member is obsolete
	{
		internal static readonly BindableProperty RendererProperty = BindableProperty.CreateAttached("Renderer", typeof(IVisualElementRenderer), typeof(Platform), default(IVisualElementRenderer));
		
		public static IVisualElementRenderer GetOrCreateRenderer(VisualElement element)
		{
			if (GetRenderer(element) == null)
			{
				SetRenderer(element, CreateRenderer(element));
			}

			return GetRenderer(element);
		}

		public static IVisualElementRenderer CreateRenderer(VisualElement element)
		{
			IVisualElementRenderer result = Registrar.Registered.GetHandlerForObject<IVisualElementRenderer>(element) ?? new DefaultViewRenderer();
			result.SetElement(element);
			return result;
		}

		public static IVisualElementRenderer GetRenderer(VisualElement self)
		{
			return (IVisualElementRenderer)self.GetValue(RendererProperty);
		}

		Rectangle _bounds;
		readonly Canvas _container;
		readonly AvaloniaForms.Controls.Page _page;

		Page _currentPage;

		readonly NavigationModel _navModel = new NavigationModel();

		public IReadOnlyList<Page> ModalStack => throw new NotImplementedException();

		public IReadOnlyList<Page> NavigationStack => throw new NotImplementedException();

		internal Platform(AvaloniaForms.Controls.Page page)
		{
			if (page == null)
				throw new ArgumentNullException(nameof(page));

			_page = page;
			_page.LayoutUpdated += OnPageLayoutUpdated;

			var current = global::Avalonia.Application.Current;
			// TODO: resources

			_container = new Canvas();
			_page.Content = _container;
			//_container.LayoutUpdated += OnRendererSizeChanged;
		}

		private void OnPageLayoutUpdated(object sender, EventArgs e)
		{
			OnRendererSizeChanged(sender, e);
		}

		private void OnRendererSizeChanged(object sender, EventArgs e)
		{
			UpdateBounds();
			UpdatePageSizes();
		}

		void UpdateBounds()
		{
			_bounds = new Rectangle(0, 0, _page.Bounds.Width, _page.Bounds.Height);
		}

		internal void UpdatePageSizes()
		{
			Rectangle bounds = ContainerBounds;
			if (bounds.IsEmpty)
				return;
			foreach (Page root in _navModel.Roots)
			{
				root.Layout(bounds);
				IVisualElementRenderer renderer = GetRenderer(root);
				if (renderer != null)
				{
					renderer.ContainerElement.Width = _container.Bounds.Width;
					renderer.ContainerElement.Height = _container.Bounds.Height;
				}
			}
		}


		internal void SetPage(Page newRoot)
		{
			if (newRoot == null)
				throw new ArgumentNullException(nameof(newRoot));

			_navModel.Clear();

			_navModel.Push(newRoot, null);
			SetCurrent(newRoot, true);
			Application.Current.NavigationProxy.Inner = this;
		}

		async void SetCurrent(Page newPage, bool popping = false, Action completedCallback = null)
		{
			try
			{
				if (newPage == _currentPage)
					return;

#pragma warning disable CS0618 // Type or member is obsolete
				// The Platform property is no longer necessary, but we have to set it because some third-party
				// library might still be retrieving it and using it
				newPage.Platform = this;
#pragma warning restore CS0618 // Type or member is obsolete

				if (_currentPage != null)
				{
					Page previousPage = _currentPage;
					IVisualElementRenderer previousRenderer = GetRenderer(previousPage);
					_container.Children.Remove(previousRenderer.ContainerElement);

					if (popping)
					{
						previousPage.Cleanup();
						// Un-parent the page; otherwise the Resources Changed Listeners won't be unhooked and the 
						// page will leak 
						previousPage.Parent = null;
					}
				}

				newPage.Layout(ContainerBounds);

				IVisualElementRenderer pageRenderer = newPage.GetOrCreateRenderer();
				_container.Children.Add(pageRenderer.ContainerElement);

				pageRenderer.ContainerElement.Width = _container.Bounds.Width;
				pageRenderer.ContainerElement.Height = _container.Bounds.Height;

				completedCallback?.Invoke();

				_currentPage = newPage;

				UpdateToolbarTracker();

				await UpdateToolbarItems();
			}
			catch (Exception error)
			{
				//This exception prevents the Main Page from being changed in a child 
				//window or a different thread, except on the Main thread. 
				//HEX 0x8001010E 
				if (error.HResult == -2147417842)
					throw new InvalidOperationException("Changing the current page is only allowed if it's being called from the same UI thread." +
						"Please ensure that the new page is in the same UI thread as the current page.");
				throw error;
			}
		}

		private void UpdateToolbarTracker()
		{
		}

		private Task UpdateToolbarItems()
		{
			return Task.CompletedTask;
		}

		public static void SetRenderer(VisualElement self, IVisualElementRenderer renderer)
		{
			self.SetValue(RendererProperty, renderer);
			self.IsPlatformEnabled = renderer != null;
		}

		SizeRequest IPlatform.GetNativeSize(VisualElement element, double widthConstraint, double heightConstraint)
		{
			return Platform.GetNativeSize(element, widthConstraint, heightConstraint);
		}

		public static SizeRequest GetNativeSize(VisualElement view, double widthConstraint, double heightConstraint)
		{
			if (widthConstraint > 0 && heightConstraint > 0 && GetRenderer(view) != null)
			{
				IVisualElementRenderer element = GetRenderer(view);
				return element.GetDesiredSize(widthConstraint, heightConstraint);
			}

			return new SizeRequest();
		}

		internal virtual Rectangle ContainerBounds
		{
			get { return _bounds; }
		}

		#region INavigation
		Task INavigation.PushAsync(Page root)
		{
			return ((INavigation)this).PushAsync(root, true);
		}

		Task<Page> INavigation.PopAsync()
		{
			return ((INavigation)this).PopAsync(true);
		}

		Task INavigation.PopToRootAsync()
		{
			return ((INavigation)this).PopToRootAsync(true);
		}

		Task INavigation.PushAsync(Page root, bool animated)
		{
			throw new InvalidOperationException("PushAsync is not supported globally on Windows, please use a NavigationPage.");
		}

		Task<Page> INavigation.PopAsync(bool animated)
		{
			throw new InvalidOperationException("PopAsync is not supported globally on Windows, please use a NavigationPage.");
		}

		Task INavigation.PopToRootAsync(bool animated)
		{
			throw new InvalidOperationException(
				"PopToRootAsync is not supported globally on Windows, please use a NavigationPage.");
		}

		void INavigation.RemovePage(Page page)
		{
			throw new InvalidOperationException("RemovePage is not supported globally on Windows, please use a NavigationPage.");
		}

		void INavigation.InsertPageBefore(Page page, Page before)
		{
			throw new InvalidOperationException(
				"InsertPageBefore is not supported globally on Windows, please use a NavigationPage.");
		}

		Task INavigation.PushModalAsync(Page page)
		{
			return ((INavigation)this).PushModalAsync(page, true);
		}

		Task<Page> INavigation.PopModalAsync()
		{
			return ((INavigation)this).PopModalAsync(true);
		}

		Task INavigation.PushModalAsync(Page page, bool animated)
		{
			if (page == null)
				throw new ArgumentNullException(nameof(page));

			var tcs = new TaskCompletionSource<bool>();
			_navModel.PushModal(page);
			SetCurrent(page, completedCallback: () => tcs.SetResult(true));
			return tcs.Task;
		}

		Task<Page> INavigation.PopModalAsync(bool animated)
		{
			var tcs = new TaskCompletionSource<Page>();
			Page result = _navModel.PopModal();
			SetCurrent(_navModel.CurrentPage, true, () => tcs.SetResult(result));
			return tcs.Task;
		}
        #endregion
    }
}
