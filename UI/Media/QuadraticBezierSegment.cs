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
    /// Represents a quadratic Bezier curve drawn between two points in a <see cref="PathFigure"/>.
    /// </summary>
    public sealed class QuadraticBezierSegment : PathSegment
    {
        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:ControlPoint"/> property.
        /// </summary>
        public static PropertyDescriptor ControlPointProperty { get; } = PropertyDescriptor.Create(nameof(ControlPoint), typeof(Point), typeof(QuadraticBezierSegment), new FrameworkPropertyMetadata(OnPathPropertyChanged));

        /// <summary>
        /// Gets or sets the control point of the curve.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", Justification = "Exception parameter refers to property name for easier understanding of invalid value.")]
        public Point ControlPoint
        {
            get { return controlPoint; }
            set
            {
                if (value.X != controlPoint.X || value.Y != controlPoint.Y)
                {
                    if (double.IsNaN(value.X) || double.IsInfinity(value.X))
                    {
                        throw new ArgumentException(Resources.Strings.ValueCannotBeNaNOrInfinity, "ControlPoint.X");
                    }

                    if (double.IsNaN(value.Y) || double.IsInfinity(value.Y))
                    {
                        throw new ArgumentException(Resources.Strings.ValueCannotBeNaNOrInfinity, "ControlPoint.Y");
                    }

                    controlPoint = value;
                    OnPropertyChanged(ControlPointProperty);
                }
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Point controlPoint;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuadraticBezierSegment"/> class.
        /// </summary>
        public QuadraticBezierSegment()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QuadraticBezierSegment"/> class.
        /// </summary>
        /// <param name="controlPoint">The control point of the curve.</param>
        /// <param name="endPoint">The point at which the segment ends.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="controlPoint"/> contains a NaN or infinite value -or- when <paramref name="endPoint"/> contains a NaN or infinite value.</exception>
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", Justification = "Exception parameter refers to argument property name for easier understanding of invalid value.")]
        public QuadraticBezierSegment(Point controlPoint, Point endPoint)
            : base(endPoint)
        {
            if (double.IsNaN(controlPoint.X) || double.IsInfinity(controlPoint.X))
            {
                throw new ArgumentException(Resources.Strings.ValueCannotBeNaNOrInfinity, "controlPoint.X");
            }

            if (double.IsNaN(controlPoint.Y) || double.IsInfinity(controlPoint.Y))
            {
                throw new ArgumentException(Resources.Strings.ValueCannotBeNaNOrInfinity, "controlPoint.Y");
            }

            this.controlPoint = controlPoint;
        }

        internal override Point GetBottomLeftCorner(Point startPoint)
        {
            // This "corner" includes the control point.
            return new Point(Math.Max(controlPoint.X, Math.Max(startPoint.X, EndPoint.X)), Math.Max(controlPoint.Y, Math.Max(startPoint.Y, EndPoint.Y)));
        }
    }
}
