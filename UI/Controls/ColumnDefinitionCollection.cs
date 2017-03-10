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
    /// Represents a collection of <see cref="ColumnDefinition"/> objects for a <see cref="Grid"/>.
    /// </summary>
    [DebuggerDisplay("Count = {Count}")]
    public sealed class ColumnDefinitionCollection : IList<ColumnDefinition>
    {
        /// <summary>
        /// Gets the number of columns contained within the collection.
        /// </summary>
        public int Count
        {
            get { return items.Count; }
        }

        /// <summary>
        /// Gets or sets the column at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the column to get or set.</param>
        public ColumnDefinition this[int index]
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
        bool ICollection<ColumnDefinition>.IsReadOnly
        {
            get { return items.IsReadOnly; }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        private readonly IList<ColumnDefinition> items;

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnDefinitionCollection"/> class.
        /// </summary>
        public ColumnDefinitionCollection()
        {
            items = new List<ColumnDefinition>();
        }

        /// <summary>
        /// Adds a <see cref="ColumnDefinition"/> to the end of the collection.
        /// </summary>
        /// <param name="item">The column to add to the collection.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="item"/> is <c>null</c>.</exception>
        public void Add(ColumnDefinition item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            items.Add(item);
        }

        /// <summary>
        /// Removes all columns from the collection.
        /// </summary>
        public void Clear()
        {
            items.Clear();
        }

        /// <summary>
        /// Determines whether the collection contains the specified column.
        /// </summary>
        /// <param name="item">The column to locate in the collection.</param>
        /// <returns><c>true</c> if the column is found in the collection; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="item"/> is <c>null</c>.</exception>
        public bool Contains(ColumnDefinition item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return items.Contains(item);
        }

        /// <summary>
        /// Copies the columns of the collection to the specified array, starting at the specified array index.
        /// </summary>
        /// <param name="array">The array that is the destination of the columns being copied from the collection.</param>
        /// <param name="arrayIndex">The zero-based index in the array at which copying begins.</param>
        public void CopyTo(ColumnDefinition[] array, int arrayIndex)
        {
            items.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Determines the index of the specified column in the collection.
        /// </summary>
        /// <param name="item">The column to locate in the collection.</param>
        /// <returns>The index of the column if it is found in the collection; otherwise, -1.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="item"/> is <c>null</c>.</exception>
        public int IndexOf(ColumnDefinition item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return items.IndexOf(item);
        }

        /// <summary>
        /// Inserts a <see cref="ColumnDefinition"/> into the collection at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which the column should be inserted.</param>
        /// <param name="item">The column to insert into the collection.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="item"/> is <c>null</c>.</exception>
        public void Insert(int index, ColumnDefinition item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            items.Insert(index, item);
        }

        /// <summary>
        /// Removes the specified column from the collection.
        /// </summary>
        /// <param name="item">The column to be removed.</param>
        /// <returns><c>true</c> if the column was found and removed from the collection; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="item"/> is <c>null</c>.</exception>
        public bool Remove(ColumnDefinition item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return items.Remove(item);
        }

        /// <summary>
        /// Removes the column at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the column to be removed.</param>
        public void RemoveAt(int index)
        {
            items.RemoveAt(index);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        public IEnumerator<ColumnDefinition> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return items.GetEnumerator();
        }
    }
}
