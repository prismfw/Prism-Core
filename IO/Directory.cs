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
using System.IO;
using System.Threading.Tasks;
using Prism.Native;

#if !DEBUG
using System.Diagnostics;
#endif

namespace Prism.IO
{
    /// <summary>
    /// Represents a utility that provides cross-platform management of directories on the file system.
    /// </summary>
    public static class Directory
    {
        /// <summary>
        /// Gets the directory path to a folder with read-only access that contains the application's bundled assets.
        /// </summary>
        public static string AssetDirectory
        {
            get { return Current.AssetDirectory; }
        }

        /// <summary>
        /// Gets the directory path to a folder with read/write access for storing persisted application data.
        /// </summary>
        public static string DataDirectory
        {
            get { return Current.DataDirectory; }
        }

        /// <summary>
        /// Gets the character that is used to separate directories.
        /// </summary>
        public static char SeparatorChar
        {
            get { return Current.SeparatorChar; }
        }

        /// <summary>
        /// Gets the directory path to a folder with read/write access for storing temporary application data.
        /// </summary>
        public static string TempDirectory
        {
            get { return Current.TempDirectory; }
        }

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private static INativeDirectory Current
        {
            get { return TypeManager.Default.Resolve<INativeDirectory>(); }
        }

        /// <summary>
        /// Copies the directory from the source path to the destination path, including all subdirectories and files within it.
        /// Any subdirectories or files at the source path with identical names to subdirectories or files at the destination path will not be copied.
        /// </summary>
        /// <param name="sourceDirectoryPath">The path of the directory to be copied.</param>
        /// <param name="destinationDirectoryPath">The path to where the copied directory should be placed.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="sourceDirectoryPath"/> is <c>null</c> -or- when <paramref name="destinationDirectoryPath"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="sourceDirectoryPath"/> is an invalid path -or- when <paramref name="destinationDirectoryPath"/> is an invalid path.</exception>
        /// <exception cref="FileNotFoundException">Thrown when <paramref name="sourceDirectoryPath"/> does not point to an existing directory.</exception>
        public static async Task CopyAsync(string sourceDirectoryPath, string destinationDirectoryPath)
        {
            await Current.CopyAsync(sourceDirectoryPath, destinationDirectoryPath, false);
        }

        /// <summary>
        /// Copies the directory from the source path to the destination path, including all subdirectories and files within it.
        /// </summary>
        /// <param name="sourceDirectoryPath">The path of the directory to be copied.</param>
        /// <param name="destinationDirectoryPath">The path to where the copied directory should be placed.</param>
        /// <param name="overwrite">Whether to overwrite any subdirectories or files at the destination path that have identical names to subdirectories or files at the source path.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="sourceDirectoryPath"/> is <c>null</c> -or- when <paramref name="destinationDirectoryPath"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="sourceDirectoryPath"/> is an invalid path -or- when <paramref name="destinationDirectoryPath"/> is an invalid path.</exception>
        /// <exception cref="FileNotFoundException">Thrown when <paramref name="sourceDirectoryPath"/> does not point to an existing directory.</exception>
        public static async Task CopyAsync(string sourceDirectoryPath, string destinationDirectoryPath, bool overwrite)
        {
            await Current.CopyAsync(sourceDirectoryPath, destinationDirectoryPath, overwrite);
        }

        /// <summary>
        /// Creates a directory at the specified path.
        /// </summary>
        /// <param name="directoryPath">The path at which to create the directory.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="directoryPath"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="directoryPath"/> is an invalid path.</exception>
        public static async Task CreateAsync(string directoryPath)
        {
            await Current.CreateAsync(directoryPath);
        }

        /// <summary>
        /// Deletes the directory at the specified path.
        /// </summary>
        /// <param name="directoryPath">The path of the directory to delete.</param>
        public static async Task DeleteAsync(string directoryPath)
        {
            await Current.DeleteAsync(directoryPath, false);
        }

        /// <summary>
        /// Deletes the directory at the specified path.
        /// </summary>
        /// <param name="directoryPath">The path of the directory to delete.</param>
        /// <param name="recursive">Whether to delete all subdirectories and files within the directory.</param>
        public static async Task DeleteAsync(string directoryPath, bool recursive)
        {
            await Current.DeleteAsync(directoryPath, recursive);
        }

