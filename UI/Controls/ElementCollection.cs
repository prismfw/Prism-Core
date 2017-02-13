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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Prism.Native;

namespace Prism.UI.Controls
{
    /// <summary>
    /// Represents a collection of UI elements.
    /// </summary>
    [DebuggerDisplay("Count = {Count}")]
    public sealed class ElementCollection : IList<Element>, IList
    {
        /// <summary>
        /// Gets the number of UI element within the collection.
        /// </summary>
        public int Count
        {
            get { return nativeObject.Children.Count; }
        }

        /// <summary>
        /// Gets or sets the UI element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the UI element to get or set.</param>
        public Element this[int index]
        {
            get { return (Element)ObjectRetriever.GetAgnosticObject(nativeObject.Children[index]); }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                nativeObject.Children[index] = ObjectRetriever.GetNativeObject(value);
                panel.InvalidateMeasure();
                panel.InvalidateArrange();
            }
        }
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        bool IList.IsFixedSize
        {
            get { return false; }
        }
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        bool IList.IsReadOnly
        {
            get { return false; }
        }
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        object IList.this[int index]
        {
            get { return ObjectRetriever.GetAgnosticObject(nativeObject.Children[index]); }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                nativeObject.Children[index] = ObjectRetriever.GetNativeObject(value);
                panel.InvalidateMeasure();
                panel.InvalidateArrange();
            }
        }
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        bool ICollection<Element>.IsReadOnly
        {
            get { return false; }
        }
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        bool ICollection.IsSynchronized
        {
            get { return false; }
        }
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        object ICollection.SyncRoot
        {
            get { return null; }
        }

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private readonly Panel panel;
#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private readonly INativePanel nativeObject;

        internal ElementCollection(Panel parent)
        {
            if (parent == null)
            {
                throw new ArgumentNullException(nameof(parent));
            }

            panel = parent;
            nativeObject = (INativePanel)ObjectRetriever.GetNativeObject(parent);
        }

        /// <summary>
        /// Adds the specified UI element to the collection.
        /// </summary>
        /// <param name="item">The UI element to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="item"/> is <c>null</c>.</exception>
        public void Add(Element item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            nativeObject.Children.Add(ObjectRetriever.GetNativeObject(item));
            panel.InvalidateMeasure();
            panel.InvalidateArrange();
        }

        /// <summary>
        /// Adds a range of UI elements to the collection.
        /// </summary>
        /// <param name="items">The UI elements to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="items"/> is <c>null</c>.</exception>
        public void AddRange(IEnumerable<Element> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            foreach (var item in items)
            {
                nativeObject.Children.Add(ObjectRetriever.GetNativeObject(item));
            }

            panel.InvalidateMeasure();
            panel.InvalidateArrange();
        }

        /// <summary>
        /// Removes all UI elements from the collection.
        /// </summary>
        public void Clear()
        {
            nativeObject.Children.Clear();
            panel.InvalidateMeasure();
        }

        /// <summary>
        /// Determines whether the collection contains the specified UI element.
        /// </summary>
        /// <param name="item">The UI element to locate in the collection.</param>
        /// <returns><c>true</c> if the element is found in the collection; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="item"/> is <c>null</c>.</exception>
        public bool Contains(Element item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return nativeObject.Children.Contains(ObjectRetriever.GetNativeObject(item));
        }

        /// <summary>
        /// Copies the UI elements of the collection to the specified array, starting at the specified array index.
        /// </summary>
        /// <param name="array">The array that is the destination of the UI elements being copied from the collection.</param>
        /// <param name="arrayIndex">The zero-based index in the array at which copying begins.</param>
        /// <exception cref="ArgumentNullException">Throw when <paramref name="array"/> is <c>null</c>.</exception>
        public void CopyTo(Element[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            for (int i = 0; i < nativeObject.Children.Count; i++)
            {
                array[arrayIndex + i] = (Element)ObjectRetriever.GetAgnosticObject(nativeObject.Children[i]);
            }
        }

        /// <summary>
        /// Determines the index of the specified UI element in the collection.
        /// </summary>
        /// <param name="item">The UI element to locate in the collection.</param>
        /// <returns>The index of the element if it is found in the collection; otherwise, -1.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="item"/> is <c>null</c>.</exception>
        public int IndexOf(Element item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return nativeObject.Children.IndexOf(ObjectRetriever.GetNativeObject(item));
        }

        /// <summary>
        /// Inserts the specified UI element into the collection at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which the UI element should be inserted.</param>
        /// <param name="item">The UI element to be inserted.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="item"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="index"/> exceeds the upper or lower bound of the collection.</exception>
        public void Insert(int index, Element item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            if (index > nativeObject.Children.Count || index < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            if (index == nativeObject.Children.Count)
            {
                nativeObject.Children.Add(ObjectRetriever.GetNativeObject(item));
            }
            else
            {
                nativeObject.Children.Insert(index, ObjectRetriever.GetNativeObject(item));
            }

            panel.InvalidateMeasure();
            panel.InvalidateArrange();
        }

        /// <summary>
        /// Moves the UI element at the specified index to a new position within the collection.
        /// </summary>
        /// <param name="oldIndex">The zero-based index of the UI element to be moved.</param>
        /// <param name="newIndex">The zero-based index to where the UI element should be moved.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when either <paramref name="oldIndex"/> or <paramref name="newIndex"/> exceed the upper or lower bound of the collection.</exception>
        public void Move(int oldIndex, int newIndex)
        {
            if (oldIndex < 0 || oldIndex >= nativeObject.Children.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(oldIndex));
            }

            if (newIndex < 0 || newIndex >= nativeObject.Children.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(newIndex));
            }

            if (oldIndex != newIndex)
            {
                var child = nativeObject.Children[oldIndex];
                nativeObject.Children.RemoveAt(oldIndex);
                nativeObject.Children.Insert(newIndex, child);

                panel.InvalidateMeasure();
                panel.InvalidateArrange();
            }
        }

        /// <summary>
        /// Removes the specified UI element from the collection.
        /// </summary>
        /// <param name="item">The UI element to be removed.</param>
        /// <returns><c>true</c> if the element was found and removed from the collection; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="item"/> is <c>null</c>.</exception>
        public bool Remove(Element item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            int count = nativeObject.Children.Count;
            nativeObject.Children.Remove(ObjectRetriever.GetNativeObject(item));

            if (count > nativeObject.Children.Count)
            {
                panel.InvalidateMeasure();
                panel.InvalidateArrange();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Removes the UI element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the UI element to be removed.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="index"/> exceeds the upper or lower bound of the collection.</exception>
        public void RemoveAt(int index)
        {
            if (index < 0 || index >= nativeObject.Children.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            nativeObject.Children.RemoveAt(index);
            panel.InvalidateMeasure();
            panel.InvalidateArrange();
        }

        /// <summary>
        /// Removes a range of UI elements from the collection.
        /// </summary>
        /// <param name="index">The zero-based starting index of the range of UI elements to remove.</param>
        /// <param name="count">The number of UI elements to remove.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="index"/> exceeds the upper or lower bound of the collection -or- when <paramref name="count"/> exceeds the upper bound of the collection.</exception>
        public void RemoveRange(int index, int count)
        {
            if (index < 0 || index >= nativeObject.Children.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            if (count < 0 || index + count > nativeObject.Children.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            for (int i = index; i < index + count; i++)
            {
                nativeObject.Children.RemoveAt(i);
            }

            panel.InvalidateMeasure();
            panel.InvalidateArrange();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        public IEnumerator<Element> GetEnumerator()
        {
            return new ElementEnumerator(nativeObject.Children.Count > 0 ? nativeObject.Children.GetEnumerator() : null);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new ElementEnumerator(nativeObject.Children.Count > 0 ? nativeObject.Children.GetEnumerator() : null);
        }

        int IList.Add(object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            int count = nativeObject.Children.Add(ObjectRetriever.GetNativeObject(value));
            if (count > 0)
            {
                panel.InvalidateMeasure();
                panel.InvalidateArrange();
            }

            return count;
        }

        bool IList.Contains(object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return nativeObject.Children.Contains(ObjectRetriever.GetNativeObject(value));
        }

        int IList.IndexOf(object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return nativeObject.Children.IndexOf(ObjectRetriever.GetNativeObject(value));
        }

        void IList.Insert(int index, object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (index > nativeObject.Children.Count || index < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            if (index == nativeObject.Children.Count)
            {
                nativeObject.Children.Add(ObjectRetriever.GetNativeObject(value));
            }
            else
            {
                nativeObject.Children.Insert(index, ObjectRetriever.GetNativeObject(value));
            }

            panel.InvalidateMeasure();
            panel.InvalidateArrange();
        }

        void IList.Remove(object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            nativeObject.Children.Remove(ObjectRetriever.GetNativeObject(value));
            panel.InvalidateMeasure();
            panel.InvalidateArrange();
        }

        void IList.RemoveAt(int index)
        {
            if (index < 0 || index >= nativeObject.Children.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            nativeObject.Children.RemoveAt(index);
            panel.InvalidateMeasure();
            panel.InvalidateArrange();
        }

        void ICollection.CopyTo(Array array, int index)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            for (int i = 0; i < nativeObject.Children.Count; i++)
            {
                array.SetValue(ObjectRetriever.GetAgnosticObject(nativeObject.Children[i]), index + i);
            }
        }

        private class ElementEnumerator : IEnumerator<Element>, IEnumerator
        {
            public Element Current
            {
                get { return (Element)ObjectRetriever.GetAgnosticObject(nativeEnumerator?.Current); }
            }

            object IEnumerator.Current
            {
                get { return ObjectRetriever.GetAgnosticObject(nativeEnumerator?.Current); }
            }

            #if !DEBUG
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
            private readonly IEnumerator nativeEnumerator;

            public ElementEnumerator(IEnumerator nativeEnumerator)
            {
                this.nativeEnumerator = nativeEnumerator;
            }

            public void Dispose()
            {
                (nativeEnumerator as IDisposable)?.Dispose();
            }

            public bool MoveNext()
            {
                return nativeEnumerator != null && nativeEnumerator.MoveNext();
            }

            public void Reset()
            {
                nativeEnumerator?.Reset();
            }
        }
    }
}
