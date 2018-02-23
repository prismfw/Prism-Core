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


namespace Prism.UI
{
    /// <summary>
    /// Describes the available action keys for soft keyboards.
    /// </summary>
    public enum ActionKeyType
    {
        /// <summary>
        /// The default action key.  This is most commonly a "Return" key.
        /// </summary>
        Default = 0,
        /// <summary>
        /// A "Done" key.  If this option is not available on the current platform, <see cref="Default"/> will be used instead.
        /// </summary>
        Done = 1,
        /// <summary>
        /// A "Go" key.  If this option is not available on the current platform, <see cref="Default"/> will be used instead.
        /// </summary>
        Go = 2,
        /// <summary>
        /// A "Next" key.  If this option is not available on the current platform, <see cref="Default"/> will be used instead.
        /// </summary>
        Next = 3,
        /// <summary>
        /// A "Search" key.  If this option is not available on the current platform, <see cref="Default"/> will be used instead.
        /// </summary>
        Search = 4
    }
}
