/*
Copyright (C) 2017  Prism Framework Team

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
using System.Globalization;

namespace Prism.Data
{
    /// <summary>
    /// Defines an object that is able to perform custom value conversions in a <see cref="MultiBinding"/>.
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Multi", Justification = "Valid prefix meaning 'multiple'.")]
    public interface IMultiValueConverter
    {
        /// <summary>
        /// Converts the values of each source property to a value for the target property.
        /// </summary>
        /// <param name="values">The values of each source property.</param>
        /// <param name="targetType">The type of the target property to which the values of the source properties are to be converted.</param>
        /// <param name="parameter">An optional parameter to assist in the conversion.</param>
        /// <param name="culture">The culture to use for the conversion.</param>
        /// <returns>The converted value for the target property.</returns>
        object Convert(object[] values, Type targetType, object parameter, CultureInfo culture);

        /// <summary>
        /// Converts the value of the target property to values for each of the source properties.
        /// </summary>
        /// <param name="value">The value of the target property.</param>
        /// <param name="targetTypes">The types of each source property to which the value of the target property is to be converted.</param>
        /// <param name="parameter">An optional parameter to assist in the conversion.</param>
        /// <param name="culture">The culture to use for the conversion.</param>
        /// <returns>
        /// An <see cref="Array"/> of values, one for each source property.
        /// If the array has more values than the number of source properties, the excess values will be ignored.
        /// If the array has fewer values than the number of source properties, the excess source properties will not be set.
        /// </returns>
        object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture);
    }
}
