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


namespace Prism.Input
{
    /// <summary>
    /// Describes the state of a holding gesture.
    /// </summary>
    public enum HoldingState
    {
        /// <summary>
        /// A pointer has been pressed, and enough time has passed without the pointer moving significantly or another gesture being recognized.
        /// </summary>
        Started = 0,
        /// <summary>
        /// The held pointer has been released, and the holding gesture has successfully completed.
        /// </summary>
        Completed = 1,
        /// <summary>
        /// The holding gesture has been canceled.  Another gesture has been recognized, or the holding gesture has failed for some reason.
        /// </summary>
        Canceled = 2
    }
}
