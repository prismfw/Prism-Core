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
    /// Represents a collection of <see cref="PathSegment"/> objects that make up a <see cref="PathFigure"/>.
    /// </summary>
    [DebuggerDisplay("Count = {Count}")]
    public sealed class PathSegmentCollection : IList<PathSegment>, IList
    {
        /// <summary>
        /// Gets the number of path segments within the collection.
        /// </summary>
        public int Count
        {
            get { return collection.Count; }
        }

        /// <summary>
        /// Gets or sets the path segment at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the path segment to get or set.</param>
        public PathSegment this[int index]
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

                SetItem(index, (PathSegment)value);
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        bool ICollection<PathSegment>.IsReadOnly
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
        private readonly Collection<PathSegment> collection;
#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private readonly PathFigure pathFigure;

        internal PathSegmentCollection(PathFigure figure)
        {
            if (figure == null)
            {
                throw new ArgumentNullException(nameof(figure));
            }

            pathFigure = figure;
            collection = new Collection<PathSegment>();
        }

        /// <summary>
        /// Adds the specified path segment to the end of the collection.
        /// </summary>
        /// <param name="item">The path segment to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="item"/> is <c>null</c>.</exception>
        public void Add(PathSegment item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            SetItem(collection.Count, item);
        }

        /// <summary>
        /// Adds a range of path segments to the end of the collection.
        /// </summary>
        /// <param name="items">The path segments to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="items"/> is <c>null</c>.</exception>
        public void AddRange(IEnumerable<PathSegment> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            foreach (var item in items)
            {
                SetItem(collection.Count, item, false);
            }

            pathFigure.Owner?.Invalidate();
        }

        /// <summary>
        /// Removes all path segments from the collection.
        /// </summary>
        public void Clear()
        {
            for (int i = 0; i < collection.Count; i++)
            {
                collection[i].Owner = null;
            }

            collection.Clear();
            pathFigure.Owner?.Invalidate();
        }

        /// <summary>
        /// Determines whether the collection contains the specified path segment.
        /// </summary>
        /// <param name="item">The path segment to locate in the collection.</param>
        /// <returns><c>true</c> if the path segment is found in the collection; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="item"/> is <c>null</c>.</exception>
        public bool Contains(PathSegment item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return collection.Contains(item);
        }

        /// <summary>
        /// Copies deep-copy clones of the path segments in the collection to the specified array, starting at the specified array index.
        /// </summary>
        /// <param name="array">The array that is the destination of the path segments being copied from the collection.</param>
        /// <param name="arrayIndex">The zero-based index in the array at which copying begins.</param>
        /// <exception cref="ArgumentNullException">Throw when <paramref name="array"/> is <c>null</c>.</exception>
        public void CopyTo(PathSegment[] array, int arrayIndex)
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
        /// Determines the index of the specified path segment in the collection.
        /// </summary>
        /// <param name="item">The path segment to locate in the collection.</param>
        /// <returns>The index of the path segment if it is found in the collection; otherwise, -1.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="item"/> is <c>null</c>.</exception>
        public int IndexOf(PathSegment item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return collection.IndexOf(item);
        }

        /// <summary>
        /// Inserts the specified path segment into the collection at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which the path segment should be inserted.</param>
        /// <param name="item">The path segment to be inserted.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="item"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="index"/> exceeds the upper or lower bound of the collection.</exception>
        public void Insert(int index, PathSegment item)
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
        /// Inserts a range of path segments into the collection at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which the path segments should be inserted.</param>
        /// <param name="items">The path segments to be inserted.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="items"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="index"/> exceeds the upper or lower bound of the collection.</exception>
        public void InsertRange(int index, IEnumerable<PathSegment> items)
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

            pathFigure.Owner?.Invalidate();
        }

        /// <summary>
        /// Removes the specified path segment from the collection.
        /// </summary>
        /// <param name="item">The path segment to be removed.</param>
        /// <returns><c>true</c> if the path segment was found and removed from the collection; otherwise, <c>false</c>.</returns>
        public bool Remove(PathSegment item)
        {
            return RemoveItem(item);
        }

        /// <summary>
        /// Removes the path segment at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the path segment to be removed.</param>
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
        /// Removes a range of path segments from the collection.
        /// </summary>
        /// <param name="index">The zero-based starting index of the range of path segments to remove.</param>
        /// <param name="count">The number of path segments to remove.</param>
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

            pathFigure.Owner?.Invalidate();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        public IEnumerator<PathSegment> GetEnumerator()
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
            SetItem(count, (PathSegment)value);
            return collection.Count - count;
        }

        bool IList.Contains(object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return collection.Contains((PathSegment)value);
        }

        int IList.IndexOf(object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return collection.IndexOf((PathSegment)value);
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

            InsertItem(index, (PathSegment)value);
        }

        void IList.Remove(object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            RemoveItem((PathSegment)value);
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

        private void InsertItem(int index, PathSegment item, bool invalidate = true)
        {
            if (item != null)
            {
                if (item.Owner != null && item.Owner != pathFigure)
                {
                    throw new ArgumentException(Resources.Strings.PathSegmentCannotHaveMultipleOwners);
                }

                collection.Insert(index, item);

                item.Owner = pathFigure;
                if (invalidate)
                {
                    pathFigure.Owner?.Invalidate();
                }
            }
        }

        private bool RemoveItem(PathSegment item, bool invalidate = true)
        {
            if (item != null && collection.Remove(item))
            {
                item.Owner = null;
                if (invalidate)
                {
                    pathFigure.Owner?.Invalidate();
                }

                return true;
            }

            return false;
        }

        private void SetItem(int index, PathSegment item, bool invalidate = true)
        {
            if (item != null)
            {
                if (item.Owner != null && item.Owner != pathFigure)
                {
                    throw new ArgumentException(Resources.Strings.PathSegmentCannotHaveMultipleOwners);
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

                item.Owner = pathFigure;
                if (invalidate)
                {
                    pathFigure.Owner?.Invalidate();
                }
            }
        }
    }
}
