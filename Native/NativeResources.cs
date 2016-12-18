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
using System.Diagnostics.CodeAnalysis;
using Prism.UI;
using Prism.UI.Controls;
using Prism.UI.Media;

namespace Prism.Native
{
    /// <summary>
    /// Defines a utility for retrieving system resources that are native to a particular platform.
    /// </summary>
    [CoreBehavior(CoreBehaviors.ExpectsSingleton)]
    public interface INativeResources
    {
        /// <summary>
        /// Gets the names of all available fonts.
        /// </summary>
        /// <returns>An <see cref="Array"/> containing the names of all available fonts.</returns>
        string[] GetAvailableFontNames();

        /// <summary>
        /// Gets the system resource associated with the specified key.
        /// </summary>
        /// <param name="owner">The object that owns the resource, or <c>null</c> if the resource is not owned by a specified object.</param>
        /// <param name="key">The key associated with the resource to get.</param>
        /// <param name="value">When this method returns, the value associated with the specified key, if the key is found; otherwise, <c>null</c>. This parameter is passed uninitialized.</param>
        /// <returns><c>true</c> if the system resources contain a resource with the specified key; otherwise, <c>false</c>.</returns>
        [SuppressMessage("Microsoft.Design", "CA1007:UseGenericsWhereAppropriate", Justification = "All resources are passed as objects.  Casting is not desired.")]
        bool TryGetResource(object owner, [CoreBehavior(CoreBehaviors.ChecksNullity)]object key, out object value);
    }

    /// <summary>
    /// Describes identifiers for system-defined resource values.
    /// </summary>
    public enum SystemResourceKeyId
    {
        /// <summary>
        /// Do not use.
        /// </summary>
        StartMarker = 0,

        #region Measurements
        /// <summary>
        /// An identifier for retrieving the default maximum number of items that can be
        /// displayed in an <see cref="ActionMenu"/> before they are placed into an overflow menu.
        /// </summary>
        ActionMenuMaxDisplayItems,
        /// <summary>
        /// An identifier for retrieving the default width of the border around a <see cref="Button"/>.
        /// </summary>
        ButtonBorderWidth,
        /// <summary>
        /// An identifier for retrieving the default amount of padding between a <see cref="Button"/>'s content and its edges.
        /// </summary>
        ButtonPadding,
        /// <summary>
        /// An identifier for retrieving the default width of the border around a <see cref="DatePicker"/> or <see cref="TimePicker"/>.
        /// </summary>
        DateTimePickerBorderWidth,
        /// <summary>
        /// An identifier for retrieving the height of a horizontal scroll bar.
        /// </summary>
        HorizontalScrollBarHeight,
        /// <summary>
        /// An identifier for retrieving the default height of a <see cref="ListBoxItem"/> with a <see cref="P:DetailTextLabel"/>.
        /// </summary>
        ListBoxItemDetailHeight,
        /// <summary>
        /// An identifier for retrieving the size of the <see cref="ListBoxItemAccessory.Indicator"/> accessory in a <see cref="ListBoxItem"/>.
        /// </summary>
        ListBoxItemIndicatorSize,
        /// <summary>
        /// An identifier for retrieving the size of the <see cref="ListBoxItemAccessory.InfoButton"/> accessory in a <see cref="ListBoxItem"/>.
        /// </summary>
        ListBoxItemInfoButtonSize,
        /// <summary>
        /// An identifier for retrieving the size of the <see cref="ListBoxItemAccessory.InfoIndicator"/> accessory in a <see cref="ListBoxItem"/>.
        /// </summary>
        ListBoxItemInfoIndicatorSize,
        /// <summary>
        /// An identifier for retrieving the default height of a standard <see cref="ListBoxItem"/>.
        /// </summary>
        ListBoxItemStandardHeight,
        /// <summary>
        /// An identifier for retrieving the size of a <see cref="Popup"/> when presented with <see cref="PopupPresentationStyle.Default"/>.
        /// </summary>
        PopupSize,
        /// <summary>
        /// An identifier for retrieving the default width of the border around a <see cref="SearchBox"/>.
        /// </summary>
        SearchBoxBorderWidth,
        /// <summary>
        /// An identifier for retrieving the default width of the border around a <see cref="SelectList"/>.
        /// </summary>
        SelectListBorderWidth,
        /// <summary>
        /// An identifier for retrieving the default amount of padding between a <see cref="SelectList"/>'s display item and its edges.
        /// </summary>
        SelectListDisplayItemPadding,
        /// <summary>
        /// An identifier for retrieving the default amount of padding between a <see cref="SelectList"/>'s list item and its edges.
        /// </summary>
        SelectListListItemPadding,
        /// <summary>
        /// An identifier for retrieving a value indicating whether the separator of a <see cref="ListBoxItem"/>
        /// should be automatically indented in order to be flush with the text labels of the item.
        /// </summary>
        ShouldAutomaticallyIndentSeparators,
        /// <summary>
        /// An identifier for retrieving the default width of the border around a <see cref="TextBox"/> or similar text entry control.
        /// </summary>
        TextBoxBorderWidth,
        /// <summary>
        /// An identifier for retrieving the width of a vertical scroll bar.
        /// </summary>
        VerticalScrollBarWidth,
        #endregion

