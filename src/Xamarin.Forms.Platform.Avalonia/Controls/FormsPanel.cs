using Avalonia.Controls;
using Avalonia.Layout;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.Forms.Platform.Avalonia.Controls
{
    public class FormsPanel : Panel
    {
        IElementController ElementController => Element as IElementController;

        public Layout Element { get; private set; }

        public FormsPanel(Layout element)
        {
            Element = element;
        }

        protected override global::Avalonia.Size ArrangeOverride(global::Avalonia.Size finalSize)
        {
            if (Element == null)
                return finalSize;

            Element.IsInNativeLayout = true;

            try
            {
                var stepX = 1;
                var stepY = 1;
                for (var i = 0; i < ElementController.LogicalChildren.Count; i++)
                {
                    var child = ElementController.LogicalChildren[i] as VisualElement;
                    if (child == null)
                        continue;

                    var childRenderer = Platform.GetRenderer(child);
                    if (childRenderer == null)
                        continue;

                    var childControl = childRenderer.GetNativeElement();
                    var childBounds = child.Bounds;
                    var childWidth = Math.Max(0, childBounds.Width);
                    var childHeight = Math.Max(0, childBounds.Height);
                    if (stepX != 1 && stepY != 1 && stepX != 0 && stepY != 0)
                    {
                        childControl.Width = childWidth = Math.Ceiling(childWidth / stepX) * stepX;
                        childControl.Height = childHeight = Math.Ceiling(childHeight / stepY) * stepY;
                    }
                    childControl.Arrange(new global::Avalonia.Rect(childBounds.X, childBounds.Y, childWidth, childHeight));
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
            }

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

                var childRenderer = Platform.GetRenderer(child);
                if (childRenderer == null)
                    continue;

                var childControl = childRenderer.GetNativeElement();
                if (childControl.Width != child.Width || childControl.Height != child.Height)
                {
                    var parentBounds = Bounds;
                    double width = child.Width <= -1 ? parentBounds.Width : child.Width;
                    double height = child.Height <= -1 ? parentBounds.Height : child.Height;
                    childControl.Measure(new global::Avalonia.Size(width, height));
                }
            }

            global::Avalonia.Size result;
            if (double.IsInfinity(availableSize.Width) || double.IsPositiveInfinity(availableSize.Height))
            {
                Size request = Element.Measure(availableSize.Width, availableSize.Height, MeasureFlags.IncludeMargins).Request;
                result = new global::Avalonia.Size(request.Width, request.Height);
            }
            else
            {
                result = availableSize;
            }
            Element.IsInNativeLayout = false;

            if (Double.IsPositiveInfinity(result.Height))
                result = result.WithHeight(0.0);
            if (Double.IsPositiveInfinity(result.Width))
                result = result.WithWidth(0.0);

            return result;
        }
    }
}
