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


using System.Diagnostics.CodeAnalysis;

namespace Prism.Native
{
    /// <summary>
    /// Defines a utility for retrieving system resources that are native to a particular platform.
    /// </summary>
    [CoreBehavior(CoreBehaviors.ExpectsSingleton)]
    public interface INativeResources
    {
        /// <summary>
        /// Gets the system resource associated with the specified key.
        /// </summary>
        /// <param name="owner">The object that owns the resource, or <c>null</c> if the resource is not owned by a specified object.</param>
        /// <param name="key">The key associated with the resource to get.</param>
        /// <param name="value">When this method returns, the value associated with the specified key, if the key is found; otherwise, <c>null</c>. This parameter is passed uninitialized.</param>
        /// <returns><c>true</c> if the system resources contain a resource with the specified key; otherwise, <c>false</c>.</returns>
        [SuppressMessage("Microsoft.Design", "CA1007:UseGenericsWhereAppropriate", Justification = "All resources are passed as objects.  Casting is not desired.")]
        bool TryGetResource(object owner, [CoreBehavior(CoreBehaviors.ChecksNullity)]object key, out object value);
    }
}
