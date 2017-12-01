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

namespace Prism.Systems.Sensors
{
    /// <summary>
    /// Represents a reading from an accelerometer.
    /// </summary>
    public class AccelerometerReading
    {
        /// <summary>
        /// Gets the acceleration along the X-axis.
        /// </summary>
        public double AccelerationX { get; }

        /// <summary>
        /// Gets the acceleration along the Y-axis.
        /// </summary>
        public double AccelerationY { get; }

        /// <summary>
        /// Gets the acceleration along the Z-axis.
        /// </summary>
        public double AccelerationZ { get; }

        /// <summary>
        /// Gets the time at which the reading took place.
        /// This value represents the number of milliseconds that the system has been awake since startup.
        /// </summary>
        public double Timestamp { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccelerometerReading"/> class.
        /// </summary>
        /// <param name="timestamp">The time at which the reading took place.</param>
        /// <param name="accelerationX">The acceleration along the X-axis.</param>
        /// <param name="accelerationY">The acceleration along the Y-axis.</param>
        /// <param name="accelerationZ">The acceleration along the Z-axis.</param>
        public AccelerometerReading(double timestamp, double accelerationX, double accelerationY, double accelerationZ)
        {
            AccelerationX = accelerationX;
            AccelerationY = accelerationY;
            AccelerationZ = accelerationZ;
            Timestamp = timestamp;
        }
    }
}
