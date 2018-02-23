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
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Prism.Native;

namespace Prism.Systems.Sensors
{
    /// <summary>
    /// Provides access to the accelerometer of the current device.
    /// </summary>
    [Resolve(typeof(INativeAccelerometer))]
    public sealed class Accelerometer : FrameworkObject
    {
        #region Event Descriptors
        /// <summary>
        /// Gets an <see cref="EventDescriptor"/> describing the <see cref="E:ReadingChanged"/> event.
        /// </summary>
        public static EventDescriptor ReadingChangedEvent { get; } = EventDescriptor.Create(nameof(ReadingChanged), typeof(TypedEventHandler<Accelerometer, AccelerometerReadingChangedEventArgs>), typeof(Accelerometer));
        #endregion

        /// <summary>
        /// Gets the accelerometer for the current device.
        /// </summary>
        public static Accelerometer Current
        {
            get { return current.nativeObject.IsAvailable ? current : null; }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static Accelerometer current = new Accelerometer();

        /// <summary>
        /// Occurs when the reading of the accelerometer has changed.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification = "Event handler provides a strongly-typed sender for easier use.")]
        public event TypedEventHandler<Accelerometer, AccelerometerReadingChangedEventArgs> ReadingChanged;

        /// <summary>
        /// Gets or sets the amount of time, in milliseconds, that should pass between readings.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", Justification = "Exception parameter refers to property name for easier understanding of invalid value.")]
        public double UpdateInterval
        {
            get { return nativeObject.UpdateInterval; }
            set
            {
                if (double.IsInfinity(value))
                {
                    throw new ArgumentException(Resources.Strings.ValueCannotBeInfinity, nameof(UpdateInterval));
                }

                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(UpdateInterval), Resources.Strings.ValueCannotBeLessThanZero);
                }

                nativeObject.UpdateInterval = value;
            }
        }

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private readonly INativeAccelerometer nativeObject;

        private Accelerometer()
            : base(ResolveParameter.EmptyParameters)
        {
            nativeObject = ObjectRetriever.GetNativeObject(this) as INativeAccelerometer;
            if (nativeObject == null)
            {
                throw new TypeResolutionException(string.Format(CultureInfo.CurrentCulture, Resources.Strings.TypeMustResolveToType,
                    ObjectRetriever.GetNativeObject(this).GetType().FullName, typeof(INativeAccelerometer).FullName));
            }

            nativeObject.ReadingChanged += (o, e) => OnReadingChanged(e);
        }

        /// <summary>
        /// Gets the current reading of the accelerometer.
        /// </summary>
        /// <returns>The current reading of the accelerometer as an <see cref="AccelerometerReading"/> instance.</returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Depending on implementation, performance may be less than what is expected of a property.")]
        public AccelerometerReading GetCurrentReading()
        {
            return nativeObject.GetCurrentReading();
        }

        private void OnReadingChanged(AccelerometerReadingChangedEventArgs e)
        {
            ReadingChanged?.Invoke(this, e);
        }
    }
}
