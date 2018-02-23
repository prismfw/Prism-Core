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

namespace Prism.Systems.Sensors
{
    /// <summary>
    /// Provides data for the <see cref="Accelerometer.ReadingChanged"/> event.
    /// </summary>
    public class AccelerometerReadingChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the reading of the accelerometer.
        /// </summary>
        public AccelerometerReading Reading { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccelerometerReadingChangedEventArgs"/> class.
        /// </summary>
        /// <param name="reading">The reading of the accelerometer.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="reading"/> is <c>null</c>.</exception>
        public AccelerometerReadingChangedEventArgs(AccelerometerReading reading)
        {
            if (reading == null)
            {
                throw new ArgumentNullException(nameof(reading));
            }

            Reading = reading;
        }
    }
}
