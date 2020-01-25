using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Xamarin.Forms.Internals;

namespace Xamarin.Forms.Platform.Avalonia
{
    internal sealed class AvaloniaExpressionSearch : IExpressionSearch
    {
        public List<T> FindObjects<T>(Expression expression) where T : class
        {
            throw new NotImplementedException();
        }
    }
}
