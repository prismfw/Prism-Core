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


using System;
using System.Globalization;

namespace Prism.Data
{
    /// <summary>
    /// Defines an object that converts a value from one type to another and vice versa.
    /// </summary>
    public interface IValueConverter
    {
        /// <summary>
        /// Converts the given value to the given target type.
        /// In data binding, this method is used when the value of the source property is moved to the target property.
        /// </summary>
        /// <param name="value">The value to be converted.</param>
        /// <param name="targetType">The type to which the value is to be converted.</param>
        /// <param name="parameter">An optional parameter to assist in the conversion.</param>
        /// <param name="culture">The culture to use for the conversion.</param>
        /// <returns>The converted value.</returns>
        object Convert(object value, Type targetType, object parameter, CultureInfo culture);

        /// <summary>
        /// Converts the given value to the given target type.
        /// In data binding, this method is used when the value of the target property is moved to the source property.
        /// </summary>
        /// <param name="value">The value to be converted.</param>
        /// <param name="targetType">The type to which the value is to be converted.</param>
        /// <param name="parameter">An optional parameter to assist in the conversion.</param>
        /// <param name="culture">The culture to use for the conversion.</param>
        /// <returns>The converted value.</returns>
        object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);
    }
}
