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
using System.Reflection;

namespace Prism.Native
{
    /// <summary>
    /// Represents the base class for native platform initializers.  This class is abstract.
    /// </summary>
    public abstract class PlatformInitializer
    {
        /// <summary>
        /// Gets a value indicating whether the platform initializer has successfully initialized.
        /// </summary>
        protected static bool HasInitialized { get; private set; }

        /// <summary>
        /// Initializes the platform and loads the specified <see cref="Application"/> instance.
        /// </summary>
        /// <param name="appInstance">The application instance to be loaded.</param>
        /// <param name="appAssemblies">Every loaded assembly that is not a system assembly.  These are scanned for types that should be registered with the IoC container.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="appInstance"/> is <c>null</c>.</exception>
        protected static void Initialize(Application appInstance, Assembly[] appAssemblies)
        {
            if (appInstance == null)
            {
                throw new ArgumentNullException(nameof(appInstance));
            }

            Application.Initialize(appInstance, appAssemblies);
            HasInitialized = true;
        }
    }
}