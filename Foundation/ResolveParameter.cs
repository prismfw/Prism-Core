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
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Prism
{
    /// <summary>
    /// Represents a parameter that is to be passed to the IoC container during resolution of a native type for a <see cref="FrameworkObject"/>.
    /// </summary>
    public struct ResolveParameter
    {
        /// <summary>
        /// Gets an empty array for passing to a <see cref="FrameworkObject"/> class constructor
        /// when a native object needs to be resolved but no parameters need to be specified.
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Justification = "Array is empty and cannot be written to.")]
        public static ResolveParameter[] EmptyParameters { get; } = new ResolveParameter[0];

        /// <summary>
        /// Gets a value indicating whether <c>null</c> is a valid value for the parameter.
        /// </summary>
        public bool AllowNull { get; }

        /// <summary>
        /// Gets the name of the parameter.
        /// </summary>
        public string ParameterName { get; }

        /// <summary>
        /// Gets the value of the parameter.
        /// </summary>
        public object ParameterValue { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResolveParameter"/> structure.
        /// </summary>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="parameterValue">The value of the parameter.  A value of <c>null</c> will generate an exception during resolution.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameterName"/> is <c>null</c>.</exception>
        public ResolveParameter(string parameterName, object parameterValue)
        {
            if (parameterName == null)
            {
                throw new ArgumentNullException(nameof(parameterName));
            }

            AllowNull = false;
            ParameterName = parameterName;
            ParameterValue = parameterValue;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResolveParameter"/> structure.
        /// </summary>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="parameterValue">The value of the parameter.</param>
        /// <param name="allowNull">Whether <c>null</c> should be considered a valid parameter value.  If <c>false</c> and <paramref name="parameterValue"/> is <c>null</c>, an exception will be thrown during resolution.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameterName"/> is <c>null</c>.</exception>
        public ResolveParameter(string parameterName, object parameterValue, bool allowNull)
        {
            if (parameterName == null)
            {
                throw new ArgumentNullException(nameof(parameterName));
            }

            AllowNull = allowNull;
            ParameterName = parameterName;
            ParameterValue = parameterValue;
        }

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to the current <see cref="ResolveParameter"/>.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to compare with the current <see cref="ResolveParameter"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="object"/> is equal to the current <see cref="ResolveParameter"/>; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (obj is ResolveParameter)
            {
                var other = (ResolveParameter)obj;
                return ParameterName == other.ParameterName && ParameterValue == other.ParameterValue && AllowNull == other.AllowNull;
            }
            return false;
        }

        /// <summary>
        /// Serves as a hash function for a <see cref="ResolveParameter"/> object.
        /// </summary>
        /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a hash table.</returns>
        public override int GetHashCode()
        {
            return ParameterName.GetHashCode() ^ (ParameterValue == null ? 0 : ParameterValue.GetHashCode()) ^ AllowNull.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents the current <see cref="ResolveParameter"/>.
        /// </summary>
        /// <returns>A <see cref="string"/> that represents the current <see cref="ResolveParameter"/>.</returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, Resources.Strings.NameValue, ParameterName, ParameterValue ?? "<null>");
        }

        /// <summary>
        /// Determines whether two <see cref="ResolveParameter"/> objects are considered equal.
        /// </summary>
        /// <param name="value1">The first object to compare.</param>
        /// <param name="value2">The second object to compare.</param>
        /// <returns><c>true</c> if the objects are considered equal; otherwise <c>false</c>.</returns>
        public static bool operator ==(ResolveParameter value1, ResolveParameter value2)
        {
            return value1.Equals(value2);
        }

        /// <summary>
        /// Determines whether two <see cref="ResolveParameter"/> objects are not considered equal.
        /// </summary>
        /// <param name="value1">The first object to compare.</param>
        /// <param name="value2">The second object to compare.</param>
        /// <returns><c>true</c> if the objects are not considered equal; otherwise <c>false</c>.</returns>
        public static bool operator !=(ResolveParameter value1, ResolveParameter value2)
        {
            return !value1.Equals(value2);
        }
    }
}
