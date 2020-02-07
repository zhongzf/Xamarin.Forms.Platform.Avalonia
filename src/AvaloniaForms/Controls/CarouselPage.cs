using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReactiveUI;
using Avalonia;
using Avalonia.Styling;

namespace AvaloniaForms.Controls
{
    public class CarouselPage : MultiContentPage, IStyleable
    {
        Type IStyleable.StyleKey => typeof(CarouselPage);

        public RepeatButton NextButton { get; private set; }
        public RepeatButton PreviousButton { get; private set; }

        public CarouselPage()
        {
        }

        protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Property == SelectedIndexProperty)
            {
                OnSelectedIndexChanged(e);
            }
        }

        protected override void OnTemplateApplied(TemplateAppliedEventArgs e)
        {
            base.OnTemplateApplied(e);

            NextButton = e.NameScope.Find<RepeatButton>("PART_NextButton");
            NextButton.Click += NextButton_Click;
            PreviousButton = e.NameScope.Find<RepeatButton>("PART_PreviousButton");
            PreviousButton.Click += PreviousButton_Click;
        }

        private void PreviousButton_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            this.OnPreviousExecuted();
        }

        private void NextButton_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            this.OnNextExecuted();
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
                ResetTransition();
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
                ResetTransition();
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

        protected virtual void ResetTransition()
        {
            DispatcherTimer.RunOnce(() =>
            {
                ContentControl.Transition = TransitionType.Default;
            }, TimeSpan.FromSeconds(0.2));
        }
    }
}