        /// <summary>
        /// Gets the names of the subdirectories in the directory at the specified path.
        /// </summary>
        /// <param name="directoryPath">The path of the directory whose subdirectories are to be retrieved.</param>
        /// <returns>An <see cref="Array"/> containing the names of the subdirectories.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="directoryPath"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="directoryPath"/> is an invalid path..</exception>
        /// <exception cref="FileNotFoundException">Thrown when <paramref name="directoryPath"/> does not point to an existing directory.</exception>
        public static async Task<string[]> GetDirectoriesAsync(string directoryPath)
        {
            return await Current.GetDirectoriesAsync(directoryPath, SearchOption.TopDirectoryOnly);
        }

        /// <summary>
        /// Gets the names of the subdirectories in the directory at the specified path,
        /// optionally getting the names of directories in any subdirectories as well.
        /// </summary>
        /// <param name="directoryPath">The path of the directory whose subdirectories are to be retrieved.</param>
        /// <param name="searchOption">A value indicating whether to search subdirectories or just the top directory.</param>
        /// <returns>An <see cref="Array"/> containing the names of the subdirectories.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="directoryPath"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="directoryPath"/> is an invalid path..</exception>
        /// <exception cref="FileNotFoundException">Thrown when <paramref name="directoryPath"/> does not point to an existing directory.</exception>
        public static async Task<string[]> GetDirectoriesAsync(string directoryPath, SearchOption searchOption)
        {
            return await Current.GetDirectoriesAsync(directoryPath, searchOption);
        }

        /// <summary>
        /// Gets the names of the files in the directory at the specified path.
        /// </summary>
        /// <param name="directoryPath">The path of the directory whose files are to be retrieved.</param>
        /// <returns>An <see cref="Array"/> containing the names of the files.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="directoryPath"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="directoryPath"/> is an invalid path..</exception>
        /// <exception cref="FileNotFoundException">Thrown when <paramref name="directoryPath"/> does not point to an existing directory.</exception>
        public static async Task<string[]> GetFilesAsync(string directoryPath)
        {
            return await Current.GetFilesAsync(directoryPath, SearchOption.TopDirectoryOnly);
        }

        /// <summary>
        /// Gets the names of the files in the directory at the specified path,
        /// optionally getting the names of files in any subdirectories as well.
        /// </summary>
        /// <param name="directoryPath">The path of the directory whose files are to be retrieved.</param>
        /// <param name="searchOption">A value indicating whether to search subdirectories or just the top directory.</param>
        /// <returns>An <see cref="Array"/> containing the names of the files.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="directoryPath"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="directoryPath"/> is an invalid path..</exception>
        /// <exception cref="FileNotFoundException">Thrown when <paramref name="directoryPath"/> does not point to an existing directory.</exception>
        public static async Task<string[]> GetFilesAsync(string directoryPath, SearchOption searchOption)
        {
            return await Current.GetFilesAsync(directoryPath, searchOption);
        }

        /// <summary>
        /// Moves the directory at the source path to the destination path.
        /// Any subdirectories or files at the source path with identical names to subdirectories or files at the destination path will not be moved.
        /// </summary>
        /// <param name="sourceDirectoryPath">The path of the directory to be moved.</param>
        /// <param name="destinationDirectoryPath">The path to where the directory should be moved.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="sourceDirectoryPath"/> is <c>null</c> -or- when <paramref name="destinationDirectoryPath"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="sourceDirectoryPath"/> is an invalid path -or- when <paramref name="destinationDirectoryPath"/> is an invalid path.</exception>
        /// <exception cref="FileNotFoundException">Thrown when <paramref name="sourceDirectoryPath"/> does not point to an existing directory.</exception>
        public static async Task MoveAsync(string sourceDirectoryPath, string destinationDirectoryPath)
        {
            await Current.MoveAsync(sourceDirectoryPath, destinationDirectoryPath, false);
        }

        /// <summary>
        /// Moves the directory at the source path to the destination path.
        /// </summary>
        /// <param name="sourceDirectoryPath">The path of the directory to be moved.</param>
        /// <param name="destinationDirectoryPath">The path to where the directory should be moved.</param>
        /// <param name="overwrite">Whether to overwrite any subdirectories or files at the destination path that have identical names to subdirectories or files at the source path.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="sourceDirectoryPath"/> is <c>null</c> -or- when <paramref name="destinationDirectoryPath"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="sourceDirectoryPath"/> is an invalid path -or- when <paramref name="destinationDirectoryPath"/> is an invalid path.</exception>
        /// <exception cref="FileNotFoundException">Thrown when <paramref name="sourceDirectoryPath"/> does not point to an existing directory.</exception>
        public static async Task MoveAsync(string sourceDirectoryPath, string destinationDirectoryPath, bool overwrite)
        {
            await Current.MoveAsync(sourceDirectoryPath, destinationDirectoryPath, overwrite);
        }
    }
}
