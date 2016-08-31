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


using System;
using System.Linq;
using System.Threading.Tasks;
using Prism.Native;

namespace Prism.UI.Media.Imaging
{
    /// <summary>
    /// Represents a utility that is able to composite a series of images together.
    /// </summary>
    public static class ImageCompositor
    {
        /// <summary>
        /// Composites the provided images into one image with the specified width and height.
        /// </summary>
        /// <param name="width">The width of the composited image.</param>
        /// <param name="height">The height of the composited image.</param>
        /// <param name="images">The images that are to be composited into one.  The first image will be drawn first and each subsequent image will be drawn on top.</param>
        /// <returns>The composited image as an <see cref="ImageSource"/> instance.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="images"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="width"/> is less than zero -or- when <paramref name="height"/> is less than zero.</exception>
        public static async Task<ImageSource> CompositeAsync(int width, int height, params ImageSource[] images)
        {
            if (images == null)
            {
                throw new ArgumentNullException(nameof(images));
            }

            if (width < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(width), Resources.Strings.ValueCannotBeLessThanZero);
            }

            if (height < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(height), Resources.Strings.ValueCannotBeLessThanZero);
            }

            return await TypeManager.Default.Resolve<INativeImageCompositor>().CompositeAsync(width, height, images.Select(i => (INativeImageSource)ObjectRetriever.GetNativeObject(i)).ToArray());
        }
    }
}
