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
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

#if !DEBUG
using System.Diagnostics;
#endif

namespace Prism.Utilities
{
    /// <summary>
    /// Represents a utility for managing weakly-referenced event handlers, allowing the subscribers of the event to be garbage collected before the event's publisher.
    /// </summary>
    public class WeakEventManager
    {
#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private readonly EventInfo eventInfo;
#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private readonly ConditionalWeakTable<object, List<Tuple<WeakReference, MethodInfo>>> handlers = new ConditionalWeakTable<object, List<Tuple<WeakReference, MethodInfo>>>();
#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private readonly Delegate invoker;

        /// <summary>
        /// Initializes a new instance of the <see cref="WeakEventManager"/> class.
        /// </summary>
        /// <param name="event">The <see cref="EventDescriptor"/> describing the event to which weakly-referenced handlers will be attached.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="event"/> is <c>null</c>.</exception>
        public WeakEventManager(EventDescriptor @event)
        {
            if (@event == null)
            {
                throw new ArgumentNullException(nameof(@event));
            }

            eventInfo = @event.OwnerType.GetRuntimeEvents().FirstOrDefault(e => e.Name == @event.Name && e.EventHandlerType == @event.HandlerType);
            invoker = typeof(WeakEventManager).GetTypeInfo().GetDeclaredMethod(nameof(OnEventRaised)).CreateDelegate(eventInfo.EventHandlerType, this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WeakEventManager"/> class.
        /// </summary>
        /// <param name="eventName">The name of the event.</param>
        /// <param name="ownerType">The type of which the event is member.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="eventName"/> is <c>null</c> -or- when <paramref name="ownerType"/> is <c>null</c>.</exception>
        /// <exception cref="MissingMemberException">Thrown when an event with the specified name cannot be located for the <paramref name="ownerType"/></exception>
        public WeakEventManager(string eventName, Type ownerType)
        {
            if (ownerType == null)
            {
                throw new ArgumentNullException(nameof(ownerType));
            }

            if (eventName == null)
            {
                throw new ArgumentNullException(nameof(eventName));
            }

            eventInfo = ownerType.GetRuntimeEvent(eventName);
            if (eventInfo == null)
            {
                throw new MissingMemberException(string.Format(CultureInfo.CurrentCulture, Resources.Strings.CannotLocateEventWithNameForType, eventName, ownerType.FullName));
            }

            invoker = typeof(WeakEventManager).GetTypeInfo().GetDeclaredMethod(nameof(OnEventRaised)).CreateDelegate(eventInfo.EventHandlerType, this);
        }

        /// <summary>
        /// Adds a weakly-referenced handler to the managed event.
        /// </summary>
        /// <param name="eventSource">The source object of the event, or <c>null</c> if the event is static.</param>
        /// <param name="handler">The handler to add to the event.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="handler"/> is <c>null</c>.</exception>
        public void AddHandler(object eventSource, Delegate handler)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }
            
            List<Tuple<WeakReference, MethodInfo>> list;
            if (!handlers.TryGetValue(eventSource ?? eventInfo.DeclaringType, out list))
            {
                handlers.Add(eventSource ?? eventInfo.DeclaringType, (list = new List<Tuple<WeakReference, MethodInfo>>()));
                eventInfo.AddEventHandler(eventSource, invoker);
            }
            list.Add(new Tuple<WeakReference, MethodInfo>(new WeakReference(handler.Target), handler.GetMethodInfo()));
        }

        /// <summary>
        /// Removes all handlers from the event that have been added through the manager.
        /// Handlers that have been added by other means will not be removed.
        /// </summary>
        /// <param name="eventSource">The source object of the event, or <c>null</c> if the event is static.</param>
        public void RemoveAllHandlers(object eventSource)
        {
            List<Tuple<WeakReference, MethodInfo>> list;
            if (handlers.TryGetValue(eventSource ?? eventInfo.DeclaringType, out list))
            {
                list.Clear();
                eventInfo.RemoveEventHandler(eventSource, invoker);
            }
        }

        /// <summary>
        /// Removes the specified handler from the event.
        /// </summary>
        /// <param name="eventSource">The source object of the event, or <c>null</c> if the event is static.</param>
        /// <param name="handler">The handler to remove from the event.</param>
        /// <returns><c>true</c> if the handler was successfully removed; otherwise, <c>false</c>.</returns>
        public bool RemoveHandler(object eventSource, Delegate handler)
        {
            if (handler == null)
            {
                return false;
            }

            bool retval = false;
            
            List<Tuple<WeakReference, MethodInfo>> list;
            if (handlers.TryGetValue(eventSource ?? eventInfo.DeclaringType, out list))
            {
                retval = list.Remove(list.FirstOrDefault(t => t.Item1.Target == handler.Target));
                if (list.Count == 0)
                {
                    handlers.Remove(eventSource ?? eventInfo.DeclaringType);
                    eventInfo.RemoveEventHandler(eventSource, invoker);
                }
            }

            return retval;
        }

        private void OnEventRaised(object sender, object e)
        {
            List<Tuple<WeakReference, MethodInfo>> list;
            if (handlers.TryGetValue(sender ?? eventInfo.DeclaringType, out list))
            {
                var parameters = new object[] { sender, e };
                for (int i = 0; i < list.Count; i++)
                {
                    var tuple = list[i];
                    var target = tuple.Item1.Target;
                    if (target == null)
                    {
                        list.RemoveAt(i--);
                        if (list.Count == 0)
                        {
                            handlers.Remove(sender ?? eventInfo.DeclaringType);
                            eventInfo.RemoveEventHandler(sender, invoker);
                        }
                    }
                    else
                    {
                        tuple.Item2.Invoke(target, parameters);
                    }
                }
            }
        }
    }
}
