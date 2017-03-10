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
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Prism.UI
{
    /// <summary>
    /// Represents a 32-bit ARGB color.
    /// </summary>
    public struct Color
    {
        /// <summary>
        /// Gets or sets the alpha component of the color.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Justification = "Commonly used abbreviation when concerning color channels.")]
        public byte A { get; set; }

        /// <summary>
        /// Gets or sets the red component of the color.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Justification = "Commonly used abbreviation when concerning color channels.")]
        public byte R { get; set; }

        /// <summary>
        /// Gets or sets the green component of the color.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Justification = "Commonly used abbreviation when concerning color channels.")]
        public byte G { get; set; }

        /// <summary>
        /// Gets or sets the blue component of the color.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Justification = "Commonly used abbreviation when concerning color channels.")]
        public byte B { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Color"/> structure.
        /// </summary>
        /// <param name="red">The red component value.</param>
        /// <param name="green">The green component value.</param>
        /// <param name="blue">The blue component value.</param>
        public Color(byte red, byte green, byte blue)
        {
            A = byte.MaxValue;
            R = red;
            G = green;
            B = blue;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Color"/> structure.
        /// </summary>
        /// <param name="alpha">The alpha component value.</param>
        /// <param name="red">The red component value.</param>
        /// <param name="green">The green component value.</param>
        /// <param name="blue">The blue component value.</param>
        public Color(byte alpha, byte red, byte green, byte blue)
        {
            A = alpha;
            R = red;
            G = green;
            B = blue;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Color"/> structure.
        /// </summary>
        /// <param name="argb">The ARGB value in a single 32-bit integer.</param>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Justification = "Commonly used acronym when concerning colors.")]
        public Color(int argb)
        {
            A = (byte)(argb >> 24 & 0xFF);
            R = (byte)(argb >> 16 & 0xFF);
            G = (byte)(argb >> 8 & 0xFF);
            B = (byte)(argb & 0xFF);
        }

        /// <summary>
        /// Adds two colors together and returns the result.
        /// </summary>
        /// <param name="color1">The first color to add.</param>
        /// <param name="color2">The second color to add.</param>
        /// <returns>The result of the addition as a <see cref="Color"/>.</returns>
        public static Color Add(Color color1, Color color2)
        {
            return new Color((byte)Math.Min(255, color1.A + color2.A), (byte)Math.Min(255, color1.R + color2.R),
                (byte)Math.Min(255, color1.G + color2.G), (byte)Math.Min(255, color1.B + color2.B));
        }

        /// <summary>
        /// Performs a linear interpolation of two colors and returns the result.
        /// </summary>
        /// <param name="color1">The starting color.</param>
        /// <param name="color2">The ending color.</param>
        /// <param name="amount">A value between 0 and 1 indicating the weight of <paramref name="color2"/>.</param>
        /// <returns>The result of the interpolation as a <see cref="Color"/>.</returns>
        public static Color Lerp(Color color1, Color color2, float amount)
        {
            amount = Math.Min(1, Math.Max(0, amount));

            byte alpha = (byte)Math.Min(255, Math.Max(0, color1.A + amount * (color2.A - color1.A)));
            byte red = (byte)Math.Min(255, Math.Max(0, color1.R + amount * (color2.R - color1.R)));
            byte green = (byte)Math.Min(255, Math.Max(0, color1.G + amount * (color2.G - color1.G)));
            byte blue = (byte)Math.Min(255, Math.Max(0, color1.B + amount * (color2.B - color1.B)));

            return new Color(alpha, red, green, blue);
        }

        /// <summary>
        /// Subtracts a color from another color and returns the result.
        /// </summary>
        /// <param name="color1">The color to be subtracted from.</param>
        /// <param name="color2">The color to subtract from <paramref name="color1"/>.</param>
        /// <returns>The result of the subtraction as a <see cref="Color"/>.</returns>
        public static Color Subtract(Color color1, Color color2)
        {
            return new Color((byte)Math.Max(0, color1.A - color2.A), (byte)Math.Max(0, color1.R - color2.R),
                (byte)Math.Max(0, color1.G - color2.G), (byte)Math.Max(0, color1.B - color2.B));
        }

        /// <summary>
        /// Determines whether the specified <see cref="Color"/> is equal to the current <see cref="Color"/>.
        /// </summary>
        /// <param name="other">The <see cref="Color"/> to compare with the current <see cref="Color"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="Color"/> is equal to the current <see cref="Color"/>; otherwise, <c>false</c>.</returns>
        public bool Equals(Color other)
        {
            return A == other.A && R == other.R && G == other.G && B == other.B;
        }

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to the current <see cref="Color"/>.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to compare with the current <see cref="Color"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="object"/> is equal to the current <see cref="Color"/>; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (obj is Color)
            {
                return Equals((Color)obj);
            }
            return false;
        }

        /// <summary>
        /// Serves as a hash function for a <see cref="Color"/> object.
        /// </summary>
        /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a hash table.</returns>
        public override int GetHashCode()
        {
            return A << 24 | R << 16 | G << 8 | B;
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents the current <see cref="Color"/>.
        /// </summary>
        /// <returns>A <see cref="string"/> that represents the current <see cref="Color"/>.</returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, Resources.Strings.ARGB, A, R, G, B);
        }

        /// <summary>
        /// Determines whether two <see cref="Color"/> objects are considered equal.
        /// </summary>
        /// <param name="value1">The first object to compare.</param>
        /// <param name="value2">The second object to compare.</param>
        /// <returns><c>true</c> if the objects are considered equal; otherwise <c>false</c>.</returns>
        public static bool operator ==(Color value1, Color value2)
        {
            return value1.Equals(value2);
        }

        /// <summary>
        /// Determines whether two <see cref="Color"/> objects are not considered equal.
        /// </summary>
        /// <param name="value1">The first object to compare.</param>
        /// <param name="value2">The second object to compare.</param>
        /// <returns><c>true</c> if the objects are not considered equal; otherwise <c>false</c>.</returns>
        public static bool operator !=(Color value1, Color value2)
        {
            return !value1.Equals(value2);
        }

        /// <summary>
        /// Adds the components of two <see cref="Color"/> objects together.
        /// </summary>
        /// <param name="value1">The left argument.</param>
        /// <param name="value2">The right argument.</param>
        /// <returns>A <see cref="Color"/> object containing the sum of each color component.</returns>
        public static Color operator +(Color value1, Color value2)
        {
            return Add(value1, value2);
        }

        /// <summary>
        /// Subtracts the components of a <see cref="Color"/> object by the components of another <see cref="Color"/> object.
        /// </summary>
        /// <param name="value1">The left argument.</param>
        /// <param name="value2">The right argument.</param>
        /// <returns>A <see cref="Color"/> object containing the difference of each color component.</returns>
        public static Color operator -(Color value1, Color value2)
        {
            return Subtract(value1, value2);
        }
    }
}
