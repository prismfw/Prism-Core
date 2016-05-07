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
using Prism.UI.Media;

namespace Prism.Native
{
    /// <summary>
    /// Defines a set of preferred fonts for a particular platform.
    /// </summary>
    public interface INativeFonts
    {
        /// <summary>
        /// Gets the preferred font size for a button.
        /// </summary>
        double ButtonFontSize { get; }

        /// <summary>
        /// Gets the preferred font style for a button.
        /// </summary>
        FontStyle ButtonFontStyle { get; }

        /// <summary>
        /// Gets the preferred font size for a date picker.
        /// </summary>
        double DatePickerFontSize { get; }

        /// <summary>
        /// Gets the preferred font style for a date picker.
        /// </summary>
        FontStyle DatePickerFontStyle { get; }

        /// <summary>
        /// Gets the default font family for UI elements that do not have a font family preference.
        /// </summary>
        FontFamily DefaultFontFamily { get; }

        /// <summary>
        /// Gets the preferred font size for the detail label of a list box item.
        /// </summary>
        double DetailLabelFontSize { get; }

        /// <summary>
        /// Gets the preferred font style for the detail label of a list box item.
        /// </summary>
        FontStyle DetailLabelFontStyle { get; }

        /// <summary>
        /// Gets the preferred font size for a section footer in a list box that uses a grouped style.
        /// </summary>
        double GroupedSectionFooterFontSize { get; }

        /// <summary>
        /// Gets the preferred font style for a section footer in a list box that uses a grouped style.
        /// </summary>
        FontStyle GroupedSectionFooterFontStyle { get; }

        /// <summary>
        /// Gets the preferred font size for a section header in a list box that uses a grouped style.
        /// </summary>
        double GroupedSectionHeaderFontSize { get; }

        /// <summary>
        /// Gets the preferred font style for a section header in a list box that uses a grouped style.
        /// </summary>
        FontStyle GroupedSectionHeaderFontStyle { get; }

        /// <summary>
        /// Gets the preferred font size for the header of a view.
        /// </summary>
        double HeaderFontSize { get; }

        /// <summary>
        /// Gets the preferred font style for the header of a view.
        /// </summary>
        FontStyle HeaderFontStyle { get; }

        /// <summary>
        /// Gets the preferred font size for the title text of a load indicator.
        /// </summary>
        double LoadIndicatorFontSize { get; }

        /// <summary>
        /// Gets the preferred font style for the title text of a load indicator.
        /// </summary>
        FontStyle LoadIndicatorFontStyle { get; }

        /// <summary>
        /// Gets the preferred font size for a search box.
        /// </summary>
        double SearchBoxFontSize { get; }

        /// <summary>
        /// Gets the preferred font style for a search box.
        /// </summary>
        FontStyle SearchBoxFontStyle { get; }

        /// <summary>
        /// Gets the preferred font size for a section footer in a list box that uses the default style.
        /// </summary>
        double SectionFooterFontSize { get; }

        /// <summary>
        /// Gets the preferred font style for a section footer in a list box that uses the default style.
        /// </summary>
        FontStyle SectionFooterFontStyle { get; }

        /// <summary>
        /// Gets the preferred font size for a section header in a list box that uses the default style.
        /// </summary>
        double SectionHeaderFontSize { get; }

        /// <summary>
        /// Gets the preferred font style for a section header in a list box that uses the default style.
        /// </summary>
        FontStyle SectionHeaderFontStyle { get; }

        /// <summary>
        /// Gets the preferred font size for a select list.
        /// </summary>
        double SelectListFontSize { get; }

        /// <summary>
        /// Gets the preferred font style for a select list.
        /// </summary>
        FontStyle SelectListFontStyle { get; }

        /// <summary>
        /// Gets the preferred font size for a standard text label.
        /// </summary>
        double StandardLabelFontSize { get; }

        /// <summary>
        /// Gets the preferred font style for a standard text label.
        /// </summary>
        FontStyle StandardLabelFontStyle { get; }

        /// <summary>
        /// Gets the preferred font size for a tab item.
        /// </summary>
        double TabItemFontSize { get; }

        /// <summary>
        /// Gets the preferred font style for a tab item.
        /// </summary>
        FontStyle TabItemFontStyle { get; }

        /// <summary>
        /// Gets the preferred font size for a text area.
        /// </summary>
        double TextAreaFontSize { get; }

        /// <summary>
        /// Gets the preferred font style for a text area.
        /// </summary>
        FontStyle TextAreaFontStyle { get; }

        /// <summary>
        /// Gets the preferred font size for a text box.
        /// </summary>
        double TextBoxFontSize { get; }

        /// <summary>
        /// Gets the preferred font style for a text box.
        /// </summary>
        FontStyle TextBoxFontStyle { get; }

        /// <summary>
        /// Gets the preferred font size for a time picker.
        /// </summary>
        double TimePickerFontSize { get; }

        /// <summary>
        /// Gets the preferred font style for a time picker.
        /// </summary>
        FontStyle TimePickerFontStyle { get; }

        /// <summary>
        /// Gets the preferred font size for the value label of a list box item.
        /// </summary>
        double ValueLabelFontSize { get; }

        /// <summary>
        /// Gets the preferred font style for the value label of a list box item.
        /// </summary>
        FontStyle ValueLabelFontStyle { get; }

        /// <summary>
        /// Gets the names of all available fonts.
        /// </summary>
        /// <returns>An <see cref="Array"/> containing the names of all available fonts.</returns>
        string[] GetAvailableFontNames();
    }
}
