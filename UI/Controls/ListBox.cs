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
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Prism.Native;
using Prism.Resources;
using Prism.UI.Media;

namespace Prism.UI.Controls
{
    /// <summary>
    /// Represents a vertically-oriented, scrollable list of selectable items.
    /// </summary>
    [Resolve(typeof(INativeListBox))]
    public class ListBox : Element, IScrollable
    {
        #region Event Descriptors
        /// <summary>
        /// Gets an <see cref="EventDescriptor"/> describing the <see cref="E:AccessoryClicked"/> event.
        /// </summary>
        public static EventDescriptor AccessoryClickedEvent { get; } = EventDescriptor.Create(nameof(AccessoryClicked), typeof(TypedEventHandler<ListBox, AccessoryClickedEventArgs>), typeof(ListBox));

        /// <summary>
        /// Gets an <see cref="EventDescriptor"/> describing the <see cref="E:ItemClicked"/> event.
        /// </summary>
        public static EventDescriptor ItemClickedEvent { get; } = EventDescriptor.Create(nameof(ItemClicked), typeof(TypedEventHandler<ListBox, ItemClickedEventArgs>), typeof(ListBox));

        /// <summary>
        /// Gets an <see cref="EventDescriptor"/> describing the <see cref="E:SelectionChanged"/> event.
        /// </summary>
        public static EventDescriptor SelectionChangedEvent { get; } = EventDescriptor.Create(nameof(SelectionChanged), typeof(TypedEventHandler<ListBox, SelectionChangedEventArgs>), typeof(ListBox));
        #endregion

        #region Property Descriptors
        /// <summary>
        /// Describes the <see cref="P:Adapter"/> property.
        /// </summary>
        public static PropertyDescriptor AdapterProperty { get; } = PropertyDescriptor.Create(nameof(Adapter), typeof(ListBoxAdapter), typeof(ListBox));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:Background"/> property.
        /// </summary>
        public static PropertyDescriptor BackgroundProperty { get; } = PropertyDescriptor.Create(nameof(Background), typeof(Brush), typeof(ListBox));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:ContentOffset"/> property.
        /// </summary>
        public static PropertyDescriptor ContentOffsetProperty { get; } = PropertyDescriptor.Create(nameof(ContentOffset), typeof(Point), typeof(ListBox), true);

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:ContentSize"/> property.
        /// </summary>
        public static PropertyDescriptor ContentSizeProperty { get; } = PropertyDescriptor.Create(nameof(ContentSize), typeof(Size), typeof(ListBox), true);

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:IsSectioningEnabled"/> property.
        /// </summary>
        public static PropertyDescriptor IsSectioningEnabledProperty { get; } = PropertyDescriptor.Create(nameof(IsSectioningEnabled), typeof(bool), typeof(ListBox));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:Items"/> property.
        /// </summary>
        public static PropertyDescriptor ItemsProperty { get; } = PropertyDescriptor.Create(nameof(Items), typeof(IList), typeof(ListBox));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:SelectedItems"/> property.
        /// </summary>
        public static PropertyDescriptor SelectedItemsProperty { get; } = PropertyDescriptor.Create(nameof(SelectedItems), typeof(IList), typeof(ListBox), true);

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:SelectionMode"/> property.
        /// </summary>
        public static PropertyDescriptor SelectionModeProperty { get; } = PropertyDescriptor.Create(nameof(SelectionMode), typeof(SelectionMode), typeof(ListBox));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:SeparatorBrush"/> property.
        /// </summary>
        public static PropertyDescriptor SeparatorBrushProperty { get; } = PropertyDescriptor.Create(nameof(SeparatorBrush), typeof(Brush), typeof(ListBox));
        #endregion

        /// <summary>
        /// Occurs when an accessory in a <see cref="ListBoxItem"/> is clicked or tapped.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification = "Event handler provides a strongly-typed sender for easier use.")]
        public event TypedEventHandler<ListBox, AccessoryClickedEventArgs> AccessoryClicked;

