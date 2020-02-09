using System;
using System.Collections.Generic;
using System.Text;

namespace AvaloniaForms
{
    public partial class SelectionChangedEventArgs : EventArgs
    {
        public SelectionChangedEventArgs(object oldElement, object newElement)
        {
            OldElement = oldElement;
            NewElement = newElement;
        }

        public object NewElement { get; private set; }

        public object OldElement { get; private set; }
    }
}
