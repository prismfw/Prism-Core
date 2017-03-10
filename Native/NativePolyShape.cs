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


using System.Collections.Generic;
using Prism.UI.Media;

namespace Prism.Native
{
    /// <summary>
    /// Defines a rendered shape built from connecting lines that is native to a particular platform.
    /// </summary>
    public interface INativePolyShape : INativeShape
    {
        /// <summary>
        /// Gets or sets the rule to use for determining the interior fill of the shape.
        /// </summary>
        FillRule FillRule { get; set; }

        /// <summary>
        /// Gets a collection of the points that describe the vertices of the shape.
        /// </summary>
        IList<Point> Points { get; }
    }
}
