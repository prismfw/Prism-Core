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
    /// Represents an elliptical arc between two points.
    /// </summary>
    public sealed class ArcSegment : PathSegment
    {
        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:IsLargeArc"/> property.
        /// </summary>
        public static PropertyDescriptor IsLargeArcProperty { get; } = PropertyDescriptor.Create(nameof(IsLargeArc), typeof(bool), typeof(ArcSegment), new FrameworkPropertyMetadata(OnPathPropertyChanged));
        
        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:Size"/> property.
        /// </summary>
        public static PropertyDescriptor SizeProperty { get; } = PropertyDescriptor.Create(nameof(Size), typeof(Size), typeof(ArcSegment), new FrameworkPropertyMetadata(OnPathPropertyChanged));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:SweepDirection"/> property.
        /// </summary>
        public static PropertyDescriptor SweepDirectionProperty { get; } = PropertyDescriptor.Create(nameof(SweepDirection), typeof(SweepDirection), typeof(ArcSegment), new FrameworkPropertyMetadata(OnPathPropertyChanged));

        /// <summary>
        /// Gets or sets a value indicating whether the arc should be greater than 180 degrees.
        /// </summary>
        public bool IsLargeArc
        {
            get { return isLargeArc; }
            set
            {
                if (value != isLargeArc)
                {
                    isLargeArc = value;
                    OnPropertyChanged(IsLargeArcProperty);
                }
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool isLargeArc;

        /// <summary>
        /// Gets or sets the radii of the arc along the X and Y axes.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", Justification = "Exception parameter refers to property name for easier understanding of invalid value.")]
        public Size Size
        {
            get { return size; }
            set
            {
                if (value.Width != size.Width || value.Height != size.Height)
                {
                    if (double.IsNaN(value.Width) || double.IsInfinity(value.Width))
                    {
                        throw new ArgumentException(Resources.Strings.ValueCannotBeNaNOrInfinity, "Size.Width");
                    }

                    if (double.IsNaN(value.Height) || double.IsInfinity(value.Height))
                    {
                        throw new ArgumentException(Resources.Strings.ValueCannotBeNaNOrInfinity, "Size.Height");
                    }

                    size = new Size(Math.Abs(value.Width), Math.Abs(value.Height));
                    OnPropertyChanged(SizeProperty);
                }
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Size size;

        /// <summary>
        /// Gets or sets the direction in which the arc is drawn.
        /// </summary>
        public SweepDirection SweepDirection
        {
            get { return sweepDirection; }
            set
            {
                if (value != sweepDirection)
                {
                    sweepDirection = value;
                    OnPropertyChanged(SweepDirectionProperty);
                }
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private SweepDirection sweepDirection;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArcSegment"/> class.
        /// </summary>
        public ArcSegment()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArcSegment"/> class.
        /// </summary>
        /// <param name="endPoint">The point at which the segment ends.</param>
        /// <param name="size">The radii of the arc along the X and Y axes.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="endPoint"/> contains a NaN or infinite value -or- when <paramref name="size"/> contains a NaN or infinite value.</exception>
        public ArcSegment(Point endPoint, Size size)
            : this(endPoint, size, false, SweepDirection.Clockwise)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArcSegment"/> class.
        /// </summary>
        /// <param name="endPoint">The point at which the segment ends.</param>
        /// <param name="size">The radii of the arc along the X and Y axes.</param>
        /// <param name="isLargeArc">A value indicating whether the arc should be greater than 180 degrees.</param>
        /// <param name="sweepDirection">The direction in which to draw the arc.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="endPoint"/> contains a NaN or infinite value -or- when <paramref name="size"/> contains a NaN or infinite value.</exception>
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", Justification = "Exception parameter refers to argument property name for easier understanding of invalid value.")]
        public ArcSegment(Point endPoint, Size size,  bool isLargeArc, SweepDirection sweepDirection)
            : base(endPoint)
        {
            if (double.IsNaN(size.Width) || double.IsInfinity(size.Width))
            {
                throw new ArgumentException(Resources.Strings.ValueCannotBeNaNOrInfinity, "size.Width");
            }

            if (double.IsNaN(size.Height) || double.IsInfinity(size.Height))
            {
                throw new ArgumentException(Resources.Strings.ValueCannotBeNaNOrInfinity, "size.Height");
            }
            
            this.size = new Size(Math.Abs(size.Width), Math.Abs(size.Height));
            this.isLargeArc = isLargeArc;
            this.sweepDirection = sweepDirection;
        }

        internal override Point GetBottomLeftCorner(Point startPoint)
        {
            var endPoint = EndPoint;
            var trueSize = Size;

            // If either radius is 0, a line is given instead of an arc.
            if (trueSize.Width == 0 || trueSize.Height == 0)
            {
                return new Point(Math.Max(startPoint.X, endPoint.X), Math.Max(startPoint.Y, endPoint.Y));
            }

            double rise = Math.Round(Math.Abs(endPoint.Y - startPoint.Y), 1);
            double run = Math.Round(Math.Abs(endPoint.X - startPoint.X), 1);
            if (rise == 0 && run == 0)
            {
                return startPoint;
            }

            Point center = new Point(double.NaN, double.NaN);

            // If the radius is not large enough to reach each point, scale the ellipse up.
            double scale = Math.Max(run / (trueSize.Width * 2), rise / (trueSize.Height * 2));
            if (scale > 1)
            {
                // The center point can be determined early in this situation.
                center.X = (startPoint.X + endPoint.X) / 2;
                center.Y = (startPoint.Y + endPoint.Y) / 2;

                double diffX = run / 2;
                double diffY = rise / 2;

                var angle = Math.Atan2(diffY / trueSize.Height, diffX / trueSize.Width);
                var cos = Math.Cos(angle) * trueSize.Width;
                var sin = Math.Sin(angle) * trueSize.Height;

                scale = Math.Sqrt(diffX * diffX + diffY * diffY) / Math.Sqrt(cos * cos + sin * sin);
                trueSize.Width *= scale;
                trueSize.Height *= scale;
            }

            // Scale the points down so that they make a unit circle (radius of 1).
            startPoint.X /= trueSize.Width;
            startPoint.Y /= trueSize.Height;
            endPoint.X /= trueSize.Width;
            endPoint.Y /= trueSize.Height;
            center.X /= trueSize.Width;
            center.Y /= trueSize.Height;

            // If any of the center values is NaN, the center point still needs to be found.
            if (double.IsNaN(center.X) || double.IsNaN(center.Y))
            {
                // Get the perpendicular angle and midpoint of the line between the two points.
                // Combining these two gives us the line on which the center of the ellipse is located.
                var midPoint = new Point((startPoint.X + endPoint.X) / 2, (startPoint.Y + endPoint.Y) / 2);
                var perpAngle = Math.Atan2(startPoint.Y - endPoint.Y, endPoint.X - startPoint.X);

                // With the distance between one point and the midpoint known, we have two sides of a right triangle.
                // Use Pythagorean theorem to get the length of the line between the midpoint and the center.
                double diffX = startPoint.X - midPoint.X;
                double diffY = startPoint.Y - midPoint.Y;
                double distance = Math.Sqrt(diffX * diffX + diffY * diffY);

                // Because it's a unit circle, we know c² is 1, so only the distance from point to midpoint needs to be squared.
                distance = Math.Sqrt(1 - distance * distance);

                // There will be 1 or 2 possibilities.  SweepDirection and IsLargeArc will determine which one to use.
                if ((isLargeArc && sweepDirection == SweepDirection.Counterclockwise) || (!isLargeArc && sweepDirection == SweepDirection.Clockwise))
                {
                    center = new Point(midPoint.X + Math.Sin(perpAngle) * distance, midPoint.Y + Math.Cos(perpAngle) * distance);
                }
                else
                {
                    center = new Point(midPoint.X - Math.Sin(perpAngle) * distance, midPoint.Y - Math.Cos(perpAngle) * distance);
                }
            }
            
            if (sweepDirection == SweepDirection.Counterclockwise)
            {
                // Swapping the two points so that they are clockwise makes measuring easier.
                var temp = startPoint;
                startPoint = endPoint;
                endPoint = temp;
            }

            double twoPi = Math.PI * 2;
            double startAngle = Math.Atan2(startPoint.Y - center.Y, startPoint.X - center.X);
            if (startAngle < 0)
            {
                // The angles are wrapped to ensure they fall between 0 and TwoPi.
                startAngle += twoPi;
            }

            double endAngle = Math.Atan2(endPoint.Y - center.Y, endPoint.X - center.X);
            if (endAngle < 0)
            {
                endAngle += twoPi;
            }

            // By adding the sweep angle of the arc to the start angle, we can check if the arc crosses certain thresholds.
            double arcAngle = Math.Abs(startAngle - endAngle);
            if ((arcAngle < Math.PI && isLargeArc) || (arcAngle > Math.PI && !isLargeArc))
            {
                arcAngle = twoPi - arcAngle;
            }

            Point retVal = new Point();

            // If the arc crosses the far right side (TwoPi), the radius plus center is our X value; otherwise, it's the larger X of the two points.
            if (startAngle + arcAngle > twoPi)
            {
                retVal.X = center.X * trueSize.Width + trueSize.Width;
            }
            else
            {
                retVal.X = Math.Max(startPoint.X * trueSize.Width, endPoint.X * trueSize.Width);
            }

            // Same thing for our Y value, except it's the very bottom of the ellipse we're checking.
            // To keep things simple, we're offsetting the start angle so that TwoPi is effectively at the bottom.
            startAngle -= Math.PI / 2;
            if (startAngle < 0)
            {
                startAngle += twoPi;
            }

            if (startAngle + arcAngle > twoPi)
            {
                retVal.Y = center.Y * trueSize.Height + trueSize.Height;
            }
            else
            {
                retVal.Y = Math.Max(startPoint.Y * trueSize.Height, endPoint.Y * trueSize.Height);
            }

            return retVal;
        }
    }
}
