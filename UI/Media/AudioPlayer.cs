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
using System.Globalization;
using Prism.Native;

#if !DEBUG
using System.Diagnostics;
#endif

namespace Prism.UI.Media
{
    /// <summary>
    /// Represents a media player for audio tracks.
    /// </summary>
    public sealed class AudioPlayer : FrameworkObject
    {
        #region Event Descriptors
        /// <summary>
        /// Gets an <see cref="EventDescriptor"/> describing the <see cref="E:AudioFailed"/> event.
        /// </summary>
        public static EventDescriptor AudioFailedEvent { get; } = EventDescriptor.Create(nameof(AudioFailed), typeof(TypedEventHandler<AudioPlayer, ErrorEventArgs>), typeof(AudioPlayer));

        /// <summary>
        /// Gets an <see cref="EventDescriptor"/> describing the <see cref="E:BufferingCompleted"/> event.
        /// </summary>
        public static EventDescriptor BufferingCompletedEvent { get; } = EventDescriptor.Create(nameof(BufferingCompleted), typeof(TypedEventHandler<AudioPlayer>), typeof(AudioPlayer));

        /// <summary>
        /// Gets an <see cref="EventDescriptor"/> describing the <see cref="E:BufferingStarted"/> event.
        /// </summary>
        public static EventDescriptor BufferingStartedEvent { get; } = EventDescriptor.Create(nameof(BufferingStarted), typeof(TypedEventHandler<AudioPlayer>), typeof(AudioPlayer));

        /// <summary>
        /// Gets an <see cref="EventDescriptor"/> describing the <see cref="E:PlaybackCompleted"/> event.
        /// </summary>
        public static EventDescriptor PlaybackCompletedEvent { get; } = EventDescriptor.Create(nameof(PlaybackCompleted), typeof(TypedEventHandler<AudioPlayer>), typeof(AudioPlayer));

        /// <summary>
        /// Gets an <see cref="EventDescriptor"/> describing the <see cref="E:PlaybackStarted"/> event.
        /// </summary>
        public static EventDescriptor PlaybackStartedEvent { get; } = EventDescriptor.Create(nameof(PlaybackStarted), typeof(TypedEventHandler<AudioPlayer>), typeof(AudioPlayer));
        #endregion

        /// <summary>
        /// Gets the current audio player.
        /// </summary>
        public static AudioPlayer Current { get; } = new AudioPlayer(typeof(INativeAudioPlayer), null);

        /// <summary>
        /// Occurs when there is an error during loading or playing of the audio track.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification = "Event handler provides a strongly-typed sender for easier use.")]
        public event TypedEventHandler<AudioPlayer, ErrorEventArgs> AudioFailed;

        /// <summary>
        /// Occurs when buffering of the audio track has finished.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification = "Event handler provides a strongly-typed sender for easier use.")]
        public event TypedEventHandler<AudioPlayer> BufferingCompleted;

        /// <summary>
        /// Occurs when buffering of the audio track has begun.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification = "Event handler provides a strongly-typed sender for easier use.")]
        public event TypedEventHandler<AudioPlayer> BufferingStarted;

        /// <summary>
        /// Occurs when playback of the audio track has finished.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification = "Event handler provides a strongly-typed sender for easier use.")]
        public event TypedEventHandler<AudioPlayer> PlaybackCompleted;

        /// <summary>
        /// Occurs when playback of the audio track has begun.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification = "Event handler provides a strongly-typed sender for easier use.")]
        public event TypedEventHandler<AudioPlayer> PlaybackStarted;

        /// <summary>
        /// Gets or sets a value indicating whether playback of the audio track should automatically begin once buffering is finished.
        /// </summary>
        public bool AutoPlay
        {
            get { return nativeObject.AutoPlay; }
            set { nativeObject.AutoPlay = value; }
        }

