using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Threading;
using ReactiveUI;
using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using Xamarin.Forms.Platform.Avalonia.Extensions;

namespace Xamarin.Forms.Platform.Avalonia.Controls
{
    public class FormsCarouselPage : FormsMultiPage
    {
        public ReactiveCommand<Unit, Unit> NextCommand { get; }
        public ReactiveCommand<Unit, Unit> PreviousCommand { get; }

        private RepeatButton _nextButton;
        private RepeatButton _previousButton;

        private global::Avalonia.Controls.Grid _buttonPanel;

        public FormsCarouselPage()
        {
            this.NextCommand = ReactiveCommand.Create(this.OnNextExecuted, this.OnNextCanExecute());
            this.PreviousCommand = ReactiveCommand.Create(this.OnPreviousExecuted, this.OnPreviousCanExecute());
        }

        protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if(e.Property == SelectedIndexProperty)
            {
                OnSelectedIndexChanged(e);
            }
        }

        protected override void OnTemplateApplied(TemplateAppliedEventArgs e)
        {
            base.OnTemplateApplied(e);

            this._nextButton = this.Find<RepeatButton>("PART_NextButton", e);
            this._nextButton.Click += _nextButton_Click;
            this._previousButton = this.Find<RepeatButton>("PART_PreviousButton", e);
            this._previousButton.Click += _previousButton_Click;

            this._buttonPanel = this.Find<global::Avalonia.Controls.Grid>("PART_ButtonPanel", e);
        }

        private void _nextButton_Click(object sender, global::Avalonia.Interactivity.RoutedEventArgs e)
        {
            this.OnNextExecuted();
        }

        private void _previousButton_Click(object sender, global::Avalonia.Interactivity.RoutedEventArgs e)
        {
            this.OnPreviousExecuted();
        }

        protected override void Appearing()
        {
            base.Appearing();
            this.SelectedIndex = 0;
        }

        private IObservable<bool> OnPreviousCanExecute()
        {
            return this.WhenAnyValue(x => x.SelectedIndex, (selectedIndex) => selectedIndex > 0);
        }

        private void OnPreviousExecuted()
        {
            if (this.SelectedIndex > 0)
            {
                this.SelectedIndex -= 1;
                FormsContentControl.Transition = TransitionType.Right;
                DispatcherTimer.RunOnce(() => FormsContentControl.Transition = TransitionType.Default, TimeSpan.FromSeconds(0.2));
            }
        }

        private IObservable<bool> OnNextCanExecute()
        {
            return this.WhenAnyValue(x => x.ItemsSource, x => x.SelectedIndex, (itemsSource, selectedIndex) => selectedIndex < (itemsSource.Cast<object>().Count() - 1));
        }

        private void OnNextExecuted()
        {
            if (this.SelectedIndex < this.ItemsSource.Count - 1)
            {
                this.SelectedIndex += 1;
                FormsContentControl.Transition = TransitionType.Left;
                DispatcherTimer.RunOnce(() => FormsContentControl.Transition = TransitionType.Default, TimeSpan.FromSeconds(0.2));
            }
        }

        private void OnSelectedIndexChanged(AvaloniaPropertyChangedEventArgs e)
        {
            if (this.ItemsSource == null) return;
            var items = this.ItemsSource.Cast<object>();

            if ((int)e.NewValue >= 0 && (int)e.NewValue < items.Count())
            {
                this.SelectedItem = items.ElementAt((int)e.NewValue);
            }
        }
    }
}
