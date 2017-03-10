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


namespace Prism.UI.Controls
{
    /// <summary>
    /// Translates data objects into UI elements for display within a <see cref="SelectList"/>.  This class is abstract.
    /// </summary>
    public abstract class SelectListAdapter
    {
        /// <summary>
        /// Used to get the object that will be displayed as the current value of the select list.
        /// </summary>
        /// <param name="value">The object for which to return a display object.</param>
        /// <returns>The display object.</returns>
        public virtual object GetDisplayItem(object value)
        {
            return value;
        }

        /// <summary>
        /// Used to get the object that will be displayed within the selection list.
        /// </summary>
        /// <param name="value">The object in the select list's <see cref="P:Items"/> collection for which to return a display object.</param>
        /// <returns>The display object for the selection list.</returns>
        public virtual object GetListItem(object value)
        {
            return value;
        }
    }
}
