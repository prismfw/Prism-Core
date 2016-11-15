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
        /// Gets a <see cref="ResourceKey"/> for retrieving the default width of the border around a generic <see cref="Control"/>.
        /// </summary>
        public static ResourceKey ControlBorderWidthKey { get; } = new ResourceKey("SystemControlBorderWidth", typeof(double), 0.0);

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the default maximum number of items that can be
        /// displayed in an <see cref="ActionMenu"/> before they are placed into an overflow menu.
        /// </summary>
        public static ResourceKey ActionMenuMaxDisplayItemsKey { get; } = new ResourceKey("SystemActionMenuMaxDisplayItems", typeof(int), 2);

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving a value indicating whether the separator of a <see cref="ListBoxItem"/>
        /// should be automatically indented in order to be flush with the text labels of the item.
        /// </summary>
        public static ResourceKey AutomaticallyIndentSeparatorsKey { get; } = new ResourceKey("SystemAutomaticallyIndentSeparators", typeof(bool), true);

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the default width of the border around a <see cref="Button"/>.
        /// </summary>
        public static ResourceKey ButtonBorderWidthKey { get; } = new ResourceKey("SystemButtonBorderWidth", typeof(double), ControlBorderWidthKey);

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the default amount of padding between a <see cref="Button"/>'s content and its edges.
        /// </summary>
        public static ResourceKey ButtonPaddingKey { get; } = new ResourceKey("SystemButtonPadding", typeof(Thickness), new Thickness());

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the default width of the border around a <see cref="DatePicker"/>.
        /// </summary>
        public static ResourceKey DatePickerBorderWidthKey { get; } = new ResourceKey("SystemDatePickerBorderWidth", typeof(double), ControlBorderWidthKey);

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the height of a horizontal scroll bar.
        /// </summary>
        public static ResourceKey HorizontalScrollBarHeightKey { get; } = new ResourceKey("SystemHorizontalScrollBarHeight", typeof(double));

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the default height of a <see cref="ListBoxItem"/> with a <see cref="P:DetailTextLabel"/>.
        /// </summary>
        public static ResourceKey ListBoxItemDetailHeightKey { get; } = new ResourceKey("SystemListBoxItemDetailHeight", typeof(double), 0.0);

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the size of the <see cref="ListBoxItemAccessory.Indicator"/> accessory in a <see cref="ListBoxItem"/>.
        /// </summary>
        public static ResourceKey ListBoxItemIndicatorSizeKey { get; } = new ResourceKey("SystemListBoxItemIndicatorSize", typeof(Size));

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the size of the <see cref="ListBoxItemAccessory.InfoButton"/> accessory in a <see cref="ListBoxItem"/>.
        /// </summary>
        public static ResourceKey ListBoxItemInfoButtonSizeKey { get; } = new ResourceKey("SystemListBoxItemInfoButtonSize", typeof(Size));

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the size of the <see cref="ListBoxItemAccessory.InfoIndicator"/> accessory in a <see cref="ListBoxItem"/>.
        /// </summary>
        public static ResourceKey ListBoxItemInfoIndicatorSizeKey { get; } = new ResourceKey("SystemListBoxItemInfoIndicatorSize", typeof(Size));

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the default height of a standard <see cref="ListBoxItem"/>.
        /// </summary>
        public static ResourceKey ListBoxItemStandardHeightKey { get; } = new ResourceKey("SystemListBoxItemStandardHeight", typeof(double), 0.0);

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the default width of the border around a <see cref="PasswordBox"/>.
        /// </summary>
        public static ResourceKey PasswordBoxBorderWidthKey { get; } = new ResourceKey("SystemPasswordBoxBorderWidth", typeof(double), ControlBorderWidthKey);

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the size of a <see cref="Popup"/> when presented with <see cref="PopupPresentationStyle.Default"/>.
        /// </summary>
        public static ResourceKey PopupSizeKey { get; } = new ResourceKey("SystemPopupSize", typeof(Size));

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the default width of the border around a <see cref="SearchBox"/>.
        /// </summary>
        public static ResourceKey SearchBoxBorderWidthKey { get; } = new ResourceKey("SystemSearchBoxBorderWidth", typeof(double), ControlBorderWidthKey);

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the default width of the border around a <see cref="SelectList"/>.
        /// </summary>
        public static ResourceKey SelectListBorderWidthKey { get; } = new ResourceKey("SystemSelectListBorderWidth", typeof(double), ControlBorderWidthKey);

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the default amount of padding between a <see cref="SelectList"/>'s display item and its edges.
        /// </summary>
        public static ResourceKey SelectListDisplayItemPaddingKey { get; } = new ResourceKey("SystemSelectListDisplayItemPadding", typeof(Thickness), new Thickness());

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the default amount of padding between a <see cref="SelectList"/>'s list item and its edges.
        /// </summary>
        public static ResourceKey SelectListListItemPaddingKey { get; } = new ResourceKey("SystemSelectListListItemPadding", typeof(Thickness), new Thickness());

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the default width of the border around a <see cref="TextArea"/>.
        /// </summary>
        public static ResourceKey TextAreaBorderWidthKey { get; } = new ResourceKey("SystemTextAreaBorderWidth", typeof(double), ControlBorderWidthKey);

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the default width of the border around a <see cref="TextBox"/>.
        /// </summary>
        public static ResourceKey TextBoxBorderWidthKey { get; } = new ResourceKey("SystemTextBoxBorderWidth", typeof(double), ControlBorderWidthKey);

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the default width of the border around a <see cref="TimePicker"/>.
        /// </summary>
        public static ResourceKey TimePickerBorderWidthKey { get; } = new ResourceKey("SystemTimePickerBorderWidth", typeof(double), ControlBorderWidthKey);

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the width of a vertical scroll bar.
        /// </summary>
        public static ResourceKey VerticalScrollBarWidthKey { get; } = new ResourceKey("SystemVerticalScrollBarWidth", typeof(double));
        #endregion

        #region Font Values
        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the default font family for UI objects.
        /// </summary>
        public static ResourceKey BaseFontFamilyKey { get; } = new ResourceKey("SystemBaseFontFamily", typeof(FontFamily));

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the default font size for UI objects.
        /// </summary>
        public static ResourceKey BaseFontSizeKey { get; } = new ResourceKey("SystemBaseFontSize", typeof(double), 12.0);

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the default font style for UI objects.
        /// </summary>
        public static ResourceKey BaseFontStyleKey { get; } = new ResourceKey("SystemBaseFontStyle", typeof(FontStyle), FontStyle.Normal);

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the default font size of a <see cref="Label"/>.
        /// </summary>
        public static ResourceKey LabelFontSizeKey { get; } = new ResourceKey("SystemLabelFontSize", typeof(double), BaseFontSizeKey);

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the default font style of a <see cref="Label"/>.
        /// </summary>
        public static ResourceKey LabelFontStyleKey { get; } = new ResourceKey("SystemLabelFontStyle", typeof(FontStyle), BaseFontStyleKey);

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the default font size of a generic <see cref="Control"/>.
        /// </summary>
        public static ResourceKey ControlFontSizeKey { get; } = new ResourceKey("SystemControlFontSize", typeof(double), BaseFontSizeKey);

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the default font style of a generic <see cref="Control"/>.
        /// </summary>
        public static ResourceKey ControlFontStyleKey { get; } = new ResourceKey("SystemControlFontStyle", typeof(FontStyle), BaseFontStyleKey);

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the default font size of a <see cref="TextBox"/>.
        /// </summary>
        public static ResourceKey TextBoxFontSizeKey { get; } = new ResourceKey("SystemTextBoxFontSize", typeof(double), ControlFontSizeKey);

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the default font style of a <see cref="TextBox"/>.
        /// </summary>
        public static ResourceKey TextBoxFontStyleKey { get; } = new ResourceKey("SystemTextBoxFontStyle", typeof(FontStyle), ControlFontStyleKey);

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the default font size of a <see cref="Button"/>.
        /// </summary>
        public static ResourceKey ButtonFontSizeKey { get; } = new ResourceKey("SystemButtonFontSize", typeof(double), ControlFontSizeKey);

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the default font style of a <see cref="Button"/>.
        /// </summary>
        public static ResourceKey ButtonFontStyleKey { get; } = new ResourceKey("SystemButtonFontStyle", typeof(FontStyle), ControlFontStyleKey);

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the default font size of a <see cref="DatePicker"/>.
        /// </summary>
        public static ResourceKey DatePickerFontSizeKey { get; } = new ResourceKey("SystemDatePickerFontSize", typeof(double), ControlFontStyleKey);

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the default font style of a <see cref="DatePicker"/>.
        /// </summary>
        public static ResourceKey DatePickerFontStyleKey { get; } = new ResourceKey("SystemDatePickerFontStyle", typeof(FontStyle), ControlFontStyleKey);

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the default font size of the <see cref="P:DetailTextLabel"/> in a <see cref="ListBoxItem"/>.
        /// </summary>
        public static ResourceKey DetailLabelFontSizeKey { get; } = new ResourceKey("SystemDetailLabelFontSize", typeof(double), LabelFontSizeKey);

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the default font style of the <see cref="P:DetailTextLabel"/> in a <see cref="ListBoxItem"/>.
        /// </summary>
        public static ResourceKey DetailLabelFontStyleKey { get; } = new ResourceKey("SystemDetailLabelFontStyle", typeof(FontStyle), LabelFontStyleKey);

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the default font size of a section header in a <see cref="ListBox"/> that uses <see cref="ListBoxStyle.Grouped"/>.
        /// </summary>
        public static ResourceKey GroupedSectionHeaderFontSizeKey { get; } = new ResourceKey("SystemGroupedSectionHeaderFontSize", typeof(double), BaseFontSizeKey);

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the default font style of a section header in a <see cref="ListBox"/> that uses <see cref="ListBoxStyle.Grouped"/>.
        /// </summary>
        public static ResourceKey GroupedSectionHeaderFontStyleKey { get; } = new ResourceKey("SystemGroupedSectionHeaderFontStyle", typeof(FontStyle), BaseFontStyleKey);

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the default font size of the title text on a <see cref="LoadIndicator"/>.
        /// </summary>
        public static ResourceKey LoadIndicatorFontSizeKey { get; } = new ResourceKey("SystemLoadIndicatorFontSize", typeof(double), BaseFontSizeKey);

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the default font style of the title text on a <see cref="LoadIndicator"/>.
        /// </summary>
        public static ResourceKey LoadIndicatorFontStyleKey { get; } = new ResourceKey("SystemLoadIndicatorFontStyle", typeof(FontStyle), BaseFontStyleKey);

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the default font size of a <see cref="PasswordBox"/>.
        /// </summary>
        public static ResourceKey PasswordBoxFontSizeKey { get; } = new ResourceKey("SystemPasswordBoxFontSize", typeof(double), TextBoxFontSizeKey);

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the default font style of a <see cref="PasswordBox"/>.
        /// </summary>
        public static ResourceKey PasswordBoxFontStyleKey { get; } = new ResourceKey("SystemPasswordBoxFontStyle", typeof(FontStyle), TextBoxFontStyleKey);

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the default font size of a <see cref="SearchBox"/>.
        /// </summary>
        public static ResourceKey SearchBoxFontSizeKey { get; } = new ResourceKey("SystemSearchBoxFontSize", typeof(double), TextBoxFontSizeKey);

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the default font style of a <see cref="SearchBox"/>.
        /// </summary>
        public static ResourceKey SearchBoxFontStyleKey { get; } = new ResourceKey("SystemSearchBoxFontStyle", typeof(FontStyle), TextBoxFontStyleKey);

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the default font size of a section header in a <see cref="ListBox"/> that uses <see cref="ListBoxStyle.Default"/>.
        /// </summary>
        public static ResourceKey SectionHeaderFontSizeKey { get; } = new ResourceKey("SystemSectionHeaderFontSize", typeof(double), BaseFontSizeKey);

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the default font style of a section header in a <see cref="ListBox"/> that uses <see cref="ListBoxStyle.Default"/>.
        /// </summary>
        public static ResourceKey SectionHeaderFontStyleKey { get; } = new ResourceKey("SystemSectionHeaderFontStyle", typeof(FontStyle), BaseFontStyleKey);

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the default font size of a <see cref="SelectList"/>.
        /// </summary>
        public static ResourceKey SelectListFontSizeKey { get; } = new ResourceKey("SystemSelectListFontSize", typeof(double), ControlFontSizeKey);

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the default font style of a <see cref="SelectList"/>.
        /// </summary>
        public static ResourceKey SelectListFontStyleKey { get; } = new ResourceKey("SystemSelectListFontStyle", typeof(FontStyle), ControlFontStyleKey);

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the default font size of a <see cref="TabItem"/>.
        /// </summary>
        public static ResourceKey TabItemFontSizeKey { get; } = new ResourceKey("SystemTabItemFontSize", typeof(double), BaseFontSizeKey);

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the default font style of a <see cref="TabItem"/>.
        /// </summary>
        public static ResourceKey TabItemFontStyleKey { get; } = new ResourceKey("SystemTabItemFontStyle", typeof(FontStyle), BaseFontStyleKey);

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the default font size of a <see cref="TextArea"/>.
        /// </summary>
        public static ResourceKey TextAreaFontSizeKey { get; } = new ResourceKey("SystemTextAreaFontSize", typeof(double), TextBoxFontSizeKey);

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the default font style of a <see cref="TextArea"/>.
        /// </summary>
        public static ResourceKey TextAreaFontStyleKey { get; } = new ResourceKey("SystemTextAreaFontStyle", typeof(FontStyle), TextBoxFontStyleKey);

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the default font size of a <see cref="TimePicker"/>.
        /// </summary>
        public static ResourceKey TimePickerFontSizeKey { get; } = new ResourceKey("SystemTimePickerFontSize", typeof(double), ControlFontSizeKey);

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the default font style of a <see cref="TimePicker"/>.
        /// </summary>
        public static ResourceKey TimePickerFontStyleKey { get; } = new ResourceKey("SystemTimePickerFontStyle", typeof(FontStyle), ControlFontStyleKey);

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the default font size of the <see cref="P:ValueTextLabel"/> in a <see cref="ListBoxItem"/>.
        /// </summary>
        public static ResourceKey ValueLabelFontSizeKey { get; } = new ResourceKey("SystemValueLabelFontSize", typeof(double), LabelFontSizeKey);

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the default font style of the <see cref="P:ValueTextLabel"/> in a <see cref="ListBoxItem"/>.
        /// </summary>
        public static ResourceKey ValueLabelFontStyleKey { get; } = new ResourceKey("SystemValueLabelFontStyle", typeof(FontStyle), LabelFontStyleKey);

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the default font size for the header of a view.
        /// </summary>
        public static ResourceKey ViewHeaderFontSizeKey { get; } = new ResourceKey("SystemViewHeaderFontSize", typeof(double), BaseFontSizeKey);

        /// <summary>
        /// Gets a <see cref="ResourceKey"/> for retrieving the default font style for the header of a view.
        /// </summary>
        public static ResourceKey ViewHeaderFontStyleKey { get; } = new ResourceKey("SystemViewHeaderFontStyle", typeof(FontStyle), BaseFontStyleKey);
        #endregion

        #region Brushes
        #endregion
    }
}
