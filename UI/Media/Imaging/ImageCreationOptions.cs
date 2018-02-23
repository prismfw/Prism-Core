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

namespace Prism.UI.Media.Imaging
{
    /// <summary>
    /// Describes options for customizing image creation.
    /// </summary>
    [Flags]
    public enum ImageCreationOptions
    {
        /// <summary>
        /// The default creation behavior is used.  No special options are taken into account.
        /// </summary>
        None = 0,
        /// <summary>
        /// The image should not be pulled from the cache.  Use this option when needing to refresh an image in the cache.
        /// The resulting image is put into the cache for later use.
        /// </summary>
        RefreshCache = 1,
        /// <summary>
        /// The image should not be pulled from the cache.  The resulting image is not cached for later use and any image
        /// currently in the cache with an identical source will be removed.
        /// </summary>
        AvoidCache = 2,
    }
}
