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
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Prism.Native;
using Prism.UI.Media;

namespace Prism.UI.Controls
{
    /// <summary>
    /// Represents a vertically-oriented, scrollable list of selectable items.
    /// </summary>
    public class ListBox : Element, IScrollable
    {
        #region Event Descriptors
        /// <summary>
        /// Describes the <see cref="E:AccessorySelected"/> event.  This field is read-only.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "EventDescriptor is immutable.")]
        public static readonly EventDescriptor AccessorySelectedEvent = EventDescriptor.Create(nameof(AccessorySelected), typeof(TypedEventHandler<ListBox, AccessorySelectedEventArgs>), typeof(ListBox));

        /// <summary>
        /// Describes the <see cref="E:SelectionChanged"/> event.  This field is read-only.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "EventDescriptor is immutable.")]
        public static readonly EventDescriptor SelectionChangedEvent = EventDescriptor.Create(nameof(SelectionChanged), typeof(TypedEventHandler<ListBox, SelectionChangedEventArgs>), typeof(ListBox));
        #endregion

        #region Property Descriptors
        /// <summary>
        /// Describes the <see cref="P:Adapter"/> property.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "PropertyDescriptor is immutable.")]
        public static readonly PropertyDescriptor AdapterProperty = PropertyDescriptor.Create(nameof(Adapter), typeof(ListBoxAdapter), typeof(ListBox));

        /// <summary>
        /// Describes the <see cref="P:Background"/> property.  This field is read-only.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "PropertyDescriptor is immutable.")]
        public static readonly PropertyDescriptor BackgroundProperty = PropertyDescriptor.Create(nameof(Background), typeof(Brush), typeof(ListBox));

        /// <summary>
        /// Describes the <see cref="P:ContentOffset"/> property.  This field is read-only.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "PropertyDescriptor is immutable.")]
        public static readonly PropertyDescriptor ContentOffsetProperty = PropertyDescriptor.Create(nameof(ContentOffset), typeof(Point), typeof(ListBox), true);

        /// <summary>
        /// Describes the <see cref="P:ContentSize"/> property.  This field is read-only.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "PropertyDescriptor is immutable.")]
        public static readonly PropertyDescriptor ContentSizeProperty = PropertyDescriptor.Create(nameof(ContentSize), typeof(Size), typeof(ListBox), true);

        /// <summary>
        /// Describes the <see cref="P:IsSectioningEnabled"/> property.  This field is read-only.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "PropertyDescriptor is immutable.")]
        public static readonly PropertyDescriptor IsSectioningEnabledProperty = PropertyDescriptor.Create(nameof(IsSectioningEnabled), typeof(bool), typeof(ListBox));

        /// <summary>
        /// Describes the <see cref="P:Items"/> property.  This field is read-only.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "PropertyDescriptor is immutable.")]
        public static readonly PropertyDescriptor ItemsProperty = PropertyDescriptor.Create(nameof(Items), typeof(IList), typeof(ListBox));

        /// <summary>
        /// Describes the <see cref="P:SelectedItems"/> property.  This field is read-only.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "PropertyDescriptor is immutable.")]
        public static readonly PropertyDescriptor SelectedItemsProperty = PropertyDescriptor.Create(nameof(SelectedItems), typeof(IList), typeof(ListBox), true);

        /// <summary>
        /// Describes the <see cref="P:SelectionMode"/> property.  This field is read-only.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "PropertyDescriptor is immutable.")]
        public static readonly PropertyDescriptor SelectionModeProperty = PropertyDescriptor.Create(nameof(SelectionMode), typeof(SelectionMode), typeof(ListBox));

        /// <summary>
        /// Describes the <see cref="P:SeparatorBrush"/> property.  This field is read-only.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "PropertyDescriptor is immutable.")]
        public static readonly PropertyDescriptor SeparatorBrushProperty = PropertyDescriptor.Create(nameof(SeparatorBrush), typeof(Brush), typeof(ListBox));
        #endregion

        /// <summary>
        /// Occurs when an accessory in a <see cref="ListBoxItem"/> is selected.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification = "Event handler provides a strongly-typed sender for easier use.")]
        public event TypedEventHandler<ListBox, AccessorySelectedEventArgs> AccessorySelected;

        /// <summary>
        /// Occurs when the selection of the list box is changed.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification = "Event handler provides a strongly-typed sender for easier use.")]
        public event TypedEventHandler<ListBox, SelectionChangedEventArgs> SelectionChanged;

