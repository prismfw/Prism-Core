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


using Prism.Native;
using Prism.UI;
using Prism.UI.Controls;

#if !DEBUG
using System.Diagnostics;
#endif

namespace Prism
{
    /// <summary>
    /// Provides a set of system constants, defaults, and preferences.
    /// </summary>
    public static class SystemParameters
    {
        /// <summary>
        /// Gets the default maximum number of items that can be displayed in an <see cref="ActionMenu"/> before they are placed into an overflow menu.
        /// </summary>
        public static int ActionMenuMaxDisplayItems
        {
            get { return Current.ActionMenuMaxDisplayItems; }
        }
    
        /// <summary>
        /// Gets the preferred amount of space between the bottom of a UI element and the bottom of its parent.
        /// </summary>
        public static double BottomMargin
        {
            get { return Current.BottomMargin; }
        }

        /// <summary>
        /// Gets the preferred width of the border around a <see cref="Button"/>.
        /// </summary>
        public static double ButtonBorderWidth
        {
            get { return Current.ButtonBorderWidth; }
        }

        /// <summary>
        /// Gets the preferred amount of padding between a <see cref="Button"/>'s content and its edges.
        /// </summary>
        public static Thickness ButtonPadding
        {
            get { return Current.ButtonPadding; }
        }

        /// <summary>
        /// Gets the amount that a header is inset on top of the content of a <see cref="ContentView"/> while in landscape orientation.
        /// </summary>
        public static Thickness ContentViewHeaderInsetLandscape
        {
            get { return Current.ContentViewHeaderInsetLandscape; }
        }

        /// <summary>
        /// Gets the amount that a header is inset on top of the content of a <see cref="ContentView"/> while in portrait orientation.
        /// </summary>
        public static Thickness ContentViewHeaderInsetPortrait
        {
            get { return Current.ContentViewHeaderInsetPortrait; }
        }

        /// <summary>
        /// Gets the amount that a header offsets the content of a <see cref="ContentView"/> while in landscape orientation.
        /// </summary>
        public static Thickness ContentViewHeaderOffsetLandscape
        {
            get { return Current.ContentViewHeaderOffsetLandscape; }
        }

        /// <summary>
        /// Gets the amount that a header offsets the content of a <see cref="ContentView"/> while in portrait orientation.
        /// </summary>
        public static Thickness ContentViewHeaderOffsetPortrait
        {
            get { return Current.ContentViewHeaderOffsetPortrait; }
        }

        /// <summary>
        /// Gets the preferred width of the border around a <see cref="DatePicker"/>.
        /// </summary>
        public static double DatePickerBorderWidth
        {
            get { return Current.DatePickerBorderWidth; }
        }

        /// <summary>
        /// Gets the height of a horizontal scroll bar.
        /// </summary>
        public static double HorizontalScrollBarHeight
        {
            get { return Current.HorizontalScrollBarHeight; }
        }

        /// <summary>
        /// Gets the preferred amount of space between the left edge of a UI element and the left edge of its parent.
        /// </summary>
        public static double LeftMargin
        {
            get { return Current.LeftMargin; }
        }

        /// <summary>
        /// Gets the preferred height of a <see cref="ListBoxItem"/> with a <see cref="P:DetailTextLabel"/>.
        /// </summary>
        public static double ListBoxItemDetailHeight
        {
            get { return Current.ListBoxItemDetailHeight; }
        }

        /// <summary>
        /// Gets the size of the <see cref="ListBoxItemAccessory.Indicator"/> accessory in a <see cref="ListBoxItem"/>.
        /// </summary>
        public static Size ListBoxItemIndicatorSize
        {
            get { return Current.ListBoxItemIndicatorSize; }
        }

        /// <summary>
        /// Gets the size of the <see cref="ListBoxItemAccessory.InfoButton"/> accessory in a <see cref="ListBoxItem"/>.
        /// </summary>
        public static Size ListBoxItemInfoButtonSize
        {
            get { return Current.ListBoxItemInfoButtonSize; }
        }

