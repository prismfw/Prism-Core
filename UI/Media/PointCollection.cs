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

namespace Prism.UI.Media
{
    /// <summary>
    /// Represents a collection of <see cref="Point"/> objects.
    /// </summary>
    // NOTE: This class is currently only set up to support the Polygon and Polyline classes.
    [DebuggerDisplay("Count = {Count}")]
    public sealed class PointCollection : IList<Point>, IList
    {
        /// <summary>
        /// Gets the number of points within the collection.
        /// </summary>
        public int Count
        {
            get { return nativeObject.Points.Count; }
        }

        /// <summary>
        /// Gets or sets the point at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the point to get or set.</param>
        public Point this[int index]
        {
            get { return nativeObject.Points[index]; }
            set
            {
                nativeObject.Points[index] = value;
                shape.InvalidateMeasure();
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
            get { return nativeObject.Points[index]; }
            set
            {
                nativeObject.Points[index] = (Point)value;
                shape.InvalidateMeasure();
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        bool ICollection<Point>.IsReadOnly
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
        private readonly Visual shape;
#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private readonly INativePolyShape nativeObject;

        internal PointCollection(Visual visual)
        {
            if (visual == null)
            {
                throw new ArgumentNullException(nameof(visual));
            }

            shape = visual;
            nativeObject = (INativePolyShape)ObjectRetriever.GetNativeObject(visual);
        }

        /// <summary>
        /// Adds the specified point to the end of the collection.
        /// </summary>
        /// <param name="item">The point to be added.</param>
        public void Add(Point item)
        {
            nativeObject.Points.Add(item);
            shape.InvalidateMeasure();
        }

        /// <summary>
        /// Adds a range of points to the end of the collection.
        /// </summary>
        /// <param name="items">The points to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="items"/> is <c>null</c>.</exception>
        public void AddRange(IEnumerable<Point> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            foreach (var item in items)
            {
                nativeObject.Points.Add(item);
            }

            shape.InvalidateMeasure();
        }

        /// <summary>
        /// Removes all points from the collection.
        /// </summary>
        public void Clear()
        {
            nativeObject.Points.Clear();
            shape.InvalidateMeasure();
        }

        /// <summary>
        /// Determines whether the collection contains the specified point.
        /// </summary>
        /// <param name="item">The point to locate in the collection.</param>
        /// <returns><c>true</c> if the point is found in the collection; otherwise, <c>false</c>.</returns>
        public bool Contains(Point item)
        {
            return nativeObject.Points.Contains(item);
        }

        /// <summary>
        /// Copies the points of the collection to the specified array, starting at the specified array index.
        /// </summary>
        /// <param name="array">The array that is the destination of the points being copied from the collection.</param>
        /// <param name="arrayIndex">The zero-based index in the array at which copying begins.</param>
        /// <exception cref="ArgumentNullException">Throw when <paramref name="array"/> is <c>null</c>.</exception>
        public void CopyTo(Point[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            for (int i = 0; i < nativeObject.Points.Count; i++)
            {
                array[arrayIndex + i] = nativeObject.Points[i];
            }
        }

        /// <summary>
        /// Determines the index of the specified point in the collection.
        /// </summary>
        /// <param name="item">The point to locate in the collection.</param>
        /// <returns>The index of the point if it is found in the collection; otherwise, -1.</returns>
        public int IndexOf(Point item)
        {
            return nativeObject.Points.IndexOf(item);
        }

        /// <summary>
        /// Inserts the specified point into the collection at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which the point should be inserted.</param>
        /// <param name="item">The point to be inserted.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="index"/> exceeds the upper or lower bound of the collection.</exception>
        public void Insert(int index, Point item)
        {
            if (index > nativeObject.Points.Count || index < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            if (index == nativeObject.Points.Count)
            {
                nativeObject.Points.Add(item);
            }
            else
            {
                nativeObject.Points.Insert(index, item);
            }

            shape.InvalidateMeasure();
        }

        /// <summary>
        /// Inserts a range of points into the collection at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which the points should be inserted.</param>
        /// <param name="items">The points to be inserted.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="items"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="index"/> exceeds the upper or lower bound of the collection.</exception>
        public void InsertRange(int index, IEnumerable<Point> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            if (index > nativeObject.Points.Count || index < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            foreach (var item in items)
            {
                if (index == nativeObject.Points.Count)
                {
                    nativeObject.Points.Add(item);
                }
                else
                {
                    nativeObject.Points.Insert(index, item);
                }
                index++;
            }

            shape.InvalidateMeasure();
        }

        /// <summary>
        /// Removes the specified point from the collection.
        /// </summary>
        /// <param name="item">The point to be removed.</param>
        /// <returns><c>true</c> if the point was found and removed from the collection; otherwise, <c>false</c>.</returns>
        public bool Remove(Point item)
        {
            if (nativeObject.Points.Remove(item))
            {
                shape.InvalidateMeasure();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Removes the point at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the point to be removed.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="index"/> exceeds the upper or lower bound of the collection.</exception>
        public void RemoveAt(int index)
        {
            if (index < 0 || index >= nativeObject.Points.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            nativeObject.Points.RemoveAt(index);
            shape.InvalidateMeasure();
        }

        /// <summary>
        /// Removes a range of points from the collection.
        /// </summary>
        /// <param name="index">The zero-based starting index of the range of points to remove.</param>
        /// <param name="count">The number of points to remove.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="index"/> exceeds the upper or lower bound of the collection -or- when <paramref name="count"/> exceeds the upper bound of the collection.</exception>
        public void RemoveRange(int index, int count)
        {
            if (index < 0 || index >= nativeObject.Points.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            if (count < 0 || index + count > nativeObject.Points.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            for (int i = index; i < index + count; i++)
            {
                nativeObject.Points.RemoveAt(i);
            }

            shape.InvalidateMeasure();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        public IEnumerator<Point> GetEnumerator()
        {
            return new PointEnumerator(nativeObject.Points.Count > 0 ? nativeObject.Points.GetEnumerator() : null);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new PointEnumerator(nativeObject.Points.Count > 0 ? nativeObject.Points.GetEnumerator() : null);
        }

        int IList.Add(object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            int count = nativeObject.Points.Count;
            nativeObject.Points.Add((Point)value);
            count = nativeObject.Points.Count - count;

            if (count > 0)
            {
                shape.InvalidateMeasure();
            }
            return count;
        }

        bool IList.Contains(object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return nativeObject.Points.Contains((Point)value);
        }

        int IList.IndexOf(object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return nativeObject.Points.IndexOf((Point)value);
        }

        void IList.Insert(int index, object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (index > nativeObject.Points.Count || index < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            if (index == nativeObject.Points.Count)
            {
                nativeObject.Points.Add((Point)value);
            }
            else
            {
                nativeObject.Points.Insert(index, (Point)value);
            }

            shape.InvalidateMeasure();
        }

        void IList.Remove(object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (nativeObject.Points.Remove((Point)value))
            {
                shape.InvalidateMeasure();
            }
        }

        void IList.RemoveAt(int index)
        {
            if (index < 0 || index >= nativeObject.Points.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            nativeObject.Points.RemoveAt(index);
            shape.InvalidateMeasure();
        }

        void ICollection.CopyTo(Array array, int index)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            for (int i = 0; i < nativeObject.Points.Count; i++)
            {
                array.SetValue(nativeObject.Points[i], index + i);
            }
        }

        private class PointEnumerator : IEnumerator<Point>, IEnumerator
        {
            public Point Current
            {
                get { return nativeEnumerator == null ? new Point() : (Point)nativeEnumerator.Current; }
            }

            object IEnumerator.Current
            {
                get { return nativeEnumerator == null ? new Point() : (Point)nativeEnumerator.Current; }
            }

#if !DEBUG
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
            private readonly IEnumerator nativeEnumerator;

            public PointEnumerator(IEnumerator nativeEnumerator)
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
