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
using Prism.UI;

namespace Prism.Native
{
    /// <summary>
    /// Represents the method that is invoked when a visual object requests an arrangement of its children.
    /// </summary>
    /// <param name="forceArrange">Whether to force the arrangement to occur even if it would normally be skipped.</param>
    /// <param name="frameOverride">An optional override of the frame in which to arrange the object.</param>
    public delegate void ArrangeRequestHandler(bool forceArrange, Rectangle? frameOverride);

    /// <summary>
    /// Represents the method that is invoked when a visual object requests a measurement of itself and its children.
    /// </summary>
    /// <param name="forceMeasure">Whether to force the measurement to occur even if it would normally be skipped.</param>
    /// <param name="constraintsOverride">An optional override of the constraints with which to measure the object.</param> 
    /// <returns>The desired size of the object as a <see cref="Size"/>.</returns>
    public delegate Size MeasureRequestHandler(bool forceMeasure, Size? constraintsOverride);

    /// <summary>
    /// Defines a visual object that is native to a particular platform.
    /// These objects are meant to be paired with platform-agnostic <see cref="Visual"/> objects.
    /// </summary>
    public interface INativeVisual : INativeObject
    {
        /// <summary>
        /// Occurs when this instance has been attached to the visual tree and is ready to be rendered.
        /// </summary>
        event EventHandler Loaded;

        /// <summary>
        /// Occurs when this instance has been detached from the visual tree.
        /// </summary>
        event EventHandler Unloaded;

        /// <summary>
        /// Gets or sets a value indicating whether animations are enabled for this instance.
        /// </summary>
        bool AreAnimationsEnabled { get; set; }

        /// <summary>
        /// Gets or sets the method to invoke when this instance requests an arrangement of its children.
        /// </summary>
        ArrangeRequestHandler ArrangeRequest { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="Rectangle"/> that represents the size and position of the object relative to its parent container.
        /// </summary>
        Rectangle Frame { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance can be considered a valid result for hit testing.
        /// </summary>
        bool IsHitTestVisible { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance has been loaded and is ready for rendering.
        /// </summary>
        bool IsLoaded { get; }

        /// <summary>
        /// Gets or sets the method to invoke when this instance requests a measurement of itself and its children.
        /// </summary>
        MeasureRequestHandler MeasureRequest { get; set; }

        /// <summary>
        /// Invalidates the arrangement of this instance's children.
        /// </summary>
        void InvalidateArrange();

        /// <summary>
        /// Invalidates the measurement of this instance and its children.
        /// </summary>
        void InvalidateMeasure();

        /// <summary>
        /// Measures the object and returns its desired size.
        /// </summary>
        /// <param name="constraints">The width and height that the object is not allowed to exceed.</param>
        /// <returns>The desired size as a <see cref="Size"/> instance.</returns>
        Size Measure(Size constraints);
    }
}
