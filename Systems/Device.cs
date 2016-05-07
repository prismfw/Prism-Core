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
    public sealed class Device : FrameworkObject
    {
        #region Event Descriptors
        /// <summary>
        /// Describes the <see cref="E:BatteryLevelChanged"/> event.  This field is read-only.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "EventDescriptor is immutable.")]
        public static readonly EventDescriptor BatteryLevelChangedEvent = EventDescriptor.Create(nameof(BatteryLevelChanged), typeof(TypedEventHandler<Device>), typeof(Device));

        /// <summary>
        /// Describes the <see cref="E:OrientationChanged"/> event.  This field is read-only.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "EventDescriptor is immutable.")]
        public static readonly EventDescriptor OrientationChangedEvent = EventDescriptor.Create(nameof(OrientationChanged), typeof(TypedEventHandler<Device>), typeof(Device));

        /// <summary>
        /// Describes the <see cref="E:PowerSourceChanged"/> event.  This field is read-only.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "EventDescriptor is immutable.")]
        public static readonly EventDescriptor PowerSourceChangedEvent = EventDescriptor.Create(nameof(PowerSourceChanged), typeof(TypedEventHandler<Device>), typeof(Device));
        #endregion

        /// <summary>
        /// Gets the current device.
        /// </summary>
        public static Device Current { get; } = new Device(typeof(INativeDevice), null);

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

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private readonly INativeDevice nativeObject;

        private Device(Type resolveType, string resolveName, params ResolveParameter[] resolveParams)
            : base(resolveType, resolveName, resolveParams)
        {
            nativeObject = ObjectRetriever.GetNativeObject(this) as INativeDevice;
            if (nativeObject == null)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.Strings.TypeMustResolveToType, resolveType.FullName, typeof(INativeDevice).FullName), nameof(resolveType));
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
