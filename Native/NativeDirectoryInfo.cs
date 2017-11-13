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
using Prism.IO;

namespace Prism.Native
{
    /// <summary>
    /// Defines a directory info object that is native to a particular platform.
    /// These objects are meant to be paired with platform-agnostic <see cref="DirectoryInfo"/> objects.
    /// </summary>
    public interface INativeDirectoryInfo
    {
        /// <summary>
        /// Gets or sets the attributes of the directory.
        /// </summary>
        FileAttributes Attributes { get; set; }

        /// <summary>
        /// Gets the date and time that the directory was created.
        /// </summary>
        DateTime CreationTime { get; }

        /// <summary>
        /// Gets the date and time, in coordinated universal time (UTC), that the directory was created.
        /// </summary>
        DateTime CreationTimeUtc { get; }

        /// <summary>
        /// Gets a value indicating whether the directory exists.
        /// </summary>
        bool Exists { get; }

        /// <summary>
        /// Gets the date and time that the directory was last accessed.
        /// </summary>
        DateTime LastAccessTime { get; }

        /// <summary>
        /// Gets the date and time, in coordinated universal time (UTC), that the directory was last accessed.
        /// </summary>
        DateTime LastAccessTimeUtc { get; }

        /// <summary>
        /// Gets the date and time that the directory was last modified.
        /// </summary>
        DateTime LastWriteTime { get; }

        /// <summary>
        /// Gets the date and time, in coordinated universal time (UTC), that the directory was last modified.
        /// </summary>
        DateTime LastWriteTimeUtc { get; }

        /// <summary>
        /// Gets the name of the directory.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the full path to the directory.
        /// </summary>
        string Path { get; }

        /// <summary>
        /// Gets information about the subdirectories within the current directory,
        /// optionally getting information about directories in any subdirectories as well.
        /// </summary>
        /// <param name="searchOption">A value indicating whether to search subdirectories or just the top directory.</param>
        /// <returns>An <see cref="Array"/> containing the directory information.</returns>
        Task<INativeDirectoryInfo[]> GetDirectoriesAsync(SearchOption searchOption);

        /// <summary>
        /// Gets information about the files in the current directory,
        /// optionally getting information about files in any subdirectories as well.
        /// </summary>
        /// <param name="searchOption">A value indicating whether to search subdirectories or just the top directory.</param>
        /// <returns>An <see cref="Array"/> containing the file information.</returns>
        Task<INativeFileInfo[]> GetFilesAsync(SearchOption searchOption);

        /// <summary>
        /// Gets information about the parent directory in which the current directory exists.
        /// </summary>
        /// <returns>The directory information.</returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Asynchronous nature of method makes property inappropriate.")]
        Task<INativeDirectoryInfo> GetParentAsync();
    }
}
