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
using System.Collections.ObjectModel;

namespace Prism.UI.Media
{
    /// <summary>
    /// Represents a brush with several colors in a linear gradient.
    /// </summary>
    public class LinearGradientBrush : Brush
    {
        /// <summary>
        /// Gets a collection of the colors in the gradient.
        /// </summary>
        public ReadOnlyCollection<Color> Colors { get; }

        /// <summary>
        /// Gets the ending point of the gradient.
        /// </summary>
        public Point EndPoint { get; }

        /// <summary>
        /// Gets the starting point of the gradient.
        /// </summary>
        public Point StartPoint { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LinearGradientBrush"/> class.
        /// </summary>
        /// <param name="startPoint">The point where the gradient begins.</param>
        /// <param name="endPoint">The point where the gradient ends.</param>
        /// <param name="colors">The colors in the gradient.</param>
        /// <exception cref="ArgumentException">Thrown when zero colors have been specified.</exception>
        public LinearGradientBrush(Point startPoint, Point endPoint, params Color[] colors)
        {
            if (colors == null || colors.Length == 0)
            {
                throw new ArgumentException(Resources.Strings.OneColorMinimum, nameof(colors));
            }

            Colors = new ReadOnlyCollection<Color>(colors);
            StartPoint = startPoint;
            EndPoint = endPoint;
        }
    }
}
