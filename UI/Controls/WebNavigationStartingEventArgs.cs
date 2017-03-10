/*
Copyright (C) 2017  Prism Framework Team

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
    /// Provides data for the <see cref="E:NavigationStarting"/> event.
    /// </summary>
    public class WebNavigationStartingEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets a value indicating whether the navigation should be canceled.
        /// </summary>
        public bool Cancel { get; set; }

        /// <summary>
        /// Gets the URI that is being navigated to.
        /// </summary>
        public Uri Uri { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebNavigationStartingEventArgs"/> class.
        /// </summary>
        /// <param name="uri">The URI that is being navigated to.</param>
        public WebNavigationStartingEventArgs(Uri uri)
        {
            Uri = uri;
        }
    }
}
