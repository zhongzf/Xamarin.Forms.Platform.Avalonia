using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Forms.Controls;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;

namespace Xamarin.Forms.Platform.Avalonia.Controls
{
    public class FormsCarouselView : FormsMultiView
    {
        public ReactiveCommand<Unit, Unit> NextCommand { get; }
        public ReactiveCommand<Unit, Unit> PreviousCommand { get; }

        private RepeatButton _nextButton;
        private RepeatButton _previousButton;

        public FormsCarouselView()
        {
            SelectedIndexProperty.Changed.AddClassHandler<FormsCarouselView>((x, e) => x.OnSelectedIndexChanged(e));

            this.NextCommand = ReactiveCommand.Create(this.OnNextExecuted, this.OnNextCanExecute());
            this.PreviousCommand = ReactiveCommand.Create(this.OnPreviousExecuted, this.OnPreviousCanExecute());
        }

        protected override void OnTemplateApplied(TemplateAppliedEventArgs e)
        {
            base.OnTemplateApplied(e);

            this._nextButton = e.NameScope.Find<RepeatButton>("PART_NextButton");
            this._nextButton.Click += _nextButton_Click;
            this._previousButton = e.NameScope.Find<RepeatButton>("PART_PreviousButton");
            this._previousButton.Click += _previousButton_Click;
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
                ContentControl.Transition = TransitionType.Right;
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
                ContentControl.Transition = TransitionType.Left;
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
