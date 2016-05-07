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


using Prism.UI;

namespace Prism.Native
{
    /// <summary>
    /// Defines a modal alert or confirmation dialog that is native to a particular platform.
    /// These objects are meant to be paired with platform-agnostic <see cref="Alert"/> objects.
    /// </summary>
    public interface INativeAlert
    {
        /// <summary>
        /// Gets the number of buttons that have been added to the alert.
        /// </summary>
        int ButtonCount { get; }

        /// <summary>
        /// Gets or sets the zero-based index of the button that is mapped to the ESC key on desktop platforms.
        /// </summary>
        int CancelButtonIndex { get; set; }

        /// <summary>
        /// Gets or sets the zero-based index of the button that is mapped to the Enter key on desktop platforms.
        /// </summary>
        int DefaultButtonIndex { get; set; }

        /// <summary>
        /// Gets the message text for the alert.
        /// </summary>
        string Message { get; }

        /// <summary>
        /// Gets the title text for the alert.
        /// </summary>
        string Title { get; }

        /// <summary>
        /// Adds the specified <see cref="AlertButton"/> to the alert.
        /// </summary>
        /// <param name="button">The button to add.</param>
        void AddButton(AlertButton button);

        /// <summary>
        /// Gets the button at the specified zero-based index.
        /// </summary>
        /// <param name="index">The zero-based index of the button to retrieve.</param>
        /// <returns>The <see cref="AlertButton"/> at the specified index -or- <c>null</c> if there is no button.</returns>
        AlertButton GetButton(int index);

        /// <summary>
        /// Modally presents the alert.
        /// </summary>
        void Show();
    }
}
