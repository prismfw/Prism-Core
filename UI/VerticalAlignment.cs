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


namespace Prism.UI
{
    /// <summary>
    /// Describes the manner in which a UI element is aligned within the vertical space allocated for it.
    /// </summary>
    public enum VerticalAlignment
    {
        /// <summary>
        /// The element is aligned to the top edge of the space.
        /// </summary>
        Top = 0,
        /// <summary>
        /// The element is centered within the space.
        /// </summary>
        Center = 1,
        /// <summary>
        /// The element is aligned to the bottom edge of the space.
        /// </summary>
        Bottom = 2,
        /// <summary>
        /// The element is stretched to fill the space.
        /// </summary>
        Stretch = 3
    }
}
