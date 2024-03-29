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
using Prism.IO;

namespace Prism.Native
{
    /// <summary>
    /// Defines an object that provides management of directories on a native file system.
    /// </summary>
    [CoreBehavior(CoreBehaviors.ExpectsSingleton)]
    public interface INativeDirectory
    {
        /// <summary>
        /// Gets the directory path to the folder that contains the application's bundled assets.
        /// </summary>
        string AssetDirectoryPath { get; }

        /// <summary>
        /// Gets the directory path to a folder for storing persisted application data that is specific to the user.
        /// </summary>
        string DataDirectoryPath { get; }

        /// <summary>
        /// Gets the character that is used to separate directories.
        /// </summary>
        char SeparatorChar { get; }

        /// <summary>
        /// Copies the directory from the source path to the destination path, including all subdirectories and files within it.
        /// </summary>
        /// <param name="sourceDirectoryPath">The path of the directory to be copied.</param>
        /// <param name="destinationDirectoryPath">The path to where the copied directory should be placed.</param>
        /// <param name="overwrite">Whether to overwrite any subdirectories or files at the destination path that have identical names to subdirectories or files at the source path.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="sourceDirectoryPath"/> is <c>null</c> -or- when <paramref name="destinationDirectoryPath"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="sourceDirectoryPath"/> is an invalid path -or- when <paramref name="destinationDirectoryPath"/> is an invalid path.</exception>
        /// <exception cref="FileNotFoundException">Thrown when <paramref name="sourceDirectoryPath"/> does not point to an existing directory.</exception>
        Task CopyAsync(string sourceDirectoryPath, string destinationDirectoryPath, bool overwrite);

        /// <summary>
        /// Creates a directory at the specified path.
        /// </summary>
        /// <param name="directoryPath">The path at which to create the directory.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="directoryPath"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="directoryPath"/> is an invalid path.</exception>
        Task CreateAsync(string directoryPath);

        /// <summary>
        /// Deletes the directory at the specified path.
        /// </summary>
        /// <param name="directoryPath">The path of the directory to delete.</param>
        /// <param name="recursive">Whether to delete all subdirectories and files within the directory.</param>
        Task DeleteAsync(string directoryPath, bool recursive);

        /// <summary>
        /// Gets the names of the subdirectories in the directory at the specified path,
        /// optionally getting the names of directories in any subdirectories as well.
        /// </summary>
        /// <param name="directoryPath">The path of the directory whose subdirectories are to be retrieved.</param>
        /// <param name="searchOption">A value indicating whether to search subdirectories or just the top directory.</param>
        /// <returns>An <see cref="Array"/> containing the names of the subdirectories.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="directoryPath"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="directoryPath"/> is an invalid path.</exception>
        /// <exception cref="FileNotFoundException">Thrown when <paramref name="directoryPath"/> does not point to an existing directory.</exception>
        Task<string[]> GetDirectoriesAsync(string directoryPath, SearchOption searchOption);

        /// <summary>
        /// Gets the names of the files in the directory at the specified path,
        /// optionally getting the names of files in any subdirectories as well.
        /// </summary>
        /// <param name="directoryPath">The path of the directory whose files are to be retrieved.</param>
        /// <param name="searchOption">A value indicating whether to search subdirectories or just the top directory.</param>
        /// <returns>An <see cref="Array"/> containing the names of the files.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="directoryPath"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="directoryPath"/> is an invalid path.</exception>
        /// <exception cref="FileNotFoundException">Thrown when <paramref name="directoryPath"/> does not point to an existing directory.</exception>
        Task<string[]> GetFilesAsync(string directoryPath, SearchOption searchOption);

        /// <summary>
        /// Gets the number of free bytes that are available on the drive that contains the directory at the specified path.
        /// </summary>
        /// <param name="directoryPath">The path of a directory on the drive.  If <c>null</c>, the current drive is used.</param>
        /// <returns>A <see cref="long"/> representing the number of free bytes on the drive.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="directoryPath"/> is an invalid path.</exception>
        /// <exception cref="FileNotFoundException">Thrown when <paramref name="directoryPath"/> does not point to an existing directory.</exception>
        Task<long> GetFreeBytesAsync(string directoryPath);

        /// <summary>
        /// Gets information about the specified system directory.
        /// </summary>
        /// <param name="directory">The system directory whose information is to be retrieved.</param>
        /// <returns>An <see cref="INativeDirectoryInfo"/> containing information about the system directory.</returns>
        Task<INativeDirectoryInfo> GetSystemDirectoryInfoAsync(SystemDirectory directory);

        /// <summary>
        /// Gets the total number of bytes on the drive that contains the directory at the specified path.
        /// </summary>
        /// <param name="directoryPath">The path of a directory on the drive.  If <c>null</c>, the current drive is used.</param>
        /// <returns>A <see cref="long"/> representing the total number of bytes on the drive.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="directoryPath"/> is an invalid path.</exception>
        /// <exception cref="FileNotFoundException">Thrown when <paramref name="directoryPath"/> does not point to an existing directory.</exception>
        Task<long> GetTotalBytesAsync(string directoryPath);

        /// <summary>
        /// Moves the directory at the source path to the destination path.
        /// </summary>
        /// <param name="sourceDirectoryPath">The path of the directory to be moved.</param>
        /// <param name="destinationDirectoryPath">The path to where the directory should be moved.</param>
        /// <param name="overwrite">Whether to overwrite any subdirectories or files at the destination path that have identical names to subdirectories or files at the source path.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="sourceDirectoryPath"/> is <c>null</c> -or- when <paramref name="destinationDirectoryPath"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="sourceDirectoryPath"/> is an invalid path -or- when <paramref name="destinationDirectoryPath"/> is an invalid path.</exception>
        /// <exception cref="FileNotFoundException">Thrown when <paramref name="sourceDirectoryPath"/> does not point to an existing directory.</exception>
        Task MoveAsync(string sourceDirectoryPath, string destinationDirectoryPath, bool overwrite);
    }
}
