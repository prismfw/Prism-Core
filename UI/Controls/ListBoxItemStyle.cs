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


namespace Prism.UI.Controls
{
    /// <summary>
    /// Describes the style of a <see cref="ListBoxItem"/>, which determines the elements that are provided on initialization.
    /// </summary>
    public enum ListBoxItemStyle
    {
        /// <summary>
        /// No elements are provided on initialization; the item is empty.
        /// </summary>
        Empty = 0,
        /// <summary>
        /// A <see cref="Grid"/> is provided with one row, two columns, an <see cref="Image"/>, and a <see cref="Label"/> aligned to the left.
        /// </summary>
        Default = 1,
        /// <summary>
        /// A <see cref="Grid"/> is provided with two rows, two columns, an <see cref="Image"/>,
        /// and two <see cref="Label"/>s stacked vertically and aligned to the left.
        /// </summary>
        Detail = 3,
        /// <summary>
        /// A <see cref="Grid"/> is provided with one row, three columns, an <see cref="Image"/>,
        /// a <see cref="Label"/> aligned to the left, and another <see cref="Label"/> aligned to the right.
        /// </summary>
        Value = 5,
        /// <summary>
        /// A <see cref="Grid"/> is provided with two rows, three columns, an <see cref="Image"/>,
        /// two <see cref="Label"/>s stacked vertically and aligned to the left, and another <see cref="Label"/> aligned to the right.
        /// </summary>
        Full = 7,
    }
}
