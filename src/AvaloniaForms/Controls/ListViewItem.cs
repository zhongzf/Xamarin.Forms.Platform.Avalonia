using Avalonia.Controls;
using Avalonia.Styling;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvaloniaForms.Controls
{
    public class ListViewItem : ListBoxItem, IStyleable
    {
        Type IStyleable.StyleKey => typeof(ListBoxItem);
    }
}
