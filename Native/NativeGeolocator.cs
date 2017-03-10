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
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Prism.Systems.Geolocation;

namespace Prism.Native
{
    /// <summary>
    /// Defines a geolocator that is native to a particular platform.
    /// These objects are meant to be paired with platform-agnostic <see cref="Geolocator"/> objects.
    /// </summary>
    public interface INativeGeolocator
    {
        /// <summary>
        /// Occurs when the location is updated.
        /// </summary>
        event EventHandler<GeolocationUpdatedEventArgs> LocationUpdated;

        /// <summary>
        /// Gets or sets the desired level of accuracy when reading geographic coordinates.
        /// </summary>
        GeolocationAccuracy DesiredAccuracy { get; set; }

        /// <summary>
        /// Gets or sets the minimum distance, in meters, that should be covered before the location is updated again.
        /// </summary>
        [CoreBehavior(CoreBehaviors.ChecksRange)]
        double DistanceThreshold { get; set; }

        /// <summary>
        /// Gets or sets the amount of time, in milliseconds, that should pass before the location is updated again.
        /// </summary>
        [CoreBehavior(CoreBehaviors.ChecksRange)]
        double UpdateInterval { get; set; }

        /// <summary>
        /// Signals the geolocation service to begin listening for location updates.
        /// </summary>
        void BeginLocationUpdates();

        /// <summary>
        /// Signals the geolocation service to stop listening for location updates.
        /// </summary>
        void EndLocationUpdates();

        /// <summary>
        /// Makes a singular request to the geolocation service for the current location.
        /// </summary>
        /// <returns>A <see cref="Coordinate"/> representing the current location.</returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Asynchronous nature of method makes property inappropriate.")]
        Task<Coordinate> GetCoordinateAsync();

        /// <summary>
        /// Requests access to the device's geolocation service.
        /// </summary>
        /// <returns><c>true</c> if access is granted; otherwise, <c>false</c>.</returns>
        Task<bool> RequestAccessAsync();
    }
}
