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
using Prism.Systems;

namespace Prism.Native
{
    /// <summary>
    /// Defines a device that is native to a particular platform.
    /// These objects are meant to be paired with platform-agnostic <see cref="Device"/> objects.
    /// </summary>
    public interface INativeDevice
    {
        /// <summary>
        /// Occurs when the battery level of the device has changed by at least 1 percent.
        /// </summary>
        event EventHandler BatteryLevelChanged;

        /// <summary>
        /// Occurs when the orientation of the device has changed.
        /// </summary>
        event EventHandler OrientationChanged;

        /// <summary>
        /// Occurs when the power source of the device has changed.
        /// </summary>
        event EventHandler PowerSourceChanged;

        /// <summary>
        /// Gets the battery level of the device as a percentage value between 0 (empty) and 100 (full).
        /// </summary>
        int BatteryLevel { get; }

        /// <summary>
        /// Gets the scaling factor of the display monitor.
        /// </summary>
        double DisplayScale { get; }

        /// <summary>
        /// Gets the form factor of the device on which the application is running.
        /// </summary>
        FormFactor FormFactor { get; }

        /// <summary>
        /// Gets a unique identifier for the device.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the orientation of the device should be monitored.
        /// This affects the ability to read the orientation of the device.
        /// </summary>
        bool IsOrientationMonitoringEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the power state of the device should be monitored.
        /// This affects the ability to read the power source and battery level of the device.
        /// </summary>
        bool IsPowerMonitoringEnabled { get; set; }

        /// <summary>
        /// Gets the name of the device.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the operating system that is running on the device.
        /// </summary>
        OperatingSystem OperatingSystem { get; }

        /// <summary>
        /// Gets the physical orientation of the device.
        /// </summary>
        DeviceOrientation Orientation { get; }

        /// <summary>
        /// Gets the version of the operating system that is running on the device.
        /// </summary>
        Version OSVersion { get; }

        /// <summary>
        /// Gets the source from which the device is receiving its power.
        /// </summary>
        PowerSource PowerSource { get; }
    }
}
