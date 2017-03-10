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
    /// Represents a cubic Bezier curve drawn between two points in a <see cref="PathFigure"/>.
    /// </summary>
    public sealed class BezierSegment : PathSegment
    {
        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:ControlPoint1"/> property.
        /// </summary>
        public static PropertyDescriptor ControlPoint1Property { get; } = PropertyDescriptor.Create(nameof(ControlPoint1), typeof(Point), typeof(BezierSegment), new FrameworkPropertyMetadata(OnPathPropertyChanged));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:ControlPoint2"/> property.
        /// </summary>
        public static PropertyDescriptor ControlPoint2Property { get; } = PropertyDescriptor.Create(nameof(ControlPoint2), typeof(Point), typeof(BezierSegment), new FrameworkPropertyMetadata(OnPathPropertyChanged));
        
        /// <summary>
        /// Gets or sets the first control point of the curve.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", Justification = "Exception parameter refers to property name for easier understanding of invalid value.")]
        public Point ControlPoint1
        {
            get { return controlPoint1; }
            set
            {
                if (value.X != controlPoint1.X || value.Y != controlPoint1.Y)
                {
                    if (double.IsNaN(value.X) || double.IsInfinity(value.X))
                    {
                        throw new ArgumentException(Resources.Strings.ValueCannotBeNaNOrInfinity, "ControlPoint1.X");
                    }

                    if (double.IsNaN(value.Y) || double.IsInfinity(value.Y))
                    {
                        throw new ArgumentException(Resources.Strings.ValueCannotBeNaNOrInfinity, "ControlPoint1.Y");
                    }

                    controlPoint1 = value;
                    OnPropertyChanged(ControlPoint1Property);
                }
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Point controlPoint1;

        /// <summary>
        /// Gets or sets the second control point of the curve.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", Justification = "Exception parameter refers to property name for easier understanding of invalid value.")]
        public Point ControlPoint2
        {
            get { return controlPoint2; }
            set
            {
                if (value.X != controlPoint2.X || value.Y != controlPoint2.Y)
                {
                    if (double.IsNaN(value.X) || double.IsInfinity(value.X))
                    {
                        throw new ArgumentException(Resources.Strings.ValueCannotBeNaNOrInfinity, "ControlPoint2.X");
                    }

                    if (double.IsNaN(value.Y) || double.IsInfinity(value.Y))
                    {
                        throw new ArgumentException(Resources.Strings.ValueCannotBeNaNOrInfinity, "ControlPoint2.Y");
                    }

                    controlPoint2 = value;
                    OnPropertyChanged(ControlPoint2Property);
                }
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Point controlPoint2;

        /// <summary>
        /// Initializes a new instance of the <see cref="BezierSegment"/> class.
        /// </summary>
        public BezierSegment()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BezierSegment"/> class.
        /// </summary>
        /// <param name="controlPoint1">The first control point of the curve.</param>
        /// <param name="controlPoint2">The second control point of the curve.</param>
        /// <param name="endPoint">The point at which the segment ends.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="controlPoint1"/> contains a NaN or infinite value -or-
        /// when <paramref name="controlPoint2"/> contains a NaN or infinite value -or- when <paramref name="endPoint"/> contains a NaN or infinite value.</exception>
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", Justification = "Exception parameter refers to argument property name for easier understanding of invalid value.")]
        public BezierSegment(Point controlPoint1, Point controlPoint2, Point endPoint)
            : base(endPoint)
        {
            if (double.IsNaN(controlPoint1.X) || double.IsInfinity(controlPoint1.X))
            {
                throw new ArgumentException(Resources.Strings.ValueCannotBeNaNOrInfinity, "controlPoint1.X");
            }

            if (double.IsNaN(controlPoint1.Y) || double.IsInfinity(controlPoint1.Y))
            {
                throw new ArgumentException(Resources.Strings.ValueCannotBeNaNOrInfinity, "controlPoint1.Y");
            }

            if (double.IsNaN(controlPoint2.X) || double.IsInfinity(controlPoint2.X))
            {
                throw new ArgumentException(Resources.Strings.ValueCannotBeNaNOrInfinity, "controlPoint2.X");
            }

            if (double.IsNaN(controlPoint2.Y) || double.IsInfinity(controlPoint2.Y))
            {
                throw new ArgumentException(Resources.Strings.ValueCannotBeNaNOrInfinity, "controlPoint2.Y");
            }
            
            this.controlPoint1 = controlPoint1;
            this.controlPoint2 = controlPoint2;
        }

        internal override Point GetBottomLeftCorner(Point startPoint)
        {
            // This "corner" includes the control points.
            return new Point(Math.Max(controlPoint1.X, Math.Max(controlPoint2.X, Math.Max(startPoint.X, EndPoint.X))),
                Math.Max(controlPoint1.Y, Math.Max(controlPoint2.Y, Math.Max(startPoint.Y, EndPoint.Y))));
        }
    }
}
