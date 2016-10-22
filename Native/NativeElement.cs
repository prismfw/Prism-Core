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


using System;
using Prism.Input;
using Prism.UI;
using Prism.UI.Controls;

namespace Prism.Native
{
    /// <summary>
    /// Defines a UI element inside of a view that is native to a particular platform.
    /// These objects are meant to be paired with platform-agnostic <see cref="Element"/> objects.
    /// </summary>
    public interface INativeElement : INativeVisual
    {
        /// <summary>
        /// Occurs when the system loses track of the pointer for some reason.
        /// </summary>
        event EventHandler<PointerEventArgs> PointerCanceled;

        /// <summary>
        /// Occurs when the pointer has moved while over the element.
        /// </summary>
        event EventHandler<PointerEventArgs> PointerMoved;

        /// <summary>
        /// Occurs when the pointer has been pressed while over the element.
        /// </summary>
        event EventHandler<PointerEventArgs> PointerPressed;

        /// <summary>
        /// Occurs when the pointer has been released while over the element.
        /// </summary>
        event EventHandler<PointerEventArgs> PointerReleased;

        /// <summary>
        /// Gets or sets the level of opacity for the element.
        /// </summary>
        [CoreBehavior(CoreBehaviors.ChecksRange)]
        double Opacity { get; set; }

        /// <summary>
        /// Gets or sets the display state of the element.
        /// </summary>
        Visibility Visibility { get; set; }
    }
}
