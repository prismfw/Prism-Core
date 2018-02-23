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


using System.Collections.Generic;
using Prism.UI.Media;
using Prism.UI.Shapes;

namespace Prism.Native
{
    /// <summary>
    /// Represents the method that is invoked when a renderable shape requests information about the path being drawn.
    /// </summary>
    /// <returns>A list of the various subsections that make up the path.</returns>
    public delegate IList<PathFigure> PathInfoRequestHandler();

    /// <summary>
    /// Defines a rendered path that is native to a particular platform.
    /// These objects are meant to be paired with platform-agnostic <see cref="Path"/> objects.
    /// </summary>
    public interface INativePath : INativeShape
    {
        /// <summary>
        /// Gets or sets the rule to use for determining the interior fill of the shape.
        /// </summary>
        FillRule FillRule { get; set; }

        /// <summary>
        /// Gets or sets the method to invoke when this instance requests information for the path being drawn.
        /// </summary>
        PathInfoRequestHandler PathInfoRequest { get; set; }

        /// <summary>
        /// Signals that the path needs to be rebuilt before it is drawn again.
        /// </summary>
        void InvalidatePathInfo();
    }
}
