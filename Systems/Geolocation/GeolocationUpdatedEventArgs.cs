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

namespace Prism.Systems.Geolocation
{
    /// <summary>
    /// Provides data for the <see cref="Geolocator.LocationUpdated"/> event.
    /// </summary>
    public class GeolocationUpdatedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the geographic coordinate describing the location.
        /// </summary>
        public Coordinate Coordinate { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeolocationUpdatedEventArgs"/> class.
        /// </summary>
        /// <param name="coordinate">The geographic coordinate describing the location.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="coordinate"/> is <c>null</c>.</exception>
        public GeolocationUpdatedEventArgs(Coordinate coordinate)
        {
            if (coordinate == null)
            {
                throw new ArgumentNullException(nameof(coordinate));
            }

            Coordinate = coordinate;
        }
    }
}
