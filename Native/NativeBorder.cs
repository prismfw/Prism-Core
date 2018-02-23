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


using Prism.UI.Media;
using Prism.UI.Controls;

namespace Prism.Native
{
    /// <summary>
    /// Defines a UI element for drawing a border around another UI element that native to a particular platform.
    /// These objects are meant to be paired with platform-agnostic <see cref="Border"/> objects.
    /// </summary>
    public interface INativeBorder : INativeElement
    {
        /// <summary>
        /// Gets or sets the background for this instance.
        /// </summary>
        Brush Background { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Brush"/> with which to paint the border.
        /// </summary>
        Brush BorderBrush { get; set; }

        /// <summary>
        /// Gets or sets the thickness of the border.
        /// </summary>
        Thickness BorderThickness { get; set; }

        /// <summary>
        /// Gets or sets the child element around which to draw the border.
        /// </summary>
        object Child { get; set; }

        /// <summary>
        /// Gets or sets the padding between the border and the child element.
        /// </summary>
        Thickness Padding { get; set; }
    }
}
