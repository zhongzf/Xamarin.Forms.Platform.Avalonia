using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Xamarin.Forms.Platform.Avalonia
{
    public class VisualElementRenderer<TElement, TNativeElement> : AvaloniaForms.Controls.Panel, IVisualNativeElementRenderer, IDisposable
        where TElement : VisualElement
        where TNativeElement : Control
    {
        public TElement Element { get; private set; }
        public TNativeElement Control { get; private set; }

        VisualElement IVisualElementRenderer.Element => Element;
        public Control ContainerElement => this;

        EventHandler<VisualElementChangedEventArgs> _elementChangedHandlers;
        event EventHandler<PropertyChangedEventArgs> _elementPropertyChanged;
        event EventHandler _controlChanging;
        event EventHandler _controlChanged;

        public event EventHandler<PropertyChangedEventArgs> ElementPropertyChanged
        {
            add => _elementPropertyChanged += value;
            remove => _elementPropertyChanged -= value;
        }

        public event EventHandler ControlChanging
        {
            add { _controlChanging += value; }
            remove { _controlChanging -= value; }
        }

        public event EventHandler ControlChanged
        {
            add { _controlChanged += value; }
            remove { _controlChanged -= value; }
        }


        event EventHandler<VisualElementChangedEventArgs> IVisualElementRenderer.ElementChanged
        {
            add
            {
                if (_elementChangedHandlers == null)
                {
                    _elementChangedHandlers = value;
                }
                else
                {
                    _elementChangedHandlers = (EventHandler<VisualElementChangedEventArgs>)Delegate.Combine(_elementChangedHandlers, value);
                }
            }

            remove
            {
                _elementChangedHandlers = (EventHandler<VisualElementChangedEventArgs>)Delegate.Remove(_elementChangedHandlers, value);
            }
        }
        public event EventHandler<ElementChangedEventArgs<TElement>> ElementChanged;

        protected virtual void OnElementChanged(ElementChangedEventArgs<TElement> e)
        {
            var args = new VisualElementChangedEventArgs(e.OldElement, e.NewElement);
            _elementChangedHandlers?.Invoke(this, args);
            ElementChanged?.Invoke(this, e);
        }

        protected bool ArrangeNativeChildren { get; set; }
        IElementController ElementController => Element as IElementController;
        Canvas _backgroundLayer;


        protected bool AutoPackage { get; set; } = true;
        VisualElementPackager Packager { get; set; }

        protected bool AutoTrack { get; set; } = true;
        VisualElementTracker<TElement, TNativeElement> _tracker;
        protected VisualElementTracker<TElement, TNativeElement> Tracker
        {
            get { return _tracker; }
            set
            {
                if (_tracker == value)
                    return;

                if (_tracker != null)
                {
                    _tracker.Dispose();
                    _tracker.Updated -= OnTrackerUpdated;
                }

                _tracker = value;

                if (_tracker != null)
                {
                    _tracker.Updated += OnTrackerUpdated;
                    UpdateTracker();
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing || _disposed)
                return;

            _disposed = true;

            Tracker?.Dispose();
            Tracker = null;

            Packager?.Dispose();
            Packager = null;

            SetNativeControl(null);
            SetElement(null);
        }

        public virtual SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
        {
            if (Children.Count == 0 || Control == null)
                return new SizeRequest();

            var constraint = new global::Avalonia.Size(widthConstraint, heightConstraint);
            TNativeElement child = Control;

            child.Measure(constraint);
            var result = new Size(Math.Ceiling(child.DesiredSize.Width), Math.Ceiling(child.DesiredSize.Height));

            return new SizeRequest(result);
        }

        public InputElement GetNativeElement()
        {
            return Control;
        }

        public void SetElement(VisualElement element)
        {
            TElement oldElement = Element;
            Element = (TElement)element;

            if (oldElement != null)
            {
                oldElement.PropertyChanged -= OnElementPropertyChanged;
            }

            if (element != null)
            {
                Element.PropertyChanged += OnElementPropertyChanged;

                if (AutoPackage && Packager == null)
                {
                    Packager = new VisualElementPackager(this);
                }

                if (AutoTrack && Tracker == null)
                {
                    Tracker = new VisualElementTracker<TElement, TNativeElement>();
                }

                if (Packager != null)
                    Packager.Load();
            }

            OnElementChanged(new ElementChangedEventArgs<TElement>(oldElement, Element));
        }

        protected virtual void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // TODO: 

            _elementPropertyChanged?.Invoke(this, e);
        }

        #region ArrangeOverride & MeasureOverride
        protected override global::Avalonia.Size ArrangeOverride(global::Avalonia.Size finalSize)
        {
            if (Element == null || finalSize.Width * finalSize.Height == 0)
                return finalSize;

            Element.IsInNativeLayout = true;

            var myRect = new Rect(0, 0, finalSize.Width, finalSize.Height);

            if (Control != null)
            {
                Control.Arrange(myRect);
            }

            List<IControl> arrangedChildren = null;
            for (var i = 0; i < ElementController.LogicalChildren.Count; i++)
            {
                var child = ElementController.LogicalChildren[i] as VisualElement;
                if (child == null)
                    continue;
                IVisualElementRenderer renderer = Platform.GetRenderer(child);
                if (renderer == null)
                    continue;
                Rectangle bounds = child.Bounds;

                renderer.ContainerElement.Arrange(new Rect(bounds.X, bounds.Y, Math.Max(0, bounds.Width), Math.Max(0, bounds.Height)));

                if (ArrangeNativeChildren)
                {
                    if (arrangedChildren == null)
                        arrangedChildren = new List<IControl>();
                    arrangedChildren.Add(renderer.ContainerElement);
                }
            }

            if (ArrangeNativeChildren)
            {
                // in the event that a custom renderer has added native controls,
                // we need to be sure to arrange them so that they are laid out.
                var nativeChildren = Children;
                for (int i = 0; i < nativeChildren.Count; i++)
                {
                    var nativeChild = nativeChildren[i];
                    if (arrangedChildren?.Contains(nativeChild) == true)
                        // don't try to rearrange renderers that were just arranged, 
                        // lest you suffer a layout cycle
                        continue;
                    else
                        nativeChild.Arrange(myRect);
                }
            }

            _backgroundLayer?.Arrange(myRect);

            Element.IsInNativeLayout = false;

            return finalSize;
        }

        protected override global::Avalonia.Size MeasureOverride(global::Avalonia.Size availableSize)
        {
            if (Element == null || availableSize.Width * availableSize.Height == 0)
                return new global::Avalonia.Size(0, 0);

            Element.IsInNativeLayout = true;

            for (var i = 0; i < ElementController.LogicalChildren.Count; i++)
            {
                var child = ElementController.LogicalChildren[i] as VisualElement;
                if (child == null)
                    continue;
                IVisualElementRenderer renderer = Platform.GetRenderer(child);
                if (renderer == null)
                    continue;

                renderer.ContainerElement.Measure(availableSize);
            }

            double width = Math.Max(0, Element.Width);
            double height = Math.Max(0, Element.Height);
            var result = new global::Avalonia.Size(width, height);
            if (Control != null)
            {
                double w = Element.Width;
                double h = Element.Height;
                if (w == -1)
                {
                    w = availableSize.Width;
                }
                if (h == -1)
                {
                    h = availableSize.Height;
                }
                w = Math.Max(0, w);
                h = Math.Max(0, h);
                Control.Measure(new global::Avalonia.Size(w, h));
            }

            Element.IsInNativeLayout = false;

            return result;
        }
        #endregion

        protected void SetNativeControl(TNativeElement control)
        {
            _controlChanging?.Invoke(this, EventArgs.Empty);

            TNativeElement oldControl = Control;
            Control = control;

            if (oldControl != null)
            {
                Children.Remove(oldControl);
            }

            if (control == null)
            {
                _controlChanged?.Invoke(this, EventArgs.Empty);
                return;
            }

            if (Element == null)
                throw new InvalidOperationException(
                    "Cannot assign a native control without an Element; Renderer unbound and/or disposed. " +
                    "Please consult Xamarin.Forms renderers for reference implementation of OnElementChanged.");

            Element.IsNativeStateConsistent = false;

            Children.Add(control);

            _controlChanged?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void UpdateNativeControl()
        {
        }

        void OnTrackerUpdated(object sender, EventArgs e)
        {
            UpdateNativeControl();
        }

        void UpdateTracker()
        {
            if (_tracker == null)
                return;

            _tracker.Control = Control;
            _tracker.Element = Element;
            _tracker.Container = ContainerElement;
        }
    }
}
