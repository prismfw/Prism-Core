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

namespace Prism
{
    /// <summary>
    /// Describes certain behaviorial characteristics pertaining to unregistration operations for a <see cref="TypeManager"/> instance.
    /// </summary>
    [Flags]
    public enum TypeUnregistrationOptions
    {
        /// <summary>
        /// No options are specified.
        /// </summary>
        None = 0,
        /// <summary>
        /// Throw an <see cref="ArgumentException"/> if there isn't an existing registration for the provided type and name.
        /// </summary>
        ThrowIfNotRegistered = 1,
        /// <summary>
        /// Unregister all entries of the same type, regardless of name.
        /// </summary>
        RemoveAllOfType = 2,
        /// <summary>
        /// Unregister all entries with the same name, regardless of type.
        /// </summary>
        RemoveAllWithName = 4
    }
}
