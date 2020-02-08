using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Styling;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvaloniaForms.Controls
{
    public class TimePicker : TextBox, IStyleable
    {
        public static readonly StyledProperty<TimeSpan?> TimeProperty = AvaloniaProperty.Register<TimePicker, TimeSpan?>(nameof(Time));
        public static readonly StyledProperty<string> TimeFormatProperty = AvaloniaProperty.Register<TimePicker, string>(nameof(TimeFormat));

        static TimePicker()
        {
            TimeProperty.Changed.AddClassHandler<TimePicker>((x, e) => x.OnTimePropertyChanged(e));
            TimeFormatProperty.Changed.AddClassHandler<TimePicker>((x, e) => x.OnTimeFormatPropertyChanged(e));
        }

        Type IStyleable.StyleKey => typeof(TextBox);

        public TimeSpan? Time
        {
            get { return (TimeSpan?)GetValue(TimeProperty); }
            set { SetValue(TimeProperty, value); }
        }

        public String TimeFormat
        {
            get { return (String)GetValue(TimeFormatProperty); }
            set { SetValue(TimeFormatProperty, value); }
        }

        public delegate void TimeChangedEventHandler(object sender, TimeChangedEventArgs e);
        public event TimeChangedEventHandler TimeChanged;


        protected override void OnTemplateApplied(TemplateAppliedEventArgs e)
        {
            base.OnTemplateApplied(e);
            SetText();
        }

        private void SetText()
        {
            if (Time == null)
            {
                Text = null;
            }
            else
            {
                var dateTime = new DateTime(Time.Value.Ticks);
                String text = dateTime.ToString(String.IsNullOrWhiteSpace(TimeFormat) ? @"hh\:mm" : TimeFormat.ToLower());
                if (text.CompareTo(Text) != 0)
                {
                    Text = text;
                }
            }
        }

        private void SetTime()
        {
            DateTime dateTime = DateTime.MinValue;
            String timeFormat = String.IsNullOrWhiteSpace(TimeFormat) ? @"hh\:mm" : TimeFormat.ToLower();

            if (DateTime.TryParseExact(Text, timeFormat, null, System.Globalization.DateTimeStyles.None, out dateTime))
            {
                if ((Time == null) || (Time != null && Time.Value.CompareTo(dateTime.TimeOfDay) != 0))
                {
                    if (dateTime.TimeOfDay < TimeSpan.FromHours(24) && dateTime.TimeOfDay > TimeSpan.Zero)
                    {
                        Time = dateTime.TimeOfDay;
                    }
                    else
                    {
                        SetText();
                    }
                }
            }
            else
            {
                SetText();
            }
        }

        #region Overrides
        protected override void OnLostFocus(RoutedEventArgs e)
        {
            SetTime();
            base.OnLostFocus(e);
        }
        #endregion

        #region Property Changes
        private void OnTimePropertyChanged(AvaloniaPropertyChangedEventArgs e)
        {
            OnTimeChanged(e.OldValue as TimeSpan?, e.NewValue as TimeSpan?);
        }

        protected virtual void OnTimeChanged(TimeSpan? oldValue, TimeSpan? newValue)
        {
            SetText();

            TimeChanged?.Invoke(this, new TimeChangedEventArgs(oldValue, newValue));
        }

        private void OnTimeFormatPropertyChanged(AvaloniaPropertyChangedEventArgs e)
        {
            OnTimeFormatChanged();
        }

        protected virtual void OnTimeFormatChanged()
        {
            SetText();
        }
        #endregion
    }

    public class TimeChangedEventArgs : EventArgs
    {
        private TimeSpan? _oldTime;
        private TimeSpan? _newTime;

        public TimeSpan? OldTime
        {
            get { return _oldTime; }
        }

        public TimeSpan? NewTime
        {
            get { return _newTime; }
        }

        public TimeChangedEventArgs(TimeSpan? oldTime, TimeSpan? newTime)
        {
            _oldTime = oldTime;
            _newTime = newTime;
        }
    }
}