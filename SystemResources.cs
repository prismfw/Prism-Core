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


using System.Diagnostics;
using Prism.Native;
using Prism.UI;
using Prism.UI.Controls;
using Prism.UI.Media;

namespace Prism
{
    /// <summary>
    /// Provides resource keys for system-defined resources such as brushes, fonts, measurements, and other parameters.
    /// </summary>
    public static class SystemResources
    {
        #region Measurements
        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the default maximum number of items that can be
        /// displayed in an <see cref="ActionMenu"/> before they are placed into an overflow menu.
        /// </summary>
        public static ResourceKey ActionMenuMaxDisplayItemsKey
        {
            get { return actionMenuMaxDisplayItemsKey ?? (actionMenuMaxDisplayItemsKey = new ResourceKey(SystemResourceKeyId.ActionMenuMaxDisplayItems, typeof(int))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey actionMenuMaxDisplayItemsKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving a value indicating whether the separator of a <see cref="ListBoxItem"/>
        /// should be automatically indented in order to be flush with the text labels of the item.
        /// </summary>
        public static ResourceKey ShouldAutomaticallyIndentSeparatorsKey
        {
            get { return shouldAutomaticallyIndentSeparatorsKey ?? (shouldAutomaticallyIndentSeparatorsKey = new ResourceKey(SystemResourceKeyId.ShouldAutomaticallyIndentSeparators, typeof(bool))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey shouldAutomaticallyIndentSeparatorsKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the default width of the border around a <see cref="Button"/>.
        /// </summary>
        public static ResourceKey ButtonBorderWidthKey
        {
            get { return buttonBorderWidthKey ?? (buttonBorderWidthKey = new ResourceKey(SystemResourceKeyId.ButtonBorderWidth, typeof(double))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey buttonBorderWidthKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the default amount of padding between a <see cref="Button"/>'s content and its edges.
        /// </summary>
        public static ResourceKey ButtonPaddingKey
        {
            get { return buttonPaddingKey ?? (buttonPaddingKey = new ResourceKey(SystemResourceKeyId.ButtonPadding, typeof(Thickness))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey buttonPaddingKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the default width of the border around a <see cref="DatePicker"/> or <see cref="TimePicker"/>.
        /// </summary>
        public static ResourceKey DateTimePickerBorderWidthKey
        {
            get { return dateTimePickerBorderWidthKey ?? (dateTimePickerBorderWidthKey = new ResourceKey(SystemResourceKeyId.DateTimePickerBorderWidth, typeof(double))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey dateTimePickerBorderWidthKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the height of a horizontal scroll bar.
        /// </summary>
        public static ResourceKey HorizontalScrollBarHeightKey
        {
            get { return horizontalScrollBarHeightKey ?? (horizontalScrollBarHeightKey = new ResourceKey(SystemResourceKeyId.HorizontalScrollBarHeight, typeof(double))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey horizontalScrollBarHeightKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the default height of a <see cref="ListBoxItem"/> with a <see cref="P:DetailTextLabel"/>.
        /// </summary>
        public static ResourceKey ListBoxItemDetailHeightKey
        {
            get { return listBoxItemDetailHeightKey ?? (listBoxItemDetailHeightKey = new ResourceKey(SystemResourceKeyId.ListBoxItemDetailHeight, typeof(double))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey listBoxItemDetailHeightKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the size of the <see cref="ListBoxItemAccessory.Indicator"/> accessory in a <see cref="ListBoxItem"/>.
        /// </summary>
        public static ResourceKey ListBoxItemIndicatorSizeKey
        {
            get { return listBoxItemIndicatorSizeKey ?? (listBoxItemIndicatorSizeKey = new ResourceKey(SystemResourceKeyId.ListBoxItemIndicatorSize, typeof(Size))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey listBoxItemIndicatorSizeKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the size of the <see cref="ListBoxItemAccessory.InfoButton"/> accessory in a <see cref="ListBoxItem"/>.
        /// </summary>
        public static ResourceKey ListBoxItemInfoButtonSizeKey
        {
            get { return listBoxItemInfoButtonSizeKey ?? (listBoxItemInfoButtonSizeKey = new ResourceKey(SystemResourceKeyId.ListBoxItemInfoButtonSize, typeof(Size))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey listBoxItemInfoButtonSizeKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the size of the <see cref="ListBoxItemAccessory.InfoIndicator"/> accessory in a <see cref="ListBoxItem"/>.
        /// </summary>
        public static ResourceKey ListBoxItemInfoIndicatorSizeKey
        {
            get { return listBoxItemInfoIndicatorSizeKey ?? (listBoxItemInfoIndicatorSizeKey = new ResourceKey(SystemResourceKeyId.ListBoxItemInfoIndicatorSize, typeof(Size))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey listBoxItemInfoIndicatorSizeKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the default height of a standard <see cref="ListBoxItem"/>.
        /// </summary>
        public static ResourceKey ListBoxItemStandardHeightKey
        {
            get { return listBoxItemStandardHeightKey ?? (listBoxItemStandardHeightKey = new ResourceKey(SystemResourceKeyId.ListBoxItemStandardHeight, typeof(double))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey listBoxItemStandardHeightKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the size of a <see cref="Popup"/> when presented with <see cref="PopupPresentationStyle.Default"/>.
        /// </summary>
        public static ResourceKey PopupSizeKey
        {
            get { return popupSizeKey ?? (popupSizeKey = new ResourceKey(SystemResourceKeyId.PopupSize, typeof(Size))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey popupSizeKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the default width of the border around a <see cref="SearchBox"/>.
        /// </summary>
        public static ResourceKey SearchBoxBorderWidthKey
        {
            get { return searchBoxBorderWidthKey ?? (searchBoxBorderWidthKey = new ResourceKey(SystemResourceKeyId.SearchBoxBorderWidth, typeof(double))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey searchBoxBorderWidthKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the default width of the border around a <see cref="SelectList"/>.
        /// </summary>
        public static ResourceKey SelectListBorderWidthKey
        {
            get { return selectListBorderWidthKey ?? (selectListBorderWidthKey = new ResourceKey(SystemResourceKeyId.SelectListBorderWidth, typeof(double))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey selectListBorderWidthKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the default amount of padding between a <see cref="SelectList"/>'s display item and its edges.
        /// </summary>
        public static ResourceKey SelectListDisplayItemPaddingKey
        {
            get { return selectListDisplayItemPaddingKey ?? (selectListDisplayItemPaddingKey = new ResourceKey(SystemResourceKeyId.SelectListDisplayItemPadding, typeof(Thickness))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey selectListDisplayItemPaddingKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the default amount of padding between a <see cref="SelectList"/>'s list item and its edges.
        /// </summary>
        public static ResourceKey SelectListListItemPaddingKey
        {
            get { return selectListListItemPaddingKey ?? (selectListListItemPaddingKey = new ResourceKey(SystemResourceKeyId.SelectListListItemPadding, typeof(Thickness))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey selectListListItemPaddingKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the default width of the border around a <see cref="TextBox"/> or similar text entry control.
        /// </summary>
        public static ResourceKey TextBoxBorderWidthKey
        {
            get { return textBoxBorderWidthKey ?? (textBoxBorderWidthKey = new ResourceKey(SystemResourceKeyId.TextBoxBorderWidth, typeof(double))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey textBoxBorderWidthKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the width of a vertical scroll bar.
        /// </summary>
        public static ResourceKey VerticalScrollBarWidthKey
        {
            get { return verticalScrollBarWidthKey ?? (verticalScrollBarWidthKey = new ResourceKey(SystemResourceKeyId.VerticalScrollBarWidth, typeof(double))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey verticalScrollBarWidthKey;
        #endregion

        #region Font Values
        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the default font family for UI objects.
        /// </summary>
        public static ResourceKey BaseFontFamilyKey
        {
            get { return baseFontFamilyKey ?? (baseFontFamilyKey = new ResourceKey(SystemResourceKeyId.BaseFontFamily, typeof(FontFamily))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey baseFontFamilyKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the default font size of a <see cref="Button"/>.
        /// </summary>
        public static ResourceKey ButtonFontSizeKey
        {
            get { return buttonFontSizeKey ?? (buttonFontSizeKey = new ResourceKey(SystemResourceKeyId.ButtonFontSize, typeof(double))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey buttonFontSizeKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the default font style of a <see cref="Button"/>.
        /// </summary>
        public static ResourceKey ButtonFontStyleKey
        {
            get { return buttonFontStyleKey ?? (buttonFontStyleKey = new ResourceKey(SystemResourceKeyId.ButtonFontStyle, typeof(FontStyle))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey buttonFontStyleKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the default font size of a <see cref="DatePicker"/> or <see cref="TimePicker"/>.
        /// </summary>
        public static ResourceKey DateTimePickerFontSizeKey
        {
            get { return dateTimePickerFontSizeKey ?? (dateTimePickerFontSizeKey = new ResourceKey(SystemResourceKeyId.DateTimePickerFontSize, typeof(double))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey dateTimePickerFontSizeKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the default font style of a <see cref="DatePicker"/> or <see cref="TimePicker"/>.
        /// </summary>
        public static ResourceKey DateTimePickerFontStyleKey
        {
            get { return dateTimePickerFontStyleKey ?? (dateTimePickerFontStyleKey = new ResourceKey(SystemResourceKeyId.DateTimePickerFontStyle, typeof(FontStyle))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey dateTimePickerFontStyleKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the default font size of the <see cref="P:DetailTextLabel"/> in a <see cref="ListBoxItem"/>.
        /// </summary>
        public static ResourceKey DetailLabelFontSizeKey
        {
            get { return detailLabelFontSizeKey ?? (detailLabelFontSizeKey = new ResourceKey(SystemResourceKeyId.DetailLabelFontSize, typeof(double))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey detailLabelFontSizeKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the default font style of the <see cref="P:DetailTextLabel"/> in a <see cref="ListBoxItem"/>.
        /// </summary>
        public static ResourceKey DetailLabelFontStyleKey
        {
            get { return detailLabelFontStyleKey ?? (detailLabelFontStyleKey = new ResourceKey(SystemResourceKeyId.DetailLabelFontStyle, typeof(FontStyle))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey detailLabelFontStyleKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the default font size of a section header in a <see cref="ListBox"/> that uses <see cref="ListBoxStyle.Grouped"/>.
        /// </summary>
        public static ResourceKey GroupedSectionHeaderFontSizeKey
        {
            get { return groupedSectionHeaderFontSizeKey ?? (groupedSectionHeaderFontSizeKey = new ResourceKey(SystemResourceKeyId.GroupedSectionHeaderFontSize, typeof(double))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey groupedSectionHeaderFontSizeKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the default font style of a section header in a <see cref="ListBox"/> that uses <see cref="ListBoxStyle.Grouped"/>.
        /// </summary>
        public static ResourceKey GroupedSectionHeaderFontStyleKey
        {
            get { return groupedSectionHeaderFontStyleKey ?? (groupedSectionHeaderFontStyleKey = new ResourceKey(SystemResourceKeyId.GroupedSectionHeaderFontStyle, typeof(FontStyle))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey groupedSectionHeaderFontStyleKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the default font size of a <see cref="Label"/>.
        /// </summary>
        public static ResourceKey LabelFontSizeKey
        {
            get { return labelFontSizeKey ?? (labelFontSizeKey = new ResourceKey(SystemResourceKeyId.LabelFontSize, typeof(double))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey labelFontSizeKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the default font style of a <see cref="Label"/>.
        /// </summary>
        public static ResourceKey LabelFontStyleKey
        {
            get { return labelFontStyleKey ?? (labelFontStyleKey = new ResourceKey(SystemResourceKeyId.LabelFontStyle, typeof(FontStyle))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey labelFontStyleKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the default font size of the title text on a <see cref="LoadIndicator"/>.
        /// </summary>
        public static ResourceKey LoadIndicatorFontSizeKey
        {
            get { return loadIndicatorFontSizeKey ?? (loadIndicatorFontSizeKey = new ResourceKey(SystemResourceKeyId.LoadIndicatorFontSize, typeof(double))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey loadIndicatorFontSizeKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the default font style of the title text on a <see cref="LoadIndicator"/>.
        /// </summary>
        public static ResourceKey LoadIndicatorFontStyleKey
        {
            get { return loadIndicatorFontStyleKey ?? (loadIndicatorFontStyleKey = new ResourceKey(SystemResourceKeyId.LoadIndicatorFontStyle, typeof(FontStyle))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey loadIndicatorFontStyleKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the default font size of a <see cref="SearchBox"/>.
        /// </summary>
        public static ResourceKey SearchBoxFontSizeKey
        {
            get { return searchBoxFontSizeKey ?? (searchBoxFontSizeKey = new ResourceKey(SystemResourceKeyId.SearchBoxFontSize, typeof(double))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey searchBoxFontSizeKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the default font style of a <see cref="SearchBox"/>.
        /// </summary>
        public static ResourceKey SearchBoxFontStyleKey
        {
            get { return searchBoxFontStyleKey ?? (searchBoxFontStyleKey = new ResourceKey(SystemResourceKeyId.SearchBoxFontStyle, typeof(FontStyle))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey searchBoxFontStyleKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the default font size of a section header in a <see cref="ListBox"/> that uses <see cref="ListBoxStyle.Default"/>.
        /// </summary>
        public static ResourceKey SectionHeaderFontSizeKey
        {
            get { return sectionHeaderFontSizeKey ?? (sectionHeaderFontSizeKey = new ResourceKey(SystemResourceKeyId.SectionHeaderFontSize, typeof(double))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey sectionHeaderFontSizeKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the default font style of a section header in a <see cref="ListBox"/> that uses <see cref="ListBoxStyle.Default"/>.
        /// </summary>
        public static ResourceKey SectionHeaderFontStyleKey
        {
            get { return sectionHeaderFontStyleKey ?? (sectionHeaderFontStyleKey = new ResourceKey(SystemResourceKeyId.SectionHeaderFontStyle, typeof(FontStyle))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey sectionHeaderFontStyleKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the default font size of a <see cref="SelectList"/>.
        /// </summary>
        public static ResourceKey SelectListFontSizeKey
        {
            get { return selectListFontSizeKey ?? (selectListFontSizeKey = new ResourceKey(SystemResourceKeyId.SelectListFontSize, typeof(double))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey selectListFontSizeKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the default font style of a <see cref="SelectList"/>.
        /// </summary>
        public static ResourceKey SelectListFontStyleKey
        {
            get { return selectListFontStyleKey ?? (selectListFontStyleKey = new ResourceKey(SystemResourceKeyId.SelectListFontStyle, typeof(FontStyle))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey selectListFontStyleKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the default font size of a <see cref="TabItem"/>.
        /// </summary>
        public static ResourceKey TabItemFontSizeKey
        {
            get { return tabItemFontSizeKey ?? (tabItemFontSizeKey = new ResourceKey(SystemResourceKeyId.TabItemFontSize, typeof(double))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey tabItemFontSizeKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the default font style of a <see cref="TabItem"/>.
        /// </summary>
        public static ResourceKey TabItemFontStyleKey
        {
            get { return tabItemFontStyleKey ?? (tabItemFontStyleKey = new ResourceKey(SystemResourceKeyId.TabItemFontStyle, typeof(FontStyle))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey tabItemFontStyleKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the default font size of a <see cref="TextBox"/> or similar text entry control.
        /// </summary>
        public static ResourceKey TextBoxFontSizeKey
        {
            get { return textBoxFontSizeKey ?? (textBoxFontSizeKey = new ResourceKey(SystemResourceKeyId.TextBoxFontSize, typeof(double))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey textBoxFontSizeKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the default font style of a <see cref="TextBox"/> or similar text entry control.
        /// </summary>
        public static ResourceKey TextBoxFontStyleKey
        {
            get { return textBoxFontStyleKey ?? (textBoxFontStyleKey = new ResourceKey(SystemResourceKeyId.TextBoxFontStyle, typeof(FontStyle))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey textBoxFontStyleKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the default font size of the <see cref="P:ValueTextLabel"/> in a <see cref="ListBoxItem"/>.
        /// </summary>
        public static ResourceKey ValueLabelFontSizeKey
        {
            get { return valueLabelFontSizeKey ?? (valueLabelFontSizeKey = new ResourceKey(SystemResourceKeyId.ValueLabelFontSize, typeof(double))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey valueLabelFontSizeKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the default font style of the <see cref="P:ValueTextLabel"/> in a <see cref="ListBoxItem"/>.
        /// </summary>
        public static ResourceKey ValueLabelFontStyleKey
        {
            get { return valueLabelFontStyleKey ?? (valueLabelFontStyleKey = new ResourceKey(SystemResourceKeyId.ValueLabelFontStyle, typeof(FontStyle))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey valueLabelFontStyleKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the default font size for the header of a view.
        /// </summary>
        public static ResourceKey ViewHeaderFontSizeKey
        {
            get { return viewHeaderFontSizeKey ?? (viewHeaderFontSizeKey = new ResourceKey(SystemResourceKeyId.ViewHeaderFontSize, typeof(double))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey viewHeaderFontSizeKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the default font style for the header of a view.
        /// </summary>
        public static ResourceKey ViewHeaderFontStyleKey
        {
            get { return viewHeaderFontStyleKey ?? (viewHeaderFontStyleKey = new ResourceKey(SystemResourceKeyId.ViewHeaderFontStyle, typeof(FontStyle))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey viewHeaderFontStyleKey;
        #endregion

        #region Colors
        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the alternative color of the current theme with high contrast.
        /// </summary>
        public static ResourceKey AltHighColorKey
        {
            get { return altHighColorKey ?? (altHighColorKey = new ResourceKey(SystemResourceKeyId.AltHighColor, typeof(Color))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey altHighColorKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the alternative color of the current theme with low contrast.
        /// </summary>
        public static ResourceKey AltLowColorKey
        {
            get { return altLowColorKey ?? (altLowColorKey = new ResourceKey(SystemResourceKeyId.AltLowColor, typeof(Color))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey altLowColorKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the alternative color of the current theme with medium contrast.
        /// </summary>
        public static ResourceKey AltMediumColorKey
        {
            get { return altMediumColorKey ?? (altMediumColorKey = new ResourceKey(SystemResourceKeyId.AltMediumColor, typeof(Color))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey altMediumColorKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the base color of the current theme with high contrast.
        /// </summary>
        public static ResourceKey BaseHighColorKey
        {
            get { return baseHighColorKey ?? (baseHighColorKey = new ResourceKey(SystemResourceKeyId.BaseHighColor, typeof(Color))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey baseHighColorKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the base color of the current theme with low contrast.
        /// </summary>
        public static ResourceKey BaseLowColorKey
        {
            get { return baseLowColorKey ?? (baseLowColorKey = new ResourceKey(SystemResourceKeyId.BaseLowColor, typeof(Color))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey baseLowColorKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the base color of the current theme with medium contrast.
        /// </summary>
        public static ResourceKey BaseMediumColorKey
        {
            get { return baseMediumColorKey ?? (baseMediumColorKey = new ResourceKey(SystemResourceKeyId.BaseMediumColor, typeof(Color))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey baseMediumColorKey;
        #endregion

        #region Brushes
        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving a <see cref="Brush"/> with the system accent color.
        /// </summary>
        public static ResourceKey AccentBrushKey
        {
            get { return accentBrushKey ?? (accentBrushKey = new ResourceKey(SystemResourceKeyId.AccentBrush, typeof(Brush))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey accentBrushKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving a <see cref="Brush"/> with the default background of an <see cref="ActionMenu"/>.
        /// </summary>
        public static ResourceKey ActionMenuBackgroundBrushKey
        {
            get { return actionMenuBackgroundBrushKey ?? (actionMenuBackgroundBrushKey = new ResourceKey(SystemResourceKeyId.ActionMenuBackgroundBrush, typeof(Brush))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey actionMenuBackgroundBrushKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving a <see cref="Brush"/> with the default foreground of an <see cref="ActionMenu"/>.
        /// </summary>
        public static ResourceKey ActionMenuForegroundBrushKey
        {
            get { return actionMenuForegroundBrushKey ?? (actionMenuForegroundBrushKey = new ResourceKey(SystemResourceKeyId.ActionMenuForegroundBrush, typeof(Brush))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey actionMenuForegroundBrushKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving a <see cref="Brush"/> with the default foreground of a <see cref="ActivityIndicator"/>.
        /// </summary>
        public static ResourceKey ActivityIndicatorForegroundBrushKey
        {
            get { return activityIndicatorForegroundBrushKey ?? (activityIndicatorForegroundBrushKey = new ResourceKey(SystemResourceKeyId.ActivityIndicatorForegroundBrush, typeof(Brush))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey activityIndicatorForegroundBrushKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving a <see cref="Brush"/> with the color that is associated with the AltHighColor resource.
        /// </summary>
        public static ResourceKey AltHighBrushKey
        {
            get { return altHighBrushKey ?? (altHighBrushKey = new ResourceKey(SystemResourceKeyId.AltHighBrush, typeof(Brush), AltHighColorKey)); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey altHighBrushKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving a <see cref="Brush"/> with the color that is associated with the AltLowColor resource.
        /// </summary>
        public static ResourceKey AltLowBrushKey
        {
            get { return altLowBrushKey ?? (altLowBrushKey = new ResourceKey(SystemResourceKeyId.AltLowBrush, typeof(Brush), AltLowColorKey)); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey altLowBrushKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving a <see cref="Brush"/> with the color that is associated with the AltMediumColor resource.
        /// </summary>
        public static ResourceKey AltMediumBrushKey
        {
            get { return altMediumBrushKey ?? (altMediumBrushKey = new ResourceKey(SystemResourceKeyId.AltMediumBrush, typeof(Brush), AltMediumColorKey)); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey altMediumBrushKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving a <see cref="Brush"/> with the color that is associated with the BaseHighColor resource.
        /// </summary>
        public static ResourceKey BaseHighBrushKey
        {
            get { return baseHighBrushKey ?? (baseHighBrushKey = new ResourceKey(SystemResourceKeyId.BaseHighBrush, typeof(Brush), BaseHighColorKey)); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey baseHighBrushKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving a <see cref="Brush"/> with the color that is associated with the BaseLowColor resource.
        /// </summary>
        public static ResourceKey BaseLowBrushKey
        {
            get { return baseLowBrushKey ?? (baseLowBrushKey = new ResourceKey(SystemResourceKeyId.BaseLowBrush, typeof(Brush), BaseLowColorKey)); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey baseLowBrushKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving a <see cref="Brush"/> with the color that is associated with the BaseMediumColor resource.
        /// </summary>
        public static ResourceKey BaseMediumBrushKey
        {
            get { return baseMediumBrushKey ?? (baseMediumBrushKey = new ResourceKey(SystemResourceKeyId.BaseMediumBrush, typeof(Brush), BaseMediumColorKey)); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey baseMediumBrushKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving a <see cref="Brush"/> with the default background of a <see cref="Button"/>.
        /// </summary>
        public static ResourceKey ButtonBackgroundBrushKey
        {
            get { return buttonBackgroundBrushKey ?? (buttonBackgroundBrushKey = new ResourceKey(SystemResourceKeyId.ButtonBackgroundBrush, typeof(Brush))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey buttonBackgroundBrushKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving a <see cref="Brush"/> with the default border color of a <see cref="Button"/>.
        /// </summary>
        public static ResourceKey ButtonBorderBrushKey
        {
            get { return buttonBorderBrushKey ?? (buttonBorderBrushKey = new ResourceKey(SystemResourceKeyId.ButtonBorderBrush, typeof(Brush))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey buttonBorderBrushKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving a <see cref="Brush"/> with the default foreground of a <see cref="Button"/>.
        /// </summary>
        public static ResourceKey ButtonForegroundBrushKey
        {
            get { return buttonForegroundBrushKey ?? (buttonForegroundBrushKey = new ResourceKey(SystemResourceKeyId.ButtonForegroundBrush, typeof(Brush))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey buttonForegroundBrushKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving a <see cref="Brush"/> with the default background of a <see cref="DatePicker"/> or <see cref="TimePicker"/>.
        /// </summary>
        public static ResourceKey DateTimePickerBackgroundBrushKey
        {
            get { return dateTimePickerBackgroundBrushKey ?? (dateTimePickerBackgroundBrushKey = new ResourceKey(SystemResourceKeyId.DateTimePickerBackgroundBrush, typeof(Brush))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey dateTimePickerBackgroundBrushKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving a <see cref="Brush"/> with the default border color of a <see cref="DatePicker"/> or <see cref="TimePicker"/>.
        /// </summary>
        public static ResourceKey DateTimePickerBorderBrushKey
        {
            get { return dateTimePickerBorderBrushKey ?? (dateTimePickerBorderBrushKey = new ResourceKey(SystemResourceKeyId.DateTimePickerBorderBrush, typeof(Brush))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey dateTimePickerBorderBrushKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving a <see cref="Brush"/> with the default foreground of a <see cref="DatePicker"/> or <see cref="TimePicker"/>.
        /// </summary>
        public static ResourceKey DateTimePickerForegroundBrushKey
        {
            get { return dateTimePickerForegroundBrushKey ?? (dateTimePickerForegroundBrushKey = new ResourceKey(SystemResourceKeyId.DateTimePickerForegroundBrush, typeof(Brush))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey dateTimePickerForegroundBrushKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving a <see cref="Brush"/> with the default foreground of the <see cref="P:DetailTextLabel"/> in a <see cref="ListBoxItem"/>.
        /// </summary>
        public static ResourceKey DetailLabelForegroundBrushKey
        {
            get { return detailLabelForegroundBrushKey ?? (detailLabelForegroundBrushKey = new ResourceKey(SystemResourceKeyId.DetailLabelForegroundBrush, typeof(Brush))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey detailLabelForegroundBrushKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving a <see cref="Brush"/> with the default background of a flyout object.
        /// </summary>
        public static ResourceKey FlyoutBackgroundBrushKey
        {
            get { return flyoutBackgroundBrushKey ?? (flyoutBackgroundBrushKey = new ResourceKey(SystemResourceKeyId.FlyoutBackgroundBrush, typeof(Brush))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey flyoutBackgroundBrushKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving a <see cref="Brush"/> with the default background of an unselected
        /// <see cref="ListBoxItem"/> in a <see cref="ListBox"/> that uses <see cref="ListBoxStyle.Grouped"/>.
        /// </summary>
        public static ResourceKey GroupedListBoxItemBackgroundBrushKey
        {
            get { return groupedListBoxItemBackgroundBrushKey ?? (groupedListBoxItemBackgroundBrushKey = new ResourceKey(SystemResourceKeyId.GroupedListBoxItemBackgroundBrush, typeof(Brush))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey groupedListBoxItemBackgroundBrushKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving a <see cref="Brush"/> with the default background of a
        /// section header in a <see cref="ListBox"/> that uses <see cref="ListBoxStyle.Grouped"/>.
        /// </summary>
        public static ResourceKey GroupedSectionHeaderBackgroundBrushKey
        {
            get { return groupedSectionHeaderBackgroundBrushKey ?? (groupedSectionHeaderBackgroundBrushKey = new ResourceKey(SystemResourceKeyId.GroupedSectionHeaderBackgroundBrush, typeof(Brush))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey groupedSectionHeaderBackgroundBrushKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving a <see cref="Brush"/> with the default foreground of a
        /// section header in a <see cref="ListBox"/> that uses <see cref="ListBoxStyle.Grouped"/>.
        /// </summary>
        public static ResourceKey GroupedSectionHeaderForegroundBrushKey
        {
            get { return groupedSectionHeaderForegroundBrushKey ?? (groupedSectionHeaderForegroundBrushKey = new ResourceKey(SystemResourceKeyId.GroupedSectionHeaderForegroundBrush, typeof(Brush))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey groupedSectionHeaderForegroundBrushKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving a <see cref="Brush"/> with the default foreground of a <see cref="Label"/>.
        /// </summary>
        public static ResourceKey LabelForegroundBrushKey
        {
            get { return labelForegroundBrushKey ?? (labelForegroundBrushKey = new ResourceKey(SystemResourceKeyId.LabelForegroundBrush, typeof(Brush))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey labelForegroundBrushKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving a <see cref="Brush"/> with the default background of a <see cref="ListBox"/>.
        /// </summary>
        public static ResourceKey ListBoxBackgroundBrushKey
        {
            get { return listBoxBackgroundBrushKey ?? (listBoxBackgroundBrushKey = new ResourceKey(SystemResourceKeyId.ListBoxBackgroundBrush, typeof(Brush))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey listBoxBackgroundBrushKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving a <see cref="Brush"/> with the default background of an unselected
        /// <see cref="ListBoxItem"/> in a <see cref="ListBox"/> that uses <see cref="ListBoxStyle.Default"/>.
        /// </summary>
        public static ResourceKey ListBoxItemBackgroundBrushKey
        {
            get { return listBoxItemBackgroundBrushKey ?? (listBoxItemBackgroundBrushKey = new ResourceKey(SystemResourceKeyId.ListBoxItemBackgroundBrush, typeof(Brush))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey listBoxItemBackgroundBrushKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving a <see cref="Brush"/> with the default background of a selected <see cref="ListBoxItem"/>.
        /// </summary>
        public static ResourceKey ListBoxItemSelectedBackgroundBrushKey
        {
            get { return listBoxItemSelectedBackgroundBrushKey ?? (listBoxItemSelectedBackgroundBrushKey = new ResourceKey(SystemResourceKeyId.ListBoxItemSelectedBackgroundBrush, typeof(Brush))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey listBoxItemSelectedBackgroundBrushKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving a <see cref="Brush"/> with the default separator color of a <see cref="ListBox"/>.
        /// </summary>
        public static ResourceKey ListBoxSeparatorBrushKey
        {
            get { return listBoxSeparatorBrushKey ?? (listBoxSeparatorBrushKey = new ResourceKey(SystemResourceKeyId.ListBoxSeparatorBrush, typeof(Brush))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey listBoxSeparatorBrushKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving a <see cref="Brush"/> with the default background of a <see cref="LoadIndicator"/>.
        /// </summary>
        public static ResourceKey LoadIndicatorBackgroundBrushKey
        {
            get { return loadIndicatorBackgroundBrushKey ?? (loadIndicatorBackgroundBrushKey = new ResourceKey(SystemResourceKeyId.LoadIndicatorBackgroundBrush, typeof(Brush))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey loadIndicatorBackgroundBrushKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving a <see cref="Brush"/> with the default foreground of a <see cref="LoadIndicator"/>.
        /// </summary>
        public static ResourceKey LoadIndicatorForegroundBrushKey
        {
            get { return loadIndicatorForegroundBrushKey ?? (loadIndicatorForegroundBrushKey = new ResourceKey(SystemResourceKeyId.LoadIndicatorForegroundBrush, typeof(Brush))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey loadIndicatorForegroundBrushKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving a <see cref="Brush"/> with the default foreground of a <see cref="MenuFlyout"/>.
        /// </summary>
        public static ResourceKey MenuFlyoutForegroundBrushKey
        {
            get { return menuFlyoutForegroundBrushKey ?? (menuFlyoutForegroundBrushKey = new ResourceKey(SystemResourceKeyId.MenuFlyoutForegroundBrush, typeof(Brush))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey menuFlyoutForegroundBrushKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving a <see cref="Brush"/> with the default background of a <see cref="ProgressBar"/>.
        /// </summary>
        public static ResourceKey ProgressBarBackgroundBrushKey
        {
            get { return progressBarBackgroundBrushKey ?? (progressBarBackgroundBrushKey = new ResourceKey(SystemResourceKeyId.ProgressBarBackgroundBrush, typeof(Brush))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey progressBarBackgroundBrushKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving a <see cref="Brush"/> with the default foreground of a <see cref="ProgressBar"/>.
        /// </summary>
        public static ResourceKey ProgressBarForegroundBrushKey
        {
            get { return progressBarForegroundBrushKey ?? (progressBarForegroundBrushKey = new ResourceKey(SystemResourceKeyId.ProgressBarForegroundBrush, typeof(Brush))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey progressBarForegroundBrushKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving a <see cref="Brush"/> with the default background of a <see cref="SearchBox"/>.
        /// </summary>
        public static ResourceKey SearchBoxBackgroundBrushKey
        {
            get { return searchBoxBackgroundBrushKey ?? (searchBoxBackgroundBrushKey = new ResourceKey(SystemResourceKeyId.SearchBoxBackgroundBrush, typeof(Brush))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey searchBoxBackgroundBrushKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving a <see cref="Brush"/> with the default border color of a <see cref="SearchBox"/>.
        /// </summary>
        public static ResourceKey SearchBoxBorderBrushKey
        {
            get { return searchBoxBorderBrushKey ?? (searchBoxBorderBrushKey = new ResourceKey(SystemResourceKeyId.SearchBoxBorderBrush, typeof(Brush))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey searchBoxBorderBrushKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving a <see cref="Brush"/> with the default foreground of a <see cref="SearchBox"/>.
        /// </summary>
        public static ResourceKey SearchBoxForegroundBrushKey
        {
            get { return searchBoxForegroundBrushKey ?? (searchBoxForegroundBrushKey = new ResourceKey(SystemResourceKeyId.SearchBoxForegroundBrush, typeof(Brush))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey searchBoxForegroundBrushKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving a <see cref="Brush"/> with the default background of a
        /// section header in a <see cref="ListBox"/> that uses <see cref="ListBoxStyle.Default"/>.
        /// </summary>
        public static ResourceKey SectionHeaderBackgroundBrushKey
        {
            get { return sectionHeaderBackgroundBrushKey ?? (sectionHeaderBackgroundBrushKey = new ResourceKey(SystemResourceKeyId.SectionHeaderBackgroundBrush, typeof(Brush))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey sectionHeaderBackgroundBrushKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving a <see cref="Brush"/> with the default foreground of a
        /// section header in a <see cref="ListBox"/> that uses <see cref="ListBoxStyle.Default"/>.
        /// </summary>
        public static ResourceKey SectionHeaderForegroundBrushKey
        {
            get { return sectionHeaderForegroundBrushKey ?? (sectionHeaderForegroundBrushKey = new ResourceKey(SystemResourceKeyId.SectionHeaderForegroundBrush, typeof(Brush))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey sectionHeaderForegroundBrushKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving a <see cref="Brush"/> with the default background of a <see cref="SelectList"/>.
        /// </summary>
        public static ResourceKey SelectListBackgroundBrushKey
        {
            get { return selectListBackgroundBrushKey ?? (selectListBackgroundBrushKey = new ResourceKey(SystemResourceKeyId.SelectListBackgroundBrush, typeof(Brush))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey selectListBackgroundBrushKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving a <see cref="Brush"/> with the default border color of a <see cref="SelectList"/>.
        /// </summary>
        public static ResourceKey SelectListBorderBrushKey
        {
            get { return selectListBorderBrushKey ?? (selectListBorderBrushKey = new ResourceKey(SystemResourceKeyId.SelectListBorderBrush, typeof(Brush))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey selectListBorderBrushKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving a <see cref="Brush"/> with the default foreground of a <see cref="SelectList"/>.
        /// </summary>
        public static ResourceKey SelectListForegroundBrushKey
        {
            get { return selectListForegroundBrushKey ?? (selectListForegroundBrushKey = new ResourceKey(SystemResourceKeyId.SelectListForegroundBrush, typeof(Brush))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey selectListForegroundBrushKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving a <see cref="Brush"/> with the default background of the selection list in a <see cref="SelectList"/>.
        /// </summary>
        public static ResourceKey SelectListListBackgroundBrushKey
        {
            get { return selectListListBackgroundBrushKey ?? (selectListListBackgroundBrushKey = new ResourceKey(SystemResourceKeyId.SelectListListBackgroundBrush, typeof(Brush))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey selectListListBackgroundBrushKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving a <see cref="Brush"/> with the default separator color of the selection list in a <see cref="SelectList"/>.
        /// </summary>
        public static ResourceKey SelectListListSeparatorBrushKey
        {
            get { return selectListListSeparatorBrushKey ?? (selectListListSeparatorBrushKey = new ResourceKey(SystemResourceKeyId.SelectListListSeparatorBrush, typeof(Brush))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey selectListListSeparatorBrushKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving a <see cref="Brush"/> with the default background of a <see cref="Slider"/>.
        /// </summary>
        public static ResourceKey SliderBackgroundBrushKey
        {
            get { return sliderBackgroundBrushKey ?? (sliderBackgroundBrushKey = new ResourceKey(SystemResourceKeyId.SliderBackgroundBrush, typeof(Brush))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey sliderBackgroundBrushKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving a <see cref="Brush"/> with the default foreground of a <see cref="Slider"/>.
        /// </summary>
        public static ResourceKey SliderForegroundBrushKey
        {
            get { return sliderForegroundBrushKey ?? (sliderForegroundBrushKey = new ResourceKey(SystemResourceKeyId.SliderForegroundBrush, typeof(Brush))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey sliderForegroundBrushKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving a <see cref="Brush"/> with the default thumb color of a <see cref="Slider"/>.
        /// </summary>
        public static ResourceKey SliderThumbBrushKey
        {
            get { return sliderThumbBrushKey ?? (sliderThumbBrushKey = new ResourceKey(SystemResourceKeyId.SliderThumbBrush, typeof(Brush))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey sliderThumbBrushKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving a <see cref="Brush"/> with the default foreground of a <see cref="TabItem"/>.
        /// </summary>
        public static ResourceKey TabItemForegroundBrushKey
        {
            get { return tabItemForegroundBrushKey ?? (tabItemForegroundBrushKey = new ResourceKey(SystemResourceKeyId.TabItemForegroundBrush, typeof(Brush))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey tabItemForegroundBrushKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving a <see cref="Brush"/> with the default background of a <see cref="TabView"/>.
        /// </summary>
        public static ResourceKey TabViewBackgroundBrushKey
        {
            get { return tabViewBackgroundBrushKey ?? (tabViewBackgroundBrushKey = new ResourceKey(SystemResourceKeyId.TabViewBackgroundBrush, typeof(Brush))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey tabViewBackgroundBrushKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving a <see cref="Brush"/> with the default foreground of a <see cref="TabView"/>.
        /// </summary>
        public static ResourceKey TabViewForegroundBrushKey
        {
            get { return tabViewForegroundBrushKey ?? (tabViewForegroundBrushKey = new ResourceKey(SystemResourceKeyId.TabViewForegroundBrush, typeof(Brush))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey tabViewForegroundBrushKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving a <see cref="Brush"/> with the default background of a <see cref="TextBox"/> or similar text entry control.
        /// </summary>
        public static ResourceKey TextBoxBackgroundBrushKey
        {
            get { return textBoxBackgroundBrushKey ?? (textBoxBackgroundBrushKey = new ResourceKey(SystemResourceKeyId.TextBoxBackgroundBrush, typeof(Brush))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey textBoxBackgroundBrushKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving a <see cref="Brush"/> with the default border color of a <see cref="TextBox"/> or similar text entry control.
        /// </summary>
        public static ResourceKey TextBoxBorderBrushKey
        {
            get { return textBoxBorderBrushKey ?? (textBoxBorderBrushKey = new ResourceKey(SystemResourceKeyId.TextBoxBorderBrush, typeof(Brush))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey textBoxBorderBrushKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving a <see cref="Brush"/> with the default foreground of a <see cref="TextBox"/> or similar text entry control.
        /// </summary>
        public static ResourceKey TextBoxForegroundBrushKey
        {
            get { return textBoxForegroundBrushKey ?? (textBoxForegroundBrushKey = new ResourceKey(SystemResourceKeyId.TextBoxForegroundBrush, typeof(Brush))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey textBoxForegroundBrushKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving a <see cref="Brush"/> with the default background of a <see cref="ToggleSwitch"/>.
        /// </summary>
        public static ResourceKey ToggleSwitchBackgroundBrushKey
        {
            get { return toggleSwitchBackgroundBrushKey ?? (toggleSwitchBackgroundBrushKey = new ResourceKey(SystemResourceKeyId.ToggleSwitchBackgroundBrush, typeof(Brush))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey toggleSwitchBackgroundBrushKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving a <see cref="Brush"/> with the default border color of a <see cref="ToggleSwitch"/>.
        /// </summary>
        public static ResourceKey ToggleSwitchBorderBrushKey
        {
            get { return toggleSwitchBorderBrushKey ?? (toggleSwitchBorderBrushKey = new ResourceKey(SystemResourceKeyId.ToggleSwitchBorderBrush, typeof(Brush))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey toggleSwitchBorderBrushKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving a <see cref="Brush"/> with the default foreground of a <see cref="ToggleSwitch"/>.
        /// </summary>
        public static ResourceKey ToggleSwitchForegroundBrushKey
        {
            get { return toggleSwitchForegroundBrushKey ?? (toggleSwitchForegroundBrushKey = new ResourceKey(SystemResourceKeyId.ToggleSwitchForegroundBrush, typeof(Brush))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey toggleSwitchForegroundBrushKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving a <see cref="Brush"/> with the default thumb color of a <see cref="ToggleSwitch"/> that is in a <c>false</c> state.
        /// </summary>
        public static ResourceKey ToggleSwitchThumbOffBrushKey
        {
            get { return toggleSwitchThumbOffBrushKey ?? (toggleSwitchThumbOffBrushKey = new ResourceKey(SystemResourceKeyId.ToggleSwitchThumbOffBrush, typeof(Brush))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey toggleSwitchThumbOffBrushKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving a <see cref="Brush"/> with the default thumb color of a <see cref="ToggleSwitch"/> that is in a <c>true</c> state.
        /// </summary>
        public static ResourceKey ToggleSwitchThumbOnBrushKey
        {
            get { return toggleSwitchThumbOnBrushKey ?? (toggleSwitchThumbOnBrushKey = new ResourceKey(SystemResourceKeyId.ToggleSwitchThumbOnBrush, typeof(Brush))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey toggleSwitchThumbOnBrushKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving a <see cref="Brush"/> with the default foreground of the <see cref="P:ValueTextLabel"/> in a <see cref="ListBoxItem"/>.
        /// </summary>
        public static ResourceKey ValueLabelForegroundBrushKey
        {
            get { return valueLabelForegroundBrushKey ?? (valueLabelForegroundBrushKey = new ResourceKey(SystemResourceKeyId.ValueLabelForegroundBrush, typeof(Brush))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey valueLabelForegroundBrushKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving a <see cref="Brush"/> with the default background of a view.
        /// </summary>
        public static ResourceKey ViewBackgroundBrushKey
        {
            get { return viewBackgroundBrushKey ?? (viewBackgroundBrushKey = new ResourceKey(SystemResourceKeyId.ViewBackgroundBrush, typeof(Brush))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey viewBackgroundBrushKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving a <see cref="Brush"/> with the default background for the header of a view.
        /// </summary>
        public static ResourceKey ViewHeaderBackgroundBrushKey
        {
            get { return viewHeaderBackgroundBrushKey ?? (viewHeaderBackgroundBrushKey = new ResourceKey(SystemResourceKeyId.ViewHeaderBackgroundBrush, typeof(Brush))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey viewHeaderBackgroundBrushKey;

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving a <see cref="Brush"/> with the default foreground for the header of a view.
        /// </summary>
        public static ResourceKey ViewHeaderForegroundBrushKey
        {
            get { return viewHeaderForegroundBrushKey ?? (viewHeaderForegroundBrushKey = new ResourceKey(SystemResourceKeyId.ViewHeaderForegroundBrush, typeof(Brush))); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ResourceKey viewHeaderForegroundBrushKey;
        #endregion
    }
}
