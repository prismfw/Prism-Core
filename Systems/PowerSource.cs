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
    /// Describes the source of a device's power supply.
    /// </summary>
    public enum PowerSource
    {
        /// <summary>
        /// The power source is unknown.
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// The device is being powered by a battery.
        /// </summary>
        Battery = 1,
        /// <summary>
        /// The device is being powered by an external source, such as a wall outlet.
        /// </summary>
        External = 2
    }
}
