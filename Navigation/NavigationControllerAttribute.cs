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
    /// Indicates that a class implements the <see cref="IController"/> interface and can be loaded through the navigation system.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public sealed class NavigationControllerAttribute : Attribute
    {
        /// <summary>
        /// Gets a value indicating whether to use the same instance of the controller for every load.
        /// </summary>
        public bool IsSingleton { get; }

        /// <summary>
        /// Gets the URI pattern of the controller.
        /// </summary>
        public string UriPattern { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationControllerAttribute"/> class.
        /// </summary>
        /// <param name="uriPattern">The URI pattern of the controller.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="uriPattern"/> is <c>null</c>.</exception>
        public NavigationControllerAttribute(string uriPattern)
            : this(uriPattern, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationControllerAttribute"/> class.
        /// </summary>
        /// <param name="isSingleton">Whether to use the same instance of the controller for every load.</param>
        public NavigationControllerAttribute(bool isSingleton)
        {
            IsSingleton = isSingleton;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationControllerAttribute"/> class.
        /// </summary>
        /// <param name="uriPattern">The URI pattern of the controller.</param>
        /// <param name="isSingleton">Whether to use the same instance of the controller for every load.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="uriPattern"/> is <c>null</c>.</exception>
        public NavigationControllerAttribute(string uriPattern, bool isSingleton)
        {
            if (uriPattern == null)
            {
                throw new ArgumentNullException(nameof(uriPattern));
            }

            UriPattern = uriPattern;
            IsSingleton = isSingleton;
        }
    }
}
