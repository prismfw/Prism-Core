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


namespace Prism.Media
{
    /// <summary>
    /// Represents an individual track in a playback item that contains audio, video, or other types of data.
    /// </summary>
    public class MediaTrack
    {
        /// <summary>
        /// Gets the language code for the media track.
        /// </summary>
        public string Language { get; }

        /// <summary>
        /// Gets the name of the media track.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the type of the media track.
        /// </summary>
        public MediaTrackType TrackType { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaTrack"/> class.
        /// </summary>
        /// <param name="name">The name of the media track.</param>
        /// <param name="trackType">The type of the media track.</param>
        /// <param name="language">The language code for the media track.</param>
        public MediaTrack(string name, MediaTrackType trackType, string language)
        {
            Name = name;
            Language = language;
            TrackType = trackType;
        }
    }
}
