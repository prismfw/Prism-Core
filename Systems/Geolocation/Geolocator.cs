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
using System.Globalization;
using System.Threading.Tasks;
using Prism.Native;

#if !DEBUG
using System.Diagnostics;
#endif

namespace Prism.Systems.Geolocation
{
    /// <summary>
    /// Provides access to the geolocation service of the current device.
    /// </summary>
    [Resolve(typeof(INativeGeolocator))]
    public sealed class Geolocator : FrameworkObject
    {
        #region Event Descriptors
        /// <summary>
        /// Gets an <see cref="EventDescriptor"/> describing the <see cref="E:LocationUpdated"/> event.
        /// </summary>
        public static EventDescriptor LocationUpdatedEvent { get; } = EventDescriptor.Create(nameof(LocationUpdated), typeof(TypedEventHandler<Geolocator, GeolocationUpdatedEventArgs>), typeof(Geolocator));
        #endregion

        /// <summary>
        /// Gets the current geolocator.
        /// </summary>
        public static Geolocator Current { get; } = new Geolocator();

        /// <summary>
        /// Occurs when the location is updated.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification = "Event handler provides a strongly-typed sender for easier use.")]
        public event TypedEventHandler<Geolocator, GeolocationUpdatedEventArgs> LocationUpdated;

        /// <summary>
        /// Gets or sets the desired level of accuracy when reading geographic coordinates.
        /// </summary>
        public GeolocationAccuracy DesiredAccuracy
        {
            get { return nativeObject.DesiredAccuracy; }
            set { nativeObject.DesiredAccuracy = value; }
        }

        /// <summary>
        /// Gets or sets the minimum distance, in meters, that should be covered before the location is updated again.  See Remarks.
        /// </summary>
        /// <remarks>
        /// Because of inconsistencies in platform behavior, it is not recommended to use this property simultaneously with <see cref="P:UpdateInterval"/>.
        /// Instead, choose either one or the other.  It's still technically possible to make use of both properties, but if you decide to do so, be aware
        /// of the differences in behavior across different platforms.  Some platforms may trigger an update as soon as the distance threshold or time
        /// interval is met, whichever one happens first.  Others may only trigger once both have been met.  Still others may completely ignore the time
        /// interval and only trigger once the distance threshold has been met, or vice versa.  If you are using both properties, be sure to thoroughly
        /// test each platform that you are targeting in order to ensure that the behavior you get is desired.
        /// </remarks>
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", Justification = "Exception parameter refers to property name for easier understanding of invalid value.")]
        public double DistanceThreshold
        {
            get { return nativeObject.DistanceThreshold; }
            set
            {
                if (double.IsInfinity(value))
                {
                    throw new ArgumentException(Resources.Strings.ValueCannotBeInfinity, nameof(DistanceThreshold));
                }

                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(DistanceThreshold), Resources.Strings.ValueCannotBeLessThanZero);
                }

                nativeObject.DistanceThreshold = value;
            }
        }

        /// <summary>
        /// Gets or sets the amount of time, in milliseconds, that should pass before the location is updated again.
        /// </summary>
        /// <remarks>
        /// Because of inconsistencies in platform behavior, it is not recommended to use this property simultaneously with <see cref="P:DistanceThreshold"/>.
        /// Instead, choose either one or the other.  It's still technically possible to make use of both properties, but if you decide to do so, be aware
        /// of the differences in behavior across different platforms.  Some platforms may trigger an update as soon as the distance threshold or time
        /// interval is met, whichever one happens first.  Others may only trigger once both have been met.  Still others may completely ignore the time
        /// interval and only trigger once the distance threshold has been met, or vice versa.  If you are using both properties, be sure to thoroughly
        /// test each platform that you are targeting in order to ensure that the behavior you get is desired.
        /// </remarks>
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
        private readonly INativeGeolocator nativeObject;

        private Geolocator()
            : base(ResolveParameter.EmptyParameters)
        {
            nativeObject = ObjectRetriever.GetNativeObject(this) as INativeGeolocator;
            if (nativeObject == null)
            {
                throw new TypeResolutionException(string.Format(CultureInfo.CurrentCulture, Resources.Strings.TypeMustResolveToType,
                    ObjectRetriever.GetNativeObject(this).GetType().FullName, typeof(INativeGeolocator).FullName));
            }

            nativeObject.LocationUpdated += (o, e) => OnLocationUpdated(e);
        }

        /// <summary>
        /// Signals the geolocation service to begin listening for location updates.
        /// </summary>
        public void BeginLocationUpdates()
        {
            nativeObject.BeginLocationUpdates();
        }

        /// <summary>
        /// Signals the geolocation service to stop listening for location updates.
        /// </summary>
        public void EndLocationUpdates()
        {
            nativeObject.EndLocationUpdates();
        }

        /// <summary>
        /// Makes a singular request to the geolocation service for the current location.
        /// </summary>
        /// <returns>A <see cref="Coordinate"/> representing the current location.</returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Asynchronous nature of method makes property inappropriate.")]
        public Task<Coordinate> GetCoordinateAsync()
        {
            return nativeObject.GetCoordinateAsync();
        }

        /// <summary>
        /// Requests access to the device's geolocation service.
        /// </summary>
        /// <returns><c>true</c> if access is granted; otherwise, <c>false</c>.</returns>
        public Task<bool> RequestAccessAsync()
        {
            return nativeObject.RequestAccessAsync();
        }

        private void OnLocationUpdated(GeolocationUpdatedEventArgs e)
        {
            LocationUpdated?.Invoke(this, e);
        }
    }
}
