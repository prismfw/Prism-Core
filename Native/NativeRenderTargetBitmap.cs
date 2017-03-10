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
using System.Threading.Tasks;
using Prism.UI.Media.Imaging;

namespace Prism.Native
{
    /// <summary>
    /// Defines an object for rendering visual tree content that is native to a particular platform.
    /// These objects are meant to be paired with platform-agnostic <see cref="RenderTargetBitmap"/> objects.
    /// </summary>
    public interface INativeRenderTargetBitmap : INativeImageSource
    {
        /// <summary>
        /// Gets the data for the captured image as a byte array.
        /// </summary>
        /// <returns>The image data as an <see cref="Array"/> of bytes.</returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Asynchronous nature of method makes property inappropriate.")]
        Task<byte[]> GetPixelsAsync();

        /// <summary>
        /// Renders a snapshot of the specified visual object.
        /// </summary>
        /// <param name="target">The visual object to render.    This value can be <c>null</c> to render the entire visual tree.</param>
        /// <param name="width">The width of the snapshot.</param>
        /// <param name="height">The height of the snapshot.</param>
        Task RenderAsync(INativeVisual target, [CoreBehavior(CoreBehaviors.ChecksRange)]int width, [CoreBehavior(CoreBehaviors.ChecksRange)]int height);
    }
}
