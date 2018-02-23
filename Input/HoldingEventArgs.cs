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

namespace Prism.Input
{
    /// <summary>
    /// Provides data for holding gestures.
    /// </summary>
    public class HoldingEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the type of the pointer device that triggered the gesture.
        /// </summary>
        public PointerType PointerType { get; }

        /// <summary>
        /// Gets the position of the pointer when the gesture was triggered, relative to the element on which it was triggered.
        /// </summary>
        public Point Position { get; }

        /// <summary>
        /// Gets the state of the gesture.
        /// </summary>
        public HoldingState State { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HoldingEventArgs"/> class.
        /// </summary>
        /// <param name="pointerType">The type of the pointer device that triggered the gesture.</param>
        /// <param name="position">The position of the pointer when the gesture was triggered, relative to the element on which it was triggered.</param>
        /// <param name="state">The state of the gesture.</param>
        public HoldingEventArgs(PointerType pointerType, Point position, HoldingState state)
        {
            PointerType = pointerType;
            Position = position;
            State = state;
        }
    }
}
