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
using System.Diagnostics;
using Prism.Native;

namespace Prism
{
    /// <summary>
    /// Represents a key for a resource in a <see cref="ResourceDictionary"/>.
    /// </summary>
    public class ResourceKey
    {
        /// <summary>
        /// Gets the type of resource that is expected to be returned with this key.
        /// </summary>
        public Type ExpectedType { get; }

        /// <summary>
        /// Gets an identifier for the resource.
        /// </summary>
        public int Id { get; }
        
        internal ResourceKey(SystemResourceKeyId id, Type expectedType)
        {
            Debug.Assert(id > SystemResourceKeyId.StartMarker && id < SystemResourceKeyId.EndMarker, "Undefined resource ID!");
            Debug.Assert(expectedType != null);

            ExpectedType = expectedType;
            Id = (int)id;
        }

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to the current <see cref="ResourceKey"/>.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to compare with the current <see cref="ResourceKey"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="object"/> is equal to the current <see cref="ResourceKey"/>; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            var key = obj as ResourceKey;
            return key == null ? false : key.Id == Id;
        }

        /// <summary>
        /// Serves as a hash function for a <see cref="ResourceKey"/> object.
        /// </summary>
        /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a hash table.</returns>
        public override int GetHashCode()
        {
            return Id;
        }
    }
}
