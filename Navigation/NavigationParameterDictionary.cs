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
using System.Collections.Generic;
using System.Diagnostics;

namespace Prism
{
    /// <summary>
    /// Represents a collection of navigation parameters that are passed to an <see cref="IController"/> for loading.
    /// </summary>
    [DebuggerDisplay("Count = {Count}")]
    public sealed class NavigationParameterDictionary : IDictionary<string, object>
    {
        /// <summary>
        /// Gets the number of parameters contained in the collection.
        /// </summary>
        public int Count
        {
            get { return items.Count; }
        }

        /// <summary>
        /// Gets an <see cref="T:ICollection&lt;string&gt;"/> containing the parameter keys in the collection.
        /// </summary>
        public ICollection<string> Keys
        {
            get { return items.Keys; }
        }

        /// <summary>
        /// Gets an <see cref="T:ICollection&lt;object&gt;"/> containing the parameter values in the collection.
        /// </summary>
        public ICollection<object> Values
        {
            get { return items.Values; }
        }

        /// <summary>
        /// Gets or sets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get or set.</param>
        /// <returns>The value associated with the specified key. If the specified key is not found,
        /// a get operation throws a System.Collections.Generic.KeyNotFoundException, and a set operation
        /// creates a new element with the specified key.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="key"/> is <c>null</c>.</exception>
        /// <exception cref="KeyNotFoundException">Thrown when <paramref name="key"/> does not exist in the collection during a get operation.</exception>
        public object this[string key]
        {
            get { return items[key]; }
            set { items[key] = value; }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly IDictionary<string, object> items = new Dictionary<string, object>();

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationParameterDictionary"/> class.
        /// </summary>
        public NavigationParameterDictionary()
        {
        }

        /// <summary>
        /// Adds a parameter with the provided key and value to the collection.
        /// </summary>
        /// <param name="key">The string to use as the key of the parameter to add.</param>
        /// <param name="value">The object to use as the value of the parameter to add.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="key"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when a parameter with the same key already exists in the collection.</exception>
        public void Add(string key, object value)
        {
            items.Add(key, value);
        }

        /// <summary>
        /// Removes all parameters from the collection.
        /// </summary>
        public void Clear()
        {
            items.Clear();
        }

        /// <summary>
        /// Determines whether the collection contains a parameter with the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the collection.</param>
        /// <returns><c>true</c> if the collection contains a parameter with the key; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="key"/> is <c>null</c>.</exception>
        public bool ContainsKey(string key)
        {
            return items.ContainsKey(key);
        }

        /// <summary>
        /// Gets the value associated with the specified key as a <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the value to get.</typeparam>
        /// <param name="key">The key associated with the value to get.</param>
        /// <returns>The value associated with the key as a <typeparamref name="T"/> -or- the default value of <typeparamref name="T"/> if the value is not of that type.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="key"/> is <c>null</c>.</exception>
        /// <exception cref="KeyNotFoundException">Thrown when <paramref name="key"/> does not exist in the collection.</exception>
        public T GetValue<T>(string key)
        {
            object value = items[key];
            if (value is T)
            {
                return (T)value;
            }
            
            if (typeof(T) == typeof(string) && value != null)
            {
                return (T)(object)value.ToString();
            }
            
            return default(T);
        }

        /// <summary>
        /// Gets the value associated with the specified key as a <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the value to get.</typeparam>
        /// <param name="key">The key associated with the value to get.</param>
        /// <returns>The value associated with the key as a <typeparamref name="T"/> -or-
        /// the default value of <typeparamref name="T"/> if the key was not found or the value associated with the key is the wrong type.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="key"/> is <c>null</c>.</exception>
        public T GetValueOrDefault<T>(string key)
        {
            object value = null;
            items.TryGetValue(key, out value);
            if (value is T)
            {
                return (T)value;
            }

            if (typeof(T) == typeof(string) && value != null)
            {
                return (T)(object)value.ToString();
            }

            return default(T);
        }

        /// <summary>
        /// Gets the value associated with the specified key as a <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the value to get.</typeparam>
        /// <param name="key">The key associated with the value to get.</param>
        /// <param name="defaultValue">The value to return if the key is not found or the value associated with the key is the wrong type.</param>
        /// <returns>The value associated with the key as a <typeparamref name="T"/> -or-
        /// <paramref name="defaultValue"/> if the key was not found or the value associated with the key is the wrong type.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="key"/> is <c>null</c>.</exception>
        public T GetValueOrDefault<T>(string key, T defaultValue)
        {
            object value = null;
            items.TryGetValue(key, out value);
            if (value is T)
            {
                return (T)value;
            }

            if (typeof(T) == typeof(string) && value != null)
            {
                return (T)(object)value.ToString();
            }

            return defaultValue;
        }

        /// <summary>
        /// Removes the parameter with the specified key from the collection.
        /// </summary>
        /// <param name="key">The key of the parameter to be removed.</param>
        /// <returns><c>true</c> if the parameter is successfully removed; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="key"/> is <c>null</c>.</exception>
        public bool Remove(string key)
        {
            return items.Remove(key);
        }

        /// <summary>
        /// Gets the value associated with the specified key as a <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the value to get.</typeparam>
        /// <param name="key">The key associated with the value to get.</param>
        /// <param name="value">When this method returns, the value associated with the specified key, if the key is found; otherwise, the default value for the type of the value parameter. This parameter is passed uninitialized.</param>
        /// <returns><c>true</c> if the collection contains a parameter with the specified key; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="key"/> is <c>null</c>.</exception>
        public bool TryGetValue<T>(string key, out T value)
        {
            object v;
            items.TryGetValue(key, out v);
            if (v is T)
            {
                value = (T)v;
                return true;
            }

            if (typeof(T) == typeof(string) && v != null)
            {
                value = (T)(object)v.ToString();
                return true;
            }

            value = default(T);
            return false;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return items.GetEnumerator();
        }


        bool IDictionary<string, object>.TryGetValue(string key, out object value)
        {
            return items.TryGetValue(key, out value);
        }

        void ICollection<KeyValuePair<string, object>>.Add(KeyValuePair<string, object> item)
        {
            items.Add(item);
        }
        
        bool ICollection<KeyValuePair<string, object>>.Contains(KeyValuePair<string, object> item)
        {
            return items.Contains(item);
        }
        
        void ICollection<KeyValuePair<string, object>>.CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            items.CopyTo(array, arrayIndex);
        }

        bool ICollection<KeyValuePair<string, object>>.Remove(KeyValuePair<string, object> item)
        {
            return items.Remove(item);
        }
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        bool ICollection<KeyValuePair<string, object>>.IsReadOnly
        {
            get { return false; }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return items.GetEnumerator();
        }
    }
}
