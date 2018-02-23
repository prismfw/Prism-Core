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
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Prism.Media;
using Prism.Native;
using Prism.Resources;

#if !DEBUG
using System.Diagnostics;
#endif

namespace Prism.UI.Controls
{
    /// <summary>
    /// Represents a UI element that plays audio and video content.
    /// </summary>
    [Resolve(typeof(INativeMediaElement))]
    public class MediaElement : Element
    {
        #region Event Descriptors
        /// <summary>
        /// Gets an <see cref="EventDescriptor"/> describing the <see cref="E:MediaEnded"/> event.
        /// </summary>
        public static EventDescriptor MediaEndedEvent { get; } = EventDescriptor.Create(nameof(MediaEnded), typeof(TypedEventHandler<MediaElement>), typeof(MediaElement));

        /// <summary>
        /// Gets an <see cref="EventDescriptor"/> describing the <see cref="E:MediaFailed"/> event.
        /// </summary>
        public static EventDescriptor MediaFailedEvent { get; } = EventDescriptor.Create(nameof(MediaFailed), typeof(TypedEventHandler<MediaElement, ErrorEventArgs>), typeof(MediaElement));

        /// <summary>
        /// Gets an <see cref="EventDescriptor"/> describing the <see cref="E:MediaOpened"/> event.
        /// </summary>
        public static EventDescriptor MediaOpenedEvent { get; } = EventDescriptor.Create(nameof(MediaOpened), typeof(TypedEventHandler<MediaElement>), typeof(MediaElement));

        /// <summary>
        /// Gets an <see cref="EventDescriptor"/> describing the <see cref="E:SeekCompleted"/> event.
        /// </summary>
        public static EventDescriptor SeekCompletedEvent { get; } = EventDescriptor.Create(nameof(SeekCompleted), typeof(TypedEventHandler<MediaElement>), typeof(MediaElement));
        #endregion

        #region Property Descriptors
        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:ArePlaybackControlsEnabled"/> property.
        /// </summary>
        public static PropertyDescriptor ArePlaybackControlsEnabledProperty { get; } = PropertyDescriptor.Create(nameof(ArePlaybackControlsEnabled), typeof(bool), typeof(MediaElement));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:AutoPlay"/> property.
        /// </summary>
        public static PropertyDescriptor AutoPlayProperty { get; } = PropertyDescriptor.Create(nameof(AutoPlay), typeof(bool), typeof(MediaElement));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:BufferingProgress"/> property.
        /// </summary>
        public static PropertyDescriptor BufferingProgressProperty { get; } = PropertyDescriptor.Create(nameof(BufferingProgress), typeof(double), typeof(MediaElement), true);

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:Duration"/> property.
        /// </summary>
        public static PropertyDescriptor DurationProperty { get; } = PropertyDescriptor.Create(nameof(Duration), typeof(TimeSpan), typeof(MediaElement), true);

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:IsLooping"/> property.
        /// </summary>
        public static PropertyDescriptor IsLoopingProperty { get; } = PropertyDescriptor.Create(nameof(IsLooping), typeof(bool), typeof(MediaElement));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:IsMuted"/> property.
        /// </summary>
        public static PropertyDescriptor IsMutedProperty { get; } = PropertyDescriptor.Create(nameof(IsMuted), typeof(bool), typeof(MediaElement));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:IsPlaying"/> property.
        /// </summary>
        public static PropertyDescriptor IsPlayingProperty { get; } = PropertyDescriptor.Create(nameof(IsPlaying), typeof(bool), typeof(MediaElement), true);

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:PlaybackRate"/> property.
        /// </summary>
        public static PropertyDescriptor PlaybackRateProperty { get; } = PropertyDescriptor.Create(nameof(PlaybackRate), typeof(double), typeof(MediaElement));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:Source"/> property.
        /// </summary>
        public static PropertyDescriptor SourceProperty { get; } = PropertyDescriptor.Create(nameof(Source), typeof(IMediaPlaybackSource), typeof(MediaElement));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:Stretch"/> property.
        /// </summary>
        public static PropertyDescriptor StretchProperty { get; } = PropertyDescriptor.Create(nameof(Stretch), typeof(Stretch), typeof(MediaElement));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:VideoSize"/> property.
        /// </summary>
        public static PropertyDescriptor VideoSizeProperty { get; } = PropertyDescriptor.Create(nameof(VideoSize), typeof(Size), typeof(MediaElement));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:Volume"/> property.
        /// </summary>
        public static PropertyDescriptor VolumeProperty { get; } = PropertyDescriptor.Create(nameof(Volume), typeof(double), typeof(MediaElement));
        #endregion

