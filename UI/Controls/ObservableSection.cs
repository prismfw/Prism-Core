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
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Prism.UI.Controls
{
    /// <summary>
    /// Defines an observable collection of elements that make up a section within a <see cref="ListBox"/>.
    /// </summary>
    public interface IObservableSection : INotifyCollectionChanged, INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets the title text of the header above this section.
        /// </summary>
        string HeaderTitle { get; set; }

        /// <summary>
        /// Gets or sets an identifier for the section so that it may be easily distinguished from other sections.
        /// </summary>
        string Id { get; set; }
    }

    /// <summary>
    /// Represents an observable collection of elements that make up a section within a <see cref="ListBox"/>.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "This class is a container of a collection, not a collection itself.")]
    [SuppressMessage("Microsoft.Design", "CA1035:ICollectionImplementationsHaveStronglyTypedMembers", Justification = "Strongly-typed implementations are available via the Items property.")]
    [SuppressMessage("Microsoft.Design", "CA1039:ListsAreStronglyTyped", Justification = "Strongly-typed implementations are available via the Items property.")]
    public class ObservableSection<T> : IObservableSection, IList
    {
        /// <summary>
        /// Occurs when the <see cref="P:Items"/> collection has been modified.
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the title text of the header above this section.
        /// </summary>
        public string HeaderTitle
        {
            get { return headerTitle; }
            set
            {
                if (value != headerTitle)
                {
                    headerTitle = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(HeaderTitle)));
                }
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string headerTitle;

        /// <summary>
        /// Gets or sets an identifier for the section so that it may be easily distinguished from other sections.
        /// </summary>
        public string Id
        {
            get { return id; }
            set
            {
                if (value != id)
                {
                    id = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(Id)));
                }
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string id;

        /// <summary>
        /// Gets a collection of the items that make up the section.
        /// </summary>
        public Collection<T> Items { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableSection&lt;T&gt;"/> class.
        /// </summary>
        public ObservableSection()
        {
            Items = new ObservableCollection<T>();
            (Items as INotifyCollectionChanged).CollectionChanged += (o, e) => OnCollectionChanged(e);
            (Items as INotifyPropertyChanged).PropertyChanged += (o, e) => OnPropertyChanged(e);
        }

        /// <summary>
        /// Called when the <see cref="P:Items"/> collection has been modified and raises the <see cref="E:CollectionChanged"/> event.
        /// </summary>
        /// <param name="e">The event arguments for the event.</param>
        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            CollectionChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Called when a property value changes and raises the <see cref="E:PropertyChanged"/> event.
        /// </summary>
        /// <param name="e">The event arguments for the event.</param>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Accessible via Items property.")]
        bool IList.IsFixedSize
        {
            get { return (Items as IList).IsFixedSize; }
        }
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Accessible via Items property.")]
        bool IList.IsReadOnly
        {
            get { return (Items as IList).IsReadOnly; }
        }
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Accessible via Items property.")]
        int ICollection.Count
        {
            get { return (Items as ICollection).Count; }
        }
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Accessible via Items property.")]
        bool ICollection.IsSynchronized
        {
            get { return (Items as ICollection).IsSynchronized; }
        }
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Accessible via Items property.")]
        object ICollection.SyncRoot
        {
            get { return (Items as ICollection).SyncRoot; }
        }
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Accessible via Items property.")]
        object IList.this[int index]
        {
            get { return Items[index]; }
            set { Items[index] = (T)value; }
        }

        [SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Accessible via Items property.")]
        IEnumerator IEnumerable.GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        [SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Accessible via Items property.")]
        int IList.Add(object value)
        {
            return (Items as IList).Add((T)value);
        }

        [SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Accessible via Items property.")]
        void IList.Clear()
        {
            Items.Clear();
        }

        [SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Accessible via Items property.")]
        bool IList.Contains(object value)
        {
            try
            {
                return Items.Contains((T)value);
            }
            catch(InvalidCastException)
            {
                return false;
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Accessible via Items property.")]
        int IList.IndexOf(object value)
        {
            return Items.IndexOf((T)value);
        }

        [SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Accessible via Items property.")]
        void IList.Insert(int index, object value)
        {
            Items.Insert(index, (T)value);
        }

        [SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Accessible via Items property.")]
        void IList.Remove(object value)
        {
            Items.Remove((T)value);
        }

        [SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Accessible via Items property.")]
        void IList.RemoveAt(int index)
        {
            Items.RemoveAt(index);
        }

        [SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Accessible via Items property.")]
        void ICollection.CopyTo(Array array, int index)
        {
            Items.CopyTo((T[])array, index);
        }
    }
}
