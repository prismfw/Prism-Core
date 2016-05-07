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


namespace Prism.Utilities
{
    /// <summary>
    /// Describes the available logging levels.
    /// </summary>
    public enum LoggingLevel
    {
        /// <summary>
        /// Log trace messages and above.  This is the most verbose level.
        /// </summary>
        Trace = 0,
        /// <summary>
        /// Log debug messages and above.
        /// </summary>
        Debug = 0x33,
        /// <summary>
        /// Log info messages and above.
        /// </summary>
        Info = 0x66,
        /// <summary>
        /// Log warning messages and above.
        /// </summary>
        Warn = 0x99,
        /// <summary>
        /// Log error messages and above.
        /// </summary>
        Error = 0xCC,
        /// <summary>
        /// Log fatal error messages only.  This is the least verbose level excluding <see cref="Off"/>.
        /// </summary>
        Fatal = 0xFF,
        /// <summary>
        /// Do not perform any logging.
        /// </summary>
        Off = 0x100
    }
}
