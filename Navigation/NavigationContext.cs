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
using System.Diagnostics.CodeAnalysis;

namespace Prism
{
    /// <summary>
    /// Represents the context in which a navigation has occurred.
    /// </summary>
    public sealed class NavigationContext
    {
        /// <summary>
        /// Gets the date and time when the navigation occurred.
        /// </summary>
        public DateTime NavigatedDateTime { get; }

        /// <summary>
        /// Gets the URI that was navigated to.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings", Justification = "Value may contain formatting that is nonstandard to System.Uri.")]
        public string NavigatedUri { get; }

        /// <summary>
        /// Gets the pane or panes from which the navigation originated.
        /// This may contain multiple values if multiple navigations have been started since the last time a view has been presented.
        /// </summary>
        public Panes OriginatingPanes { get; internal set; }

        /// <summary>
        /// Gets the parameters that are a part of the navigation.
        /// </summary>
        public NavigationParameterDictionary Parameters { get; }

        internal NavigationContext(string navigatedUri)
        {
            NavigatedUri = navigatedUri;
            NavigatedDateTime = DateTime.Now;
            OriginatingPanes = Panes.Unknown;
            Parameters = new NavigationParameterDictionary();
        }

        internal NavigationContext(NavigationContext context)
        {
            NavigatedUri = context.NavigatedUri;
            NavigatedDateTime = context.NavigatedDateTime;
            OriginatingPanes = context.OriginatingPanes;
            Parameters = new NavigationParameterDictionary();
            foreach (var key in context.Parameters.Keys)
            {
                Parameters[key] = context.Parameters[key];
            }
        }
    }
}
