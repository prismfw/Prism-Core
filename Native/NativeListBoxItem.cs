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


using Prism.UI.Controls;
using Prism.UI.Media;

namespace Prism.Native
{
    /// <summary>
    /// Defines a selectable item in an <see cref="INativeListBox"/> that is native to a particular platform.
    /// These objects are meant to be paired with platform-agnostic <see cref="ListBoxItem"/> objects.
    /// </summary>
    public interface INativeListBoxItem : INativeElement
    {
        /// <summary>
        /// Gets or sets the accessory for the item.
        /// </summary>
        ListBoxItemAccessory Accessory { get; set; }

        /// <summary>
        /// Gets or sets the background of the item.
        /// </summary>
        Brush Background { get; set; }

        /// <summary>
        /// Gets or sets the panel containing the content to be displayed by the item.
        /// </summary>
        INativePanel ContentPanel { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is selected.
        /// </summary>
        bool IsSelected { get; }

        /// <summary>
        /// Gets or sets the background of the item when it is selected.
        /// </summary>
        Brush SelectedBackground { get; set; }

        /// <summary>
        /// Gets or sets the amount to indent the separator.
        /// </summary>
        Thickness SeparatorIndentation { get; set; }
    }
}
