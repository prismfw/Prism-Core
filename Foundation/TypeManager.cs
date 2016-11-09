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
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Prism.Utilities;

namespace Prism
{
    /// <summary>
    /// Represents an Inversion of Control tool, allowing for registration and resolution of different types and singleton objects.
    /// </summary>
    public class TypeManager
    {
        /// <summary>
        /// Gets the <see cref="TypeManager"/> instance that is used by the framework to register and resolve various types.
        /// </summary>
        public static TypeManager Default { get; } = new TypeManager();

        /// <summary>
        /// Gets or sets the options to be used when registering a type or singleton object without any options specified.
        /// </summary>
        public TypeRegistrationOptions DefaultRegistrationOptions { get; set; }

        /// <summary>
        /// Gets or sets the options to be used when resolving a type or singleton object without any options specified.
        /// </summary>
        public TypeResolutionOptions DefaultResolutionOptions { get; set; }

        /// <summary>
        /// Gets or sets the options to be used when unregistering a type or singleton object without any options specified.
        /// </summary>
        public TypeUnregistrationOptions DefaultUnregistrationOptions { get; set; }

        /// <summary>
        /// Gets the number of registrations contained within the <see cref="TypeManager"/>.
        /// </summary>
        public int RegisteredCount
        {
            get { return entries.Count; }
        }

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private readonly Dictionary<ITypeRegistrationKey, TypeRegistrationData> entries = new Dictionary<ITypeRegistrationKey, TypeRegistrationData>(107);

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeManager"/> class.
        /// </summary>
        public TypeManager()
        {
        }

        /// <summary>
        /// Determines whether any registration with the specified type exists within the <see cref="TypeManager"/>.
        /// </summary>
        /// <param name="registerType">The type to check.</param>
        /// <returns><c>true</c> if a registration with the specified type exists; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="registerType"/> is <c>null</c>.</exception>
        public bool IsRegistered(Type registerType)
        {
            return entries.Keys.Any(key => key.RegisteredType == registerType);
        }

        /// <summary>
        /// Determines whether a registration with the specified type and name exists within the <see cref="TypeManager"/>.
        /// </summary>
        /// <param name="registerType">The type to check.</param>
        /// <param name="name">The name to check.</param>
        /// <returns><c>true</c> if a registration with the specified type and name exists; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="registerType"/> is <c>null</c>.</exception>
        public bool IsRegistered(Type registerType, string name)
        {
            if (registerType == null)
            {
                throw new ArgumentNullException(nameof(registerType));
            }

            return entries.ContainsKey(new TypeRegistrationKey(registerType, name));
        }

        /// <summary>
        /// Registers the specified type and implementation for the <see cref="M:Resolve"/> methods.
        /// A new instance of the implementation type will be returned each time the registered type is resolved.
        /// </summary>
        /// <param name="registerType">The type to register.</param>
        /// <param name="implementationType">The type to instantiate when resolving the registered type.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="registerType"/> is <c>null</c> -or- when <paramref name="implementationType"/> is <c>null</c>.</exception>
        public void Register(Type registerType, Type implementationType)
        {
            Register(registerType, null, implementationType, null, DefaultRegistrationOptions);
        }

        /// <summary>
        /// Registers the specified type and implementation for the <see cref="M:Resolve"/> methods.
        /// A new instance of the implementation type will be returned each time the registered type is resolved.
        /// </summary>
        /// <param name="registerType">The type to register.</param>
        /// <param name="implementationType">The type to instantiate when resolving the registered type.</param>
        /// <param name="initializeMethod">An optional name of a static method on the implementation type to use in place of a constructor for initializing the object.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="registerType"/> is <c>null</c> -or- when <paramref name="implementationType"/> is <c>null</c>.</exception>
        public void Register(Type registerType, Type implementationType, string initializeMethod)
        {
            Register(registerType, null, implementationType, initializeMethod, DefaultRegistrationOptions);
        }

        /// <summary>
        /// Registers the specified type and implementation for the <see cref="M:Resolve"/> methods.
        /// A new instance of the implementation type will be returned each time the registered type is resolved.
        /// </summary>
        /// <param name="registerType">The type to register.</param>
        /// <param name="implementationType">The type to instantiate when resolving the registered type.</param>
        /// <param name="initializeMethod">An optional name of a static method on the implementation type to use in place of a constructor for initializing the object.</param>
        /// <param name="options">Additional options to adhere to when performing the registration.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="registerType"/> is <c>null</c> -or- when <paramref name="implementationType"/> is <c>null</c>.</exception>
        public void Register(Type registerType, Type implementationType, string initializeMethod, TypeRegistrationOptions options)
        {
            Register(registerType, null, implementationType, initializeMethod, options);
        }

        /// <summary>
        /// Registers the specified type and implementation for the <see cref="M:Resolve"/> methods.
        /// A new instance of the implementation type will be returned each time the registered type is resolved.
        /// </summary>
        /// <param name="registerType">The type to register.</param>
        /// <param name="name">An optional unique identifier for the implementation type.</param>
        /// <param name="implementationType">The type to instantiate when resolving the registered type.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="registerType"/> is <c>null</c> -or- when <paramref name="implementationType"/> is <c>null</c>.</exception>
        public void Register(Type registerType, string name, Type implementationType)
        {
            Register(registerType, name, implementationType, null, DefaultRegistrationOptions);
        }

