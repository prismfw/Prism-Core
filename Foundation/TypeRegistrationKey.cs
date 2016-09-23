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
    /// Defines an object that acts as a registration key for a <see cref="TypeManager"/> instance.
    /// </summary>
    public interface ITypeRegistrationKey
    {
        /// <summary>
        /// Gets the name of the registration key.
        /// </summary>
        string RegisteredName { get; }

        /// <summary>
        /// Gets the type of the registration key.
        /// </summary>
        Type RegisteredType { get; }
    }
}