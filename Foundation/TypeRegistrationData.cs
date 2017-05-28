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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Prism
{
    /// <summary>
    /// Represents the data for a type that is registered with a <see cref="TypeManager"/>.
    /// </summary>
    public sealed class TypeRegistrationData
    {
        /// <summary>
        /// Gets a value indicating whether the registration is protected, i.e. it cannot be replaced or removed from the IoC container.
        /// </summary>
        public bool IsProtected { get; }

        /// <summary>
        /// Gets a value indicating whether the registered type represents a singleton object.
        /// </summary>
        public bool IsSingleton { get; }

        /// <summary>
        /// Gets the object instance for the registered singleton type, if applicable.
        /// </summary>
        public object SingletonInstance { get; internal set; }
        
        /// <summary>
        /// Gets the type that contains the implementation details for the registered type.
        /// </summary>
        public Type ImplementationType { get; }

        internal IEnumerable<MethodInfo> InitializationMethods { get; }

        internal TypeRegistrationData(Type implementationType, bool isProtected, bool isSingleton, object singletonInstance, string initializeMethod)
        {
            ImplementationType = implementationType;
            IsProtected = isProtected;
            IsSingleton = isSingleton;
            SingletonInstance = singletonInstance;
            InitializationMethods = initializeMethod == null ? null : implementationType.GetTypeInfo().GetDeclaredMethods(initializeMethod).Where(m => m.IsStatic);
        }
    }
}