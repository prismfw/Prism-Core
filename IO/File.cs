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
using System.IO;
using System.Threading.Tasks;
using Prism.Native;

#if !DEBUG
using System.Diagnostics;
#endif

namespace Prism.IO
{
    /// <summary>
    /// Represents a utility that provides cross-platform file system access.
    /// </summary>
    public static class File
    {
#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private static INativeFile Current
        {
            get { return TypeManager.Default.Resolve<INativeFile>(); }
        }

        /// <summary>
        /// Opens the file at the specified path, appends the specified bytes to the end of the file, and then closes the file.
        /// </summary>
        /// <param name="filePath">The path of the file in which to append the bytes.</param>
        /// <param name="value">The bytes to append to the end of the file.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="filePath"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="filePath"/> is an invalid path.</exception>
        /// <exception cref="FileNotFoundException">Thrown when <paramref name="filePath"/> does not point to an existing file.</exception>
        public static async Task AppendBytesAsync(string filePath, byte[] value)
        {
            await Current.AppendBytesAsync(filePath, value);
        }

        /// <summary>
        /// Opens the file at the specified path, appends the specified bytes to the end of the file, and then closes the file.
        /// If the file does not exist, one is created.
        /// </summary>
        /// <param name="filePath">The path of the file in which to append the bytes.</param>
        /// <param name="value">The bytes to append to the end of the file.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="filePath"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="filePath"/> is an invalid path.</exception>
        public static async Task AppendAllBytesAsync(string filePath, byte[] value)
        {
            await Current.AppendAllBytesAsync(filePath, value);
        }

        /// <summary>
        /// Opens the file at the specified path, appends the specified text to the end of the file, and then closes the file.
        /// </summary>
        /// <param name="filePath">The path of the file in which to append the text.</param>
        /// <param name="value">The text to append to the end of the file.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="filePath"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="filePath"/> is an invalid path.</exception>
        /// <exception cref="FileNotFoundException">Thrown when <paramref name="filePath"/> does not point to an existing file.</exception>
        public static async Task AppendTextAsync(string filePath, string value)
        {
            await Current.AppendTextAsync(filePath, value);
        }

        /// <summary>
        /// Opens the file at the specified path, appends the specified text to the end of the file, and then closes the file.
        /// If the file does not exist, one is created.
        /// </summary>
        /// <param name="filePath">The path of the file in which to append the text.</param>
        /// <param name="value">The text to append to the end of the file.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="filePath"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="filePath"/> is an invalid path.</exception>
        public static async Task AppendAllTextAsync(string filePath, string value)
        {
            await Current.AppendAllTextAsync(filePath, value);
        }

        /// <summary>
        /// Copies the file at the source path to the destination path, overwriting any existing file.
        /// </summary>
        /// <param name="sourceFilePath">The path of the file to be copied.</param>
        /// <param name="destinationFilePath">The path to where the copied file should be placed.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="sourceFilePath"/> is <c>null</c> -or- when <paramref name="destinationFilePath"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="sourceFilePath"/> is an invalid path -or- when <paramref name="destinationFilePath"/> is an invalid path.</exception>
        /// <exception cref="FileNotFoundException">Thrown when <paramref name="sourceFilePath"/> does not point to an existing file.</exception>
        public static async Task CopyAsync(string sourceFilePath, string destinationFilePath)
        {
            await Current.CopyAsync(sourceFilePath, destinationFilePath);
        }

        /// <summary>
        /// Creates a file at the specified path, overwriting any existing file.
        /// </summary>
        /// <param name="filePath">The path at which to create the file.</param>
        /// <returns>A <see cref="Stream"/> for reading or writing to the file.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="filePath"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="filePath"/> is an invalid path.</exception>
        public static async Task<Stream> CreateAsync(string filePath)
        {
            return await Current.CreateAsync(filePath, 4096);
        }

        /// <summary>
        /// Creates a file at the specified path, overwriting any existing file.
        /// </summary>
        /// <param name="filePath">The path at which to create the file.</param>
        /// <param name="bufferSize">The number of bytes buffered for reading and writing to the file.</param>
        /// <returns>A <see cref="Stream"/> for reading or writing to the file.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="filePath"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="filePath"/> is an invalid path.</exception>
        public static async Task<Stream> CreateAsync(string filePath, int bufferSize)
        {
            return await Current.CreateAsync(filePath, bufferSize);
        }

