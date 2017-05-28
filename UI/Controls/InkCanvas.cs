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
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Prism.Native;
using Prism.Resources;
using Prism.UI.Media.Inking;

namespace Prism.UI.Controls
{
    /// <summary>
    /// Represents a UI element that renders pointer input as ink strokes.
    /// </summary>
    [Resolve(typeof(INativeInkCanvas))]
    public class InkCanvas : Element
    {
        #region Property Descriptors
        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:DefaultDrawingAttributes"/> property.
        /// </summary>
        public static PropertyDescriptor DefaultDrawingAttributesProperty { get; } = PropertyDescriptor.Create(nameof(DefaultDrawingAttributes), typeof(InkDrawingAttributes), typeof(InkCanvas));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:InputMode"/> property.
        /// </summary>
        public static PropertyDescriptor InputModeProperty { get; } = PropertyDescriptor.Create(nameof(InputMode), typeof(InkInputMode), typeof(InkCanvas));
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
            : this(ResolveParameter.EmptyParameters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InkCanvas"/> class and pairs it with the specified native object.
        /// </summary>
        /// <param name="nativeObject">The native object with which to pair this instance.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="nativeObject"/> doesn't match the type specified by the topmost <see cref="ResolveAttribute"/> in the inheritance chain.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="nativeObject"/> is <c>null</c>.</exception>
        protected InkCanvas(INativeInkCanvas nativeObject)
            : base(nativeObject)
        {
            this.nativeObject = nativeObject;

            Strokes = new InkStrokeContainer(nativeObject);
            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InkCanvas"/> class and pairs it with a native object that is resolved from the IoC container.
        /// </summary>
        /// <param name="resolveParameters">Any parameters to pass along to the constructor of the native type.</param>
        /// <exception cref="TypeResolutionException">Thrown when the native object does not resolve to an <see cref="INativeInkCanvas"/> instance.</exception>
        protected InkCanvas(ResolveParameter[] resolveParameters)
            : base(resolveParameters)
        {
            nativeObject = ObjectRetriever.GetNativeObject(this) as INativeInkCanvas;
            if (nativeObject == null)
            {
                throw new TypeResolutionException(string.Format(CultureInfo.CurrentCulture, Strings.TypeMustResolveToType,
                    ObjectRetriever.GetNativeObject(this).GetType().FullName, typeof(INativeInkCanvas).FullName));
            }

            Strokes = new InkStrokeContainer(nativeObject);
            Initialize();
        }

        private void Initialize()
        {
            DefaultDrawingAttributes = new InkDrawingAttributes();
            HorizontalAlignment = HorizontalAlignment.Stretch;
            InputMode = InkInputMode.Inking;
            VerticalAlignment = VerticalAlignment.Stretch;
        }

        private void OnDrawingAttributeChanged(object sender, PropertyChangedEventArgs e)
        {
            nativeObject.UpdateDrawingAttributes(drawingAttributes);
        }
    }
}
