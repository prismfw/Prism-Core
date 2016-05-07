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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Prism
{
    /// <summary>
    /// Represents a mapping of key types to concrete types that the key types represent.
    /// </summary>
    [DebuggerDisplay("Count = {Count}")]
    [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "Behavior is more consistent with a dictionary than a standard collection.")]
    public class NamedTypeMap : IEnumerable<KeyValuePair<Type, Type>>, IEnumerable
    {
        /// <summary>
        /// Gets the number of entries contained in the collection.
        /// </summary>
        public int Count
        {
            get { return _items.Count; }
        }

        /// <summary>
        /// Gets or sets the implementation type associated with the specified registered type.
        /// </summary>
        /// <param name="registerType">The registered type associated with the implementation type to get or set.</param>
        /// <returns>The implementation type associated with the <paramref name="registerType"/>.</returns>
        [SuppressMessage("Microsoft.Design", "CA1043:UseIntegralOrStringArgumentForIndexers", Justification = "Map entries are stored through a type-to-type mapping, so a key of System.Type is the most logical.")]
        public Type this[Type registerType]
        {
            get
            {
                var loader = this[registerType, null];
                return loader == null ? null : loader.InstanceType;
            }
            set 
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                OnInsert(registerType, null, new TypeLoader(value));
            }
        }

        /// <summary>
        /// Gets or sets the implementation type associated with the specified registered type.
        /// </summary>
        /// <param name="registerType">The registered type associated with the implementation type to get or set.</param>
        /// <param name="name">A unique identifier for the implementation type.</param>
        /// <returns>The implementation type associated with the <paramref name="registerType"/>.</returns>
        [SuppressMessage("Microsoft.Design", "CA1023:IndexersShouldNotBeMultidimensional", Justification = "Name is an integral component to the key structure of the map.  As such, it should be usable with the indexer.")]
        public TypeLoader this[Type registerType, string name]
        {
            get { return GetTypeLoader(registerType, name); }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                OnInsert(registerType, name, value);
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        private readonly IDictionary<NamedType, TypeLoader> _items;

        /// <summary>
        /// Initializes a new instance of the <see cref="NamedTypeMap"/> class.
        /// </summary>
        public NamedTypeMap()
        {
            _items = new Dictionary<NamedType, TypeLoader>();
        }

        /// <summary>
        /// Adds an entry with the specified registered type and implementation type to the collection.
        /// </summary>
        /// <param name="registerType">The type to associate with the implementation type.</param>
        /// <param name="implementationType">The implementation type to associate with the registered type.</param>
        public void Add(Type registerType, Type implementationType)
        {
            Add(registerType, null, new TypeLoader(implementationType));
        }

        /// <summary>
        /// Adds an entry with the specified registered type and implementation type to the collection.
        /// </summary>
        /// <param name="registerType">The type to associate with the implementation type.</param>
        /// <param name="name">A unique identifier for the implementation type.</param>
        /// <param name="implementationType">The implementation type to associate with the registered type.</param>
        public void Add(Type registerType, string name, Type implementationType)
        {
            Add(registerType, name, new TypeLoader(implementationType));
        }

        /// <summary>
        /// Adds an entry with the specified registered type and type loader to the collection.
        /// </summary>
        /// <param name="registerType">The type to associate with the type loader.</param>
        /// <param name="name">A unique identifier for the type loader.</param>
        /// <param name="typeLoader">The type loader to associate with the registered type.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="typeLoader"/> is <c>null</c>.</exception>
        public void Add(Type registerType, string name, TypeLoader typeLoader)
        {
            if (typeLoader == null)
            {
                throw new ArgumentNullException(nameof(typeLoader));
            }

            OnInsert(registerType, name, typeLoader);
        }

        /// <summary>
        /// Determines whether the collection contains an entry with the specified registered type.
        /// </summary>
        /// <param name="registerType">The registered type to locate in the collection.</param>
        /// <returns><c>true</c> if the registered type was located in the collection; otherwise, <c>false</c>.</returns>
        public bool ContainsKey(Type registerType)
        {
            return _items.ContainsKey(new NamedType(registerType, null));
        }

        /// <summary>
        /// Determines whether the collection contains an entry with the specified registered type.
        /// </summary>
        /// <param name="registerType">The registered type to locate in the collection.</param>
        /// <param name="name">A unique identifier for the implementation type that is associated with the registered type.</param>
        /// <returns><c>true</c> if the registered type was located in the collection; otherwise, <c>false</c>.</returns>
        public bool ContainsKey(Type registerType, string name)
        {
            return _items.ContainsKey(new NamedType(registerType, name));
        }

        /// <summary>
        /// Removes all custom entries from the collection.
        /// </summary>
        public virtual void Clear()
        {
            _items.Clear();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the map.
        /// </summary>
        public IEnumerator<KeyValuePair<Type, Type>> GetEnumerator()
        {
            return _items.Select(i => new KeyValuePair<Type, Type>(i.Key.RegisterType, i.Value.InstanceType)).GetEnumerator();
        }

        /// <summary>
        /// Removes the entry with the specified registered type from the collection.
        /// </summary>
        /// <param name="registerType">The registered type of the entry to remove.</param>
        /// <returns><c>true</c> if the entry was successfully removed; otherwise, <c>false</c>.</returns>
        public bool Remove(Type registerType)
        {
            return Remove(registerType, null);
        }

        /// <summary>
        /// Removes the entry with the specified registered type from the collection.
        /// </summary>
        /// <param name="registerType">The registered type of the entry to remove.</param>
        /// <param name="name">A unique identifier for the implementation type that is associated with the registered type.</param>
        /// <returns><c>true</c> if the entry was successfully removed; otherwise, <c>false</c>.</returns>
        public bool Remove(Type registerType, string name)
        {
            return _items.Remove(new NamedType(registerType, name));
        }

        /// <summary>
        /// Gets the implementation type associated with the specified registered type.
        /// </summary>
        /// <param name="registerType">The registered type whose associated implementation type to get.</param>
        /// <param name="value">When the method returns, the implementation type associated with the specified registered type, if the registered type was found; otherwise, <c>null</c>.</param>
        /// <returns><c>true</c> if the implementation type was successfully retrieved; otherwise, <c>false</c>.</returns>
        public bool TryGetValue(Type registerType, out Type value)
        {
            return TryGetValue(registerType, null, out value);
        }

        /// <summary>
        /// Gets the implementation type associated with the specified registered type.
        /// </summary>
        /// <param name="registerType">The registered type whose associated implementation type to get.</param>
        /// <param name="name">A unique identifier for the implementation type.</param>
        /// <param name="value">When the method returns, the implementation type associated with the specified registered type, if the registered type was found; otherwise, <c>null</c>."/></param>
        /// <returns><c>true</c> if the implementation type was successfully retrieved; otherwise, <c>false</c>.</returns>
        public bool TryGetValue(Type registerType, string name, out Type value)
        {
            TypeLoader loader;
            var retval = _items.TryGetValue(new NamedType(registerType, name), out loader);
            value = loader == null ? null : loader.InstanceType;
            return retval;
        }

        /// <summary>
        /// Resolves the specified registered type as an instance of its affiliated implementation.
        /// </summary>
        /// <typeparam name="T">The type of the object that is to be returned.</typeparam>
        /// <param name="resolveType">The registered type to resolve.</param>
        /// <param name="result">When the method returns, the resolved object instance; otherwise, the default value of <typeparamref name="T"/>.</param>
        /// <returns><c>true</c> if the object was successfully resolved; otherwise, <c>false</c>.</returns>
        public bool TryResolve<T>(Type resolveType, out T result)
        {
            return TryResolve(resolveType, null, null, out result);
        }

        /// <summary>
        /// Resolves the specified registered type as an instance of its affiliated implementation.
        /// </summary>
        /// <typeparam name="T">The type of the object that is to be returned.</typeparam>
        /// <param name="resolveType">The registered type to resolve.</param>
        /// <param name="name">An optional unique identifier for the affiliated implementation.</param>
        /// <param name="result">When the method returns, the resolved object instance; otherwise, the default value of <typeparamref name="T"/>.</param>
        /// <returns><c>true</c> if the object was successfully resolved; otherwise, <c>false</c>.</returns>
        public bool TryResolve<T>(Type resolveType, string name, out T result)
        {
            return TryResolve(resolveType, name, null, out result);
        }

        /// <summary>
        /// Resolves the specified registered type as an instance of its affiliated implementation.
        /// </summary>
        /// <typeparam name="T">The type of the object that is to be returned.</typeparam>
        /// <param name="resolveType">The registered type to resolve.</param>
        /// <param name="name">An optional unique identifier for the affiliated implementation.</param>
        /// <param name="parameters">An array of constructor parameters for initialization.</param>
        /// <param name="result">When the method returns, the resolved object instance; otherwise, the default value of <typeparamref name="T"/>.</param>
        /// <returns><c>true</c> if the object was successfully resolved; otherwise, <c>false</c>.</returns>
        public bool TryResolve<T>(Type resolveType, string name, object[] parameters, out T result)
        {
            var initer = GetTypeLoader(resolveType, name);
            if (initer == null)
            {
                result = default(T);
                return false;
            }

            result = (T)initer.Load(parameters);
            _items[new NamedType(resolveType, name)] = initer;
            return true;
        }

        /// <summary>
        /// Resolves the specified registered type as an instance of its affiliated implementation.
        /// </summary>
        /// <param name="resolveType">The registered type to resolve.</param>
        /// <param name="name">An optional unique identifier for the affiliated implementation.</param>
        /// <param name="parameters">An array of constructor parameters for initialization.</param>
        /// <exception cref="KeyNotFoundException">Thrown if the registered type cannot be found in the map.</exception>
        /// <returns>The object instance.</returns>
        public object Resolve(Type resolveType, string name, params object[] parameters)
        {
            var initer = GetTypeLoader(resolveType, name);
            if (initer == null)
            {
                throw new KeyNotFoundException(string.Format(CultureInfo.CurrentCulture, Resources.Strings.CannotLocateTypeLoaderForType, resolveType?.FullName, name));
            }

            var retval = initer.Load(parameters);
            _items[new NamedType(resolveType, name)] = initer;
            return retval;
        }

        /// <summary>
        /// Called when a new entry is inserted into the map, allowing for custom validation of the entry.
        /// </summary>
        /// <param name="registerType">The registered <see cref="Type"/>.</param>
        /// <param name="name">A unique identifier for the type loader.</param>
        /// <param name="loader">The <see cref="TypeLoader"/> that will be responsible for loading instances of the affiliated implementation.</param>
        protected virtual void OnInsert(Type registerType, string name, TypeLoader loader)
        {
            _items[new NamedType(registerType, name)] = loader;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<Type, Type>>)this).GetEnumerator();
        }

        private TypeLoader GetTypeLoader(Type type, string name)
        {
            TypeLoader initer;
            if (_items.TryGetValue(new NamedType(type, name), out initer))
            {
                return initer;
            }

            if (name != null)
            {
                if (_items.TryGetValue(new NamedType(type), out initer))
                {
                    return initer;
                }
            }

            var keys = type.GetTypeInfo().ImplementedInterfaces;
            initer = keys.Select(inter => GetTypeLoader(inter, name)).FirstOrDefault();

            return initer;
        }

        /// <summary>
        /// Represents a key with a type and an optional name.
        /// </summary>
        protected struct NamedType : IComparable, IEqualityComparer<NamedType>
        {
            /// <summary>
            /// Gets the name, if one has been specified.
            /// </summary>
            public string Name { get; }

            /// <summary>
            /// Gets the registered type.
            /// </summary>
            public Type RegisterType { get; }

            /// <summary>
            /// Initializes a new instance of the <see cref="NamedType"/> structure.
            /// </summary>
            /// <param name="registerType">The registered type.</param>
            /// <exception cref="ArgumentNullException">Thrown when <paramref name="registerType"/> is <c>null</c>.</exception>
            public NamedType(Type registerType) : this(registerType, null) { }

            /// <summary>
            /// Initializes a new instance of the <see cref="NamedType"/> structure.
            /// </summary>
            /// <param name="registerType">The registered type.</param>
            /// <param name="name">An optional name.</param>
            /// <exception cref="ArgumentNullException">Thrown when <paramref name="registerType"/> is <c>null</c>.</exception>
            public NamedType(Type registerType, string name)
            {
                if (registerType == null) throw new ArgumentNullException(nameof(registerType));

                RegisterType = registerType;
                Name = name;
            }

            #region Equality members

            /// <summary>
            /// Returns a hash code for this instance.
            /// </summary>
            /// <returns>
            /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
            /// </returns>
            public override int GetHashCode()
            {
                return RegisterType == null ? -1 : RegisterType.GetHashCode() ^ (Name == null ? 0 : Name.GetHashCode());
            }

            /// <summary>
            /// Determines whether the specified <see cref="object"/> is equal to this instance.
            /// </summary>
            /// <param name="obj">The <see cref="object" /> to compare with this instance.</param>
            /// <returns><c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>. </returns>
            public override bool Equals(object obj)
            {
                return this == (NamedType)obj;
            }

            /// <summary>
            /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
            /// </summary>
            /// <param name="obj">An object to compare with this instance.</param>
            /// <returns>A value that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This instance precedes <paramref name="obj" /> in the sort order. Zero This instance occurs in the same position in the sort order as <paramref name="obj" />. Greater than zero This instance follows <paramref name="obj" /> in the sort order.</returns>
            public int CompareTo(object obj)
            {
                var p = (NamedType)obj;
                return GetHashCode() == p.GetHashCode() ? 0 : -1;
            }

            /// <summary>
            /// Determines whether the specified objects are equal.
            /// </summary>
            /// <param name="x">The first object of type <see cref="NamedType"/> to compare.</param>
            /// <param name="y">The second object of type <see cref="NamedType"/> to compare.</param>
            /// <returns>
            /// true if the specified objects are equal; otherwise, false.
            /// </returns>
            public bool Equals(NamedType x, NamedType y)
            {
                return x.GetHashCode() == y.GetHashCode();
            }

            /// <summary>
            /// Returns a hash code for this instance.
            /// </summary>
            /// <param name="obj">The object.</param>
            /// <returns>
            /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
            /// </returns>
            public int GetHashCode(NamedType obj)
            {
                return obj.GetHashCode();
            }

            /// <summary>
            /// Checks the specified NamedTypes for equality.
            /// </summary>
            /// <param name="value1">The first <see cref="NamedType"/> to check.</param>
            /// <param name="value2">The second <see cref="NamedType"/> to check.</param>
            /// <returns><c>true</c> if the parameters are equal; otherwise, <c>false</c>.</returns>
            public static bool operator ==(NamedType value1, NamedType value2)
            {
                return value1.CompareTo(value2) == 0;
            }

            /// <summary>
            /// Checks the specified NamedTypes for inequality.
            /// </summary>
            /// <param name="value1">The first <see cref="NamedType"/> to check.</param>
            /// <param name="value2">The second <see cref="NamedType"/> to check.</param>
            /// <returns><c>true</c> if the parameters are not equal; otherwise, <c>false</c>.</returns>
            public static bool operator !=(NamedType value1, NamedType value2)
            {
                return value1.CompareTo(value2) != 0;
            }

            /// <summary>
            /// Determines whether a <see cref="NamedType"/> instance is considered greater than another <see cref="NamedType"/> instance.
            /// </summary>
            /// <param name="value1">The first value to compare.</param>
            /// <param name="value2">The second value to compare.</param>
            /// <returns><c>true</c> is <paramref name="value1"/> is considered greater than <paramref name="value2"/>; otherwise, <c>false</c>.</returns>
            public static bool operator >(NamedType value1, NamedType value2)
            {
                return value1.CompareTo(value2) > 0;
            }

            /// <summary>
            /// Determines whether a <see cref="NamedType"/> instance is considered less than another <see cref="NamedType"/> instance.
            /// </summary>
            /// <param name="value1">The first value to compare.</param>
            /// <param name="value2">The second value to compare.</param>
            /// <returns><c>true</c> is <paramref name="value1"/> is considered less than <paramref name="value2"/>; otherwise, <c>false</c>.</returns>
            public static bool operator <(NamedType value1, NamedType value2)
            {
                return value1.CompareTo(value2) < 0;
            }

            #endregion
        }
    }
}