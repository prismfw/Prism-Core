/*
Copyright (C) 2018  Prism Framework Team

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
    /// Represents a collection of <see cref="TabItem"/> objects.
    /// </summary>
    [DebuggerDisplay("Count = {Count}")]
    public sealed class TabItemCollection : IList<TabItem>, IList
    {
        /// <summary>
        /// Gets the number of tab items within the collection.
        /// </summary>
        public int Count
        {
            get { return nativeObject.TabItems.Count; }
        }

        /// <summary>
        /// Gets or sets the tab item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the tab item to get or set.</param>
        public TabItem this[int index]
        {
            get { return (TabItem)ObjectRetriever.GetAgnosticObject(nativeObject.TabItems[index]); }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                nativeObject.TabItems[index] = ObjectRetriever.GetNativeObject(value);
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
            get { return ObjectRetriever.GetAgnosticObject(nativeObject.TabItems[index]); }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                nativeObject.TabItems[index] = ObjectRetriever.GetNativeObject(value);
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        bool ICollection<TabItem>.IsReadOnly
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
        private readonly INativeTabView nativeObject;

        internal TabItemCollection(INativeTabView parent)
        {
            if (parent == null)
            {
                throw new ArgumentNullException(nameof(parent));
            }

            nativeObject = parent;
        }

        /// <summary>
        /// Adds the specified <see cref="TabItem"/> to the collection.
        /// </summary>
        /// <param name="item">The tab item to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="item"/> is <c>null</c>.</exception>
        public void Add(TabItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            nativeObject.TabItems.Add(ObjectRetriever.GetNativeObject(item));
        }

        /// <summary>
        /// Adds a range of tab items to the collection.
        /// </summary>
        /// <param name="items">The tab items to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="items"/> is <c>null</c>.</exception>
        public void AddRange(IEnumerable<TabItem> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            foreach (var item in items)
            {
                nativeObject.TabItems.Add(ObjectRetriever.GetNativeObject(item));
            }
        }

        /// <summary>
        /// Removes all tab items from the collection.
        /// </summary>
        public void Clear()
        {
            nativeObject.TabItems.Clear();
        }

        /// <summary>
        /// Determines whether the collection contains the specified <see cref="TabItem"/>.
        /// </summary>
        /// <param name="item">The tab item to locate in the collection.</param>
        /// <returns><c>true</c> if the item is found in the collection; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="item"/> is <c>null</c>.</exception>
        public bool Contains(TabItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return nativeObject.TabItems.Contains(ObjectRetriever.GetNativeObject(item));
        }

        /// <summary>
        /// Copies the tab items of the collection to the specified array, starting at the specified array index.
        /// </summary>
        /// <param name="array">The array that is the destination of the tab items being copied from the collection.</param>
        /// <param name="arrayIndex">The zero-based index in the array at which copying begins.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="array"/> is <c>null</c>.</exception>
        public void CopyTo(TabItem[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            for (int i = 0; i < nativeObject.TabItems.Count; i++)
            {
                array[arrayIndex + i] = (TabItem)ObjectRetriever.GetAgnosticObject(nativeObject.TabItems[i]);
            }
        }

        /// <summary>
        /// Determines the index of the specified <see cref="TabItem"/> in the collection.
        /// </summary>
        /// <param name="item">The tab item to locate in the collection.</param>
        /// <returns>The index of the item if it is found in the collection; otherwise, -1.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="item"/> is <c>null</c>.</exception>
        public int IndexOf(TabItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return nativeObject.TabItems.IndexOf(ObjectRetriever.GetNativeObject(item));
        }

        /// <summary>
        /// Inserts the specified <see cref="TabItem"/> into the collection at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which the tab item should be inserted.</param>
        /// <param name="item">The tab item to be inserted.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="item"/> is <c>null</c>.</exception>
        public void Insert(int index, TabItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            if (index == nativeObject.TabItems.Count)
            {
                nativeObject.TabItems.Add(ObjectRetriever.GetNativeObject(item));
            }
            else
            {
                nativeObject.TabItems.Insert(index, ObjectRetriever.GetNativeObject(item));
            }
        }

        /// <summary>
        /// Removes the specified <see cref="TabItem"/> from the collection.
        /// </summary>
        /// <param name="item">The tab item to be removed.</param>
        /// <returns><c>true</c> if the item was found and removed from the collection; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="item"/> is <c>null</c>.</exception>
        public bool Remove(TabItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            int count = nativeObject.TabItems.Count;
            nativeObject.TabItems.Remove(ObjectRetriever.GetNativeObject(item));
            return count > nativeObject.TabItems.Count;
        }

        /// <summary>
        /// Removes all of the tab items that match the conditions defined by the specified predicate.
        /// </summary>
        /// <param name="match">The delegate that defines the conditions of the tab items to remove.</param>
        /// <returns>The number of tab items that were removed.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="match"/> is <c>null</c>.</exception>
        public int RemoveAll(Predicate<TabItem> match)
        {
            if (match == null)
            {
                throw new ArgumentNullException(nameof(match));
            }

            int count = nativeObject.TabItems.Count;
            for (int i = 0; i < nativeObject.TabItems.Count;)
            {
                if (match((TabItem)ObjectRetriever.GetAgnosticObject(nativeObject.TabItems[i])))
                {
                    nativeObject.TabItems.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }

            return count - nativeObject.TabItems.Count;
        }

        /// <summary>
        /// Removes the tab item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the tab item to be removed.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="index"/> exceeds the upper or lower bound of the collection.</exception>
        public void RemoveAt(int index)
        {
            if (index < 0 || index >= nativeObject.TabItems.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            nativeObject.TabItems.RemoveAt(index);
        }

        /// <summary>
        /// Removes a range of tab items from the collection.
        /// </summary>
        /// <param name="index">The zero-based starting index of the range of tab items to remove.</param>
        /// <param name="count">The number of tab items to remove.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="index"/> exceeds the upper or lower bound of the collection -or- when <paramref name="count"/> exceeds the upper bound of the collection.</exception>
        public void RemoveRange(int index, int count)
        {
            if (index < 0 || index >= nativeObject.TabItems.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            if (count < 0 || index + count > nativeObject.TabItems.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            for (int i = index; i < index + count; i++)
            {
                nativeObject.TabItems.RemoveAt(i);
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        public IEnumerator<TabItem> GetEnumerator()
        {
            return new TabItemEnumerator(nativeObject.TabItems.Count > 0 ? nativeObject.TabItems.GetEnumerator() : null);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new TabItemEnumerator(nativeObject.TabItems.Count > 0 ? nativeObject.TabItems.GetEnumerator() : null);
        }

        int IList.Add(object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return nativeObject.TabItems.Add(ObjectRetriever.GetNativeObject(value));
        }

        bool IList.Contains(object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return nativeObject.TabItems.Contains(ObjectRetriever.GetNativeObject(value));
        }

        int IList.IndexOf(object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return nativeObject.TabItems.IndexOf(ObjectRetriever.GetNativeObject(value));
        }

        void IList.Insert(int index, object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (index == nativeObject.TabItems.Count)
            {
                nativeObject.TabItems.Add(ObjectRetriever.GetNativeObject(value));
            }
            else
            {
                nativeObject.TabItems.Insert(index, ObjectRetriever.GetNativeObject(value));
            }
        }

        void IList.Remove(object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            nativeObject.TabItems.Remove(ObjectRetriever.GetNativeObject(value));
        }

        void IList.RemoveAt(int index)
        {
            nativeObject.TabItems.RemoveAt(index);
        }

        void ICollection.CopyTo(Array array, int index)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            for (int i = 0; i < nativeObject.TabItems.Count; i++)
            {
                array.SetValue(ObjectRetriever.GetAgnosticObject(nativeObject.TabItems[i]), index + i);
            }
        }

        private class TabItemEnumerator : IEnumerator<TabItem>, IEnumerator
        {
            public TabItem Current
            {
                get { return (TabItem)ObjectRetriever.GetAgnosticObject(nativeEnumerator?.Current); }
            }

            object IEnumerator.Current
            {
                get { return ObjectRetriever.GetAgnosticObject(nativeEnumerator?.Current); }
            }

#if !DEBUG
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
            private readonly IEnumerator nativeEnumerator;

            public TabItemEnumerator(IEnumerator nativeEnumerator)
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
