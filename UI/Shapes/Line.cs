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
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Prism.Native;
using Prism.Resources;

namespace Prism.UI.Shapes
{
    /// <summary>
    /// Represents a straight line between two points.
    /// </summary>
    public class Line : Shape
    {
        #region Property Descriptors
        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:EndPoint"/> property.
        /// </summary>
        public static PropertyDescriptor EndPointProperty { get; } = PropertyDescriptor.Create(nameof(EndPoint), typeof(Point), typeof(Line));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:StartPoint"/> property.
        /// </summary>
        public static PropertyDescriptor StartPointProperty { get; } = PropertyDescriptor.Create(nameof(StartPoint), typeof(Point), typeof(Line));
        #endregion

        /// <summary>
        /// Gets or sets the end point of the line.
        /// </summary>
        public Point EndPoint
        {
            get { return nativeObject.EndPoint; }
            set { nativeObject.EndPoint = value; }
        }

        /// <summary>
        /// Gets or sets the start point of the line.
        /// </summary>
        public Point StartPoint
        {
            get { return nativeObject.StartPoint; }
            set { nativeObject.StartPoint = value; }
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
            : this(typeof(INativeLine), null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Line"/> class.
        /// </summary>
        /// <param name="resolveType">The type to pass to the IoC container in order to resolve the native object.</param>
        /// <param name="resolveName">An optional name to use when resolving the native object.</param>
        /// <param name="resolveParameters">Any parameters to pass along to the constructor of the resolve type.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="resolveType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="resolveType"/> does not resolve to an <see cref="INativeLine"/> instance.</exception>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", Justification = "resolveType is validated in base constructor.")]
        protected Line(Type resolveType, string resolveName, params ResolveParameter[] resolveParameters)
            : base(resolveType, resolveName, resolveParameters)
        {
            nativeObject = ObjectRetriever.GetNativeObject(this) as INativeLine;
            if (nativeObject == null)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Strings.TypeMustResolveToType, resolveType.FullName, typeof(INativeLine).FullName), nameof(resolveType));
            }
        }
    }
}
