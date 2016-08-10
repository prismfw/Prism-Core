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
    /// Represents a transformation that rotates an object clockwise about a specified point in two-dimensional space.
    /// </summary>
    public class RotateTransform : Transform
    {
        #region Property Descriptors
        /// <summary>
        /// Describes the <see cref="P:Angle"/> property.  This field is read-only.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "PropertyDescriptor is immutable.")]
        public static readonly PropertyDescriptor AngleProperty = PropertyDescriptor.Create(nameof(Angle), typeof(double), typeof(RotateTransform));

        /// <summary>
        /// Describes the <see cref="P:CenterX"/> property.  This field is read-only.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "PropertyDescriptor is immutable.")]
        public static readonly PropertyDescriptor CenterXProperty = PropertyDescriptor.Create(nameof(CenterX), typeof(double), typeof(RotateTransform));

        /// <summary>
        /// Describes the <see cref="P:CenterY"/> property.  This field is read-only.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "PropertyDescriptor is immutable.")]
        public static readonly PropertyDescriptor CenterYProperty = PropertyDescriptor.Create(nameof(CenterY), typeof(double), typeof(RotateTransform));
        #endregion

        /// <summary>
        /// Gets or sets the angle of clockwise rotation, in degrees.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", Justification = "Exception parameter refers to property name for easier understanding of invalid value.")]
        public double Angle
        {
            get { return angle; }
            set
            {
                if (value != angle)
                {
                    if (double.IsNaN(value) || double.IsInfinity(value))
                    {
                        throw new ArgumentException(Resources.Strings.ValueCannotBeNaNOrInfinity, nameof(Angle));
                    }

                    angle = value;
                    SetValue();
                    OnPropertyChanged(AngleProperty);
                }
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private double angle;

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
        /// Initializes a new instance of the <see cref="RotateTransform"/> class.
        /// </summary>
        public RotateTransform()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RotateTransform"/> class.
        /// </summary>
        /// <param name="angle">The angle of clockwise rotation, in degrees.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="angle"/> is NaN or infinite.</exception>
        public RotateTransform(double angle)
            : this(angle, 0, 0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RotateTransform"/> class.
        /// </summary>
        /// <param name="angle">The angle of clockwise rotation, in degrees.</param>
        /// <param name="centerX">The X-coordinate of the center point of the transform.</param>
        /// <param name="centerY">The Y-coordinate of the center point of the transform.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="angle"/> is NaN or infinite -or- when <paramref name="centerX"/> is NaN or infinite -or-
        /// when <paramref name="centerY"/> is NaN or infinite.</exception>
        public RotateTransform(double angle, double centerX, double centerY)
        {
            if (double.IsNaN(angle) || double.IsInfinity(angle))
            {
                throw new ArgumentException(Resources.Strings.ValueCannotBeNaNOrInfinity, nameof(angle));
            }

            if (double.IsNaN(centerX) || double.IsInfinity(centerX))
            {
                throw new ArgumentException(Resources.Strings.ValueCannotBeNaNOrInfinity, nameof(centerX));
            }

            if (double.IsNaN(centerY) || double.IsInfinity(centerY))
            {
                throw new ArgumentException(Resources.Strings.ValueCannotBeNaNOrInfinity, nameof(centerY));
            }

            this.angle = angle;
            this.centerX = centerX;
            this.centerY = centerY;
            SetValue();
        }

        private void SetValue()
        {
            var matrix = Matrix.Identity;
            matrix.Rotate(angle, centerX, centerY);
            nativeObject.Value = matrix;
        }
    }
}
