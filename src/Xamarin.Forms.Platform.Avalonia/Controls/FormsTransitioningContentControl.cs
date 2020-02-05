using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Styling;
using Avalonia.Threading;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using Xamarin.Forms.Platform.Avalonia.Extensions;

namespace Xamarin.Forms.Platform.Avalonia.Controls
{
    /// <summary>
    /// enumeration for the different transition types
    /// </summary>
    public enum TransitionType
    {
        /// <summary>
        /// Use the VisualState DefaultTransition
        /// </summary>
        Default,
        /// <summary>
        /// Use the VisualState Normal
        /// </summary>
        Normal,
        /// <summary>
        /// Use the VisualState UpTransition
        /// </summary>
        Up,
        /// <summary>
        /// Use the VisualState DownTransition
        /// </summary>
        Down,
        /// <summary>
        /// Use the VisualState RightTransition
        /// </summary>
        Right,
        /// <summary>
        /// Use the VisualState RightReplaceTransition
        /// </summary>
        RightReplace,
        /// <summary>
        /// Use the VisualState LeftTransition
        /// </summary>
        Left,
        /// <summary>
        /// Use the VisualState LeftReplaceTransition
        /// </summary>
        LeftReplace,
        /// <summary>
        /// Use a custom VisualState, the name must be set using CustomVisualStatesName property
        /// </summary>
        Custom
    }

    public class FormsTransitioningContentControl : FormsContentControl, IStyleable
    {
        internal const string PresentationGroup = "PresentationStates";
        internal const string NormalState = "Normal";
        internal const string PreviousContentPresentationSitePartName = "PreviousContentPresentationSite";
        internal const string CurrentContentPresentationSitePartName = "CurrentContentPresentationSite";

        private global::Avalonia.Controls.Presenters.ContentPresenter currentContentPresentationSite;
        private global::Avalonia.Controls.Presenters.ContentPresenter previousContentPresentationSite;
        private bool allowIsTransitioningPropertyWrite;

        public event EventHandler TransitionCompleted;

        public const TransitionType DefaultTransitionState = TransitionType.Default;

        public static readonly AvaloniaProperty IsTransitioningProperty = AvaloniaProperty.Register<FormsTransitioningContentControl, bool>(nameof(IsTransitioning));
        public static readonly StyledProperty<TransitionType> TransitionProperty = AvaloniaProperty.Register<FormsTransitioningContentControl, TransitionType>(nameof(Transition));
        public static readonly AvaloniaProperty RestartTransitionOnContentChangeProperty = AvaloniaProperty.Register<FormsTransitioningContentControl, bool>(nameof(RestartTransitionOnContentChange));
        public static readonly AvaloniaProperty CustomVisualStatesProperty = AvaloniaProperty.Register<FormsTransitioningContentControl, ObservableCollection<VisualState>>(nameof(CustomVisualStates));
        public static readonly AvaloniaProperty CustomVisualStatesNameProperty = AvaloniaProperty.Register<FormsTransitioningContentControl, string>(nameof(CustomVisualStatesName), defaultValue: "CustomTransition");

        public ObservableCollection<VisualState> CustomVisualStates
        {
            get { return (ObservableCollection<VisualState>)this.GetValue(CustomVisualStatesProperty); }
            set { this.SetValue(CustomVisualStatesProperty, value); }
        }

        /// <summary>
        /// Gets or sets the name of the custom transition visual state.
        /// </summary>
        public string CustomVisualStatesName
        {
            get { return (string)this.GetValue(CustomVisualStatesNameProperty); }
            set { this.SetValue(CustomVisualStatesNameProperty, value); }
        }

        /// <summary>
        /// Gets/sets if the content is transitioning.
        /// </summary>
        public bool IsTransitioning
        {
            get { return (bool)this.GetValue(IsTransitioningProperty); }
            private set
            {
                this.allowIsTransitioningPropertyWrite = true;
                this.SetValue(IsTransitioningProperty, value);
                this.allowIsTransitioningPropertyWrite = false;
            }
        }

        public TransitionType Transition
        {
            get { return (TransitionType)this.GetValue(TransitionProperty); }
            set { this.SetValue(TransitionProperty, value); }
        }

        Type IStyleable.StyleKey => typeof(FormsTransitioningContentControl);

        public bool RestartTransitionOnContentChange
        {
            get { return (bool)this.GetValue(RestartTransitionOnContentChangeProperty); }
            set { this.SetValue(RestartTransitionOnContentChangeProperty, value); }
        }

