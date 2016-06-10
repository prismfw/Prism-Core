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
using Prism.UI.Media.Inking;

namespace Prism.Native
{
    /// <summary>
    /// Defines an ink stroke that is native to a particular platform.
    /// These objects are meant to be paired with platform-agnostic <see cref="InkStroke"/> objects.
    /// </summary>
    public interface INativeInkStroke
    {
        /// <summary>
        /// Gets a rectangle that encompasses all of the points in the ink stroke.
        /// </summary>
        Rectangle BoundingBox { get; }

        /// <summary>
        /// Gets a collection of points that make up the ink stroke.
        /// </summary>
        IEnumerable<Point> Points { get; }

        /// <summary>
        /// Returns a deep-copy clone of this instance.
        /// </summary>
        INativeInkStroke Clone();

        /// <summary>
        /// Returns a copy of the ink stroke's drawing attributes.
        /// </summary>
        InkDrawingAttributes CopyDrawingAttributes();

        /// <summary>
        /// Updates the drawing attributes of the ink stroke.
        /// </summary>
        /// <param name="attributes">The drawing attributes to apply to the ink stroke.</param>
        void UpdateDrawingAttributes(InkDrawingAttributes attributes);
    }
}
