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

namespace Prism
{
    /// <summary>
    /// Represents a method that handles general events.
    /// </summary>
    /// <typeparam name="TSender">The type of the event source.</typeparam>
    /// <param name="sender">The source of the event.</param>
    /// <param name="args">The event data, or <c>null</c> if no data was generated.</param>
    public delegate void TypedEventHandler<TSender>(TSender sender, EventArgs args);

    /// <summary>
    /// Represents a method that handles general events.
    /// </summary>
    /// <typeparam name="TSender">The type of the event source.</typeparam>
    /// <typeparam name="TEventArgs">The type of event data generated by the event.</typeparam>
    /// <param name="sender">The source of the event.</param>
    /// <param name="args">The event data, or <c>null</c> if no data was generated.</param>
    public delegate void TypedEventHandler<TSender, TEventArgs>(TSender sender, TEventArgs args) where TEventArgs : EventArgs;
}
