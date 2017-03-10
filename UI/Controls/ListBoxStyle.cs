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
    /// Describes the rendering styles of a <see cref="ListBox"/>.
    /// </summary>
    public enum ListBoxStyle
    {
        /// <summary>
        /// The default rendering style.
        /// </summary>
        Default = 0,
        /// <summary>
        /// The items in the list are rendered in groups.
        /// Any platform that does not support this style will use the default style instead.
        /// </summary>
        Grouped = 1
    }
}

