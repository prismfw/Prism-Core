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


namespace Prism.UI.Controls
{
    /// <summary>
    /// Describes the available accessories that can be added to a <see cref="ListBoxItem"/>.
    /// </summary>
    public enum ListBoxItemAccessory
    {
        /// <summary>
        /// No accessory.
        /// </summary>
        None = 0,
        /// <summary>
        /// A visual indicator that signifies to the user that the item is selectable.
        /// </summary>
        Indicator = 1,
        /// <summary>
        /// A pressable button that signifies to the user there is additional information
        /// available related to the data in the item.
        /// </summary>
        InfoButton = 2,
        /// <summary>
        /// A combination of <see cref="Indicator"/> and <see cref="InfoButton"/>.
        /// </summary>
        InfoIndicator = 3
    }
}
