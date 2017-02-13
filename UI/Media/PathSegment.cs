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

#if !DEBUG
using System.Diagnostics;
#endif

namespace Prism.UI.Media
{
    /// <summary>
    /// Represents an individual segment within a <see cref="PathFigure"/>.  Each segment may only be attached to one path figure at a time. 
    /// </summary>
    public abstract class PathSegment : FrameworkObject
    {
        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:EndPoint"/> property.
        /// </summary>
        public static PropertyDescriptor EndPointProperty { get; } = PropertyDescriptor.Create(nameof(EndPoint), typeof(Point), typeof(PathSegment), new FrameworkPropertyMetadata(OnPathPropertyChanged));

        /// <summary>
        /// Gets or sets the point at which the segment ends.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", Justification = "Exception parameter refers to property name for easier understanding of invalid value.")]
        public Point EndPoint
        {
            get { return endPoint; }
            set
            {
                if (value.X != endPoint.X || value.Y != endPoint.Y)
                {
                    if (double.IsNaN(value.X) || double.IsInfinity(value.X))
                    {
                        throw new ArgumentException(Resources.Strings.ValueCannotBeNaNOrInfinity, "EndPoint.X");
                    }

                    if (double.IsNaN(value.Y) || double.IsInfinity(value.Y))
                    {
                        throw new ArgumentException(Resources.Strings.ValueCannotBeNaNOrInfinity, "EndPoint.Y");
                    }

                    endPoint = value;
                    OnPropertyChanged(EndPointProperty);
                }
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Point endPoint;

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        internal PathFigure Owner { get; set; }

        internal PathSegment()
        {
        }

        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", Justification = "Exception parameter refers to argument property name for easier understanding of invalid value.")]
        internal PathSegment(Point endPoint)
        {
            if (double.IsNaN(endPoint.X) || double.IsInfinity(endPoint.X))
            {
                throw new ArgumentException(Resources.Strings.ValueCannotBeNaNOrInfinity, "endPoint.X");
            }

            if (double.IsNaN(endPoint.Y) || double.IsInfinity(endPoint.Y))
            {
                throw new ArgumentException(Resources.Strings.ValueCannotBeNaNOrInfinity, "endPoint.Y");
            }

            this.endPoint = endPoint;
        }

        /// <summary>
        /// Creates a deep-copy clone of the segment.  This clone is not attached to the original
        /// segment's path figure and can be immediately added to any path of your choosing.
        /// </summary>
        /// <returns>The cloned object as a <see cref="PathSegment"/> instance.</returns>
        public PathSegment Clone()
        {
            var clone = (PathSegment)MemberwiseClone();
            clone.Owner = null;
            return clone;
        }

        internal abstract Point GetBottomLeftCorner(Point startPoint);

        internal static void OnPathPropertyChanged(FrameworkObject obj, PropertyDescriptor property)
        {
            (obj as PathSegment)?.Owner?.Owner?.Invalidate();
        }
    }
}