        /// <summary>
        /// Registers the specified type and implementation for the <see cref="M:Resolve"/> methods.
        /// A new instance of the implementation type will be returned each time the registered type is resolved.
        /// </summary>
        /// <param name="registerType">The type to register.</param>
        /// <param name="name">An optional unique identifier for the implementation type.</param>
        /// <param name="implementationType">The type to instantiate when resolving the registered type.</param>
        /// <param name="initializeMethod">An optional name of a static method on the implementation type to use in place of a constructor for initializing the object.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="registerType"/> is <c>null</c> -or- when <paramref name="implementationType"/> is <c>null</c>.</exception>
        public void Register(Type registerType, string name, Type implementationType, string initializeMethod)
        {
            Register(registerType, name, implementationType, initializeMethod, DefaultRegistrationOptions);
        }

        /// <summary>
        /// Registers the specified type and implementation for the <see cref="M:Resolve"/> methods.
        /// A new instance of the implementation type will be returned each time the registered type is resolved.
        /// </summary>
        /// <param name="registerType">The type to register.</param>
        /// <param name="name">An optional unique identifier for the implementation type.</param>
        /// <param name="implementationType">The type to instantiate when resolving the registered type.</param>
        /// <param name="initializeMethod">An optional name of a static method on the implementation type to use in place of a constructor for initializing the object.</param>
        /// <param name="options">Additional options to adhere to when performing the registration.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="registerType"/> is <c>null</c> -or- when <paramref name="implementationType"/> is <c>null</c>.</exception>
        public void Register(Type registerType, string name, Type implementationType, string initializeMethod, TypeRegistrationOptions options)
        {
            if (registerType == null)
            {
                throw new ArgumentNullException(nameof(registerType));
            }

            if (implementationType == null)
            {
                throw new ArgumentNullException(nameof(implementationType));
            }

            Register(new TypeRegistrationKey(registerType, name), implementationType, false, null, initializeMethod, options);
        }

        /// <summary>
        /// Registers the specified type and implementation for the <see cref="M:Resolve"/> methods.  The implementation type will be
        /// instantiated only once when the registered type is first resolved.  That same instance will then be returned on subsequent resolves.
        /// </summary>
        /// <param name="registerType">The type to register.</param>
        /// <param name="implementationType">The type to instantiate when resolving the registered type.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="registerType"/> is <c>null</c> -or- when <paramref name="implementationType"/> is <c>null</c>.</exception>
        public void RegisterSingleton(Type registerType, Type implementationType)
        {
            RegisterSingleton(registerType, null, implementationType, null, DefaultRegistrationOptions);
        }

        /// <summary>
        /// Registers the specified type and implementation for the <see cref="M:Resolve"/> methods.  The implementation type will be
        /// instantiated only once when the registered type is first resolved.  That same instance will then be returned on subsequent resolves.
        /// </summary>
        /// <param name="registerType">The type to register.</param>
        /// <param name="implementationType">The type to instantiate when resolving the registered type.</param>
        /// <param name="initializeMethod">An optional name of a static method on the implementation type to use in place of a constructor for initializing the object.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="registerType"/> is <c>null</c> -or- when <paramref name="implementationType"/> is <c>null</c>.</exception>
        public void RegisterSingleton(Type registerType, Type implementationType, string initializeMethod)
        {
            RegisterSingleton(registerType, null, implementationType, initializeMethod, DefaultRegistrationOptions);
        }

        /// <summary>
        /// Registers the specified type and implementation for the <see cref="M:Resolve"/> methods.  The implementation type will be
        /// instantiated only once when the registered type is first resolved.  That same instance will then be returned on subsequent resolves.
        /// </summary>
        /// <param name="registerType">The type to register.</param>
        /// <param name="implementationType">The type to instantiate when resolving the registered type.</param>
        /// <param name="initializeMethod">An optional name of a static method on the implementation type to use in place of a constructor for initializing the object.</param>
        /// <param name="options">Additional options to adhere to when performing the registration.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="registerType"/> is <c>null</c> -or- when <paramref name="implementationType"/> is <c>null</c>.</exception>
        public void RegisterSingleton(Type registerType, Type implementationType, string initializeMethod, TypeRegistrationOptions options)
        {
            RegisterSingleton(registerType, null, implementationType, initializeMethod, options);
        }

        /// <summary>
        /// Registers the specified type and implementation for the <see cref="M:Resolve"/> methods.  The implementation type will be
        /// instantiated only once when the registered type is first resolved.  That same instance will then be returned on subsequent resolves.
        /// </summary>
        /// <param name="registerType">The type to register.</param>
        /// <param name="name">An optional unique identifier for the implementation type.</param>
        /// <param name="implementationType">The type to instantiate when resolving the registered type.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="registerType"/> is <c>null</c> -or- when <paramref name="implementationType"/> is <c>null</c>.</exception>
        public void RegisterSingleton(Type registerType, string name, Type implementationType)
        {
            RegisterSingleton(registerType, name, implementationType, null, DefaultRegistrationOptions);
        }

        /// <summary>
        /// Registers the specified type and implementation for the <see cref="M:Resolve"/> methods.  The implementation type will be
        /// instantiated only once when the registered type is first resolved.  That same instance will then be returned on subsequent resolves.
        /// </summary>
        /// <param name="registerType">The type to register.</param>
        /// <param name="name">An optional unique identifier for the implementation type.</param>
        /// <param name="implementationType">The type to instantiate when resolving the registered type.</param>
        /// <param name="initializeMethod">An optional name of a static method on the implementation type to use in place of a constructor for initializing the object.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="registerType"/> is <c>null</c> -or- when <paramref name="implementationType"/> is <c>null</c>.</exception>
        public void RegisterSingleton(Type registerType, string name, Type implementationType, string initializeMethod)
        {
            RegisterSingleton(registerType, name, implementationType, initializeMethod, DefaultRegistrationOptions);
        }

