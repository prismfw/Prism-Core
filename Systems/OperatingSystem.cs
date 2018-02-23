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


using System.Diagnostics.CodeAnalysis;

namespace Prism.Systems
{
    /// <summary>
    /// Describes the operating system on a device.
    /// </summary>
    public enum OperatingSystem
    {
        /// <summary>
        /// The operating system is unknown.
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// Google's operating system for phones and tablets as well as desktop and laptop devices.
        /// </summary>
        Android = 0x91,
        /// <summary>
        /// Apple's operating system for mobile devices such as iPhone, iPod, and iPad.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "Officially recognized name.")]
        iOS = 0xA2,
        /// <summary>
        /// Apple's operating system for desktop and laptop devices.
        /// </summary>
        MacOS = 0xA1,
        /// <summary>
        /// Microsoft's operating system for desktop and laptop devices.
        /// </summary>
        Windows = 0xC1
    }
}
