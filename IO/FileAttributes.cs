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

namespace Prism.IO
{
    /// <summary>
    /// Describes the attributes of a file or directory.
    /// </summary>
    [Flags]
    public enum FileAttributes
    {
        /// <summary>
        /// The file is read-only.
        /// </summary>
        ReadOnly = 1,
        /// <summary>
        /// The file is hidden, and thus is not included in an ordinary directory listing.
        /// </summary>
        Hidden = 2,
        /// <summary>
        /// The file is a system file. That is, the file is part of the operating system
        /// or is used exclusively by the operating system.
        /// </summary>
        System = 4,
        /// <summary>
        /// The file is a directory.
        /// </summary>
        Directory = 16,
        /// <summary>
        /// The file is a candidate for backup or removal.
        /// </summary>
        Archive = 32,
        /// <summary>
        /// Reserved for future use.
        /// </summary>
        Device = 64,
        /// <summary>
        /// The file is a standard file that has no special attributes. This attribute is
        /// valid only if it is used alone.
        /// </summary>
        Normal = 128,
        /// <summary>
        /// The file is temporary. A temporary file contains data that is needed while an
        /// application is executing but is not needed after the application is finished.
        /// File systems try to keep all the data in memory for quicker access rather than
        /// flushing the data back to mass storage. A temporary file should be deleted by
        /// the application as soon as it is no longer needed.
        /// </summary>
        Temporary = 256,
        /// <summary>
        /// The file is a sparse file. Sparse files are typically large files whose data
        /// consists of mostly zeros.
        /// </summary>
        SparseFile = 512,
        /// <summary>
        /// The file contains a reparse point, which is a block of user-defined data associated
        /// with a file or a directory.
        /// </summary>
        ReparsePoint = 1024,
        /// <summary>
        /// The file is compressed.
        /// </summary>
        Compressed = 2048,
        /// <summary>
        /// The file is offline. The data of the file is not immediately available.
        /// </summary>
        Offline = 4096,
        /// <summary>
        /// The file will not be indexed by the operating system's content indexing service.
        /// </summary>
        NotContentIndexed = 8192,
        /// <summary>
        /// The file or directory is encrypted. For a file, this means that all data in the
        /// file is encrypted. For a directory, this means that encryption is the default
        /// for newly created files and directories.
        /// </summary>
        Encrypted = 16384,
        /// <summary>
        /// The file or directory includes data integrity support. When this value is applied
        /// to a file, all data streams in the file have integrity support. When this value
        /// is applied to a directory, all new files and subdirectories within that directory,
        /// by default, include integrity support.
        /// </summary>
        IntegrityStream = 32768,
        /// <summary>
        /// The file or directory is excluded from the data integrity scan. When this value
        /// is applied to a directory, by default, all new files and subdirectories within
        /// that directory are excluded from data integrity.
        /// </summary>
        NoScrubData = 131072
    }
}
