using Avalonia;
using Avalonia.Controls;
using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Xamarin.Forms.Platform.Avalonia
{
	internal static class OtherExtensions
	{
		public static void SetBinding(this AvaloniaObject self, AvaloniaProperty property, string path)
		{
			self.Bind(property, new global::Avalonia.Data.Binding { Path = path });
		}

		public static void SetBinding(this AvaloniaObject self, AvaloniaProperty property, string path, global::Avalonia.Data.Converters.IValueConverter converter)
		{
			self.Bind(property, new global::Avalonia.Data.Binding { Path = path, Converter = converter });
		}

		internal static InputScopeNameValue GetKeyboardButtonType(this ReturnType returnType)
		{
			switch (returnType)
			{
				case ReturnType.Default:
				case ReturnType.Done:
				case ReturnType.Go:
				case ReturnType.Next:
				case ReturnType.Send:
					return InputScopeNameValue.Default;
				case ReturnType.Search:
					return InputScopeNameValue.Search;
				default:
					throw new System.NotImplementedException($"ReturnType {returnType} not supported");
			}
		}

		internal static InputScope ToInputScope(this ReturnType returnType)
		{
			var scopeName = new InputScopeName()
			{
				NameValue = GetKeyboardButtonType(returnType)
			};

			var inputScope = new InputScope
			{
				Names = { scopeName }
			};

			return inputScope;
		}

		internal static global::Avalonia.Controls.Primitives.ScrollBarVisibility ToNativeScrollBarVisibility(this ScrollBarVisibility visibility)
		{
			switch (visibility)
			{
				case ScrollBarVisibility.Always:
					return global::Avalonia.Controls.Primitives.ScrollBarVisibility.Visible;
				case ScrollBarVisibility.Default:
					return global::Avalonia.Controls.Primitives.ScrollBarVisibility.Auto;
				case ScrollBarVisibility.Never:
					return global::Avalonia.Controls.Primitives.ScrollBarVisibility.Hidden;
				default:
					return global::Avalonia.Controls.Primitives.ScrollBarVisibility.Auto;
			}
		}

		public static T Clamp<T>(this T value, T min, T max) where T : IComparable<T>
		{
			if (value.CompareTo(min) < 0)
				return min;
			if (value.CompareTo(max) > 0)
				return max;
			return value;
		}


		internal static int ToEm(this double pt)
		{
			return Convert.ToInt32( pt * 0.0624f * 1000); //Coefficient for converting Pt to Em. The value is uniform spacing between characters, in units of 1/1000 of an em.
		}
	}
}