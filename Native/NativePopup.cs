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
using Prism.UI;

namespace Prism.Native
{
    /// <summary>
    /// Defines a popup that is native to a particular platform.
    /// These objects are meant to be paired with platform-agnostic <see cref="Popup"/> objects.
    /// </summary>
    [CoreBehavior(CoreBehaviors.MeasuresByContent)]
    public interface INativePopup : INativeVisual
    {
        /// <summary>
        /// Occurs when the popup has been closed.
        /// </summary>
        event EventHandler Closed;

        /// <summary>
        /// Occurs when the popup has been opened.
        /// </summary>
        event EventHandler Opened;

        /// <summary>
        /// Gets or sets the object that acts as the content of the popup.
        /// This is typically an <see cref="IView"/> or <see cref="INativeViewStack"/> instance.
        /// </summary>
        [CoreBehavior(CoreBehaviors.ChecksInequality | CoreBehaviors.TriggersChangeNotification)]
        object Content { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the popup can be dismissed by pressing outside of its bounds.
        /// </summary>
        bool IsLightDismissEnabled { get; set; }

        /// <summary>
        /// Closes the popup.
        /// </summary>
        void Close();

        /// <summary>
        /// Opens the popup using the specified presenter and presentation style.
        /// </summary>
        /// <param name="presenter">The object responsible for presenting the popup.</param>
        /// <param name="style">The style in which to present the popup.</param>
        void Open([CoreBehavior(CoreBehaviors.ChecksNullity)]object presenter, PopupPresentationStyle style);
    }
}
