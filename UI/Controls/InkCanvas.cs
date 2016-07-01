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
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Prism.Native;
using Prism.UI.Media.Inking;

namespace Prism.UI.Controls
{
    /// <summary>
    /// Represents a UI element that renders pointer input as ink strokes.
    /// </summary>
    public class InkCanvas : Element
    {
        #region Property Descriptors
        /// <summary>
        /// Describes the <see cref="P:DefaultDrawingAttributes"/> property.  This field is read-only.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "PropertyDescriptor is immutable.")]
        public static readonly PropertyDescriptor DefaultDrawingAttributesProperty = PropertyDescriptor.Create(nameof(DefaultDrawingAttributes), typeof(InkDrawingAttributes), typeof(InkCanvas));

        /// <summary>
        /// Describes the <see cref="P:InputMode"/> property.  This field is read-only.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "PropertyDescriptor is immutable.")]
        public static readonly PropertyDescriptor InputModeProperty = PropertyDescriptor.Create(nameof(InputMode), typeof(InkInputMode), typeof(InkCanvas));
        #endregion

        /// <summary>
        /// Gets or sets the drawing attributes to apply to new ink strokes on the canvas.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", Justification = "Exception parameter refers to property name for easier understanding of invalid value.")]
        public InkDrawingAttributes DefaultDrawingAttributes
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
                        throw new ArgumentNullException(nameof(DefaultDrawingAttributes));
                    }

                    drawingAttributes = value;
                    nativeObject.UpdateDrawingAttributes(drawingAttributes);
                    drawingAttributes.PropertyChanged -= OnDrawingAttributeChanged;
                    drawingAttributes.PropertyChanged += OnDrawingAttributeChanged;

                    OnPropertyChanged(DefaultDrawingAttributesProperty);
                }
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private InkDrawingAttributes drawingAttributes;

        /// <summary>
        /// Gets or sets how the ink canvas handles input.
        /// </summary>
        public InkInputMode InputMode
        {
            get { return nativeObject.InputMode; }
            set { nativeObject.InputMode = value; }
        }

        /// <summary>
        /// Gets a container of the ink strokes that are currently on the canvas.
        /// </summary>
        public InkStrokeContainer Strokes { get; }

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        // this field is to avoid casting
        private readonly INativeInkCanvas nativeObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="InkCanvas"/> class.
        /// </summary>
        public InkCanvas()
            : this(typeof(INativeInkCanvas), null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InkCanvas"/> class.
        /// </summary>
        /// <param name="resolveType">The type to pass to the IoC container in order to resolve the native object.</param>
        /// <param name="resolveName">An optional name to use when resolving the native object.</param>
        /// <param name="resolveParameters">Any parameters to pass along to the constructor of the resolve type.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="resolveType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="resolveType"/> does not resolve to an <see cref="INativeInkCanvas"/> instance.</exception>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", Justification = "resolveType is validated in base constructor.")]
        protected InkCanvas(Type resolveType, string resolveName, params ResolveParameter[] resolveParameters)
            : base(resolveType, resolveName, resolveParameters)
        {
            nativeObject = ObjectRetriever.GetNativeObject(this) as INativeInkCanvas;
            if (nativeObject == null)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.Strings.TypeMustResolveToType, resolveType.FullName, typeof(INativeInkCanvas).FullName), nameof(resolveType));
            }

            DefaultDrawingAttributes = new InkDrawingAttributes() { Color = Colors.Black };
            HorizontalAlignment = HorizontalAlignment.Stretch;
            InputMode = InkInputMode.Inking;
            Strokes = new InkStrokeContainer(nativeObject);
            VerticalAlignment = VerticalAlignment.Stretch;
        }

        private void OnDrawingAttributeChanged(object sender, PropertyChangedEventArgs e)
        {
            nativeObject.UpdateDrawingAttributes(drawingAttributes);
        }
    }
}
