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
    /// Represents a coordinate for a geographic location.
    /// </summary>
    public class Coordinate
    {
        /// <summary>
        /// Gets the altitude of the location in meters.
        /// </summary>
        public double Altitude { get; }

        /// <summary>
        /// Gets the heading in degrees relative to true north, if available.
        /// </summary>
        public double? Heading { get; }

        /// <summary>
        /// Gets the accuracy of the <see cref="P:Latitude"/> and <see cref="P:Longitude"/> values in meters, if available.
        /// </summary>
        public double? HorizontalAccuracy { get; }

        /// <summary>
        /// Gets the latitude of the location in degrees.
        /// </summary>
        public double Latitude { get; }

        /// <summary>
        /// Gets the longitude of the location in degrees.
        /// </summary>
        public double Longitude { get; }

        /// <summary>
        /// Gets the speed over the location in meters per second, if available.
        /// </summary>
        public double? Speed { get; }

        /// <summary>
        /// Gets the time at which the location was determined.
        /// </summary>
        public DateTimeOffset Timestamp { get; }

        /// <summary>
        /// Gets the accuracy of the <see cref="P:Altitude"/> value in meters, if available.
        /// </summary>
        public double? VerticalAccuracy { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Coordinate"/> class.
        /// </summary>
        /// <param name="timestamp">The time at which the location was determined.</param>
        /// <param name="latitude">The latitude of the location in degrees.</param>
        /// <param name="longitude">The longitude of the location in degrees.</param>
        /// <param name="altitude">The altitude of the location in meters.</param>
        public Coordinate(DateTimeOffset timestamp, double latitude, double longitude, double altitude)
            : this (timestamp, latitude, longitude, altitude, null, null, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Coordinate"/> class.
        /// </summary>
        /// <param name="timestamp">The time at which the location was determined.</param>
        /// <param name="latitude">The latitude of the location in degrees.</param>
        /// <param name="longitude">The longitude of the location in degrees.</param>
        /// <param name="altitude">The altitude of the location in meters.</param>
        /// <param name="heading">The heading in degrees relative to true north.</param>
        /// <param name="speed">The speed over the location in meters per second.</param>
        /// <param name="horizontalAccuracy">The accuracy of the latitude and longitude values in meters.</param>
        /// <param name="verticalAccuracy">The accuracy of the altitude value in meters.</param>
        public Coordinate(DateTimeOffset timestamp, double latitude, double longitude, double altitude,
            double? heading, double? speed, double? horizontalAccuracy, double? verticalAccuracy)
        {
            Altitude = altitude;
            Heading = heading == null || double.IsNaN(heading.Value) ? null : heading;
            HorizontalAccuracy = horizontalAccuracy == null || double.IsNaN(horizontalAccuracy.Value) ? null : horizontalAccuracy;
            Latitude = latitude;
            Longitude = longitude;
            Speed = speed == null || double.IsNaN(speed.Value) ? null : speed;
            Timestamp = timestamp;
            VerticalAccuracy = verticalAccuracy == null || double.IsNaN(verticalAccuracy.Value) ? null : verticalAccuracy;
        }
    }
}
