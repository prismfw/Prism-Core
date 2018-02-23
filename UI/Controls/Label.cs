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
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Prism.Native;
using Prism.Resources;
using Prism.UI.Media;

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
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:IsAutoScalingEnabled"/> property.
        /// </summary>
        public static PropertyDescriptor IsAutoScalingEnabledProperty { get; } = PropertyDescriptor.Create(nameof(IsAutoScalingEnabled), typeof(bool), typeof(Label), new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.AffectsArrange));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:Lines"/> property.
        /// </summary>
        public static PropertyDescriptor LinesProperty { get; } = PropertyDescriptor.Create(nameof(Lines), typeof(int), typeof(Label), new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:MinScaledFontSize"/> property.
        /// </summary>
        public static PropertyDescriptor MinScaledFontSizeProperty { get; } = PropertyDescriptor.Create(nameof(MinScaledFontSize), typeof(double), typeof(Label));

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
            get { return fontSize; }
            set
            {
                if (value != fontSize)
                {
                    if (double.IsNaN(value) || double.IsInfinity(value))
                    {
                        throw new ArgumentException(Strings.ValueCannotBeNaNOrInfinity, nameof(FontSize));
                    }

                    if (value < 0)
                    {
                        throw new ArgumentOutOfRangeException(nameof(FontSize), Strings.ValueCannotBeLessThanZero);
                    }

                    fontSize = value;
                    nativeObject.FontSize = fontSize;
                    OnPropertyChanged(FontSizeProperty);
                }
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private double fontSize;

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
        /// Gets or sets a value indicating whether the size of the text is automatically scaled down to fit within the space that it is given.
        /// </summary>
        public bool IsAutoScalingEnabled
        {
            get { return isAutoScalingEnabled; }
            set
            {
                if (value != isAutoScalingEnabled)
                {
                    isAutoScalingEnabled = value;
                    if (!isAutoScalingEnabled)
                    {
                        nativeObject.FontSize = fontSize;
                    }

                    OnPropertyChanged(IsAutoScalingEnabledProperty);
                }
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool isAutoScalingEnabled;

        /// <summary>
        /// Gets or sets the maximum number of lines of text that the label can show.
        /// A value of 0 means there is no limit.
        /// </summary>
        public int Lines
        {
            get { return lines; }
            set
            {
                if (value != lines)
                {
                    lines = Math.Max(value, 0);
                    nativeObject.SetMaxLines(lines);
                    OnPropertyChanged(LinesProperty);
                }
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int lines;

        /// <summary>
        /// Gets or sets the minimum allowable size of the font when scaling down the text through auto-scaling.
        /// <see cref="P:IsAutoScalingEnabled"/> must be <c>true</c> for this to have any effect.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", Justification = "Exception parameter refers to property name for easier understanding of invalid value.")]
        public double MinScaledFontSize
        {
            get { return minScaledFontSize; }
            set
            {
                if (value != minScaledFontSize)
                {
                    if (double.IsNaN(value) || double.IsInfinity(value))
                    {
                        throw new ArgumentException(Strings.ValueCannotBeNaNOrInfinity, nameof(MinScaledFontSize));
                    }

                    if (value < 0)
                    {
                        throw new ArgumentOutOfRangeException(nameof(MinScaledFontSize), Strings.ValueCannotBeLessThanZero);
                    }

                    minScaledFontSize = value;
                    OnPropertyChanged(MinScaledFontSizeProperty);

                    if (isAutoScalingEnabled)
                    {
                        InvalidateArrange();
                    }
                }
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private double minScaledFontSize = 1;

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

        /// <summary>
        /// Called when this instance is ready to arrange its children and returns the final rendering size of the object.
        /// </summary>
        /// <param name="constraints">The width and height that this instance should not exceed.</param>
        /// <returns>The final rendering size of the object as a <see cref="Size"/> instance.</returns>
        protected override Size ArrangeOverride(Size constraints)
        {
            var renderSize = base.ArrangeOverride(constraints);
            if (isAutoScalingEnabled)
            {
                nativeObject.SetMaxLines(0);
                nativeObject.FontSize = fontSize;

                var realSize = nativeObject.Measure(new Size(constraints.Width, double.MaxValue));
                if (realSize.Height > renderSize.Height && minScaledFontSize < nativeObject.FontSize)
                {
                    realSize = Size.Empty;
                    double maxFontSize = nativeObject.FontSize;
                    double minFontSize = minScaledFontSize;
                    
                    while (maxFontSize - minFontSize > 1)
                    {
                        nativeObject.FontSize = (minFontSize + maxFontSize) / 2;
                        var size = nativeObject.Measure(new Size(constraints.Width, double.MaxValue));

                        if (lines > 0)
                        {
                            nativeObject.SetMaxLines(lines);
                            renderSize = nativeObject.Measure(new Size(constraints.Width, double.MaxValue));
                            renderSize.Height = Math.Min(renderSize.Height, constraints.Height);
                            nativeObject.SetMaxLines(0);
                        }

                        if (size.Height > renderSize.Height)
                        {
                            maxFontSize = nativeObject.FontSize;
                        }
                        else
                        {
                            minFontSize = nativeObject.FontSize;
                            realSize = size;
                        }
                    }

                    nativeObject.FontSize = minFontSize;
                }

                nativeObject.SetMaxLines(lines);
                return realSize == Size.Empty ? nativeObject.Measure(new Size(constraints.Width, double.MaxValue)) : realSize;
            }

            return renderSize;
        }

        /// <summary>
        /// Called when this instance is ready to be measured and returns the desired size of the object.
        /// </summary>
        /// <param name="constraints">The width and height that this instance should not exceed.</param>
        /// <returns>The desired size of the object as a <see cref="Size"/> instance.</returns>
        protected override Size MeasureOverride(Size constraints)
        {
            if (isAutoScalingEnabled)
            {
                double currentFontSize = nativeObject.FontSize;
                nativeObject.FontSize = FontSize;

                var desiredSize = base.MeasureOverride(constraints);
                nativeObject.FontSize = currentFontSize;
                return desiredSize;
            }

            return base.MeasureOverride(constraints);
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