        /// <summary>
        /// Gets or sets the object responsible for translating data objects into UI elements for display within the list box.
        /// </summary>
        public ListBoxAdapter Adapter
        {
            get { return adapter; }
            set
            {
                if (value != adapter)
                {
                    adapter = value;
                    OnPropertyChanged(AdapterProperty);

                    if (nativeObject.Items != null && nativeObject.Items.Count > 0)
                    {
                        Reload();
                    }
                }
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ListBoxAdapter adapter;

        /// <summary>
        /// Gets or sets the background of the list box.
        /// </summary>
        public Brush Background
        {
            get { return nativeObject.Background; }
            set { nativeObject.Background = value; }
        }

        /// <summary>
        /// Gets the distance that the contents of the list box has been scrolled.
        /// </summary>
        public Point ContentOffset
        {
            get { return nativeObject.ContentOffset; }
        }

        /// <summary>
        /// Gets the size of the scrollable area within the list box.
        /// </summary>
        public Size ContentSize
        {
            get { return nativeObject.ContentSize; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether each object in the <see cref="P:Items"/> collection represents a different section in the list.
        /// When <c>true</c>, objects that implement <see cref="IList"/> will have each of their items represent a different entry in the same section.
        /// </summary>
        public bool IsSectioningEnabled
        {
            get { return nativeObject.IsSectioningEnabled; }
            set { nativeObject.IsSectioningEnabled = value; }
        }

        /// <summary>
        /// Gets or sets the items that make up the contents of the list box.
        /// Items that implement the <see cref="IList"/> interface will be treated as different sections if <see cref="P:IsSectioningEnabled"/> is <c>true</c>.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Consuming developers should be able to specify the type of collection they wish to use.")]
        public IList Items
        {
            get { return nativeObject.Items; }
            set { nativeObject.Items = value; }
        }

        /// <summary>
        /// Gets the currently selected items.
        /// To programmatically select and deselect items, use the <see cref="M:SelectItem"/> and <see cref="M:DeselectItem"/> methods.
        /// </summary>
        public IList SelectedItems
        {
            get { return nativeObject.SelectedItems; }
        }

        /// <summary>
        /// Gets or sets the selection behavior for the list box.
        /// </summary>
        public SelectionMode SelectionMode
        {
            get { return nativeObject.SelectionMode; }
            set { nativeObject.SelectionMode = value; }
        }

        /// <summary>
        /// Gets or sets the <see cref="Brush"/> to apply to the separators between each item in the list.
        /// </summary>
        public Brush SeparatorBrush
        {
            get { return nativeObject.SeparatorBrush; }
            set { nativeObject.SeparatorBrush = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the contents of the list box can be scrolled horizontally.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Functionality not officially supported and should not be used.")]
        bool IScrollable.CanScrollHorizontally
        {
            get { return nativeObject.CanScrollHorizontally; }
            set { nativeObject.CanScrollHorizontally = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the contents of the list box can be scrolled vertically.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Functionality not officially supported and should not be used.")]
        bool IScrollable.CanScrollVertically
        {
            get { return nativeObject.CanScrollVertically; }
            set { nativeObject.CanScrollVertically = value; }
        }

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        // this field is to avoid casting
        private readonly INativeListBox nativeObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListBox"/> class.
        /// </summary>
        public ListBox()
            : this(typeof(INativeListBox), null, new ResolveParameter("style", ListBoxStyle.Default))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListBox"/> class.
        /// </summary>
        /// <param name="style">The style in which to render the list box.</param>
        public ListBox(ListBoxStyle style)
            : this(typeof(INativeListBox), null, new ResolveParameter(nameof(style), style))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListBox"/> class.
        /// </summary>
        /// <param name="resolveType">The type to pass to the IoC container in order to resolve the native object.</param>
        /// <param name="resolveName">An optional name to use when resolving the native object.</param>
        /// <param name="resolveParameters">Any parameters to pass along to the constructor of the resolve type.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="resolveType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="resolveType"/> does not resolve to an <see cref="INativeListBox"/> instance.</exception>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", Justification = "resolveType is validated in base constructor.")]
        protected ListBox(Type resolveType, string resolveName, params ResolveParameter[] resolveParameters)
            : base(resolveType, resolveName, resolveParameters)
        {
            nativeObject = ObjectRetriever.GetNativeObject(this) as INativeListBox;
            if (nativeObject == null)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.Strings.TypeMustResolveToType, resolveType.FullName, typeof(INativeListBox).FullName), nameof(resolveType));
            }

            nativeObject.AccessorySelected += (o, e) => OnAccessorySelected(e);
            nativeObject.SelectionChanged += (o, e) => OnSelectionChanged(e);

            nativeObject.ItemIdRequest = (value) =>
            {
                if (adapter == null)
                {
                    return (value is Element) ? value.GetType().FullName : string.Empty;
                }
                else
                {
                    string id = adapter.GetItemId(value);
                    if (id == null)
                    {
                        throw new InvalidOperationException(Resources.Strings.NullReuseIdReturned);
                    }

                    return id;
                }
            };

            nativeObject.ItemRequest = (value, reusedItem) =>
            {
                var agnosticItem = ObjectRetriever.GetAgnosticObject(reusedItem) as ListBoxItem;
                if (agnosticItem != null)
                {
                    agnosticItem.OnReusing();
                }

                if (adapter != null)
                {
                    return ObjectRetriever.GetNativeObject(adapter.GetItem(value, agnosticItem)) as INativeListBoxItem;
                }

                var item = ObjectRetriever.GetNativeObject(ListBoxAdapter.GetDefaultItem(value, agnosticItem)) as INativeListBoxItem;
                item.Accessory = SelectionChanged == null ? ListBoxItemAccessory.None : ListBoxItemAccessory.Indicator;
                return item;
            };

            nativeObject.SectionHeaderRequest = (value, reusedItem) =>
            {
                var agnosticItem = ObjectRetriever.GetAgnosticObject(reusedItem) as ListBoxSectionHeader;
                if (agnosticItem != null)
                {
                    agnosticItem.OnReusing();
                }

                if (adapter != null)
                {
                    return ObjectRetriever.GetNativeObject(adapter.GetSectionHeader(value, agnosticItem)) as INativeListBoxSectionHeader;
                }

                return ObjectRetriever.GetNativeObject(ListBoxAdapter.GetDefaultSectionHeader(value, agnosticItem)) as INativeListBoxSectionHeader;
            };

            nativeObject.SectionHeaderIdRequest = (value) =>
            {
                if (adapter == null)
                {
                    return string.Empty;
                }
                else
                {
                    string id = adapter.GetSectionHeaderId(value);
                    if (id == null)
                    {
                        throw new InvalidOperationException(Resources.Strings.NullReuseIdReturned);
                    }

                    return id;
                }
            };

            nativeObject.CanScrollHorizontally = false;
            nativeObject.CanScrollVertically = true;
            HorizontalAlignment = HorizontalAlignment.Stretch;
            VerticalAlignment = VerticalAlignment.Stretch;
            SelectionMode = SelectionMode.Single;
        }

        /// <summary>
        /// Deselects the specified item.
        /// </summary>
        /// <param name="item">The item within the <see cref="P:Items"/> collection to deselect.</param>
        /// <param name="animate">Whether to animate the deselection.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="item"/> is <c>null</c>.</exception>
        public void DeselectItem(object item, Animate animate)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            nativeObject.DeselectItem(item, animate);
        }

        /// <summary>
        /// Forces a reload of the list box's entire contents.
        /// </summary>
        public void Reload()
        {
            nativeObject.Reload();
        }

        /// <summary>
        /// Scrolls to the specified item.
        /// </summary>
        /// <param name="item">The item within the <see cref="P:Items"/> collection to which the list box should scroll.</param>
        /// <param name="animate">Whether to animate the scrolling.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="item"/> is <c>null</c>.</exception>
        public void ScrollTo(object item, Animate animate)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            nativeObject.ScrollTo(item, animate);
        }

        /// <summary>
        /// Scrolls the contents within the list box to the specified offset.
        /// </summary>
        /// <param name="offset">The position to which to scroll the contents.</param>
        /// <param name="animate">Whether to animate the scrolling.</param>
        public void ScrollTo(Point offset, Animate animate)
        {
            ((IScrollable)nativeObject).ScrollTo(offset, animate);
        }

        /// <summary>
        /// Selects the specified item.
        /// </summary>
        /// <param name="item">The item within the <see cref="P:Items"/> collection to select.</param>
        /// <param name="animate">Whether to animate the selection.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="item"/> is <c>null</c>.</exception>
        public void SelectItem(object item, Animate animate)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            nativeObject.SelectItem(item, animate);
        }

        /// <summary>
        /// Called when an accessory in a <see cref="ListBoxItem"/> is selected and raises the <see cref="AccessorySelected"/> event.
        /// </summary>
        /// <param name="e">The event arguments containing the selection details.</param>
        protected virtual void OnAccessorySelected(AccessorySelectedEventArgs e)
        {
            AccessorySelected?.Invoke(this, e);
        }

        /// <summary>
        /// Called when the selection of the list box is changed and raises the <see cref="SelectionChanged"/> event.
        /// </summary>
        /// <param name="e">The event arguments containing the selection details.</param>
        protected virtual void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            SelectionChanged?.Invoke(this, e);
        }
    }
}