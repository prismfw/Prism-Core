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


namespace Prism.UI.Media
{
    /// <summary>
    /// Describes how the interior fill of a shape is determined.
    /// </summary>
    public enum FillRule
    {
        /// <summary>
        /// Determines if a point is inside of the shape by drawing a ray from that point to infinity
        /// in any direction and counting the number of times that the ray crosses one of the shape's
        /// path segments.  If the number of crossings is odd, the point is considered to be inside
        /// of the shape; if even, the point is considered to be outside of the shape.
        /// </summary>
        EvenOdd = 0,
        /// <summary>
        /// Determines if a point is inside of a shape by drawing a ray from that point to infinity
        /// in any direction and counting the number of times one of the shape's path segments crosses
        /// the ray.  If the segment crosses the ray from left to right, the count is incremented by one;
        /// if the segment crosses the ray from right to left, the count is decremented by one.  If the
        /// total count equals zero, the point is considered to be outside of the shape; otherwise,
        /// the point is considered to be inside of the shape.
        /// </summary>
        Nonzero = 1,
    }
}
