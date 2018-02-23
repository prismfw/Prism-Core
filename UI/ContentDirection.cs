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


namespace Prism.UI
{
    /// <summary>
    /// Describes the flow direction of UI elements within a parent container.
    /// </summary>
    public enum ContentDirection
    {
        /// <summary>
        /// The flow direction should go from right to left.
        /// </summary>
        Left = 0,
        /// <summary>
        /// The flow direction should go from left to right.
        /// </summary>
        Right = 2,
        /// <summary>
        /// The flow direction should go from bottom to top.
        /// </summary>
        Up = 1,
        /// <summary>
        /// The flow direction should go from top to bottom.
        /// </summary>
        Down = 3
    }
}
