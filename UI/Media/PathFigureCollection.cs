/*
Copyright (C) 2017  Prism Framework Team

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
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Prism.UI.Media
{
    /// <summary>
    /// Represents a collection of <see cref="PathFigure"/> objects.
    /// </summary>
    [DebuggerDisplay("Count = {Count}")]
    public sealed class PathFigureCollection : IList<PathFigure>, IList
    {
        /// <summary>
        /// Gets the number of path figures within the collection.
        /// </summary>
        public int Count
        {
            get { return collection.Count; }
        }

        /// <summary>
        /// Gets or sets the path figure at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the path figure to get or set.</param>
        public PathFigure this[int index]
        {
            get { return collection[index]; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                if (index >= collection.Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }

                SetItem(index, value);
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
            get { return collection[index]; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                if (index >= collection.Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }

                SetItem(index, (PathFigure)value);
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        bool ICollection<PathFigure>.IsReadOnly
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
        private readonly Collection<PathFigure> collection;
#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private readonly Shapes.Path path;

        internal PathFigureCollection(Shapes.Path path)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            this.path = path;
            collection = new Collection<PathFigure>();
        }

        /// <summary>
        /// Adds the specified path figure to the end of the collection.
        /// </summary>
        /// <param name="item">The path figure to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="item"/> is <c>null</c>.</exception>
        public void Add(PathFigure item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            SetItem(collection.Count, item);
        }

        /// <summary>
        /// Adds a range of path figures to the end of the collection.
        /// </summary>
        /// <param name="items">The path figures to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="items"/> is <c>null</c>.</exception>
        public void AddRange(IEnumerable<PathFigure> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            foreach (var item in items)
            {
                SetItem(collection.Count, item, false);
            }

            path.Invalidate();
        }

        /// <summary>
        /// Removes all path figures from the collection.
        /// </summary>
        public void Clear()
        {
            for (int i = 0; i < collection.Count; i++)
            {
                collection[i].Owner = null;
            }

            collection.Clear();
            path.Invalidate();
        }

        /// <summary>
        /// Determines whether the collection contains the specified path figure.
        /// </summary>
        /// <param name="item">The path figure to locate in the collection.</param>
        /// <returns><c>true</c> if the path figure is found in the collection; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="item"/> is <c>null</c>.</exception>
        public bool Contains(PathFigure item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return collection.Contains(item);
        }

        /// <summary>
        /// Copies deep-copy clones of the path figures in the collection to the specified array, starting at the specified array index.
        /// </summary>
        /// <param name="array">The array that is the destination of the path figures being copied from the collection.</param>
        /// <param name="arrayIndex">The zero-based index in the array at which copying begins.</param>
        /// <exception cref="ArgumentNullException">Throw when <paramref name="array"/> is <c>null</c>.</exception>
        public void CopyTo(PathFigure[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            for (int i = 0; i < collection.Count; i++)
            {
                array[arrayIndex + i] = collection[i].Clone();
            }
        }

        /// <summary>
        /// Determines the index of the specified path figure in the collection.
        /// </summary>
        /// <param name="item">The path figure to locate in the collection.</param>
        /// <returns>The index of the path figure if it is found in the collection; otherwise, -1.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="item"/> is <c>null</c>.</exception>
        public int IndexOf(PathFigure item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return collection.IndexOf(item);
        }

        /// <summary>
        /// Inserts the specified path figure into the collection at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which the path figure should be inserted.</param>
        /// <param name="item">The path figure to be inserted.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="item"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="index"/> exceeds the upper or lower bound of the collection.</exception>
        public void Insert(int index, PathFigure item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            if (index > collection.Count || index < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            InsertItem(index, item);
        }

        /// <summary>
        /// Inserts a range of path figures into the collection at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which the path figures should be inserted.</param>
        /// <param name="items">The path figures to be inserted.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="items"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="index"/> exceeds the upper or lower bound of the collection.</exception>
        public void InsertRange(int index, IEnumerable<PathFigure> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            if (index > collection.Count || index < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            foreach (var item in items)
            {
                InsertItem(index++, item, false);
            }

            path.Invalidate();
        }

        /// <summary>
        /// Removes the specified path figure from the collection.
        /// </summary>
        /// <param name="item">The path figure to be removed.</param>
        /// <returns><c>true</c> if the path figure was found and removed from the collection; otherwise, <c>false</c>.</returns>
        public bool Remove(PathFigure item)
        {
            return RemoveItem(item);
        }

        /// <summary>
        /// Removes the path figure at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the path figure to be removed.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="index"/> exceeds the upper or lower bound of the collection.</exception>
        public void RemoveAt(int index)
        {
            if (index < 0 || index >= collection.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            RemoveItem(collection[index]);
        }

        /// <summary>
        /// Removes a range of path figures from the collection.
        /// </summary>
        /// <param name="index">The zero-based starting index of the range of path figures to remove.</param>
        /// <param name="count">The number of path figures to remove.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="index"/> exceeds the upper or lower bound of the collection -or- when <paramref name="count"/> exceeds the upper bound of the collection.</exception>
        public void RemoveRange(int index, int count)
        {
            if (index < 0 || index >= collection.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            if (count < 0 || index + count > collection.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            for (int i = index; i < index + count; i++)
            {
                RemoveItem(collection[index], false);
            }

            path.Invalidate();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        public IEnumerator<PathFigure> GetEnumerator()
        {
            return collection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return collection.GetEnumerator();
        }

        int IList.Add(object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            int count = collection.Count;
            SetItem(count, (PathFigure)value);
            return collection.Count - count;
        }

        bool IList.Contains(object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return collection.Contains((PathFigure)value);
        }

        int IList.IndexOf(object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return collection.IndexOf((PathFigure)value);
        }

        void IList.Insert(int index, object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (index > collection.Count || index < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            InsertItem(index, (PathFigure)value);
        }

        void IList.Remove(object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            RemoveItem((PathFigure)value);
        }

        void IList.RemoveAt(int index)
        {
            if (index < 0 || index >= collection.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            RemoveItem(collection[index]);
        }

        void ICollection.CopyTo(Array array, int index)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            for (int i = 0; i < collection.Count; i++)
            {
                array.SetValue(collection[i].Clone(), index + i);
            }
        }

        private void InsertItem(int index, PathFigure item, bool invalidate = true)
        {
            if (item != null)
            {
                if (item.Owner != null && item.Owner != path)
                {
                    throw new ArgumentException(Resources.Strings.PathFigureCannotHaveMultipleOwners);
                }

                collection.Insert(index, item);

                item.Owner = path;
                if (invalidate)
                {
                    path.Invalidate();
                }
            }
        }

        private bool RemoveItem(PathFigure item, bool invalidate = true)
        {
            if (item != null && collection.Remove(item))
            {
                item.Owner = null;
                if (invalidate)
                {
                    path.Invalidate();
                }

                return true;
            }

            return false;
        }

        private void SetItem(int index, PathFigure item, bool invalidate = true)
        {
            if (item != null)
            {
                if (item.Owner != null && item.Owner != path)
                {
                    throw new ArgumentException(Resources.Strings.PathFigureCannotHaveMultipleOwners);
                }

                if (index == collection.Count)
                {
                    collection.Add(item);
                }
                else
                {
                    collection[index].Owner = null;
                    collection[index] = item;
                }

                item.Owner = path;
                if (invalidate)
                {
                    path.Invalidate();
                }
            }
        }
    }
}
