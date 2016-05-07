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
    /// Indicates that a type is an implementation that should be registered with the <see cref="Application"/>'s IoC container.
    /// The type can then be resolved through the Application's <see cref="M:Resolve"/> methods.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true, Inherited = false)]
    public sealed class RegisterAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets an optional name of a static method on the attributed implementation to use in place of a constructor for initialization.
        /// </summary>
        public string InitializeMethod { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the attributed implementation should be registered as a singleton.
        /// </summary>
        public bool IsSingleton { get; set; }

        /// <summary>
        /// Gets or sets an optional name with which to identify the attributed implementation.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets the type to register in the IoC container along with the attributed implementation.
        /// When this value is passed to a <see cref="M:Resolve"/> method, an instance of the attributed type will be returned.
        /// </summary>
        public Type RegisterType { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterAttribute"/> class.
        /// </summary>
        /// <param name="registerType">The type to register in the IoC container along with the attributed implementation.
        /// When the given type is passed to a <see cref="M:Resolve"/> method, an instance of the attributed type will be returned.
        /// This value is most commonly an interface type that is implemented by the attributed type.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="registerType"/> is <c>null</c>.</exception>
        public RegisterAttribute(Type registerType)
        {
            if (registerType == null)
            {
                throw new ArgumentNullException(nameof(registerType));
            }

            RegisterType = registerType;
        }
    }
}
