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
using Prism.Utilities;

namespace Prism.Native
{
    /// <summary>
    /// Defines a timer that is native to a particular platform.
    /// These objects are meant to be paired with platform-agnostic <see cref="Timer"/> objects.
    /// </summary>
    public interface INativeTimer
    {
        /// <summary>
        /// Occurs when the number of milliseconds specified by <see cref="Interval"/> have passed.
        /// </summary>
        event EventHandler Elapsed;

        /// <summary>
        /// Gets or sets a value indicating whether the timer should restart after each interval.
        /// </summary>
        bool AutoReset { get; set; }

        /// <summary>
        /// Gets or sets the amount of time, in milliseconds, before the <see cref="Elapsed"/> event is fired.
        /// </summary>
        [CoreBehavior(CoreBehaviors.ChecksRange)]
        double Interval { get; set; }

        /// <summary>
        /// Gets a value indicating whether the timer is current running.
        /// </summary>
        bool IsRunning { get; }

        /// <summary>
        /// Starts the timer.
        /// </summary>
        void StartTimer();

        /// <summary>
        /// Stops the timer.
        /// </summary>
        void StopTimer();
    }
}
