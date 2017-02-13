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
using System.Diagnostics.CodeAnalysis;
using Prism.UI.Controls;
using Prism.UI.Media;

namespace Prism.Native
{
    /// <summary>
    /// Represents the method that is invoked when a display item is requested for a select list.
    /// </summary>
    /// <returns>The display item.</returns>
    public delegate object SelectListDisplayItemRequestHandler();

    /// <summary>
    /// Represents the method that is invoked when a list item is requested for an object in a select list.
    /// </summary>
    /// <param name="value">The object from which to get a list item.</param>
    /// <returns>The list item.</returns>
    public delegate object SelectListListItemRequestHandler(object value);

    /// <summary>
    /// Defines a select list that is native to a particular platform.
    /// These objects are meant to be paired with platform-agnostic <see cref="SelectList"/> objects.
    /// </summary>
    public interface INativeSelectList : INativeControl
    {
        /// <summary>
        /// Occurs when the selection of the select list is changed.
        /// </summary>
        event EventHandler<SelectionChangedEventArgs> SelectionChanged;

        /// <summary>
        /// Gets or sets the method to invoke when this instance requests a display item for the select list.
        /// </summary>
        SelectListDisplayItemRequestHandler DisplayItemRequest { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the list is open for selection.
        /// </summary>
        bool IsOpen { get; set; }

        /// <summary>
        /// Gets or sets a list of the items that make up the selection list.
        /// </summary>
        [CoreBehavior(CoreBehaviors.ExpectsEarlyChangeNotification)]
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Consuming developers should be able to specify the type of collection they wish to use.")]
        IList Items { get; set; }

        /// <summary>
        /// Gets or sets the background of the selection list.
        /// </summary>
        Brush ListBackground { get; set; }

        /// <summary>
        /// Gets or sets the method to invoke when this instance requests a list item for an object in the select list.
        /// </summary>
        SelectListListItemRequestHandler ListItemRequest { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Brush"/> to apply to the separators in the selection list, if applicable.
        /// </summary>
        Brush ListSeparatorBrush { get; set; }

        /// <summary>
        /// Gets or sets the zero-based index of the selected item.
        /// </summary>
        [CoreBehavior(CoreBehaviors.ExpectsEarlyChangeNotification)]
        int SelectedIndex { get; set; }

        /// <summary>
        /// Forces a refresh of the display item.
        /// </summary>
        void RefreshDisplayItem();

        /// <summary>
        /// Forces a refresh of the items in the selection list.
        /// </summary>
        void RefreshListItems();
    }
}
