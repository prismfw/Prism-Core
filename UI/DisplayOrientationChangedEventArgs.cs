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

namespace Prism.UI
{
    /// <summary>
    /// Provides data for events that are triggered by a change in display orientation.
    /// </summary>
    public class DisplayOrientationChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the new orientation of the display.
        /// </summary>
        public DisplayOrientations NewOrientation { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DisplayOrientationChangedEventArgs"/> class.
        /// </summary>
        /// <param name="newOrientation">The new orientation of the display.</param>
        public DisplayOrientationChangedEventArgs(DisplayOrientations newOrientation)
        {
            NewOrientation = newOrientation;
        }
    }
}