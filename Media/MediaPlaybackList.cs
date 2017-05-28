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
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Prism.Native;

#if !DEBUG
using System.Diagnostics;
#endif

namespace Prism.Media
{
    /// <summary>
    /// Represents a playlist of media items for an audio or video player.
    /// </summary>
    public class MediaPlaybackList : IMediaPlaybackSource
    {
        /// <summary>
        /// Occurs when the currently playing item has changed.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification = "Event handler provides a strongly-typed sender for easier use.")]
        public event TypedEventHandler<MediaPlaybackList, MediaPlaybackItemChangedEventArgs> CurrentItemChanged;

        /// <summary>
        /// Occurs when a playback item has failed to open.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification = "Event handler provides a strongly-typed sender for easier use.")]
        public event TypedEventHandler<MediaPlaybackList, MediaPlaybackItemFailedEventArgs> ItemFailed;

        /// <summary>
        /// Occurs when a playback item has been successfully opened.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification = "Event handler provides a strongly-typed sender for easier use.")]
        public event TypedEventHandler<MediaPlaybackList, MediaPlaybackItemOpenedEventArgs> ItemOpened;

        /// <summary>
        /// Gets the zero-based index of the current item in the <see cref="Items"/> collection.
        /// </summary>
        public int CurrentItemIndex
        {
            get { return nativeObject.CurrentItemIndex; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the playlist should automatically restart after the last item has finished playing.
        /// </summary>
        public bool IsRepeatEnabled
        {
            get { return nativeObject.IsRepeatEnabled; }
            set { nativeObject.IsRepeatEnabled = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the items in the playlist should be played in random order.
        /// </summary>
        public bool IsShuffleEnabled
        {
            get { return nativeObject.IsShuffleEnabled; }
            set { nativeObject.IsShuffleEnabled = value; }
        }

        /// <summary>
        /// Gets a collection of playback items that make up the playlist.
        /// </summary>
        public IList<MediaPlaybackItem> Items { get; }

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private readonly INativeMediaPlaybackList nativeObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaPlaybackList"/> class.
        /// </summary>
        public MediaPlaybackList()
        {
            nativeObject = TypeManager.Default.Resolve<INativeMediaPlaybackList>(TypeResolutionOptions.UseFuzzyNameResolution | TypeResolutionOptions.UseFuzzyParameterResolution);
            if (nativeObject == null)
            {
                throw new TypeResolutionException(string.Format(CultureInfo.CurrentCulture, Resources.Strings.TypeCouldNotBeResolved, typeof(INativeMediaPlaybackList).FullName));
            }

            ObjectRetriever.SetPair(this, nativeObject);

            nativeObject.CurrentItemChanged += (o, e) =>
            {
                OnCurrentItemChanged(new MediaPlaybackItemChangedEventArgs(
                    (MediaPlaybackItem)ObjectRetriever.GetAgnosticObject(e.OldItem),
                    (MediaPlaybackItem)ObjectRetriever.GetAgnosticObject(e.NewItem)));
            };

            nativeObject.ItemFailed += (o, e) =>
            {
                OnItemFailed(new MediaPlaybackItemFailedEventArgs((MediaPlaybackItem)ObjectRetriever.GetAgnosticObject(e.Item), e.Exception));
            };

            nativeObject.ItemOpened += (o, e) =>
            {
                var item = (MediaPlaybackItem)ObjectRetriever.GetAgnosticObject(e.Item);
                item.IsOpen = true;

                OnItemOpened(new MediaPlaybackItemOpenedEventArgs(item));
            };

            Items = new MediaPlaybackItemCollection(nativeObject);
        }

        /// <summary>
        /// Moves to the next item in the playlist.
        /// </summary>
        public void MoveNext()
        {
            if (Items.Count > 0)
            {
                nativeObject.MoveNext();
            }
        }

        /// <summary>
        /// Moves to the previous item in the playlist.
        /// </summary>
        public void MovePrevious()
        {
            if (Items.Count > 0)
            {
                nativeObject.MovePrevious();
            }
        }

        /// <summary>
        /// Moves to the item in the playlist that is located at the specified index.
        /// </summary>
        /// <param name="itemIndex">The zero-based index of the item to move to.</param>
        public void MoveTo(int itemIndex)
        {
            if (itemIndex < 0 || itemIndex >= Items.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(itemIndex));
            }

            nativeObject.MoveTo(itemIndex);
        }

        /// <summary>
        /// Called when the currently playing item has changed and raises the <see cref="CurrentItemChanged"/> event.
        /// </summary>
        /// <param name="e">The event arguments for the event.</param>
        protected virtual void OnCurrentItemChanged(MediaPlaybackItemChangedEventArgs e)
        {
            CurrentItemChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Called when a playback item has failed to open and raises the <see cref="ItemFailed"/> event.
        /// </summary>
        /// <param name="e">The event arguments for the event.</param>
        protected virtual void OnItemFailed(MediaPlaybackItemFailedEventArgs e)
        {
            ItemFailed?.Invoke(this, e);
        }

        /// <summary>
        /// Called when a playback item has been successfully opened and raises the <see cref="ItemOpened"/> event.
        /// </summary>
        /// <param name="e">The event arguments for the event.</param>
        protected virtual void OnItemOpened(MediaPlaybackItemOpenedEventArgs e)
        {
            ItemOpened?.Invoke(this, e);
        }
    }
}
