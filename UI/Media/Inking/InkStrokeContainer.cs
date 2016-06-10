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
using System.Linq;
using Prism.Native;
using Prism.UI.Controls;

#if !DEBUG
using System.Diagnostics;
#endif

namespace Prism.UI.Media.Inking
{
    /// <summary>
    /// Represents a container for <see cref="InkStroke"/> objects on an <see cref="InkCanvas"/> element.
    /// </summary>
    public sealed class InkStrokeContainer
    {
#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private readonly INativeInkCanvas nativeObject;

        internal InkStrokeContainer(INativeInkCanvas parent)
        {
            if (parent == null)
            {
                throw new ArgumentNullException(nameof(parent));
            }

            nativeObject = parent;
        }

        /// <summary>
        /// Adds the specified <see cref="InkStroke"/> to the canvas.
        /// </summary>
        /// <param name="stroke">The ink stroke to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="stroke"/> is <c>null</c>.</exception>
        public void AddStroke(InkStroke stroke)
        {
            if (stroke == null)
            {
                throw new ArgumentNullException(nameof(stroke));
            }

            nativeObject.AddStroke((INativeInkStroke)ObjectRetriever.GetNativeObject(stroke));
        }

        /// <summary>
        /// Adds the specified <see cref="InkStroke"/> instances to the canvas.
        /// </summary>
        /// <param name="strokes">The ink strokes to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="strokes"/> is <c>null</c>.</exception>
        public void AddStrokes(IEnumerable<InkStroke> strokes)
        {
            if (strokes == null)
            {
                throw new ArgumentNullException(nameof(strokes));
            }

            nativeObject.AddStrokes(strokes.Select(s => (INativeInkStroke)ObjectRetriever.GetNativeObject(s)));
        }

        /// <summary>
        /// Removes all ink strokes from the canvas.
        /// </summary>
        public void Clear()
        {
            nativeObject.ClearStrokes();
        }

        /// <summary>
        /// Gets the ink strokes that are currently on the canvas.
        /// </summary>
        /// <returns>An <see cref="IEnumerable&lt;T&gt;"/> that contains the ink strokes on the canvas.</returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Performance may be less than what is expected of a property.")]
        public IEnumerable<InkStroke> GetStrokes()
        {
            return nativeObject.Strokes.Select(s =>
            {
                var stroke = ObjectRetriever.GetAgnosticObject(s);
                return stroke as InkStroke ?? new InkStroke(s);
            });
        }
    }
}
