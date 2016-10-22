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


using Prism.UI;

namespace Prism.Native
{
    /// <summary>
    /// Defines a utility for searching an application's visual hierarchy that is native to a particular platform.
    /// These objects are meant to be paired with the platform-agnostic <see cref="VisualTreeHelper"/> object.
    /// </summary>
    [CoreBehavior(CoreBehaviors.ExpectsSingleton)]
    public interface INativeVisualTreeHelper
    {
        /// <summary>
        /// Returns the number of children in the specified object's child collection.
        /// </summary>
        /// <param name="reference">The parent object.</param>
        /// <returns>The number of children in the parent object's child collection.</returns>
        int GetChildrenCount([CoreBehavior(CoreBehaviors.ChecksNullity)]object reference);

        /// <summary>
        /// Returns the child that is located at the specified index in the child collection of the specified object.
        /// </summary>
        /// <param name="reference">The parent object.</param>
        /// <param name="childIndex">The zero-based index of the child to return.</param>
        /// <returns>The child at the specified index, or <c>null</c> if no such child exists.</returns>
        object GetChild([CoreBehavior(CoreBehaviors.ChecksNullity)]object reference, int childIndex);

        /// <summary>
        /// Returns the parent of the specified object.
        /// </summary>
        /// <param name="reference">The child object.</param>
        /// <returns>The parent object of the child, or <c>null</c> if no parent is found.</returns>
        object GetParent([CoreBehavior(CoreBehaviors.ChecksNullity)]object reference);
    }
}
