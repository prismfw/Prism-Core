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
using System.Diagnostics.CodeAnalysis;
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
    /// Represents the base class for shape elements.  This class is abstract.
    /// </summary>
    public abstract class Shape : Element
    {
        #region Property Descriptors
        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:Fill"/> property.
        /// </summary>
        public static PropertyDescriptor FillProperty { get; } = PropertyDescriptor.Create(nameof(Fill), typeof(Brush), typeof(Shape));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:Stroke"/> property.
        /// </summary>
        public static PropertyDescriptor StrokeProperty { get; } = PropertyDescriptor.Create(nameof(Stroke), typeof(Brush), typeof(Shape));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:StrokeLineCap"/> property.
        /// </summary>
        public static PropertyDescriptor StrokeLineCapProperty { get; } = PropertyDescriptor.Create(nameof(StrokeLineCap), typeof(LineCap), typeof(Shape));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:StrokeLineJoin"/> property.
        /// </summary>
        public static PropertyDescriptor StrokeLineJoinProperty { get; } = PropertyDescriptor.Create(nameof(StrokeLineJoin), typeof(LineJoin), typeof(Shape));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:StrokeMiterLimit"/> property.
        /// </summary>
        public static PropertyDescriptor StrokeMiterLimitProperty { get; } = PropertyDescriptor.Create(nameof(StrokeMiterLimit), typeof(double), typeof(Shape));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:StrokeThickness"/> property.
        /// </summary>
        public static PropertyDescriptor StrokeThicknessProperty { get; } = PropertyDescriptor.Create(nameof(StrokeThickness), typeof(double), typeof(Shape));
        #endregion

        /// <summary>
        /// Gets or sets the <see cref="Brush"/> to apply to the interior of the shape.
        /// </summary>
        public Brush Fill
        {
            get { return nativeObject.Fill; }
            set { nativeObject.Fill = value; }
        }

        /// <summary>
        /// Gets or sets the <see cref="Brush"/> to apply to the outline of the shape.
        /// </summary>
        public Brush Stroke
        {
            get { return nativeObject.Stroke; }
            set { nativeObject.Stroke = value; }
        }

        /// <summary>
        /// Gets or sets the manner in which the ends of a line are drawn.
        /// </summary>
        public LineCap StrokeLineCap
        {
            get { return nativeObject.StrokeLineCap; }
            set { nativeObject.StrokeLineCap = value; }
        }

        /// <summary>
        /// Gets or sets the manner in which the connections between different lines are drawn.
        /// </summary>
        public LineJoin StrokeLineJoin
        {
            get { return nativeObject.StrokeLineJoin; }
            set { nativeObject.StrokeLineJoin = value; }
        }

        /// <summary>
        /// Gets or sets the miter limit for connecting lines.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", Justification = "Exception parameter refers to property name for easier understanding of invalid value.")]
        public double StrokeMiterLimit
        {
            get { return nativeObject.StrokeMiterLimit; }
            set
            {
                if (double.IsNaN(value) || double.IsInfinity(value))
                {
                    throw new ArgumentException(Strings.ValueCannotBeNaNOrInfinity, nameof(StrokeMiterLimit));
                }

                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(StrokeMiterLimit), Strings.ValueCannotBeLessThanZero);
                }

                nativeObject.StrokeMiterLimit = value;
            }
        }

        /// <summary>
        /// Gets or sets the width of the shape's outline.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", Justification = "Exception parameter refers to property name for easier understanding of invalid value.")]
        public double StrokeThickness
        {
            get { return nativeObject.StrokeThickness; }
            set
            {
                if (double.IsNaN(value) || double.IsInfinity(value))
                {
                    throw new ArgumentException(Strings.ValueCannotBeNaNOrInfinity, nameof(StrokeThickness));
                }

                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(StrokeThickness), Strings.ValueCannotBeLessThanZero);
                }

                nativeObject.StrokeThickness = value;
            }
        }

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        // this field is to avoid casting
        private readonly INativeShape nativeObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="Shape"/> class.
        /// </summary>
        /// <param name="resolveType">The type to pass to the IoC container in order to resolve the native object.</param>
        /// <param name="resolveName">An optional name to use when resolving the native object.</param>
        /// <param name="resolveParameters">Any parameters to pass along to the constructor of the resolve type.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="resolveType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="resolveType"/> does not resolve to an <see cref="INativeShape"/> instance.</exception>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", Justification = "resolveType is validated in base constructor.")]
        protected Shape(Type resolveType, string resolveName, params ResolveParameter[] resolveParameters)
            : base(resolveType, resolveName, resolveParameters)
        {
            nativeObject = ObjectRetriever.GetNativeObject(this) as INativeShape;
            if (nativeObject == null)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Strings.TypeMustResolveToType, resolveType.FullName, typeof(INativeShape).FullName), nameof(resolveType));
            }

            StrokeLineCap = LineCap.Flat;
            StrokeLineJoin = LineJoin.Miter;
            StrokeMiterLimit = 1;
            StrokeThickness = 2;
        }

        /// <summary>
        /// Sets the dash pattern to be used when drawing the outline of the shape.
        /// </summary>
        /// <param name="pattern">An array of values that defines the dash pattern.  Each value represents the length of a dash, alternating between "on" and "off".</param>
        /// <param name="offset">The distance within the dash pattern where dashes begin.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="offset"/> is not a number or is infinite.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="offset"/> is less than zero.</exception>
        public void SetStrokeDashPattern(double[] pattern, double offset)
        {
            if (double.IsNaN(offset) || double.IsInfinity(offset))
            {
                throw new ArgumentException(Strings.ValueCannotBeNaNOrInfinity, nameof(offset));
            }

            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(offset), Strings.ValueCannotBeLessThanZero);
            }

            nativeObject.SetStrokeDashPattern(pattern, offset);
        }
    }
}
