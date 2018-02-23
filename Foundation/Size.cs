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
    /// Represents the width and height of an object.
    /// </summary>
    public struct Size
    {
        /// <summary>
        /// A <see cref="Size"/> instance with no width or height.  This field is read-only.
        /// </summary>
        public static readonly Size Empty = new Size();

        /// <summary>
        /// Gets or sets the height of this instance.
        /// </summary>
        public double Height { get; set; }

        /// <summary>
        /// Gets or sets the width of this instance.
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is empty.
        /// </summary>
        public bool IsEmpty
        {
            get { return Width == 0 && Height == 0; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Size"/> structure.
        /// </summary>
        /// <param name="width">The width of the structure.</param>
        /// <param name="height">The height of the structure.</param>
        public Size(double width, double height)
        {
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Determines whether the specified <see cref="Size"/> is equal to the current <see cref="Size"/>.
        /// </summary>
        /// <param name="other">The <see cref="Size"/> to compare with the current <see cref="Size"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="Size"/> is equal to the current
        /// <see cref="Size"/>; otherwise, <c>false</c>.</returns>
        public bool Equals(Size other)
        {
            return Width == other.Width && Height == other.Height;
        }

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to the current <see cref="Size"/>.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to compare with the current <see cref="Size"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="object"/> is equal to the current
        /// <see cref="Size"/>; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (obj is Size)
            {
                return Equals((Size)obj);
            }
            return false;
        }

        /// <summary>
        /// Serves as a hash function for a <see cref="Size"/> object.
        /// </summary>
        /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a
        /// hash table.</returns>
        public override int GetHashCode()
        {
            return Width.GetHashCode() ^ Height.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents the current <see cref="Size"/>.
        /// </summary>
        /// <returns>A <see cref="string"/> that represents the current <see cref="Size"/>.</returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, Resources.Strings.WidthHeight, Width, Height);
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents the current <see cref="Size"/>.
        /// </summary>
        /// <param name="format">A numeric format string.</param>
        /// <returns>A <see cref="string"/> that represents the current <see cref="Size"/>.</returns>
        public string ToString(string format)
        {
            return string.Format(CultureInfo.CurrentCulture, Resources.Strings.WidthHeight,
                Width.ToString(format, CultureInfo.CurrentCulture), Height.ToString(format, CultureInfo.CurrentCulture));
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents the current <see cref="Size"/>.
        /// </summary>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <returns>A <see cref="string"/> that represents the current <see cref="Size"/>.</returns>
        public string ToString(IFormatProvider provider)
        {
            return string.Format(provider, Resources.Strings.WidthHeight, Width.ToString(provider), Height.ToString(provider));
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents the current <see cref="Size"/>.
        /// </summary>
        /// <param name="format">A numeric format string.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <returns>A <see cref="string"/> that represents the current <see cref="Size"/>.</returns>
        public string ToString(string format, IFormatProvider provider)
        {
            return string.Format(provider, Resources.Strings.WidthHeight, Width.ToString(format, provider), Height.ToString(format, provider));
        }

        /// <summary>
        /// Adds the widths and heights of two <see cref="Size"/> objects together.
        /// </summary>
        /// <param name="size1">The size to add together with <paramref name="size2"/>.</param>
        /// <param name="size2">The size to add together with <paramref name="size1"/>.</param>
        /// <returns>A <see cref="Size"/> object containing the sum of both widths and the sum of both heights.</returns>
        public static Size Add(Size size1, Size size2)
        {
            return new Size(size1.Width + size2.Width, size1.Height + size2.Height);
        }

        /// <summary>
        /// Divides the width and height of a <see cref="Size"/> object by
        /// the width and height of another <see cref="Size"/> object.
        /// </summary>
        /// <param name="size1">The size from which <paramref name="size2"/> is divided.</param>
        /// <param name="size2">The size to divide from <paramref name="size1"/>.</param>
        /// <returns>A <see cref="Size"/> object containing the quotient of both widths and the quotient of both heights.</returns>
        public static Size Divide(Size size1, Size size2)
        {
            return new Size(size1.Width / size2.Width, size1.Height / size2.Height);
        }

        /// <summary>
        /// Divides the width and height of a <see cref="Size"/> object by a scalar value.
        /// </summary>
        /// <param name="value">The object whose width and height are to be scaled.</param>
        /// <param name="scalar">The amount to divide the width and height by.</param>
        /// <returns>A <see cref="Size"/> object containing the quotient of the width
        /// and scalar value and the quotient of the height and scalar value.</returns>
        public static Size Divide(Size value, double scalar)
        {
            return new Size(value.Width / scalar, value.Height / scalar);
        }

        /// <summary>
        /// Multiplies the widths and heights of two <see cref="Size"/> objects together.
        /// </summary>
        /// <param name="size1">The size to multiply with <paramref name="size2"/>.</param>
        /// <param name="size2">The size to multiply with <paramref name="size1"/>.</param>
        /// <returns>A <see cref="Size"/> object containing the product of both widths and the product of both heights.</returns>
        public static Size Multiply(Size size1, Size size2)
        {
            return new Size(size1.Width * size2.Width, size1.Height * size2.Height);
        }

        /// <summary>
        /// Multiplies the width and height of a <see cref="Size"/> object by a scalar value.
        /// </summary>
        /// <param name="value">The object whose width and height are to be scaled.</param>
        /// <param name="scalar">The amount to multiply the width and height by.</param>
        /// <returns>A <see cref="Size"/> object containing the product of the width
        /// and scalar value and the product of the height and scalar value.</returns>
        public static Size Multiply(Size value, double scalar)
        {
            return new Size(value.Width * scalar, value.Height * scalar);
        }

        /// <summary>
        /// Subtracts the width and height of a <see cref="Size"/> object
        /// by the width and height of another <see cref="Size"/> object.
        /// </summary>
        /// <param name="size1">The size from which <paramref name="size2"/> is subtracted.</param>
        /// <param name="size2">The size to subtract from <paramref name="size1"/>.</param>
        /// <returns>A <see cref="Size"/> object containing the difference of both widths and the difference of both heights.</returns>
        public static Size Subtract(Size size1, Size size2)
        {
            return new Size(size1.Width - size2.Width, size1.Height - size2.Height);
        }

        /// <summary>
        /// Determines whether two <see cref="Size"/> objects are considered equal.
        /// </summary>
        /// <param name="value1">The first object to compare.</param>
        /// <param name="value2">The second object to compare.</param>
        /// <returns><c>true</c> if the objects are considered equal; otherwise <c>false</c>.</returns>
        public static bool operator ==(Size value1, Size value2)
        {
            return value1.Equals(value2);
        }

        /// <summary>
        /// Determines whether two <see cref="Size"/> objects are not considered equal.
        /// </summary>
        /// <param name="value1">The first object to compare.</param>
        /// <param name="value2">The second object to compare.</param>
        /// <returns><c>true</c> if the objects are not considered equal; otherwise <c>false</c>.</returns>
        public static bool operator !=(Size value1, Size value2)
        {
            return !value1.Equals(value2);
        }

        /// <summary>
        /// Adds the widths and heights of two <see cref="Size"/> objects together.
        /// </summary>
        /// <param name="value1">The left argument.</param>
        /// <param name="value2">The right argument.</param>
        /// <returns>A <see cref="Size"/> object containing the sum of both widths and the sum of both heights.</returns>
        public static Size operator +(Size value1, Size value2)
        {
            return new Size(value1.Width + value2.Width, value1.Height + value2.Height);
        }

        /// <summary>
        /// Subtracts the width and height of a <see cref="Size"/> object
        /// by the width and height of another <see cref="Size"/> object.
        /// </summary>
        /// <param name="value1">The left argument.</param>
        /// <param name="value2">The right argument.</param>
        /// <returns>A <see cref="Size"/> object containing the difference of both widths and the difference of both heights.</returns>
        public static Size operator -(Size value1, Size value2)
        {
            return new Size(value1.Width - value2.Width, value1.Height - value2.Height);
        }

        /// <summary>
        /// Multiplies the widths and heights of two <see cref="Size"/> objects together.
        /// </summary>
        /// <param name="value1">The left argument.</param>
        /// <param name="value2">The right argument.</param>
        /// <returns>A <see cref="Size"/> object containing the product of both widths and the product of both heights.</returns>
        public static Size operator *(Size value1, Size value2)
        {
            return new Size(value1.Width * value2.Width, value1.Height * value2.Height);
        }

        /// <summary>
        /// Multiplies the width and height of a <see cref="Size"/> object by a scalar value.
        /// </summary>
        /// <param name="value">The object whose width and height are to be scaled.</param>
        /// <param name="scalar">The amount to multiply the width and height by.</param>
        /// <returns>A <see cref="Size"/> object containing the product of the width
        /// and scalar value and the product of the height and scalar value.</returns>
        public static Size operator *(Size value, double scalar)
        {
            return new Size(value.Width * scalar, value.Height * scalar);
        }

        /// <summary>
        /// Divides the width and height of a <see cref="Size"/> object by
        /// the width and height of another <see cref="Size"/> object.
        /// </summary>
        /// <param name="value1">The left argument.</param>
        /// <param name="value2">The right argument.</param>
        /// <returns>A <see cref="Size"/> object containing the quotient of both widths and the quotient of both heights.</returns>
        public static Size operator /(Size value1, Size value2)
        {
            return new Size(value1.Width / value2.Width, value1.Height / value2.Height);
        }

        /// <summary>
        /// Divides the width and height of a <see cref="Size"/> object by a scalar value.
        /// </summary>
        /// <param name="value">The object whose width and height are to be scaled.</param>
        /// <param name="scalar">The amount to divide the width and height by.</param>
        /// <returns>A <see cref="Size"/> object containing the quotient of the width
        /// and scalar value and the quotient of the height and scalar value.</returns>
        public static Size operator /(Size value, double scalar)
        {
            return new Size(value.Width / scalar, value.Height / scalar);
        }
    }
}
