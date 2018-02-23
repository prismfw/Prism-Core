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
    /// Represents the method that is invoked when an alert button is pressed.
    /// </summary>
    /// <param name="button">The button that was pressed.</param>
    public delegate void AlertButtonPressedHandler(AlertButton button);

    /// <summary>
    /// Represents a button in an alert dialog.
    /// </summary>
    public class AlertButton
    {
        /// <summary>
        /// Gets or sets the action to perform when the button is pressed by the user.
        /// </summary>
        public AlertButtonPressedHandler Action { get; set; }

        /// <summary>
        /// Gets the title text of the button.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AlertButton"/> class.
        /// </summary>
        /// <param name="title">The title text of the button.</param>
        public AlertButton(string title)
        {
            Title = title;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AlertButton"/> class.
        /// </summary>
        /// <param name="title">The title text of the button.</param>
        /// <param name="action">The action to perform when the button is pressed.</param>
        public AlertButton(string title, AlertButtonPressedHandler action)
        {
            Title = title;
            Action = action;
        }
    }
}
