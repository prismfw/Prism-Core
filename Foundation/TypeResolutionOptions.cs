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
    /// Describes certain behaviorial characteristics pertaining to resolution operations for a <see cref="TypeManager"/> instance.
    /// </summary>
    [Flags]
    public enum TypeResolutionOptions
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
        /// Force the creation of a new instance, even if the type was registered as a singleton.
        /// Be aware of potential side effects when utilizing this option for singletons.
        /// </summary>
        CreateNew = 2,
        /// <summary>
        /// If a registration with the exact name cannot be found, use fuzzy resolution logic to find
        /// a substitute registration with the same type and an inexact name.
        /// </summary>
        UseFuzzyNameResolution = 4,
        /// <summary>
        /// If a constructor or static initialize method with the exact parameters cannot be found, use fuzzy
        /// resolution logic to find a substitute constructor or static initialize method with inexact parameters.
        /// </summary>
        UseFuzzyParameterResolution = 8
    }
}