        /// <summary>
        /// Deletes the file at the specified path.
        /// </summary>
        /// <param name="filePath">The path of the file to delete.</param>
        public static async Task DeleteAsync(string filePath)
        {
            await Current.DeleteAsync(filePath);
        }

        /// <summary>
        /// Moves the file at the source path to the destination path, overwriting any existing file.
        /// </summary>
        /// <param name="sourceFilePath">The path of the file to be moved.</param>
        /// <param name="destinationFilePath">The path to where the file should be moved.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="sourceFilePath"/> is <c>null</c> -or- when <paramref name="destinationFilePath"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="sourceFilePath"/> is an invalid path -or- when <paramref name="destinationFilePath"/> is an invalid path.</exception>
        /// <exception cref="FileNotFoundException">Thrown when <paramref name="sourceFilePath"/> does not point to an existing file.</exception>
        public static async Task MoveAsync(string sourceFilePath, string destinationFilePath)
        {
            await Current.MoveAsync(sourceFilePath, destinationFilePath);
        }

        /// <summary>
        /// Opens the file at the specified path, optionally creating one if it doesn't exist.
        /// </summary>
        /// <param name="filePath">The path of the file to be opened.</param>
        /// <param name="mode">The manner in which the file should be opened.</param>
        /// <returns>A <see cref="Stream"/> for reading or writing to the file.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="filePath"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="filePath"/> is an invalid path.</exception>
        /// <exception cref="FileNotFoundException">Thrown when <paramref name="filePath"/> does not point to an existing file and <paramref name="mode"/> is <see cref="FileMode.Open"/>.</exception>
        public static async Task<Stream> OpenAsync(string filePath, FileMode mode)
        {
            return await Current.OpenAsync(filePath, mode);
        }

        /// <summary>
        /// Opens the file at the specified path, reads all of the bytes in the file, and then closes the file.
        /// </summary>
        /// <param name="filePath">The path of the file from which to read the bytes.</param>
        /// <returns>An <see cref="Array"/> containing the bytes that were read from the file.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="filePath"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="filePath"/> is an invalid path.</exception>
        /// <exception cref="FileNotFoundException">Thrown when <paramref name="filePath"/> does not point to an existing file.</exception>
        public static async Task<byte[]> ReadAllBytesAsync(string filePath)
        {
            return await Current.ReadAllBytesAsync(filePath);
        }

        /// <summary>
        /// Opens the file at the specified path, reads all of the text in the file, and then closes the file.
        /// </summary>
        /// <param name="filePath">The path of the file from which to read the text.</param>
        /// <returns>A <see cref="string"/> containing the text that was read from the file.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="filePath"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="filePath"/> is an invalid path.</exception>
        /// <exception cref="FileNotFoundException">Thrown when <paramref name="filePath"/> does not point to an existing file.</exception>
        public static async Task<string> ReadAllTextAsync(string filePath)
        {
            return await Current.ReadAllTextAsync(filePath);
        }

        /// <summary>
        /// Creates a new file at the specified path, writes the specified bytes to the file, and then closes the file.
        /// If a file already exists, it is overwritten.
        /// </summary>
        /// <param name="filePath">The path of the file in which to write the bytes.</param>
        /// <param name="value">The bytes to write to the file.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="filePath"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="filePath"/> is an invalid path.</exception>
        public static async Task WriteAllBytesAsync(string filePath, byte[] value)
        {
            await Current.WriteAllBytesAsync(filePath, value);
        }

        /// <summary>
        /// Creates a new file at the specified path, writes the specified text to the file, and then closes the file.
        /// If a file already exists, it is overwritten.
        /// </summary>
        /// <param name="filePath">The path of the file in which to write the text.</param>
        /// <param name="value">The text to write to the file.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="filePath"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="filePath"/> is an invalid path.</exception>
        public static async Task WriteAllTextAsync(string filePath, string value)
        {
            await Current.WriteAllTextAsync(filePath, value);
        }
    }
}
