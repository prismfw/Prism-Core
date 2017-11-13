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
using System.Diagnostics;
using System.Globalization;
using Prism.Native;

namespace Prism.IO
{
    /// <summary>
    /// Contains information about a file that exists on disk.
    /// </summary>
    public sealed class FileInfo
    {
        /// <summary>
        /// Gets or sets the attributes of the file.
        /// </summary>
        public FileAttributes Attributes
        {
            get { return nativeObject.Attributes; }
            set { nativeObject.Attributes = value; }
        }

        /// <summary>
        /// Gets the date and time that the file was created.
        /// </summary>
        public DateTime CreationTime
        {
            get { return nativeObject.CreationTime; }
        }

        /// <summary>
        /// Gets the date and time, in coordinated universal time (UTC), that the file was created.
        /// </summary>
        public DateTime CreationTimeUtc
        {
            get { return nativeObject.CreationTimeUtc; }
        }

        /// <summary>
        /// Gets the directory in which the file exists.
        /// </summary>
        public DirectoryInfo Directory
        {
            get
            {
                if (directory != null)
                {
                    return directory;
                }

                var ndir = nativeObject.Directory;
                return ndir == null ? null : (directory = new DirectoryInfo(ndir));
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DirectoryInfo directory;

        /// <summary>
        /// Gets a value indicating whether the file exists.
        /// </summary>
        public bool Exists
        {
            get { return nativeObject.Exists; }
        }

        /// <summary>
        /// Gets the extension of the file.
        /// </summary>
        public string Extension
        {
            get { return nativeObject.Extension; }
        }

        /// <summary>
        /// Gets a value indicating whether the file is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return nativeObject.IsReadOnly; }
        }

        /// <summary>
        /// Gets the date and time that the file was last accessed.
        /// </summary>
        public DateTime LastAccessTime
        {
            get { return nativeObject.LastAccessTime; }
        }

        /// <summary>
        /// Gets the date and time, in coordinated universal time (UTC), that the file was last accessed.
        /// </summary>
        public DateTime LastAccessTimeUtc
        {
            get { return nativeObject.LastAccessTimeUtc; }
        }

        /// <summary>
        /// Gets the date and time that the file was last modified.
        /// </summary>
        public DateTime LastWriteTime
        {
            get { return nativeObject.LastWriteTime; }
        }

        /// <summary>
        /// Gets the date and time, in coordinated universal time (UTC), that the file was last modified.
        /// </summary>
        public DateTime LastWriteTimeUtc
        {
            get { return nativeObject.LastWriteTimeUtc; }
        }

        /// <summary>
        /// Gets the size of the file, in bytes.
        /// </summary>
        public long Length
        {
            get { return nativeObject.Length; }
        }

        /// <summary>
        /// Gets the name of the file.
        /// </summary>
        public string Name
        {
            get { return nativeObject.Name; }
        }

        /// <summary>
        /// Gets the full path to the file.
        /// </summary>
        public string Path
        {
            get { return nativeObject.Path; }
        }

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private readonly INativeFileInfo nativeObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileInfo"/> class.
        /// </summary>
        /// <param name="filePath">The path to the file.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="filePath"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="filePath"/> is an invalid path.</exception>
        public FileInfo(string filePath)
        {
            if (filePath == null)
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            nativeObject = TypeManager.Default.Resolve<INativeFileInfo>(new object[] { filePath },
                TypeResolutionOptions.UseFuzzyNameResolution | TypeResolutionOptions.UseFuzzyParameterResolution);

            if (nativeObject == null)
            {
                throw new TypeResolutionException(string.Format(CultureInfo.CurrentCulture, Resources.Strings.TypeCouldNotBeResolved, typeof(INativeFileInfo).FullName));
            }

            ObjectRetriever.SetPair(this, nativeObject);
        }

        internal FileInfo(INativeFileInfo nativeObject)
        {
            this.nativeObject = nativeObject;
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents the current <see cref="FileInfo"/>.
        /// </summary>
        /// <returns>A <see cref="string"/> that represents the current <see cref="FileInfo"/>.</returns>
        public override string ToString()
        {
            return nativeObject.Path;
        }
    }
}
