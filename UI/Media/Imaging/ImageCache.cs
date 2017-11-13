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


using System;
using System.Diagnostics.CodeAnalysis;
using Prism.IO;

#if !DEBUG
using System.Diagnostics;
#endif

namespace Prism.UI.Media.Imaging
{
    /// <summary>
    /// Defines an object that provides caching of images in memory.
    /// </summary>
    public interface IImageCache
    {
        /// <summary>
        /// Gets or sets the maximum capacity of the image cache.
        /// </summary>
        int Capacity { get; set; }

        /// <summary>
        /// Gets the number of images currently in the cache.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Adds the specified image to the image cache.
        /// </summary>
        /// <param name="sourceUri">The URI of the source file containing the image data.</param>
        /// <param name="image">The image to be cached.</param>
        /// <param name="expirationDate">An optional point in time for when the image should be marked for removal from the cache.
        /// A value of <c>null</c> means the image will not expire.</param>
        void Add(Uri sourceUri, ImageSource image, DateTime? expirationDate);

        /// <summary>
        /// Clears all images from the cache.
        /// </summary>
        void Clear();

        /// <summary>
        /// Returns a value indicating whether the cache contains an image for the specified URI.
        /// </summary>
        /// <param name="sourceUri">The URI of the image to check.</param>
        /// <returns><c>true</c> if the cache contains an image for the URI; otherwise <c>false</c>.</returns>
        bool Contains(Uri sourceUri);

        /// <summary>
        /// Retrieves from the cache the image for the specified URI.
        /// </summary>
        /// <param name="sourceUri">The URI of the image to retrieve.</param>
        /// <returns>The image for the URI as an <see cref="ImageSource"/> instance, or <c>null</c> if no such image is found.</returns>
        ImageSource GetImage(Uri sourceUri);

        /// <summary>
        /// Removes from the cache the image for the specified URI.
        /// </summary>
        /// <param name="sourceUri">The URI of the image to remove.</param>
        void Remove(Uri sourceUri);

        /// <summary>
        /// Immediately removes any images that have passed their expiration dates.
        /// </summary>
        void RemoveExpired();
    }

    /// <summary>
    /// Represents a utility for managing images that are cached in memory.
    /// </summary>
    public static class ImageCache
    {
        /// <summary>
        /// Gets or sets the maximum capacity of the image cache.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", Justification = "Exception parameter refers to property name for easier understanding of invalid value.")]
        public static int Capacity
        {
            get { return Current.Capacity; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(Capacity), Resources.Strings.ValueCannotBeLessThanZero);
                }

                Current.Capacity = value;
            }
        }

        /// <summary>
        /// Gets the number of images currently in the cache.
        /// </summary>
        public static int Count
        {
            get { return Current.Count; }
        }

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private static IImageCache Current
        {
            get
            {
                var current = TypeManager.Default.Resolve<IImageCache>();
                if (current == null)
                {
                    TypeManager.Default.RegisterSingleton(typeof(IImageCache), (current = new CommonImageCache()));
                }

                return current;
            }
        }

        /// <summary>
        /// Adds the specified image to the image cache.
        /// </summary>
        /// <param name="sourceUri">The URI of the source file containing the image data.</param>
        /// <param name="image">The image to be cached.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="sourceUri"/> is <c>null</c> -or- when <paramref name="image"/> is <c>null</c>.</exception>
        public static void Add(Uri sourceUri, ImageSource image)
        {
            Add(sourceUri, image, null);
        }

        /// <summary>
        /// Adds the specified image to the image cache.
        /// </summary>
        /// <param name="sourceUri">The URI of the source file containing the image data.</param>
        /// <param name="image">The image to be cached.</param>
        /// <param name="expirationDate">An optional point in time for when the image should be marked for removal from the cache.
        /// A value of <c>null</c> means the image will not expire.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="sourceUri"/> is <c>null</c> -or- when <paramref name="image"/> is <c>null</c>.</exception>
        public static void Add(Uri sourceUri, ImageSource image, DateTime? expirationDate)
        {
            if (sourceUri == null)
            {
                throw new ArgumentNullException(nameof(sourceUri));
            }

            if (image == null)
            {
                throw new ArgumentNullException(nameof(image));
            }

            Current.Add(Directory.ValidateUri(sourceUri), image, expirationDate);
        }

        /// <summary>
        /// Clears all images from the cache.
        /// </summary>
        public static void Clear()
        {
            Current.Clear();
        }

        /// <summary>
        /// Returns a value indicating whether the cache contains an image for the specified URI.
        /// </summary>
        /// <param name="sourceUri">The URI of the image to check.</param>
        /// <returns><c>true</c> if the cache contains an image for the URI; otherwise <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="sourceUri"/> is <c>null</c>.</exception>
        public static bool Contains(Uri sourceUri)
        {
            if (sourceUri == null)
            {
                throw new ArgumentNullException(nameof(sourceUri));
            }

            return Current.Contains(Directory.ValidateUri(sourceUri));
        }

        /// <summary>
        /// Retrieves from the cache the image for the specified URI.
        /// </summary>
        /// <param name="sourceUri">The URI of the image to retrieve.</param>
        /// <returns>The image for the URI as an <see cref="ImageSource"/> instance, or <c>null</c> if no such image is found.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="sourceUri"/> is <c>null</c>.</exception>
        public static ImageSource GetImage(Uri sourceUri)
        {
            if (sourceUri == null)
            {
                throw new ArgumentNullException(nameof(sourceUri));
            }

            return Current.GetImage(Directory.ValidateUri(sourceUri));
        }

        /// <summary>
        /// Removes from the cache the image for the specified URI.
        /// </summary>
        /// <param name="sourceUri">The URI of the image to remove.</param>
        public static void Remove(Uri sourceUri)
        {
            if (sourceUri != null)
            {
                Current.Remove(Directory.ValidateUri(sourceUri));
            }
        }

        /// <summary>
        /// Immediately removes any images that have passed their expiration dates.
        /// </summary>
        public static void RemoveExpired()
        {
            Current.RemoveExpired();
        }
    }
}
