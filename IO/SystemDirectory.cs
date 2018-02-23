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


namespace Prism.IO
{
    /// <summary>
    /// Describes various directories that are known to the system.
    /// </summary>
    public enum SystemDirectory
    {
        /// <summary>
        /// The directory containing the application's bundled assets.
        /// </summary>
        Assets = 0,
        /// <summary>
        /// The directory containing persisted application data that is specific to the current user.
        /// </summary>
        Local = 1,
        /// <summary>
        /// The directory containing persisted application data that is shared between users.
        /// </summary>
        Shared = 2,
        /// <summary>
        /// The directory containing temporary application data that may be deleted by the system at any time.
        /// </summary>
        Temp = 3,
        /// <summary>
        /// The directory for the SD card or an external storage drive.
        /// </summary>
        External = 4,
        /// <summary>
        /// The directory for the current user's music library.
        /// </summary>
        Music = 5,
        /// <summary>
        /// The directory for the current user's photo library.
        /// </summary>
        Photos = 6,
        /// <summary>
        /// The directory for the current user's video library.
        /// </summary>
        Videos = 7
    }
}