        /// <summary>
        /// Occurs when playback of a media source has finished.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification = "Event handler provides a strongly-typed sender for easier use.")]
        public event TypedEventHandler<MediaElement> MediaEnded;

        /// <summary>
        /// Occurs when a media source has failed to open.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification = "Event handler provides a strongly-typed sender for easier use.")]
        public event TypedEventHandler<MediaElement, ErrorEventArgs> MediaFailed;

        /// <summary>
        /// Occurs when a media source has been successfully opened.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification = "Event handler provides a strongly-typed sender for easier use.")]
        public event TypedEventHandler<MediaElement> MediaOpened;

        /// <summary>
        /// Occurs when a seek operation has been completed.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification = "Event handler provides a strongly-typed sender for easier use.")]
        public event TypedEventHandler<MediaElement> SeekCompleted;

        /// <summary>
        /// Gets or sets a value indicating whether to show the default playback controls (play, pause, etc).
        /// </summary>
        public bool ArePlaybackControlsEnabled
        {
            get { return nativeObject.ArePlaybackControlsEnabled; }
            set { nativeObject.ArePlaybackControlsEnabled = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether playback of a media source should automatically begin once buffering is finished.
        /// </summary>
        public bool AutoPlay
        {
            get { return nativeObject.AutoPlay; }
            set { nativeObject.AutoPlay = value; }
        }

        /// <summary>
        /// Gets the amount that the current playback item has buffered as a value between 0.0 and 1.0.
        /// </summary>
        public double BufferingProgress
        {
            get { return nativeObject.BufferingProgress; }
        }

        /// <summary>
        /// Gets the duration of the current playback item.
        /// </summary>
        public TimeSpan Duration
        {
            get { return nativeObject.Duration; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the current media source will automatically begin playback again once it has finished.
        /// </summary>
        public bool IsLooping
        {
            get { return nativeObject.IsLooping; }
            set { nativeObject.IsLooping = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the media content is muted.
        /// </summary>
        public bool IsMuted
        {
            get { return nativeObject.IsMuted; }
            set { nativeObject.IsMuted = value; }
        }

        /// <summary>
        /// Gets a value indicating whether a playback item is currently playing.
        /// </summary>
        public bool IsPlaying
        {
            get { return nativeObject.IsPlaying; }
        }

        /// <summary>
        /// Gets or sets a coefficient of the rate at which media content is played back.  A value of 1.0 is a normal playback rate.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", Justification = "Exception parameter refers to property name for easier understanding of invalid value.")]
        public double PlaybackRate
        {
            get { return nativeObject.PlaybackRate; }
            set
            {
                if (double.IsNaN(value) || double.IsInfinity(value))
                {
                    throw new ArgumentException(Strings.ValueCannotBeNaNOrInfinity, nameof(PlaybackRate));
                }

                nativeObject.PlaybackRate = value;
            }
        }

        /// <summary>
        /// Gets or sets the position of the playback item.
        /// </summary>
        public TimeSpan Position
        {
            get { return nativeObject.Position; }
            set
            {
                if (value.Ticks < 0)
                {
                    value = TimeSpan.Zero;
                }
                else if (value.Ticks > Duration.Ticks)
                {
                    value = Duration;
                }

                nativeObject.Position = value;
            }
        }

        /// <summary>
        /// Gets or sets the source of the media content to be played.
        /// </summary>
        public IMediaPlaybackSource Source
        {
            get { return (IMediaPlaybackSource)ObjectRetriever.GetAgnosticObject(nativeObject.Source); }
            set { nativeObject.Source = ObjectRetriever.GetNativeObject(value); }
        }

        /// <summary>
        /// Gets or sets the manner in which video content is stretched within its allocated space.
        /// </summary>
        public Stretch Stretch
        {
            get { return nativeObject.Stretch; }
            set { nativeObject.Stretch = value; }
        }

        /// <summary>
        /// Gets the size of the video content, or <see cref="Size.Empty"/> if there is no video content.
        /// </summary>
        public Size VideoSize
        {
            get { return nativeObject.VideoSize; }
        }

        /// <summary>
        /// Gets or sets the volume of the media content as a range between 0.0 (silent) and 1.0 (full).
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", Justification = "Exception parameter refers to property name for easier understanding of invalid value.")]
        public double Volume
        {
            get { return nativeObject.Volume; }
            set
            {
                if (double.IsNaN(value) || double.IsInfinity(value))
                {
                    throw new ArgumentException(Strings.ValueCannotBeNaNOrInfinity, nameof(Volume));
                }

                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(Volume), Strings.ValueCannotBeLessThanZero);
                }

                nativeObject.Volume = Math.Min(value, 1);
            }
        }

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        // this field is to avoid casting
        private readonly INativeMediaElement nativeObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaElement"/> class.
        /// </summary>
        public MediaElement()
            : this(ResolveParameter.EmptyParameters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaElement"/> class and pairs it with the specified native object.
        /// </summary>
        /// <param name="nativeObject">The native object with which to pair this instance.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="nativeObject"/> doesn't match the type specified by the topmost <see cref="ResolveAttribute"/> in the inheritance chain.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="nativeObject"/> is <c>null</c>.</exception>
        protected MediaElement(INativeMediaElement nativeObject)
            : base(nativeObject)
        {
            this.nativeObject = nativeObject;

            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaElement"/> class and pairs it with a native object that is resolved from the IoC container.
        /// </summary>
        /// <param name="resolveParameters">Any parameters to pass along to the constructor of the native type.</param>
        /// <exception cref="TypeResolutionException">Thrown when the native object does not resolve to an <see cref="INativeMediaElement"/> instance.</exception>
        protected MediaElement(ResolveParameter[] resolveParameters)
            : base(resolveParameters)
        {
            nativeObject = ObjectRetriever.GetNativeObject(this) as INativeMediaElement;
            if (nativeObject == null)
            {
                throw new TypeResolutionException(string.Format(CultureInfo.CurrentCulture, Strings.TypeMustResolveToType,
                    ObjectRetriever.GetNativeObject(this).GetType().FullName, typeof(INativeMediaElement).FullName));
            }

            Initialize();
        }

        /// <summary>
        /// Pauses playback of the current media source.
        /// </summary>
        public void Pause()
        {
            if (nativeObject.IsPlaying)
            {
                nativeObject.PausePlayback();
            }
        }

        /// <summary>
        /// Starts or resumes playback of the current media source.
        /// </summary>
        public void Play()
        {
            if (!nativeObject.IsPlaying)
            {
                nativeObject.StartPlayback();
            }
        }

        /// <summary>
        /// Stops playback of the current media source.
        /// </summary>
        public void Stop()
        {
            if (nativeObject.IsPlaying)
            {
                nativeObject.StopPlayback();
            }
        }

        /// <summary>
        /// Called when the playback of a media source has finished and raises the <see cref="MediaEnded"/> event.
        /// </summary>
        /// <param name="e">The event arguments for the event.</param>
        protected virtual void OnMediaEnded(EventArgs e)
        {
            MediaEnded?.Invoke(this, e);
        }

        /// <summary>
        /// Called when a media source has failed to open and raises the <see cref="MediaFailed"/> event.
        /// </summary>
        /// <param name="e">The event arguments for the event.</param>
        protected virtual void OnMediaFailed(ErrorEventArgs e)
        {
            MediaFailed?.Invoke(this, e);
        }

        /// <summary>
        /// Called when a media source has been successfully opened and raises the <see cref="MediaOpened"/> event.
        /// </summary>
        /// <param name="e">The event arguments for the event.</param>
        protected virtual void OnMediaOpened(EventArgs e)
        {
            MediaOpened?.Invoke(this, e);
        }

        /// <summary>
        /// Called when a seek operation has completed and raises the <see cref="SeekCompleted"/> event.
        /// </summary>
        /// <param name="e">The event arguments for the event.</param>
        protected virtual void OnSeekCompleted(EventArgs e)
        {
            SeekCompleted?.Invoke(this, e);
        }

        private void Initialize()
        {
            nativeObject.SeekCompleted += (o, e) => OnSeekCompleted(e);
            nativeObject.MediaEnded += (o, e) => OnMediaEnded(e);
            nativeObject.MediaFailed += (o, e) => OnMediaFailed(e);
            nativeObject.MediaOpened += (o, e) =>
            {
                var item = Source as MediaPlaybackItem;
                if (item != null)
                {
                    item.IsOpen = true;
                }

                OnMediaOpened(e);
            };

            ArePlaybackControlsEnabled = true;
            AutoPlay = true;
            Stretch = Stretch.Uniform;
        }
    }
}
