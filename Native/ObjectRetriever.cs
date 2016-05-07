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
using System.Runtime.CompilerServices;

#if !DEBUG
using System.Diagnostics;
#endif

namespace Prism.Native
{
    /// <summary>
    /// Provides methods for retrieving the native or agnostic counterpart for an agnostic or native object, respectively.
    /// </summary>
    public static class ObjectRetriever
    {
#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private readonly static ConditionalWeakTable<object, object> agnostics = new ConditionalWeakTable<object, object>();
#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private readonly static ConditionalWeakTable<object, object> natives = new ConditionalWeakTable<object, object>();

        /// <summary>
        /// Gets the agnostic object that is paired with the specified object.
        /// </summary>
        /// <param name="nativeObject">The object for which to retrieve the agnostic component.</param>
        /// <returns>
        /// The agnostic object that is paired with the specified object.  This may be the
        /// same object as what is provided if the given object is agnostic or is not paired.
        /// </returns>
        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "object", Justification = "'Object' is not meant to signify a type.")]
        public static object GetAgnosticObject(object nativeObject)
        {
            if (nativeObject == null)
            {
                return null;
            }

            object agnosticObject;
            agnostics.TryGetValue(nativeObject, out agnosticObject);
            return agnosticObject ?? nativeObject;
        }

        /// <summary>
        /// Gets the native object that is paired with the specified object.
        /// </summary>
        /// <param name="agnosticObject">The object for which to retrieve the native component.</param>
        /// <returns>
        /// The native object that is paired with the specified object.  This may be the
        /// same object as what is provided if the given object is native or is not paired.
        /// </returns>
        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "object", Justification = "'Object' is not meant to signify a type.")]
        public static object GetNativeObject(object agnosticObject)
        {
            if (agnosticObject == null)
            {
                return null;
            }

            object nativeObject;
            natives.TryGetValue(agnosticObject, out nativeObject);
            return nativeObject ?? agnosticObject;
        }

        internal static void SetPair(object agnosticObject, object nativeObject)
        {
            if (agnosticObject == null)
            {
                throw new ArgumentNullException(nameof(agnosticObject));
            }

            if (nativeObject == null)
            {
                throw new ArgumentNullException(nameof(nativeObject));
            }

            agnostics.Add(nativeObject, agnosticObject);
            natives.Add(agnosticObject, nativeObject);
        }
    }
}
