using Avalonia;
using Avalonia.Controls.Templates;
using Avalonia.Markup.Xaml.Templates;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvaloniaForms.Interfaces
{
    public interface IDataTemplateSelector
    {
        IDataTemplate SelectTemplate(object item, object container);
    }
}
