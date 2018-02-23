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
using System.Globalization;

namespace Prism
{
    /// <summary>
    /// Represents an X- and Y-coordinate pair in two-dimensional space.
    /// </summary>
    public struct Point
    {
        /// <summary>
        /// Gets or sets the X-coordinate of this instance.
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Gets or sets the Y-coordinate of this instance.
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Point"/> structure.
        /// </summary>
        /// <param name="x">The X coordinate of the point.</param>
        /// <param name="y">The Y coordinate of the point.</param>
        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Adds the Xs and Ys of two <see cref="Point"/> objects together.
        /// </summary>
        /// <param name="point1">The point to add together with <paramref name="point2"/>.</param>
        /// <param name="point2">The point to add together with <paramref name="point1"/>.</param>
        /// <returns>A <see cref="Point"/> object containing the sum of both Xs and the sum of both Ys.</returns>
        public static Point Add(Point point1, Point point2)
        {
            return new Point(point1.X + point2.X, point1.Y + point2.Y);
        }

        /// <summary>
        /// Divides the X and Y of a <see cref="Point"/> object by the X and Y of another <see cref="Point"/> object.
        /// </summary>
        /// <param name="point1">The point from which <paramref name="point2"/> is divided.</param>
        /// <param name="point2">The point to divide from <paramref name="point1"/>.</param>
        /// <returns>A <see cref="Point"/> object containing the quotient of both Xs and the quotient of both Ys.</returns>
        public static Point Divide(Point point1, Point point2)
        {
            return new Point(point1.X / point2.X, point1.Y / point2.Y);
        }

        /// <summary>
        /// Multiplies the Xs and Ys of two <see cref="Point"/> objects together.
        /// </summary>
        /// <param name="point1">The point to multiply with <paramref name="point2"/>.</param>
        /// <param name="point2">The point to multiply with <paramref name="point1"/>.</param>
        /// <returns>A <see cref="Point"/> object containing the product of both Xs and the product of both Ys.</returns>
        public static Point Multiply(Point point1, Point point2)
        {
            return new Point(point1.X * point2.X, point1.Y * point2.Y);
        }

        /// <summary>
        /// Subtracts the X and Y of a <see cref="Point"/> object from the X and Y of another <see cref="Point"/> object.
        /// </summary>
        /// <param name="point1">The point from which <paramref name="point2"/> is subtracted.</param>
        /// <param name="point2">The point to subtract from <paramref name="point1"/>.</param>
        /// <returns>A <see cref="Point"/> object containing the difference of both Xs and the difference of both Ys.</returns>
        public static Point Subtract(Point point1, Point point2)
        {
            return new Point(point1.X - point2.X, point1.Y - point2.Y);
        }

        /// <summary>
        /// Determines whether the specified <see cref="Point"/> is equal to the current <see cref="Point"/>.
        /// </summary>
        /// <param name="other">The <see cref="Point"/> to compare with the current <see cref="Point"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="Point"/> is equal to the current
        /// <see cref="Point"/>; otherwise, <c>false</c>.</returns>
        public bool Equals(Point other)
        {
            return X == other.X && Y == other.Y;
        }

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to the current <see cref="Point"/>.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to compare with the current <see cref="Point"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="object"/> is equal to the current
        /// <see cref="Point"/>; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (obj is Point)
            {
                return Equals((Point)obj);
            }
            return false;
        }

        /// <summary>
        /// Serves as a hash function for a <see cref="Point"/> object.
        /// </summary>
        /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a
        /// hash table.</returns>
        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents the current <see cref="Point"/>.
        /// </summary>
        /// <returns>A <see cref="string"/> that represents the current <see cref="Point"/>.</returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, Resources.Strings.XY, X, Y);
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents the current <see cref="Point"/>.
        /// </summary>
        /// <param name="format">A numeric format string.</param>
        /// <returns>A <see cref="string"/> that represents the current <see cref="Point"/>.</returns>
        public string ToString(string format)
        {
            return string.Format(CultureInfo.CurrentCulture, Resources.Strings.XY,
                X.ToString(format, CultureInfo.CurrentCulture), Y.ToString(format, CultureInfo.CurrentCulture));
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents the current <see cref="Point"/>.
        /// </summary>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <returns>A <see cref="string"/> that represents the current <see cref="Point"/>.</returns>
        public string ToString(IFormatProvider provider)
        {
            return string.Format(provider, Resources.Strings.XY, X.ToString(provider), Y.ToString(provider));
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents the current <see cref="Point"/>.
        /// </summary>
        /// <param name="format">A numeric format string.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <returns>A <see cref="string"/> that represents the current <see cref="Point"/>.</returns>
        public string ToString(string format, IFormatProvider provider)
        {
            return string.Format(provider, Resources.Strings.XY, X.ToString(format, provider), Y.ToString(format, provider));
        }

        /// <summary>
        /// Determines whether two <see cref="Point"/> objects are considered equal.
        /// </summary>
        /// <param name="value1">The first object to compare.</param>
        /// <param name="value2">The second object to compare.</param>
        /// <returns><c>true</c> if the objects are considered equal; otherwise <c>false</c>.</returns>
        public static bool operator ==(Point value1, Point value2)
        {
            return value1.Equals(value2);
        }

        /// <summary>
        /// Determines whether two <see cref="Point"/> objects are not considered equal.
        /// </summary>
        /// <param name="value1">The first object to compare.</param>
        /// <param name="value2">The second object to compare.</param>
        /// <returns><c>true</c> if the objects are not considered equal; otherwise <c>false</c>.</returns>
        public static bool operator !=(Point value1, Point value2)
        {
            return !value1.Equals(value2);
        }

        /// <summary>
        /// Adds the Xs and Ys of two <see cref="Point"/> objects together.
        /// </summary>
        /// <param name="value1">The left argument.</param>
        /// <param name="value2">The right argument.</param>
        /// <returns>A <see cref="Point"/> object containing the sum of both Xs and the sum of both Ys.</returns>
        public static Point operator +(Point value1, Point value2)
        {
            return Add(value1, value2);
        }

        /// <summary>
        /// Subtracts the X and Y of a <see cref="Point"/> object from the X and Y of another <see cref="Point"/> object.
        /// </summary>
        /// <param name="value1">The left argument.</param>
        /// <param name="value2">The right argument.</param>
        /// <returns>A <see cref="Point"/> object containing the difference of both Xs and the difference of both Ys.</returns>
        public static Point operator -(Point value1, Point value2)
        {
            return Subtract(value1, value2);
        }

        /// <summary>
        /// Multiplies the Xs and Ys of two <see cref="Point"/> objects together.
        /// </summary>
        /// <param name="value1">The left argument.</param>
        /// <param name="value2">The right argument.</param>
        /// <returns>A <see cref="Point"/> object containing the product of both Xs and the product of both Ys.</returns>
        public static Point operator *(Point value1, Point value2)
        {
            return new Point(value1.X * value2.X, value1.Y * value2.Y);
        }

        /// <summary>
        /// Divides the X and Y of a <see cref="Point"/> object by the X and Y of another <see cref="Point"/> object.
        /// </summary>
        /// <param name="value1">The left argument.</param>
        /// <param name="value2">The right argument.</param>
        /// <returns>A <see cref="Point"/> object containing the quotient of both Xs and the quotient of both Ys.</returns>
        public static Point operator /(Point value1, Point value2)
        {
            return new Point(value1.X / value2.X, value1.Y / value2.Y);
        }
    }
}
