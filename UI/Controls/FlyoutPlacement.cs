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
    /// Describes the position of a flyout object in relation to its placement target.
    /// </summary>
    public enum FlyoutPlacement
    {
        /// <summary>
        /// The flyout determines its own position.
        /// </summary>
        Auto = 0,
        /// <summary>
        /// The flyout is positioned above its target.
        /// </summary>
        Top = 1,
        /// <summary>
        /// The flyout is positioned below its target.
        /// </summary>
        Bottom = 2,
        /// <summary>
        /// The flyout is positioned to the left of its target.
        /// </summary>
        Left = 3,
        /// <summary>
        /// The flyout is positioned to the right of its target.
        /// </summary>
        Right = 4
    }
}
