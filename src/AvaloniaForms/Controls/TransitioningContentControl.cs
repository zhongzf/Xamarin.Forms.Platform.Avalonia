using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Styling;
using Avalonia.Threading;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace AvaloniaForms.Controls
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

    public class TransitioningContentControl : DynamicContentControl, IStyleable
    {
        internal const string PreviousContentPresentationSitePartName = "PreviousContentPresentationSite";
        internal const string CurrentContentPresentationSitePartName = "CurrentContentPresentationSite";

        public static readonly AvaloniaProperty IsTransitioningProperty = AvaloniaProperty.Register<TransitioningContentControl, bool>(nameof(IsTransitioning));
        public static readonly StyledProperty<TransitionType> TransitionProperty = AvaloniaProperty.Register<TransitioningContentControl, TransitionType>(nameof(Transition));
        public static readonly AvaloniaProperty RestartTransitionOnContentChangeProperty = AvaloniaProperty.Register<TransitioningContentControl, bool>(nameof(RestartTransitionOnContentChange));

        static TransitioningContentControl()
        {
            ContentProperty.Changed.AddClassHandler<TransitioningContentControl>((x, e) => x.OnContentPropertyChanged(e));
            IsTransitioningProperty.Changed.AddClassHandler<TransitioningContentControl>((x, e) => x.OnIsTransitioningPropertyChanged(e));
            TransitionProperty.Changed.AddClassHandler<TransitioningContentControl>((x, e) => x.OnTransitionPropertyChanged(e));
        }

        Type IStyleable.StyleKey => typeof(TransitioningContentControl);

        private bool allowIsTransitioningPropertyWrite;

        private ContentPresenter currentContentPresentationSite;
        private ContentPresenter previousContentPresentationSite;

        public event EventHandler TransitionCompleted;

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

        public bool RestartTransitionOnContentChange
        {
            get { return (bool)this.GetValue(RestartTransitionOnContentChangeProperty); }
            set { this.SetValue(RestartTransitionOnContentChangeProperty, value); }
        }


        protected override void OnTemplateApplied(TemplateAppliedEventArgs e)
        {
            if (this.IsTransitioning)
            {
                this.AbortTransition();
            }

            base.OnTemplateApplied(e);

            this.previousContentPresentationSite = e.NameScope.Find<ContentPresenter>(PreviousContentPresentationSitePartName);
            this.currentContentPresentationSite = e.NameScope.Find<ContentPresenter>(CurrentContentPresentationSitePartName);
        }

        private void OnTransitionPropertyChanged(AvaloniaPropertyChangedEventArgs e)
        {
            if (IsTransitioning)
            {
                AbortTransition();
            }
        }

        private void OnIsTransitioningPropertyChanged(AvaloniaPropertyChangedEventArgs e)
        {
            if (!allowIsTransitioningPropertyWrite)
            {
                IsTransitioning = (bool)e.OldValue;
                throw new InvalidOperationException();
            }
        }

        private void OnContentPropertyChanged(AvaloniaPropertyChangedEventArgs e)
        {
            StartTransition(e.OldValue, e.NewValue);
        }

        #region Transition
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
                ResetTransition();
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
            }
        }

        protected virtual void ResetTransition()
        {
            DispatcherTimer.RunOnce(() =>
            {
                this.previousContentPresentationSite.Content = null;
                TransitionCompleted?.Invoke(this, EventArgs.Empty);
            }, TimeSpan.FromSeconds(0.2));
        }

        #endregion
    }
}
