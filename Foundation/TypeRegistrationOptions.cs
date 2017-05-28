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
    /// Describes certain behaviorial characteristics pertaining to registration operations for a <see cref="TypeManager"/> instance.
    /// </summary>
    [Flags]
    public enum TypeRegistrationOptions
    {
        /// <summary>
        /// No options are specified.
        /// </summary>
        None = 0,
        /// <summary>
        /// Throw an <see cref="ArgumentException"/> if a registration already exists for the provided type and name.
        /// </summary>
        ThrowIfExists = 1,
        /// <summary>
        /// If a registration already exists for the provided type and name, then abort the registration process.
        /// If this option is not specified, any existing registration will be overwritten.
        /// </summary>
        SkipIfExists = 2,
        /// <summary>
        /// The registration will not be able to be removed or replaced with a different entry.
        /// Attempting to replace a protected registration will throw an <see cref="ArgumentException"/>.
        /// </summary>
        Protect = 4,
    }
}
