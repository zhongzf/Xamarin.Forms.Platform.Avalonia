using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using System.ComponentModel;
using Xamarin.Forms.Platform.Avalonia.Controls;
using Xamarin.Forms.Platform.Avalonia.Extensions;
using static System.String;
using AControl = Avalonia.Controls.Control;

namespace Xamarin.Forms.Platform.Avalonia
{
    public class EntryRenderer : ViewRenderer<Entry, FormsTextBox>
    {
        bool _fontApplied;
        bool _ignoreTextChange;
        Brush _placeholderDefaultBrush;

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            if (e.NewElement != null)
            {
                if (Control == null) // Construct and SetNativeControl and suscribe control event
                {
                    SetNativeControl(new FormsTextBox());
                    Control.LostFocus += OnTextBoxUnfocused;
                    Control.TextChanged += TextBoxOnTextChanged;
                    Control.KeyUp += TextBoxOnKeyUp;
                }

                // Update Control properties
                UpdateInputScope();
                UpdateIsPassword();
                UpdateText();
                UpdatePlaceholder();
                UpdateColor();
                UpdateFont();
                UpdateHorizontalTextAlignment();
                UpdateVerticalTextAlignment();
                UpdatePlaceholderColor();
                UpdateMaxLength();
                UpdateIsReadOnly();
            }

