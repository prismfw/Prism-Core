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
using System.Globalization;
using Prism.Native;

#if !DEBUG
using System.Diagnostics;
#endif

namespace Prism.Media
{
    /// <summary>
    /// Represents an item that is to be played by an audio or video player.
    /// </summary>
    public class MediaPlaybackItem : IMediaPlaybackSource
    {
        /// <summary>
        /// Gets the duration of the playback item.
        /// </summary>
        public TimeSpan Duration
        {
            get { return nativeObject.Duration; }
        }

        /// <summary>
        /// Gets a value indicating whether the playback item has been successfully opened.
        /// </summary>
        public bool IsOpen { get; internal set; }

        /// <summary>
        /// Gets a collection of the individual media tracks that contain the playback data.
        /// </summary>
        public ReadOnlyCollection<MediaTrack> Tracks
        {
            get { return nativeObject.Tracks; }
        }

        /// <summary>
        /// Gets the URI of the playback item.
        /// </summary>
        public Uri Uri
        {
            get { return nativeObject.Uri; }
        }

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private readonly INativeMediaPlaybackItem nativeObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaPlaybackItem"/> class.
        /// </summary>
        /// <param name="uri">The URI of the playback item.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="uri"/> is <c>null</c>.</exception>
        public MediaPlaybackItem(Uri uri)
        {
            if (uri == null)
            {
                throw new ArgumentNullException(nameof(uri));
            }

            uri = IO.Directory.ValidateUri(uri);

            nativeObject = TypeManager.Default.Resolve<INativeMediaPlaybackItem>(new object[] { uri },
                TypeResolutionOptions.UseFuzzyNameResolution | TypeResolutionOptions.UseFuzzyParameterResolution);

            if (nativeObject == null)
            {
                throw new TypeResolutionException(string.Format(CultureInfo.CurrentCulture, Resources.Strings.TypeCouldNotBeResolved, typeof(INativeMediaPlaybackItem).FullName));
            }

            ObjectRetriever.SetPair(this, nativeObject);
        }

        /// <summary>
        /// Releases any resources that are being used by the playback item.
        /// </summary>
        public void Dispose()
        {
            nativeObject.Dispose();
        }
    }
}
