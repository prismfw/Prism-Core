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
using Prism.Systems.Sensors;

namespace Prism.Native
{
    /// <summary>
    /// Defines an accelerometer that is native to a particular platform.
    /// These objects are meant to be paired with platform-agnostic <see cref="Accelerometer"/> objects.
    /// </summary>
    [CoreBehavior(CoreBehaviors.ExpectsSingleton)]
    public interface INativeAccelerometer
    {
        /// <summary>
        /// Occurs when the reading of the accelerometer has changed.
        /// </summary>
        event EventHandler<AccelerometerReadingChangedEventArgs> ReadingChanged;

        /// <summary>
        /// Gets a value indicating whether an accelerometer is available for the current device.
        /// </summary>
        bool IsAvailable { get; }

        /// <summary>
        /// Gets or sets the amount of time, in milliseconds, that should pass between readings.
        /// </summary>
        [CoreBehavior(CoreBehaviors.ChecksRange)]
        double UpdateInterval { get; set; }

        /// <summary>
        /// Gets the current reading of the accelerometer.
        /// </summary>
        /// <returns>The current reading of the accelerometer as an <see cref="AccelerometerReading"/> instance.</returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Depending on implementation, performance may be less than what is expected of a property.")]
        AccelerometerReading GetCurrentReading();
    }
}