        /// <summary>
        /// Registers the specified type and implementation for the <see cref="M:Resolve"/> methods.  The implementation type will be
        /// instantiated only once when the registered type is first resolved.  That same instance will then be returned on subsequent resolves.
        /// </summary>
        /// <param name="registerType">The type to register.</param>
        /// <param name="name">An optional unique identifier for the implementation type.</param>
        /// <param name="implementationType">The type to instantiate when resolving the registered type.</param>
        /// <param name="initializeMethod">An optional name of a static method on the implementation type to use in place of a constructor for initializing the object.</param>
        /// <param name="options">Additional options to adhere to when performing the registration.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="registerType"/> is <c>null</c> -or- when <paramref name="implementationType"/> is <c>null</c>.</exception>
        public void RegisterSingleton(Type registerType, string name, Type implementationType, string initializeMethod, TypeRegistrationOptions options)
        {
            if (registerType == null)
            {
                throw new ArgumentNullException(nameof(registerType));
            }

            if (implementationType == null)
            {
                throw new ArgumentNullException(nameof(implementationType));
            }

            Register(new TypeRegistrationKey(registerType, name), implementationType, true, null, initializeMethod, options);
        }

        /// <summary>
        /// Registers the specified type and object instance for the <see cref="M:Resolve"/> methods.
        /// The object will be returned when the registered type is resolved.
        /// </summary>
        /// <param name="registerType">The type to register.</param>
        /// <param name="instance">The instance to return when resolving the registered type.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="registerType"/> is <c>null</c> -or- when <paramref name="instance"/> is <c>null</c>.</exception>
        public void RegisterSingleton(Type registerType, object instance)
        {
            RegisterSingleton(registerType, null, instance, DefaultRegistrationOptions);
        }

        /// <summary>
        /// Registers the specified type and object instance for the <see cref="M:Resolve"/> methods.
        /// The object will be returned when the registered type is resolved.
        /// </summary>
        /// <param name="registerType">The key type to associate with the object instance.</param>
        /// <param name="instance">The instance to return when resolving the key type.</param>
        /// <param name="options">Additional options to adhere to when performing the registration.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="registerType"/> is <c>null</c> -or- when <paramref name="instance"/> is <c>null</c>.</exception>
        public void RegisterSingleton(Type registerType, object instance, TypeRegistrationOptions options)
        {
            RegisterSingleton(registerType, null, instance, options);
        }

        /// <summary>
        /// Registers the specified type and object instance for the <see cref="M:Resolve"/> methods.
        /// The object will be returned when the registered type is resolved.
        /// </summary>
        /// <param name="registerType">The key type to associate with the object instance.</param>
        /// <param name="name">An optional unique identifier for the object instance.</param>
        /// <param name="instance">The instance to return when resolving the key type.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="registerType"/> is <c>null</c> -or- when <paramref name="instance"/> is <c>null</c>.</exception>
        public void RegisterSingleton(Type registerType, string name, object instance)
        {
            RegisterSingleton(registerType, name, instance, DefaultRegistrationOptions);
        }

        /// <summary>
        /// Registers the specified type and object instance for the <see cref="M:Resolve"/> methods.
        /// The object will be returned when the registered type is resolved.
        /// </summary>
        /// <param name="registerType">The key type to associate with the object instance.</param>
        /// <param name="name">An optional unique identifier for the object instance.</param>
        /// <param name="instance">The instance to return when resolving the key type.</param>
        /// <param name="options">Additional options to adhere to when performing the registration.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="registerType"/> is <c>null</c> -or- when <paramref name="instance"/> is <c>null</c>.</exception>
        public void RegisterSingleton(Type registerType, string name, object instance, TypeRegistrationOptions options)
        {
            if (registerType == null)
            {
                throw new ArgumentNullException(nameof(registerType));
            }

            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            Register(new TypeRegistrationKey(registerType, name), instance.GetType(), true, instance, null, options);
        }

        /// <summary>
        /// Resolves the specified registered type as an instance of its associated implementation.
        /// </summary>
        /// <typeparam name="T">The type to resolve.</typeparam>
        /// <returns>The instance of the resolved implementation type or singleton object as an object of type <typeparamref name="T"/>.</returns>
        public T Resolve<T>()
        {
            return (T)Resolve(typeof(T), null, null, DefaultResolutionOptions);
        }

        /// <summary>
        /// Resolves the specified registered type as an instance of its associated implementation.
        /// </summary>
        /// <typeparam name="T">The type to resolve.</typeparam>
        /// <param name="parameters">An array of parameters to pass to the constructor of the associated implementation type during initialization.</param>
        /// <returns>The instance of the resolved implementation type or singleton object as an object of type <typeparamref name="T"/>.</returns>
        public T Resolve<T>(object[] parameters)
        {
            return (T)Resolve(typeof(T), null, parameters, DefaultResolutionOptions);
        }

