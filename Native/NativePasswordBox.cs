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
using Prism.Input;
using Prism.UI;
using Prism.UI.Controls;

namespace Prism.Native
{
    /// <summary>
    /// Defines a text entry control for entering passwords and other sensitive information that is native to a particular platform.
    /// These objects are meant to be paired with platform-agnostic <see cref="PasswordBox"/> objects.
    /// </summary>
    public interface INativePasswordBox : INativeControl
    {
        /// <summary>
        /// Occurs when the action key, most commonly mapped to the "Return" key, is pressed while the control has focus.
        /// </summary>
        event EventHandler<HandledEventArgs> ActionKeyPressed;

        /// <summary>
        /// Occurs when the value of the <see cref="P:Password"/> property has changed.
        /// </summary>
        event EventHandler PasswordChanged;

        /// <summary>
        /// Gets or sets the type of action key to use for the soft keyboard when the control has focus.
        /// </summary>
        ActionKeyType ActionKeyType { get; set; }

        /// <summary>
        /// Gets or sets the type of text that the user is expected to input.
        /// </summary>
        [CoreBehavior(CoreBehaviors.ChecksRange)]
        InputType InputType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the password is displayed in plain text.
        /// </summary>
        bool IsPasswordVisible { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of characters that are allowed to be entered into the control.
        /// A value of 0 means there is no limit.
        /// </summary>
        [CoreBehavior(CoreBehaviors.ChecksRange)]
        int MaxLength { get; set; }

        /// <summary>
        /// Gets or sets the password value of the control.
        /// </summary>
        string Password { get; set; }

        /// <summary>
        /// Gets or sets the text to display when the control does not have a value.
        /// </summary>
        string Placeholder { get; set; }
    }
}
