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
using Prism.UI.Media;

namespace Prism.Native
{
    /// <summary>
    /// Defines an interactive UI element that is native to a particular platform.
    /// These objects are meant to be paired with platform-agnostic <see cref="Control"/> objects.
    /// </summary>
    public interface INativeControl : INativeElement
    {
        /// <summary>
        /// Occurs when the control receives focus.
        /// </summary>
        event EventHandler GotFocus;

        /// <summary>
        /// Occurs when the control loses focus.
        /// </summary>
        event EventHandler LostFocus;

        /// <summary>
        /// Gets or sets the background for the control.
        /// </summary>
        Brush Background { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Brush"/> to apply to the border of the control.
        /// </summary>
        Brush BorderBrush { get; set; }

        /// <summary>
        /// Gets or sets the width of the border around the control.
        /// </summary>
        double BorderWidth { get; set; }

        /// <summary>
        /// Gets or sets the font to use for displaying the text in the control.
        /// </summary>
        [CoreBehavior(CoreBehaviors.ChecksNullity)]
        object FontFamily { get; set; }

        /// <summary>
        /// Gets or sets the size of the text in the control.
        /// </summary>
        [CoreBehavior(CoreBehaviors.ChecksRange)]
        double FontSize { get; set; }

        /// <summary>
        /// Gets or sets the style with which to render the text in the control.
        /// </summary>
        FontStyle FontStyle { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Brush"/> to apply to the foreground content of the control.
        /// </summary>
        Brush Foreground { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user can interact with the control.
        /// </summary>
        bool IsEnabled { get; set; }

        /// <summary>
        /// Gets a value indicating whether the control has focus.
        /// </summary>
        bool IsFocused { get; }

        /// <summary>
        /// Attempts to set focus to the control.
        /// </summary>
        void Focus();

        /// <summary>
        /// Attempts to remove focus from the control.
        /// </summary>
        void Unfocus();
    }
}
