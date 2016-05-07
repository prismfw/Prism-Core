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


using Prism.UI.Controls;
using Prism.UI.Media;

namespace Prism.Native
{
    /// <summary>
    /// Defines a header on a view stack that is native to a particular platform.
    /// These objects are meant to be paired with platform-agnostic <see cref="ViewStackHeader"/> objects.
    /// </summary>
    public interface INativeViewStackHeader : INativeObject
    {
        /// <summary>
        /// Gets or sets the background for the header.
        /// </summary>
        Brush Background { get; set; }

        /// <summary>
        /// Gets or sets the font to use for displaying the title text.
        /// </summary>
        object FontFamily { get; set; }

        /// <summary>
        /// Gets or sets the size of the title text.
        /// </summary>
        double FontSize { get; set; }

        /// <summary>
        /// Gets or sets the style with which to render the title text.
        /// </summary>
        FontStyle FontStyle { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Brush"/> to apply to the foreground content of the header.
        /// </summary>
        Brush Foreground { get; set; }

        /// <summary>
        /// Gets or sets the title for the header.
        /// </summary>
        string Title { get; set; }
    }
}
