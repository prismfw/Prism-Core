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

namespace Prism.Native
{
    /// <summary>
    /// Defines a date picker that is native to a particular platform.
    /// These objects are meant to be paired with platform-agnostic <see cref="DatePicker"/> objects.
    /// </summary>
    public interface INativeDatePicker : INativeControl
    {
        /// <summary>
        /// Occurs when the selected date has changed.
        /// </summary>
        event EventHandler<DateChangedEventArgs> DateChanged;

        /// <summary>
        /// Gets or sets the format in which to display the string value of the selected date.
        /// </summary>
        [CoreBehavior(CoreBehaviors.ExpectsEarlyChangeNotification)]
        string DateStringFormat { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the picker is open.
        /// </summary>
        bool IsOpen { get; set; }

        /// <summary>
        /// Gets or sets the selected date.
        /// </summary>
        [CoreBehavior(CoreBehaviors.ExpectsEarlyChangeNotification)]
        DateTime? SelectedDate { get; set; }
    }
}