        /// <summary>
        /// Gets the duration of the audio track.
        /// </summary>
        public TimeSpan Duration
        {
            get { return nativeObject.Duration; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the audio track will automatically begin playing again once it has finished.
        /// </summary>
        public bool IsLooping
        {
            get { return nativeObject.IsLooping; }
            set { nativeObject.IsLooping = value; }
        }

        /// <summary>
        /// Gets a value indicating whether the audio track is currently playing.
        /// </summary>
        public bool IsPlaying
        {
            get { return nativeObject.IsPlaying; }
        }

        /// <summary>
        /// Gets or sets a coefficient of the rate at which the audio track is played back.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", Justification = "Exception parameter refers to property name for easier understanding of invalid value.")]
        public double PlaybackRate
        {
            get { return nativeObject.PlaybackRate; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(PlaybackRate), Resources.Strings.ValueCannotBeLessThanZero);
                }

                nativeObject.PlaybackRate = value;
            }
        }

        /// <summary>
        /// Gets or sets the position of the audio track.
        /// </summary>
        public TimeSpan Position
        {
            get { return nativeObject.Position; }
            set { nativeObject.Position = value; }
        }

        /// <summary>
        /// Gets the URI of the source file that is currently loading or playing.
        /// </summary>
        public Uri Source { get; private set; }

        /// <summary>
        /// Gets or sets the volume of the audio track as a range between 0.0 (silent) and 1.0 (full).
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", Justification = "Exception parameter refers to property name for easier understanding of invalid value.")]
        public double Volume
        {
            get { return nativeObject.Volume; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(Volume), Resources.Strings.ValueCannotBeLessThanZero);
                }

                nativeObject.Volume = Math.Min(value, 1);
            }
        }

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private readonly INativeAudioPlayer nativeObject;

        private AudioPlayer(Type resolveType, string resolveName, params ResolveParameter[] resolveParams)
            : base(resolveType, resolveName, resolveParams)
        {
            nativeObject = ObjectRetriever.GetNativeObject(this) as INativeAudioPlayer;
            if (nativeObject == null)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.Strings.TypeMustResolveToType, resolveType.FullName, typeof(INativeAudioPlayer).FullName), nameof(resolveType));
            }

            nativeObject.AudioFailed += (o, e) => OnAudioFailed(e);
            nativeObject.BufferingCompleted += (o, e) => OnBufferingCompleted(e);
            nativeObject.BufferingStarted += (o, e) => OnBufferingStarted(e);
            nativeObject.PlaybackCompleted += (o, e) => OnPlaybackCompleted(e);
            nativeObject.PlaybackStarted += (o, e) => OnPlaybackStarted(e);

            AutoPlay = true;
            IsLooping = false;
            PlaybackRate = 1;
            Volume = 1;
        }

        /// <summary>
        /// Loads the audio track from the file at the specified location.
        /// </summary>
        /// <param name="source">The URI of the source file for the audio track.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <c>null</c>.</exception>
        public void Open(Uri source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            Source = source;
            nativeObject.Open(source);
        }

        /// <summary>
        /// Pauses playback of the audio track.
        /// </summary>
        public void Pause()
        {
            nativeObject.Pause();
        }

        /// <summary>
        /// Starts or resumes playback of the audio track.
        /// </summary>
        public void Play()
        {
            nativeObject.Play();
        }

        private void OnAudioFailed(ErrorEventArgs e)
        {
            AudioFailed?.Invoke(this, e);
        }

        private void OnBufferingCompleted(EventArgs e)
        {
            BufferingCompleted?.Invoke(this, e);
        }

        private void OnBufferingStarted(EventArgs e)
        {
            BufferingStarted?.Invoke(this, e);
        }

        private void OnPlaybackCompleted(EventArgs e)
        {
            PlaybackCompleted?.Invoke(this, e);
        }

        private void OnPlaybackStarted(EventArgs e)
        {
            PlaybackStarted?.Invoke(this, e);
        }
    }
}
