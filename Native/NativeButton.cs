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


using System;
using Prism.UI;
using Prism.UI.Controls;

namespace Prism.Native
{
    /// <summary>
    /// Defines a button that is native to a particular platform.
    /// These objects are meant to be paired with platform-agnostic <see cref="Button"/> objects.
    /// </summary>
    public interface INativeButton : INativeControl
    {
        /// <summary>
        /// Occurs when the button is clicked or tapped.
        /// </summary>
        event EventHandler Clicked;

        /// <summary>
        /// Gets or sets the direction in which the button image should be placed in relation to the button title.
        /// </summary>
        [CoreBehavior(CoreBehaviors.ExpectsEarlyChangeNotification)]
        ContentDirection ContentDirection { get; set; }

        /// <summary>
        /// Gets or sets an image to display within the button.
        /// </summary>
        INativeImageSource Image { get; set; }

        /// <summary>
        /// Gets or sets the inner padding of the element.
        /// </summary>
        Thickness Padding { get; set; }

        /// <summary>
        /// Gets or sets the title of the button.
        /// </summary>
        string Title { get; set; }
    }
}
