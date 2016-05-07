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
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

#if !DEBUG
using System.Diagnostics;
#endif

namespace Prism
{
    /// <summary>
    /// Represents a wrapper for initializing and managing objects by type.
    /// </summary>
    public class TypeLoader
    {
        /// <summary>
        /// Gets the initialized object of <see cref="P:InstanceType"/>, if it exists.
        /// </summary>
        public object Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// Gets the concrete type managed by this instance.
        /// </summary>
        public Type InstanceType
        {
            get { return _instanceType; }
        }

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private readonly bool _singletonInstance;
#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private object _instance;
#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private readonly Type _instanceType;
#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private readonly IEnumerable<MethodInfo> _initializeMethods;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeLoader"/> class.
        /// </summary>
        /// <param name="instanceType">The <see cref="Type"/> to manage.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="instanceType"/> is <c>null</c>.</exception>
        public TypeLoader(Type instanceType) : this(instanceType, false) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeLoader"/> class.
        /// </summary>
        /// <param name="instanceType">The <see cref="Type"/> to manage.</param>
        /// <param name="isSingleton">Whether to cache the created instance and reuse it.  A value of <c>false</c> will mean a new instance is created every time.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="instanceType"/> is <c>null</c>.</exception>
        public TypeLoader(Type instanceType, bool isSingleton) : this(instanceType, isSingleton, null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeLoader"/> class.
        /// </summary>
        /// <param name="instanceType">The <see cref="Type"/> to manage.</param>
        /// <param name="initializeMethod">An optional name of a static method on the type to use in place of a constructor for initializing the object.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="instanceType"/> is <c>null</c>.</exception>
        public TypeLoader(Type instanceType, string initializeMethod) : this(instanceType, false, initializeMethod) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeLoader"/> class.
        /// </summary>
        /// <param name="instanceType">The <see cref="Type"/> to manage.</param>
        /// <param name="isSingleton">Whether to cache the created instance and reuse it.  A value of <c>false</c> will mean a new instance is created every time.</param>
        /// <param name="initializeMethod">An optional name of a static method on the type to use in place of a constructor for initializing the object.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="instanceType"/> is <c>null</c>.</exception>
        public TypeLoader(Type instanceType, bool isSingleton, string initializeMethod)
        {
            if (instanceType == null)
            {
                throw new ArgumentNullException(nameof(instanceType));
            }

            _singletonInstance = isSingleton;
            _instanceType = instanceType;
            _instance = null;

            if (initializeMethod != null)
            {
                _initializeMethods = instanceType.GetTypeInfo().GetDeclaredMethods(initializeMethod).Where(m => m.IsStatic);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeLoader"/> class.
        /// </summary>
        /// <param name="instance">The singleton object to manage.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="instance"/> is <c>null</c>.</exception>
        public TypeLoader(object instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            _singletonInstance = true;
            _instance = instance;
            _instanceType = instance.GetType();
            _initializeMethods = null;
        }

        #endregion

        /// <summary>
        /// Generates a new instance of <see cref="P:InstanceType"/> and returns it, or returns the managed singleton instance.
        /// </summary>
        /// <param name="parameters">An optional array of constructor parameters for initialization.</param>
        /// <returns>The object instance.</returns>
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "Method is sufficiently maintainable.")]
        public object Load(params object[] parameters)
        {
            if (_instance != null)
            {
                return _instance;
            }

            if (_instanceType == null)
            {
                return null;
            }

            object retval = null;

            MethodInfo method = null;
            if (_initializeMethods != null)
            {
                method = _initializeMethods.FirstOrDefault(m =>
                {
                    var p = m.GetParameters();
                    return (parameters == null && p.Length == 0) || (parameters.Length == p.Length && !p.Where((t, i) =>
                    {
                        var param = parameters[i];
                        return param == null ? t.ParameterType.GetTypeInfo().IsValueType :
                            !t.ParameterType.GetTypeInfo().IsAssignableFrom(param.GetType().GetTypeInfo());
                    }).Any());
                });

                if (method == null)
                {
                    method = _initializeMethods.FirstOrDefault(m => m.GetParameters().Length == 0);
                    if (method != null)
                    {
                        retval = method.Invoke(null, null);
                    }
                }
                else
                {
                    retval = method.Invoke(null, parameters);
                }
            }

            if (method == null)
            {
                try
                {
                    var ctors = _instanceType.GetTypeInfo().DeclaredConstructors;
                    if (parameters == null || parameters.Length == 0)
                    {
                        retval = Activator.CreateInstance(_instanceType);
                    }
                    else
                    {
                        var ctor = ctors.FirstOrDefault(c =>
                        {
                            var p = c.GetParameters();
                            return p.Length == parameters.Length && !p.Where((t, i) =>
                            {
                                var param = parameters[i];
                                return param == null ? t.ParameterType.GetTypeInfo().IsValueType :
                                    !t.ParameterType.GetTypeInfo().IsAssignableFrom(param.GetType().GetTypeInfo());
                            }).Any();
                        });

                        retval = ctor == null ? Activator.CreateInstance(_instanceType) : ctor.Invoke(parameters);
                    }
                }
                catch (MissingMemberException)
                {
                    retval = Activator.CreateInstance(_instanceType);
                }
            }

            if (_singletonInstance)
            {
                _instance = retval;
            }

            return retval;
        }
    }
}