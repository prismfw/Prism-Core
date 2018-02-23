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

namespace Prism
{
    /// <summary>
    /// The exception that is thrown when there is a failure in resolving a type from the IoC container.
    /// </summary>
    public class TypeResolutionException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TypeResolutionException"/> class.
        /// </summary>
        public TypeResolutionException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeResolutionException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public TypeResolutionException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeResolutionException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a
        /// <c>null</c> reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public TypeResolutionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