        /// <summary>
        /// Resolves the specified registered type as an instance of its associated implementation.
        /// </summary>
        /// <typeparam name="T">The type to resolve.</typeparam>
        /// <param name="options">Additional options to adhere to when resolving the type.</param>
        /// <returns>The instance of the resolved implementation type or singleton object as an object of type <typeparamref name="T"/>.</returns>
        public T Resolve<T>(TypeResolutionOptions options)
        {
            return (T)Resolve(typeof(T), null, null, options);
        }

        /// <summary>
        /// Resolves the specified registered type as an instance of its associated implementation.
        /// </summary>
        /// <typeparam name="T">The type to resolve.</typeparam>
        /// <param name="parameters">An array of parameters to pass to the constructor of the associated implementation type during initialization.</param>
        /// <param name="options">Additional options to adhere to when resolving the type.</param>
        /// <returns>The instance of the resolved implementation type or singleton object as an object of type <typeparamref name="T"/>.</returns>
        public T Resolve<T>(object[] parameters, TypeResolutionOptions options)
        {
            return (T)Resolve(typeof(T), null, parameters, options);
        }

        /// <summary>
        /// Resolves the specified registered type as an instance of its associated implementation.
        /// </summary>
        /// <typeparam name="T">The type to resolve.</typeparam>
        /// <param name="name">An optional unique identifier for the associated implementation.</param>
        /// <returns>The instance of the resolved implementation type or singleton object as an object of type <typeparamref name="T"/>.</returns>
        public T Resolve<T>(string name)
        {
            return (T)Resolve(typeof(T), name, null, DefaultResolutionOptions);
        }

        /// <summary>
        /// Resolves the specified registered type as an instance of its associated implementation.
        /// </summary>
        /// <typeparam name="T">The type to resolve.</typeparam>
        /// <param name="name">An optional unique identifier for the associated implementation.</param>
        /// <param name="parameters">An array of parameters to pass to the constructor of the associated implementation type during initialization.</param>
        /// <returns>The instance of the resolved implementation type or singleton object as an object of type <typeparamref name="T"/>.</returns>
        public T Resolve<T>(string name, object[] parameters)
        {
            return (T)Resolve(typeof(T), name, parameters, DefaultResolutionOptions);
        }

        /// <summary>
        /// Resolves the specified registered type as an instance of its associated implementation.
        /// </summary>
        /// <typeparam name="T">The type to resolve.</typeparam>
        /// <param name="name">An optional unique identifier for the associated implementation.</param>
        /// <param name="options">Additional options to adhere to when resolving the type.</param>
        /// <returns>The instance of the resolved implementation type or singleton object as an object of type <typeparamref name="T"/>.</returns>
        public T Resolve<T>(string name, TypeResolutionOptions options)
        {
            return (T)Resolve(typeof(T), name, null, options);
        }

        /// <summary>
        /// Resolves the specified registered type as an instance of its associated implementation.
        /// </summary>
        /// <typeparam name="T">The type to resolve.</typeparam>
        /// <param name="name">An optional unique identifier for the associated implementation.</param>
        /// <param name="parameters">An array of parameters to pass to the constructor of the associated implementation type during initialization.</param>
        /// <param name="options">Additional options to adhere to when resolving the type.</param>
        /// <returns>The instance of the resolved implementation type or singleton object as an object of type <typeparamref name="T"/>.</returns>
        public T Resolve<T>(string name, object[] parameters, TypeResolutionOptions options)
        {
            return (T)Resolve(typeof(T), name, parameters, options);
        }

        /// <summary>
        /// Resolves the specified registered type as an instance of its associated implementation.
        /// </summary>
        /// <param name="resolveType">The type to resolve.</param>
        /// <returns>The instance of the resolved implementation type or singleton object.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="resolveType"/> is <c>null</c>.</exception>
        public object Resolve(Type resolveType)
        {
            return Resolve(resolveType, null, null, DefaultResolutionOptions);
        }

        /// <summary>
        /// Resolves the specified registered type as an instance of its associated implementation.
        /// </summary>
        /// <param name="resolveType">The type to resolve.</param>
        /// <param name="parameters">An array of parameters to pass to the constructor of the associated implementation during initialization.</param>
        /// <returns>The instance of the resolved implementation type or singleton object.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="resolveType"/> is <c>null</c>.</exception>
        public object Resolve(Type resolveType, object[] parameters)
        {
            return Resolve(resolveType, null, parameters, DefaultResolutionOptions);
        }

        /// <summary>
        /// Resolves the specified registered type as an instance of its associated implementation.
        /// </summary>
        /// <param name="resolveType">The type to resolve.</param>
        /// <param name="options">Additional options to adhere to when resolving the type.</param>
        /// <returns>The instance of the resolved implementation type or singleton object.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="resolveType"/> is <c>null</c>.</exception>
        public object Resolve(Type resolveType, TypeResolutionOptions options)
        {
            return Resolve(resolveType, null, null, options);
        }

        /// <summary>
        /// Resolves the specified registered type as an instance of its associated implementation.
        /// </summary>
        /// <param name="resolveType">The type to resolve.</param>
        /// <param name="parameters">An array of parameters to pass to the constructor of the associated implementation type during initialization.</param>
        /// <param name="options">Additional options to adhere to when resolving the type.</param>
        /// <returns>The instance of the resolved implementation type or singleton object.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="resolveType"/> is <c>null</c>.</exception>
        public object Resolve(Type resolveType, object[] parameters, TypeResolutionOptions options)
        {
            return Resolve(resolveType, null, parameters, options);
        }

