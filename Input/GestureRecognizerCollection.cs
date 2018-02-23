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
using System.Collections.Generic;
using System.Diagnostics;

namespace Prism.Input
{
    /// <summary>
    /// Represents a collection of <see cref="GestureRecognizer"/> objects.
    /// </summary>
    [DebuggerDisplay("Count = {Count}")]
    public sealed class GestureRecognizerCollection : IList<GestureRecognizer>
    {
        /// <summary>
        /// Gets the number of gesture recognizers contained within the collection.
        /// </summary>
        public int Count
        {
            get { return items.Count; }
        }

        /// <summary>
        /// Gets or sets the gesture recognizer at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the gesture recognizer to get or set.</param>
        public GestureRecognizer this[int index]
        {
            get { return items[index]; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                if (index >= items.Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }

                SetItem(index, value);
            }
        }
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        bool ICollection<GestureRecognizer>.IsReadOnly
        {
            get { return items.IsReadOnly; }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        private readonly IList<GestureRecognizer> items;

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private readonly object targetObject;
        
        internal GestureRecognizerCollection(object target)
        {
            items = new List<GestureRecognizer>();
            targetObject = target;
        }

        /// <summary>
        /// Adds a <see cref="GestureRecognizer"/> to the end of the collection.
        /// </summary>
        /// <param name="item">The gesture recognizer to add to the collection.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="item"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="item"/> is already targeting another element.</exception>
        public void Add(GestureRecognizer item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            SetItem(items.Count, item);
        }

        /// <summary>
        /// Removes all gesture recognizers from the collection.
        /// </summary>
        public void Clear()
        {
            for (int i = items.Count - 1; i >= 0; i--)
            {
                items[i].ClearTarget();
                items.RemoveAt(i);
            }
        }

        /// <summary>
        /// Determines whether the collection contains the specified gesture recognizer.
        /// </summary>
        /// <param name="item">The gesture recognizer to locate in the collection.</param>
        /// <returns><c>true</c> if the gesture recognizer is found in the collection; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="item"/> is <c>null</c>.</exception>
        public bool Contains(GestureRecognizer item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return items.Contains(item);
        }

        /// <summary>
        /// Copies the gesture recognizers of the collection to the specified array, starting at the specified array index.
        /// </summary>
        /// <param name="array">The array that is the destination of the gesture recognizers being copied from the collection.</param>
        /// <param name="arrayIndex">The zero-based index in the array at which copying begins.</param>
        public void CopyTo(GestureRecognizer[] array, int arrayIndex)
        {
            items.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Determines the index of the specified gesture recognizer in the collection.
        /// </summary>
        /// <param name="item">The gesture recognizer to locate in the collection.</param>
        /// <returns>The index of the gesture recognizer if it is found in the collection; otherwise, -1.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="item"/> is <c>null</c>.</exception>
        public int IndexOf(GestureRecognizer item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return items.IndexOf(item);
        }

        /// <summary>
        /// Inserts a <see cref="GestureRecognizer"/> into the collection at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which the gesture recognizer should be inserted.</param>
        /// <param name="item">The gesture recognizer to insert into the collection.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="item"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="item"/> is already targeting another element.</exception>
        public void Insert(int index, GestureRecognizer item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            
            if (item.Target != null && item.Target != targetObject)
            {
                throw new ArgumentException(Resources.Strings.GestureRecognizerCannotHaveMultipleTargets);
            }

            items.Insert(index, item);

            try
            {
                item.SetTarget(targetObject);
            }
            catch
            {
                items.Remove(item);
                throw;
            }
        }

        /// <summary>
        /// Removes the specified gesture recognizer from the collection.
        /// </summary>
        /// <param name="item">The gesture recognizer to be removed.</param>
        /// <returns><c>true</c> if the gesture recognizer was found and removed from the collection; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="item"/> is <c>null</c>.</exception>
        public bool Remove(GestureRecognizer item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            if (item.Target == targetObject)
            {
                item.ClearTarget();
                items.Remove(item);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Removes the gesture recognizer at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the gesture recognizer to be removed.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="index"/> exceeds the upper or lower bound of the collection.</exception>
        public void RemoveAt(int index)
        {
            if (index < 0 || index >= items.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            items[index].ClearTarget();
            items.RemoveAt(index);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        public IEnumerator<GestureRecognizer> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return items.GetEnumerator();
        }

        private void SetItem(int index, GestureRecognizer item)
        {
            if (item != null)
            {
                if (item.Target != null && item.Target != targetObject)
                {
                    throw new ArgumentException(Resources.Strings.GestureRecognizerCannotHaveMultipleTargets);
                }

                if (index == items.Count)
                {
                    items.Add(item);
                }
                else
                {
                    items[index].ClearTarget();
                    items[index] = item;
                }

                try
                {
                    item.SetTarget(targetObject);
                }
                catch
                {
                    items.Remove(item);
                    throw;
                }
            }
        }
    }
}
