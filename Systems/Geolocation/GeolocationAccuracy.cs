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


namespace Prism.Systems.Geolocation
{
    /// <summary>
    /// Describes the desired accuracy of a geolocation service.
    /// </summary>
    public enum GeolocationAccuracy
    {
        /// <summary>
        /// A lower accuracy, typically within 500 meters.
        /// Use this when a general location is acceptable and power consumption is a concern.
        /// </summary>
        Approximate = 0,
        /// <summary>
        /// A higher accuracy, typically within 10 meters.
        /// Use this when a precise location is needed and power consumption is not a concern.
        /// </summary>
        Precise = 1
    }
}
