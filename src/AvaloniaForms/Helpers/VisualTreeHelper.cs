using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.VisualTree;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AvaloniaForms.Helpers
{
    /// <summary>
    /// Helper methods for UI-related tasks.
    /// This class was obtained from Philip Sumi (a fellow WPF Disciples blog)
    /// http://www.hardcodet.net/uploads/2009/06/UIHelper.cs
    /// </summary>
    public static class VisualTreeHelper
    {
        public static IAvaloniaReadOnlyList<IVisual> GetVisualChildren(IVisual parent)
        {
            return parent.VisualChildren;
        }

        /// <summary>
        /// Finds a parent of a given item on the visual tree.
        /// </summary>
        /// <typeparam name="T">The type of the queried item.</typeparam>
        /// <param name="child">A direct or indirect child of the
        /// queried item.</param>
        /// <returns>The first parent item that matches the submitted
        /// type parameter. If not matching item can be found, a null
        /// reference is being returned.</returns>
        public static T TryFindParent<T>(this IVisual child)
            where T : class, IVisual
        {
            //get parent item
            var parentObject = GetParentObject(child);

            //we've reached the end of the tree
            if (parentObject == null) return null;

            //check if the parent matches the type we're looking for
            T parent = parentObject as T;
            return parent ?? TryFindParent<T>(parentObject);
        }

        /// <summary>
        /// Finds a Child of a given item in the visual tree. 
        /// </summary>
        /// <param name="parent">A direct parent of the queried item.</param>
        /// <typeparam name="T">The type of the queried item.</typeparam>
        /// <param name="childName">x:Name or Name of child. </param>
        /// <returns>The first parent item that matches the submitted type parameter. 
        /// If not matching item can be found, 
        /// a null parent is being returned.</returns>
        public static T FindChild<T>(this IVisual parent, string childName)
           where T : class, IVisual
        {
            // Confirm parent and childName are valid. 
            if (parent == null) return default(T);

            T foundChild = default(T);

            var children = GetVisualChildren(parent);
            for (int i = 0; i < children.Count; i++)
            {
                var child = children[i];
                // If the child is not of the request child type child
                T childType = child as T;
                if (childType == null)
                {
                    // recursively drill down the tree
                    foundChild = FindChild<T>(child, childName);

                    // If the child is found, break so we do not overwrite the found child. 
                    if (foundChild != null) break;
                }
                else if (!string.IsNullOrEmpty(childName))
                {
                    var control = child as Control;
                    // If the child's name is set for search
                    if (control != null && control.Name == childName)
                    {
                        // if the child's name is of the request name
                        foundChild = (T)child;
                        break;
                    }
                }
                else
                {
                    // child element found.
                    foundChild = (T)child;
                    break;
                }
            }

            return foundChild;
        }

        public static object GetChild(AvaloniaObject element, int i)
        {
            return GetChildObjects(element as IVisual)?.Skip(i).Take(1);
        }

        public static int GetChildrenCount(AvaloniaObject element)
        {
            return GetVisualChildren(element as IVisual)?.Count ?? 0;
        }

        /// <summary>
        /// This method is an alternative to WPF's
        /// <see cref="VisualTreeHelper.GetParent"/> method, which also
        /// supports content elements. Keep in mind that for content element,
        /// this method falls back to the logical tree of the element!
        /// </summary>
        /// <param name="child">The item to be processed.</param>
        /// <returns>The submitted item's parent, if available. Otherwise
        /// null.</returns>
        public static IVisual GetParentObject(this IVisual child)
        {
            if (child == null) return null;

            //handle content elements separately
            var contentElement = child as ContentControl;
            if (contentElement != null)
            {
                var parent = contentElement.Parent;
                if (parent != null) return parent;

                var fce = contentElement as Control;
                return fce != null ? fce.Parent : null;
            }

            //also try searching for parent in framework elements (such as DockPanel, etc)
            var control = child as Control;
            if (control != null)
            {
                var parent = control.Parent;
                if (parent != null) return parent;
            }

            //if it's not a ContentElement/FrameworkElement, rely on VisualTreeHelper
            return child.VisualParent;
        }

        /// <summary>
        /// Analyzes both visual and logical tree in order to find all elements of a given
        /// type that are descendants of the <paramref name="source"/> item.
        /// </summary>
        /// <typeparam name="T">The type of the queried items.</typeparam>
        /// <param name="source">The root element that marks the source of the search. If the
        /// source is already of the requested type, it will not be included in the result.</param>
        /// <param name="forceUsingTheVisualTreeHelper">Sometimes it's better to search in the VisualTree (e.g. in tests)</param>
        /// <returns>All descendants of <paramref name="source"/> that match the requested type.</returns>
        public static IEnumerable<T> FindChildren<T>(this IVisual source, bool forceUsingTheVisualTreeHelper = false) where T : IVisual
        {
            if (source != null)
            {
                var childs = GetChildObjects(source, forceUsingTheVisualTreeHelper);
                foreach (var child in childs)
                {
                    //analyze if children match the requested type
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    //recurse tree
                    foreach (T descendant in FindChildren<T>(child, forceUsingTheVisualTreeHelper))
                    {
                        yield return descendant;
                    }
                }
            }
        }

        /// <summary>
        /// This method is an alternative to WPF's
        /// <see cref="VisualTreeHelper.GetChild"/> method, which also
        /// supports content elements. Keep in mind that for content elements,
        /// this method falls back to the logical tree of the element.
        /// </summary>
        /// <param name="parent">The item to be processed.</param>
        /// <param name="forceUsingTheVisualTreeHelper">Sometimes it's better to search in the VisualTree (e.g. in tests)</param>
        /// <returns>The submitted item's child elements, if available.</returns>
        public static IEnumerable<IVisual> GetChildObjects(this IVisual parent, bool forceUsingTheVisualTreeHelper = false)
        {
            if (parent == null) yield break;

            if (!forceUsingTheVisualTreeHelper && (parent is ContentControl || parent is Control))
            {
                //use the logical tree for content / framework elements
                foreach (object obj in GetVisualChildren(parent))
                {
                    var depObj = obj as Visual;
                    if (depObj != null) yield return (Visual)obj;
                }
            }
            else
            {
                //use the visual tree per default
                var visualChildren = GetVisualChildren(parent);
                for (int i = 0; i < visualChildren.Count; i++)
                {
                    yield return visualChildren[i];
                }
            }
        }

        /// <summary>
        /// Tries to locate a given item within the visual tree,
        /// starting with the dependency object at a given position. 
        /// </summary>
        /// <typeparam name="T">The type of the element to be found
        /// on the visual tree of the element at the given location.</typeparam>
        /// <param name="reference">The main element which is used to perform
        /// hit testing.</param>
        /// <param name="point">The position to be evaluated on the origin.</param>
        public static T TryFindFromPoint<T>(InputElement reference, Point point) where T : class, IVisual
        {
            var element = reference.InputHitTest(point);
            if (element == null)
            {
                return null;
            }
            if (element is T)
            {
                return (T)element;
            }
            return TryFindParent<T>(element);
        }

        public static IEnumerable<T> FindVisualChildren<T>(this IVisual parent) where T : IVisual
        {
            var visualChildren = GetVisualChildren(parent);
            for (int i = 0; i < visualChildren.Count; i++)
            {
                var child = visualChildren[i];
                if (child is T childType)
                {
                    yield return (T)child;
                }

                foreach (var other in FindVisualChildren<T>(child))
                {
                    yield return other;
                }
            }
        }

        public static T FindVisualChild<T>(this IVisual parent) where T : class, IVisual
        {
            var child = default(T);

            var visualChildren = GetVisualChildren(parent);
            for (int i = 0; i < visualChildren.Count; i++)
            {
                var v = visualChildren[i];
                child = v as T ?? FindVisualChild<T>(v);
                if (child != null)
                {
                    break;
                }
            }
            return child;
        }
    }
}
