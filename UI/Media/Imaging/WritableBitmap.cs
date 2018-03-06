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
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Threading.Tasks;
using Prism.Native;
using Prism.UI.Controls;

#if !DEBUG
using System.Diagnostics;
#endif

namespace Prism.UI.Media.Imaging
{
    /// <summary>
    /// Represents a bitmap that can have pixel data written to it.
    /// Instances of this type can be used as a source for <see cref="Image"/> and <see cref="ImageBrush"/> objects.
    /// </summary>
    [Resolve(typeof(INativeWritableBitmap))]
    public sealed class WritableBitmap : ImageSource
    {
#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private readonly INativeWritableBitmap nativeObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="WritableBitmap"/> class.
        /// </summary>
        /// <param name="pixelWidth">The number of pixels along the image's X-axis.</param>
        /// <param name="pixelHeight">The number of pixels along the image's Y-axis.</param>
        public WritableBitmap(int pixelWidth, int pixelHeight)
            : base(new[] { new ResolveParameter(nameof(pixelWidth), pixelWidth), new ResolveParameter(nameof(pixelHeight), pixelHeight) })
        {
            nativeObject = ObjectRetriever.GetNativeObject(this) as INativeWritableBitmap;
            if (nativeObject == null)
            {
                throw new TypeResolutionException(string.Format(CultureInfo.CurrentCulture, Resources.Strings.TypeMustResolveToType,
                    ObjectRetriever.GetNativeObject(this).GetType().FullName, typeof(INativeWritableBitmap).FullName));
            }
        }

        internal WritableBitmap(INativeWritableBitmap nativeObject)
            : base(nativeObject)
        {
            this.nativeObject = nativeObject;
        }

        /// <summary>
        /// Gets the pixel data of the bitmap as a byte array.
        /// </summary>
        /// <returns>The pixel data as an <see cref="Array"/> of bytes.</returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Asynchronous nature of method makes property inappropriate.")]
        public Task<byte[]> GetPixelsAsync()
        {
            return nativeObject.GetPixelsAsync();
        }

        /// <summary>
        /// Sets the pixel data of the bitmap to the specified byte array.
        /// </summary>
        /// <param name="pixelData">The byte array containing the pixel data.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="pixelData"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="pixelData"/> is of an invalid length.</exception>
        public Task SetPixelsAsync(byte[] pixelData)
        {
            if (pixelData == null)
            {
                throw new ArgumentNullException(nameof(pixelData));
            }

            int expectedLength = PixelWidth * PixelWidth * 4;
            if (pixelData.Length != expectedLength)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.Strings.ArrayLengthIsInvalid, expectedLength), nameof(pixelData));
            }

            return nativeObject.SetPixelsAsync(pixelData);
        }
    }
}
