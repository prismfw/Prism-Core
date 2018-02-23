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


using System;
using Prism.UI.Controls;
using Prism.UI.Media;

namespace Prism.Native
{
    /// <summary>
    /// Defines an action menu that is native to a particular platform.
    /// These objects are meant to be paired with platform-agnostic <see cref="ActionMenu"/> objects.
    /// </summary>
    public interface INativeActionMenu : INativeMenu, INativeVisual
    {
        /// <summary>
        /// Gets or sets the background for the menu.
        /// </summary>
        Brush Background { get; set; }

        /// <summary>
        /// Gets or sets the title of the menu's Cancel button, if one exists.
        /// </summary>
        string CancelButtonTitle { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Brush"/> to apply to the foreground content of the menu.
        /// </summary>
        Brush Foreground { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of menu items that can be displayed before they are placed into an overflow menu.
        /// </summary>
        [CoreBehavior(CoreBehaviors.ChecksRange)]
        int MaxDisplayItems { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Uri"/> of the image to use for representing the overflow menu when one is present.
        /// </summary>
        Uri OverflowImageUri { get; set; }
    }
}
