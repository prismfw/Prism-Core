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
using System.Globalization;
using Prism.Native;
using Prism.UI.Media;

#if !DEBUG
using System.Diagnostics;
#endif

namespace Prism.UI.Controls
{
    /// <summary>
    /// Represents an item in an <see cref="ActionMenu"/> and is the base class for the
    /// <see cref="MenuButton"/> and <see cref="MenuSeparator"/> classes.  This class is abstract.
    /// </summary>
    public abstract class MenuItem : FrameworkObject
    {
        #region Property Descriptors
        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:Foreground"/> property.
        /// </summary>
        public static PropertyDescriptor ForegroundProperty { get; } = PropertyDescriptor.Create(nameof(Foreground), typeof(Brush), typeof(MenuItem));
        #endregion

        /// <summary>
        /// Gets or sets the <see cref="Brush"/> to apply to the foreground content of the menu item.
        /// </summary>
        public Brush Foreground
        {
            get { return nativeObject.Foreground; }
            set { nativeObject.Foreground = value; }
        }

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private readonly INativeMenuItem nativeObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="MenuItem"/> class and pairs it with the specified native object.
        /// </summary>
        /// <param name="nativeObject">The native object with which to pair this instance.</param>
        /// <exception cref="ArgumentException">Thrown when a <see cref="ResolveAttribute"/> is located in the inheritance chain and <paramref name="nativeObject"/> doesn't match the type specified by the attribute.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="nativeObject"/> is <c>null</c>.</exception>
        protected MenuItem(INativeMenuItem nativeObject)
            : base(nativeObject)
        {
            this.nativeObject = nativeObject;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MenuItem"/> class and pairs it with a native object that is resolved from the IoC container.
        /// At least one class in the inheritance chain must be decorated with a <see cref="ResolveAttribute"/> or an exception will be thrown.
        /// </summary>
        /// <param name="resolveParameters">Any parameters to pass along to the constructor of the native type.</param>
        /// <exception cref="TypeResolutionException">Thrown when the native object does not resolve to an <see cref="INativeMenuItem"/> instance.</exception>
        protected MenuItem(ResolveParameter[] resolveParameters)
            : base(resolveParameters)
        {
            nativeObject = ObjectRetriever.GetNativeObject(this) as INativeMenuItem;
            if (nativeObject == null)
            {
                throw new TypeResolutionException(string.Format(CultureInfo.CurrentCulture, Resources.Strings.TypeMustResolveToType,
                    ObjectRetriever.GetNativeObject(this).GetType().FullName, typeof(INativeMenuItem).FullName));
            }
        }
    }
}
