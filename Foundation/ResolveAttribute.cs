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

namespace Prism
{
    /// <summary>
    /// Indicates that a class is a <see cref="FrameworkObject"/> with a backing implementation that needs to be resolved by the IoC container.
    /// When an instance of the class is initialized, the topmost attribute in the class's inheritance chain will be used to resolve the backing
    /// implementation.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class ResolveAttribute : Attribute
    {
        /// <summary>
        /// Gets an optional name to use when resolving the backing implementation.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the type to pass to the IoC container in order to resolve the backing implementation.
        /// </summary>
        public Type ResolveType { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResolveAttribute"/> class.
        /// </summary>
        /// <param name="resolveType">The type to give to the IoC container in order to resolve the backing implementation.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="resolveType"/> is <c>null</c>.</exception>
        public ResolveAttribute(Type resolveType)
            : this(resolveType, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResolveAttribute"/> class.
        /// </summary>
        /// <param name="resolveType">The type to pass to the IoC container in order to resolve the backing implementation.</param>
        /// <param name="name">An optional name to use when resolving the backing implementation.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="resolveType"/> is <c>null</c>.</exception>
        public ResolveAttribute(Type resolveType, string name)
        {
            if (resolveType == null)
            {
                throw new ArgumentNullException(nameof(resolveType));
            }

            ResolveType = resolveType;
            Name = name;
        }
    }
}
