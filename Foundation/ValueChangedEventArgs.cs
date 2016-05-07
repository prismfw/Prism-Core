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

namespace Prism
{
    /// <summary>
    /// Provides data for events that are triggered by a change in a value.
    /// </summary>
    /// <typeparam name="T">The type of the value that was changed.</typeparam>
    public class ValueChangedEventArgs<T> : EventArgs
    {
        /// <summary>
        /// Gets the value after the change was made.
        /// </summary>
        public T NewValue { get; }

        /// <summary>
        /// Gets the value before the change was made.
        /// </summary>
        public T OldValue { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueChangedEventArgs&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="oldValue">The value before the change.</param>
        /// <param name="newValue">The value after the change.</param>
        public ValueChangedEventArgs(T oldValue, T newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}
