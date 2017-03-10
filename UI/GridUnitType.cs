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


using Prism.UI.Controls;

namespace Prism.UI
{
    /// <summary>
    /// Describes the kind of value that a <see cref="RowDefinition"/> or <see cref="ColumnDefinition"/> is holding.
    /// </summary>
    public enum GridUnitType
    {
        /// <summary>
        /// The row or column is automatically sized to fit the elements within it.
        /// </summary>
        Auto = 0,
        /// <summary>
        /// The value represents units in the current platform's coordinate system (pixels, points, etc).
        /// </summary>
        Absolute = 1,
        /// <summary>
        /// The value represents a weighted proportion of the available space after all other rows or columns have been sized.
        /// </summary>
        Star = 2
    }
}
