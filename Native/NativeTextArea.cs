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
using Prism.UI;
using Prism.UI.Controls;

namespace Prism.Native
{
    /// <summary>
    /// Defines a text entry control that is native to a particular platform.
    /// These objects are meant to be paired with platform-agnostic <see cref="TextArea"/> objects.
    /// </summary>
    public interface INativeTextArea : INativeControl
    {
        /// <summary>
        /// Occurs when the action key, most commonly mapped to the "Return" key, is pressed while the control has focus.
        /// </summary>
        event EventHandler<HandledEventArgs> ActionKeyPressed;

        /// <summary>
        /// Occurs when the value of the <see cref="P:Text"/> property has changed.
        /// </summary>
        event EventHandler<TextChangedEventArgs> TextChanged;

        /// <summary>
        /// Gets or sets the type of action key to use for the soft keyboard when the control has focus.
        /// </summary>
        ActionKeyType ActionKeyType { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of lines of text that should be shown.
        /// </summary>
        [CoreBehavior(CoreBehaviors.ChecksRange)]
        int MaxLines { get; set; }

        /// <summary>
        /// Gets or sets the minimum number of lines of text that should be shown.
        /// </summary>
        [CoreBehavior(CoreBehaviors.ChecksRange)]
        int MinLines { get; set; }

        /// <summary>
        /// Gets or sets the text to display when the control does not have a value.
        /// </summary>
        string Placeholder { get; set; }

        /// <summary>
        /// Gets or sets the text value of the control.
        /// </summary>
        string Text { get; set; }

        /// <summary>
        /// Gets or sets the manner in which the text is aligned within the control.
        /// </summary>
        TextAlignment TextAlignment { get; set; }
    }
}
