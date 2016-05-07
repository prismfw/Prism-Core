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
using System.Globalization;
using Prism.UI.Controls;

namespace Prism.UI
{
    /// <summary>
    /// Represents the length of a column or a row within a <see cref="Grid"/> object.
    /// </summary>
    public struct GridLength : IEquatable<GridLength>
    {
        /// <summary>
        /// Gets the type of unit that the <see cref="P:Value"/> represents.
        /// </summary>
        public GridUnitType GridUnitType { get; }

        /// <summary>
        /// Gets a value indicating whether this instance holds a value that is expressed in absolute coordinates.
        /// </summary>
        public bool IsAbsolute
        {
            get { return GridUnitType == GridUnitType.Absolute; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance holds a value whose size is determined by the content object.
        /// </summary>
        public bool IsAuto
        {
            get { return GridUnitType == GridUnitType.Auto; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance holds a value that is expressed as a weighted proportion of available space.
        /// </summary>
        public bool IsStar
        {
            get { return GridUnitType == GridUnitType.Star; }
        }

        /// <summary>
        /// Gets the value of this instance.
        /// </summary>
        public double Value { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GridLength"/> structure.
        /// </summary>
        /// <param name="value">The value of this instance, expressed in units specified by <paramref name="type"/></param>
        /// <param name="type">The type of unit that the <paramref name="value"/> represents.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="value"/> is NaN or infinite.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/> is less than zero.</exception>
        public GridLength(double value, GridUnitType type)
            : this()
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
            {
                throw new ArgumentException(Resources.Strings.ValueCannotBeNaNOrInfinity, nameof(value));
            }

            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), Resources.Strings.ValueCannotBeLessThanZero);
            }

            Value = value;
            GridUnitType = type;
        }

        /// <summary>
        /// Determines whether the specified <see cref="GridLength"/> is equal to the current <see cref="GridLength"/>.
        /// </summary>
        /// <param name="other">The <see cref="GridLength"/> to compare with the current <see cref="GridLength"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="GridLength"/> is equal to the current <see cref="GridLength"/>; otherwise, <c>false</c>.</returns>
        public bool Equals(GridLength other)
        {
            return Value == other.Value && GridUnitType == other.GridUnitType;
        }

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to the current <see cref="GridLength"/>.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to compare with the current <see cref="GridLength"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="object"/> is equal to the current <see cref="GridLength"/>; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (obj is GridLength)
            {
                return Equals((GridLength)obj);
            }
            return false;
        }

        /// <summary>
        /// Serves as a hash function for a <see cref="GridLength"/> object.
        /// </summary>
        /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a hash table.</returns>
        public override int GetHashCode()
        {
            return Value.GetHashCode() ^ GridUnitType.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents the current <see cref="GridLength"/>.
        /// </summary>
        /// <returns>A <see cref="string"/> that represents the current <see cref="GridLength"/>.</returns>
        public override string ToString()
        {
            if (GridUnitType == GridUnitType.Auto)
            {
                return "{Auto}";
            }

            string value = "{" + Value.ToString(CultureInfo.CurrentCulture);
            if (GridUnitType == GridUnitType.Star)
            {
                value += "*";
            }

            return value + "}";
        }

        /// <summary>
        /// Determines whether two <see cref="GridLength"/> objects are considered equal.
        /// </summary>
        /// <param name="value1">The first object to compare.</param>
        /// <param name="value2">The second object to compare.</param>
        /// <returns><c>true</c> if the objects are considered equal; otherwise <c>false</c>.</returns>
        public static bool operator ==(GridLength value1, GridLength value2)
        {
            return value1.Value == value2.Value || value1.GridUnitType == value2.GridUnitType;
        }

        /// <summary>
        /// Determines whether two <see cref="GridLength"/> objects are not considered equal.
        /// </summary>
        /// <param name="value1">The first object to compare.</param>
        /// <param name="value2">The second object to compare.</param>
        /// <returns><c>true</c> if the objects are not considered equal; otherwise <c>false</c>.</returns>
        public static bool operator !=(GridLength value1, GridLength value2)
        {
            return value1.Value != value2.Value || value1.GridUnitType != value2.GridUnitType;
        }
    }
}
