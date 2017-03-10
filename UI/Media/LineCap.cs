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


namespace Prism.UI.Media
{
    /// <summary>
    /// Describes the manner in which the ends of a line are drawn.
    /// </summary>
    public enum LineCap
    {
        /// <summary>
        /// Line ends do not extend beyond their start and end points.
        /// </summary>
        Flat = 0,
        /// <summary>
        /// Line ends are squared into a rectangle whose height is equal to the thickness of the line
        /// and whose width is equal to half of the thickness of the line.
        /// </summary>
        Square = 1,
        /// <summary>
        /// Line ends are rounded into a semicircle whose diameter is equal to the thickness of the line.
        /// </summary>
        Round = 2
    }
}
