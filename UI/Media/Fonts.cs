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
using Prism.Native;
using Prism.UI.Controls;

#if !DEBUG
using System.Diagnostics;
#endif

namespace Prism.UI.Media
{
    /// <summary>
    /// Provides a set of predefined font preferences.
    /// </summary>
    public static class Fonts
    {
        /// <summary>
        /// Gets the preferred font size for a <see cref="Button"/>.
        /// </summary>
        public static double ButtonFontSize
        {
            get { return Current.ButtonFontSize; }
        }

        /// <summary>
        /// Gets the preferred font style for a <see cref="Button"/>.
        /// </summary>
        public static FontStyle ButtonFontStyle
        {
            get { return Current.ButtonFontStyle; }
        }

        /// <summary>
        /// Gets the preferred font size for a <see cref="DatePicker"/>.
        /// </summary>
        public static double DatePickerFontSize
        {
            get { return Current.DatePickerFontSize; }
        }

        /// <summary>
        /// Gets the preferred font style for a <see cref="DatePicker"/>.
        /// </summary>
        public static FontStyle DatePickerFontStyle
        {
            get { return Current.DatePickerFontStyle; }
        }

        /// <summary>
        /// Gets the default font family for UI elements that do not have a font family preference.
        /// </summary>
        public static FontFamily DefaultFontFamily
        {
            get { return Current.DefaultFontFamily; }
        }

        /// <summary>
        /// Gets the preferred font size for the <see cref="P:DetailTextLabel"/> of a <see cref="ListBoxItem"/>.
        /// </summary>
        public static double DetailLabelFontSize
        {
            get { return Current.DetailLabelFontSize; }
        }

        /// <summary>
        /// Gets the preferred font style for the <see cref="P:DetailTextLabel"/> of a <see cref="ListBoxItem"/>.
        /// </summary>
        public static FontStyle DetailLabelFontStyle
        {
            get { return Current.DetailLabelFontStyle; }
        }

        /// <summary>
        /// Gets the preferred font size for a section footer in a <see cref="ListBox"/> that uses <see cref="ListBoxStyle.Grouped"/>.
        /// </summary>
        public static double GroupedSectionFooterFontSize
        {
            get { return Current.GroupedSectionFooterFontSize; }
        }

        /// <summary>
        /// Gets the preferred font style for a section footer in a <see cref="ListBox"/> that uses <see cref="ListBoxStyle.Grouped"/>.
        /// </summary>
        public static FontStyle GroupedSectionFooterFontStyle
        {
            get { return Current.GroupedSectionFooterFontStyle; }
        }

        /// <summary>
        /// Gets the preferred font size for a section header in a <see cref="ListBox"/> that uses <see cref="ListBoxStyle.Grouped"/>.
        /// </summary>
        public static double GroupedSectionHeaderFontSize
        {
            get { return Current.GroupedSectionHeaderFontSize; }
        }

        /// <summary>
        /// Gets the preferred font style for a section header in a <see cref="ListBox"/> that uses <see cref="ListBoxStyle.Grouped"/>.
        /// </summary>
        public static FontStyle GroupedSectionHeaderFontStyle
        {
            get { return Current.GroupedSectionHeaderFontStyle; }
        }

        /// <summary>
        /// Gets the preferred font size for the header of a view.
        /// </summary>
        public static double HeaderFontSize
        {
            get { return Current.HeaderFontSize; }
        }

        /// <summary>
        /// Gets the preferred font style for the header of a view.
        /// </summary>
        public static FontStyle HeaderFontStyle
        {
            get { return Current.HeaderFontStyle; }
        }

        /// <summary>
        /// Gets the preferred font size for the title text of a <see cref="LoadIndicator"/>.
        /// </summary>
        public static double LoadIndicatorFontSize
        {
            get { return Current.LoadIndicatorFontSize; }
        }

        /// <summary>
        /// Gets the preferred font style for the title text of a <see cref="LoadIndicator"/>.
        /// </summary>
        public static FontStyle LoadIndicatorFontStyle
        {
            get { return Current.LoadIndicatorFontStyle; }
        }

        /// <summary>
        /// Gets the preferred font size for a <see cref="SearchBox"/>.
        /// </summary>
        public static double SearchBoxFontSize
        {
            get { return Current.SearchBoxFontSize; }
        }

        /// <summary>
        /// Gets the preferred font style for a <see cref="SearchBox"/>.
        /// </summary>
        public static FontStyle SearchBoxFontStyle
        {
            get { return Current.SearchBoxFontStyle; }
        }

