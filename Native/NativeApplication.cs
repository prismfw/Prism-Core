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
    /// Defines an object for sending and receiving application-wide messages.
    /// These objects are meant to be paired with platform-agnostic <see cref="Application"/> objects.
    /// </summary>
    public interface INativeApplication
    {
        /// <summary>
        /// Occurs when the application is shutting down.
        /// </summary>
        event EventHandler Exiting;

        /// <summary>
        /// Occurs when the application is resuming from suspension.
        /// </summary>
        event EventHandler Resuming;

        /// <summary>
        /// Occurs when the application is suspending.
        /// </summary>
        event EventHandler Suspending;

        /// <summary>
        /// Occurs when an unhandled exception is encountered.
        /// </summary>
        event EventHandler<ErrorEventArgs> UnhandledException;

        /// <summary>
        /// Gets the default theme that is used by the application.
        /// </summary>
        Theme DefaultTheme { get; }

        /// <summary>
        /// Gets the platform on which the application is running.
        /// </summary>
        Platform Platform { get; }

        /// <summary>
        /// Signals the system to begin ignoring any user interactions within the application.
        /// </summary>
        void BeginIgnoringUserInput();

        /// <summary>
        /// Asynchronously invokes the specified delegate on the platform's main thread.
        /// </summary>
        /// <param name="action">The action to invoke on the main thread.</param>
        void BeginInvokeOnMainThread(Action action);

        /// <summary>
        /// Asynchronously invokes the specified delegate on the platform's main thread.
        /// </summary>
        /// <param name="del">A delegate to a method that takes multiple parameters.</param>
        /// <param name="parameters">The parameters for the delegate method.</param>
        void BeginInvokeOnMainThreadWithParameters(Delegate del, params object[] parameters);

        /// <summary>
        /// Signals the system to stop ignoring user interactions within the application.
        /// </summary>
        void EndIgnoringUserInput();

        /// <summary>
        /// Launches the specified URL in an external application, most commonly a web browser.
        /// </summary>
        /// <param name="url">The URL to launch to.</param>
        void LaunchUrl(Uri url);
    }
}
