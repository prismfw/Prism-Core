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
using Prism.UI;
using Prism.UI.Controls;

namespace Prism.Native
{
    /// <summary>
    /// Defines an audio and video player that is native to a particular platform.
    /// These objects are meant to be paired with platform-agnostic <see cref="MediaElement"/> objects.
    /// </summary>
    public interface INativeMediaElement : INativeElement
    {
        /// <summary>
        /// Occurs when playback of a media source has finished.
        /// </summary>
        event EventHandler MediaEnded;

        /// <summary>
        /// Occurs when a media source has failed to open.
        /// </summary>
        event EventHandler<ErrorEventArgs> MediaFailed;

        /// <summary>
        /// Occurs when a media source has been successfully opened.
        /// </summary>
        event EventHandler MediaOpened;

        /// <summary>
        /// Occurs when a seek operation has been completed.
        /// </summary>
        event EventHandler SeekCompleted;

        /// <summary>
        /// Gets or sets a value indicating whether to show the default playback controls (play, pause, etc).
        /// </summary>
        bool ArePlaybackControlsEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether playback of a media source should automatically begin once buffering is finished.
        /// </summary>
        bool AutoPlay { get; set; }

        /// <summary>
        /// Gets the amount that the current playback item has buffered as a value between 0.0 and 1.0.
        /// </summary>
        double BufferingProgress { get; }

        /// <summary>
        /// Gets the duration of the current playback item.
        /// </summary>
        TimeSpan Duration { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the current media source will automatically begin playback again once it has finished.
        /// </summary>
        bool IsLooping { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the media content is muted.
        /// </summary>
        bool IsMuted { get; set; }

        /// <summary>
        /// Gets a value indicating whether a playback item is currently playing.
        /// </summary>
        bool IsPlaying { get; }

        /// <summary>
        /// Gets or sets a coefficient of the rate at which media content is played back.  A value of 1.0 is a normal playback rate.
        /// </summary>
        [CoreBehavior(CoreBehaviors.ChecksRange)]
        double PlaybackRate { get; set; }

        /// <summary>
        /// Gets or sets the position of the playback item.
        /// </summary>
        [CoreBehavior(CoreBehaviors.ChecksRange)]
        TimeSpan Position { get; set; }

        /// <summary>
        /// Gets or sets the source of the media content to be played.
        /// </summary>
        [CoreBehavior(CoreBehaviors.ExpectsEarlyChangeNotification)]
        object Source { get; set; }

        /// <summary>
        /// Gets or sets the manner in which video content is stretched within its allocated space.
        /// </summary>
        Stretch Stretch { get; set; }

        /// <summary>
        /// Gets the size of the video content, or Size.Empty if there is no video content.
        /// </summary>
        Size VideoSize { get; }

        /// <summary>
        /// Gets or sets the volume of the media content as a range between 0.0 (silent) and 1.0 (full).
        /// </summary>
        [CoreBehavior(CoreBehaviors.ChecksRange)]
        double Volume { get; set; }

        /// <summary>
        /// Pauses playback of the current media source.
        /// </summary>
        void PausePlayback();

        /// <summary>
        /// Starts or resumes playback of the current media source.
        /// </summary>
        void StartPlayback();

        /// <summary>
        /// Stops playback of the current media source.
        /// </summary>
        void StopPlayback();
    }
}
