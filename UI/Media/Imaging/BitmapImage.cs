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
using Prism.Native;
using Prism.UI.Controls;

#if !DEBUG
using System.Diagnostics;
#endif

namespace Prism.UI.Media.Imaging
{
    /// <summary>
    /// Represents a bitmap that asynchronously loads image data from a <see cref="Uri"/> or byte array.
    /// Instances of this type can be used as a source for <see cref="Image"/> and <see cref="ImageBrush"/> objects.
    /// </summary>
    public sealed class BitmapImage : ImageSource
    {
        #region Event Descriptors
        /// <summary>
        /// Gets an <see cref="EventDescriptor"/> describing the <see cref="E:ImageFailed"/> event.
        /// </summary>
        public static EventDescriptor ImageFailedEvent { get; } = EventDescriptor.Create(nameof(ImageFailed), typeof(TypedEventHandler<BitmapImage, ErrorEventArgs>), typeof(BitmapImage));

        /// <summary>
        /// Gets an <see cref="EventDescriptor"/> describing the <see cref="E:ImageLoaded"/> event.
        /// </summary>
        public static EventDescriptor ImageLoadedEvent { get; } = EventDescriptor.Create(nameof(ImageLoaded), typeof(TypedEventHandler<BitmapImage>), typeof(BitmapImage));
        #endregion

        /// <summary>
        /// Occurs when the image fails to load.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification = "Event handler provides a strongly-typed sender for easier use.")]
        public event TypedEventHandler<BitmapImage, ErrorEventArgs> ImageFailed;

        /// <summary>
        /// Occurs when the image has been loaded into memory.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification = "Event handler provides a strongly-typed sender for easier use.")]
        public event TypedEventHandler<BitmapImage> ImageLoaded;

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
        /// Gets the URI of the source file containing the image data.
        /// </summary>
        public Uri SourceUri
        {
            get { return nativeObject.SourceUri; }
        }

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private readonly INativeBitmapImage nativeObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="BitmapImage"/> class.
        /// </summary>
        /// <param name="sourceUri">The URI of the source file containing the image data.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="sourceUri"/> is <c>null</c>.</exception>
        public BitmapImage(Uri sourceUri)
            : this(sourceUri, ImageCreationOptions.None)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BitmapImage"/> class.
        /// </summary>
        /// <param name="sourceUri">The URI of the source file containing the image data.</param>
        /// <param name="options">Additional options to adhere to when creating the image.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="sourceUri"/> is <c>null</c>.</exception>
        public BitmapImage(Uri sourceUri, ImageCreationOptions options)
            : this(typeof(INativeBitmapImage), null, new ResolveParameter(nameof(sourceUri), sourceUri),
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
        /// Initializes a new instance of the <see cref="BitmapImage"/> class.
        /// </summary>
        /// <param name="imageData">The byte array containing the data for the image.</param>
        public BitmapImage(byte[] imageData)
            : this(typeof(INativeBitmapImage), null, new ResolveParameter(nameof(imageData), imageData, true))
        {
        }

        private BitmapImage(Type resolveType, string resolveName, params ResolveParameter[] resolveParams)
            : base(resolveType, resolveName, resolveParams)
        {
            nativeObject = ObjectRetriever.GetNativeObject(this) as INativeBitmapImage;
            if (nativeObject == null)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.Strings.TypeMustResolveToType, resolveType.FullName, typeof(INativeBitmapImage).FullName), nameof(resolveType));
            }

            if (nativeObject.IsLoaded)
            {
                OnImageLoaded(EventArgs.Empty);
            }

            nativeObject.ImageFailed += (o, e) => OnImageFailed(e);
            nativeObject.ImageLoaded += (o, e) => OnImageLoaded(e);
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
