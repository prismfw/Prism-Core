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

namespace Prism.Utilities
{
    /// <summary>
    /// Represents an object that executes a method after a specified interval.
    /// </summary>
    [Resolve(typeof(INativeTimer))]
    public sealed class Timer : FrameworkObject
    {
        /// <summary>
        /// Occurs when the number of milliseconds specified by <see cref="Interval"/> have passed.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification = "Event handler provides a strongly-typed sender for easier use.")]
        public event TypedEventHandler<Timer> Elapsed;

        /// <summary>
        /// Gets or sets a value indicating whether the timer should restart after each interval.
        /// </summary>
        public bool AutoReset
        {
            get { return nativeObject.AutoReset; }
            set { nativeObject.AutoReset = value; }
        }

        /// <summary>
        /// Gets or sets the amount of time, in milliseconds, before the <see cref="Elapsed"/> event is fired.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", Justification = "Exception parameter refers to property name for easier understanding of invalid value.")]
        public double Interval
        {
            get { return nativeObject.Interval; }
            set
            {
                if (double.IsInfinity(value) || double.IsNaN(value))
                {
                    throw new ArgumentException(Resources.Strings.ValueCannotBeNaNOrInfinity, nameof(Interval));
                }

                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(Interval), Resources.Strings.ValueCannotBeLessThanZero);
                }

                nativeObject.Interval = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the timer is currently running.
        /// </summary>
        public bool IsRunning
        {
            get { return nativeObject.IsRunning; }
        }

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private readonly INativeTimer nativeObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="Timer"/> class.
        /// </summary>
        public Timer()
            : base(ResolveParameter.EmptyParameters)
        {
            nativeObject = ObjectRetriever.GetNativeObject(this) as INativeTimer;
            if (nativeObject == null)
            {
                throw new TypeResolutionException(string.Format(CultureInfo.CurrentCulture, Resources.Strings.TypeMustResolveToType,
                    ObjectRetriever.GetNativeObject(this).GetType().FullName, typeof(INativeTimer).FullName));
            }

            nativeObject.Elapsed += (o, e) => OnElapsed(e);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Timer"/> class.
        /// </summary>
        /// <param name="interval">The amount of time, in milliseconds, before the <see cref="Elapsed"/> event is fired.</param>
        public Timer(double interval)
            : this()
        {
            Interval = interval;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Timer"/> class.
        /// </summary>
        /// <param name="interval">The amount of time, in milliseconds, before the <see cref="Elapsed"/> event is fired.</param>
        /// <param name="autoReset">Whether the timer should restart after each interval.</param>
        public Timer(double interval, bool autoReset)
            : this()
        {
            Interval = interval;
            AutoReset = autoReset;
        }

        /// <summary>
        /// Starts the timer.
        /// </summary>
        public void Start()
        {
            nativeObject.StartTimer();
        }

        /// <summary>
        /// Stops the timer.
        /// </summary>
        public void Stop()
        {
            nativeObject.StopTimer();
        }

        private void OnElapsed(EventArgs e)
        {
            Elapsed?.Invoke(this, e);
        }
    }
}
