/*
Copyright (C) 2016  Prism Framework Team

This file is part of the Prism Framework.

The Prism Framework is free software; you can redistribute it and/or
modify it under the terms of the GNU General Public License
as published by the Free Software Foundation; either version 2
of the License, or (at your option) any later version.

The Prism Framework is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software
Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
*/


using System;
using System.Collections.Generic;
using Prism.Native;

#if !DEBUG
using System.Diagnostics;
#endif

namespace Prism.UI
{
    /// <summary>
    /// Represents a utility that searches an application's visual hierarchy for children and parents that meet specified conditions.
    /// </summary>
    public static class VisualTreeHelper
    {
#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private static INativeVisualTreeHelper Current
        {
            get { return Application.Resolve<INativeVisualTreeHelper>(); }
        }

        /// <summary>
        /// Returns the number of children in the specified object's child collection.
        /// </summary>
        /// <param name="reference">The parent object.</param>
        /// <returns>The number of children in the parent object's child collection.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="reference"/> is <c>null</c>.</exception>
        public static int GetChildrenCount(object reference)
        {
            if (reference == null)
            {
                throw new ArgumentNullException(nameof(reference));
            }

            return Current.GetChildrenCount(ObjectRetriever.GetNativeObject(reference));
        }

        /// <summary>
        /// Returns the child that is located at the specified index in the child collection of the specified object.
        /// </summary>
        /// <param name="reference">The parent object.</param>
        /// <param name="childIndex">The zero-based index of the child to return.</param>
        /// <returns>The child at the specified index, or <c>null</c> if no such child exists.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="reference"/> is <c>null</c>.</exception>
        public static object GetChild(object reference, int childIndex)
        {
            if (reference == null)
            {
                throw new ArgumentNullException(nameof(reference));
            }
            
            return ObjectRetriever.GetAgnosticObject(Current.GetChild(ObjectRetriever.GetNativeObject(reference), childIndex));
        }

        /// <summary>
        /// Searches the visual hierarchy for the child that satisfies the specified condition.
        /// </summary>
        /// <param name="reference">The parent object.</param>
        /// <param name="predicate">A function to test each child for a condition.</param>
        /// <returns>The first child that satisfies the specified condition, or <c>null</c> if no such child is found.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="reference"/> is <c>null</c>.</exception>
        public static object GetChild(object reference, Func<object, bool> predicate)
        {
            if (reference == null)
            {
                throw new ArgumentNullException(nameof(reference));
            }
            
            var parents = new List<object>() { reference };
            while (parents.Count > 0)
            {
                var parent = ObjectRetriever.GetNativeObject(parents[0]);
                parents.RemoveAt(0);
                
                int count = Current.GetChildrenCount(parent);
                for (int i = 0; i < count; i++)
                {
                    var child = ObjectRetriever.GetAgnosticObject(Current.GetChild(parent, i));
                    if (predicate == null || predicate(child))
                    {
                        return child;
                    }
                    
                    if (Current.GetChildrenCount(child) > 0)
                    {
                        parents.Add(child);
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Returns the first child that is of type <typeparamref name="T"/>.
        /// </summary>
        /// <param name="reference">The parent object.</param>
        /// <returns>The first child that is of type <typeparamref name="T"/>, or <c>null</c> if no such child is found.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="reference"/> is <c>null</c>.</exception>
        public static T GetChild<T>(object reference)
            where T : class
        {
            return GetChild<T>(reference, null);
        }

        /// <summary>
        /// Searches the visual hierarchy for the child that is of type <typeparamref name="T"/> and satisfies the specified condition.
        /// </summary>
        /// <param name="reference">The parent object.</param>
        /// <param name="predicate">A function to test each child for a condition.</param>
        /// <returns>The first child that is of type <typeparamref name="T"/> and satisfies the specified condition, or <c>null</c> if no such child is found.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="reference"/> is <c>null</c>.</exception>
        public static T GetChild<T>(object reference, Func<T, bool> predicate)
            where T : class
        {
            if (reference == null)
            {
                throw new ArgumentNullException(nameof(reference));
            }

            var parents = new List<object>() { ObjectRetriever.GetNativeObject(reference) };
            while (parents.Count > 0)
            {
                var parent = parents[0];
                parents.RemoveAt(0);
                
                int count = Current.GetChildrenCount(parent);
                for (int i = 0; i < count; i++)
                {
                    var child = Current.GetChild(parent, i);
                    var tChild = ObjectRetriever.GetAgnosticObject(child) as T ?? child as T;
                    if (tChild != null && (predicate == null || predicate(tChild)))
                    {
                        return tChild;
                    }
                    
                    if (Current.GetChildrenCount(child) > 0)
                    {
                        parents.Add(child);
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Returns the parent of the specified object.
        /// </summary>
        /// <param name="reference">The child object.</param>
        /// <returns>The parent object of the child, or <c>null</c> if no parent is found.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="reference"/> is <c>null</c>.</exception>
        public static object GetParent(object reference)
        {
            if (reference == null)
            {
                throw new ArgumentNullException(nameof(reference));
            }
            
            return ObjectRetriever.GetAgnosticObject(Current.GetParent(ObjectRetriever.GetNativeObject(reference)));
        }

        /// <summary>
        /// Climbs the visual hierarchy and searches for the parent that satisfies the specified condition.
        /// </summary>
        /// <param name="reference">The child object.</param>
        /// <param name="predicate">A function to test each parent for a condition.</param>
        /// <returns>The first parent that satisfies the specified condition, or <c>null</c> if no such parent is found.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="reference"/> is <c>null</c>.</exception>
        public static object GetParent(object reference, Func<object, bool> predicate)
        {
            if (reference == null)
            {
                throw new ArgumentNullException(nameof(reference));
            }
            
            var parent = Current.GetParent(ObjectRetriever.GetNativeObject(reference));
            if (parent != null)
            {
                parent = ObjectRetriever.GetAgnosticObject(parent);
                if (predicate == null || predicate.Invoke(parent))
                {
                    return parent;
                }

                return GetParent(parent, predicate);
            }

            return null;
        }

        /// <summary>
        /// Climbs the visual hierarchy and searches for the parent that is of type <typeparamref name="T"/>.
        /// </summary>
        /// <param name="reference">The child object.</param>
        /// <returns>The first parent that is of type <typeparamref name="T"/>, or <c>null</c> if no such parent is found.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="reference"/> is <c>null</c>.</exception>
        public static T GetParent<T>(object reference)
            where T : class
        {
            return GetParent<T>(reference, null);
        }

        /// <summary>
        /// Climbs the visual hierarchy and searches for the parent that is of type <typeparamref name="T"/> and satisfies the specified condition.
        /// </summary>
        /// <param name="reference">The child object.</param>
        /// <param name="predicate">A function to test each parent for a condition.</param>
        /// <returns>The first parent that is of type <typeparamref name="T"/> and satisfies the specified condition, or <c>null</c> if no such parent is found.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="reference"/> is <c>null</c>.</exception>
        public static T GetParent<T>(object reference, Func<T, bool> predicate)
            where T : class
        {
            if (reference == null)
            {
                throw new ArgumentNullException(nameof(reference));
            }
            
            var parent = Current.GetParent(ObjectRetriever.GetNativeObject(reference));
            if (parent != null)
            {
                var tParent = ObjectRetriever.GetAgnosticObject(parent) as T ?? parent as T;
                if (tParent != null && (predicate == null || predicate(tParent)))
                {
                    return tParent;
                }

                return GetParent(parent, predicate);
            }

            return null;
        }
    }
}
