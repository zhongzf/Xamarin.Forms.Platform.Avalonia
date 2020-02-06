using Avalonia.Controls;
using Avalonia.Forms.Interfaces;
using Avalonia.Styling;
using System;
using System.Collections.Generic;
using System.Text;

namespace Avalonia.Forms.Controls
{
    public class ListViewItem : ListBoxItem, IStyleable
    {
        Type IStyleable.StyleKey => typeof(ListBoxItem);
    }
}
