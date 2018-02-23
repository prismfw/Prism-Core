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

namespace Prism.UI.Media
{
    /// <summary>
    /// Represents a transformation that translates (moves) an object in two-dimensional space.
    /// </summary>
    public class TranslateTransform : Transform
    {
        #region Property Descriptors
        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:X"/> property.
        /// </summary>
        public static PropertyDescriptor XProperty { get; } = PropertyDescriptor.Create(nameof(X), typeof(double), typeof(TranslateTransform));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:Y"/> property.
        /// </summary>
        public static PropertyDescriptor YProperty { get; } = PropertyDescriptor.Create(nameof(Y), typeof(double), typeof(TranslateTransform));
        #endregion

        /// <summary>
        /// Gets or sets the distance to translate along the X-axis.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Justification = "Name in common usage.")]
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", Justification = "Exception parameter refers to property name for easier understanding of invalid value.")]
        public double X
        {
            get { return nativeObject.Value.OffsetX; }
            set
            {
                var matrix = nativeObject.Value;
                if (value != matrix.OffsetX)
                {
                    if (double.IsNaN(value) || double.IsInfinity(value))
                    {
                        throw new ArgumentException(Resources.Strings.ValueCannotBeNaNOrInfinity, nameof(X));
                    }

                    matrix.OffsetX = value;
                    nativeObject.Value = matrix;
                    OnPropertyChanged(XProperty);
                }
            }
        }

        /// <summary>
        /// Gets or sets the distance to translate along the Y-axis.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Justification = "Name in common usage.")]
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", Justification = "Exception parameter refers to property name for easier understanding of invalid value.")]
        public double Y
        {
            get { return nativeObject.Value.OffsetY; }
            set
            {
                var matrix = nativeObject.Value;
                if (value != matrix.OffsetY)
                {
                    if (double.IsNaN(value) || double.IsInfinity(value))
                    {
                        throw new ArgumentException(Resources.Strings.ValueCannotBeNaNOrInfinity, nameof(Y));
                    }

                    matrix.OffsetY = value;
                    nativeObject.Value = matrix;
                    OnPropertyChanged(YProperty);
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TranslateTransform"/> class.
        /// </summary>
        public TranslateTransform()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TranslateTransform"/> class.
        /// </summary>
        /// <param name="x">The distance to translate along the X-axis.</param>
        /// <param name="y">The distance to translate along the Y-axis.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="x"/> is NaN or infinite -or- when <paramref name="y"/> is NaN or infinite.</exception>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "x", Justification = "Parameter name corresponds to identically named property.")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "y", Justification = "Parameter name corresponds to identically named property.")]
        public TranslateTransform(double x, double y)
        {
            if (double.IsNaN(x) || double.IsInfinity(x))
            {
                throw new ArgumentException(Resources.Strings.ValueCannotBeNaNOrInfinity, nameof(x));
            }

            if (double.IsNaN(y) || double.IsInfinity(y))
            {
                throw new ArgumentException(Resources.Strings.ValueCannotBeNaNOrInfinity, nameof(y));
            }

            nativeObject.Value = new Matrix(1, 0, 0, 1, x, y);
        }
    }
}
