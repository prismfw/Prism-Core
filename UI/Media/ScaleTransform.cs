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

namespace Prism.UI.Media
{
    /// <summary>
    /// Represents a transformation that scales an object in two-dimensional space.
    /// </summary>
    public class ScaleTransform : Transform
    {
        #region Property Descriptors
        /// <summary>
        /// Describes the <see cref="P:CenterX"/> property.  This field is read-only.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "PropertyDescriptor is immutable.")]
        public static readonly PropertyDescriptor CenterXProperty = PropertyDescriptor.Create(nameof(CenterX), typeof(double), typeof(ScaleTransform));

        /// <summary>
        /// Describes the <see cref="P:CenterY"/> property.  This field is read-only.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "PropertyDescriptor is immutable.")]
        public static readonly PropertyDescriptor CenterYProperty = PropertyDescriptor.Create(nameof(CenterY), typeof(double), typeof(ScaleTransform));

        /// <summary>
        /// Describes the <see cref="P:ScaleX"/> property.  This field is read-only.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "PropertyDescriptor is immutable.")]
        public static readonly PropertyDescriptor ScaleXProperty = PropertyDescriptor.Create(nameof(ScaleX), typeof(double), typeof(ScaleTransform));

        /// <summary>
        /// Describes the <see cref="P:ScaleY"/> property.  This field is read-only.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "PropertyDescriptor is immutable.")]
        public static readonly PropertyDescriptor ScaleYProperty = PropertyDescriptor.Create(nameof(ScaleY), typeof(double), typeof(ScaleTransform));
        #endregion

        /// <summary>
        /// Gets or sets the X-coordinate of the center point of the transform.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", Justification = "Exception parameter refers to property name for easier understanding of invalid value.")]
        public double CenterX
        {
            get { return centerX; }
            set
            {
                if (value != centerX)
                {
                    if (double.IsNaN(value) || double.IsInfinity(value))
                    {
                        throw new ArgumentException(Resources.Strings.ValueCannotBeNaNOrInfinity, nameof(CenterX));
                    }

                    centerX = value;

                    var matrix = nativeObject.Value;
                    matrix.OffsetX = centerX - (matrix.M11 * centerX);
                    nativeObject.Value = matrix;

                    OnPropertyChanged(CenterXProperty);
                }
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private double centerX;

        /// <summary>
        /// Gets or sets the Y-coordinate of the center point of the transform.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", Justification = "Exception parameter refers to property name for easier understanding of invalid value.")]
        public double CenterY
        {
            get { return centerY; }
            set
            {
                if (value != centerY)
                {
                    if (double.IsNaN(value) || double.IsInfinity(value))
                    {
                        throw new ArgumentException(Resources.Strings.ValueCannotBeNaNOrInfinity, nameof(CenterY));
                    }

                    centerY = value;

                    var matrix = nativeObject.Value;
                    matrix.OffsetY = centerY - (matrix.M22 * centerY);
                    nativeObject.Value = matrix;

                    OnPropertyChanged(CenterXProperty);
                }
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private double centerY;

        /// <summary>
        /// Gets or sets the scaling factor along the X-axis.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", Justification = "Exception parameter refers to property name for easier understanding of invalid value.")]
        public double ScaleX
        {
            get { return nativeObject.Value.M11; }
            set
            {
                var matrix = nativeObject.Value;
                if (value != matrix.M11)
                {
                    if (double.IsNaN(value) || double.IsInfinity(value))
                    {
                        throw new ArgumentException(Resources.Strings.ValueCannotBeNaNOrInfinity, nameof(ScaleX));
                    }

                    matrix.M11 = value;
                    matrix.OffsetX = centerX - (matrix.M11 * centerX);
                    nativeObject.Value = matrix;
                    OnPropertyChanged(ScaleXProperty);
                }
            }
        }

        /// <summary>
        /// Gets or sets the scaling factor along the Y-axis.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", Justification = "Exception parameter refers to property name for easier understanding of invalid value.")]
        public double ScaleY
        {
            get { return nativeObject.Value.M22; }
            set
            {
                var matrix = nativeObject.Value;
                if (value != matrix.M22)
                {
                    if (double.IsNaN(value) || double.IsInfinity(value))
                    {
                        throw new ArgumentException(Resources.Strings.ValueCannotBeNaNOrInfinity, nameof(ScaleY));
                    }

                    matrix.M22 = value;
                    matrix.OffsetY = centerY - (matrix.M22 * centerY);
                    nativeObject.Value = matrix;
                    OnPropertyChanged(ScaleYProperty);
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScaleTransform"/> class.
        /// </summary>
        public ScaleTransform()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScaleTransform"/> class.
        /// </summary>
        /// <param name="scaleX">The scaling factor along the X-axis.</param>
        /// <param name="scaleY">The scaling factor along the Y-axis.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="scaleX"/> is NaN or infinite -or- when <paramref name="scaleY"/> is NaN or infinite.</exception>
        public ScaleTransform(double scaleX, double scaleY)
            : this(scaleX, scaleY, 0, 0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScaleTransform"/> class.
        /// </summary>
        /// <param name="scaleX">The scaling factor along the X-axis.</param>
        /// <param name="scaleY">The scaling factor along the Y-axis.</param>
        /// <param name="centerX">The X-coordinate of the center point of the transform.</param>
        /// <param name="centerY">The Y-coordinate of the center point of the transform.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="scaleX"/> is NaN or infinite -or- when <paramref name="scaleY"/> is NaN or infinite -or-
        /// when <paramref name="centerX"/> is NaN or infinite -or- when <paramref name="centerY"/> is NaN or infinite.</exception>
        public ScaleTransform(double scaleX, double scaleY, double centerX, double centerY)
        {
            if (double.IsNaN(scaleX) || double.IsInfinity(scaleX))
            {
                throw new ArgumentException(Resources.Strings.ValueCannotBeNaNOrInfinity, nameof(scaleX));
            }

            if (double.IsNaN(scaleY) || double.IsInfinity(scaleY))
            {
                throw new ArgumentException(Resources.Strings.ValueCannotBeNaNOrInfinity, nameof(scaleY));
            }

            if (double.IsNaN(centerX) || double.IsInfinity(centerX))
            {
                throw new ArgumentException(Resources.Strings.ValueCannotBeNaNOrInfinity, nameof(centerX));
            }

            if (double.IsNaN(centerY) || double.IsInfinity(centerY))
            {
                throw new ArgumentException(Resources.Strings.ValueCannotBeNaNOrInfinity, nameof(centerY));
            }

            this.centerX = centerX;
            this.centerY = centerY;

            nativeObject.Value = new Matrix(scaleX, 0, 0, scaleY, centerX - (scaleX * centerX), centerY - (scaleY * centerY));
        }
    }
}
