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
using System.Linq;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Prism.UI.Media
{
    /// <summary>
    /// Represents a subsection of a path.  Multiple figures can be added to a path to define separate sections of the path.
    /// Each path figure may only be attached to one path at a time.
    /// </summary>
    public sealed class PathFigure : FrameworkObject
    {
        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:IsClosed"/> property.
        /// </summary>
        public static PropertyDescriptor IsClosedProperty { get; } = PropertyDescriptor.Create(nameof(IsClosed), typeof(bool), typeof(PathFigure), new FrameworkPropertyMetadata(OnPathPropertyChanged));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:StartPoint"/> property.
        /// </summary>
        public static PropertyDescriptor StartPointProperty { get; } = PropertyDescriptor.Create(nameof(StartPoint), typeof(Point), typeof(PathFigure), new FrameworkPropertyMetadata(OnPathPropertyChanged));

        /// <summary>
        /// Gets or sets a value indicating whether the first and last segments of the figure should be connected.
        /// </summary>
        public bool IsClosed
        {
            get { return isClosed; }
            set
            {
                if (value != isClosed)
                {
                    isClosed = value;
                    OnPropertyChanged(IsClosedProperty);
                }
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool isClosed;

        /// <summary>
        /// Gets a collection of the segments that define the shape of the figure.
        /// </summary>
        public PathSegmentCollection Segments { get; private set; }

        /// <summary>
        /// Gets or sets the point at which the figure begins.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", Justification = "Exception parameter refers to property name for easier understanding of invalid value.")]
        public Point StartPoint
        {
            get { return startPoint; }
            set
            {
                if (value.X != startPoint.X || value.Y != startPoint.Y)
                {
                    if (double.IsNaN(value.X) || double.IsInfinity(value.X))
                    {
                        throw new ArgumentException(Resources.Strings.ValueCannotBeNaNOrInfinity, "StartPoint.X");
                    }

                    if (double.IsNaN(value.Y) || double.IsInfinity(value.Y))
                    {
                        throw new ArgumentException(Resources.Strings.ValueCannotBeNaNOrInfinity, "StartPoint.Y");
                    }

                    startPoint = value;
                    OnPropertyChanged(StartPointProperty);
                }
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Point startPoint;

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        internal Shapes.Path Owner { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PathFigure"/> class.
        /// </summary>
        public PathFigure()
        {
            Segments = new PathSegmentCollection(this);
        }

        /// <summary>
        /// Creates a deep-copy clone of the figure.  This clone is not attached to the original
        /// figure's path and can be immediately added to any path of your choosing.
        /// </summary>
        /// <returns>The cloned object as a <see cref="PathFigure"/> instance.</returns>
        public PathFigure Clone()
        {
            var clone = (PathFigure)MemberwiseClone();

            clone.Owner = null;
            clone.Segments = new PathSegmentCollection(clone);
            clone.Segments.AddRange(Segments.Select(s => s.Clone()));

            return clone;
        }

        private static void OnPathPropertyChanged(FrameworkObject obj, PropertyDescriptor property)
        {
            (obj as PathFigure)?.Owner?.Invalidate();
        }
    }
}
