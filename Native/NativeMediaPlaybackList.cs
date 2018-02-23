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
using System.Collections;
using Prism.Media;

namespace Prism.Native
{
    /// <summary>
    /// Defines a media playback list that is native to a particular platform.
    /// These objects are meant to be paired with platform-agnostic <see cref="MediaPlaybackList"/> objects.
    /// </summary>
    public interface INativeMediaPlaybackList
    {
        /// <summary>
        /// Occurs when the currently playing item has changed.
        /// </summary>
        event EventHandler<NativeItemChangedEventArgs> CurrentItemChanged;

        /// <summary>
        /// Occurs when a playback item has failed to open.
        /// </summary>
        event EventHandler<NativeErrorEventArgs> ItemFailed;

        /// <summary>
        /// Occurs when a playback item has been successfully opened.
        /// </summary>
        event EventHandler<NativeItemEventArgs> ItemOpened;

        /// <summary>
        /// Gets the zero-based index of the current item in the <see cref="Items"/> collection.
        /// </summary>
        int CurrentItemIndex { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the playlist should automatically restart after the last item has finished playing.
        /// </summary>
        bool IsRepeatEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the items in the playlist should be played in random order.
        /// </summary>
        bool IsShuffleEnabled { get; set; }

        /// <summary>
        /// Gets a collection of playback items that make up the playlist.
        /// </summary>
        IList Items { get; }

        /// <summary>
        /// Moves to the next item in the playlist.
        /// </summary>
        void MoveNext();

        /// <summary>
        /// Moves to the previous item in the playlist.
        /// </summary>
        void MovePrevious();

        /// <summary>
        /// Moves to the item in the playlist that is located at the specified index.
        /// </summary>
        /// <param name="itemIndex">The zero-based index of the item to move to.</param>
        void MoveTo([CoreBehavior(CoreBehaviors.ChecksRange)]int itemIndex);
    }
}
