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
using System.Collections.ObjectModel;
using Prism.Media;

namespace Prism.Native
{
    /// <summary>
    /// Defines a media playback item that is native to a particular platform.
    /// These objects are meant to be paired with platform-agnostic <see cref="MediaPlaybackItem"/> objects.
    /// </summary>
    public interface INativeMediaPlaybackItem
    {
        /// <summary>
        /// Gets the duration of the playback item.
        /// </summary>
        TimeSpan Duration { get; }

        /// <summary>
        /// Gets a collection of the individual media tracks that contain the playback data.
        /// </summary>
        ReadOnlyCollection<MediaTrack> Tracks { get; }

        /// <summary>
        /// Gets the URI of the playback item.
        /// </summary>
        Uri Uri { get; }

        /// <summary>
        /// Releases any resources that are being used by the playback item.
        /// </summary>
        void Dispose();
    }
}
