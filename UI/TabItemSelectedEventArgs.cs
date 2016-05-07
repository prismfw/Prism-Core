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
using Prism.UI.Controls;

namespace Prism.UI
{
    /// <summary>
    /// Provides data for the <see cref="E:TabItemSelected"/> event.
    /// </summary>
    public class TabItemSelectedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the <see cref="TabItem"/> instance that is now selected.
        /// </summary>
        public TabItem CurrentTabItem { get; }

        /// <summary>
        /// Gets the <see cref="TabItem"/> instance that was previously selected.
        /// </summary>
        public TabItem PreviousTabItem { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TabItemSelectedEventArgs"/> class.
        /// </summary>
        /// <param name="previousTabItem">The <see cref="TabItem"/> instance that was previously selected.</param>
        /// <param name="currentTabItem">The <see cref="TabItem"/> instance that is now selected.</param>
        public TabItemSelectedEventArgs(TabItem previousTabItem, TabItem currentTabItem)
        {
            CurrentTabItem = currentTabItem;
            PreviousTabItem = previousTabItem;
        }
    }
}
