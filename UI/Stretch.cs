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


namespace Prism.UI
{
    /// <summary>
    /// Describes the manner in which content is stretched to fill the space allocated for it.
    /// </summary>
    public enum Stretch
    {
        /// <summary>
        /// The content is not stretched.
        /// </summary>
        None = 0,
        /// <summary>
        /// The content is stretched to fill the dimensions of the space.  Aspect ratio is not preserved.
        /// </summary>
        Fill = 1,
        /// <summary>
        /// The content is stretched to fit the dimensions of the space while preserving aspect ratio.
        /// </summary>
        Uniform = 2,
        /// <summary>
        /// The content is stretched to fill the dimensions of the space while preserving aspect ratio.
        /// Any portion of the content that doesn't fit within the space is clipped.
        /// </summary>
        UniformToFill = 3
    }
}
