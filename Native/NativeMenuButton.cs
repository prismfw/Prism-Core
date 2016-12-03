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


using System;
using Prism.UI.Controls;

namespace Prism.Native
{
    /// <summary>
    /// Defines a menu button that is native to a particular platform.
    /// These objects are meant to be paired with platform-agnostic <see cref="MenuButton"/> objects.
    /// </summary>
    public interface INativeMenuButton : INativeMenuItem
    {
        /// <summary>
        /// Gets or sets the action to perform when the button is pressed by the user.
        /// </summary>
        Action Action { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Uri"/> of the image to display within the button.
        /// </summary>
        Uri ImageUri { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the button is enabled and should respond to user interaction.
        /// </summary>
        bool IsEnabled { get; set; }

        /// <summary>
        /// Gets or sets the title of the button.
        /// </summary>
        string Title { get; set; }
    }
}
