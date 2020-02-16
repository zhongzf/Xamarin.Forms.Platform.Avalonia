using Avalonia.Animation;
using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Xamarin.Forms.Platform.Avalonia.Controls;

namespace Xamarin.Forms.Platform.Avalonia
{
	public class ScrollViewRenderer : ViewRenderer<ScrollView, FormsScrollViewer>
	{
		VisualElement _currentView;
		Animatable _animatable;
		
		protected IScrollViewController Controller
		{
			get { return Element; }
		}

		public ScrollViewRenderer()
		{
			AutoPackage = false;
		}

		public override SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
		{
			SizeRequest result = base.GetDesiredSize(widthConstraint, heightConstraint);
			result.Minimum = new Size(40, 40);
			return result;
		}


		protected override void OnElementChanged(ElementChangedEventArgs<ScrollView> e)
		{
			if (e.OldElement != null) // Clear old element event
			{
				((IScrollViewController)e.OldElement).ScrollToRequested -= OnScrollToRequested;
			}

			if (e.NewElement != null)
			{
				if (Control == null) // construct and SetNativeControl and suscribe control event
				{
					SetNativeControl(new FormsScrollViewer() { 
						HorizontalScrollBarVisibility = e.NewElement.HorizontalScrollBarVisibility.ToNativeScrollBarVisibility(),
						VerticalScrollBarVisibility = e.NewElement.VerticalScrollBarVisibility.ToNativeScrollBarVisibility()
					});
					Control.LayoutUpdated += NativeLayoutUpdated;
				}

				// Update control property 
				UpdateOrientation();
				UpdateContent();

				// Suscribe element event
				Controller.ScrollToRequested += OnScrollToRequested;
			}

			base.OnElementChanged(e);
		}

		
		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == "Content")
				UpdateContent();
			else if (e.PropertyName == Layout.PaddingProperty.PropertyName)
				UpdateMargins();
			else if (e.PropertyName == ScrollView.OrientationProperty.PropertyName)
				UpdateOrientation();
			else if (e.PropertyName == ScrollView.VerticalScrollBarVisibilityProperty.PropertyName)
				UpdateVerticalScrollBarVisibility();
			else if (e.PropertyName == ScrollView.HorizontalScrollBarVisibilityProperty.PropertyName)
				UpdateHorizontalScrollBarVisibility();
		}

		void NativeLayoutUpdated(object sender, EventArgs e)
		{
			UpdateScrollPosition();
		}

		static double GetDistance(double start, double position, double v)
		{
			return start + (position - start) * v;
		}

		protected void OnContentElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == View.MarginProperty.PropertyName)
				UpdateContentMargins();
		}

		void UpdateContent()
		{
			if (_currentView != null)
				_currentView.Cleanup();

			if (Control?.Content is Control oldElement)
			{
				if (oldElement is IVisualElementRenderer oldRenderer
					&& oldRenderer.Element is View oldContentView)
					oldContentView.PropertyChanged -= OnContentElementPropertyChanged;
			}

			_currentView = Element.Content;

			IVisualElementRenderer renderer = null;
			if (_currentView != null)
				renderer = _currentView.GetOrCreateRenderer();

			Control.Content = renderer != null ? renderer.ContainerElement : null;

			UpdateContentMargins();
			if (renderer?.Element != null)
				renderer.Element.PropertyChanged += OnContentElementPropertyChanged;
		}
		

		void OnScrollToRequested(object sender, ScrollToRequestedEventArgs e)
		{
			if (_animatable == null && e.ShouldAnimate)
				_animatable = new Animatable();

			ScrollToPosition position = e.Position;
			double x = e.ScrollX;
			double y = e.ScrollY;

			if (e.Mode == ScrollToMode.Element)
			{
				Point itemPosition = Controller.GetScrollPositionForElement(e.Element as VisualElement, e.Position);
				x = itemPosition.X;
				y = itemPosition.Y;
			}

			if (Control.Offset.Y == y && Control.Offset.X == x)
				return;

			if (e.ShouldAnimate)
			{
				//var animation = new Animation(v => { UpdateScrollOffset(GetDistance(Control.ViewportWidth, x, v), GetDistance(Control.ViewportHeight, y, v)); });

				//animation.Commit(_animatable, "ScrollTo", length: 500, easing: Easing.CubicInOut, finished: (v, d) =>
				//{
				//	UpdateScrollOffset(x, y);
				//	Controller.SendScrollFinished();
				//});
			}
			else
			{
				UpdateScrollOffset(x, y);
				Controller.SendScrollFinished();
			}
		}

		global::Avalonia.Thickness AddMargin(Thickness original, double left, double top, double right, double bottom)
		{
			return new global::Avalonia.Thickness(original.Left + left, original.Top + top, original.Right + right, original.Bottom + bottom);
		}

		// UAP ScrollView forces Content origin to be the same as the ScrollView origin.
		// This prevents Forms layout from emulating Padding and Margin by offsetting the origin. 
		// So we must actually set the UAP Margin property instead of emulating it with an origin offset. 
		// Not only that, but in UAP Padding and Margin are aliases with
		// the former living on the parent and the latter on the child. 
		// So that's why the UAP Margin is set to the sum of the Forms Padding and Forms Margin.
		void UpdateContentMargins()
		{
			if (!(Control.Content is Control element
				&& element is IVisualElementRenderer renderer
				&& renderer.Element is View contentView))
				return;

			var margin = contentView.Margin;
			var padding = Element.Padding;
			switch (Element.Orientation)
			{
				case ScrollOrientation.Horizontal:
					// need to add left/right margins
					element.Margin = AddMargin(margin, padding.Left, 0, padding.Right, 0);
					break;
				case ScrollOrientation.Vertical:
					// need to add top/bottom margins
					element.Margin = AddMargin(margin, 0, padding.Top, 0, padding.Bottom);
					break;
				case ScrollOrientation.Both:
					// need to add all margins
					element.Margin = AddMargin(margin, padding.Left, padding.Top, padding.Right, padding.Bottom);
					break;
			}
		}

		void UpdateMargins()
		{
			var element = Control.Content as Control;
			if (element == null)
				return;

			switch (Element.Orientation)
			{
				case ScrollOrientation.Horizontal:
					// need to add left/right margins
					element.Margin = new global::Avalonia.Thickness(Element.Padding.Left, 0, 10, 0);
					break;
				case ScrollOrientation.Vertical:
					// need to add top/bottom margins
					element.Margin = new global::Avalonia.Thickness(0, Element.Padding.Top, 0, Element.Padding.Bottom);
					break;
				case ScrollOrientation.Both:
					// need to add all margins
					element.Margin = new global::Avalonia.Thickness(Element.Padding.Left, Element.Padding.Top, Element.Padding.Right, Element.Padding.Bottom);
					break;
			}
		}

		void UpdateOrientation()
		{
			var orientation = Element.Orientation;
			if (orientation == ScrollOrientation.Horizontal || orientation == ScrollOrientation.Both)
				Control.HorizontalScrollBarVisibility = global::Avalonia.Controls.Primitives.ScrollBarVisibility.Auto;
			else
				Control.HorizontalScrollBarVisibility = global::Avalonia.Controls.Primitives.ScrollBarVisibility.Disabled;

			if (orientation == ScrollOrientation.Vertical || orientation == ScrollOrientation.Both)
				Control.VerticalScrollBarVisibility = global::Avalonia.Controls.Primitives.ScrollBarVisibility.Auto;
			else
				Control.VerticalScrollBarVisibility = global::Avalonia.Controls.Primitives.ScrollBarVisibility.Disabled;
		}

		void UpdateScrollOffset(double x, double y)
		{
			if (Element.Orientation == ScrollOrientation.Horizontal)
			{
				Control.Offset = Control.Offset.WithX(x);
			}
			else
			{
				Control.Offset = Control.Offset.WithY(y);
			}
		}

		void UpdateScrollPosition()
		{
			if (Element != null)
			{
				Controller.SetScrolledPosition(Control.Offset.X, Control.Offset.Y);
			}
		}

		void UpdateVerticalScrollBarVisibility()
		{
			Control.VerticalScrollBarVisibility = Element.VerticalScrollBarVisibility.ToNativeScrollBarVisibility();
		}

		void UpdateHorizontalScrollBarVisibility()
		{
			var orientation = Element.Orientation;
			if (orientation == ScrollOrientation.Horizontal || orientation == ScrollOrientation.Both)
				Control.HorizontalScrollBarVisibility = Element.HorizontalScrollBarVisibility.ToNativeScrollBarVisibility();
		}


		void CleanUp(ScrollView scrollView, ScrollViewer scrollViewer)
		{
			if (scrollView != null)
				scrollView.ScrollToRequested -= OnScrollToRequested;

			if (_currentView != null)
				_currentView.Cleanup();
		}


		bool _isDisposed;

		protected override void Dispose(bool disposing)
		{
			if (_isDisposed)
				return;

			if (disposing)
			{
				CleanUp(Element, Control);
			}

			_isDisposed = true;
			base.Dispose(disposing);
		}

		protected override global::Avalonia.Size ArrangeOverride(global::Avalonia.Size finalSize)
		{
			if (Element == null)
				return finalSize;

			Element.IsInNativeLayout = true;

			Control?.Arrange(new global::Avalonia.Rect(0, 0, finalSize.Width, finalSize.Height));

			Element.IsInNativeLayout = false;

			return finalSize;
		}

		protected override global::Avalonia.Size MeasureOverride(global::Avalonia.Size availableSize)
		{
			if (Element == null)
				return new global::Avalonia.Size(0, 0);

			double width = Math.Max(0, Element.Width);
			double height = Math.Max(0, Element.Height);
			var result = new global::Avalonia.Size(width, height);

			Control?.Measure(result);

			return result;
		}

	}
}