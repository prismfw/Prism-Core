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
using Prism.Media;

namespace Prism.Native
{
    /// <summary>
    /// Defines a lightweight audio player that is native to a particular platform.
    /// These objects are meant to be paired with platform-agnostic <see cref="AudioPlayer"/> objects.
    /// </summary>
    public interface INativeAudioPlayer
    {
        /// <summary>
        /// Occurs when there is an error during loading or playing of the audio track.
        /// </summary>
        event EventHandler<ErrorEventArgs> AudioFailed;

        /// <summary>
        /// Occurs when buffering of the audio track has finished.
        /// </summary>
        event EventHandler BufferingEnded;

        /// <summary>
        /// Occurs when buffering of the audio track has begun.
        /// </summary>
        event EventHandler BufferingStarted;

        /// <summary>
        /// Occurs when playback of the audio track has finished.
        /// </summary>
        event EventHandler PlaybackEnded;

        /// <summary>
        /// Occurs when playback of the audio track has begun.
        /// </summary>
        event EventHandler PlaybackStarted;

        /// <summary>
        /// Gets or sets a value indicating whether playback of the audio track should automatically begin once buffering is finished.
        /// </summary>
        bool AutoPlay { get; set; }

        /// <summary>
        /// Gets the duration of the audio track.
        /// </summary>
        TimeSpan Duration { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the audio track will automatically begin playing again once it has finished.
        /// </summary>
        bool IsLooping { get; set; }

        /// <summary>
        /// Gets a value indicating whether the audio track is currently playing.
        /// </summary>
        bool IsPlaying { get; }

        /// <summary>
        /// Gets or sets a coefficient of the rate at which the audio track is played back.
        /// </summary>
        [CoreBehavior(CoreBehaviors.ChecksRange)]
        double PlaybackRate { get; set; }

        /// <summary>
        /// Gets or sets the position of the audio track.
        /// </summary>
        TimeSpan Position { get; set; }

        /// <summary>
        /// Gets or sets the volume of the audio track as a range between 0.0 (silent) and 1.0 (full).
        /// </summary>
        [CoreBehavior(CoreBehaviors.ChecksRange)]
        double Volume { get; set; }

        /// <summary>
        /// Loads the audio track from the file at the specified location.
        /// </summary>
        /// <param name="source">The URI of the source file for the audio track.</param>
        void Open([CoreBehavior(CoreBehaviors.ChecksNullity)]Uri source);

        /// <summary>
        /// Pauses playback of the audio track.
        /// </summary>
        void Pause();

        /// <summary>
        /// Starts or resumes playback of the audio track.
        /// </summary>
        void Play();
    }
}