        /// <summary>
        /// Gets the size of the <see cref="ListBoxItemAccessory.InfoIndicator"/> accessory in a <see cref="ListBoxItem"/>.
        /// </summary>
        public static Size ListBoxItemInfoIndicatorSize
        {
            get { return Current.ListBoxItemInfoIndicatorSize; }
        }

        /// <summary>
        /// Gets the preferred height of a standard <see cref="ListBoxItem"/>.
        /// </summary>
        public static double ListBoxItemStandardHeight
        {
            get { return Current.ListBoxItemStandardHeight; }
        }

        /// <summary>
        /// Gets the preferred width of the border around a <see cref="PasswordBox"/>.
        /// </summary>
        public static double PasswordBoxBorderWidth
        {
            get { return Current.PasswordBoxBorderWidth; }
        }

        /// <summary>
        /// Gets the size of a <see cref="Popup"/> when presented with <see cref="PopupPresentationStyle.Default"/>.
        /// </summary>
        public static Size PopupSize
        {
            get { return Current.PopupSize; }
        }

        /// <summary>
        /// Gets the preferred amount of space between the right edge of a UI element and the right edge of its parent.
        /// </summary>
        public static double RightMargin
        {
            get { return Current.RightMargin; }
        }

        /// <summary>
        /// Gets the preferred width of the border around a <see cref="SearchBox"/>.
        /// </summary>
        public static double SearchBoxBorderWidth
        {
            get { return Current.SearchBoxBorderWidth; }
        }

        /// <summary>
        /// Gets the preferred width of the border around a <see cref="SelectList"/>.
        /// </summary>
        public static double SelectListBorderWidth
        {
            get { return Current.SelectListBorderWidth; }
        }

        /// <summary>
        /// Gets the default amount of padding between a <see cref="SelectList"/>'s display item and its edges.
        /// </summary>
        public static Thickness SelectListDisplayItemPadding
        {
            get { return Current.SelectListDisplayItemPadding; }
        }

        /// <summary>
        /// Gets the default amount of padding between a <see cref="SelectList"/>'s list items and its edges.
        /// </summary>
        public static Thickness SelectListListItemPadding
        {
            get { return Current.SelectListListItemPadding; }
        }

        /// <summary>
        /// Gets a value indicating whether the separator of a <see cref="ListBoxItem"/> should be automatically indented
        /// in order to be flush with the text labels of the item.
        /// </summary>
        public static bool ShouldAutomaticallyIndentSeparators
        {
            get { return Current.ShouldAutomaticallyIndentSeparators; }
        }

        /// <summary>
        /// Gets the preferred width of the border around a <see cref="TextArea"/>.
        /// </summary>
        public static double TextAreaBorderWidth
        {
            get { return Current.TextAreaBorderWidth; }
        }

        /// <summary>
        /// Gets the preferred width of the border around a <see cref="TextBox"/>.
        /// </summary>
        public static double TextBoxBorderWidth
        {
            get { return Current.TextBoxBorderWidth; }
        }

        /// <summary>
        /// Gets the preferred width of the border around a <see cref="TimePicker"/>.
        /// </summary>
        public static double TimePickerBorderWidth
        {
            get { return Current.TimePickerBorderWidth; }
        }

        /// <summary>
        /// Gets the preferred amount of space between the top of a UI element and the top of its parent.
        /// </summary>
        public static double TopMargin
        {
            get { return Current.TopMargin; }
        }

        /// <summary>
        /// Gets the width of a vertical scroll bar.
        /// </summary>
        public static double VerticalScrollBarWidth
        {
            get { return Current.VerticalScrollBarWidth; }
        }

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private static INativeSystemParameters Current
        {
            get { return Application.Resolve<INativeSystemParameters>(); }
        }
    }
}
