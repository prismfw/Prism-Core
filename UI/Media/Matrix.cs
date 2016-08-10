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
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Prism.UI.Media
{
    /// <summary>
    /// Represents a 3x3 affine transformation matrix used for transformations in two-dimensional space.
    /// </summary>
    public struct Matrix
    {
        /// <summary>
        /// Gets an identity matrix.
        /// </summary>
        public static Matrix Identity
        {
            get { return new Matrix(1, 0, 0, 1, 0, 0); }
        }

        /// <summary>
        /// Gets the determinant of this instance.
        /// </summary>
        public double Determinant
        {
            get { return (M11 * M22) - (M12 * M21); }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is an identity matrix.
        /// </summary>
        public bool IsIdentity
        {
            get { return M11 == 1 && M12 == 0 && M21 == 0 && M22 == 1 && OffsetX == 0 && OffsetY == 0; }
        }

        /// <summary>
        /// Gets or sets the value of the first row and first column of this instance.
        /// </summary>
        public double M11
        {
            get { return flag ? m11 : 1; }
            set
            {
                m11 = value;
                SetFlag();
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private double m11;

        /// <summary>
        /// Gets or sets the value of the first row and second column of this instance.
        /// </summary>
        public double M12
        {
            get { return m12; }
            set
            {
                m12 = value;
                SetFlag();
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private double m12;

        /// <summary>
        /// Gets or sets the value of the second row and first column of this instance.
        /// </summary>
        public double M21
        {
            get { return m21; }
            set
            {
                m21 = value;
                SetFlag();
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private double m21;

        /// <summary>
        /// Gets or sets the value of the second row and second column of this instance.
        /// </summary>
        public double M22
        {
            get { return flag ? m22 : 1; }
            set
            {
                m22 = value;
                SetFlag();
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private double m22;

        /// <summary>
        /// Gets or sets the value of the third row and first column of this instance.
        /// </summary>
        public double OffsetX
        {
            get { return m31; }
            set
            {
                m31 = value;
                SetFlag();
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private double m31;

        /// <summary>
        /// Gets or sets the value of the third row and second column of this instance.
        /// </summary>
        public double OffsetY
        {
            get { return m32; }
            set
            {
                m32 = value;
                SetFlag();
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private double m32;

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private bool flag;

        /// <summary>
        /// Initializes a new instance of the <see cref="Matrix"/> structure.
        /// </summary>
        /// <param name="m11">The value of the first row and first column of this instance.</param>
        /// <param name="m12">The value of the first row and second column of this instance.</param>
        /// <param name="m21">The value of the second row and first column of this instance.</param>
        /// <param name="m22">The value of the second row and second column of this instance.</param>
        /// <param name="offsetX">The value of the third row and first column of this instance.</param>
        /// <param name="offsetY">The value of the third row and second column of this instance.</param>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "m", Justification = "Parameter name corresponds to identically named property.")]
        public Matrix(double m11, double m12, double m21, double m22, double offsetX, double offsetY)
        {
            this.m11 = m11;
            this.m12 = m12;
            this.m21 = m21;
            this.m22 = m22;
            m31 = offsetX;
            m32 = offsetY;
            flag = true;
        }

        /// <summary>
        /// Determines whether the specified <see cref="Matrix"/> is equal to the current <see cref="Matrix"/>.
        /// </summary>
        /// <param name="other">The <see cref="Matrix"/> to compare with the current <see cref="Matrix"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="Matrix"/> is equal to the current <see cref="Matrix"/>; otherwise, <c>false</c>.</returns>
        public bool Equals(Matrix other)
        {
            return M11 == other.M11 && M12 == other.M12 && M21 == other.M21 && M22 == other.M22 &&
                OffsetX == other.OffsetX && OffsetY == other.OffsetY;
        }

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to the current <see cref="Matrix"/>.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to compare with the current <see cref="Matrix"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="object"/> is equal to the current <see cref="Matrix"/>; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (obj is Matrix)
            {
                return Equals((Matrix)obj);
            }
            return false;
        }

        /// <summary>
        /// Serves as a hash function for a <see cref="Matrix"/> object.
        /// </summary>
        /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a hash table.</returns>
        public override int GetHashCode()
        {
            return M11.GetHashCode() ^ M12.GetHashCode() ^ M21.GetHashCode() ^ M22.GetHashCode() ^ OffsetX.GetHashCode() ^ OffsetY.GetHashCode();
        }

        /// <summary>
        /// Rotates this instance about the specified point.
        /// </summary>
        /// <param name="angle">The angle of rotation, in degrees.</param>
        /// <param name="centerX">The X-coordinate of the point about which to rotate this instance.</param>
        /// <param name="centerY">The Y-coordinate of the point about which to rotate this instance.</param>
        public void Rotate(double angle, double centerX, double centerY)
        {
            double radians = (angle % 360) * (Math.PI / 180);
            double sin = Math.Sin(radians);
            double cos = Math.Cos(radians);

            this *= new Matrix(cos, sin, -sin, cos, (centerX * (1.0 - cos)) + (centerY * sin), (centerY * (1.0 - cos)) - (centerX * sin));
        }

        /// <summary>
        /// Scales this instance by the specified amount about the specified point.
        /// </summary>
        /// <param name="scaleX">The amount by which to scale this instance along the X-axis.</param>
        /// <param name="scaleY">The amount by which to scale this instance along the Y-axis.</param>
        /// <param name="centerX">The X-coordinate of the scale operation's center point.</param>
        /// <param name="centerY">The Y-coordinate of the scale operation's center point.</param>
        public void Scale(double scaleX, double scaleY, double centerX, double centerY)
        {
            this *= new Matrix(scaleX, 0, 0, scaleY, centerX - (scaleX * centerX), centerY - (scaleY * centerY));
        }

        /// <summary>
        /// Appends a skew of the specified degrees in the X and Y dimensions to this instance.
        /// </summary>
        /// <param name="skewX">The angle in the X dimension by which to skew this instance.</param>
        /// <param name="skewY">The angle in the Y dimension by which to skew this instance.</param>
        public void Skew(double skewX, double skewY)
        {
            this *= new Matrix(1, Math.Tan((skewY % 360) * (Math.PI / 180)), Math.Tan((skewX % 360) * (Math.PI / 180)), 1, 0, 0);
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents the current <see cref="Matrix"/>.
        /// </summary>
        /// <returns>A <see cref="string"/> that represents the current <see cref="Matrix"/>.</returns>
        public override string ToString()
        {
            if (IsIdentity)
            {
                return Resources.Strings.Identity;
            }
            
            return string.Format(CultureInfo.CurrentCulture, "{0},{1},{2},{3},{4},{5}", M11, M12, M21, M22, OffsetX, OffsetY);
        }

        /// <summary>
        /// Transforms the specified <see cref="Point"/> by this instance and returns the result.
        /// </summary>
        /// <param name="point">The point to transform.</param>
        /// <returns>The transformed point as a new <see cref="Point"/> instance.</returns>
        public Point Transform(Point point)
        {
            return new Point()
            {
                X = (point.X * M11) + (point.Y * M21) + OffsetX,
                Y = (point.X * M12) + (point.Y * M22) + OffsetY
            };
        }

        /// <summary>
        /// Appends a translation of the specified offsets to this instance.
        /// </summary>
        /// <param name="offsetX">The amount to offset this instance along the X-axis.</param>
        /// <param name="offsetY">The amount to offset this instance along the Y-axis.</param>
        public void Translate(double offsetX, double offsetY)
        {
            OffsetX += offsetX;
            OffsetY += offsetY;
        }

        /// <summary>
        /// Multiplies two <see cref="Matrix"/> objects together.
        /// </summary>
        /// <param name="value1">The left argument.</param>
        /// <param name="value2">The right argument.</param>
        /// <returns>A <see cref="Matrix"/> object containing the result of the multiplication.</returns>
        public static Matrix Multiply(Matrix value1, Matrix value2)
        {
            MultiplyMatrix(ref value1, ref value2);
            return value1;
        }

        /// <summary>
        /// Determines whether two <see cref="Matrix"/> objects are considered equal.
        /// </summary>
        /// <param name="value1">The first object to compare.</param>
        /// <param name="value2">The second object to compare.</param>
        /// <returns><c>true</c> if the objects are considered equal; otherwise <c>false</c>.</returns>
        public static bool operator ==(Matrix value1, Matrix value2)
        {
            return value1.Equals(value2);
        }

        /// <summary>
        /// Determines whether two <see cref="Matrix"/> objects are not considered equal.
        /// </summary>
        /// <param name="value1">The first object to compare.</param>
        /// <param name="value2">The second object to compare.</param>
        /// <returns><c>true</c> if the objects are not considered equal; otherwise <c>false</c>.</returns>
        public static bool operator !=(Matrix value1, Matrix value2)
        {
            return !value1.Equals(value2);
        }

        /// <summary>
        /// Multiplies two <see cref="Matrix"/> objects together.
        /// </summary>
        /// <param name="value1">The left argument.</param>
        /// <param name="value2">The right argument.</param>
        /// <returns>A <see cref="Matrix"/> object containing the result of the multiplication.</returns>
        public static Matrix operator *(Matrix value1, Matrix value2)
        {
            MultiplyMatrix(ref value1, ref value2);
            return value1;
        }

        private static void MultiplyMatrix(ref Matrix value1, ref Matrix value2)
        {
            if (value2.IsIdentity)
            {
                return;
            }

            if (value1.IsIdentity)
            {
                value1 = value2;
                return;
            }

            var retVal = new Matrix();
            retVal.m11 = (value1.M11 * value2.M11) + (value1.M12 * value2.M21);
            retVal.m12 = (value1.M11 * value2.M12) + (value1.M12 * value2.M22);
            retVal.m21 = (value1.M21 * value2.M11) + (value1.M22 * value2.M21);
            retVal.m22 = (value1.M21 * value2.M12) + (value1.M22 * value2.M22);
            retVal.m31 = (value1.OffsetX * value2.M11) + (value1.OffsetY * value2.M21) + value2.OffsetX;
            retVal.m32 = (value1.OffsetX * value2.M12) + (value1.OffsetY * value2.M22) + value2.OffsetY;
            value1 = retVal;
        }

        private void SetFlag()
        {
            if (!flag)
            {
                if (m11 == 0)
                {
                    m11 = 1;
                }

                if (m22 == 0)
                {
                    m22 = 1;
                }

                flag = true;
            }
        }
    }
}
