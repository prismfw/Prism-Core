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

namespace Prism.Native
{
    /// <summary>
    /// Provides data for native selection events.
    /// </summary>
    public class NativeItemSelectedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the item that is now selected.
        /// </summary>
        public object CurrentItem { get; }

        /// <summary>
        /// Gets the item that was previously selected.
        /// </summary>
        public object PreviousItem { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NativeItemSelectedEventArgs"/> class.
        /// </summary>
        /// <param name="previousItem">The object that was previously selected.</param>
        /// <param name="currentItem">The object instance that is now selected.</param>
        public NativeItemSelectedEventArgs(object previousItem, object currentItem)
        {
            CurrentItem = currentItem;
            PreviousItem = previousItem;
        }
    }
}

