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

namespace Prism.Media
{
    /// <summary>
    /// Provides data for the <see cref="E:Opened"/> events of media playback devices.
    /// </summary>
    public class MediaPlaybackItemOpenedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the playback item that was opened.
        /// </summary>
        public MediaPlaybackItem Item { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaPlaybackItemOpenedEventArgs"/> class.
        /// </summary>
        /// <param name="item">The playback item that was opened.</param>
        public MediaPlaybackItemOpenedEventArgs(MediaPlaybackItem item)
        {
            Item = item;
        }
    }
}