using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Threading;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Xamarin.Forms.Platform.Avalonia.Controls
{
    /// <summary>
    ///     An intermediate class for injecting bindings for things the default
    ///     textbox doesn't allow us to bind/modify
    /// </summary>
    public class FormsTextBox : TextBox
    {
        const char ObfuscationCharacter = '●';

        public static readonly StyledProperty<string> PlaceholderTextProperty = AvaloniaProperty.Register<FormsTextBox, string>(nameof(PlaceholderText));
        public static readonly StyledProperty<Brush> PlaceholderForegroundBrushProperty = AvaloniaProperty.Register<FormsTextBox, Brush>(nameof(PlaceholderForegroundBrush));
        public static readonly StyledProperty<bool> IsPasswordProperty = AvaloniaProperty.Register<FormsTextBox, bool>(nameof(IsPassword));
        public new static readonly StyledProperty<string> TextProperty = AvaloniaProperty.Register<FormsTextBox, string>(nameof(Text));
        protected internal static readonly StyledProperty<string> DisabledTextProperty = AvaloniaProperty.Register<FormsTextBox, string>(nameof(DisabledText));

        static InputScope s_passwordInputScope;
        InputScope _cachedInputScope;
        CancellationTokenSource _cts;
        bool _internalChangeFlag;
        int _cachedSelectionLength;

        //
        // Summary:
        //     Occurs when the control loses focus.
        public event EventHandler<RoutedEventArgs> TextChanged;

        public FormsTextBox()
        {
            TextProperty.Changed.AddClassHandler<FormsTextBox>((x, e) => x.OnTextPropertyChanged(e));
            IsPasswordProperty.Changed.AddClassHandler<FormsTextBox>((x, e) => x.OnIsPasswordChanged(e));
        }

        protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Property == TextBox.TextProperty)
            {
                OnTextChanged(e);
            }
            if (e.Property == SelectionStartProperty || e.Property == SelectionEndProperty)
            {
                OnSelectionChanged(e);
            }
        }

        public bool IsPassword
        {
            get { return (bool)GetValue(IsPasswordProperty); }
            set { SetValue(IsPasswordProperty, value); }
        }

        public string PlaceholderText
        {
            get { return (string)GetValue(PlaceholderTextProperty); }
            set { SetValue(PlaceholderTextProperty, value); }
        }

        public Brush PlaceholderForegroundBrush
        {
            get { return (Brush)GetValue(PlaceholderForegroundBrushProperty); }
            set { SetValue(PlaceholderForegroundBrushProperty, value); }
        }

        public new string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        protected internal string DisabledText
        {
            get { return (string)GetValue(DisabledTextProperty); }
            set { SetValue(DisabledTextProperty, value); }
        }

        public InputScope InputScope { get; set; }

        static InputScope PasswordInputScope
        {
            get
            {
                if (s_passwordInputScope != null)
                    return s_passwordInputScope;

                s_passwordInputScope = new InputScope();
                var name = new InputScopeName { NameValue = InputScopeNameValue.Default };
                s_passwordInputScope.Names.Add(name);

                return s_passwordInputScope;
            }
        }

        void OnIsPasswordChanged(AvaloniaPropertyChangedEventArgs e)
        {
            var textBox = this;
            if (IsPassword)
            {
                textBox.PasswordChar = ObfuscationCharacter;
            }
            else
            {
                textBox.PasswordChar = (char)0;
            }
            textBox.UpdateInputScope();
            textBox.SyncBaseText();
        }

        void OnSelectionChanged(AvaloniaPropertyChangedEventArgs e)
        {
            // Cache this value for later use as explained in OnKeyDown below
            var selectionLength = SelectionEnd - SelectionStart;
            _cachedSelectionLength = selectionLength;
        }

        // Because the implementation of a password entry is based around inheriting from TextBox (via FormsTextBox), there
        // are some inaccuracies in the behavior. OnKeyDown is what needs to be used for a workaround in this case because 
        // there's no easy way to disable specific keyboard shortcuts in a TextBox, so key presses are being intercepted and 
        // handled accordingly.
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (IsPassword)
            {
                // The ctrlDown flag is used to track if the Ctrl key is pressed; if it's actively being used and the most recent
                // key to trigger OnKeyDown, then treat it as handled.
                var ctrlDown = (e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl); // && e.IsDown;

                // The shift, tab, and directional (Home/End/PgUp/PgDown included) keys can be used to select text and should otherwise
                // be ignored.
                if (
                    e.Key == Key.LeftShift ||
                    e.Key == Key.RightShift ||
                    e.Key == Key.Tab ||
                    e.Key == Key.Left ||
                    e.Key == Key.Right ||
                    e.Key == Key.Up ||
                    e.Key == Key.Down ||
                    e.Key == Key.Home ||
                    e.Key == Key.End ||
                    e.Key == Key.PageUp ||
                    e.Key == Key.PageDown)
                {
                    base.OnKeyDown(e);
                    return;
                }
                // For anything else, continue on (calling base.OnKeyDown) and then if Ctrl is still being pressed, do nothing about it.
                // The tricky part here is that the SelectionLength value needs to be cached because in an example where the user entered
                // '123' into the field and selects all of it, the moment that any character key is pressed to replace the entire string,
                // the SelectionLength is equal to zero, which is not what's desired. Entering a key will thus remove the selected number
                // of characters from the Text value. OnKeyDown is fortunately called before OnSelectionChanged which enables this.
                else
                {
                    // If the C or X keys (copy/cut) are pressed while Ctrl is active, ignore handing them at all. Undo and Redo (Z/Y) should 
                    // be ignored as well as this emulates the regular behavior of a PasswordBox.
                    if ((e.Key == Key.C || e.Key == Key.X || e.Key == Key.Z || e.Key == Key.Y) && ctrlDown)
                    {
                        e.Handled = false;
                        return;
                    }

                    base.OnKeyDown(e);
                    if (_cachedSelectionLength > 0 && !ctrlDown)
                    {
                        var savedSelectionStart = SelectionStart;
                        Text = Text.Remove(SelectionStart, _cachedSelectionLength);
                        SelectionStart = savedSelectionStart;
                    }
                }
            }
            else
            {
                base.OnKeyDown(e);
            }
        }

        void OnTextChanged(AvaloniaPropertyChangedEventArgs textChangedEventArgs)
        {
            if (base.Text != Text)
            {
                // Not in password mode, so we just need to make the "real" Text match
                // what's in the textbox; the internalChange flag keeps the TextProperty
                // synchronization from happening 
                _internalChangeFlag = true;
                Text = base.Text;
                _internalChangeFlag = false;
            }
        }

        void SyncBaseText()
        {
            if (_internalChangeFlag)
                return;
            var savedSelectionStart = SelectionStart;
            base.Text = Text;
            DisabledText = base.Text;
            var len = base.Text?.Length ?? 0;
            SelectionStart = savedSelectionStart > len ? len : savedSelectionStart;
        }

        protected virtual void OnTextPropertyChanged(AvaloniaPropertyChangedEventArgs e)
        {
            SyncBaseText();
            TextChanged?.Invoke(this, new RoutedEventArgs());
        }

        void UpdateInputScope()
        {
            if (IsPassword)
            {
                _cachedInputScope = InputScope;
                InputScope = PasswordInputScope; // We don't want suggestions turned on if we're in password mode
            }
            else
            {
                InputScope = _cachedInputScope;
            }
        }
    }
}