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

namespace Prism.Native
{
    /// <summary>
    /// Defines a window that is native to a particular platform.
    /// These objects are meant to be paired with platform-agnostic <see cref="Window"/> objects.
    /// </summary>
    public interface INativeWindow
    {
        /// <summary>
        /// Occurs when the window is brought to the foreground.
        /// </summary>
        event EventHandler Activated;

        /// <summary>
        /// Occurs when the window is about to be closed.
        /// This may not fire for the main window on certain platforms.
        /// </summary>
        event EventHandler<CancelEventArgs> Closing;

        /// <summary>
        /// Occurs when the window is pushed to the background.
        /// </summary>
        event EventHandler Deactivated;

        /// <summary>
        /// Occurs when the orientation of the rendered content has changed.
        /// </summary>
        event EventHandler<DisplayOrientationChangedEventArgs> OrientationChanged;

        /// <summary>
        /// Occurs when the size of the window has changed.
        /// </summary>
        event EventHandler<WindowSizeChangedEventArgs> SizeChanged;

        /// <summary>
        /// Gets or sets the preferred orientations in which to automatically rotate the window in response to orientation changes of the physical device.
        /// </summary>
        DisplayOrientations AutorotationPreferences { get; set; }

        /// <summary>
        /// Gets or sets the object that acts as the content of the window.
        /// </summary>
        object Content { get; set; }

        /// <summary>
        /// Gets the height of the window.
        /// </summary>
        double Height { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is currently visible.
        /// </summary>
        bool IsVisible { get; }

        /// <summary>
        /// Gets the current orientation of the rendered content within the window.
        /// </summary>
        DisplayOrientations Orientation { get; }

        /// <summary>
        /// Gets or sets the style for the window.
        /// </summary>
        WindowStyle Style { get; set; }

        /// <summary>
        /// Gets the width of the window.
        /// </summary>
        double Width { get; }

        /// <summary>
        /// Attempts to close the window.  If this is the main window, attempts to shut down the application.
        /// </summary>
        void Close();

        /// <summary>
        /// Sets the preferred minimum size of the window.
        /// </summary>
        /// <param name="minSize">The preferred minimum size.</param>
        void SetPreferredMinSize(Size minSize);

        /// <summary>
        /// Displays the window if it is not already visible.
        /// </summary>
        void Show();

        /// <summary>
        /// Attempts to resize the window to the specified size.
        /// </summary>
        /// <param name="newSize">The width and height at which to size the window.</param>
        /// <returns><c>true</c> if the window was successfully resized; otherwise, <c>false</c>.</returns>
        bool TryResize(Size newSize);
    }
}
