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
using System.Globalization;

namespace Prism
{
    /// <summary>
    /// Represents the thickness of a frame around a rectangle.
    /// </summary>
    public struct Thickness
    {
        /// <summary>
        /// Gets or sets the thickness of the lower side of the rectangle.
        /// </summary>
        public double Bottom { get; set; }

        /// <summary>
        /// Gets or sets the thickness of the left side of the rectangle.
        /// </summary>
        public double Left { get; set; }

        /// <summary>
        /// Gets or sets the thickness of the right side of the rectangle.
        /// </summary>
        public double Right { get; set; }

        /// <summary>
        /// Gets or sets the thickness of the upper side of the rectangle.
        /// </summary>
        public double Top { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Thickness"/> structure.
        /// </summary>
        /// <param name="uniformLength">The uniform length of all four sides of the rectangle.</param>
        public Thickness(double uniformLength)
        {
            Left = Top = Right = Bottom = uniformLength;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Thickness"/> structure.
        /// </summary>
        /// <param name="horizontalLength">The length of the left and right edges of the rectangle.</param>
        /// <param name="verticalLength">The length of the upper and lower edges of the rectangle.</param>
        public Thickness(double horizontalLength, double verticalLength)
        {
            Left = Right = horizontalLength;
            Top = Bottom = verticalLength;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Thickness"/> structure.
        /// </summary>
        /// <param name="left">The thickness of the left side of the rectangle.</param>
        /// <param name="top">The thickness of the upper side of the rectangle.</param>
        /// <param name="right">The thickness of the right side of the rectangle.</param>
        /// <param name="bottom">The thickness of the lower side of the rectangle.</param>
        public Thickness(double left, double top, double right, double bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        /// <summary>
        /// Determines whether the specified <see cref="Thickness"/> is equal to the current <see cref="Thickness"/>.
        /// </summary>
        /// <param name="other">The <see cref="Thickness"/> to compare with the current <see cref="Thickness"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="Thickness"/> is equal to the current
        /// <see cref="Thickness"/>; otherwise, <c>false</c>.</returns>
        public bool Equals(Thickness other)
        {
            return Left == other.Left && Top == other.Top && Right == other.Right && Bottom == other.Bottom;
        }

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to the current <see cref="Thickness"/>.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to compare with the current <see cref="Thickness"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="object"/> is equal to the current
        /// <see cref="Thickness"/>; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (obj is Thickness)
            {
                return Equals((Thickness)obj);
            }
            return false;
        }

        /// <summary>
        /// Serves as a hash function for a <see cref="Thickness"/> object.
        /// </summary>
        /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a
        /// hash table.</returns>
        public override int GetHashCode()
        {
            return Left.GetHashCode() ^ Top.GetHashCode() ^ Right.GetHashCode() ^ Bottom.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents the current <see cref="Thickness"/>.
        /// </summary>
        /// <returns>A <see cref="string"/> that represents the current <see cref="Thickness"/>.</returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, Resources.Strings.LeftTopRightBottom, Left, Top, Right, Bottom);
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents the current <see cref="Thickness"/>.
        /// </summary>
        /// <param name="format">A numeric format string.</param>
        /// <returns>A <see cref="string"/> that represents the current <see cref="Thickness"/>.</returns>
        public string ToString(string format)
        {
            return string.Format(CultureInfo.CurrentCulture, Resources.Strings.LeftTopRightBottom,
                Left.ToString(format, CultureInfo.CurrentCulture), Top.ToString(format, CultureInfo.CurrentCulture),
                Right.ToString(format, CultureInfo.CurrentCulture), Bottom.ToString(format, CultureInfo.CurrentCulture));
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents the current <see cref="Thickness"/>.
        /// </summary>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <returns>A <see cref="string"/> that represents the current <see cref="Thickness"/>.</returns>
        public string ToString(IFormatProvider provider)
        {
            return string.Format(provider, Resources.Strings.LeftTopRightBottom,
                Left.ToString(provider), Top.ToString(provider), Right.ToString(provider), Bottom.ToString(provider));
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents the current <see cref="Thickness"/>.
        /// </summary>
        /// <param name="format">A numeric format string.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <returns>A <see cref="string"/> that represents the current <see cref="Thickness"/>.</returns>
        public string ToString(string format, IFormatProvider provider)
        {
            return string.Format(provider, Resources.Strings.LeftTopRightBottom, Left.ToString(format, provider),
                Top.ToString(format, provider), Right.ToString(format, provider), Bottom.ToString(format, provider));
        }

        /// <summary>
        /// Adds the specified amount to each side of a <see cref="Thickness"/> object.
        /// </summary>
        /// <param name="value">The object whose sides are to be incremented.</param>
        /// <param name="amount">The amount to add to each side.</param>
        /// <returns>A <see cref="Thickness"/> object containing the sum of each side and the amount.</returns>
        public static Thickness Add(Thickness value, Thickness amount)
        {
            return new Thickness(value.Left + amount.Left, value.Top + amount.Top, value.Right + amount.Right, value.Bottom + amount.Bottom);
        }

        /// <summary>
        /// Adds the specified amount to each side of a <see cref="Thickness"/> object.
        /// </summary>
        /// <param name="value">The object whose side are to be incremented.</param>
        /// <param name="amount">The amount to add to each side.</param>
        /// <returns>A <see cref="Thickness"/> object containing the sum of each side and the amount.</returns>
        public static Thickness Add(Thickness value, double amount)
        {
            return new Thickness(value.Left + amount, value.Top + amount, value.Right + amount, value.Bottom + amount);
        }

        /// <summary>
        /// Divides each side of a <see cref="Thickness"/> object by the specified amount.
        /// </summary>
        /// <param name="value">The object whose sides are to be scaled.</param>
        /// <param name="amount">The amount by which to divide each side.</param>
        /// <returns>A <see cref="Thickness"/> object containing the quotient of each side and the amount.</returns>
        public static Thickness Divide(Thickness value, Thickness amount)
        {
            return new Thickness(value.Left / amount.Left, value.Top / amount.Top, value.Right / amount.Right, value.Bottom / amount.Bottom);
        }

        /// <summary>
        /// Divides each side of a <see cref="Thickness"/> object by a scalar value.
        /// </summary>
        /// <param name="value">The object whose sides are to be scaled.</param>
        /// <param name="scalar">The amount by which to divide each side.</param>
        /// <returns>A <see cref="Thickness"/> object containing the quotient of each side and the scalar value.</returns>
        public static Thickness Divide(Thickness value, double scalar)
        {
            return new Thickness(value.Left / scalar, value.Top / scalar, value.Right / scalar, value.Bottom / scalar);
        }

        /// <summary>
        /// Multiplies each side of a <see cref="Thickness"/> object by the specified amount.
        /// </summary>
        /// <param name="value">The object whose sides are to be scaled.</param>
        /// <param name="amount">The amount by which to multiply each side.</param>
        /// <returns>A <see cref="Thickness"/> object containing the product of each side and the amount.</returns>
        public static Thickness Multiply(Thickness value, Thickness amount)
        {
            return new Thickness(value.Left * amount.Left, value.Top * amount.Top, value.Right * amount.Right, value.Bottom * amount.Bottom);
        }

        /// <summary>
        /// Multiplies each side of a <see cref="Thickness"/> object by a scalar value.
        /// </summary>
        /// <param name="value">The object whose sides are to be scaled.</param>
        /// <param name="scalar">The amount by which to multiply each side.</param>
        /// <returns>A <see cref="Thickness"/> object containing the product of each side and the scalar value.</returns>
        public static Thickness Multiply(Thickness value, double scalar)
        {
            return new Thickness(value.Left * scalar, value.Top * scalar, value.Right * scalar, value.Bottom * scalar);
        }

        /// <summary>
        /// Negates the values of a <see cref="Thickness"/> object.
        /// </summary>
        /// <param name="value">The object whose values are to be negated.</param>
        /// <returns>A <see cref="Thickness"/> object containing the negated values.</returns>
        public static Thickness Negate(Thickness value)
        {
            return new Thickness(-value.Left, -value.Top, -value.Right, -value.Bottom);
        }

        /// <summary>
        /// Subtracts each side of a <see cref="Thickness"/> object by the specified amount.
        /// </summary>
        /// <param name="value">The object whose sides are to be decremented.</param>
        /// <param name="amount">The amount to subtract from each side.</param>
        /// <returns>A <see cref="Thickness"/> object containing the difference of each side and the amount.</returns>
        public static Thickness Subtract(Thickness value, Thickness amount)
        {
            return new Thickness(value.Left - amount.Left, value.Top - amount.Top, value.Right - amount.Right, value.Bottom - amount.Bottom);
        }

        /// <summary>
        /// Subtracts each side of a <see cref="Thickness"/> object by the specified amount.
        /// </summary>
        /// <param name="value">The object whose sides are to be decremented.</param>
        /// <param name="amount">The amount to subtract from each side.</param>
        /// <returns>A <see cref="Thickness"/> object containing the difference of each side and the amount.</returns>
        public static Thickness Subtract(Thickness value, double amount)
        {
            return new Thickness(value.Left - amount, value.Top - amount, value.Right - amount, value.Bottom - amount);
        }

        /// <summary>
        /// Determines whether two <see cref="Thickness"/> objects are considered equal.
        /// </summary>
        /// <param name="value1">The first object to compare.</param>
        /// <param name="value2">The second object to compare.</param>
        /// <returns><c>true</c> if the objects are considered equal; otherwise <c>false</c>.</returns>
        public static bool operator ==(Thickness value1, Thickness value2)
        {
            return value1.Equals(value2);
        }

        /// <summary>
        /// Determines whether two <see cref="Thickness"/> objects are not considered equal.
        /// </summary>
        /// <param name="value1">The first object to compare.</param>
        /// <param name="value2">The second object to compare.</param>
        /// <returns><c>true</c> if the objects are not considered equal; otherwise <c>false</c>.</returns>
        public static bool operator !=(Thickness value1, Thickness value2)
        {
            return !value1.Equals(value2);
        }

        /// <summary>
        /// Adds the specified amount to each side of a <see cref="Thickness"/> object.
        /// </summary>
        /// <param name="value">The object whose sides are to be incremented.</param>
        /// <param name="amount">The amount to add to each side.</param>
        /// <returns>A <see cref="Thickness"/> object containing the sum of each side and the amount.</returns>
        public static Thickness operator +(Thickness value, Thickness amount)
        {
            return new Thickness(value.Left + amount.Left, value.Top + amount.Top, value.Right + amount.Right, value.Bottom + amount.Bottom);
        }

        /// <summary>
        /// Adds the specified amount to each side of a <see cref="Thickness"/> object.
        /// </summary>
        /// <param name="value">The object whose side are to be incremented.</param>
        /// <param name="amount">The amount to add to each side.</param>
        /// <returns>A <see cref="Thickness"/> object containing the sum of each side and the amount.</returns>
        public static Thickness operator +(Thickness value, double amount)
        {
            return new Thickness(value.Left + amount, value.Top + amount, value.Right + amount, value.Bottom + amount);
        }

        /// <summary>
        /// Negates the values of a <see cref="Thickness"/> object.
        /// </summary>
        /// <param name="value">The object whose values are to be negated.</param>
        /// <returns>A <see cref="Thickness"/> object containing the negated values.</returns>
        public static Thickness operator -(Thickness value)
        {
            return new Thickness(-value.Left, -value.Top, -value.Right, -value.Bottom);
        }

        /// <summary>
        /// Subtracts each side of a <see cref="Thickness"/> object by the specified amount.
        /// </summary>
        /// <param name="value">The object whose sides are to be decremented.</param>
        /// <param name="amount">The amount to subtract from each side.</param>
        /// <returns>A <see cref="Thickness"/> object containing the difference of each side and the amount.</returns>
        public static Thickness operator -(Thickness value, Thickness amount)
        {
            return new Thickness(value.Left - amount.Left, value.Top - amount.Top, value.Right - amount.Right, value.Bottom - amount.Bottom);
        }

        /// <summary>
        /// Subtracts each side of a <see cref="Thickness"/> object by the specified amount.
        /// </summary>
        /// <param name="value">The object whose sides are to be decremented.</param>
        /// <param name="amount">The amount to subtract from each side.</param>
        /// <returns>A <see cref="Thickness"/> object containing the difference of each side and the amount.</returns>
        public static Thickness operator -(Thickness value, double amount)
        {
            return new Thickness(value.Left - amount, value.Top - amount, value.Right - amount, value.Bottom - amount);
        }

        /// <summary>
        /// Multiplies each side of a <see cref="Thickness"/> object by the specified amount.
        /// </summary>
        /// <param name="value">The object whose sides are to be scaled.</param>
        /// <param name="amount">The amount by which to multiply each side.</param>
        /// <returns>A <see cref="Thickness"/> object containing the product of each side and the amount.</returns>
        public static Thickness operator *(Thickness value, Thickness amount)
        {
            return new Thickness(value.Left * amount.Left, value.Top * amount.Top, value.Right * amount.Right, value.Bottom * amount.Bottom);
        }

        /// <summary>
        /// Multiplies each side of a <see cref="Thickness"/> object by a scalar value.
        /// </summary>
        /// <param name="value">The object whose sides are to be scaled.</param>
        /// <param name="scalar">The amount by which to multiply each side.</param>
        /// <returns>A <see cref="Thickness"/> object containing the product of each side and the scalar value.</returns>
        public static Thickness operator *(Thickness value, double scalar)
        {
            return new Thickness(value.Left * scalar, value.Top * scalar, value.Right * scalar, value.Bottom * scalar);
        }

        /// <summary>
        /// Divides each side of a <see cref="Thickness"/> object by the specified amount.
        /// </summary>
        /// <param name="value">The object whose sides are to be scaled.</param>
        /// <param name="amount">The amount by which to divide each side.</param>
        /// <returns>A <see cref="Thickness"/> object containing the quotient of each side and the amount.</returns>
        public static Thickness operator /(Thickness value, Thickness amount)
        {
            return new Thickness(value.Left / amount.Left, value.Top / amount.Top, value.Right / amount.Right, value.Bottom / amount.Bottom);
        }

        /// <summary>
        /// Divides each side of a <see cref="Thickness"/> object by a scalar value.
        /// </summary>
        /// <param name="value">The object whose sides are to be scaled.</param>
        /// <param name="scalar">The amount by which to divide each side.</param>
        /// <returns>A <see cref="Thickness"/> object containing the quotient of each side and the scalar value.</returns>
        public static Thickness operator /(Thickness value, double scalar)
        {
            return new Thickness(value.Left / scalar, value.Top / scalar, value.Right / scalar, value.Bottom / scalar);
        }
    }
}
