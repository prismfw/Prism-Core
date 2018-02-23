/*
Copyright (C) 2018  Prism Framework Team

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

namespace Prism.UI.Controls
{
    /// <summary>
    /// Represents an item separator in an <see cref="ActionMenu"/>.
    /// </summary>
    [Resolve(typeof(INativeMenuSeparator))]
    public class MenuSeparator : MenuItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MenuSeparator"/> class.
        /// </summary>
        public MenuSeparator()
            : this(ResolveParameter.EmptyParameters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MenuSeparator"/> class and pairs it with the specified native object.
        /// </summary>
        /// <param name="nativeObject">The native object with which to pair this instance.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="nativeObject"/> doesn't match the type specified by the topmost <see cref="ResolveAttribute"/> in the inheritance chain.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="nativeObject"/> is <c>null</c>.</exception>
        protected MenuSeparator(INativeMenuSeparator nativeObject)
            : base(nativeObject)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MenuSeparator"/> class and pairs it with a native object that is resolved from the IoC container.
        /// </summary>
        /// <param name="resolveParameters">Any parameters to pass along to the constructor of the native type.</param>
        /// <exception cref="TypeResolutionException">Thrown when the native object does not resolve to an <see cref="INativeMenuSeparator"/> instance.</exception>
        protected MenuSeparator(ResolveParameter[] resolveParameters)
            : base(resolveParameters)
        {
            if (!(ObjectRetriever.GetNativeObject(this) is INativeMenuSeparator))
            {
                throw new TypeResolutionException(string.Format(CultureInfo.CurrentCulture, Resources.Strings.TypeMustResolveToType,
                    ObjectRetriever.GetNativeObject(this).GetType().FullName, typeof(INativeMenuSeparator).FullName));
            }
        }
    }
}