            base.OnElementChanged(e);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == Entry.TextProperty.PropertyName)
                UpdateText();
            else if (e.PropertyName == Entry.PlaceholderProperty.PropertyName)
                UpdatePlaceholder();
            else if (e.PropertyName == Entry.IsPasswordProperty.PropertyName)
                UpdateIsPassword();
            else if (e.PropertyName == Entry.TextColorProperty.PropertyName)
                UpdateColor();
            else if (e.PropertyName == InputView.KeyboardProperty.PropertyName)
                UpdateInputScope();
            else if (e.PropertyName == Entry.FontAttributesProperty.PropertyName)
                UpdateFont();
            else if (e.PropertyName == Entry.FontFamilyProperty.PropertyName)
                UpdateFont();
            else if (e.PropertyName == Entry.FontSizeProperty.PropertyName)
                UpdateFont();
            else if (e.PropertyName == Entry.HorizontalTextAlignmentProperty.PropertyName)
                UpdateHorizontalTextAlignment();
            else if (e.PropertyName == Entry.VerticalTextAlignmentProperty.PropertyName)
                UpdateVerticalTextAlignment();
            else if (e.PropertyName == Entry.PlaceholderColorProperty.PropertyName)
                UpdatePlaceholderColor();
            else if (e.PropertyName == InputView.MaxLengthProperty.PropertyName)
                UpdateMaxLength();
            else if (e.PropertyName == InputView.IsReadOnlyProperty.PropertyName)
                UpdateIsReadOnly();
        }

        internal override void OnModelFocusChangeRequested(object sender, VisualElement.FocusRequestArgs args)
        {
            if (args.Focus)
            {
                Control.Focus();
                args.Result = Control.IsFocused;
            }
            else
            {
                UnfocusControl(Control);
                args.Result = true;
            }
        }

        void OnTextBoxUnfocused(object sender, RoutedEventArgs e)
        {
            if (Element.TextColor.IsDefault)
                return;

            if (!IsNullOrEmpty(Element.Text))
                Control.Foreground = Element.TextColor.ToBrush();
        }

        void TextBoxOnKeyUp(object sender, KeyEventArgs keyEventArgs)
        {
            if (keyEventArgs.Key == Key.Enter)
                ((IEntryController)Element).SendCompleted();
        }

        void TextBoxOnTextChanged(object sender, global::Avalonia.Interactivity.RoutedEventArgs e)
        {
            // Signal to the UpdateText method that the change to TextProperty doesn't need to update the control
            // This prevents the cursor position from getting lost
            _ignoreTextChange = true;
            ((IElementController)Element).SetValueFromRenderer(Entry.TextProperty, Control.Text);

            // If an Entry.TextChanged handler modified the value of the Entry's text, the values could now be 
            // out-of-sync; re-sync them and fix TextBox cursor position
            string entryText = Element.Text;
            if (Control.Text != entryText)
            {
                Control.Text = entryText;
                if (Control.Text != null)
                {
                    var savedSelectionStart = Control.SelectionStart;
                    var len = Control.Text.Length;
                    Control.SelectionStart = savedSelectionStart > len ? len : savedSelectionStart;
                }
            }

            _ignoreTextChange = false;
        }

        void UpdateHorizontalTextAlignment()
        {
            if (Control == null)
                return;

            Control.TextAlignment = Element.HorizontalTextAlignment.ToNativeTextAlignment();
        }

        void UpdateVerticalTextAlignment()
        {
            if (Control == null)
                return;

            Control.VerticalAlignment = Element.VerticalTextAlignment.ToNativeVerticalAlignment();
        }

        void UpdateColor()
        {
            if (Control == null)
                return;

            Entry entry = Element;
            if (entry != null)
            {
                if (!entry.TextColor.IsDefault)
                {
                    Control.Foreground = entry.TextColor.ToBrush();
                }
            }
        }

        void UpdateFont()
        {
            if (Control == null)
                return;

            Entry entry = Element;

            if (entry == null)
                return;

            bool entryIsDefault = entry.FontFamily == null && entry.FontSize == Device.GetNamedSize(NamedSize.Default, typeof(Entry), true) && entry.FontAttributes == FontAttributes.None;

            if (entryIsDefault && !_fontApplied)
                return;

            if (entryIsDefault)
            {
                Control.ClearValue(global::Avalonia.Controls.Primitives.TemplatedControl.FontStyleProperty);
                Control.ClearValue(global::Avalonia.Controls.Primitives.TemplatedControl.FontSizeProperty);
                Control.ClearValue(global::Avalonia.Controls.Primitives.TemplatedControl.FontFamilyProperty);
                Control.ClearValue(global::Avalonia.Controls.Primitives.TemplatedControl.FontWeightProperty);
            }
            else
            {
                Control.ApplyFont(entry);
            }

            _fontApplied = true;
        }

        void UpdateInputScope()
        {
            Control.InputScope = Element.Keyboard.ToInputScope();
        }

        void UpdateIsPassword()
        {
            Control.IsPassword = Element.IsPassword;
        }

        void UpdatePlaceholder()
        {
            Control.PlaceholderText = Element.Placeholder ?? string.Empty;
        }

        void UpdatePlaceholderColor()
        {
            Color placeholderColor = Element.PlaceholderColor;

            if (placeholderColor.IsDefault)
            {
                if (_placeholderDefaultBrush == null)
                {
                    _placeholderDefaultBrush = (Brush)global::Avalonia.Controls.Primitives.TemplatedControl.ForegroundProperty.GetMetadata(typeof(FormsTextBox)).GetDefaultValue();
                }

                // Use the cached default brush
                Control.PlaceholderForegroundBrush = _placeholderDefaultBrush;
                return;
            }

            if (_placeholderDefaultBrush == null)
            {
                // Cache the default brush in case we need to set the color back to default
                _placeholderDefaultBrush = Control.PlaceholderForegroundBrush;
            }

            Control.PlaceholderForegroundBrush = placeholderColor.ToBrush();
        }

        void UpdateText()
        {
            // If the text property has changed because TextBoxOnTextChanged called SetValueFromRenderer,
            // we don't want to re-update the text and reset the cursor position
            if (_ignoreTextChange)
                return;

            if (Control.Text == Element.Text)
                return;

            Control.Text = Element.Text ?? "";
            Control.SelectionStart = string.IsNullOrEmpty(Control.Text) ? 0 : Control.Text.Length;
            Control.SelectionEnd = Control.SelectionStart;
        }

        void UpdateMaxLength()
        {
            Control.MaxLength = Element.MaxLength;

            var currentControlText = Control.Text;

            if (currentControlText != null && currentControlText.Length > Element.MaxLength)
                Control.Text = currentControlText.Substring(0, Element.MaxLength);
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
                    Control.LostFocus -= OnTextBoxUnfocused;
                    Control.TextChanged -= TextBoxOnTextChanged;
                    Control.KeyUp -= TextBoxOnKeyUp;
                }
            }

            _isDisposed = true;
            base.Dispose(disposing);
        }

        void UpdateIsReadOnly()
        {
            Control.IsReadOnly = Element.IsReadOnly;
        }
    }
}