        #region Font Values
        /// <summary>
        /// An identifier for retrieving the default font family for UI objects.
        /// </summary>
        BaseFontFamily,
        /// <summary>
        /// An identifier for retrieving the default font size of a <see cref="Button"/>.
        /// </summary>
        ButtonFontSize,
        /// <summary>
        /// An identifier for retrieving the default font style of a <see cref="Button"/>.
        /// </summary>
        ButtonFontStyle,
        /// <summary>
        /// An identifier for retrieving the default font size of a <see cref="DatePicker"/> or <see cref="TimePicker"/>.
        /// </summary>
        DateTimePickerFontSize,
        /// <summary>
        /// An identifier for retrieving the default font style of a <see cref="DatePicker"/> or <see cref="TimePicker"/>.
        /// </summary>
        DateTimePickerFontStyle,
        /// <summary>
        /// An identifier for retrieving the default font size of the <see cref="P:DetailTextLabel"/> in a <see cref="ListBoxItem"/>.
        /// </summary>
        DetailLabelFontSize,
        /// <summary>
        /// An identifier for retrieving the default font style of the <see cref="P:DetailTextLabel"/> in a <see cref="ListBoxItem"/>.
        /// </summary>
        DetailLabelFontStyle,
        /// <summary>
        /// An identifier for retrieving the default font size of a section header in a <see cref="ListBox"/> that uses <see cref="ListBoxStyle.Grouped"/>.
        /// </summary>
        GroupedSectionHeaderFontSize,
        /// <summary>
        /// An identifier for retrieving the default font style of a section header in a <see cref="ListBox"/> that uses <see cref="ListBoxStyle.Grouped"/>.
        /// </summary>
        GroupedSectionHeaderFontStyle,
        /// <summary>
        /// An identifier for retrieving the default font size of a <see cref="Label"/>.
        /// </summary>
        LabelFontSize,
        /// <summary>
        /// An identifier for retrieving the default font style of a <see cref="Label"/>.
        /// </summary>
        LabelFontStyle,
        /// <summary>
        /// An identifier for retrieving the default font size of the title text on a <see cref="LoadIndicator"/>.
        /// </summary>
        LoadIndicatorFontSize,
        /// <summary>
        /// An identifier for retrieving the default font style of the title text on a <see cref="LoadIndicator"/>.
        /// </summary>
        LoadIndicatorFontStyle,
        /// <summary>
        /// An identifier for retrieving the default font size of a <see cref="SearchBox"/>.
        /// </summary>
        SearchBoxFontSize,
        /// <summary>
        /// An identifier for retrieving the default font style of a <see cref="SearchBox"/>.
        /// </summary>
        SearchBoxFontStyle,
        /// <summary>
        /// An identifier for retrieving the default font size of a section header in a <see cref="ListBox"/> that uses <see cref="ListBoxStyle.Default"/>.
        /// </summary>
        SectionHeaderFontSize,
        /// <summary>
        /// An identifier for retrieving the default font style of a section header in a <see cref="ListBox"/> that uses <see cref="ListBoxStyle.Default"/>.
        /// </summary>
        SectionHeaderFontStyle,
        /// <summary>
        /// An identifier for retrieving the default font size of a <see cref="SelectList"/>.
        /// </summary>
        SelectListFontSize,
        /// <summary>
        /// An identifier for retrieving the default font style of a <see cref="SelectList"/>.
        /// </summary>
        SelectListFontStyle,
        /// <summary>
        /// An identifier for retrieving the default font size of a <see cref="TabItem"/>.
        /// </summary>
        TabItemFontSize,
        /// <summary>
        /// An identifier for retrieving the default font style of a <see cref="TabItem"/>.
        /// </summary>
        TabItemFontStyle,
        /// <summary>
        /// An identifier for retrieving the default font size of a <see cref="TextBox"/> or similar text entry control.
        /// </summary>
        TextBoxFontSize,
        /// <summary>
        /// An identifier for retrieving the default font style of a <see cref="TextBox"/> or similar text entry control.
        /// </summary>
        TextBoxFontStyle,
        /// <summary>
        /// An identifier for retrieving the default font size of the <see cref="P:ValueTextLabel"/> in a <see cref="ListBoxItem"/>.
        /// </summary>
        ValueLabelFontSize,
        /// <summary>
        /// An identifier for retrieving the default font style of the <see cref="P:ValueTextLabel"/> in a <see cref="ListBoxItem"/>.
        /// </summary>
        ValueLabelFontStyle,
        /// <summary>
        /// An identifier for retrieving the default font size for the header of a view.
        /// </summary>
        ViewHeaderFontSize,
        /// <summary>
        /// An identifier for retrieving the default font style for the header of a view.
        /// </summary>
        ViewHeaderFontStyle,
        #endregion

