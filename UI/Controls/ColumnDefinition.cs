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
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Prism.UI.Controls
{
    /// <summary>
    /// Represents a horizontal partition of space in a <see cref="Grid"/>.
    /// </summary>
    public class ColumnDefinition
    {
        /// <summary>
        /// Gets the actual calculated width of the column.
        /// </summary>
        public double ActualWidth { get; internal set; }

        /// <summary>
        /// Gets or sets the maximum width of the column.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", Justification = "Exception parameter refers to property name for easier understanding of invalid value.")]
        public double MaxWidth
        {
            get { return maxWidth; }
            set
            {
                if (value != maxWidth)
                {
                    if (double.IsNaN(value) || double.IsNegativeInfinity(value))
                    {
                        throw new ArgumentException(Resources.Strings.ValueCannotBeNaNOrNegativeInfinity, nameof(MaxWidth));
                    }

                    if (value < 0)
                    {
                        throw new ArgumentOutOfRangeException(nameof(MaxWidth), Resources.Strings.ValueCannotBeLessThanZero);
                    }

                    maxWidth = value;
                }
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private double maxWidth;

        /// <summary>
        /// Gets or sets the minimum width of the column.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", Justification = "Exception parameter refers to property name for easier understanding of invalid value.")]
        public double MinWidth
        {
            get { return minWidth; }
            set
            {
                if (value != minWidth)
                {
                    if (double.IsNaN(value) || double.IsInfinity(value))
                    {
                        throw new ArgumentException(Resources.Strings.ValueCannotBeNaNOrInfinity, nameof(MinWidth));
                    }

                    if (value < 0)
                    {
                        throw new ArgumentOutOfRangeException(nameof(MinWidth), Resources.Strings.ValueCannotBeLessThanZero);
                    }

                    minWidth = value;
                }
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private double minWidth;

        /// <summary>
        /// Gets the offset of the column.
        /// </summary>
        public double Offset { get; internal set; }

        /// <summary>
        /// Gets or sets the width of the column.
        /// </summary>
        public GridLength Width { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnDefinition"/> class.
        /// </summary>
        public ColumnDefinition()
        {
            MaxWidth = double.PositiveInfinity;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnDefinition"/> class.
        /// </summary>
        /// <param name="width">The width of the column.</param>
        public ColumnDefinition(GridLength width)
        {
            Width = width;

            MaxWidth = double.PositiveInfinity;
        }
    }
}
