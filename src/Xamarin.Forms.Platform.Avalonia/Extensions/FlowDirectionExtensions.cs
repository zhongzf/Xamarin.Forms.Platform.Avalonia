using Avalonia.Controls;

namespace Xamarin.Forms.Platform.Avalonia
{
	internal static class FlowDirectionExtensions
	{
		internal static void UpdateFlowDirection(this Control control, IVisualElementController controller)
		{
			if (controller == null || control == null)
				return;

			//if (controller.EffectiveFlowDirection.IsRightToLeft())
			//	control.FlowDirection = WFlowDirection.RightToLeft;
			//else if (controller.EffectiveFlowDirection.IsLeftToRight())
			//	control.FlowDirection = WFlowDirection.LeftToRight;
		}

		internal static void UpdateTextAlignment(this TextBox control, IVisualElementController controller)
		{
			if (controller == null || control == null)
				return;

			if (controller.EffectiveFlowDirection.IsRightToLeft())
				control.TextAlignment = global::Avalonia.Media.TextAlignment.Right;
			else if (controller.EffectiveFlowDirection.IsLeftToRight())
				control.TextAlignment = global::Avalonia.Media.TextAlignment.Left;
		}
	}
}
