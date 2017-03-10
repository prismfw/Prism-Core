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
    /// Defines a slider that is native to a particular platform.
    /// These objects are meant to be paired with platform-agnostic <see cref="Slider"/> objects.
    /// </summary>
    public interface INativeSlider : INativeControl
    {
        /// <summary>
        /// Occurs when the <see cref="P:Value"/> property has changed.
        /// </summary>
        event EventHandler<ValueChangedEventArgs<double>> ValueChanged;

        /// <summary>
        /// Gets or sets a value indicating whether the thumb should automatically move to the closest step.
        /// </summary>
        [CoreBehavior(CoreBehaviors.ExpectsEarlyChangeNotification)]
        bool IsSnapToStepEnabled { get; set; }

        /// <summary>
        /// Gets or sets the maximum value that the control is allowed to have.
        /// </summary>
        double MaxValue { get; set; }

        /// <summary>
        /// Gets or sets the minimum value that the control is allowed to have.
        /// </summary>
        double MinValue { get; set; }

        /// <summary>
        /// Gets or sets the interval between steps along the track.
        /// </summary>
        [CoreBehavior(CoreBehaviors.ExpectsEarlyChangeNotification)]
        double StepFrequency { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Brush"/> to apply to the thumb of the control.
        /// </summary>
        Brush ThumbBrush { get; set; }

        /// <summary>
        /// Gets or sets the current value of the control.
        /// </summary>
        double Value { get; set; }
    }
}
