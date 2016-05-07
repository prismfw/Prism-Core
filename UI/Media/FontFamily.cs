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
using System.Globalization;
using Prism.Native;

#if !DEBUG
using System.Diagnostics;
#endif

namespace Prism.UI.Media
{
    /// <summary>
    /// Represents a family of fonts.
    /// </summary>
    public sealed class FontFamily : FrameworkObject
    {
        /// <summary>
        /// Gets the name of the font family.
        /// </summary>
        public string Name
        {
            get { return nativeObject.Name; }
        }

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        // this field is to avoid casting
        private readonly INativeFontFamily nativeObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="FontFamily"/> class.
        /// </summary>
        /// <param name="familyName">The family name of the font.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="familyName"/> is <c>null</c>.</exception>
        public FontFamily(string familyName)
            : this(typeof(INativeFontFamily), null, new ResolveParameter(nameof(familyName), familyName), new ResolveParameter("traits", null, true))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FontFamily"/> class.
        /// </summary>
        /// <param name="familyName">The family name of the font.</param>
        /// <param name="traits">Any special traits to assist in defining the font.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="familyName"/> is <c>null</c>.</exception>
        public FontFamily(string familyName, string traits)
            : this(typeof(INativeFontFamily), null, new ResolveParameter(nameof(familyName), familyName), new ResolveParameter(nameof(traits), traits, true))
        {
        }

        private FontFamily(Type resolveType, string resolveName, params ResolveParameter[] resolveParams)
            : base(resolveType, resolveName, resolveParams)
        {
            nativeObject = ObjectRetriever.GetNativeObject(this) as INativeFontFamily;
            if (nativeObject == null)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.Strings.TypeMustResolveToType, resolveType.FullName, typeof(INativeFontFamily).FullName), nameof(resolveType));
            }
        }
    }
}
