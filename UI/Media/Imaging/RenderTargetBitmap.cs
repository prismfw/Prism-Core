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
    /// Represents a bitmap that stores a snapshot of visual tree content.
    /// Instances of this type can be used as a source for <see cref="Image"/> and <see cref="ImageBrush"/> objects.
    /// </summary>
    [Resolve(typeof(INativeRenderTargetBitmap))]
    public sealed class RenderTargetBitmap : ImageSource
    {
#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private readonly INativeRenderTargetBitmap nativeObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="RenderTargetBitmap"/> class.
        /// </summary>
        public RenderTargetBitmap()
            : base(ResolveParameter.EmptyParameters)
        {
            nativeObject = ObjectRetriever.GetNativeObject(this) as INativeRenderTargetBitmap;
            if (nativeObject == null)
            {
                throw new TypeResolutionException(string.Format(CultureInfo.CurrentCulture, Resources.Strings.TypeMustResolveToType,
                    ObjectRetriever.GetNativeObject(this).GetType().FullName, typeof(INativeRenderTargetBitmap).FullName));
            }
        }

        /// <summary>
        /// Gets the data for the captured image as a byte array.
        /// </summary>
        /// <returns>The image data as an <see cref="Array"/> of bytes.</returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Asynchronous nature of method makes property inappropriate.")]
        public Task<byte[]> GetPixelsAsync()
        {
            return nativeObject.GetPixelsAsync();
        }

        /// <summary>
        /// Renders a snapshot of the specified visual object.
        /// </summary>
        /// <param name="target">The visual object to render.  This value can be <c>null</c> to render the entire visual tree.</param>
        public Task RenderAsync(Visual target)
        {
            return RenderAsync(target, 0, 0);
        }

        /// <summary>
        /// Renders a snapshot of the specified visual object.
        /// </summary>
        /// <param name="target">The visual object to render.  This value can be <c>null</c> to render the entire visual tree.</param>
        /// <param name="width">The width of the snapshot.  A value of 0 means the width will be determined automatically.</param>
        /// <param name="height">The height of the snapshot.  A value of 0 means the height will be determined automatically.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="width"/> is less than zero -or- when <paramref name="height"/> is less than zero.</exception>
        public Task RenderAsync(Visual target, int width, int height)
        {
            if (width < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(width), Resources.Strings.ValueCannotBeLessThanZero);
            }

            if (height < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(height), Resources.Strings.ValueCannotBeLessThanZero);
            }

            if (width == 0 && height == 0)
            {
                width = (int)(target?.RenderSize.Width ?? Window.Current.Width);
                height = (int)(target?.RenderSize.Height ?? Window.Current.Height);
            }
            else if (width == 0)
            {
                width = (int)((target?.RenderSize.Width ?? Window.Current.Width) * (height / (target?.RenderSize.Height ?? Window.Current.Height)));
            }
            else if (height == 0)
            {
                height = (int)((target?.RenderSize.Height ?? Window.Current.Height) * (width / (target?.RenderSize.Width ?? Window.Current.Width)));
            }

            return nativeObject.RenderAsync((INativeVisual)ObjectRetriever.GetNativeObject(target), width, height);
        }
    }
}
