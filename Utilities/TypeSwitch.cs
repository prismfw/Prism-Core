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
using System.Reflection;

namespace Prism.Utilities
{
    /// <summary>
    /// Represents a switch statement for <see cref="Type"/> objects.
    /// </summary>
    public class TypeSwitch
    {
        /// <summary>
        /// Gets or sets an optional type filter to apply to the switch statement.
        /// This is useful for when the object matches several cases and you want to target a specific case.
        /// </summary>
        public Type TypeFilter { get; set; }

        /// <summary>
        /// Gets or sets the object on which the switch statement will be performed.
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeSwitch"/> class.
        /// </summary>
        public TypeSwitch()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeSwitch"/> class.
        /// </summary>
        /// <param name="value"></param>
        public TypeSwitch(object value)
        {
            TypeFilter = null;
            Value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeSwitch"/> class.
        /// </summary>
        /// <param name="value">The object on which the switch statement will be performed.</param>
        /// <param name="type">An optional type filter to apply to the switch statement.
        /// This is useful for when the object matches several cases and you want to target a specific case.</param>
        public TypeSwitch(object value, Type type)
        {
            TypeFilter = type;
            Value = value;
        }
    }

    /// <summary>
    /// Provides case statements for use with the <see cref="TypeSwitch"/> class.
    /// </summary>
    public static class TypeSwitchExtensions
    {
        /// <summary>
        /// Adds a case statement to a <see cref="TypeSwitch"/> instance.
        /// </summary>
        /// <typeparam name="T">The type to check against the object.</typeparam>
        /// <param name="typeSwitch">The current <see cref="TypeSwitch"/> instance.</param>
        /// <param name="action">The action to perform if <see cref="TypeSwitch.Value"/> is of type <typeparamref name="T"/>.</param>
        /// <returns>The current TypeSwitch to be used with chaining cases -or- <c>null</c> if the switch statement is completed.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="action"/> is <c>null</c>.</exception>
        public static TypeSwitch Case<T>(this TypeSwitch typeSwitch, Action<T> action)
        {
            return Case(typeSwitch, null, action, false);
        }

        /// <summary>
        /// Adds a case statement to a <see cref="TypeSwitch"/> instance.
        /// </summary>
        /// <typeparam name="T">The type to check against the object.</typeparam>
        /// <param name="typeSwitch">The current <see cref="TypeSwitch"/> instance.</param>
        /// <param name="action">The action to perform if <see cref="TypeSwitch.Value"/> is of type <typeparamref name="T"/>.</param>
        /// <param name="fallThrough">Whether to fall through to any following case statements.</param>
        /// <returns>The current TypeSwitch to be used with chaining cases -or- <c>null</c> if the switch statement is completed.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="action"/> is <c>null</c>.</exception>
        public static TypeSwitch Case<T>(this TypeSwitch typeSwitch, Action<T> action, bool fallThrough)
        {
            return Case(typeSwitch, null, action, fallThrough);
        }

        /// <summary>
        /// Adds a case statement to a <see cref="TypeSwitch"/> instance.
        /// </summary>
        /// <typeparam name="T">The type to check against the object.</typeparam>
        /// <param name="typeSwitch">The current <see cref="TypeSwitch"/> instance.</param>
        /// <param name="condition">An optional condition that must return <c>true</c> for the case statement to run.</param>
        /// <param name="action">The action to perform if <see cref="TypeSwitch.Value"/> is of type <typeparamref name="T"/>.</param>
        /// <returns>The current TypeSwitch to be used with chaining cases -or- <c>null</c> if the switch statement is completed.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="action"/> is <c>null</c>.</exception>
        public static TypeSwitch Case<T>(this TypeSwitch typeSwitch, Func<T, bool> condition, Action<T> action)
        {
            return Case(typeSwitch, condition, action, false);
        }

        /// <summary>
        /// Adds a case statement to a <see cref="TypeSwitch"/> instance.
        /// </summary>
        /// <typeparam name="T">The type to check against the object.</typeparam>
        /// <param name="typeSwitch">The current <see cref="TypeSwitch"/> instance.</param>
        /// <param name="condition">An optional condition that must return <c>true</c> for the case statement to run.</param>
        /// <param name="action">The action to perform if <see cref="TypeSwitch.Value"/> is of type <typeparamref name="T"/>.</param>
        /// <param name="fallThrough"><c>true</c> to fall-through to any following cases; otherwise <c>false</c>.</param>
        /// <returns>The current TypeSwitch to be used with chaining cases -or- <c>null</c> if the switch statement is completed.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="action"/> is <c>null</c>.</exception>
        public static TypeSwitch Case<T>(this TypeSwitch typeSwitch, Func<T, bool> condition, Action<T> action, bool fallThrough)
        {
            if (typeSwitch == null)
            {
                return null;
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var typedObject = typeSwitch.Value is T ? (T)typeSwitch.Value : default(T);
            var type = typeSwitch.TypeFilter;
            var caseType = typeof(T);

            if ((typedObject == null ||
                typedObject.Equals(default(T)) ||
                (condition != null && !condition(typedObject)) ||
                type != null && !caseType.GetTypeInfo().IsAssignableFrom(type.GetTypeInfo())) &&
                (type == null || !caseType.GetTypeInfo().IsAssignableFrom(type.GetTypeInfo()) || 
                (condition != null && !condition(default(T)))))
            {
                return typeSwitch;
            }

            action(typedObject);
            return fallThrough ? typeSwitch : null;
        }

        /// <summary>
        /// Adds a default statement to a <see cref="TypeSwitch"/> instance.
        /// </summary>
        /// <param name="typeSwitch">The current <see cref="TypeSwitch"/> instance.</param>
        /// <param name="action">The action to perform if no other cases have been run.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="action"/> is <c>null</c>.</exception>
        public static void Default(this TypeSwitch typeSwitch, Action<object> action)
        {
            if (typeSwitch != null)
            {
                if (action == null)
                {
                    throw new ArgumentNullException(nameof(action));
                }

                action(typeSwitch.Value);
            }
        }
    }
}