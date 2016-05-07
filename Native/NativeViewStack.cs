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


using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Prism.UI;

namespace Prism.Native
{
    /// <summary>
    /// Defines a navigable stack of views that is native to a particular platform.
    /// These objects are meant to be paired with platform-agnostic <see cref="ViewStack"/> objects.
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix", Justification = "Class behavior is consistent with that of a stack.")]
    public interface INativeViewStack : INativeVisual
    {
        /// <summary>
        /// Occurs when a view is being popped off of the view stack.
        /// </summary>
        event EventHandler<NativeViewStackPoppingEventArgs> Popping;

        /// <summary>
        /// Gets the view that is currently on top of the stack.
        /// </summary>
        object CurrentView { get; }

        /// <summary>
        /// Gets the header for the view stack.
        /// </summary>
        INativeViewStackHeader Header { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the header is hidden.
        /// </summary>
        bool IsHeaderHidden { get; set; }

        /// <summary>
        /// Gets a collection of the views that are currently a part of the stack.
        /// </summary>
        IEnumerable<object> Views { get; }

        /// <summary>
        /// Inserts the specified view into the stack at the specified index.
        /// </summary>
        /// <param name="view">The view to be inserted.</param>
        /// <param name="index">The zero-based index of the location in the stack in which to insert the view.</param>
        /// <param name="animate">Whether to use any system-defined transition animation.</param>
        void InsertView(object view, int index, Animate animate);

        /// <summary>
        /// Removes the top view from the stack.
        /// </summary>
        /// <param name="animate">Whether to use any system-defined transition animation.</param>
        /// <returns>The view that was removed from the stack.</returns>
        object PopView(Animate animate);

        /// <summary>
        /// Removes every view from the stack except for the root view.
        /// </summary>
        /// <param name="animate">Whether to use any system-defined transition animation.</param>
        /// <returns>An <see cref="Array"/> containing the views that were removed from the stack.</returns>
        object[] PopToRoot(Animate animate);

        /// <summary>
        /// Removes from the stack every view on top of the specified view.
        /// </summary>
        /// <param name="view">The view to pop to.</param>
        /// <param name="animate">Whether to use any system-defined transition animation.</param>
        /// <returns>An <see cref="Array"/> containing the views that were removed from the stack.</returns>
        object[] PopToView(object view, Animate animate);

        /// <summary>
        /// Pushes the specified view onto the top of the stack.
        /// </summary>
        /// <param name="view">The view to push to the top of the stack.</param>
        /// <param name="animate">Whether to use any system-defined transition animation.</param>
        void PushView(object view, Animate animate);

        /// <summary>
        /// Replaces a view that is currently on the stack with the specified view.
        /// </summary>
        /// <param name="oldView">The view to be replaced.</param>
        /// <param name="newView">The view with which to replace the old view.</param>
        /// <param name="animate">Whether to use any system-defined transition animation.</param>
        void ReplaceView(object oldView, object newView, Animate animate);
    }
}
