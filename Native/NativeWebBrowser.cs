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

namespace Prism.Native
{
    /// <summary>
    /// Defines a web browser that is native to a particular platform.
    /// These objects are meant to be paired with platform-agnostic <see cref="WebBrowser"/> objects.
    /// </summary>
    public interface INativeWebBrowser : INativeElement
    {
        /// <summary>
        /// Occurs when the web browser has finished loading the document.
        /// </summary>
        event EventHandler<WebNavigationCompletedEventArgs> NavigationCompleted;

        /// <summary>
        /// Occurs when the web browser has begun navigating to a document.
        /// </summary>
        event EventHandler<WebNavigationStartingEventArgs> NavigationStarting;

        /// <summary>
        /// Occurs when a script invocation has completed.
        /// </summary>
        event EventHandler<WebScriptCompletedEventArgs> ScriptCompleted;

        /// <summary>
        /// Gets a value indicating whether the web browser has at least one document in its back navigation history.
        /// </summary>
        bool CanGoBack { get; }

        /// <summary>
        /// Gets a value indicating whether the web browser has at least one document in its forward navigation history.
        /// </summary>
        bool CanGoForward { get; }

        /// <summary>
        /// Gets the title of the current document.
        /// </summary>
        string Title { get; }

        /// <summary>
        /// Gets the URI of the current document.
        /// </summary>
        Uri Uri { get; }

        /// <summary>
        /// Navigates to the previous document in the navigation history.
        /// </summary>
        void GoBack();

        /// <summary>
        /// Navigates to the next document in the navigation history.
        /// </summary>
        void GoForward();

        /// <summary>
        /// Executes a script function that is implemented by the current document.
        /// </summary>
        /// <param name="scriptName">The name of the script function to execute.</param>
        void InvokeScript([CoreBehavior(CoreBehaviors.ChecksNullity)]string scriptName);

        /// <summary>
        /// Navigates to the specified <see cref="Uri"/>.
        /// </summary>
        /// <param name="uri">The URI to navigate to.</param>
        void Navigate([CoreBehavior(CoreBehaviors.ChecksNullity)]Uri uri);

        /// <summary>
        /// Navigates to the specified <see cref="string"/> containing the content for a document.
        /// </summary>
        /// <param name="html">The string containing the content for a document.</param>
        void NavigateToString([CoreBehavior(CoreBehaviors.ChecksNullity)]string html);

        /// <summary>
        /// Reloads the current document.
        /// </summary>
        void Refresh();
    }
}
