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


using System.Threading.Tasks;
using Prism.UI.Media.Imaging;

namespace Prism.Native
{
    /// <summary>
    /// Defines a container for digital image data that is native to a particular platform.
    /// These objects are meant to be paired with platform-agnostic <see cref="ImageSource"/> objects.
    /// </summary>
    public interface INativeImageSource
    {
        /// <summary>
        /// Gets the number of pixels along the image's Y-axis.
        /// </summary>
        int PixelHeight { get; }

        /// <summary>
        /// Gets the number of pixels along the image's X-axis.
        /// </summary>
        int PixelWidth { get; }

        /// <summary>
        /// Gets the scaling factor of the image.
        /// </summary>
        double Scale { get; }

        /// <summary>
        /// Saves the image data to a file at the specified path using the specified file format.
        /// </summary>
        /// <param name="filePath">The path to the file in which to save the image data.</param>
        /// <param name="fileFormat">The file format in which to save the image data.</param>
        Task SaveAsync([CoreBehavior(CoreBehaviors.ChecksNullity)]string filePath, ImageFileFormat fileFormat);
    }
}
