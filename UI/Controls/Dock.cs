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
    /// Describes the position of a UI element within a <see cref="DockPanel"/>.
    /// </summary>
    public enum Dock
    {
        /// <summary>
        /// The element is positioned on the left side of the panel.
        /// </summary>
        Left = 0,
        /// <summary>
        /// The element is positioned at the top of the panel.
        /// </summary>
        Top = 1,
        /// <summary>
        /// The element is positioned on the right side of the panel.
        /// </summary>
        Right = 2,
        /// <summary>
        /// The element is positioned at the bottom of the panel.
        /// </summary>
        Bottom = 3
    }
}
