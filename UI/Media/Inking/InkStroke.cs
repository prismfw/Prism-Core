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
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Prism.Native;
using Prism.UI.Controls;

namespace Prism.UI.Media.Inking
{
    /// <summary>
    /// Represents a stroke that is rendered on an <see cref="InkCanvas"/>.
    /// </summary>
    public sealed class InkStroke : FrameworkObject
    {
        /// <summary>
        /// Gets a rectangle that encompasses all of the points in the ink stroke.
        /// </summary>
        public Rectangle BoundingBox
        {
            get { return nativeObject.BoundingBox; }
        }

        /// <summary>
        /// Gets or sets the drawing attributes for the ink stroke.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", Justification = "Exception parameter refers to property name for easier understanding of invalid value.")]
        public InkDrawingAttributes DrawingAttributes
        {
            get { return drawingAttributes; }
            set
            {
                if (value != drawingAttributes)
                {
                    if (drawingAttributes != null)
                    {
                        drawingAttributes.PropertyChanged -= OnDrawingAttributeChanged;
                    }

                    if (value == null)
                    {
                        throw new ArgumentNullException(nameof(DrawingAttributes));
                    }

                    drawingAttributes = value;
                    nativeObject.UpdateDrawingAttributes(drawingAttributes);
                    drawingAttributes.PropertyChanged -= OnDrawingAttributeChanged;
                    drawingAttributes.PropertyChanged += OnDrawingAttributeChanged;
                }
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private InkDrawingAttributes drawingAttributes;

        /// <summary>
        /// Gets a collection of points that make up the ink stroke.
        /// </summary>
        public IEnumerable<Point> Points
        {
            get { return nativeObject.Points; }
        }

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        // this field is to avoid casting
        private readonly INativeInkStroke nativeObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="InkStroke"/> class.
        /// </summary>
        /// <param name="points">A collection of <see cref="Point"/> objects that defines the shape of the stroke.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="points"/> is <c>null</c>.</exception>
        public InkStroke(IEnumerable<Point> points)
            : base(typeof(INativeInkStroke), null, new ResolveParameter(nameof(points), points, false))
        {
            nativeObject = ObjectRetriever.GetNativeObject(this) as INativeInkStroke;
            if (nativeObject == null)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.Strings.TypeMustResolveToType, typeof(INativeInkStroke).FullName, typeof(INativeInkStroke).FullName));
            }

            DrawingAttributes = new InkDrawingAttributes() { Color = Colors.Black };
        }

        internal InkStroke(INativeInkStroke nativeStroke)
            : base(nativeStroke)
        {
            nativeObject = nativeStroke;

            drawingAttributes = nativeObject.CopyDrawingAttributes();
            drawingAttributes.PropertyChanged -= OnDrawingAttributeChanged;
            drawingAttributes.PropertyChanged += OnDrawingAttributeChanged;
        }

        /// <summary>
        /// Returns a deep-copy clone of this instance.
        /// </summary>
        /// <returns>The cloned object as an <see cref="InkStroke"/> instance.</returns>
        public InkStroke Clone()
        {
            return new InkStroke(nativeObject.Clone());
        }

        private void OnDrawingAttributeChanged(object sender, PropertyChangedEventArgs e)
        {
            nativeObject.UpdateDrawingAttributes(drawingAttributes);
        }
    }
}
