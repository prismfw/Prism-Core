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


namespace Prism.Data
{
    /// <summary>
    /// Represents the base class for <see cref="Binding"/> and <see cref="MultiBinding"/> objects.  This class is abstract.
    /// </summary>
    public abstract class BindingBase
    {
        /// <summary>
        /// Activates the binding.
        /// </summary>
        /// <param name="targetObject">The target object of the binding.</param>
        /// <param name="targetPath">The <see cref="PropertyPath"/> describing the target property of the binding.</param>
        internal virtual void Activate(object targetObject, PropertyPath targetPath) { }

        /// <summary>
        /// Deactivates the binding.
        /// </summary>
        internal virtual void Deactivate() { }
    }
}
