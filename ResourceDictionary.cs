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
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Prism.Native;
using Prism.UI;
using Prism.UI.Media;

namespace Prism
{
    /// <summary>
    /// Represents a dictionary that contains resources used by elements of the application.
    /// </summary>
    [DebuggerDisplay("Count = {Count}")]
    public sealed class ResourceDictionary : IDictionary<object, object>
    {
        internal event EventHandler<object> ResourceChanged;

        internal event EventHandler ResourceCollectionChanged;

        /// <summary>
        /// Gets the number of entries in the base dictionary.
        /// </summary>
        public int Count
        {
            get { return baseDictionary.Count; }
        }

        /// <summary>
        /// Gets a collection containing the keys in the base dictionary.
        /// </summary>
        public ICollection<object> Keys
        {
            get { return baseDictionary.Keys; }
        }

        /// <summary>
        /// Gets a collection of the <see cref="ResourceDictionary"/> instances that make up the merged dictionaries.
        /// </summary>
        public IList<ResourceDictionary> MergedDictionaries { get; }

        /// <summary>
        /// Gets a collection of the <see cref="ResourceDictionary"/> instances that contain theme resources for UI elements.
        /// </summary>
        public ThemeDictionaryCollection ThemeDictionaries { get; }

        /// <summary>
        /// Gets a collection containing the values in the base dictionary.
        /// </summary>
        public ICollection<object> Values
        {
            get { return baseDictionary.Values; }
        }

        /// <summary>
        /// Gets or sets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get or set.</param>
        /// <returns>The value associated with the specified key. If the specified key is not found,
        /// a get operation throws a System.Collections.Generic.KeyNotFoundException, and a set operation
        /// creates a new resource with the specified key.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="key"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="key"/> is not a <see cref="string"/> or <see cref="ResourceKey"/> instance during a set operation.</exception>
        /// <exception cref="KeyNotFoundException">Thrown when <paramref name="key"/> does not exist in the base dictionary, merged dictionaries, theme dictionaries, or system resources during a get operation.</exception>
        public object this[object key]
        {
            get
            {
                object value;
                if (TryGetValue(key, out value))
                {
                    return value;
                }

                throw new KeyNotFoundException();
            }
            set
            {
                CheckKey(key, value);
                baseDictionary[key] = value;
                OnResourceChanged(this, key);
            }
        }

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private readonly IDictionary<object, object> baseDictionary = new Dictionary<object, object>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceDictionary"/> class.
        /// </summary>
        public ResourceDictionary()
        {
            MergedDictionaries = new ResourceDictionaryCollection(this);
            ThemeDictionaries = new ThemeDictionaryCollection(this);
        }

        /// <summary>
        /// Adds a resource with the specified key and value to the base dictionary.
        /// </summary>
        /// <param name="key">The object to use as the key of the resource to add.</param>
        /// <param name="value">The object to use as the value of the resource to add.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="key"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when a resource with the same key already exists in the base dictionary -or- when <paramref name="key"/> is not a <see cref="string"/> or <see cref="ResourceKey"/> instance.</exception>
        public void Add(object key, object value)
        {
            CheckKey(key, value);
            baseDictionary.Add(key, value);
            OnResourceChanged(this, key);
        }

        /// <summary>
        /// Removes all resources from the base dictionary.
        /// </summary>
        public void Clear()
        {
            baseDictionary.Clear();
            OnResourceCollectionChanged(this, EventArgs.Empty);
        }

        /// <summary>
        /// Determines whether the specified key is contained within the base dictionary or any of the merged dictionaries or theme dictionaries.
        /// </summary>
        /// <param name="key">The key to locate within the the base dictionary or any of the merged dictionaries or theme dictionaries.</param>
        /// <returns><c>true</c> if the key was located in the base dictionary or one of the merged dictionaries or theme dictionaries; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="key"/> is <c>null</c>.</exception>
        public bool ContainsKey(object key)
        {
            return baseDictionary.ContainsKey(key) || MergedDictionaries.Any(d => d.ContainsKey(key)) || ThemeDictionaries.Any(d => d.Value.ContainsKey(key));
        }

