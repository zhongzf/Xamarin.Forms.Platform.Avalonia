﻿using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using AvaloniaForms.Controls;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvaloniaForms
{
    public interface INavigation
    {
        int StackDepth { get; }

        void InsertPageBefore(object page, object before);

        void Pop();

        void Pop(bool animated);

        void PopModal();

        void PopModal(bool animated);

        void PopToRoot();

        void PopToRoot(bool animated);

        void Push(object page);

        void Push(object page, bool animated);

        void PushModal(object page);

        void PushModal(object page, bool animated);

        void RemovePage(object page);
    }

    public class DefaultNavigation : INavigation
    {
        public static ApplicationWindow MainWindow
        {
            get
            {
                return (Application.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow as ApplicationWindow;
            }
        }

        public void InsertPageBefore(object page, object before)
        {
            throw new InvalidOperationException("InsertPageBefore is not supported globally on Windows, please use a NavigationPage.");
        }

        public void Pop()
        {
            Pop(true);
        }

        public void Pop(bool animated)
        {
            throw new InvalidOperationException("Pop is not supported globally on Windows, please use a LightNavigationPage.");
        }

        public void PopModal()
        {
            PopModal(true);
        }

        public void PopModal(bool animated)
        {
            MainWindow?.PopModal(animated);
        }

        public void PopToRoot()
        {
            PopToRoot(true);
        }

        public void PopToRoot(bool animated)
        {
            throw new InvalidOperationException("PopToRoot is not supported globally on Windows, please use a NavigationPage.");
        }

        public void Push(object page)
        {
            Push(page, true);
        }

        public void Push(object page, bool animated)
        {
            throw new InvalidOperationException("Push is not supported globally on Windows, please use a NavigationPage.");
        }

        public void PushModal(object page)
        {
            PushModal(page, true);
        }

        public void PushModal(object page, bool animated)
        {
            MainWindow?.PushModal(page, animated);
        }

        public void RemovePage(object page)
        {
            throw new InvalidOperationException("RemovePage is not supported globally on Windows, please use a NavigationPage.");
        }

        public int StackDepth => throw new InvalidOperationException("StackDepth is not supported globally on Windows, please use a NavigationPage.");
    }
}
