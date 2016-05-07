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
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Threading.Tasks;
using Prism.Native;

#if !DEBUG
using System.Diagnostics;
#endif

namespace Prism.UI.Media.Imaging
{
    /// <summary>
    /// Represents an object that contains digital data for an image.
    /// </summary>
    public sealed class ImageSource : FrameworkObject
    {
        #region Event Descriptors
        /// <summary>
        /// Describes the <see cref="E:ImageFailed"/> event.  This field is read-only.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "EventDescriptor is immutable.")]
        public static readonly EventDescriptor ImageFailedEvent = EventDescriptor.Create(nameof(ImageFailed), typeof(TypedEventHandler<ImageSource, ErrorEventArgs>), typeof(ImageSource));

        /// <summary>
        /// Describes the <see cref="E:ImageLoaded"/> event.  This field is read-only.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "EventDescriptor is immutable.")]
        public static readonly EventDescriptor ImageLoadedEvent = EventDescriptor.Create(nameof(ImageLoaded), typeof(TypedEventHandler<ImageSource>), typeof(ImageSource));
        #endregion

        /// <summary>
        /// Occurs when the image fails to load.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification = "Event handler provides a strongly-typed sender for easier use.")]
        public event TypedEventHandler<ImageSource, ErrorEventArgs> ImageFailed;

        /// <summary>
        /// Occurs when the image has been loaded into memory.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification = "Event handler provides a strongly-typed sender for easier use.")]
        public event TypedEventHandler<ImageSource> ImageLoaded;

        /// <summary>
        /// Gets a value indicating whether the image has encountered an error during loading.
        /// </summary>
        public bool IsFaulted
        {
            get { return nativeObject.IsFaulted; }
        }

        /// <summary>
        /// Gets a value indicating whether the image has been loaded into memory.
        /// </summary>
        public bool IsLoaded
        {
            get { return nativeObject.IsLoaded; }
        }

        /// <summary>
        /// Gets the number of pixels along the image's Y-axis.
        /// </summary>
        public int PixelHeight
        {
            get { return nativeObject.PixelHeight; }
        }

        /// <summary>
        /// Gets the number of pixels along the image's X-axis.
        /// </summary>
        public int PixelWidth
        {
            get { return nativeObject.PixelWidth; }
        }

        /// <summary>
        /// Gets the URI of the source file containing the image data.
        /// </summary>
        public Uri SourceUri
        {
            get { return nativeObject.SourceUri; }
        }

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private readonly INativeImageSource nativeObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageSource"/> class.
        /// </summary>
        /// <param name="sourceUri">The URI of the source file containing the image data.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="sourceUri"/> is <c>null</c>.</exception>
        public ImageSource(Uri sourceUri)
            : this(sourceUri, ImageCreationOptions.None)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageSource"/> class.
        /// </summary>
        /// <param name="sourceUri">The URI of the source file containing the image data.</param>
        /// <param name="options">Additional options to adhere to whening creating the image.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="sourceUri"/> is <c>null</c>.</exception>
        public ImageSource(Uri sourceUri, ImageCreationOptions options)
            : this(typeof(INativeImageSource), null, new ResolveParameter(nameof(sourceUri), sourceUri),
            new ResolveParameter("cachedImage", options.HasFlag(ImageCreationOptions.RefreshCache) || options.HasFlag(ImageCreationOptions.AvoidCache) ? null : ObjectRetriever.GetNativeObject(ImageCache.GetImage(sourceUri)), true))
        {
            if (options.HasFlag(ImageCreationOptions.AvoidCache))
            {
                ImageCache.Remove(sourceUri);
            }
            else
            {
                ImageCache.Add(sourceUri, this);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageSource"/> class.
        /// </summary>
        /// <param name="imageData">The byte array containing the data for the image.</param>
        public ImageSource(byte[] imageData)
            : this(typeof(INativeImageSource), null, new ResolveParameter(nameof(imageData), imageData, true))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageSource"/> class.
        /// </summary>
        /// <param name="resolveType">The type to pass to the IoC container in order to resolve the native object.</param>
        /// <param name="resolveName">An optional name to use when resolving the native object.</param>
        /// <param name="resolveParams">Any parameters to pass along to the constructor of the resolve type.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="resolveType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="resolveType"/> does not resolve to an <see cref="INativeImageSource"/> instance.</exception>
        private ImageSource(Type resolveType, string resolveName, params ResolveParameter[] resolveParams)
            : base(resolveType, resolveName, resolveParams)
        {
            nativeObject = ObjectRetriever.GetNativeObject(this) as INativeImageSource;
            if (nativeObject == null)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.Strings.TypeMustResolveToType, resolveType.FullName, typeof(INativeImageSource).FullName), nameof(resolveType));
            }

            if (nativeObject.IsLoaded)
            {
                OnImageLoaded(EventArgs.Empty);
            }

            nativeObject.ImageFailed += (o, e) => OnImageFailed(e);
            nativeObject.ImageLoaded += (o, e) => OnImageLoaded(e);
        }

        /// <summary>
        /// Saves the image data to a file at the specified path using the specified file format.
        /// </summary>
        /// <param name="filePath">The path to the file in which to save the image data.</param>
        /// <param name="fileFormat">The file format in which to save the image data.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="filePath"/> is <c>null</c>.</exception>
        public async Task SaveAsync(string filePath, ImageFileFormat fileFormat)
        {
            if (filePath == null)
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            await nativeObject.SaveAsync(filePath, fileFormat);
        }

        /// <summary>
        /// Called when an image fails to load and raises the <see cref="ImageFailed"/> event.
        /// </summary>
        /// <param name="e">The event arguments containing the error details.</param>
        private void OnImageFailed(ErrorEventArgs e)
        {
            ImageFailed?.Invoke(this, e);
        }

        /// <summary>
        /// Called when an image has been loaded into memory and raises the <see cref="ImageLoaded"/> event.
        /// </summary>
        /// <param name="e">The event arguments for the event.</param>
        private void OnImageLoaded(EventArgs e)
        {
            ImageLoaded?.Invoke(this, e);
        }
    }
}
