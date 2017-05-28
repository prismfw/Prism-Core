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

#if !DEBUG
using System.Diagnostics;
#endif

namespace Prism.UI.Shapes
{
    /// <summary>
    /// Represents a straight line between two points.
    /// </summary>
    [Resolve(typeof(INativeLine))]
    public class Line : Shape
    {
        #region Property Descriptors
        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:X1"/> property.
        /// </summary>
        public static PropertyDescriptor X1Property { get; } = PropertyDescriptor.Create(nameof(X1), typeof(double), typeof(Line));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:X2"/> property.
        /// </summary>
        public static PropertyDescriptor X2Property { get; } = PropertyDescriptor.Create(nameof(X2), typeof(double), typeof(Line));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:Y1"/> property.
        /// </summary>
        public static PropertyDescriptor Y1Property { get; } = PropertyDescriptor.Create(nameof(Y1), typeof(double), typeof(Line));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:Y2"/> property.
        /// </summary>
        public static PropertyDescriptor Y2Property { get; } = PropertyDescriptor.Create(nameof(Y2), typeof(double), typeof(Line));
        #endregion

        /// <summary>
        /// Gets or sets the X-coordinate of the start point of the line.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", Justification = "Exception parameter refers to property name for easier understanding of invalid value.")]
        public double X1
        {
            get { return nativeObject.X1; }
            set
            {
                if (double.IsNaN(value) || double.IsInfinity(value))
                {
                    throw new ArgumentException(Strings.ValueCannotBeNaNOrInfinity, nameof(X1));
                }
                
                nativeObject.X1 = value;
            }
        }

        /// <summary>
        /// Gets or sets the X-coordinate of the end point of the line.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", Justification = "Exception parameter refers to property name for easier understanding of invalid value.")]
        public double X2
        {
            get { return nativeObject.X2; }
            set
            {
                if (double.IsNaN(value) || double.IsInfinity(value))
                {
                    throw new ArgumentException(Strings.ValueCannotBeNaNOrInfinity, nameof(X2));
                }

                nativeObject.X2 = value;
            }
        }

        /// <summary>
        /// Gets or sets the Y-coordinate of the start point of the line.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", Justification = "Exception parameter refers to property name for easier understanding of invalid value.")]
        public double Y1
        {
            get { return nativeObject.Y1; }
            set
            {
                if (double.IsNaN(value) || double.IsInfinity(value))
                {
                    throw new ArgumentException(Strings.ValueCannotBeNaNOrInfinity, nameof(Y1));
                }

                nativeObject.Y1 = value;
            }
        }

        /// <summary>
        /// Gets or sets the Y-coordinate of the end point of the line.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", Justification = "Exception parameter refers to property name for easier understanding of invalid value.")]
        public double Y2
        {
            get { return nativeObject.Y2; }
            set
            {
                if (double.IsNaN(value) || double.IsInfinity(value))
                {
                    throw new ArgumentException(Strings.ValueCannotBeNaNOrInfinity, nameof(Y2));
                }

                nativeObject.Y2 = value;
            }
        }

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        // this field is to avoid casting
        private readonly INativeLine nativeObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="Line"/> class.
        /// </summary>
        public Line()
            : this(ResolveParameter.EmptyParameters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Line"/> class and pairs it with the specified native object.
        /// </summary>
        /// <param name="nativeObject">The native object with which to pair this instance.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="nativeObject"/> doesn't match the type specified by the topmost <see cref="ResolveAttribute"/> in the inheritance chain.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="nativeObject"/> is <c>null</c>.</exception>
        protected Line(INativeLine nativeObject)
            : base(nativeObject)
        {
            this.nativeObject = nativeObject;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Line"/> class and pairs it with a native object that is resolved from the IoC container.
        /// </summary>
        /// <param name="resolveParameters">Any parameters to pass along to the constructor of the native type.</param>
        /// <exception cref="TypeResolutionException">Thrown when the native object does not resolve to an <see cref="INativeLine"/> instance.</exception>
        protected Line(ResolveParameter[] resolveParameters)
            : base(resolveParameters)
        {
            nativeObject = ObjectRetriever.GetNativeObject(this) as INativeLine;
            if (nativeObject == null)
            {
                throw new TypeResolutionException(string.Format(CultureInfo.CurrentCulture, Strings.TypeMustResolveToType,
                    ObjectRetriever.GetNativeObject(this).GetType().FullName, typeof(INativeLine).FullName));
            }
        }

        /// <summary>
        /// Called when this instance is ready to be measured and returns the desired size of the object.
        /// </summary>
        /// <param name="constraints">The width and height that this instance should not exceed.</param>
        /// <returns>The desired size of the object as a <see cref="Size"/> instance.</returns>
        protected override Size MeasureOverride(Size constraints)
        {
            constraints = base.MeasureOverride(constraints);
            return new Size(Math.Min(constraints.Width, Math.Max(X1, X2) + StrokeThickness * 0.5),
                Math.Min(constraints.Height, Math.Max(Y1, Y2) + StrokeThickness * 0.5));
        }
    }
}
