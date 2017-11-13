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
using System.Globalization;
using System.Threading.Tasks;
using Prism.Native;

#if !DEBUG
using System.Diagnostics;
#endif

namespace Prism.IO
{
    /// <summary>
    /// Contains information about a directory that exists on disk.
    /// </summary>
    public sealed class DirectoryInfo
    {
        /// <summary>
        /// Gets or sets the attributes of the directory.
        /// </summary>
        public FileAttributes Attributes
        {
            get { return nativeObject.Attributes; }
            set { nativeObject.Attributes = value; }
        }

        /// <summary>
        /// Gets the date and time that the directory was created.
        /// </summary>
        public DateTime CreationTime
        {
            get { return nativeObject.CreationTime; }
        }

        /// <summary>
        /// Gets the date and time, in coordinated universal time (UTC), that the directory was created.
        /// </summary>
        public DateTime CreationTimeUtc
        {
            get { return nativeObject.CreationTimeUtc; }
        }

        /// <summary>
        /// Gets a value indicating whether the directory exists.
        /// </summary>
        public bool Exists
        {
            get { return nativeObject.Exists; }
        }

        /// <summary>
        /// Gets the date and time that the directory was last accessed.
        /// </summary>
        public DateTime LastAccessTime
        {
            get { return nativeObject.LastAccessTime; }
        }

        /// <summary>
        /// Gets the date and time, in coordinated universal time (UTC), that the directory was last accessed.
        /// </summary>
        public DateTime LastAccessTimeUtc
        {
            get { return nativeObject.LastAccessTimeUtc; }
        }

        /// <summary>
        /// Gets the date and time that the directory was last modified.
        /// </summary>
        public DateTime LastWriteTime
        {
            get { return nativeObject.LastWriteTime; }
        }

        /// <summary>
        /// Gets the date and time, in coordinated universal time (UTC), that the directory was last modified.
        /// </summary>
        public DateTime LastWriteTimeUtc
        {
            get { return nativeObject.LastWriteTimeUtc; }
        }

        /// <summary>
        /// Gets the name of the directory.
        /// </summary>
        public string Name
        {
            get { return nativeObject.Name; }
        }

        /// <summary>
        /// Gets the full path to the directory.
        /// </summary>
        public string Path
        {
            get { return nativeObject.Path; }
        }

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private readonly INativeDirectoryInfo nativeObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="DirectoryInfo"/> class.
        /// </summary>
        /// <param name="directoryPath">The path to the directory.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="directoryPath"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="directoryPath"/> is an invalid path.</exception>
        public DirectoryInfo(string directoryPath)
        {
            if (directoryPath == null)
            {
                throw new ArgumentNullException(nameof(directoryPath));
            }

            nativeObject = TypeManager.Default.Resolve<INativeDirectoryInfo>(new object[] { directoryPath },
                TypeResolutionOptions.UseFuzzyNameResolution | TypeResolutionOptions.UseFuzzyParameterResolution);

            if (nativeObject == null)
            {
                throw new TypeResolutionException(string.Format(CultureInfo.CurrentCulture, Resources.Strings.TypeCouldNotBeResolved, typeof(INativeDirectoryInfo).FullName));
            }

            ObjectRetriever.SetPair(this, nativeObject);
        }

        internal DirectoryInfo(INativeDirectoryInfo nativeObject)
        {
            this.nativeObject = nativeObject;
        }

        /// <summary>
        /// Gets information about the subdirectories within the current directory.
        /// </summary>
        /// <returns>An <see cref="Array"/> containing the directory information.</returns>
        public async Task<DirectoryInfo[]> GetDirectoriesAsync()
        {
            return await GetDirectoriesAsync(SearchOption.TopDirectoryOnly);
        }

        /// <summary>
        /// Gets information about the subdirectories within the current directory,
        /// optionally getting information about directories in any subdirectories as well.
        /// </summary>
        /// <param name="searchOption">A value indicating whether to search subdirectories or just the top directory.</param>
        /// <returns>An <see cref="Array"/> containing the directory information.</returns>
        public async Task<DirectoryInfo[]> GetDirectoriesAsync(SearchOption searchOption)
        {
            var directories = await nativeObject.GetDirectoriesAsync(searchOption);
            var retVal = new DirectoryInfo[directories.Length];
            for (int i = 0; i < retVal.Length; i++)
            {
                retVal[i] = new DirectoryInfo(directories[i]);
            }

            return retVal;
        }

        /// <summary>
        /// Gets information about the files in the current directory.
        /// </summary>
        /// <returns>An <see cref="Array"/> containing the file information.</returns>
        public async Task<FileInfo[]> GetFilesAsync()
        {
            return await GetFilesAsync(SearchOption.TopDirectoryOnly);
        }

        /// <summary>
        /// Gets information about the files in the current directory,
        /// optionally getting information about files in any subdirectories as well.
        /// </summary>
        /// <param name="searchOption">A value indicating whether to search subdirectories or just the top directory.</param>
        /// <returns>An <see cref="Array"/> containing the file information.</returns>
        public async Task<FileInfo[]> GetFilesAsync(SearchOption searchOption)
        {
            var files = await nativeObject.GetFilesAsync(searchOption);
            var retVal = new FileInfo[files.Length];
            for (int i = 0; i < retVal.Length; i++)
            {
                retVal[i] = new FileInfo(files[i]);
            }

            return retVal;
        }

        /// <summary>
        /// Gets information about the parent directory in which the current directory exists.
        /// </summary>
        /// <returns>The directory information.</returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Asynchronous nature of method makes property inappropriate.")]
        public async Task<DirectoryInfo> GetParentAsync()
        {
            var nparent = await nativeObject.GetParentAsync();
            return nparent == null ? null : new DirectoryInfo(nparent);
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents the current <see cref="DirectoryInfo"/>.
        /// </summary>
        /// <returns>A <see cref="string"/> that represents the current <see cref="DirectoryInfo"/>.</returns>
        public override string ToString()
        {
            return nativeObject.Path;
        }
    }
}
