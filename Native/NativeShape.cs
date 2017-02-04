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


using Prism.UI.Media;
using Prism.UI.Shapes;

namespace Prism.Native
{
    /// <summary>
    /// Defines a rendered shape that is native to a particular platform.
    /// These objects are meant to be paired with platform-agnostic <see cref="Shape"/> objects.
    /// </summary>
    [CoreBehavior(CoreBehaviors.MeasuresByContent)]
    public interface INativeShape : INativeElement
    {
        /// <summary>
        /// Gets or sets the <see cref="Brush"/> to apply to the interior of the shape.
        /// </summary>
        Brush Fill { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Brush"/> to apply to the outline of the shape.
        /// </summary>
        Brush Stroke { get; set; }

        /// <summary>
        /// Gets or sets the manner in which the ends of a line are drawn.
        /// </summary>
        LineCap StrokeLineCap { get; set; }

        /// <summary>
        /// Gets or sets the manner in which the connections between different lines are drawn.
        /// </summary>
        LineJoin StrokeLineJoin { get; set; }

        /// <summary>
        /// Gets or sets the miter limit for connecting lines.
        /// </summary>
        [CoreBehavior(CoreBehaviors.ChecksRange)]
        double StrokeMiterLimit { get; set; }

        /// <summary>
        /// Gets or sets the width of the shape's outline.
        /// </summary>
        [CoreBehavior(CoreBehaviors.ChecksRange)]
        double StrokeThickness { get; set; }

        /// <summary>
        /// Sets the dash pattern to be used when drawing the outline of the shape.
        /// </summary>
        /// <param name="pattern">An array of values that defines the dash pattern.  Each value represents the length of a dash, alternating between "on" and "off".</param>
        /// <param name="offset">The distance within the dash pattern where dashes begin.</param>
        void SetStrokeDashPattern(double[] pattern, [CoreBehavior(CoreBehaviors.ChecksRange)]double offset);
    }
}