        /// <summary>
        /// Resolves the specified registered type as an instance of its associated implementation.
        /// </summary>
        /// <param name="resolveType">The type to resolve.</param>
        /// <param name="name">An optional unique identifier for the associated implementation.</param>
        /// <returns>The instance of the resolved implementation type or singleton object.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="resolveType"/> is <c>null</c>.</exception>
        public object Resolve(Type resolveType, string name)
        {
            return Resolve(resolveType, name, null, DefaultResolutionOptions);
        }

        /// <summary>
        /// Resolves the specified registered type as an instance of its associated implementation.
        /// </summary>
        /// <param name="resolveType">The type to resolve.</param>
        /// <param name="name">An optional unique identifier for the associated implementation.</param>
        /// <param name="parameters">An array of parameters to pass to the constructor of the associated implementation type during initialization.</param>
        /// <returns>The instance of the resolved implementation type or singleton object.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="resolveType"/> is <c>null</c>.</exception>
        public object Resolve(Type resolveType, string name, object[] parameters)
        {
            return Resolve(resolveType, name, parameters, DefaultResolutionOptions);
        }

        /// <summary>
        /// Resolves the specified registered type as an instance of its associated implementation.
        /// </summary>
        /// <param name="resolveType">The type to resolve.</param>
        /// <param name="name">An optional unique identifier for the associated implementation.</param>
        /// <param name="options">Additional options to adhere to when resolving the type.</param>
        /// <returns>The instance of the resolved implementation type or singleton object.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="resolveType"/> is <c>null</c>.</exception>
        public object Resolve(Type resolveType, string name, TypeResolutionOptions options)
        {
            return Resolve(resolveType, name, null, options);
        }

        /// <summary>
        /// Resolves the specified registered type as an instance of its associated implementation.
        /// </summary>
        /// <param name="resolveType">The type to resolve.</param>
        /// <param name="name">An optional unique identifier for the associated implementation.</param>
        /// <param name="parameters">An array of parameters to pass to the constructor of the associated implementation type during initialization.</param>
        /// <param name="options">Additional options to adhere to when resolving the type.</param>
        /// <returns>The instance of the resolved implementation type or singleton object.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="resolveType"/> is <c>null</c>.</exception>
        public object Resolve(Type resolveType, string name, object[] parameters, TypeResolutionOptions options)
        {
            if (resolveType == null)
            {
                throw new ArgumentNullException(nameof(resolveType));
            }

            var typeData = GetDataForResolution(resolveType, name, options.HasFlag(TypeResolutionOptions.UseFuzzyNameResolution));
            if (typeData == null)
            {
                if (options.HasFlag(TypeResolutionOptions.ThrowIfNotRegistered))
                {
                    throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.Strings.TypeRegistrationNotFound, resolveType.FullName, name));
                }

                return null;
            }

            if (typeData.SingletonInstance != null && !options.HasFlag(TypeResolutionOptions.CreateNew))
            {
                return typeData.SingletonInstance;
            }

            object retval = null;
            if (parameters == null)
            {
                parameters = new object[0];
            }

            MethodBase method = null;
            if (typeData.InitializationMethods != null)
            {
                method = GetMethod(typeData.InitializationMethods, parameters, options.HasFlag(TypeResolutionOptions.UseFuzzyParameterResolution));
            }

            if (method == null)
            {
                method = GetMethod(typeData.ImplementationType.GetTypeInfo().DeclaredConstructors.Where(c => !c.IsStatic),
                    parameters, options.HasFlag(TypeResolutionOptions.UseFuzzyParameterResolution));
            }

            if (method == null)
            {
                try { retval = Activator.CreateInstance(typeData.ImplementationType); }
                catch (MissingMemberException) { }
            }
            else
            {
                var constructor = method as ConstructorInfo;
                var methodParams = method.GetParameters();
                if (methodParams.Length == 0)
                {
                    retval = constructor?.Invoke(null) ?? method.Invoke(null, null);
                }
                else
                {
                    if (parameters.Length != methodParams.Length)
                    {
                        var newParams = new object[methodParams.Length];
                        for (int i = 0; i < newParams.Length; i++)
                        {
                            if (i >= parameters.Length)
                            {
                                break;
                            }

                            newParams[i] = parameters[i];
                        }

                        for (int i = parameters.Length; i < newParams.Length; i++)
                        {
                            var methodParam = methodParams[i];
                            newParams[i] = methodParam.ParameterType.GetTypeInfo().IsValueType ? Activator.CreateInstance(methodParam.ParameterType) : null;
                        }

                        parameters = newParams;
                    }

                    retval = constructor?.Invoke(parameters) ?? method.Invoke(null, parameters);
                }
            }

            if (typeData.IsSingleton)
            {
                typeData.SingletonInstance = retval;
            }