        /// <summary>
        /// Occurs when an item in the list box is clicked or tapped.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification = "Event handler provides a strongly-typed sender for easier use.")]
        public event TypedEventHandler<ListBox, ItemClickedEventArgs> ItemClicked;

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
        /// Gets the style of the list box.
        /// </summary>
        public ListBoxStyle Style { get; }

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
            : this(new[] { new ResolveParameter("style", ListBoxStyle.Default) })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListBox"/> class.
        /// </summary>
        /// <param name="style">The style in which to render the list box.</param>
        public ListBox(ListBoxStyle style)
            : this(new[] { new ResolveParameter(nameof(style), style) })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListBox"/> class and pairs it with the specified native object.
        /// </summary>
        /// <param name="nativeObject">The native object with which to pair this instance.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="nativeObject"/> doesn't match the type specified by the topmost <see cref="ResolveAttribute"/> in the inheritance chain.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="nativeObject"/> is <c>null</c>.</exception>
        protected ListBox(INativeListBox nativeObject)
            : base(nativeObject)
        {
            this.nativeObject = nativeObject;

            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListBox"/> class and pairs it with a native object that is resolved from the IoC container.
        /// </summary>
        /// <param name="resolveParameters">Any parameters to pass along to the constructor of the native type.</param>
        /// <exception cref="TypeResolutionException">Thrown when the native object does not resolve to an <see cref="INativeListBox"/> instance.</exception>
        protected ListBox(ResolveParameter[] resolveParameters)
            : base(resolveParameters)
        {
            nativeObject = ObjectRetriever.GetNativeObject(this) as INativeListBox;
            if (nativeObject == null)
            {
                throw new TypeResolutionException(string.Format(CultureInfo.CurrentCulture, Strings.TypeMustResolveToType,
                    ObjectRetriever.GetNativeObject(this).GetType().FullName, typeof(INativeListBox).FullName));
            }

            if (resolveParameters != null)
            {
                for (int i = 0; i < resolveParameters.Length; i++)
                {
                    var parameter = resolveParameters[i];
                    if (parameter.ParameterName == "style" && parameter.ParameterValue is ListBoxStyle)
                    {
                        Style = (ListBoxStyle)parameter.ParameterValue;
                        break;
                    }
                }
            }

            Initialize();
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
        /// Called when an accessory in a <see cref="ListBoxItem"/> is clicked or tapped and raises the <see cref="AccessoryClicked"/> event.
        /// </summary>
        /// <param name="e">The event arguments containing the selection details.</param>
        protected virtual void OnAccessoryClicked(AccessoryClickedEventArgs e)
        {
            AccessoryClicked?.Invoke(this, e);
        }

        /// <summary>
        /// Called when an item in the list box is clicked or tapped and raises the <see cref="ItemClicked"/> event.
        /// </summary>
        /// <param name="e">The event arguments containing the selection details.</param>
        protected virtual void OnItemClicked(ItemClickedEventArgs e)
        {
            ItemClicked?.Invoke(this, e);
        }

        /// <summary>
        /// Called when the selection of the list box is changed and raises the <see cref="SelectionChanged"/> event.
        /// </summary>
        /// <param name="e">The event arguments containing the selection details.</param>
        protected virtual void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            SelectionChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Called when this instance is ready to arrange its children and returns the final rendering size of the object.
        /// </summary>
        /// <param name="constraints">The width and height that this instance should not exceed.</param>
        /// <returns>The final rendering size of the object as a <see cref="Size"/> instance.</returns>
        protected override Size ArrangeOverride(Size constraints)
        {
            var renderSize = constraints = base.ArrangeOverride(constraints);
            foreach (var item in nativeObject.GetChildItems())
            {
                var agnostic = ObjectRetriever.GetAgnosticObject(item) as Visual;
                if (agnostic != null)
                {
                    agnostic.Arrange(new Rectangle(0, item.Frame.Y, constraints.Width, agnostic.DesiredSize.Height));
                }
            }

            return renderSize;
        }

        /// <summary>
        /// Called when this instance is ready to be measured and returns the desired size of the object.
        /// </summary>
        /// <param name="constraints">The width and height that this instance should not exceed.</param>
        /// <returns>The desired size of the object as a <see cref="Size"/> instance.</returns>
        protected override Size MeasureOverride(Size constraints)
        {
            constraints = base.MeasureOverride(constraints);
            foreach (var item in nativeObject.GetChildItems())
            {
                (ObjectRetriever.GetAgnosticObject(item) as Visual)?.Measure(new Size(constraints.Width, double.PositiveInfinity));
            }

            return constraints;
        }

        private void Initialize()
        {
            nativeObject.AccessoryClicked += (o, e) => OnAccessoryClicked(e);
            nativeObject.ItemClicked += (o, e) => OnItemClicked(e);
            nativeObject.SelectionChanged += (o, e) => OnSelectionChanged(e);

            nativeObject.ItemIdRequest = (value) =>
            {
                if (adapter == null)
                {
                    return (value is Element) ? value.GetType().FullName : string.Empty;
                }
                else
                {
                    string id = adapter.GetItemId(value, this);
                    if (id == null)
                    {
                        throw new InvalidOperationException(Strings.NullReuseIdReturned);
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
                    return ObjectRetriever.GetNativeObject(adapter.GetItem(value, agnosticItem, this)) as INativeListBoxItem;
                }

                var item = ObjectRetriever.GetNativeObject(ListBoxAdapter.GetDefaultItem(value, agnosticItem, this)) as INativeListBoxItem;
                if (AccessoryClicked == null)
                {
                    item.Accessory = ItemClicked == null ? ListBoxItemAccessory.None : ListBoxItemAccessory.Indicator;
                }
                else
                {
                    item.Accessory = ItemClicked == null ? ListBoxItemAccessory.InfoButton : ListBoxItemAccessory.InfoIndicator;
                }
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
                    return ObjectRetriever.GetNativeObject(adapter.GetSectionHeader(value, agnosticItem, this)) as INativeListBoxSectionHeader;
                }

                return ObjectRetriever.GetNativeObject(ListBoxAdapter.GetDefaultSectionHeader(value, agnosticItem, this)) as INativeListBoxSectionHeader;
            };

            nativeObject.SectionHeaderIdRequest = (value) =>
            {
                if (adapter == null)
                {
                    return string.Empty;
                }
                else
                {
                    string id = adapter.GetSectionHeaderId(value, this);
                    if (id == null)
                    {
                        throw new InvalidOperationException(Strings.NullReuseIdReturned);
                    }

                    return id;
                }
            };

            nativeObject.CanScrollHorizontally = false;
            nativeObject.CanScrollVertically = true;
            HorizontalAlignment = HorizontalAlignment.Stretch;
            VerticalAlignment = VerticalAlignment.Stretch;
            SelectionMode = SelectionMode.Single;

            SetResourceReference(BackgroundProperty, SystemResources.ListBoxBackgroundBrushKey);
            SetResourceReference(SeparatorBrushProperty, SystemResources.ListBoxSeparatorBrushKey);
        }
    }
}
