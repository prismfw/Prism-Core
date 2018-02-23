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
    /// Represents a vertical partition of space in a <see cref="Grid"/>.
    /// </summary>
    public class RowDefinition
    {
        /// <summary>
        /// Gets the actual calculated height of the row.
        /// </summary>
        public double ActualHeight { get; internal set; }

        /// <summary>
        /// Gets or sets the maximum height of the row.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", Justification = "Exception parameter refers to property name for easier understanding of invalid value.")]
        public double MaxHeight
        {
            get { return maxHeight; }
            set
            {
                if (value != maxHeight)
                {
                    if (double.IsNaN(value) || double.IsNegativeInfinity(value))
                    {
                        throw new ArgumentException(Resources.Strings.ValueCannotBeNaNOrNegativeInfinity, nameof(MaxHeight));
                    }

                    if (value < 0)
                    {
                        throw new ArgumentOutOfRangeException(nameof(MaxHeight), Resources.Strings.ValueCannotBeLessThanZero);
                    }

                    maxHeight = value;
                }
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private double maxHeight;

        /// <summary>
        /// Gets or sets the minimum height of the row.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", Justification = "Exception parameter refers to property name for easier understanding of invalid value.")]
        public double MinHeight
        {
            get { return minHeight; }
            set
            {
                if (value != minHeight)
                {
                    if (double.IsNaN(value) || double.IsInfinity(value))
                    {
                        throw new ArgumentException(Resources.Strings.ValueCannotBeNaNOrInfinity, nameof(MinHeight));
                    }

                    if (value < 0)
                    {
                        throw new ArgumentOutOfRangeException(nameof(MinHeight), Resources.Strings.ValueCannotBeLessThanZero);
                    }

                    minHeight = value;
                }
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private double minHeight;

        /// <summary>
        /// Gets the offset of the row.
        /// </summary>
        public double Offset { get; internal set; }

        /// <summary>
        /// Gets or sets the height of the row.
        /// </summary>
        public GridLength Height { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RowDefinition"/> class.
        /// </summary>
        public RowDefinition()
        {
            MaxHeight = double.PositiveInfinity;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RowDefinition"/> class.
        /// </summary>
        /// <param name="height">The height of the row.</param>
        public RowDefinition(GridLength height)
        {
            Height = height;

            MaxHeight = double.PositiveInfinity;
        }
    }
}
