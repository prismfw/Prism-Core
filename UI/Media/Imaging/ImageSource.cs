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
using System.Globalization;
using System.Threading.Tasks;
using Prism.Native;

#if !DEBUG
using System.Diagnostics;
#endif

namespace Prism.UI.Media.Imaging
{
    /// <summary>
    /// Represents the base class for objects that contain digital data for an image.  This class is abstract.
    /// </summary>
    public abstract class ImageSource : FrameworkObject
    {
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
        /// Gets the scaling factor of the image.
        /// </summary>
        public double Scale
        {
            get { return nativeObject.Scale; }
        }

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private readonly INativeImageSource nativeObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageSource"/> class and pairs it with the specified native object.
        /// </summary>
        /// <param name="nativeObject">The native object with which to pair this instance.</param>
        /// <exception cref="ArgumentException">Thrown when a <see cref="ResolveAttribute"/> is located in the inheritance chain and <paramref name="nativeObject"/> doesn't match the type specified by the attribute.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="nativeObject"/> is <c>null</c>.</exception>
        protected ImageSource(INativeImageSource nativeObject)
            : base(nativeObject)
        {
            this.nativeObject = nativeObject;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageSource"/> class and pairs it with a native object that is resolved from the IoC container.
        /// At least one class in the inheritance chain must be decorated with a <see cref="ResolveAttribute"/> or an exception will be thrown.
        /// </summary>
        /// <param name="resolveParameters">Any parameters to pass along to the constructor of the native type.</param>
        /// <exception cref="TypeResolutionException">Thrown when the native object does not resolve to an <see cref="INativeImageSource"/> instance.</exception>
        protected ImageSource(ResolveParameter[] resolveParameters)
            : base(resolveParameters)
        {
            nativeObject = ObjectRetriever.GetNativeObject(this) as INativeImageSource;
            if (nativeObject == null)
            {
                throw new TypeResolutionException(string.Format(CultureInfo.CurrentCulture, Resources.Strings.TypeMustResolveToType,
                    ObjectRetriever.GetNativeObject(this).GetType().FullName, typeof(INativeImageSource).FullName));
            }
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
    }
}
