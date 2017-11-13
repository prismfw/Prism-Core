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
using Prism.IO;

namespace Prism.Native
{
    /// <summary>
    /// Defines a file info object that is native to a particular platform.
    /// These objects are meant to be paired with platform-agnostic <see cref="FileInfo"/> objects.
    /// </summary>
    public interface INativeFileInfo
    {
        /// <summary>
        /// Gets or sets the attributes of the file.
        /// </summary>
        FileAttributes Attributes { get; set; }

        /// <summary>
        /// Gets the date and time that the file was created.
        /// </summary>
        DateTime CreationTime { get; }

        /// <summary>
        /// Gets the date and time, in coordinated universal time (UTC), that the file was created.
        /// </summary>
        DateTime CreationTimeUtc { get; }

        /// <summary>
        /// Gets the directory in which the file exists.
        /// </summary>
        INativeDirectoryInfo Directory { get; }
        
        /// <summary>
        /// Gets a value indicating whether the file exists.
        /// </summary>
        bool Exists { get; }

        /// <summary>
        /// Gets the extension of the file.
        /// </summary>
        string Extension { get; }

        /// <summary>
        /// Gets a value indicating whether the file is read-only.
        /// </summary>
        bool IsReadOnly { get; }

        /// <summary>
        /// Gets the date and time that the file was last accessed.
        /// </summary>
        DateTime LastAccessTime { get; }

        /// <summary>
        /// Gets the date and time, in coordinated universal time (UTC), that the file was last accessed.
        /// </summary>
        DateTime LastAccessTimeUtc { get; }

        /// <summary>
        /// Gets the date and time that the file was last modified.
        /// </summary>
        DateTime LastWriteTime { get; }

        /// <summary>
        /// Gets the date and time, in coordinated universal time (UTC), that the file was last modified.
        /// </summary>
        DateTime LastWriteTimeUtc { get; }

        /// <summary>
        /// Gets the size of the file, in bytes.
        /// </summary>
        long Length { get; }

        /// <summary>
        /// Gets the name of the file.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the full path to the file.
        /// </summary>
        string Path { get; }
    }
}
