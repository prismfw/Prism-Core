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


namespace Prism.Input
{
    /// <summary>
    /// Describes the type of data that is expected from an input method.
    /// </summary>
    public enum InputType
    {
        /// <summary>
        /// Alphabet characters, punctuation, and whole or fractional numbers.
        /// </summary>
        Alphanumeric = 0,
        /// <summary>
        /// Whole or fractional numbers.
        /// </summary>
        Number = 1,
        /// <summary>
        /// Whole or fractional numbers with relevant symbols, such as currency symbols, parentheses, etc.
        /// </summary>
        NumberAndSymbol = 2,
        /// <summary>
        /// Telephone numbers.
        /// </summary>
        Phone = 3,
        /// <summary>
        /// Web addresses, file paths, etc.
        /// </summary>
        Url = 4,
        /// <summary>
        /// Email addresses.
        /// </summary>
        EmailAddress = 5
    }
}