        /// <summary>
        /// Gets the preferred font size for a section footer in a <see cref="ListBox"/> that uses <see cref="ListBoxStyle.Default"/>.
        /// </summary>
        public static double SectionFooterFontSize
        {
            get { return Current.SectionFooterFontSize; }
        }

        /// <summary>
        /// Gets the preferred font size for a section footer in a <see cref="ListBox"/> that uses <see cref="ListBoxStyle.Default"/>.
        /// </summary>
        public static FontStyle SectionFooterFontStyle
        {
            get { return Current.SectionFooterFontStyle; }
        }

        /// <summary>
        /// Gets the preferred font size for a section header in a <see cref="ListBox"/> that uses <see cref="ListBoxStyle.Default"/>.
        /// </summary>
        public static double SectionHeaderFontSize
        {
            get { return Current.SectionHeaderFontSize; }
        }

        /// <summary>
        /// Gets the preferred font style for a section header in a <see cref="ListBox"/> that uses <see cref="ListBoxStyle.Default"/>.
        /// </summary>
        public static FontStyle SectionHeaderFontStyle
        {
            get { return Current.SectionHeaderFontStyle; }
        }

        /// <summary>
        /// Gets the preferred font size for a <see cref="SelectList"/>.
        /// </summary>
        public static double SelectListFontSize
        {
            get { return Current.SelectListFontSize; }
        }

        /// <summary>
        /// Gets the preferred font style for a <see cref="SelectList"/>.
        /// </summary>
        public static FontStyle SelectListFontStyle
        {
            get { return Current.SelectListFontStyle; }
        }

        /// <summary>
        /// Gets the preferred font size for a standard text label.
        /// </summary>
        public static double StandardLabelFontSize
        {
            get { return Current.StandardLabelFontSize; }
        }

        /// <summary>
        /// Gets the preferred font style for a standard text label.
        /// </summary>
        public static FontStyle StandardLabelFontStyle
        {
            get { return Current.StandardLabelFontStyle; }
        }

        /// <summary>
        /// Gets the preferred font size for a <see cref="TabItem"/>.
        /// </summary>
        public static double TabItemFontSize
        {
            get { return Current.TabItemFontSize; }
        }

        /// <summary>
        /// Gets the preferred font style for a <see cref="TabItem"/>.
        /// </summary>
        public static FontStyle TabItemFontStyle
        {
            get { return Current.TabItemFontStyle; }
        }

        /// <summary>
        /// Gets the preferred font size for a <see cref="TextArea"/>.
        /// </summary>
        public static double TextAreaFontSize
        {
            get { return Current.TextAreaFontSize; }
        }

        /// <summary>
        /// Gets the preferred font style for a <see cref="TextArea"/>.
        /// </summary>
        public static FontStyle TextAreaFontStyle
        {
            get { return Current.TextAreaFontStyle; }
        }

        /// <summary>
        /// Gets the preferred font size for a <see cref="TextBox"/>.
        /// </summary>
        public static double TextBoxFontSize
        {
            get { return Current.TextBoxFontSize; }
        }

        /// <summary>
        /// Gets the preferred font style for a <see cref="TextBox"/>.
        /// </summary>
        public static FontStyle TextBoxFontStyle
        {
            get { return Current.TextBoxFontStyle; }
        }

        /// <summary>
        /// Gets the preferred font size for a <see cref="TimePicker"/>.
        /// </summary>
        public static double TimePickerFontSize
        {
            get { return Current.TimePickerFontSize; }
        }

        /// <summary>
        /// Gets the preferred font style for a <see cref="TimePicker"/>.
        /// </summary>
        public static FontStyle TimePickerFontStyle
        {
            get { return Current.TimePickerFontStyle; }
        }

        /// <summary>
        /// Gets the preferred font size for the <see cref="P:ValueTextLabel"/> of a <see cref="ListBoxItem"/>.
        /// </summary>
        public static double ValueLabelFontSize
        {
            get { return Current.ValueLabelFontSize; }
        }

        /// <summary>
        /// Gets the preferred font style for the <see cref="P:ValueTextLabel"/> of a <see cref="ListBoxItem"/>.
        /// </summary>
        public static FontStyle ValueLabelFontStyle
        {
            get { return Current.ValueLabelFontStyle; }
        }

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private static INativeFonts Current
        {
            get { return Application.Resolve<INativeFonts>(); }
        }

        /// <summary>
        /// Gets the names of all available fonts.
        /// </summary>
        /// <returns>An <see cref="Array"/> containing the names of all available fonts.</returns>
        public static string[] GetAvailableFontNames()
        {
            return Current.GetAvailableFontNames();
        }
    }
}
