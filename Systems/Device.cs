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
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Prism.Native;

#if !DEBUG
using System.Diagnostics;
#endif

namespace Prism.Systems
{
    /// <summary>
    /// Provides information about the device on which the application is running.
    /// </summary>
    [Resolve(typeof(INativeDevice))]
    public sealed class Device : FrameworkObject
    {
        #region Event Descriptors
        /// <summary>
        /// Gets an <see cref="EventDescriptor"/> describing the <see cref="E:BatteryLevelChanged"/> event.
        /// </summary>
        public static EventDescriptor BatteryLevelChangedEvent { get; } = EventDescriptor.Create(nameof(BatteryLevelChanged), typeof(TypedEventHandler<Device>), typeof(Device));

        /// <summary>
        /// Gets an <see cref="EventDescriptor"/> describing the <see cref="E:OrientationChanged"/> event.
        /// </summary>
        public static EventDescriptor OrientationChangedEvent { get; } = EventDescriptor.Create(nameof(OrientationChanged), typeof(TypedEventHandler<Device>), typeof(Device));

        /// <summary>
        /// Gets an <see cref="EventDescriptor"/> describing the <see cref="E:PowerSourceChanged"/> event.
        /// </summary>
        public static EventDescriptor PowerSourceChangedEvent { get; } = EventDescriptor.Create(nameof(PowerSourceChanged), typeof(TypedEventHandler<Device>), typeof(Device));
        #endregion

        /// <summary>
        /// Gets the current device.
        /// </summary>
        public static Device Current { get; } = new Device();

        /// <summary>
        /// Occurs when the battery level of the device has changed by at least 1 percent.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification = "Event handler provides a strongly-typed sender for easier use.")]
        public event TypedEventHandler<Device> BatteryLevelChanged;

        /// <summary>
        /// Occurs when the orientation of the device has changed.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification = "Event handler provides a strongly-typed sender for easier use.")]
        public event TypedEventHandler<Device> OrientationChanged;

        /// <summary>
        /// Occurs when the power source of the device has changed.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification = "Event handler provides a strongly-typed sender for easier use.")]
        public event TypedEventHandler<Device> PowerSourceChanged;

        /// <summary>
        /// Gets the battery level of the device as a percentage value between 0 (empty) and 100 (full).
        /// </summary>
        public int BatteryLevel
        {
            get { return nativeObject.BatteryLevel; }
        }

        /// <summary>
        /// Gets the scaling factor of the display monitor.
        /// </summary>
        public double DisplayScale
        {
            get { return nativeObject.DisplayScale; }
        }

        /// <summary>
        /// Gets the form factor of the device on which the application is running.
        /// </summary>
        public FormFactor FormFactor
        {
            get { return nativeObject.FormFactor; }
        }

        /// <summary>
        /// Gets a unique identifier for the device.
        /// </summary>
        public string Id
        {
            get { return nativeObject.Id; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the orientation of the device should be monitored.
        /// This affects the ability to read the <see cref="P:Orientation"/> of the device.
        /// </summary>
        public bool IsOrientationMonitoringEnabled
        {
            get { return nativeObject.IsOrientationMonitoringEnabled; }
            set { nativeObject.IsOrientationMonitoringEnabled = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the power state of the device should be monitored.
        /// This affects the ability to read the <see cref="P:PowerSource"/> and <see cref="P:BatteryLevel"/> of the device.
        /// </summary>
        public bool IsPowerMonitoringEnabled
        {
            get { return nativeObject.IsPowerMonitoringEnabled; }
            set { nativeObject.IsPowerMonitoringEnabled = value; }
        }

        /// <summary>
        /// Gets the model of the device.
        /// </summary>
        public string Model
        {
            get { return nativeObject.Model; }
        }

        /// <summary>
        /// Gets the name of the device.
        /// </summary>
        public string Name
        {
            get { return nativeObject.Name; }
        }

        /// <summary>
        /// Gets the operating system that is running on the device.
        /// </summary>
        public OperatingSystem OperatingSystem
        {
            get { return nativeObject.OperatingSystem; }
        }

        /// <summary>
        /// Gets the physical orientation of the device.
        /// </summary>
        public DeviceOrientation Orientation
        {
            get { return nativeObject.Orientation; }
        }

        /// <summary>
        /// Gets the version of the operating system that is running on the device.
        /// </summary>
        public Version OSVersion
        {
            get { return nativeObject.OSVersion; }
        }

        /// <summary>
        /// Gets the source from which the device is receiving its power.
        /// </summary>
        public PowerSource PowerSource
        {
            get { return nativeObject.PowerSource; }
        }

        /// <summary>
        /// Gets the amount of time, in milliseconds, that the system has been awake since it was last restarted.
        /// </summary>
        public long SystemUptime
        {
            get { return nativeObject.SystemUptime; }
        }

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private readonly INativeDevice nativeObject;

        private Device()
            : base(ResolveParameter.EmptyParameters)
        {
            nativeObject = ObjectRetriever.GetNativeObject(this) as INativeDevice;
            if (nativeObject == null)
            {
                throw new TypeResolutionException(string.Format(CultureInfo.CurrentCulture, Resources.Strings.TypeMustResolveToType,
                    ObjectRetriever.GetNativeObject(this).GetType().FullName, typeof(INativeDevice).FullName));
            }

            nativeObject.BatteryLevelChanged += (o, e) => OnBatteryLevelChanged(e);
            nativeObject.OrientationChanged += (o, e) => OnOrientationChanged(e);
            nativeObject.PowerSourceChanged += (o, e) => OnPowerSourceChanged(e);
        }

        private void OnBatteryLevelChanged(EventArgs e)
        {
            BatteryLevelChanged?.Invoke(this, e);
        }

        private void OnOrientationChanged(EventArgs e)
        {
            OrientationChanged?.Invoke(this, e);
        }

        private void OnPowerSourceChanged(EventArgs e)
        {
            PowerSourceChanged?.Invoke(this, e);
        }
    }
}
