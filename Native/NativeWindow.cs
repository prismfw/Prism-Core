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
using System.Threading.Tasks;
using Prism.UI;
using Prism.UI.Media.Imaging;

namespace Prism.Native
{
    /// <summary>
    /// Defines an area for renderable content that is native to a particular platform.
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
        /// Occurs when the size of the window has changed.
        /// </summary>
        event EventHandler<WindowSizeChangedEventArgs> SizeChanged;

        /// <summary>
        /// Gets or sets the object that acts as the content of the window.
        /// </summary>
        object Content { get; set; }

        /// <summary>
        /// Gets (or sets, but see Remarks) the height of the window.
        /// </summary>
        /// <remarks>
        /// Setting the height of a window is generally only supported on desktop environments.
        /// Most platforms will ignore any attempts to set the height explicitly.
        /// </remarks>
        double Height { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is currently visible.
        /// </summary>
        bool IsVisible { get; }

        /// <summary>
        /// Gets (or sets, but see Remarks) the width of the window.
        /// </summary>
        /// <remarks>
        /// Setting the width of a window is generally only supported on desktop environments.
        /// Most platforms will ignore any attempts to set the width explicitly.
        /// </remarks>
        double Width { get; set; }

        /// <summary>
        /// Attempts to close the window.  If this is the main window, attempts to shut down the application.
        /// </summary>
        /// <param name="animate">Whether to use any system-defined transition animation.</param>
        void Close(Animate animate);

        /// <summary>
        /// Displays the window if it is not already visible.
        /// </summary>
        /// <param name="animate">Whether to use any system-defined transition animation.</param>
        void Show(Animate animate);

        /// <summary>
        /// Captures the contents of the window in an image and returns the result.
        /// </summary>
        /// <returns>The captured image as an <see cref="ImageSource"/> instance.</returns>
        Task<ImageSource> TakeScreenshotAsync();
    }
}
