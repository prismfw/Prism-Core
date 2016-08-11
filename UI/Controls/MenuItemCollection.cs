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
    /// Represents a collection of <see cref="MenuItem"/> objects.
    /// </summary>
    [DebuggerDisplay("Count = {Count}")]
    public sealed class MenuItemCollection : IList<MenuItem>, IList
    {
        /// <summary>
        /// Gets the number of menu items within the collection.
        /// </summary>
        public int Count
        {
            get { return nativeObject.Items.Count; }
        }

        /// <summary>
        /// Gets or sets the menu item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the menu item to get or set.</param>
        public MenuItem this[int index]
        {
            get { return (MenuItem)ObjectRetriever.GetAgnosticObject(nativeObject.Items[index]); }
            set { nativeObject.Items[index] = ObjectRetriever.GetNativeObject(value); }
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
            get { return ObjectRetriever.GetAgnosticObject(nativeObject.Items[index]); }
            set { nativeObject.Items[index] = ObjectRetriever.GetNativeObject(value); }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        bool ICollection<MenuItem>.IsReadOnly
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
        private readonly INativeMenu nativeObject;

        internal MenuItemCollection(INativeMenu parent)
        {
            if (parent == null)
            {
                throw new ArgumentNullException(nameof(parent));
            }

            nativeObject = parent;
        }

        /// <summary>
        /// Adds the specified <see cref="MenuItem"/> to the collection.
        /// </summary>
        /// <param name="item">The menu item to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="item"/> is <c>null</c>.</exception>
        public void Add(MenuItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            nativeObject.Items.Add(ObjectRetriever.GetNativeObject(item));
        }

        /// <summary>
        /// Adds a range of menu items to the collection.
        /// </summary>
        /// <param name="items">The menu items to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="items"/> is <c>null</c>.</exception>
        public void AddRange(IEnumerable<MenuItem> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            foreach (var item in items)
            {
                nativeObject.Items.Add(ObjectRetriever.GetNativeObject(item));
            }
        }

        /// <summary>
        /// Removes all menu items from the collection.
        /// </summary>
        public void Clear()
        {
            nativeObject.Items.Clear();
        }

        /// <summary>
        /// Determines whether the collection contains the specified <see cref="MenuItem"/>.
        /// </summary>
        /// <param name="item">The menu item to locate in the collection.</param>
        /// <returns><c>true</c> if the item is found in the collection; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="item"/> is <c>null</c>.</exception>
        public bool Contains(MenuItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return nativeObject.Items.Contains(ObjectRetriever.GetNativeObject(item));
        }

        /// <summary>
        /// Copies the menu items of the collection to the specified array, starting at the specified array index.
        /// </summary>
        /// <param name="array">The array that is the destination of the menu items being copied from the collection.</param>
        /// <param name="arrayIndex">The zero-based index in the array at which copying begins.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="array"/> is <c>null</c>.</exception>
        public void CopyTo(MenuItem[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            for (int i = 0; i < nativeObject.Items.Count; i++)
            {
                array[arrayIndex + i] = (MenuItem)ObjectRetriever.GetAgnosticObject(nativeObject.Items[i]);
            }
        }

        /// <summary>
        /// Determines the index of the specified <see cref="MenuItem"/> in the collection.
        /// </summary>
        /// <param name="item">The menu item to locate in the collection.</param>
        /// <returns>The index of the item if it is found in the collection; otherwise, -1.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="item"/> is <c>null</c>.</exception>
        public int IndexOf(MenuItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return nativeObject.Items.IndexOf(ObjectRetriever.GetNativeObject(item));
        }

        /// <summary>
        /// Inserts the specified <see cref="MenuItem"/> into the collection at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which the menu item should be inserted.</param>
        /// <param name="item">The menu item to be inserted.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="item"/> is <c>null</c>.</exception>
        public void Insert(int index, MenuItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            if (index == nativeObject.Items.Count)
            {
                nativeObject.Items.Add(ObjectRetriever.GetNativeObject(item));
            }
            else
            {
                nativeObject.Items.Insert(index, ObjectRetriever.GetNativeObject(item));
            }
        }

        /// <summary>
        /// Removes the specified <see cref="MenuItem"/> from the collection.
        /// </summary>
        /// <param name="item">The menu item to be removed.</param>
        /// <returns><c>true</c> if the item was found and removed from the collection; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="item"/> is <c>null</c>.</exception>
        public bool Remove(MenuItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            int count = nativeObject.Items.Count;
            nativeObject.Items.Remove(ObjectRetriever.GetNativeObject(item));
            return count > nativeObject.Items.Count;
        }

        /// <summary>
        /// Removes the menu item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the menu item to be removed.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="index"/> exceeds the upper or lower bound of the collection.</exception>
        public void RemoveAt(int index)
        {
            if (index < 0 || index >= nativeObject.Items.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            nativeObject.Items.RemoveAt(index);
        }

        /// <summary>
        /// Removes a range of menu items from the collection.
        /// </summary>
        /// <param name="index">The zero-based starting index of the range of menu items to remove.</param>
        /// <param name="count">The number of menu items to remove.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="index"/> exceeds the upper or lower bound of the collection -or- when <paramref name="count"/> exceeds the upper bound of the collection.</exception>
        public void RemoveRange(int index, int count)
        {
            if (index < 0 || index >= nativeObject.Items.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            if (count < 0 || index + count > nativeObject.Items.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            for (int i = index; i < index + count; i++)
            {
                nativeObject.Items.RemoveAt(i);
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        public IEnumerator<MenuItem> GetEnumerator()
        {
            return new MenuItemEnumerator(nativeObject.Items.Count > 0 ? nativeObject.Items.GetEnumerator() : null);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new MenuItemEnumerator(nativeObject.Items.Count > 0 ? nativeObject.Items.GetEnumerator() : null);
        }

        int IList.Add(object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return nativeObject.Items.Add(ObjectRetriever.GetNativeObject(value));
        }

        bool IList.Contains(object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return nativeObject.Items.Contains(ObjectRetriever.GetNativeObject(value));
        }

        int IList.IndexOf(object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return nativeObject.Items.IndexOf(ObjectRetriever.GetNativeObject(value));
        }

        void IList.Insert(int index, object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (index == nativeObject.Items.Count)
            {
                nativeObject.Items.Add(ObjectRetriever.GetNativeObject(value));
            }
            else
            {
                nativeObject.Items.Insert(index, ObjectRetriever.GetNativeObject(value));
            }
        }

        void IList.Remove(object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            nativeObject.Items.Remove(ObjectRetriever.GetNativeObject(value));
        }

        void IList.RemoveAt(int index)
        {
            nativeObject.Items.RemoveAt(index);
        }

        void ICollection.CopyTo(Array array, int index)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            for (int i = 0; i < nativeObject.Items.Count; i++)
            {
                array.SetValue(ObjectRetriever.GetAgnosticObject(nativeObject.Items[i]), index + i);
            }
        }

        private class MenuItemEnumerator : IEnumerator<MenuItem>, IEnumerator
        {
            public MenuItem Current
            {
                get { return (MenuItem)ObjectRetriever.GetAgnosticObject(nativeEnumerator?.Current); }
            }

            object IEnumerator.Current
            {
                get { return ObjectRetriever.GetAgnosticObject(nativeEnumerator?.Current); }
            }

#if !DEBUG
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
            private readonly IEnumerator nativeEnumerator;

            public MenuItemEnumerator(IEnumerator nativeEnumerator)
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