        private void OnIsTransitioningPropertyChanged(FormsTransitioningContentControl o, AvaloniaPropertyChangedEventArgs e)
        {
            if (!o.allowIsTransitioningPropertyWrite)
            {
                o.IsTransitioning = (bool)e.OldValue;
                throw new InvalidOperationException();
            }
        }

        private static void OnTransitionPropertyChanged(object d, AvaloniaPropertyChangedEventArgs e)
        {
            var source = (FormsTransitioningContentControl)d;
            var oldTransition = (TransitionType)e.OldValue;
            var newTransition = (TransitionType)e.NewValue;

            if (source.IsTransitioning)
            {
                source.AbortTransition();
            }
        }

        private static void OnRestartTransitionOnContentChangePropertyChanged(object d, AvaloniaPropertyChangedEventArgs e)
        {
        }

        protected virtual void OnRestartTransitionOnContentChangeChanged(object d, AvaloniaPropertyChangedEventArgs e)
        {
            ((FormsTransitioningContentControl)d).OnContentChanged(e.OldValue, e.NewValue);
        }

        public FormsTransitioningContentControl()
        {
            this.CustomVisualStates = new ObservableCollection<VisualState>();

            ContentProperty.Changed.AddClassHandler<FormsTransitioningContentControl>((o, e) => OnRestartTransitionOnContentChangeChanged(o, e));
            TransitionProperty.Changed.AddClassHandler<FormsTransitioningContentControl>((o, e) => OnTransitionPropertyChanged(o, e));
            IsTransitioningProperty.Changed.AddClassHandler<FormsTransitioningContentControl>((o, e) => OnIsTransitioningPropertyChanged(o, e));
            RestartTransitionOnContentChangeProperty.Changed.AddClassHandler<FormsTransitioningContentControl>((o, e) => OnRestartTransitionOnContentChangePropertyChanged(o, e));
        }

        protected override void OnTemplateApplied(TemplateAppliedEventArgs e)
        {
            if (this.IsTransitioning)
            {
                this.AbortTransition();
            }

            base.OnTemplateApplied(e);

            this.previousContentPresentationSite = this.Find<global::Avalonia.Controls.Presenters.ContentPresenter>(PreviousContentPresentationSitePartName, e);
            this.currentContentPresentationSite = this.Find<global::Avalonia.Controls.Presenters.ContentPresenter>(CurrentContentPresentationSitePartName, e);
        }

        protected void OnContentChanged(object oldContent, object newContent)
        {
            this.StartTransition(oldContent, newContent);
        }

        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "newContent", Justification = "Should be used in the future.")]
        private void StartTransition(object oldContent, object newContent)
        {
            // both presenters must be available, otherwise a transition is useless.
            if (this.currentContentPresentationSite != null)
            {
                this.currentContentPresentationSite.Content = newContent;
            }
            if (previousContentPresentationSite != null)
            {
                this.previousContentPresentationSite.Content = oldContent;
                DispatcherTimer.RunOnce(() => this.previousContentPresentationSite.Content = null, TimeSpan.FromSeconds(0.2));
            }

            // and start a new transition
            if (!this.IsTransitioning || this.RestartTransitionOnContentChange)
            {
                this.IsTransitioning = true;
            }
        }

        /// <summary>
        /// Reload the current transition if the content is the same.
        /// </summary>
        public void ReloadTransition()
        {
            // both presenters must be available, otherwise a transition is useless.
            if (this.currentContentPresentationSite != null && this.previousContentPresentationSite != null)
            {
                if (!this.IsTransitioning || this.RestartTransitionOnContentChange)
                {
                    this.IsTransitioning = true;
                }
            }
        }

        public void AbortTransition()
        {
            // go to normal state and release our hold on the old content.
            this.IsTransitioning = false;
            if (this.previousContentPresentationSite != null)
            {
                this.previousContentPresentationSite.Content = null;
            }
        }

        private string GetTransitionName(TransitionType transition)
        {
            switch (transition)
            {
                default:
                case TransitionType.Default:
                    return "DefaultTransition";
                case TransitionType.Normal:
                    return "Normal";
                case TransitionType.Up:
                    return "UpTransition";
                case TransitionType.Down:
                    return "DownTransition";
                case TransitionType.Right:
                    return "RightTransition";
                case TransitionType.RightReplace:
                    return "RightReplaceTransition";
                case TransitionType.Left:
                    return "LeftTransition";
                case TransitionType.LeftReplace:
                    return "LeftReplaceTransition";
                case TransitionType.Custom:
                    return this.CustomVisualStatesName;
            }
        }
    }
}
