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


namespace Prism.Data
{
    /// <summary>
    /// Describes the state of a data binding.
    /// </summary>
    public enum BindingStatus
    {
        /// <summary>
        /// The binding has not been activated or has been deactivated.
        /// </summary>
        Inactive = 0,
        /// <summary>
        /// The binding has been successfully activated.
        /// </summary>
        Active = 1,
        /// <summary>
        /// An error occurred while resolving the path of the source property.
        /// </summary>
        SourcePathError = 2,
        /// <summary>
        /// An error occurred while resolving the path of the target property.
        /// </summary>
        TargetPathError = 3,
        /// <summary>
        /// An error occurred while attempting to update the value of the source property.
        /// </summary>
        SourceUpdateError = 4,
        /// <summary>
        /// An error occurred while attempting to update the value of the target property.
        /// </summary>
        TargetUpdateError = 5
    }
}
