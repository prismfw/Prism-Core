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
    /// Defines an image compositor that is native to a particular platform.
    /// These objects are meant to be paired with the platform-agnostic <see cref="ImageCompositor"/> object.
    /// </summary>
    public interface INativeImageCompositor
    {
        /// <summary>
        /// Composites the provided images into one image with the specified width and height.
        /// </summary>
        /// <param name="width">The width of the composited image.</param>
        /// <param name="height">The height of the composited image.</param>
        /// <param name="images">The images that are to be composited.  The first image will be drawn first and each subsequent image will be drawn on top.</param>
        /// <returns>The composited image as an <see cref="ImageSource"/> instance.</returns>
        Task<ImageSource> CompositeAsync([CoreBehavior(CoreBehaviors.ChecksRange)]int width,
            [CoreBehavior(CoreBehaviors.ChecksRange)]int height,
            [CoreBehavior(CoreBehaviors.ChecksNullity)]params INativeImageSource[] images);
    }
}
