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
using Prism.UI.Media.Imaging;

namespace Prism.Native
{
    /// <summary>
    /// Defines a bitmap image that is native to a particular platform.
    /// These objects are meant to be paired with platform-agnostic <see cref="BitmapImage"/> objects.
    /// </summary>
    public interface INativeBitmapImage : INativeImageSource
    {
        /// <summary>
        /// Occurs when the image fails to load.
        /// </summary>
        event EventHandler<ErrorEventArgs> ImageFailed;

        /// <summary>
        /// Occurs when the image has been loaded into memory.
        /// </summary>
        event EventHandler ImageLoaded;

        /// <summary>
        /// Gets a value indicating whether the image has encountered an error during loading.
        /// </summary>
        bool IsFaulted { get; }

        /// <summary>
        /// Gets a value indicating whether the image has been loaded into memory.
        /// </summary>
        bool IsLoaded { get; }

        /// <summary>
        /// Gets the URI of the source file containing the image data.
        /// </summary>
        Uri SourceUri { get; }
    }
}
