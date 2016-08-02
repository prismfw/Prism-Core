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


namespace Prism.Systems
{
    /// <summary>
    /// Describes the orientation of a device.
    /// </summary>
    public enum DeviceOrientation
    {
        /// <summary>
        /// The orientation of the device is unknown or cannot be determined.
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// The device is in portrait orientation, either right-side up or upside down.
        /// </summary>
        Portrait = 1,
        /// <summary>
        /// The device is in landscape orientation, either facing left or facing right.
        /// </summary>
        Landscape = 2,
        /// <summary>
        /// The device is in portrait orientation and right-side up.
        /// </summary>
        PortraitUp = 5,
        /// <summary>
        /// The device is in landscape orientation and facing left.
        /// </summary>
        LandscapeLeft = 6,
        /// <summary>
        /// The device is in portrait orientation and upside down.
        /// </summary>
        PortraitDown = 9,
        /// <summary>
        /// The device is in landscape orientation and facing right.
        /// </summary>
        LandscapeRight = 10,
        /// <summary>
        /// The device is facing upward.
        /// </summary>
        FaceUp = 16,
        /// <summary>
        /// The device is facing downward.
        /// </summary>
        FaceDown = 32
    }
}
