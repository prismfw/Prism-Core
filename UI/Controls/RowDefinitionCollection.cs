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
using System.Collections.Generic;
using System.Diagnostics;

namespace Prism.UI.Controls
{
    /// <summary>
    /// Represents a collection of <see cref="RowDefinition"/> objects for a <see cref="Grid"/>.
    /// </summary>
    [DebuggerDisplay("Count = {Count}")]
    public sealed class RowDefinitionCollection : IList<RowDefinition>
    {
        /// <summary>
        /// Gets the number of rows contained within the collection.
        /// </summary>
        public int Count
        {
            get { return items.Count; }
        }

        /// <summary>
        /// Gets or sets the row at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the row to get or set.</param>
        public RowDefinition this[int index]
        {
            get { return items[index]; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                items[index] = value;
            }
        }
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        bool ICollection<RowDefinition>.IsReadOnly
        {
            get { return items.IsReadOnly; }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        private readonly IList<RowDefinition> items;

        /// <summary>
        /// Initializes a new instance of the <see cref="RowDefinitionCollection"/> class.
        /// </summary>
        public RowDefinitionCollection()
        {
            items = new List<RowDefinition>();
        }

        /// <summary>
        /// Adds a <see cref="RowDefinition"/> to the end of the collection.
        /// </summary>
        /// <param name="item">The row to add to the collection.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="item"/> is <c>null</c>.</exception>
        public void Add(RowDefinition item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            items.Add(item);
        }

        /// <summary>
        /// Removes all rows from the collection.
        /// </summary>
        public void Clear()
        {
            items.Clear();
        }

        /// <summary>
        /// Determines whether the collection contains the specified row.
        /// </summary>
        /// <param name="item">The row to locate in the collection.</param>
        /// <returns><c>true</c> if the row is found in the collection; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="item"/> is <c>null</c>.</exception>
        public bool Contains(RowDefinition item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return items.Contains(item);
        }

        /// <summary>
        /// Copies the rows of the collection to the specified array, starting at the specified array index.
        /// </summary>
        /// <param name="array">The array that is the destination of the rows being copied from the collection.</param>
        /// <param name="arrayIndex">The zero-based index in the array at which copying begins.</param>
        public void CopyTo(RowDefinition[] array, int arrayIndex)
        {
            items.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Determines the index of the specified row in the collection.
        /// </summary>
        /// <param name="item">The row to locate in the collection.</param>
        /// <returns>The index of the row if it is found in the collection; otherwise, -1.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="item"/> is <c>null</c>.</exception>
        public int IndexOf(RowDefinition item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return items.IndexOf(item);
        }

        /// <summary>
        /// Inserts a <see cref="RowDefinition"/> into the collection at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which the row should be inserted.</param>
        /// <param name="item">The row to insert into the collection.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="item"/> is <c>null</c>.</exception>
        public void Insert(int index, RowDefinition item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            items.Insert(index, item);
        }

        /// <summary>
        /// Removes the specified row from the collection.
        /// </summary>
        /// <param name="item">The row to be removed.</param>
        /// <returns><c>true</c> if the row was found and removed from the collection; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="item"/> is <c>null</c>.</exception>
        public bool Remove(RowDefinition item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return items.Remove(item);
        }

        /// <summary>
        /// Removes the row at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the row to be removed.</param>
        public void RemoveAt(int index)
        {
            items.RemoveAt(index);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        public IEnumerator<RowDefinition> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return items.GetEnumerator();
        }
    }
}
