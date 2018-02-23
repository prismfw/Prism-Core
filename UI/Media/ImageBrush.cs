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


using System;
using Prism.UI.Media.Imaging;

namespace Prism.UI.Media
{
    /// <summary>
    /// Represents a brush with an image.
    /// </summary>
    public class ImageBrush : Brush
    {
        /// <summary>
        /// Gets the image of the brush.
        /// </summary>
        public ImageSource Image { get; }

        /// <summary>
        /// Gets the manner in which the image will be stretched to fit its allocated space.
        /// </summary>
        public Stretch Stretch { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageBrush"/> class.
        /// </summary>
        /// <param name="sourceUri">The URI of the source file containing the image data for the brush.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="sourceUri"/> is <c>null</c>.</exception>
        public ImageBrush(Uri sourceUri)
            : this(sourceUri, Stretch.None)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageBrush"/> class.
        /// </summary>
        /// <param name="image">The <see cref="ImageSource"/> instance containing the image data for the brush.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="image"/> is <c>null</c>.</exception>
        public ImageBrush(ImageSource image)
            : this(image, Stretch.None)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageBrush"/> class.
        /// </summary>
        /// <param name="sourceUri">The URI of the source file containing the image data for the brush.</param>
        /// <param name="stretch">The manner in which the image will be stretched to fit its allocated space.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="sourceUri"/> is <c>null</c>.</exception>
        public ImageBrush(Uri sourceUri, Stretch stretch)
        {
            if (sourceUri == null)
            {
                throw new ArgumentNullException(nameof(sourceUri));
            }
            
            Image = new BitmapImage(sourceUri);
            Stretch = stretch;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageBrush"/> class.
        /// </summary>
        /// <param name="image">The <see cref="ImageSource"/> instance containing the image data for the brush.</param>
        /// <param name="stretch">The manner in which the image will be stretched to fit its allocated space.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="image"/> is <c>null</c>.</exception>
        public ImageBrush(ImageSource image, Stretch stretch)
        {
            if (image == null)
            {
                throw new ArgumentNullException(nameof(image));
            }

            Image = image;
            Stretch = stretch;
        }
    }
}
