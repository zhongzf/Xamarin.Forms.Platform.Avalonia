using Avalonia.Controls;
using Avalonia.Forms.Controls;
using System;
using System.Collections.Generic;
using System.Text;

namespace Avalonia.Forms.Extensions
{
    public static class CommandsExtensions
    {
        public static IEnumerable<Control> Merge(this IEnumerable<Control> originalCommands, ContentControl contentControl, Func<DynamicContentPage, IEnumerable<Control>> callback)
        {
            List<Control> frameworkElements = new List<Control>();
            frameworkElements.AddRange(originalCommands);

            if (contentControl?.Content is DynamicContentPage page)
            {
                frameworkElements.AddRange(callback(page));
            }

            return frameworkElements;
        }
    }
}
