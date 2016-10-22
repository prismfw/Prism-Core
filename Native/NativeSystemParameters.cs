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


namespace Prism.Native
{
    /// <summary>
    /// Defines a set of system constants, defaults, and preferences for a particular platform.
    /// </summary>
    [CoreBehavior(CoreBehaviors.ExpectsSingleton)]
    public interface INativeSystemParameters
    {
        /// <summary>
        /// Gets the default maximum number of items that can be displayed in an action menu before they are placed into an overflow menu.
        /// </summary>
        int ActionMenuMaxDisplayItems { get; }
    
        /// <summary>
        /// Gets the preferred amount of space between the bottom of a UI element and the bottom of its parent.
        /// </summary>
        double BottomMargin { get; }

        /// <summary>
        /// Gets the preferred width of the border around a button.
        /// </summary>
        double ButtonBorderWidth { get; }

        /// <summary>
        /// Gets the preferred amount of padding between a button's content and its edges.
        /// </summary>
        Thickness ButtonPadding { get; }

        /// <summary>
        /// Gets the preferred width of the border around a date picker.
        /// </summary>
        double DatePickerBorderWidth { get; }

        /// <summary>
        /// Gets the height of a horizontal scroll bar.
        /// </summary>
        double HorizontalScrollBarHeight { get; }

        /// <summary>
        /// Gets the preferred amount of space between the left edge of a UI element and the left edge of its parent.
        /// </summary>
        double LeftMargin { get; }

        /// <summary>
        /// Gets the preferred height of a list box item with a detail text label.
        /// </summary>
        double ListBoxItemDetailHeight { get; }

        /// <summary>
        /// Gets the size of the indicator accessory in a list box item.
        /// </summary>
        Size ListBoxItemIndicatorSize { get; }

        /// <summary>
        /// Gets the size of the info button accessory in a list box item.
        /// </summary>
        Size ListBoxItemInfoButtonSize { get; }

        /// <summary>
        /// Gets the size of the info indicator accessory in a list box item.
        /// </summary>
        Size ListBoxItemInfoIndicatorSize { get; }

        /// <summary>
        /// Gets the preferred height of a standard list box item.
        /// </summary>
        double ListBoxItemStandardHeight { get; }

        /// <summary>
        /// Gets the preferred width of the border around a password box.
        /// </summary>
        double PasswordBoxBorderWidth { get; }

        /// <summary>
        /// Gets the size of a popup when presented with the default style.
        /// </summary>
        Size PopupSize { get; }

        /// <summary>
        /// Gets the preferred amount of space between the right edge of a UI element and the right edge of its parent.
        /// </summary>
        double RightMargin { get; }

        /// <summary>
        /// Gets the preferred width of the border around a search box.
        /// </summary>
        double SearchBoxBorderWidth { get; }

        /// <summary>
        /// Gets the preferred width of the border around a select list.
        /// </summary>
        double SelectListBorderWidth { get; }

        /// <summary>
        /// Gets the default amount of padding between a select list's display item and its edges.
        /// </summary>
        Thickness SelectListDisplayItemPadding { get; }

        /// <summary>
        /// Gets the default amount of padding between a select list's list items and its edges.
        /// </summary>
        Thickness SelectListListItemPadding { get; }

        /// <summary>
        /// Gets the preferred width of the border around a text area.
        /// </summary>
        double TextAreaBorderWidth { get; }

        /// <summary>
        /// Gets the preferred width of the border around a text box.
        /// </summary>
        double TextBoxBorderWidth { get; }

        /// <summary>
        /// Gets a value indicating whether the separator of a list box item should be automatically indented
        /// in order to be flush with the text labels of the item.
        /// </summary>
        bool ShouldAutomaticallyIndentSeparators { get; }

        /// <summary>
        /// Gets the preferred width of the border around a time picker.
        /// </summary>
        double TimePickerBorderWidth { get; }

        /// <summary>
        /// Gets the preferred amount of space between the top of a UI element and the top of its parent.
        /// </summary>
        double TopMargin { get; }

        /// <summary>
        /// Gets the width of a vertical scroll bar.
        /// </summary>
        double VerticalScrollBarWidth { get; }

        /// <summary>
        /// Gets the amount that a header is inset on top of the current view of a view stack while in landscape orientation.
        /// </summary>
        Thickness ViewStackHeaderInsetLandscape { get; }

        /// <summary>
        /// Gets the amount that a header is inset on top of the current view of a view stack while in portrait orientation.
        /// </summary>
        Thickness ViewStackHeaderInsetPortrait { get; }

        /// <summary>
        /// Gets the amount that a header offsets the current view of a view stack while in landscape orientation.
        /// </summary>
        Thickness ViewStackHeaderOffsetLandscape { get; }

        /// <summary>
        /// Gets the amount that a header offsets the current view of a view stack while in portrait orientation.
        /// </summary>
        Thickness ViewStackHeaderOffsetPortrait { get; }
    }
}
