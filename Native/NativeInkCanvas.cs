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


using System.Collections.Generic;
using Prism.UI.Controls;
using Prism.UI.Media.Inking;

namespace Prism.Native
{
    /// <summary>
    /// Defines an ink canvas that is native to a particular platform.
    /// These objects are meant to be paired with platform-agnostic <see cref="InkCanvas"/> objects.
    /// </summary>
    public interface INativeInkCanvas : INativeElement
    {
        /// <summary>
        /// Gets or sets how the ink canvas handles input.
        /// </summary>
        InkInputMode InputMode { get; set; }

        /// <summary>
        /// Gets the ink strokes that are on the canvas.
        /// </summary>
        IEnumerable<INativeInkStroke> Strokes { get; }

        /// <summary>
        /// Adds the specified ink stroke to the canvas.
        /// </summary>
        /// <param name="stroke">The ink stroke to add.</param>
        void AddStroke(INativeInkStroke stroke);

        /// <summary>
        /// Adds the specified ink strokes to the canvas.
        /// </summary>
        /// <param name="strokes">The ink strokes to add.</param>
        void AddStrokes(IEnumerable<INativeInkStroke> strokes);

        /// <summary>
        /// Removes all ink strokes from the canvas.
        /// </summary>
        void ClearStrokes();

        /// <summary>
        /// Updates the drawing attributes to apply to new ink strokes on the canvas.
        /// </summary>
        /// <param name="attributes">The drawing attributes to apply to new ink strokes.</param>
        void UpdateDrawingAttributes(InkDrawingAttributes attributes);
    }
}
