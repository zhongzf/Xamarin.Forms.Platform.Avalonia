using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvaloniaForms.Controls
{
    public interface INavigate
    {
        bool Navigate(Type sourcePageType);
    }

    internal interface IFrame
    {
        void GoBack();
        void GoForward();
        bool Navigate(Type sourcePageType, object parameter);
        string GetNavigationState();
        void SetNavigationState(string navigationState);

        int BackStackDepth { get; }
        int CacheSize { get; set; }
        bool CanGoBack { get; }
        bool CanGoForward { get; }
        Type CurrentSourcePageType { get; }
        Type SourcePageType { get; set; }

        event EventHandler<object> Navigated;
        event EventHandler<object> Navigating;
        event EventHandler<object> NavigationFailed;
        event EventHandler<object> NavigationStopped;
    }

    public partial class Frame : ContentControl, IFrame, INavigate
    {
        public int BackStackDepth => throw new NotImplementedException();

        public int CacheSize { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool CanGoBack => throw new NotImplementedException();

        public bool CanGoForward => throw new NotImplementedException();

        public Type CurrentSourcePageType => throw new NotImplementedException();

        public Type SourcePageType { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public event EventHandler<object> Navigated;
        public event EventHandler<object> Navigating;
        public event EventHandler<object> NavigationFailed;
        public event EventHandler<object> NavigationStopped;

        public string GetNavigationState()
        {
            throw new NotImplementedException();
        }

        public void GoBack()
        {
            throw new NotImplementedException();
        }

        public void GoForward()
        {
            throw new NotImplementedException();
        }

        public bool Navigate(Type sourcePageType, object parameter)
        {
            throw new NotImplementedException();
        }

        public bool Navigate(Type sourcePageType)
        {
            throw new NotImplementedException();
        }

        public void SetNavigationState(string navigationState)
        {
            throw new NotImplementedException();
        }
    }
}
