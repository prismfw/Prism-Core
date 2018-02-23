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


using Prism.Input;

namespace Prism.Native
{
    /// <summary>
    /// Defines a gesture recognizer that is native to a particular platform.
    /// These objects are meant to be paired with platform-agnostic <see cref="GestureRecognizer"/> objects.
    /// </summary>
    public interface INativeGestureRecognizer : INativeObject
    {
        /// <summary>
        /// Removes the specified object as the target of the gesture recognizer.
        /// </summary>
        /// <param name="target">The object to clear as the target.</param>
        void ClearTarget(object target);

        /// <summary>
        /// Sets the specified object as the target of the gesture recognizer.
        /// </summary>
        /// <param name="target">The object to set as the target.</param>
        void SetTarget(object target);
    }
}
