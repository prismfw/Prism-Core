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
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

#if !DEBUG
using System.Diagnostics;
#endif

namespace Prism
{
    /// <summary>
    /// Represents a description of an event member.
    /// </summary>
    public sealed class EventDescriptor
    {
        /// <summary>
        /// Gets the type of handler for the event.
        /// </summary>
        public Type HandlerType
        {
            get { return eventInfo.EventHandlerType; }
        }

        /// <summary>
        /// Gets the name of the event.
        /// </summary>
        public string Name
        {
            get { return eventInfo.Name; }
        }

        /// <summary>
        /// Gets the type of which the event is a member.
        /// </summary>
        public Type OwnerType
        {
            get { return eventInfo.DeclaringType; }
        }

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private static readonly Dictionary<Type, List<EventDescriptor>> currentDescriptors = new Dictionary<Type, List<EventDescriptor>>();

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private readonly EventInfo eventInfo;
        
        private EventDescriptor(string name, Type handlerType, Type ownerType)
        {
            eventInfo = ownerType.GetRuntimeEvents().FirstOrDefault(e => e.Name == name && e.EventHandlerType == handlerType);
            if (eventInfo == null)
            {
                throw new MissingMemberException(string.Format(CultureInfo.CurrentCulture, Resources.Strings.CannotLocateEventWithNameAndHandlerForType, name, handlerType.Name, ownerType.FullName));
            }
        }

        /// <summary>
        /// Creates an <see cref="EventDescriptor"/> for the specified event.
        /// </summary>
        /// <param name="name">The name of the event.</param>
        /// <param name="handlerType">The type of handler for the event.</param>
        /// <param name="ownerType">The type of which the event is a member.</param>
        /// <returns>The newly created <see cref="EventDescriptor"/> instance.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="name"/> is <c>null</c>, an empty string, or a whitespace-only string.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="handlerType"/> is <c>null</c> -or- when <paramref name="ownerType"/> is <c>null</c>.</exception>
        /// <exception cref="MissingMemberException">Thrown when an event with the specified name and type cannot be located for the <paramref name="ownerType"/>.</exception>
        /// <exception cref="InvalidOperationException">Thrown when a descriptor for the specified event has already been created.</exception>
        public static EventDescriptor Create(string name, Type handlerType, Type ownerType)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException(Resources.Strings.ValueCannotBeNullEmptyOrWhitespace, nameof(name));
            }

            if (handlerType == null)
            {
                throw new ArgumentNullException(nameof(handlerType));
            }

            if (ownerType == null)
            {
                throw new ArgumentNullException(nameof(ownerType));
            }

            List<EventDescriptor> descriptors;
            if (!currentDescriptors.TryGetValue(ownerType, out descriptors))
            {
                descriptors = new List<EventDescriptor>();
                currentDescriptors[ownerType] = descriptors;
            }

            if (descriptors.Any(d => d.Name == name && d.HandlerType == handlerType))
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.Strings.EventDescriptorAlreadyCreated, name, ownerType.FullName));
            }

            var descriptor = new EventDescriptor(name, handlerType, ownerType);
            descriptors.Add(descriptor);
            return descriptor;
        }

        internal void AddHandler(object obj, Delegate handler)
        {
            eventInfo.AddEventHandler(obj, handler);
        }

        internal void RemoveHandler(object obj, Delegate handler)
        {
            eventInfo.RemoveEventHandler(obj, handler);
        }
    }
}