        /// <summary>
        /// Returns an enumerator that iterates through the base dictionary.
        /// </summary>
        public IEnumerator<KeyValuePair<object, object>> GetEnumerator()
        {
            return baseDictionary.GetEnumerator();
        }

        /// <summary>
        /// Removes the resource with the specified key from the base dictionary.
        /// </summary>
        /// <param name="key">The key of the resource to remove.</param>
        /// <returns><c>true</c> if the resource is successfully removed; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="key"/> is <c>null</c>.</exception>
        public bool Remove(object key)
        {
            if (baseDictionary.Remove(key))
            {
                OnResourceChanged(this, key);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key associated with the value to get.</param>
        /// <param name="value">When this method returns, the value associated with the specified key, if the key is found; otherwise, <c>null</c>. This parameter is passed uninitialized.</param>
        /// <returns><c>true</c> if the base dictionary, merged dictionaries, theme dictionaries, or system resources contain a resource with the specified key; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="key"/> is <c>null</c>.</exception>
        public bool TryGetValue(object key, out object value)
        {
            return TryGetResource(null, key, out value, true);
        }

        internal bool TryGetResource(object obj, object key, out object value, bool readFromSystem)
        {
            if (baseDictionary.TryGetValue(key, out value))
            {
                return true;
            }

            for (int i = MergedDictionaries.Count - 1; i >= 0; i--)
            {
                if (MergedDictionaries[i].TryGetResource(obj, key, out value, false))
                {
                    return true;
                }
            }

            var theme = Theme.Default;
            var visual = obj as Visual;
            if (visual != null)
            {
                theme = visual.RequestedTheme;
                if (theme == Theme.Default)
                {
                    theme = VisualTreeHelper.GetParent<Visual>(visual, p => p.RequestedTheme != Theme.Default)?.RequestedTheme ??
                        (ObjectRetriever.GetNativeObject(Application.Current) as INativeApplication)?.DefaultTheme ?? Theme.Default;
                }
            }
            else
            {
                theme = (ObjectRetriever.GetNativeObject(Application.Current) as INativeApplication)?.DefaultTheme ?? Theme.Default;
            }

            ResourceDictionary themeDictionary;
            if (ThemeDictionaries.TryGetValue(theme, out themeDictionary) && themeDictionary.TryGetResource(obj, key, out value, false))
            {
                return true;
            }

            if (theme != Theme.Default && ThemeDictionaries.TryGetValue(Theme.Default, out themeDictionary) && themeDictionary.TryGetResource(obj, key, out value, false))
            {
                return true;
            }

            if (readFromSystem)
            {
                var resourceKey = key as ResourceKey;
                if (TryGetCoreResource(obj, resourceKey, theme, out value))
                {
                    return true;
                }

                if (TypeManager.Default.Resolve<INativeResources>()?.TryGetResource(ObjectRetriever.GetNativeObject(obj), key, out value) ?? false)
                {
                    return true;
                }

                if (resourceKey != null)
                {
                    Debug.Assert(false, string.Format(CultureInfo.CurrentCulture, "Missing system resource value for {0}!", (SystemResourceKeyId)resourceKey.Id));
                }
            }

            return false;
        }

        private static void CheckKey(object key, object value)
        {
            var resourceKey = key as ResourceKey;
            if (resourceKey?.ExpectedType != null)
            {
                var typeInfo = resourceKey.ExpectedType.GetTypeInfo();
                if ((value == null && typeInfo.IsValueType) || (value != null && !typeInfo.IsAssignableFrom(value.GetType().GetTypeInfo())))
                {
                    throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.Strings.ResourceMustBeOfType, typeInfo.FullName));
                }
            }
            else if (!(key is string))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.Strings.ResourceKeyCanOnlyBeOfType, typeof(string).FullName, typeof(ResourceKey).FullName));
            }
        }

        private bool TryGetCoreResource(object obj, ResourceKey key, Theme theme, out object value)
        {
            if (key != null)
            {
                switch ((SystemResourceKeyId)key.Id)
                {
                    case SystemResourceKeyId.AltHighColor:
                        value = theme == Theme.Dark ? Colors.White : Colors.Black;
                        return true;
                    case SystemResourceKeyId.AltMediumColor:
                        value = theme == Theme.Dark ? new Color(214, 214, 214) : new Color(42, 42, 42);
                        return true;
                    case SystemResourceKeyId.AltLowColor:
                        value = theme == Theme.Dark ? new Color(171, 171, 171) : new Color(85, 85, 85);
                        return true;
                    case SystemResourceKeyId.BaseHighColor:
                        value = theme == Theme.Dark ? Colors.Black : Colors.White;
                        return true;
                    case SystemResourceKeyId.BaseMediumColor:
                        value = theme == Theme.Dark ? new Color(42, 42, 42) : new Color(214, 214, 214);
                        return true;
                    case SystemResourceKeyId.BaseLowColor:
                        value = theme == Theme.Dark ? new Color(85, 85, 85) : new Color(171, 171, 171);
                        return true;
                    case SystemResourceKeyId.AltHighBrush:
                    case SystemResourceKeyId.AltMediumBrush:
                    case SystemResourceKeyId.AltLowBrush:
                    case SystemResourceKeyId.BaseHighBrush:
                    case SystemResourceKeyId.BaseMediumBrush:
                    case SystemResourceKeyId.BaseLowBrush:
                        var visual = obj as Visual;
                        if (visual == null)
                        {
                            TryGetResource(obj, key.DependencyKey, out value, true);
                        }
                        else
                        {
                            value = visual.TryFindResource(key.DependencyKey);
                        }
                        value = new SolidColorBrush((Color)(value ?? new Color()));
                        return true;
                }
            }

            value = null;
            return false;
        }

        private void OnResourceChanged(object sender, object key)
        {
            ResourceChanged?.Invoke(sender, key);
        }

        private void OnResourceCollectionChanged(object sender, EventArgs e)
        {
            ResourceCollectionChanged?.Invoke(sender, e);
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        bool ICollection<KeyValuePair<object, object>>.IsReadOnly
        {
            get { return false; }
        }

        void ICollection<KeyValuePair<object, object>>.Add(KeyValuePair<object, object> item)
        {
            baseDictionary.Add(item);
        }
        
        bool ICollection<KeyValuePair<object, object>>.Contains(KeyValuePair<object, object> item)
        {
            return baseDictionary.Contains(item);
        }

        void ICollection<KeyValuePair<object, object>>.CopyTo(KeyValuePair<object, object>[] array, int arrayIndex)
        {
            baseDictionary.CopyTo(array, arrayIndex);
        }

        bool ICollection<KeyValuePair<object, object>>.Remove(KeyValuePair<object, object> item)
        {
            return baseDictionary.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return baseDictionary.GetEnumerator();
        }

        /// <summary>
        /// Represents the collection of theme dictionaries within a <see cref="ResourceDictionary"/>.
        /// </summary>
        [DebuggerDisplay("Count = {Count}")]
        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = "Class is not meant to be referenced by type or used anywhere other than the ThemeDictionaries property on the ResourceDictionary class.")]
        public sealed class ThemeDictionaryCollection : IEnumerable<KeyValuePair<string, ResourceDictionary>>
        {
            /// <summary>
            /// Gets the number of theme dictionaries in the collection.
            /// </summary>
            public int Count
            {
                get { return dictionaries.Count; }
            }

            /// <summary>
            /// Gets or sets the dictionary associated with the specified case-insensitive theme name.
            /// </summary>
            /// <param name="name">The case-insensitive name of the dictionary to get or set.</param>
            /// <returns>The dictionary associated with the specified theme name. If the specified name is not found,
            /// a get operation throws a System.Collections.Generic.KeyNotFoundException, and a set operation
            /// creates a new entry with the specified name.</returns>
            /// <exception cref="ArgumentNullException">Thrown when <paramref name="name"/> is <c>null</c>.</exception>
            /// <exception cref="KeyNotFoundException">Thrown when <paramref name="name"/> does not exist in the collection during a get operation.</exception>
            public ResourceDictionary this[string name]
            {
                get { return dictionaries[name]; }
                set
                {
                    dictionaries[name] = value;
                    parentDictionary.OnResourceCollectionChanged(parentDictionary, EventArgs.Empty);
                }
            }

            /// <summary>
            /// Gets or sets the dictionary associated with the specified theme.
            /// </summary>
            /// <param name="theme">The theme of the dictionary to get or set.</param>
            /// <returns>The dictionary associated with the specified theme. If the specified theme is not found,
            /// a get operation throws a System.Collections.Generic.KeyNotFoundException, and a set operation
            /// creates a new entry with the specified theme.</returns>
            /// <exception cref="KeyNotFoundException">Thrown when <paramref name="theme"/> does not exist in the collection during a get operation.</exception>
            public ResourceDictionary this[Theme theme]
            {
                get { return dictionaries[theme.ToString()]; }
                set { this[theme.ToString()] = value; }
            }

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
            private readonly IDictionary<string, ResourceDictionary> dictionaries = new Dictionary<string, ResourceDictionary>(StringComparer.OrdinalIgnoreCase);
#if !DEBUG
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
            private readonly ResourceDictionary parentDictionary;

            internal ThemeDictionaryCollection(ResourceDictionary parent)
            {
                parentDictionary = parent;
            }

            /// <summary>
            /// Adds an entry with the specified case-insensitive theme name and dictionary to the collection.
            /// </summary>
            /// <param name="name">The case-insensitive name of the dictionary to add.</param>
            /// <param name="dictionary">The dictionary to add.</param>
            /// <exception cref="ArgumentNullException">Thrown when <paramref name="name"/> is <c>null</c>.</exception>
            /// <exception cref="ArgumentException">Thrown when an entry with an equivalent name already exists in the collection.</exception>
            public void Add(string name, ResourceDictionary dictionary)
            {
                dictionaries.Add(name, dictionary);
                parentDictionary.OnResourceCollectionChanged(parentDictionary, EventArgs.Empty);
            }

            /// <summary>
            /// Adds an entry with the specified theme and dictionary to the collection.
            /// </summary>
            /// <param name="theme">The theme of the dictionary to add.</param>
            /// <param name="dictionary">The dictionary to add.</param>
            /// <exception cref="ArgumentException">Thrown when an entry for the theme already exists in the collection.</exception>
            public void Add(Theme theme, ResourceDictionary dictionary)
            {
                Add(theme.ToString(), dictionary);
            }

            /// <summary>
            /// Removes all theme dictionaries from the collection.
            /// </summary>
            public void Clear()
            {
                dictionaries.Clear();
                parentDictionary.OnResourceCollectionChanged(parentDictionary, EventArgs.Empty);
            }

            /// <summary>
            /// Determines whether the collection contains a dictionary with the specified case-insensitive theme name.
            /// </summary>
            /// <param name="name">The case-insensitive name of the entry to locate in the collection.</param>
            /// <returns><c>true</c> if the collection contains an entry with an equivalent name; otherwise, <c>false</c>.</returns>
            /// <exception cref="ArgumentNullException">Thrown when <paramref name="name"/> is <c>null</c>.</exception>
            public bool Contains(string name)
            {
                return dictionaries.ContainsKey(name);
            }

            /// <summary>
            /// Determines whether the collection contains a theme dictionary for the specified theme.
            /// </summary>
            /// <param name="theme">The theme of the entry to locate in the collection.</param>
            /// <returns><c>true</c> if the collection contains an entry for the specified theme; otherwise, <c>false</c>.</returns>
            public bool Contains(Theme theme)
            {
                return dictionaries.ContainsKey(theme.ToString());
            }

            /// <summary>
            /// Removes the dictionary with the specified case-insensitive theme name from the collection.
            /// </summary>
            /// <param name="name">The case-insensitive theme name of the dictionary to remove.</param>
            /// <returns><c>true</c> if the dictionary is successfully removed; otherwise, <c>false</c>.</returns>
            /// <exception cref="ArgumentNullException">Thrown when <paramref name="name"/> is <c>null</c>.</exception>
            public bool Remove(string name)
            {
                if (dictionaries.Remove(name))
                {
                    parentDictionary.OnResourceCollectionChanged(parentDictionary, EventArgs.Empty);
                    return true;
                }

                return false;
            }

            /// <summary>
            /// Removes the dictionary for the specified theme from the collection.
            /// </summary>
            /// <param name="theme">The theme of the dictionary to remove.</param>
            /// <returns><c>true</c> if the dictionary is successfully removed; otherwise, <c>false</c>.</returns>
            public bool Remove(Theme theme)
            {
                return Remove(theme.ToString());
            }

            /// <summary>
            /// Gets the dictionary associated with the specified case-insensitive theme name.
            /// </summary>
            /// <param name="name">The case-insensitive theme name of the dictionary to get.</param>
            /// <param name="dictionary">When this method returns, the dictionary associated with the specified theme name, if found; otherwise, <c>null</c>. This parameter is passed uninitialized.</param>
            /// <returns><c>true</c> if the collection contains an entry with an equivalent name; otherwise, <c>false</c>.</returns>
            /// <exception cref="ArgumentNullException">Thrown when <paramref name="name"/> is <c>null</c>.</exception>
            public bool TryGetValue(string name, out ResourceDictionary dictionary)
            {
                return dictionaries.TryGetValue(name, out dictionary);
            }

            /// <summary>
            /// Gets the dictionary associated with the specified theme.
            /// </summary>
            /// <param name="theme">The theme of the dictionary to get.</param>
            /// <param name="dictionary">When this method returns, the dictionary associated with the specified theme, if found; otherwise, <c>null</c>. This parameter is passed uninitialized.</param>
            /// <returns><c>true</c> if the collection contains an entry for the specified theme; otherwise, <c>false</c>.</returns>
            public bool TryGetValue(Theme theme, out ResourceDictionary dictionary)
            {
                return dictionaries.TryGetValue(theme.ToString(), out dictionary);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return dictionaries.GetEnumerator();
            }

            IEnumerator<KeyValuePair<string, ResourceDictionary>> IEnumerable<KeyValuePair<string, ResourceDictionary>>.GetEnumerator()
            {
                return dictionaries.GetEnumerator();
            }
        }

        private class ResourceDictionaryCollection : Collection<ResourceDictionary>
        {
#if !DEBUG
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
            private readonly ResourceDictionary parentDictionary;

            public ResourceDictionaryCollection(ResourceDictionary parent)
            {
                parentDictionary = parent;
            }

            protected override void ClearItems()
            {
                foreach (var item in Items)
                {
                    OnRemoveItem(item);
                }

                base.ClearItems();

                parentDictionary.OnResourceCollectionChanged(parentDictionary, EventArgs.Empty);
            }

            protected override void InsertItem(int index, ResourceDictionary item)
            {
                if (item == null)
                {
                    throw new ArgumentNullException(nameof(item));
                }

                base.InsertItem(index, item);
                OnAddItem(item);

                parentDictionary.OnResourceCollectionChanged(parentDictionary, EventArgs.Empty);
            }

            protected override void RemoveItem(int index)
            {
                if (index < Count)
                {
                    OnRemoveItem(Items[index]);
                }

                base.RemoveItem(index);

                parentDictionary.OnResourceCollectionChanged(parentDictionary, EventArgs.Empty);
            }

            protected override void SetItem(int index, ResourceDictionary item)
            {
                if (item == null)
                {
                    throw new ArgumentNullException(nameof(item));
                }

                if (index < Count)
                {
                    OnRemoveItem(Items[index]);
                }

                base.SetItem(index, item);
                OnAddItem(item);

                parentDictionary.OnResourceCollectionChanged(parentDictionary, EventArgs.Empty);
            }

            private void OnAddItem(ResourceDictionary item)
            {
                item.ResourceChanged -= parentDictionary.OnResourceChanged;
                item.ResourceChanged += parentDictionary.OnResourceChanged;

                item.ResourceCollectionChanged -= parentDictionary.OnResourceCollectionChanged;
                item.ResourceCollectionChanged += parentDictionary.OnResourceCollectionChanged;
            }

            private void OnRemoveItem(ResourceDictionary item)
            {
                item.ResourceChanged -= parentDictionary.OnResourceChanged;
                item.ResourceCollectionChanged -= parentDictionary.OnResourceCollectionChanged;
            }
        }
    }
}