            return retval;
        }

        /// <summary>
        /// Unregisters the specified type and name from the <see cref="TypeManager"/>.
        /// </summary>
        /// <param name="registerType">The type of the registration entry to remove.</param>
        /// <param name="name">The name of the registration entry to remove.</param>
        /// <exception cref="ArgumentNullException">Throw when <paramref name="registerType"/> is <c>null</c> and the default unregistration options do not allow a <c>null</c> value.</exception>
        public void Unregister(Type registerType, string name)
        {
            Unregister(registerType, name, DefaultUnregistrationOptions);
        }

        /// <summary>
        /// Unregisters the specified type and name from the <see cref="TypeManager"/>.
        /// </summary>
        /// <param name="registerType">The type of the registration entry to remove.</param>
        /// <param name="name">The name of the registration entry to remove.</param>
        /// <param name="options">Additional options to adhere to when performing the unregistration.</param>
        /// <exception cref="ArgumentNullException">Throw when <paramref name="registerType"/> is <c>null</c> and the unregistration options do not allow a <c>null</c> value.</exception>
        public void Unregister(Type registerType, string name, TypeUnregistrationOptions options)
        {
            if (registerType == null && options != TypeUnregistrationOptions.RemoveAllWithName)
            {
                throw new ArgumentNullException(nameof(registerType));
            }
            
            bool throwIfUnregistered = options.HasFlag(TypeUnregistrationOptions.ThrowIfNotRegistered);
            if (registerType != null && (options == 0 || throwIfUnregistered))
            {
                var key = GetKeyForUnregistration(registerType, name);
                if ((key == null || !entries.Remove(key)) && throwIfUnregistered)
                {
                    throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.Strings.TypeRegistrationNotFound, registerType.FullName, name));
                }
            }

            if ((int)options >= 2)
            {
                var keys = GetKeysForUnregistration(registerType, name, options.HasFlag(TypeUnregistrationOptions.RemoveAllOfType),
                    options.HasFlag(TypeUnregistrationOptions.RemoveAllWithName));

                for (int i = 0; i < keys.Length; i++)
                {
                    entries.Remove(keys[i]);
                }
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through all of the registration keys in the <see cref="TypeManager"/>.
        /// </summary>
        protected IEnumerable<ITypeRegistrationKey> GetEnumerator()
        {
            return entries.Keys;
        }

        /// <summary>
        /// Called when the registration data for the specified type and name is needed for resolution.  The value that is returned will contain the
        /// implementation details of the registration so that it may be resolved.  Derived classes can override this method for customized behavior.
        /// </summary>
        /// <param name="resolveType">The type of the registration entry that is being resolved.</param>
        /// <param name="name">The name of the registration entry that is being resolved.</param>
        /// <param name="allowFuzzyNames">Whether fuzzy comparison should be used to help with name resolution.</param>
        /// <returns>The <see cref="TypeRegistrationData"/> instance containing the implementation details of the registration.</returns>
        protected virtual TypeRegistrationData GetDataForResolution(Type resolveType, string name, bool allowFuzzyNames)
        {
            TypeRegistrationData retval;
            if (!entries.TryGetValue(new TypeRegistrationKey(resolveType, name), out retval) && allowFuzzyNames)
            {
                var key = new TypeRegistrationKey(resolveType, GetClosestName(name, entries.Keys.Where(k => k.RegisteredType == resolveType).Select(k => k.RegisteredName).ToArray()));
                entries.TryGetValue(key, out retval);

#if DEBUG
                if (retval != null)
                {
                    Logger.Debug(CultureInfo.CurrentCulture, "Exact match for name '{0}' not found.  Using substitute name '{1}' instead.", name, key.RegisteredName);
                }
#endif
            }

            return retval;
        }
        
        /// <summary>
        /// Called when a single registration key with the specified type and name is being unregistered.  The value that is returned will be removed from
        /// the <see cref="TypeManager"/> along with its associated registration data.  Derived classes can override this method for customized behavior.
        /// </summary>
        /// <param name="registerType">The registered type that is being unregistered.</param>
        /// <param name="name">The registered name that is being unregistered.</param>
        /// <returns>The <see cref="ITypeRegistrationKey"/> instance that is to be removed along with its associated registration data.</returns>
        protected virtual ITypeRegistrationKey GetKeyForUnregistration(Type registerType, string name)
        {
            return new TypeRegistrationKey(registerType, name);
        }

        /// <summary>
        /// Called when multiple registration keys with the specified type or name are being unregistered.  The values that are returned will be removed from
        /// the <see cref="TypeManager"/> along with their associated registration data.  Derived classes can override this method for customized behavior.
        /// </summary>
        /// <param name="registerType">The registered type that is being unregistered.</param>
        /// <param name="name">The registered name that is being unregistered.</param>
        /// <param name="removeAllOfType">Whether all keys with the specified type, regardless of name, are to be unregistered.</param>
        /// <param name="removeAllWithName">Whether all keys with the specified name, regardless of type, are to be unregistered.</param>
        /// <returns>An <see cref="Array"/> containing all of the keys that are to be removed along with their associated registration data.</returns>
        protected virtual ITypeRegistrationKey[] GetKeysForUnregistration(Type registerType, string name, bool removeAllOfType, bool removeAllWithName)
        {
            return entries.Keys.Where(key => (removeAllOfType && key.RegisteredType == registerType) || (removeAllWithName && key.RegisteredName == name)).ToArray();
        }

        /// <summary>
        /// Determines whether a registration with the specified key exists within the <see cref="TypeManager"/>.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns><c>true</c> if a registration with the specified key exists; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="key"/> is <c>null</c>.</exception>
        protected bool IsRegistered(ITypeRegistrationKey key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            return entries.ContainsKey(key);
        }

        /// <summary>
        /// Registers the specified key and implementation for the <see cref="M:Resolve"/> methods.
        /// A new instance of the implementation type will be returned each time the registration key is resolved.
        /// </summary>
        /// <param name="key">The key to register.</param>
        /// <param name="implementationType">The type to instantiate when resolving the registration key.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="key"/> is <c>null</c> -or- when <paramref name="implementationType"/> is <c>null</c>.</exception>
        protected void Register(ITypeRegistrationKey key, Type implementationType)
        {
            Register(key, implementationType, null, DefaultRegistrationOptions);
        }

        /// <summary>
        /// Registers the specified key and implementation for the <see cref="M:Resolve"/> methods.
        /// A new instance of the implementation type will be returned each time the registration key is resolved.
        /// </summary>
        /// <param name="key">The key to register.</param>
        /// <param name="implementationType">The type to instantiate when resolving the registration key.</param>
        /// <param name="initializeMethod">An optional name of a static method on the implementation type to use in place of a constructor for initializing the object.</param>
        /// <param name="options">Additional options to adhere to when performing the registration.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="key"/> is <c>null</c> -or- when <paramref name="implementationType"/> is <c>null</c>.</exception>
        protected void Register(ITypeRegistrationKey key, Type implementationType, string initializeMethod, TypeRegistrationOptions options)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (implementationType == null)
            {
                throw new ArgumentNullException(nameof(implementationType));
            }

            Register(key, implementationType, false, null, initializeMethod, options);
        }

        /// <summary>
        /// Registers the specified key and implementation for the <see cref="M:Resolve"/> methods.
        /// A new instance of the implementation type will be returned each time the registration key is resolved.
        /// </summary>
        /// <param name="key">The key to register.</param>
        /// <param name="implementationType">The type to instantiate when resolving the registration key.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="key"/> is <c>null</c> -or- when <paramref name="implementationType"/> is <c>null</c>.</exception>
        protected void RegisterSingleton(ITypeRegistrationKey key, Type implementationType)
        {
            RegisterSingleton(key, implementationType, null, DefaultRegistrationOptions);
        }

        /// <summary>
        /// Registers the specified key and implementation for the <see cref="M:Resolve"/> methods.
        /// A new instance of the implementation type will be returned each time the registration key is resolved.
        /// </summary>
        /// <param name="key">The key to register.</param>
        /// <param name="implementationType">The type to instantiate when resolving the registration key.</param>
        /// <param name="initializeMethod">An optional name of a static method on the implementation type to use in place of a constructor for initializing the object.</param>
        /// <param name="options">Additional options to adhere to when performing the registration.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="key"/> is <c>null</c> -or- when <paramref name="implementationType"/> is <c>null</c>.</exception>
        protected void RegisterSingleton(ITypeRegistrationKey key, Type implementationType, string initializeMethod, TypeRegistrationOptions options)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (implementationType == null)
            {
                throw new ArgumentNullException(nameof(implementationType));
            }

            Register(key, implementationType, true, null, initializeMethod, options);
        }

        /// <summary>
        /// Registers the specified key and implementation for the <see cref="M:Resolve"/> methods.
        /// A new instance of the implementation type will be returned each time the registration key is resolved.
        /// </summary>
        /// <param name="key">The key to register.</param>
        /// <param name="instance">The instance to return when resolving the key type.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="key"/> is <c>null</c> -or- when <paramref name="instance"/> is <c>null</c>.</exception>
        protected void RegisterSingleton(ITypeRegistrationKey key, object instance)
        {
            RegisterSingleton(key, instance, DefaultRegistrationOptions);
        }

        /// <summary>
        /// Registers the specified key and implementation for the <see cref="M:Resolve"/> methods.
        /// A new instance of the implementation type will be returned each time the registration key is resolved.
        /// </summary>
        /// <param name="key">The key to register.</param>
        /// <param name="instance">The instance to return when resolving the key type.</param>
        /// <param name="options">Additional options to adhere to when performing the registration.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="key"/> is <c>null</c> -or- when <paramref name="instance"/> is <c>null</c>.</exception>
        protected void RegisterSingleton(ITypeRegistrationKey key, object instance, TypeRegistrationOptions options)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            Register(key, instance.GetType(), true, instance, null, options);
        }

        /// <summary>
        /// Gets the registration data associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the registration data to get.</param>
        /// <param name="value">When this method returns, contains the registration data associated with the specified key, if the key is found; otherwise, <c>null</c>.</param>
        /// <returns><c>true</c> if the registration key was found; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="key"/> is <c>null</c>.</exception>
        protected bool TryGetData(ITypeRegistrationKey key, out TypeRegistrationData value)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            return entries.TryGetValue(key, out value);
        }

        [SuppressMessage("Microsoft.Performance", "CA1814:PreferJaggedArraysOverMultidimensional", MessageId = "Body", Justification = "Array is rectangular and does not waste space.  Use of a jagged array would mean additional, unnecessary allocations.")]
        private static string GetClosestName(string value, string[] names)
        {
            // Fuzzy equality of strings is determined by a simple Levenshtein distance algorithm.
            string retval = null;
            if (value == null || value.Length == 0)
            {
                for (int i = 0; i < names.Length; i++)
                {
                    string name = names[i];
                    if (name.Length == 0)
                    {
                        return name;
                    }

                    if (retval == null || retval.Length > name.Length)
                    {
                        retval = name;
                    }
                }

                return retval;
            }

            int distance = int.MaxValue;
            int valueLength = value.Length;
            for (int i = 0; i < names.Length; i++)
            {
                string name = names[i];
                if (name == null || name.Length == 0)
                {
                    if (retval == null || Math.Abs(valueLength - retval.Length) > valueLength)
                    {
                        retval = name;
                    }
                    continue;
                }
                
                int nameLength = name.Length;
                if ((Math.Abs(valueLength - nameLength)) >= distance)
                {
                    // Since the resulting distance is guaranteed to be greater or equal to the current distance, we can safely skip this one.
                    continue;
                }

                int[,] array = new int[valueLength + 1, nameLength + 1];

                for (int j = 1; j <= valueLength; array[j, 0] = j++) { }
                for (int j = 1; j <= nameLength; array[0, j] = j++) { }

                for (int j = 1; j <= valueLength; j++)
                {
                    for (int k = 1; k <= nameLength; k++)
                    {
                        array[j, k] = Math.Min(Math.Min(array[j - 1, k] + 1, array[j, k - 1] + 1),
                            array[j - 1, k - 1] + (value[j - 1] == name[k - 1] ? 0 : 1));
                    }
                }

                int cost = array[valueLength, nameLength];
                if (cost < distance)
                {
                    distance = cost;
                    retval = name;
                }
            }

            return retval;
        }

        private static MethodBase GetMethod(IEnumerable<MethodBase> methods, object[] parameters, bool allowFuzzyParameters)
        {
            var retval = methods.FirstOrDefault(m => IsMatch(m, parameters));
            if (retval == null && allowFuzzyParameters)
            {
                int paramDistance = 0;
                foreach (var method in methods)
                {
                    var methodParams = method.GetParameters();
                    int diff = methodParams.Length - parameters.Length;

                    // Skip conditions:
                    // Parameter count is the same.  This means the parameter types will not be compatible or else it would've been found when looking for an exact match.
                    // The method has fewer parameters (than the provided object array) and we've already found one with more parameters.  Minimize data loss!
                    // The method has more parameters, but we've already found one with more parameters and this one has a greater difference in number.  Reduce waste!
                    // The method has fewer parameters, but we've already found one with fewer parameters and this one has a greater difference in number.  Minimize data loss!
                    if (diff == 0 || (paramDistance > 0 && (diff >= paramDistance || diff < 0) || (paramDistance < 0 && diff <= paramDistance)))
                    {
                        continue;
                    }

                    for (int i = 0; i < parameters.Length; i++)
                    {
                        if (methodParams.Length <= i)
                        {
                            break;
                        }

                        var param = parameters[i];
                        var mParamType = methodParams[i].ParameterType.GetTypeInfo();
                        if ((param == null && mParamType.IsValueType) || (param != null && !mParamType.IsAssignableFrom(param.GetType().GetTypeInfo())))
                        {
                            diff = 0;
                        }
                    }

                    if (diff != 0)
                    {
                        retval = method;
                        paramDistance = diff;
                    }
                }

#if DEBUG
                if (retval == null)
                {
                    Logger.Debug(CultureInfo.CurrentCulture, "Unable to locate an appropriate method for parameters [{0}].",
                        string.Join(" ", parameters.Select(p => p.GetType().FullName)));
                }
                else
                {
                    Logger.Debug(CultureInfo.CurrentCulture, "Exact method match for parameters [{0}] not found.  Using subtitute method with parameters [{1}] instead.",
                        string.Join(" ", parameters.Select(p => p.GetType().FullName)), string.Join(" ", retval.GetParameters().Select(p => p.ParameterType.FullName)));
                }
#endif
            }

            return retval;
        }

        private static bool IsMatch(MethodBase method, object[] parameters)
        {
            var methodParams = method.GetParameters();
            if (parameters.Length != methodParams.Length)
            {
                return false;
            }

            for (int i = 0; i < methodParams.Length; i++)
            {
                var param = parameters[i];
                var mParamType = methodParams[i].ParameterType.GetTypeInfo();
                if ((param == null && mParamType.IsValueType) || (param != null && !mParamType.IsAssignableFrom(param.GetType().GetTypeInfo())))
                {
                    return false;
                }
            }

            return true;
        }

        private void Register(ITypeRegistrationKey key, Type implementationType, bool isSingleton, object singletonInstance, string initializeMethod, TypeRegistrationOptions options)
        {
            if (entries.ContainsKey(key))
            {
                if (options.HasFlag(TypeRegistrationOptions.ThrowIfExists))
                {
                    throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.Strings.TypeRegistrationAlreadyExists, key.RegisteredType.FullName, key.RegisteredName));
                }

                if (options.HasFlag(TypeRegistrationOptions.SkipIfExists))
                {
                    return;
                }
            }

            entries[key] = new TypeRegistrationData(implementationType, isSingleton, singletonInstance, initializeMethod);
        }

        [DebuggerDisplay("[{RegisteredType}, {RegisteredName}]")]
        private struct TypeRegistrationKey : ITypeRegistrationKey
        {
            public string RegisteredName { get; }

            public Type RegisteredType { get; }

            private readonly int hash;
            
            public TypeRegistrationKey(Type type, string name)
            {
                RegisteredType = type;
                RegisteredName = name;

                hash = RegisteredType == null ? -1 : RegisteredType.GetHashCode() ^ (RegisteredName == null ? 0 : RegisteredName.GetHashCode());
            }

            public override bool Equals(object obj)
            {
                if (obj is TypeRegistrationKey)
                {
                    var typeKey = (TypeRegistrationKey)obj;
                    return typeKey.RegisteredType == RegisteredType && typeKey.RegisteredName == RegisteredName;
                }

                return false;
            }

            public override int GetHashCode()
            {
                return hash;
            }
        }
    }
}
