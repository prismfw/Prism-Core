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


namespace Prism.Systems
{
    /// <summary>
    /// Describes various device form factors.
    /// </summary>
    public enum FormFactor
    {
        /// <summary>
        /// The form factor is unknown or cannot be determined.
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// The device uses an undefined form factor.
        /// </summary>
        Other = 1,
        /// <summary>
        /// The device is a phone or similar device.
        /// </summary>
        Phone = 6,
        /// <summary>
        /// The device is a tablet or similar device.
        /// </summary>
        Tablet = 10,
        /// <summary>
        /// The device is a desktop computer, laptop computer, or similar device.
        /// </summary>
        Desktop = 18
    }
}
