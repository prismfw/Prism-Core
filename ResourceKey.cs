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

namespace Prism
{
    /// <summary>
    /// Represents a key for a resource in a <see cref="ResourceDictionary"/>.
    /// </summary>
    public class ResourceKey
    {
        /// <summary>
        /// Gets an optional value to return when a resource for the key cannot be found.
        /// </summary>
        public object DefaultValue { get; }

        /// <summary>
        /// Gets an optional key for a fallback resource to look for if a resource for the current key cannot be found.
        /// </summary>
        public ResourceKey FallbackKey { get; }

        /// <summary>
        /// Gets the identifier for the resource.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Gets the type that the resource value must be.
        /// The resource value is only valid if it's an instance of this type or derived from this type.
        /// </summary>
        public Type TypeRestriction { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceKey"/> class.
        /// </summary>
        /// <param name="id">The identifier for the resource.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="id"/> is <c>null</c>.</exception>
        public ResourceKey(string id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            Id = id;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceKey"/> class.
        /// </summary>
        /// <param name="id">The identifier for the resource.</param>
        /// <param name="typeRestriction">The type that the resource value must be.  A value of <c>null</c> means that there is no type restriction.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="id"/> is <c>null</c>.</exception>
        public ResourceKey(string id, Type typeRestriction)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            Id = id;
            TypeRestriction = typeRestriction;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceKey"/> class.
        /// </summary>
        /// <param name="id">The identifier for the resource.</param>
        /// <param name="typeRestriction">The type that the resource value must be.  A value of <c>null</c> means that there is no type restriction.</param>
        /// <param name="defaultValue">An optional value to return when a resource for the key cannot be found.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="id"/> is <c>null</c>.</exception>
        public ResourceKey(string id, Type typeRestriction, object defaultValue)
            : this(id, typeRestriction)
        {
            DefaultValue = defaultValue;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceKey"/> class.
        /// </summary>
        /// <param name="id">The identifier for the resource.</param>
        /// <param name="typeRestriction">The type that the resource value must be.  A value of <c>null</c> means that there is no type restriction.</param>
        /// <param name="fallbackKey">An optional key for a fallback resource to look for if a resource for the current key cannot be found.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="id"/> is <c>null</c>.</exception>
        public ResourceKey(string id, Type typeRestriction, ResourceKey fallbackKey)
            : this(id, typeRestriction)
        {
            FallbackKey = fallbackKey;
        }
    }
}
