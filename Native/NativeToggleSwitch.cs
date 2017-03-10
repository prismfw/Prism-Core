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
using Prism.UI.Controls;
using Prism.UI.Media;

namespace Prism.Native
{
    /// <summary>
    /// Defines a toggle switch that is native to a particular platform.
    /// These objects are meant to be paired with platform-agnostic <see cref="ToggleSwitch"/> objects.
    /// </summary>
    public interface INativeToggleSwitch : INativeControl
    {
        /// <summary>
        /// Occurs when the value of the toggle switch is changed.
        /// </summary>
        event EventHandler ValueChanged;

        /// <summary>
        /// Gets or sets the <see cref="Brush"/> to apply to the thumb of the control when the control's value is false.
        /// </summary>
        Brush ThumbOffBrush { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Brush"/> to apply to the thumb of the control when the control's value is true.
        /// </summary>
        Brush ThumbOnBrush { get; set; }

        /// <summary>
        /// Gets or sets the value of the toggle switch.
        /// </summary>
        bool Value { get; set; }
    }
}