        #region Brushes
        /// <summary>
        /// An identifier for retrieving a <see cref="Brush"/> with the system accent color.
        /// </summary>
        AccentBrush,
        /// <summary>
        /// An identifier for retrieving a <see cref="Brush"/> with the default background of an <see cref="ActionMenu"/>.
        /// </summary>
        ActionMenuBackgroundBrush,
        /// <summary>
        /// An identifier for retrieving a <see cref="Brush"/> with the default foreground of an <see cref="ActionMenu"/>.
        /// </summary>
        ActionMenuForegroundBrush,
        /// <summary>
        /// An identifier for retrieving a <see cref="Brush"/> with the default foreground of a <see cref="ActivityIndicator"/>.
        /// </summary>
        ActivityIndicatorForegroundBrush,
        /// <summary>
        /// An identifier for retrieving a <see cref="Brush"/> with the default background of a <see cref="Button"/>.
        /// </summary>
        ButtonBackgroundBrush,
        /// <summary>
        /// An identifier for retrieving a <see cref="Brush"/> with the default border color of a <see cref="Button"/>.
        /// </summary>
        ButtonBorderBrush,
        /// <summary>
        /// An identifier for retrieving a <see cref="Brush"/> with the default foreground of a <see cref="Button"/>.
        /// </summary>
        ButtonForegroundBrush,
        /// <summary>
        /// An identifier for retrieving a <see cref="Brush"/> with the default background of a <see cref="DatePicker"/> or <see cref="TimePicker"/>.
        /// </summary>
        DateTimePickerBackgroundBrush,
        /// <summary>
        /// An identifier for retrieving a <see cref="Brush"/> with the default border color of a <see cref="DatePicker"/> or <see cref="TimePicker"/>.
        /// </summary>
        DateTimePickerBorderBrush,
        /// <summary>
        /// An identifier for retrieving a <see cref="Brush"/> with the default foreground of a <see cref="DatePicker"/> or <see cref="TimePicker"/>.
        /// </summary>
        DateTimePickerForegroundBrush,
        /// <summary>
        /// An identifier for retrieving a <see cref="Brush"/> with the default foreground of the <see cref="P:DetailTextLabel"/> in a <see cref="ListBoxItem"/>.
        /// </summary>
        DetailLabelForegroundBrush,
        /// <summary>
        /// An identifier for retrieving a <see cref="Brush"/> with the default background of an unselected
        /// <see cref="ListBoxItem"/> in a <see cref="ListBox"/> that uses <see cref="ListBoxStyle.Grouped"/>.
        /// </summary>
        GroupedListBoxItemBackgroundBrush,
        /// <summary>
        /// An identifier for retrieving a <see cref="Brush"/> with the default background of a
        /// section header in a <see cref="ListBox"/> that uses <see cref="ListBoxStyle.Grouped"/>.
        /// </summary>
        GroupedSectionHeaderBackgroundBrush,
        /// <summary>
        /// An identifier for retrieving a <see cref="Brush"/> with the default foreground of a
        /// section header in a <see cref="ListBox"/> that uses <see cref="ListBoxStyle.Grouped"/>.
        /// </summary>
        GroupedSectionHeaderForegroundBrush,
        /// <summary>
        /// An identifier for retrieving a <see cref="Brush"/> with the default foreground of a <see cref="Label"/>.
        /// </summary>
        LabelForegroundBrush,
        /// <summary>
        /// An identifier for retrieving a <see cref="Brush"/> with the default background of a <see cref="ListBox"/>.
        /// </summary>
        ListBoxBackgroundBrush,
        /// <summary>
        /// An identifier for retrieving a <see cref="Brush"/> with the default background of an unselected
        /// <see cref="ListBoxItem"/> in a <see cref="ListBox"/> that uses <see cref="ListBoxStyle.Default"/>.
        /// </summary>
        ListBoxItemBackgroundBrush,
        /// <summary>
        /// An identifier for retrieving a <see cref="Brush"/> with the default background of a selected <see cref="ListBoxItem"/>.
        /// </summary>
        ListBoxItemSelectedBackgroundBrush,
        /// <summary>
        /// An identifier for retrieving a <see cref="Brush"/> with the default separator color of a <see cref="ListBox"/>.
        /// </summary>
        ListBoxSeparatorBrush,
        /// <summary>
        /// An identifier for retrieving a <see cref="Brush"/> with the default background of a <see cref="LoadIndicator"/>.
        /// </summary>
        LoadIndicatorBackgroundBrush,
        /// <summary>
        /// An identifier for retrieving a <see cref="Brush"/> with the default foreground of a <see cref="LoadIndicator"/>.
        /// </summary>
        LoadIndicatorForegroundBrush,
        /// <summary>
        /// An identifier for retrieving a <see cref="Brush"/> with the default background of a <see cref="SearchBox"/>.
        /// </summary>
        SearchBoxBackgroundBrush,
        /// <summary>
        /// An identifier for retrieving a <see cref="Brush"/> with the default border color of a <see cref="SearchBox"/>.
        /// </summary>
        SearchBoxBorderBrush,
        /// <summary>
        /// An identifier for retrieving a <see cref="Brush"/> with the default foreground of a <see cref="SearchBox"/>.
        /// </summary>
        SearchBoxForegroundBrush,
        /// <summary>
        /// An identifier for retrieving a <see cref="Brush"/> with the default background of a
        /// section header in a <see cref="ListBox"/> that uses <see cref="ListBoxStyle.Default"/>.
        /// </summary>
        SectionHeaderBackgroundBrush,
        /// <summary>
        /// An identifier for retrieving a <see cref="Brush"/> with the default foreground of a
        /// section header in a <see cref="ListBox"/> that uses <see cref="ListBoxStyle.Default"/>.
        /// </summary>
        SectionHeaderForegroundBrush,
        /// <summary>
        /// An identifier for retrieving a <see cref="Brush"/> with the default background of a <see cref="SelectList"/>.
        /// </summary>
        SelectListBackgroundBrush,
        /// <summary>
        /// An identifier for retrieving a <see cref="Brush"/> with the default border color of a <see cref="SelectList"/>.
        /// </summary>
        SelectListBorderBrush,
        /// <summary>
        /// An identifier for retrieving a <see cref="Brush"/> with the default foreground of a <see cref="SelectList"/>.
        /// </summary>
        SelectListForegroundBrush,
        /// <summary>
        /// An identifier for retrieving a <see cref="Brush"/> with the default background of a <see cref="Slider"/>.
        /// </summary>
        SliderBackgroundBrush,
        /// <summary>
        /// An identifier for retrieving a <see cref="Brush"/> with the default foreground of a <see cref="Slider"/>.
        /// </summary>
        SliderForegroundBrush,
        /// <summary>
        /// An identifier for retrieving a <see cref="Brush"/> with the default thumb color of a <see cref="Slider"/>.
        /// </summary>
        SliderThumbBrush,
        /// <summary>
        /// An identifier for retrieving a <see cref="Brush"/> with the default foreground of a <see cref="TabItem"/>.
        /// </summary>
        TabItemForegroundBrush,
        /// <summary>
        /// An identifier for retrieving a <see cref="Brush"/> with the default background of a <see cref="TabView"/>.
        /// </summary>
        TabViewBackgroundBrush,
        /// <summary>
        /// An identifier for retrieving a <see cref="Brush"/> with the default foreground of a <see cref="TabView"/>.
        /// </summary>
        TabViewForegroundBrush,
        /// <summary>
        /// An identifier for retrieving a <see cref="Brush"/> with the default background of a <see cref="TextBox"/> or similar text entry control.
        /// </summary>
        TextBoxBackgroundBrush,
        /// <summary>
        /// An identifier for retrieving a <see cref="Brush"/> with the default border color of a <see cref="TextBox"/> or similar text entry control.
        /// </summary>
        TextBoxBorderBrush,
        /// <summary>
        /// An identifier for retrieving a <see cref="Brush"/> with the default foreground of a <see cref="TextBox"/> or similar text entry control.
        /// </summary>
        TextBoxForegroundBrush,
        /// <summary>
        /// An identifier for retrieving a <see cref="Brush"/> with the default background of a <see cref="ToggleSwitch"/>.
        /// </summary>
        ToggleSwitchBackgroundBrush,
        /// <summary>
        /// An identifier for retrieving a <see cref="Brush"/> with the default border color of a <see cref="ToggleSwitch"/>.
        /// </summary>
        ToggleSwitchBorderBrush,
        /// <summary>
        /// An identifier for retrieving a <see cref="Brush"/> with the default foreground of a <see cref="ToggleSwitch"/>.
        /// </summary>
        ToggleSwitchForegroundBrush,
        /// <summary>
        /// An identifier for retrieving a <see cref="Brush"/> with the default thumb color of a <see cref="ToggleSwitch"/> that is in a <c>false</c> state.
        /// </summary>
        ToggleSwitchThumbOffBrush,
        /// <summary>
        /// An identifier for retrieving a <see cref="Brush"/> with the default thumb color of a <see cref="ToggleSwitch"/> that is in a <c>true</c> state.
        /// </summary>
        ToggleSwitchThumbOnBrush,
        /// <summary>
        /// An identifier for retrieving a <see cref="Brush"/> with the default foreground of the <see cref="P:ValueTextLabel"/> in a <see cref="ListBoxItem"/>.
        /// </summary>
        ValueLabelForegroundBrush,
        /// <summary>
        /// An identifier for retrieving a <see cref="Brush"/> with the default background of a view.
        /// </summary>
        ViewBackgroundBrush,
        /// <summary>
        /// An identifier for retrieving a <see cref="Brush"/> with the default background for the header of a view.
        /// </summary>
        ViewHeaderBackgroundBrush,
        /// <summary>
        /// An identifier for retrieving a <see cref="Brush"/> with the default foreground for the header of a view.
        /// </summary>
        ViewHeaderForegroundBrush,
        #endregion

        /// <summary>
        /// Do not use.
        /// </summary>
        EndMarker
    }
}
