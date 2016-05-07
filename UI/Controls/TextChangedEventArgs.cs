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

namespace Prism.UI.Controls
{
    /// <summary>
    /// Provides data for TextChanged events.
    /// </summary>
    public class TextChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the value of the text after the change was made.
        /// </summary>
        public string NewText { get; }

        /// <summary>
        /// Gets the value of the text before the change was made.
        /// </summary>
        public string OldText { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextChangedEventArgs"/> class.
        /// </summary>
        /// <param name="oldText">The value of the text before the change was made.</param>
        /// <param name="newText">The value of the text after the change was made.</param>
        public TextChangedEventArgs(string oldText, string newText)
        {
            NewText = newText;
            OldText = oldText;
        }
    }
}
