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

namespace Prism
{
    /// <summary>
    /// Represents the width, height, and location of a rectangle.
    /// </summary>
    public struct Rectangle
    {
        /// <summary>
        /// Gets the Y-axis value of the bottom edge of the rectangle.
        /// </summary>
        public double Bottom
        {
            get { return Y + Height; }
        }

        /// <summary>
        /// Gets the X- and Y-axis values of the bottom left corner of the rectangle.
        /// </summary>
        public Point BottomLeft
        {
            get { return new Point(X, Y + Height); }
        }

        /// <summary>
        /// Gets the X- and Y-axis values of the bottom right corner of the rectangle.
        /// </summary>
        public Point BottomRight
        {
            get { return new Point(X + Width, Y + Height); }
        }

        /// <summary>
        /// Gets or sets the height of this instance.
        /// </summary>
        public double Height { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is empty.
        /// </summary>
        public bool IsEmpty
        {
            get { return Width == 0 && Height == 0; }
        }

        /// <summary>
        /// Gets the X-axis value of the left edge of the rectangle.
        /// </summary>
        public double Left
        {
            get { return X; }
        }

        /// <summary>
        /// Gets the X-axis value of the right edge of the rectangle.
        /// </summary>
        public double Right
        {
            get { return X + Width; }
        }

        /// <summary>
        /// Gets the width and height of the rectangle as a <see cref="Size"/> instance.
        /// </summary>
        public Size Size
        {
            get { return new Size(Width, Height); }
        }

        /// <summary>
        /// Gets the Y-axis value of the top edge of the rectangle.
        /// </summary>
        public double Top
        {
            get { return Y; }
        }

        /// <summary>
        /// Gets the X- and Y-axis values of the top left corner of the rectangle.
        /// </summary>
        public Point TopLeft
        {
            get { return new Point(X, Y); }
        }

        /// <summary>
        /// Gets the X- and Y-axis values of the top right corner of the rectangle.
        /// </summary>
        public Point TopRight
        {
            get { return new Point(X + Width, Y); }
        }

        /// <summary>
        /// Gets or sets the width of this instance.
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        /// Gets or sets the X-coordinate of the upper left corner of this instance.
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Gets or sets the Y-coordinate of the upper left corner of this instance.
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Rectangle"/> structure.
        /// </summary>
        /// <param name="size">The size of the rectangle.  The location of the upper left corner is set to (0, 0).</param>
        public Rectangle(Size size)
        {
            X = Y = 0;
            Width = size.Width;
            Height = size.Height;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Rectangle"/> structure.
        /// </summary>
        /// <param name="point1">The point describing the location of the upper left corner of the rectangle.</param>
        /// <param name="point2">The point describing the location of the lower right corner of the rectangle.</param>
        public Rectangle(Point point1, Point point2)
        {
            X = point1.X;
            Y = point1.Y;
            Width = point2.X - point1.X;
            Height = point2.Y - point1.Y;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Rectangle"/> structure.
        /// </summary>
        /// <param name="location">The point describing the location of the upper left corner of the rectangle.</param>
        /// <param name="size">The size of the rectangle.</param>
        public Rectangle(Point location, Size size)
        {
            X = location.X;
            Y = location.Y;
            Width = size.Width;
            Height = size.Height;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Rectangle"/> structure.
        /// </summary>
        /// <param name="x">The X coordinate of the upper left corner.</param>
        /// <param name="y">The Y coordinate of the upper left corner.</param>
        /// <param name="width">The width of the rectangle.</param>
        /// <param name="height">The height of the rectangle.</param>
        public Rectangle(double x, double y, double width, double height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Determines whether the current <see cref="Rectangle"/> contains the specified <see cref="Point"/>.
        /// </summary>
        /// <param name="point">The point to check.</param>
        /// <returns><c>true</c> if the rectangle contains the <paramref name="point"/>; otherwise, <c>false</c>.</returns>
        public bool Contains(Point point)
        {
            return X <= point.X && Y <= point.Y && Right >= point.X && Bottom >= point.Y;
        }

        /// <summary>
        /// Determines whether the current <see cref="Rectangle"/> completely contains the specified <see cref="Rectangle"/>.
        /// </summary>
        /// <param name="rect">The rectangle to check.</param>
        /// <returns><c>true</c> if the current rectangle contains the specified rectangle; otherwise, <c>false</c>.</returns>
        public bool Contains(Rectangle rect)
        {
            return X <= rect.X && Y <= rect.Y && Right >= rect.Right && Bottom >= rect.Bottom;
        }

        /// <summary>
        /// Determines whether the specified <see cref="Rectangle"/> is equal to the current <see cref="Rectangle"/>.
        /// </summary>
        /// <param name="other">The <see cref="Rectangle"/> to compare with the current <see cref="Rectangle"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="Rectangle"/> is equal to the current
        /// <see cref="Rectangle"/>; otherwise, <c>false</c>.</returns>
        public bool Equals(Rectangle other)
        {
            return X == other.X && Y == other.Y && Width == other.Width && Height == other.Height;
        }

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to the current <see cref="Rectangle"/>.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to compare with the current <see cref="Rectangle"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="object"/> is equal to the current
        /// <see cref="Rectangle"/>; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (obj is Rectangle)
            {
                return Equals((Rectangle)obj);
            }
            return false;
        }

        /// <summary>
        /// Serves as a hash function for a <see cref="Rectangle"/> object.
        /// </summary>
        /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a
        /// hash table.</returns>
        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode() ^ Width.GetHashCode() ^ Height.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents the current <see cref="Rectangle"/>.
        /// </summary>
        /// <returns>A <see cref="string"/> that represents the current <see cref="Rectangle"/>.</returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, Resources.Strings.XYWidthHeight, X, Y, Width, Height);
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents the current <see cref="Rectangle"/>.
        /// </summary>
        /// <param name="format">A numeric format string.</param>
        /// <returns>A <see cref="string"/> that represents the current <see cref="Rectangle"/>.</returns>
        public string ToString(string format)
        {
            return string.Format(CultureInfo.CurrentCulture, Resources.Strings.XYWidthHeight,
                X.ToString(format, CultureInfo.CurrentCulture), Y.ToString(format, CultureInfo.CurrentCulture),
                Width.ToString(format, CultureInfo.CurrentCulture), Height.ToString(format, CultureInfo.CurrentCulture));
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents the current <see cref="Rectangle"/>.
        /// </summary>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <returns>A <see cref="string"/> that represents the current <see cref="Rectangle"/>.</returns>
        public string ToString(IFormatProvider provider)
        {
            return string.Format(provider, Resources.Strings.XYWidthHeight,
                X.ToString(provider), Y.ToString(provider), Width.ToString(provider), Height.ToString(provider));
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents the current <see cref="Rectangle"/>.
        /// </summary>
        /// <param name="format">A numeric format string.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <returns>A <see cref="string"/> that represents the current <see cref="Rectangle"/>.</returns>
        public string ToString(string format, IFormatProvider provider)
        {
            return string.Format(provider, Resources.Strings.XYWidthHeight, X.ToString(format, provider),
                Y.ToString(format, provider), Width.ToString(format, provider), Height.ToString(format, provider));
        }

        /// <summary>
        /// Expands the current <see cref="Rectangle"/> to contain the specified <see cref="Point"/>.
        /// </summary>
        /// <param name="point">The point to include.</param>
        public void Union(Point point)
        {
            double x = Math.Min(X, point.X);
            double y = Math.Min(Y, point.Y);
            Width = Math.Max(X + Width, point.X) - x;
            Height = Math.Max(Y + Height, point.Y) - y;
            X = x;
            Y = y;
        }

        /// <summary>
        /// Expands the current <see cref="Rectangle"/> to contain the specified <see cref="Rectangle"/>.
        /// </summary>
        /// <param name="rect">The rectangle to include.</param>
        public void Union(Rectangle rect)
        {
            double x = Math.Min(X, rect.X);
            double y = Math.Min(Y, rect.Y);
            Width = Math.Max(X + Width, rect.X + rect.Width) - x;
            Height = Math.Max(Y + Height, rect.Y + rect.Height) - y;
            X = x;
            Y = y;
        }

        /// <summary>
        /// Determines whether two <see cref="Rectangle"/> objects are considered equal.
        /// </summary>
        /// <param name="value1">The first object to compare.</param>
        /// <param name="value2">The second object to compare.</param>
        /// <returns><c>true</c> if the objects are considered equal; otherwise <c>false</c>.</returns>
        public static bool operator ==(Rectangle value1, Rectangle value2)
        {
            return value1.Equals(value2);
        }

        /// <summary>
        /// Determines whether two <see cref="Rectangle"/> objects are not considered equal.
        /// </summary>
        /// <param name="value1">The first object to compare.</param>
        /// <param name="value2">The second object to compare.</param>
        /// <returns><c>true</c> if the objects are not considered equal; otherwise <c>false</c>.</returns>
        public static bool operator !=(Rectangle value1, Rectangle value2)
        {
            return !value1.Equals(value2);
        }
    }
}
