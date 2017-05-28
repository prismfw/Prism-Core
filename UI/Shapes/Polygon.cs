﻿/*
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
using System.Globalization;
using Prism.Native;
using Prism.Resources;
using Prism.UI.Media;

#if !DEBUG
using System.Diagnostics;
#endif

namespace Prism.UI.Shapes
{
    /// <summary>
    /// Represents a series of connected lines that form a closed shape.
    /// </summary>
    [Resolve(typeof(INativePolygon))]
    public class Polygon : Shape
    {
        #region Property Descriptors
        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:FillRule"/> property.
        /// </summary>
        public static PropertyDescriptor FillRuleProperty { get; } = PropertyDescriptor.Create(nameof(FillRule), typeof(FillRule), typeof(Polygon));
        #endregion

        /// <summary>
        /// Gets or sets the rule to use for determining the interior fill of the shape.
        /// </summary>
        public FillRule FillRule
        {
            get { return nativeObject.FillRule; }
            set { nativeObject.FillRule = value; }
        }

        /// <summary>
        /// Gets a collection of the points that describe the vertices of the shape.
        /// </summary>
        public PointCollection Points { get; }

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        // this field is to avoid casting
        private readonly INativePolygon nativeObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="Polygon"/> class.
        /// </summary>
        public Polygon()
            : this(ResolveParameter.EmptyParameters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Polygon"/> class and pairs it with the specified native object.
        /// </summary>
        /// <param name="nativeObject">The native object with which to pair this instance.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="nativeObject"/> doesn't match the type specified by the topmost <see cref="ResolveAttribute"/> in the inheritance chain.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="nativeObject"/> is <c>null</c>.</exception>
        protected Polygon(INativePolygon nativeObject)
            : base(nativeObject)
        {
            this.nativeObject = nativeObject;

            Points = new PointCollection(this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Polygon"/> class and pairs it with a native object that is resolved from the IoC container.
        /// At least one class in the inheritance chain must be decorated with a <see cref="ResolveAttribute"/> or an exception will be thrown.
        /// </summary>
        /// <param name="resolveParameters">Any parameters to pass along to the constructor of the native type.</param>
        /// <exception cref="TypeResolutionException">Thrown when the native object does not resolve to an <see cref="INativePolygon"/> instance.</exception>
        protected Polygon(ResolveParameter[] resolveParameters)
            : base(resolveParameters)
        {
            nativeObject = ObjectRetriever.GetNativeObject(this) as INativePolygon;
            if (nativeObject == null)
            {
                throw new TypeResolutionException(string.Format(CultureInfo.CurrentCulture, Strings.TypeMustResolveToType,
                    ObjectRetriever.GetNativeObject(this).GetType().FullName, typeof(INativePolygon).FullName));
            }

            Points = new PointCollection(this);
        }

        /// <summary>
        /// Called when this instance is ready to be measured and returns the desired size of the object.
        /// </summary>
        /// <param name="constraints">The width and height that this instance should not exceed.</param>
        /// <returns>The desired size of the object as a <see cref="Size"/> instance.</returns>
        protected override Size MeasureOverride(Size constraints)
        {
            constraints = base.MeasureOverride(constraints);

            var desiredSize = new Size();
            for (int i = 0; i < Points.Count; i++)
            {
                var point = Points[i];
                desiredSize.Width = Math.Max(desiredSize.Width, point.X);
                desiredSize.Height = Math.Max(desiredSize.Height, point.Y);
            }

            desiredSize.Width = Math.Min(desiredSize.Width + StrokeThickness * 0.5, constraints.Width);
            desiredSize.Height = Math.Min(desiredSize.Height + StrokeThickness * 0.5, constraints.Height);

            return desiredSize;
        }
    }
}
