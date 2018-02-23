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


using System.Collections.Generic;
using Prism;

namespace System.Reflection
{
	/// <summary>
	/// Provides additional methods for reflecting types that allow <see cref="FrameworkObject"/>
    /// classes and their native backing implementations to be reflected upon simultaneously.
	/// </summary>
    public static class RuntimeReflectionNExtensions
    {
        /// <summary>
        /// Retrieves an object that represents the specified agnostic or native event.
        /// </summary>
        /// <param name="type">The type that contains the event.</param>
        /// <param name="name">The name of the event.</param>
        /// <param name="platforms">The platforms on which to retrieve the event information.  Platforms that are not specified will return <c>null</c>.</param>
        /// <returns>An object that represents the specified event, or <c>null</c> if the event is not found.</returns>
        public static EventInfoN GetRuntimeEvent(this Type type, string name, PlatformMask platforms)
        {
            if (((int)platforms & (int)Application.Current.Platform) == 0)
            {
                return null;
            }

            var eventInfo = type.GetRuntimeEvent(name);
            if (eventInfo == null)
            {
                var typeInfo = type.GetTypeInfo();
                if (typeInfo.IsSubclassOf(typeof(FrameworkObject)))
                {
                    var attr = typeInfo.GetCustomAttribute<ResolveAttribute>(true);
                    if (attr != null)
                    {
                        eventInfo = TypeManager.Default.GetDataForResolution(attr.ResolveType, attr.Name, false)?.ImplementationType?.GetRuntimeEvent(name);
                    }
                }
            }

            return eventInfo == null ? null : new EventInfoN(eventInfo);
        }

        /// <summary>
        /// Retrieves a collection that represents all the agnostic and native events for a specified type.
        /// </summary>
        /// <param name="type">The type that contains the events.</param>
        /// <param name="platforms">The platforms on which to retrieve the event information.  Platforms that are not specified will return an empty collection.</param>
        /// <returns>A collection of events for the specified type.</returns>
        public static IEnumerable<EventInfoN> GetRuntimeEvents(this Type type, PlatformMask platforms)
        {
            if (((int)platforms & (int)Application.Current.Platform) == 0)
            {
                yield break;
            }

            var eventInfos = type.GetRuntimeEvents();
            foreach (var info in eventInfos)
            {
                yield return new EventInfoN(info);
            }

            var typeInfo = type.GetTypeInfo();
            if (typeInfo.IsSubclassOf(typeof(FrameworkObject)))
            {
                var attr = typeInfo.GetCustomAttribute<ResolveAttribute>(true);
                if (attr != null)
                {
                    eventInfos = TypeManager.Default.GetDataForResolution(attr.ResolveType, attr.Name, false)?.ImplementationType?.GetRuntimeEvents();
                    foreach (var info in eventInfos)
                    {
                        yield return new EventInfoN(info);
                    }
                }
            }
        }

        /// <summary>
        /// Retrieves an object that represents a specified agnostic or native field.
        /// </summary>
        /// <param name="type">The type that contains the field.</param>
        /// <param name="name">The name of the field.</param>
        /// <param name="platforms">The platforms on which to retrieve the field information.  Platforms that are not specified will return <c>null</c>.</param>
        /// <returns>An object that represents the specified field, or <c>null</c> if the field is not found.</returns>
        public static FieldInfoN GetRuntimeField(this Type type, string name, PlatformMask platforms)
        {
            if (((int)platforms & (int)Application.Current.Platform) == 0)
            {
                return null;
            }

            var fieldInfo = type.GetRuntimeField(name);
            if (fieldInfo == null)
            {
                var typeInfo = type.GetTypeInfo();
                if (typeInfo.IsSubclassOf(typeof(FrameworkObject)))
                {
                    var attr = typeInfo.GetCustomAttribute<ResolveAttribute>(true);
                    if (attr != null)
                    {
                        fieldInfo = TypeManager.Default.GetDataForResolution(attr.ResolveType, attr.Name, false)?.ImplementationType?.GetRuntimeField(name);
                    }
                }
            }

            return fieldInfo == null ? null : new FieldInfoN(fieldInfo);
        }

        /// <summary>
        /// Retrieves a collection that represents all the agnostic and native fields for a specified type.
        /// </summary>
        /// <param name="type">The type that contains the fields.</param>
        /// <param name="platforms">The platforms on which to retrieve the field information.  Platforms that are not specified will return an empty collection.</param>
        /// <returns>A collection of fields for the specified type.</returns>
        public static IEnumerable<FieldInfoN> GetRuntimeFields(this Type type, PlatformMask platforms)
        {
            if (((int)platforms & (int)Application.Current.Platform) == 0)
            {
                yield break;
            }

            var fieldInfos = type.GetRuntimeFields();
            foreach (var info in fieldInfos)
            {
                yield return new FieldInfoN(info);
            }

            var typeInfo = type.GetTypeInfo();
            if (typeInfo.IsSubclassOf(typeof(FrameworkObject)))
            {
                var attr = typeInfo.GetCustomAttribute<ResolveAttribute>(true);
                if (attr != null)
                {
                    fieldInfos = TypeManager.Default.GetDataForResolution(attr.ResolveType, attr.Name, false)?.ImplementationType?.GetRuntimeFields();
                    foreach (var info in fieldInfos)
                    {
                        yield return new FieldInfoN(info);
                    }
                }
            }
        }

