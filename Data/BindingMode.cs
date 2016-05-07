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


namespace Prism.Data
{
    /// <summary>
    /// Describes the direction in which property values are passed in a data binding.
    /// </summary>
    public enum BindingMode
    {
        /// <summary>
        /// The target property will determine the direction of the binding.
        /// This will either be one-way-to-target or two-way, depending on the property's metadata.
        /// </summary>
        Default = 0,
        /// <summary>
        /// Changes to the source property should update the target property.
        /// </summary>
        OneWayToTarget = 1,
        /// <summary>
        /// Changes to the target property should update the source property.
        /// </summary>
        OneWayToSource = 2,
        /// <summary>
        /// Changes to the source property should update the target property,
        /// and changes to the target property should update the source property.
        /// </summary>
        TwoWay = 3,
        /// <summary>
        /// The value of the source property should be passed to the target property only once upon activation of the binding.
        /// The binding is then deactivated.
        /// </summary>
        OneTimeToTarget = 4
    }
}
