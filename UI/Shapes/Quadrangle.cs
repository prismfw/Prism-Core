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
    /// Represents a four-side shape, optionally with rounded corners.
    /// </summary>
    [Resolve(typeof(INativeQuadrangle))]
    public class Quadrangle : Shape
    {
        #region Property Descriptors
        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:RadiusX"/> property.
        /// </summary>
        public static PropertyDescriptor RadiusXProperty { get; } = PropertyDescriptor.Create(nameof(RadiusX), typeof(double), typeof(Quadrangle));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:RadiusY"/> property.
        /// </summary>
        public static PropertyDescriptor RadiusYProperty { get; } = PropertyDescriptor.Create(nameof(RadiusY), typeof(double), typeof(Quadrangle));
        #endregion

        /// <summary>
        /// Gets or sets the X-axis radius of the ellipse that is used to round the corners of the quadrangle.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", Justification = "Exception parameter refers to property name for easier understanding of invalid value.")]
        public double RadiusX
        {
            get { return nativeObject.RadiusX; }
            set
            {
                if (double.IsNaN(value) || double.IsInfinity(value))
                {
                    throw new ArgumentException(Strings.ValueCannotBeNaNOrInfinity, nameof(RadiusX));
                }

                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(RadiusX), Strings.ValueCannotBeLessThanZero);
                }

                nativeObject.RadiusX = value;
            }
        }

        /// <summary>
        /// Gets or sets the Y-axis radius of the ellipse that is used to round the corners of the quadrangle.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", Justification = "Exception parameter refers to property name for easier understanding of invalid value.")]
        public double RadiusY
        {
            get { return nativeObject.RadiusY; }
            set
            {
                if (double.IsNaN(value) || double.IsInfinity(value))
                {
                    throw new ArgumentException(Strings.ValueCannotBeNaNOrInfinity, nameof(RadiusY));
                }

                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(RadiusY), Strings.ValueCannotBeLessThanZero);
                }

                nativeObject.RadiusY = value;
            }
        }

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        // this field is to avoid casting
        private readonly INativeQuadrangle nativeObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="Quadrangle"/> class.
        /// </summary>
        public Quadrangle()
            : this(ResolveParameter.EmptyParameters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Quadrangle"/> class and pairs it with the specified native object.
        /// </summary>
        /// <param name="nativeObject">The native object with which to pair this instance.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="nativeObject"/> doesn't match the type specified by the topmost <see cref="ResolveAttribute"/> in the inheritance chain.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="nativeObject"/> is <c>null</c>.</exception>
        protected Quadrangle(INativeQuadrangle nativeObject)
            : base(nativeObject)
        {
            this.nativeObject = nativeObject;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Quadrangle"/> class and pairs it with a native object that is resolved from the IoC container.
        /// </summary>
        /// <param name="resolveParameters">Any parameters to pass along to the constructor of the native type.</param>
        /// <exception cref="TypeResolutionException">Thrown when the native object does not resolve to an <see cref="INativeQuadrangle"/> instance.</exception>
        protected Quadrangle(ResolveParameter[] resolveParameters)
            : base(resolveParameters)
        {
            nativeObject = ObjectRetriever.GetNativeObject(this) as INativeQuadrangle;
            if (nativeObject == null)
            {
                throw new TypeResolutionException(string.Format(CultureInfo.CurrentCulture, Strings.TypeMustResolveToType,
                    ObjectRetriever.GetNativeObject(this).GetType().FullName, typeof(INativeQuadrangle).FullName));
            }
        }

        /// <summary>
        /// Called when this instance is ready to be measured and returns the desired size of the object.
        /// </summary>
        /// <param name="constraints">The width and height that this instance should not exceed.</param>
        /// <returns>The desired size of the object as a <see cref="Size"/> instance.</returns>
        protected override Size MeasureOverride(Size constraints)
        {
            // The Quadrangle's size is determined entirely by its Width and Height values.
            // There is no autosizing available.
            return Size.Empty;
        }
    }
}
