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


using System;

namespace Prism.UI.Media
{
    /// <summary>
    /// Represents a straight line in a <see cref="PathFigure"/>.
    /// </summary>
    public sealed class LineSegment : PathSegment
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LineSegment"/> class.
        /// </summary>
        public LineSegment()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LineSegment"/> class.
        /// </summary>
        /// <param name="endPoint">The point at which the segment ends.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="endPoint"/> contains a NaN or infinite value.</exception>
        public LineSegment(Point endPoint)
            : base(endPoint)
        {
        }

        internal override Point GetBottomLeftCorner(Point startPoint)
        {
            return new Point(Math.Max(startPoint.X, EndPoint.X), Math.Max(startPoint.Y, EndPoint.Y));
        }
    }
}
