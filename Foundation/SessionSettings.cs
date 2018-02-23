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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Xml;
using System.Xml.Serialization;
using Prism.Utilities;

namespace Prism
{
    /// <summary>
    /// Represents a collection of settings that are scoped to a user session.
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
    public class SessionSettings : IEnumerable<KeyValuePair<string, object>>, IXmlSerializable
    {
        /// <summary>
        /// Gets the number of items in the collection.
        /// </summary>
        public int Count
        {
            get { return items.Count; }
        }

        /// <summary>
        /// Gets an <see cref="T:ICollection&lt;string&gt;"/> containing all of the keys in the collection.
        /// </summary>
        public ICollection<string> Keys
        {
            get { return items.Keys; }
        }

        /// <summary>
        /// Gets an identifier for the user session.
        /// </summary>
        public string SessionId
        {
            get { return sessionId; }
        }
        private readonly string sessionId;

        /// <summary>
        /// Gets an <see cref="T:ICollection&lt;object&gt;"/> containing all of the values in the collection.
        /// </summary>
        public ICollection<object> Values
        {
            get{ return items.Values; }
        }

        /// <summary>
        /// Gets or sets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get or set.</param>
        /// <returns>The value associated with the specified key. If the specified key is not found,
        /// a get operation returns <c>null</c>, and a set operation creates a new element with the specified key.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="key"/> is <c>null</c>.</exception>
        public object this[string key]
        {
            get
            {
                if (key == null)
                {
                    throw new ArgumentNullException(nameof(key));
                }

                object value;
                return items.TryGetValue(key, out value) ? value : null;
            }
            set
            {
                if (key == null)
                {
                    throw new ArgumentNullException(nameof(key));
                }

                items[key] = value;
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        private readonly SerializableDictionary<string, object> items = new SerializableDictionary<string, object>();

        internal SessionSettings(string sessionId)
        {
            this.sessionId = sessionId;
        }

        /// <summary>
        /// Cancels the session.
        /// </summary>
        public void Abandon()
        {
            SessionManager.AbandonSession(SessionId);
            items.Clear();
        }

        /// <summary>
        /// Adds an item with the provided key and value to the collection.
        /// </summary>
        /// <param name="key">The string to use as the key of the item to add.</param>
        /// <param name="value">The object to use as the value of the item to add.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="key"/> is <c>null</c>.</exception>
        public void Add(string key, object value)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            items[key] = value;
        }

        /// <summary>
        /// Removes all items from the collection.
        /// </summary>
        public void Clear()
        {
            items.Clear();
        }

        /// <summary>
        /// Copies the items in the collection to an <see cref="Array"/>, starting at a particular index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="Array"/> that is the destination of the elements copied from the collection.</param>
        /// <param name="arrayIndex">The index in the array at which copying begins.</param>
        public void CopyTo(Array array, int arrayIndex)
        {
            items.CopyTo(array, arrayIndex);
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
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

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
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

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
        /// Generates an element from the XML that is at the current position of the specified <see cref="XmlReader"/>.
        /// </summary>
        /// <param name="reader">The <see cref="XmlReader"/> stream from which the element is deserialized.</param>
        public void ReadXml(XmlReader reader)
        {
            items.ReadXml(reader);
        }

        /// <summary>
        /// Removes the item with the specified key from the collection.
        /// </summary>
        /// <param name="key">The key of the item to be removed.</param>
        /// <returns><c>true</c> if the item is successfully removed; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="key"/> is <c>null</c>.</exception>
        public bool Remove(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            return items.Remove(key);
        }

        /// <summary>
        /// Gets the value associated with the specified key as a <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the value to get.</typeparam>
        /// <param name="key">The key associated with the value to get.</param>
        /// <param name="value">When this method returns, the value associated with the specified key, if the key is found; otherwise, the default value for the type of the value parameter. This parameter is passed uninitialized.</param>
        /// <returns><c>true</c> if the collection contains an item with the specified key; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="key"/> is <c>null</c>.</exception>
        public bool TryGetValue<T>(string key, out T value)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

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
        /// Converts an element into its XML representation and writes it to the specified <see cref="XmlWriter"/>.
        /// </summary>
        /// <param name="writer">The <see cref="XmlWriter"/> stream to which the object is serialized.</param>
        public void WriteXml(XmlWriter writer)
        {
            items.WriteXml(writer);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return items.GetEnumerator();
        }

        [SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Not used.  Always returns null as recommended by interface documentation.")]
        System.Xml.Schema.XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }
    }
}