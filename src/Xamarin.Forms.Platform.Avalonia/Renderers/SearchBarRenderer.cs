﻿using Avalonia.Input;
using Avalonia.Media;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms.Platform.Avalonia.Controls;

namespace Xamarin.Forms.Platform.Avalonia
{
	public class SearchBarRenderer : ViewRenderer<SearchBar, FormsTextBox>
	{
		const string DefaultPlaceholder = "Search";
		Brush _defaultPlaceholderColorBrush;
		Brush _defaultTextColorBrush;
		bool _fontApplied;

		protected override void OnElementChanged(ElementChangedEventArgs<SearchBar> e)
		{
			if (e.NewElement != null)
			{
				if (Control == null) // construct and SetNativeControl and suscribe control event
				{
					//var scope = new InputScope();
					//var name = new InputScopeName();
					////name.NameValue = InputScopeNameValue.;
					//scope.Names.Add(name);
					
					SetNativeControl(new FormsTextBox { /*InputScope = scope*/ });
					Control.KeyUp += PhoneTextBoxOnKeyUp;
					Control.TextChanged += PhoneTextBoxOnTextChanged;
				}

				// Update control property 
				UpdateText();
				UpdatePlaceholder();
				UpdateHorizontalTextAlignment();
				UpdateVerticalTextAlignment();
				UpdateFont();
				UpdatePlaceholderColor();
				UpdateTextColor();
			}

			base.OnElementChanged(e);
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == SearchBar.TextProperty.PropertyName)
				UpdateText();
			else if (e.PropertyName == SearchBar.PlaceholderProperty.PropertyName)
				UpdatePlaceholder();
			else if (e.PropertyName == SearchBar.FontAttributesProperty.PropertyName)
				UpdateFont();
			else if (e.PropertyName == SearchBar.FontFamilyProperty.PropertyName)
				UpdateFont();
			else if (e.PropertyName == SearchBar.FontSizeProperty.PropertyName)
				UpdateFont();
			else if (e.PropertyName == SearchBar.HorizontalTextAlignmentProperty.PropertyName)
				UpdateHorizontalTextAlignment();
			else if (e.PropertyName == SearchBar.VerticalTextAlignmentProperty.PropertyName)
				UpdateVerticalTextAlignment();
			else if (e.PropertyName == SearchBar.PlaceholderColorProperty.PropertyName)
				UpdatePlaceholderColor();
			else if (e.PropertyName == SearchBar.TextColorProperty.PropertyName)
				UpdateTextColor();
		}
		
		void PhoneTextBoxOnKeyUp(object sender, KeyEventArgs keyEventArgs)
		{
			if (keyEventArgs.Key == Key.Enter)
			{
				((ISearchBarController)Element).OnSearchButtonPressed();
			}
		}

		void PhoneTextBoxOnTextChanged(object sender, global::Avalonia.Interactivity.RoutedEventArgs textChangedEventArgs)
		{
			((IElementController)Element).SetValueFromRenderer(SearchBar.TextProperty, Control.Text);
		}

		void UpdateHorizontalTextAlignment()
		{
			Control.TextAlignment = Element.HorizontalTextAlignment.ToNativeTextAlignment();
		}

		void UpdateVerticalTextAlignment()
		{
			Control.VerticalAlignment = Element.VerticalTextAlignment.ToNativeVerticalAlignment();
		}

		void UpdateFont()
		{
			if (Control == null)
				return;

			SearchBar searchbar = Element;

			if (searchbar == null)
				return;

			bool searchbarIsDefault = searchbar.FontFamily == null && searchbar.FontSize == Device.GetNamedSize(NamedSize.Default, typeof(SearchBar), true) && searchbar.FontAttributes == FontAttributes.None;

			if (searchbarIsDefault && !_fontApplied)
				return;

			if (searchbarIsDefault)
			{
				Control.ClearValue(global::Avalonia.Controls.Primitives.TemplatedControl.FontStyleProperty);
				Control.ClearValue(global::Avalonia.Controls.Primitives.TemplatedControl.FontSizeProperty);
				Control.ClearValue(global::Avalonia.Controls.Primitives.TemplatedControl.FontFamilyProperty);
				Control.ClearValue(global::Avalonia.Controls.Primitives.TemplatedControl.FontWeightProperty);
			}
			else
			{
				Control.ApplyFont(searchbar);
			}

			_fontApplied = true;
		}

		void UpdatePlaceholder()
		{
			Control.PlaceholderText = Element.Placeholder ?? DefaultPlaceholder;
		}

		void UpdatePlaceholderColor()
		{
			Color placeholderColor = Element.PlaceholderColor;

			if (placeholderColor.IsDefault)
			{
				if (_defaultPlaceholderColorBrush == null)
				{
					_defaultPlaceholderColorBrush = (Brush)global::Avalonia.Controls.Primitives.TemplatedControl.ForegroundProperty.GetMetadata(typeof(FormsTextBox)).GetDefaultValue();
				}
				Control.PlaceholderForegroundBrush = _defaultPlaceholderColorBrush;
				return;
			}

			if (_defaultPlaceholderColorBrush == null)
				_defaultPlaceholderColorBrush = Control.PlaceholderForegroundBrush;

			Control.PlaceholderForegroundBrush = placeholderColor.ToBrush();
		}

		void UpdateText()
		{
			Control.Text = Element.Text ?? "";
		}

		void UpdateTextColor()
		{
			Color textColor = Element.TextColor;

			if (textColor.IsDefault)
			{
				if (_defaultTextColorBrush == null)
					return;

				Control.Foreground = _defaultTextColorBrush;
			}

			if (_defaultTextColorBrush == null)
			{
				_defaultTextColorBrush = (Brush)Control.Foreground;
			}

			Control.Foreground = textColor.ToBrush();
		}

		bool _isDisposed;

		protected override void Dispose(bool disposing)
		{
			if (_isDisposed)
				return;

			if (disposing)
			{
				if (Control != null)
				{
					Control.KeyUp -= PhoneTextBoxOnKeyUp;
					Control.TextChanged -= PhoneTextBoxOnTextChanged;
				}
			}

			_isDisposed = true;
			base.Dispose(disposing);
		}
	}
}