        /// <summary>
        /// Retrieves an object that represents a specified agnostic or native method.
        /// </summary>
        /// <param name="type">The type that contains the method.</param>
        /// <param name="name">The name of the method.</param>
        /// <param name="parameters">An array that contains the method's parameters.</param>
        /// <param name="platforms">The platforms on which to retrieve the method information.  Platforms that are not specified will return <c>null</c>.</param>
        /// <returns>An object that represents the specified method, or <c>null</c> if the method is not found.</returns>
        public static MethodInfoN GetRuntimeMethod(this Type type, string name, Type[] parameters, PlatformMask platforms)
        {
            if (((int)platforms & (int)Application.Current.Platform) == 0)
            {
                return null;
            }

            var methodInfo = type.GetRuntimeMethod(name, parameters);
            if (methodInfo == null)
            {
                var typeInfo = type.GetTypeInfo();
                if (typeInfo.IsSubclassOf(typeof(FrameworkObject)))
                {
                    var attr = typeInfo.GetCustomAttribute<ResolveAttribute>(true);
                    if (attr != null)
                    {
                        methodInfo = TypeManager.Default.GetDataForResolution(attr.ResolveType, attr.Name, false)?.ImplementationType?.GetRuntimeMethod(name, parameters);
                    }
                }
            }

            return methodInfo == null ? null : new MethodInfoN(methodInfo);
        }

        /// <summary>
        /// Retrieves a collection that represents all the agnostic and native methods for a specified type.
        /// </summary>
        /// <param name="type">The type that contains the methods.</param>
        /// <param name="platforms">The platforms on which to retrieve the method information.  Platforms that are not specified will return an empty collection.</param>
        /// <returns>A collection of methods for the specified type.</returns>
        public static IEnumerable<MethodInfoN> GetRuntimeMethods(this Type type, PlatformMask platforms)
        {
            if (((int)platforms & (int)Application.Current.Platform) == 0)
            {
                yield break;
            }

            var methodInfos = type.GetRuntimeMethods();
            foreach (var info in methodInfos)
            {
                yield return new MethodInfoN(info);
            }

            var typeInfo = type.GetTypeInfo();
            if (typeInfo.IsSubclassOf(typeof(FrameworkObject)))
            {
                var attr = typeInfo.GetCustomAttribute<ResolveAttribute>(true);
                if (attr != null)
                {
                    methodInfos = TypeManager.Default.GetDataForResolution(attr.ResolveType, attr.Name, false)?.ImplementationType?.GetRuntimeMethods();
                    foreach (var info in methodInfos)
                    {
                        yield return new MethodInfoN(info);
                    }
                }
            }
        }

        /// <summary>
        /// Retrieves a collection that represents all the agnostic and native properties for a specified type.
        /// </summary>
        /// <param name="type">The type that contains the properties.</param>
        /// <param name="platforms">The platforms on which to retrieve the property information.  Platforms that are not specified will return an empty collection.</param>
        /// <returns>A collection of properties for the specified type.</returns>
        public static IEnumerable<PropertyInfoN> GetRuntimeProperties(this Type type, PlatformMask platforms)
        {
            if (((int)platforms & (int)Application.Current.Platform) == 0)
            {
                yield break;
            }

            var propInfos = type.GetRuntimeProperties();
            foreach (var info in propInfos)
            {
                yield return new PropertyInfoN(info);
            }

            var typeInfo = type.GetTypeInfo();
            if (typeInfo.IsSubclassOf(typeof(FrameworkObject)))
            {
                var attr = typeInfo.GetCustomAttribute<ResolveAttribute>(true);
                if (attr != null)
                {
                    propInfos = TypeManager.Default.GetDataForResolution(attr.ResolveType, attr.Name, false)?.ImplementationType?.GetRuntimeProperties();
                    foreach (var info in propInfos)
                    {
                        yield return new PropertyInfoN(info);
                    }
                }
            }
        }

        /// <summary>
        /// Retrieves an object that represents a specified agnostic or native property.
        /// </summary>
        /// <param name="type">The type that contains the property.</param>
        /// <param name="name">The name of the property.</param>
        /// <param name="platforms">The platforms on which to retrieve the property information.  Platforms that are not specified will return <c>null</c>.</param>
        /// <returns>An object that represents the specified property, or <c>null</c> if the property is not found.</returns>
        public static PropertyInfoN GetRuntimeProperty(this Type type, string name, PlatformMask platforms)
        {
            if (((int)platforms & (int)Application.Current.Platform) == 0)
            {
                return null;
            }

            var propInfo = type.GetRuntimeProperty(name);
            if (propInfo == null)
            {
                var typeInfo = type.GetTypeInfo();
                if (typeInfo.IsSubclassOf(typeof(FrameworkObject)))
                {
                    var attr = typeInfo.GetCustomAttribute<ResolveAttribute>(true);
                    if (attr != null)
                    {
                        propInfo = TypeManager.Default.GetDataForResolution(attr.ResolveType, attr.Name, false)?.ImplementationType?.GetRuntimeProperty(name);
                    }
                }
            }

            return propInfo == null ? null : new PropertyInfoN(propInfo);
        }
    }
}
