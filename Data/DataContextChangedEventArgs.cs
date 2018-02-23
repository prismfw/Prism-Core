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

namespace Prism.Data
{
    /// <summary>
    /// Provides data for the <see cref="E:DataContextChanged"/> event.
    /// </summary>
    public class DataContextChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the data context after the change.
        /// </summary>
        public object NewContext { get; }

        /// <summary>
        /// Gets the data context before the change.
        /// </summary>
        public object OldContext { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataContextChangedEventArgs"/> class.
        /// </summary>
        /// <param name="oldContext">The data context before the change.</param>
        /// <param name="newContext">The data context after the change.</param>
        public DataContextChangedEventArgs(object oldContext, object newContext)
        {
            NewContext = newContext;
            OldContext = oldContext;
        }
    }
}
