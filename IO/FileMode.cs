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


namespace Prism.IO
{
    /// <summary>
    /// Describes the manner in which a file should be opened.
    /// </summary>
    public enum FileMode
    {
        /// <summary>
        /// A new file should be created.  Any existing file should be replaced.
        /// </summary>
        Create,
        /// <summary>
        /// The existing file should be opened.  If no such file exists, an exception should be thrown.
        /// </summary>
        Open,
        /// <summary>
        /// The existing file should be opened.  If no such file exists, a new file should be created.
        /// </summary>
        OpenOrCreate
    }
}
