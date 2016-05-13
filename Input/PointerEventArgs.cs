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
using Prism.Native;
using Prism.UI.Controls;

namespace Prism.Input
{
    /// <summary>
    /// Provides data for pointer-related events such as <see cref="Element.PointerMoved"/>,
    /// <see cref="Element.PointerPressed"/>, and <see cref="Element.PointerReleased"/>.
    /// </summary>
    public class PointerEventArgs : HandledEventArgs
    {
        /// <summary>
        /// Gets the type of the pointer device that raised the event.
        /// </summary>
        public PointerType PointerType { get; }

        /// <summary>
        /// Gets the position of the pointer when the event was raised, relative to the element that raised it.
        /// </summary>
        public Point Position { get; }

        /// <summary>
        /// Gets the amount of pressure that is applied by the pointer.  A value of 1.0 indicates normal pressure.
        /// </summary>
        public double Pressure { get; }
        
        /// <summary>
        /// Gets the object that initially raised the event.
        /// </summary>
        public object Source { get; }

        /// <summary>
        /// Gets the time at which the event took place.
        /// This value represents the number of milliseconds that the system has been awake since startup.
        /// </summary>
        public long Timestamp { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PointerEventArgs"/> class.
        /// </summary>
        /// <param name="source">The object that initially raised the event.</param>
        /// <param name="pointerType">The type of the pointer device that raised the event.</param>
        /// <param name="position">The position of the pointer when the event was raised, relative to the element that raised it.</param>
        /// <param name="pressure">The amount of pressure applied by the pointer.</param>
        /// <param name="timestamp">The time at which the event took place, in milliseconds since system startup.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="pressure"/> is NaN or infinite.</exception>
        public PointerEventArgs(object source, PointerType pointerType, Point position, double pressure, long timestamp)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (double.IsNaN(pressure) || double.IsInfinity(pressure))
            {
                throw new ArgumentException(Resources.Strings.ValueCannotBeNaNOrInfinity, nameof(pressure));
            }

            Source = ObjectRetriever.GetAgnosticObject(source);
            PointerType = pointerType;
            Position = position;
            Pressure = Math.Max(pressure, 0);
            Timestamp = timestamp;
        }
    }
}
