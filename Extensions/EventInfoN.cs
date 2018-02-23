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


using System.Diagnostics;

namespace System.Reflection
{
    /// <summary>
    /// Discovers the attributes of an event and provides access to event metadata.
    /// </summary>
    public sealed class EventInfoN : MemberInfoN
    {
        /// <summary>
        /// Gets the <see cref="MethodInfoN"/> object for the <see cref="AddEventHandler(object, Delegate)"/> method of the event, including non-public methods.
        /// </summary>
        public MethodInfoN AddMethod
        {
            get
            {
                if (addMethod == null && eventInfo.AddMethod != null)
                {
                    addMethod = new MethodInfoN(eventInfo.AddMethod);
                }

                return addMethod;
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private MethodInfoN addMethod;

        /// <summary>
        /// Gets the attributes for this event.
        /// </summary>
        public EventAttributes Attributes
        {
            get { return eventInfo.Attributes; }
        }

        /// <summary>
        /// Gets the <see cref="Type"/> object of the underlying event-handler delegate associated with this event.
        /// </summary>
        public Type EventHandlerType
        {
            get { return eventInfo.EventHandlerType; }
        }

        /// <summary>
        /// Gets a value indicating whether the EventInfo has a name with a special meaning.
        /// </summary>
        public bool IsSpecialName
        {
            get { return eventInfo.IsSpecialName; }
        }

        /// <summary>
        /// Gets the method that is called when the event is raised, including non-public methods.
        /// </summary>
        public MethodInfoN RaiseMethod
        {
            get
            {
                if (raiseMethod == null && eventInfo.RaiseMethod != null)
                {
                    raiseMethod = new MethodInfoN(eventInfo.RaiseMethod);
                }

                return raiseMethod;
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private MethodInfoN raiseMethod;

        /// <summary>
        /// Gets the MethodInfo object for removing a method of the event, including non-public methods.
        /// </summary>
        public MethodInfoN RemoveMethod
        {
            get
            {
                if (removeMethod == null && eventInfo.RemoveMethod != null)
                {
                    removeMethod = new MethodInfoN(eventInfo.RemoveMethod);
                }

                return removeMethod;
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private MethodInfoN removeMethod;

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private readonly EventInfo eventInfo;

        internal EventInfoN(EventInfo eventInfo)
            : base(eventInfo)
        {
            this.eventInfo = eventInfo;
        }

        /// <summary>
        /// Adds an event handler to an event source.
        /// </summary>
        /// <param name="target">The event source.</param>
        /// <param name="handler">Encapsulates a method or methods to be invoked when the event is raised by the target.</param>
        /// <exception cref="ArgumentException">The handler that was passed in cannot be used.</exception>
        /// <exception cref="Exception">The target parameter is null and the event is not static -or- the <see cref="EventInfoN"/> is not declared on the target.</exception>
        /// <exception cref="InvalidOperationException">The event does not have a public add accessor.</exception>
        /// <exception cref="MemberAccessException">The caller does not have access permission to the member.</exception>
        public void  AddEventHandler(object target, Delegate handler)
        {
            eventInfo.AddEventHandler(GetObject(target), handler);
        }

        /// <summary>
        /// Removes an event handler from an event source.
        /// </summary>
        /// <param name="target">The event source.</param>
        /// <param name="handler">The delegate to be disassociated from the events raised by the target.</param>
        /// <exception cref="ArgumentException">The handler that was passed in cannot be used.</exception>
        /// <exception cref="Exception">The target parameter is null and the event is not static -or- the <see cref="EventInfoN"/> is not declared on the target.</exception>
        /// <exception cref="InvalidOperationException">The event does not have a public remove accessor.</exception>
        /// <exception cref="MemberAccessException">The caller does not have access permission to the member.</exception>
        public void SetValue(object target, Delegate handler)
        {
            eventInfo.RemoveEventHandler(GetObject(target), handler);
        }
    }
}
