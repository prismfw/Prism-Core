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


using Prism.UI;
using Prism.UI.Controls;

namespace Prism.Native
{
    /// <summary>
    /// Defines an image that is native to a particular platform.
    /// These objects are meant to be paired with platform-agnostic <see cref="Image"/> objects.
    /// </summary>
    public interface INativeImage : INativeElement
    {
        /// <summary>
        /// Gets or sets the <see cref="INativeImageSource"/> object that contains the image data for the element.
        /// </summary>
        INativeImageSource Source { get; set; }

        /// <summary>
        /// Gets or sets the manner in which the image will be stretched to fit its allocated space.
        /// </summary>
        Stretch Stretch { get; set; }
    }
}
