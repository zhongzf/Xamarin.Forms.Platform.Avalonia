using Avalonia.Controls;
using Avalonia.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Xamarin.Forms.Platform.Avalonia
{
    public class VisualElementRenderer<TElement, TNativeElement> : Panel, IVisualNativeElementRenderer, IDisposable
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

        public event EventHandler<VisualElementChangedEventArgs> ElementChanged
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


        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public virtual SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
        {
            throw new NotImplementedException();
        }

        public InputElement GetNativeElement()
        {
            throw new NotImplementedException();
        }

        public void SetElement(VisualElement element)
        {
            throw new NotImplementedException();
        }
    }
}
