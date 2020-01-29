using Avalonia.Controls;
using Avalonia.Layout;
using System;
using System.Collections.Generic;
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

            var stepX = 1;
            var stepY = 1;
            for (var i = 0; i < ElementController.LogicalChildren.Count; i++)
            {
                var child = ElementController.LogicalChildren[i] as VisualElement;
                if (child == null)
                    continue;

                IVisualElementRenderer renderer = Platform.GetRenderer(child);
                if (renderer == null)
                    continue;
                Rectangle bounds = child.Bounds;
                var control = renderer.GetNativeElement();
                var width = Math.Max(bounds.Width, control.DesiredSize.Width);
                var height = Math.Max(bounds.Height, control.DesiredSize.Height);
                if (stepX != 1 && stepY != 1 && stepX != 0 && stepY != 0)
                {
                    control.Width = width = Math.Ceiling(width / stepX) * stepX;
                    control.Height = height = Math.Ceiling(height / stepY) * stepY;
                }
                var childRect = new global::Avalonia.Rect(bounds.X, bounds.Y, width, height);
                control.Arrange(childRect);
            }

            Element.IsInNativeLayout = false;

            return finalSize;
        }

        protected override global::Avalonia.Size MeasureOverride(global::Avalonia.Size availableSize)
        {
            var baseDesiredSize = base.MeasureOverride(availableSize);
            bool hasVisibleChild = false;

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

                Control control = renderer.GetNativeElement();
                bool isVisible = control.IsVisible;

                if (isVisible && !hasVisibleChild)
                {
                    hasVisibleChild = true;
                }
                var controlBounds = control.Bounds;
                if (controlBounds.Width != child.Width || controlBounds.Height != child.Height)
                {
                    double width = child.Width <= -1 ? availableSize.Width : child.Width;
                    double height = child.Height <= -1 ? availableSize.Height : child.Height;
                    control.Measure(new global::Avalonia.Size(width, height));
                    var childDesiredSize = control.DesiredSize;
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

            //var finalDesiredSize = new global::Avalonia.Size(Math.Max(baseDesiredSize.Width, result.Width), Math.Max(baseDesiredSize.Height, result.Height));
            //return finalDesiredSize;
            return result;
        }
    }
}
