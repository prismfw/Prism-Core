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
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Prism.Native;
using Prism.Resources;
using Prism.UI.Media;

#if !DEBUG
using System.Diagnostics;
#endif

namespace Prism.UI.Controls
{
    /// <summary>
    /// Represents a UI element that displays a string of read-only text.
    /// </summary>
    [Resolve(typeof(INativeLabel))]
    public class Label : Element
    {
        #region Property Descriptors
        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:FontFamily"/> property.
        /// </summary>
        public static PropertyDescriptor FontFamilyProperty { get; } = PropertyDescriptor.Create(nameof(FontFamily), typeof(FontFamily), typeof(Label));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:FontSize"/> property.
        /// </summary>
        public static PropertyDescriptor FontSizeProperty { get; } = PropertyDescriptor.Create(nameof(FontSize), typeof(double), typeof(Label), new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:FontStyle"/> property.
        /// </summary>
        public static PropertyDescriptor FontStyleProperty { get; } = PropertyDescriptor.Create(nameof(FontStyle), typeof(FontStyle), typeof(Label), new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:Foreground"/> property.
        /// </summary>
        public static PropertyDescriptor ForegroundProperty { get; } = PropertyDescriptor.Create(nameof(Foreground), typeof(Brush), typeof(Label));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:HighlightBrush"/> property.
        /// </summary>
        public static PropertyDescriptor HighlightBrushProperty { get; } = PropertyDescriptor.Create(nameof(HighlightBrush), typeof(Brush), typeof(Label));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:Lines"/> property.
        /// </summary>
        public static PropertyDescriptor LinesProperty { get; } = PropertyDescriptor.Create(nameof(Lines), typeof(int), typeof(Label), new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:Text"/> property.
        /// </summary>
        public static PropertyDescriptor TextProperty { get; } = PropertyDescriptor.Create(nameof(Text), typeof(string), typeof(Label), new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:TextAlignment"/> property.
        /// </summary>
        public static PropertyDescriptor TextAlignmentProperty { get; } = PropertyDescriptor.Create(nameof(TextAlignment), typeof(TextAlignment), typeof(Label));
        #endregion

        /// <summary>
        /// Gets or sets the font to use for displaying the text in the label.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", Justification = "Exception parameter refers to property name for easier understanding of invalid value.")]
        public FontFamily FontFamily
        {
            get { return ObjectRetriever.GetAgnosticObject(nativeObject.FontFamily) as FontFamily; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(FontFamily));
                }

                nativeObject.FontFamily = ObjectRetriever.GetNativeObject(value);
            }
        }

        /// <summary>
        /// Gets or sets the size of the text in the label.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", Justification = "Exception parameter refers to property name for easier understanding of invalid value.")]
        public double FontSize
        {
            get { return nativeObject.FontSize; }
            set
            {
                if (double.IsNaN(value) || double.IsInfinity(value))
                {
                    throw new ArgumentException(Strings.ValueCannotBeNaNOrInfinity, nameof(FontSize));
                }

                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(FontSize), Strings.ValueCannotBeLessThanZero);
                }

                nativeObject.FontSize = value;
            }
        }

        /// <summary>
        /// Gets or sets the style with which to render the text in the label.
        /// </summary>
        public FontStyle FontStyle
        {
            get { return nativeObject.FontStyle; }
            set { nativeObject.FontStyle = value; }
        }

        /// <summary>
        /// Gets or sets the <see cref="Brush"/> to apply to the text contents of the label.
        /// </summary>
        public Brush Foreground
        {
            get { return nativeObject.Foreground; }
            set { nativeObject.Foreground = value; }
        }

        /// <summary>
        /// Gets or sets the <see cref="Brush"/> to apply to the text contents when the label resides within a highlighted element.
        /// </summary>
        public Brush HighlightBrush
        {
            get { return nativeObject.HighlightBrush; }
            set { nativeObject.HighlightBrush = value; }
        }

        /// <summary>
        /// Gets or sets the maximum number of lines of text that the label can show.
        /// A value of 0 means there is no limit.
        /// </summary>
        public int Lines
        {
            get { return nativeObject.Lines; }
            set { nativeObject.Lines = value; }
        }

        /// <summary>
        /// Gets or sets the text of the label.
        /// </summary>
        public string Text
        {
            get { return nativeObject.Text; }
            set { nativeObject.Text = value; }
        }

        /// <summary>
        /// Gets or sets the manner in which the text is aligned within the label.
        /// </summary>
        public TextAlignment TextAlignment
        {
            get { return nativeObject.TextAlignment; }
            set { nativeObject.TextAlignment = value; }
        }

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        // this field is to avoid casting
        private readonly INativeLabel nativeObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="Label"/> class.
        /// </summary>
        public Label()
            : this(ResolveParameter.EmptyParameters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Label"/> class and pairs it with the specified native object.
        /// </summary>
        /// <param name="nativeObject">The native object with which to pair this instance.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="nativeObject"/> doesn't match the type specified by the topmost <see cref="ResolveAttribute"/> in the inheritance chain.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="nativeObject"/> is <c>null</c>.</exception>
        protected Label(INativeLabel nativeObject)
            : base(nativeObject)
        {
            this.nativeObject = nativeObject;

            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Label"/> class and pairs it with a native object that is resolved from the IoC container.
        /// </summary>
        /// <param name="resolveParameters">Any parameters to pass along to the constructor of the native type.</param>
        /// <exception cref="TypeResolutionException">Thrown when the native object does not resolve to an <see cref="INativeLabel"/> instance.</exception>
        protected Label(ResolveParameter[] resolveParameters)
            : base(resolveParameters)
        {
            nativeObject = ObjectRetriever.GetNativeObject(this) as INativeLabel;
            if (nativeObject == null)
            {
                throw new TypeResolutionException(string.Format(CultureInfo.CurrentCulture, Strings.TypeMustResolveToType,
                    ObjectRetriever.GetNativeObject(this).GetType().FullName, typeof(INativeLabel).FullName));
            }

            Initialize();
        }

        private void Initialize()
        {
            Lines = 0;

            SetResourceReference(FontFamilyProperty, SystemResources.BaseFontFamilyKey);
            SetResourceReference(FontSizeProperty, SystemResources.LabelFontSizeKey);
            SetResourceReference(FontStyleProperty, SystemResources.LabelFontStyleKey);
            SetResourceReference(ForegroundProperty, SystemResources.LabelForegroundBrushKey);
        }
    }
}
