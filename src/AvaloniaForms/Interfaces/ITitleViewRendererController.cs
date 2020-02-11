using Avalonia.Controls;
using AvaloniaForms.Controls;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvaloniaForms
{
    public interface ITitleViewRendererController
    {
        object TitleViewContent { get; }
        Control TitleViewPresenter { get; }
        bool TitleViewVisibility { get; set; }
        CommandBar CommandBar { get; }
    }
}
