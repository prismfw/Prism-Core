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

namespace Prism
{
    /// <summary>
    /// Describes the platform on which an application is running.
    /// </summary>
    public enum Platform
    {
        /// <summary>
        /// The platform is unknown.
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// Google's Android platform, which includes phones, tablets, laptops, and desktops.
        /// </summary>
        Android = 0x1030001,
        /// <summary>
        /// Apple's iOS platform, which includes phones and tablets.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "Officially recognized name.")]
        iOS = 0x2020002,
        /// <summary>
        /// Apple's Macintosh platform, which includes laptops and desktops.
        /// </summary>
        Macintosh = 0x2010004,
        /// <summary>
        /// Microsoft's Windows platform, which includes tablets, laptops, and desktops.
        /// </summary>
        Windows = 0x4010008,
        /// <summary>
        /// Microsoft's Universal Windows platform, which includes phones, tablets, laptops, and desktops.
        /// </summary>
        UniversalWindows = 0x4030010
    }
}
