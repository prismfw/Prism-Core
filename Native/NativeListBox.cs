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
using System.Diagnostics.CodeAnalysis;
using Prism.UI;
using Prism.UI.Controls;
using Prism.UI.Media;

namespace Prism.Native
{
    /// <summary>
    /// Represents the method that is invoked when a reuse identifier is requested for an object.
    /// </summary>
    /// <param name="value">The object from which to get a reuse identifier.</param>
    /// <returns>The reuse identifier as a <see cref="string"/>.</returns>
    public delegate string ItemIdRequestHandler(object value);

    /// <summary>
    /// Represents the method that is invoked when a display item is requested for an object in a list box.
    /// </summary>
    /// <param name="value">The object from which to get a display item.</param>
    /// <param name="reusedItem">A recycled display item, or <c>null</c> if no item was recycled.</param>
    /// <returns>The display item as an <see cref="INativeListBoxItem"/>.</returns>
    public delegate INativeListBoxItem ListBoxItemRequestHandler(object value, INativeListBoxItem reusedItem);

    /// <summary>
    /// Represents the method that is invoked when a section header is requested above an object in a list box.
    /// </summary>
    /// <param name="value">The object underneath the section header.</param>
    /// <param name="reusedItem">A recycled section header, or <c>null</c> if no section header was recycled.</param>
    /// <returns>The section header as an <see cref="INativeListBoxSectionHeader"/>.</returns>
    public delegate INativeListBoxSectionHeader ListBoxSectionHeaderRequestHandler(object value, INativeListBoxSectionHeader reusedItem);

    /// <summary>
    /// Defines a vertical list of selectable items that is native to a particular platform.
    /// These objects are meant to be paired with platform-agnostic <see cref="ListBox"/> objects.
    /// </summary>
    public interface INativeListBox : INativeElement, IScrollable
    {
        /// <summary>
        /// Occurs when an accessory in a list box item is clicked or tapped.
        /// </summary>
        event EventHandler<AccessoryClickedEventArgs> AccessoryClicked;

        /// <summary>
        /// Occurs when an item in the list box is clicked or tapped.
        /// </summary>
        event EventHandler<ItemClickedEventArgs> ItemClicked;

        /// <summary>
        /// Occurs when the selection of the list box is changed.
        /// </summary>
        event EventHandler<SelectionChangedEventArgs> SelectionChanged;

        /// <summary>
        /// Gets or sets the background of the list box.
        /// </summary>
        Brush Background { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether each object in the <see cref="P:Items"/> collection represents a different section in the list.
        /// When <c>true</c>, objects that implement <see cref="IList"/> will have each of their items represent a different entry in the same section.
        /// </summary>
        [CoreBehavior(CoreBehaviors.ExpectsEarlyChangeNotification)]
        bool IsSectioningEnabled { get; set; }

        /// <summary>
        /// Gets or sets the method to invoke when this instance requests a reuse identifier for an object in the list box.
        /// </summary>
        ItemIdRequestHandler ItemIdRequest { get; set; }

        /// <summary>
        /// Gets or sets the method to invoke when this instance requests a display item for an object in the list box.
        /// </summary>
        ListBoxItemRequestHandler ItemRequest { get; set; }

        /// <summary>
        /// Gets or sets the items that make up the contents of the list box.
        /// Items that implement the <see cref="IList"/> interface will be treated as different sections if <see cref="P:IsSectioningEnabled"/> is <c>true</c>.
        /// </summary>
        [CoreBehavior(CoreBehaviors.ExpectsEarlyChangeNotification)]
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Consuming developers should be able to specify the type of collection they wish to use.")]
        IList Items { get; set; }

        /// <summary>
        /// Gets or sets the method to invoke when this instance requests a section header in the list box.
        /// </summary>
        ListBoxSectionHeaderRequestHandler SectionHeaderRequest { get; set; }

        /// <summary>
        /// Gets or sets the method to invoke when this instance requests a reuse identifier for a section header.
        /// </summary>
        ItemIdRequestHandler SectionHeaderIdRequest { get; set; }

        /// <summary>
        /// Gets the currently selected items.
        /// To programmatically select and deselect items, use the <see cref="M:Select"/> and <see cref="M:Deselect"/> methods.
        /// </summary>
        IList SelectedItems { get; }

        /// <summary>
        /// Gets or sets the selection behavior for the list box.
        /// </summary>
        SelectionMode SelectionMode { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Brush"/> to apply to the separators between each item in the list.
        /// </summary>
        Brush SeparatorBrush { get; set; }

        /// <summary>
        /// Deselects the specified item.
        /// </summary>
        /// <param name="item">The item within the <see cref="P:Items"/> collection to deselect.</param>
        /// <param name="animate">Whether to animate the deselection.</param>
        void DeselectItem([CoreBehavior(CoreBehaviors.ChecksNullity)]object item, Animate animate);

        /// <summary>
        /// Returns a collection of the <see cref="INativeListBoxItem"/> objects that are in the list.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Depending on implementation, performance may be less than what is expected of a property.")]
        IEnumerable<INativeListBoxItem> GetChildItems();

        /// <summary>
        /// Forces a reload of the list box's entire contents.
        /// </summary>
        void Reload();

        /// <summary>
        /// Scrolls to the specified item.
        /// </summary>
        /// <param name="item">The item within the <see cref="P:Items"/> collection to which the list box should scroll.</param>
        /// <param name="animate">Whether to animate the scrolling.</param>
        void ScrollTo([CoreBehavior(CoreBehaviors.ChecksNullity)]object item, Animate animate);

        /// <summary>
        /// Selects the specified item.
        /// </summary>
        /// <param name="item">The item within the <see cref="P:Items"/> collection to select.</param>
        /// <param name="animate">Whether to animate the selection.</param>
        void SelectItem([CoreBehavior(CoreBehaviors.ChecksNullity)]object item, Animate animate);
    }
}
