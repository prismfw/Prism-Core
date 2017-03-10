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
using System.Diagnostics.CodeAnalysis;

namespace Prism.UI.Controls
{
    /// <summary>
    /// Provides data for SelectionChanged events.
    /// </summary>
    public class SelectionChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets a collection of the items that have been selected.
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Justification = "Reading the property does not produce a copy of the array and modifying the contents of the array has no effect on internal framework functions.")]
        public object[] AddedItems { get; }

        /// <summary>
        /// Gets a collection of the items that have been deselected.
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Justification = "Reading the property does not produce a copy of the array and modifying the contents of the array has no effect on internal framework functions.")]
        public object[] RemovedItems { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectionChangedEventArgs"/> class.
        /// </summary>
        /// <param name="addedItem">The item that has been selected.</param>
        /// <param name="removedItem">The item that has been deselected.</param>
        public SelectionChangedEventArgs(object addedItem, object removedItem)
        {
            AddedItems = addedItem == null ? new object[0] : new[] { addedItem };
            RemovedItems = removedItem == null ? new object[0] : new[] { removedItem };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectionChangedEventArgs"/> class.
        /// </summary>
        /// <param name="addedItems">The items that have been selected.</param>
        /// <param name="removedItems">The items that have been deselected.</param>
        public SelectionChangedEventArgs(object[] addedItems, object[] removedItems)
        {
            AddedItems = addedItems ?? new object[0];
            RemovedItems = removedItems ?? new object[0];
        }
    }
}
