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
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Prism.UI.Media
{
    /// <summary>
    /// Represents a transformation that skews an object in two-dimensional space.
    /// </summary>
    public class SkewTransform : Transform
    {
        #region Property Descriptors
        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:AngleX"/> property.
        /// </summary>
        public static PropertyDescriptor AngleXProperty { get; } = PropertyDescriptor.Create(nameof(AngleX), typeof(double), typeof(SkewTransform));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:AngleY"/> property.
        /// </summary>
        public static PropertyDescriptor AngleYProperty { get; } = PropertyDescriptor.Create(nameof(AngleY), typeof(double), typeof(SkewTransform));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:CenterX"/> property.
        /// </summary>
        public static PropertyDescriptor CenterXProperty { get; } = PropertyDescriptor.Create(nameof(CenterX), typeof(double), typeof(SkewTransform));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:CenterY"/> property.
        /// </summary>
        public static PropertyDescriptor CenterYProperty { get; } = PropertyDescriptor.Create(nameof(CenterY), typeof(double), typeof(SkewTransform));
        #endregion

        /// <summary>
        /// Gets or sets the X-axis skew angle, which is measured in degrees counterclockwise from the Y-axis.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", Justification = "Exception parameter refers to property name for easier understanding of invalid value.")]
        public double AngleX
        {
            get { return angleX; }
            set
            {
                if (value != angleX)
                {
                    if (double.IsNaN(value) || double.IsInfinity(value))
                    {
                        throw new ArgumentException(Resources.Strings.ValueCannotBeNaNOrInfinity, nameof(AngleX));
                    }

                    angleX = value;
                    SetValue();
                    OnPropertyChanged(AngleXProperty);
                }
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private double angleX;

        /// <summary>
        /// Gets or sets the Y-axis skew angle, which is measured in degrees counterclockwise from the X-axis.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", Justification = "Exception parameter refers to property name for easier understanding of invalid value.")]
        public double AngleY
        {
            get { return angleY; }
            set
            {
                if (value != angleY)
                {
                    if (double.IsNaN(value) || double.IsInfinity(value))
                    {
                        throw new ArgumentException(Resources.Strings.ValueCannotBeNaNOrInfinity, nameof(AngleY));
                    }

                    angleY = value;
                    SetValue();
                    OnPropertyChanged(AngleYProperty);
                }
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private double angleY;

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
                    SetValue();
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
                    SetValue();
                    OnPropertyChanged(CenterXProperty);
                }
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private double centerY;

        /// <summary>
        /// Initializes a new instance of the <see cref="SkewTransform"/> class.
        /// </summary>
        public SkewTransform()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SkewTransform"/> class.
        /// </summary>
        /// <param name="angleX">The X-axis skew angle, which is measured in degrees counterclockwise from the Y-axis.</param>
        /// <param name="angleY">The Y-axis skew angle, which is measured in degrees counterclockwise from the X-axis.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="angleX"/> is NaN or infinite -or- when <paramref name="angleY"/> is NaN or infinite.</exception>
        public SkewTransform(double angleX, double angleY)
            : this(angleX, angleY, 0, 0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SkewTransform"/> class.
        /// </summary>
        /// <param name="angleX">The X-axis skew angle, which is measured in degrees counterclockwise from the Y-axis.</param>
        /// <param name="angleY">The Y-axis skew angle, which is measured in degrees counterclockwise from the X-axis.</param>
        /// <param name="centerX">The X-coordinate of the center point of the transform.</param>
        /// <param name="centerY">The Y-coordinate of the center point of the transform.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="angleX"/> is NaN or infinite -or- when <paramref name="angleY"/> is NaN or infinite -or-
        /// when <paramref name="centerX"/> is NaN or infinite -or- when <paramref name="centerY"/> is NaN or infinite.</exception>
        public SkewTransform(double angleX, double angleY, double centerX, double centerY)
        {
            if (double.IsNaN(angleX) || double.IsInfinity(angleX))
            {
                throw new ArgumentException(Resources.Strings.ValueCannotBeNaNOrInfinity, nameof(angleX));
            }

            if (double.IsNaN(angleY) || double.IsInfinity(angleY))
            {
                throw new ArgumentException(Resources.Strings.ValueCannotBeNaNOrInfinity, nameof(angleY));
            }

            if (double.IsNaN(centerX) || double.IsInfinity(centerX))
            {
                throw new ArgumentException(Resources.Strings.ValueCannotBeNaNOrInfinity, nameof(centerX));
            }

            if (double.IsNaN(centerY) || double.IsInfinity(centerY))
            {
                throw new ArgumentException(Resources.Strings.ValueCannotBeNaNOrInfinity, nameof(centerY));
            }

            this.angleX = angleX;
            this.angleY = angleY;
            this.centerX = centerX;
            this.centerY = centerY;
            SetValue();
        }

        private void SetValue()
        {
            var matrix = Matrix.Identity;
            matrix.Translate(-centerX, -centerY);
            matrix.Skew(angleX, angleY);
            matrix.Translate(centerX, centerY);

            nativeObject.Value = matrix;
        }
    }
}
