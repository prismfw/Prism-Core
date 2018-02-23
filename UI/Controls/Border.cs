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
    /// Represents a UI element for drawing a border around another UI element.
    /// </summary>
    [Resolve(typeof(INativeBorder))]
    public class Border : Element
    {
        #region Property Descriptors
        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:Background"/> property.
        /// </summary>
        public static PropertyDescriptor BackgroundProperty { get; } = PropertyDescriptor.Create(nameof(Background), typeof(Brush), typeof(Border));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:BorderBrush"/> property.
        /// </summary>
        public static PropertyDescriptor BorderBrushProperty { get; } = PropertyDescriptor.Create(nameof(BorderBrush), typeof(Brush), typeof(Border));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:BorderThickness"/> property.
        /// </summary>
        public static PropertyDescriptor BorderThicknessProperty { get; } = PropertyDescriptor.Create(nameof(BorderThickness), typeof(Thickness), typeof(Border), new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:Child"/> property.
        /// </summary>
        public static PropertyDescriptor ChildProperty { get; } = PropertyDescriptor.Create(nameof(Child), typeof(Element), typeof(Border), new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:Padding"/> property.
        /// </summary>
        public static PropertyDescriptor PaddingProperty { get; } = PropertyDescriptor.Create(nameof(Padding), typeof(Thickness), typeof(Border), new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));
        #endregion

        /// <summary>
        /// Gets or sets the background for this instance.
        /// </summary>
        public Brush Background
        {
            get { return nativeObject.Background; }
            set { nativeObject.Background = value; }
        }

        /// <summary>
        /// Gets or sets the <see cref="Brush"/> with which to paint the border.
        /// </summary>
        public Brush BorderBrush
        {
            get { return nativeObject.BorderBrush; }
            set { nativeObject.BorderBrush = value; }
        }

        /// <summary>
        /// Gets or sets the thickness of the border.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", Justification = "Exception parameter refers to property name for easier understanding of invalid value.")]
        public Thickness BorderThickness
        {
            get { return nativeObject.BorderThickness; }
            set
            {
                if (double.IsNaN(value.Left) || double.IsInfinity(value.Left) || double.IsNaN(value.Top) || double.IsInfinity(value.Top) ||
                    double.IsNaN(value.Right) || double.IsInfinity(value.Right) || double.IsNaN(value.Bottom) || double.IsInfinity(value.Bottom))
                {
                    throw new ArgumentException(Strings.ThicknessContainsNaNOrInfiniteValue, nameof(BorderThickness));
                }

                nativeObject.BorderThickness = value;
            }
        }

        /// <summary>
        /// Gets or sets the child element around which to draw the border.
        /// </summary>
        public Element Child
        {
            get { return (Element)ObjectRetriever.GetAgnosticObject(nativeObject.Child); }
            set { nativeObject.Child = ObjectRetriever.GetNativeObject(value); }
        }

        /// <summary>
        /// Gets or sets the padding between the border and the child element.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", Justification = "Exception parameter refers to property name for easier understanding of invalid value.")]
        public Thickness Padding
        {
            get { return nativeObject.Padding; }
            set
            {
                if (double.IsNaN(value.Left) || double.IsInfinity(value.Left) || double.IsNaN(value.Top) || double.IsInfinity(value.Top) ||
                    double.IsNaN(value.Right) || double.IsInfinity(value.Right) || double.IsNaN(value.Bottom) || double.IsInfinity(value.Bottom))
                {
                    throw new ArgumentException(Strings.ThicknessContainsNaNOrInfiniteValue, nameof(Padding));
                }

                nativeObject.Padding = value;
            }
        }

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        // this field is to avoid casting
        private readonly INativeBorder nativeObject;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="Border"/> class.
        /// </summary>
        public Border()
            : this(ResolveParameter.EmptyParameters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Border"/> class and pairs it with the specified native object.
        /// </summary>
        /// <param name="nativeObject">The native object with which to pair this instance.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="nativeObject"/> doesn't match the type specified by the topmost <see cref="ResolveAttribute"/> in the inheritance chain.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="nativeObject"/> is <c>null</c>.</exception>
        protected Border(INativeBorder nativeObject)
            : base(nativeObject)
        {
            this.nativeObject = nativeObject;

            BorderThickness = new Thickness(1);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Border"/> class and pairs it with a native object that is resolved from the IoC container.
        /// </summary>
        /// <param name="resolveParameters">Any parameters to pass along to the constructor of the native type.</param>
        /// <exception cref="TypeResolutionException">Thrown when the native object does not resolve to an <see cref="INativeBorder"/> instance.</exception>
        protected Border(ResolveParameter[] resolveParameters)
            : base(resolveParameters)
        {
            nativeObject = ObjectRetriever.GetNativeObject(this) as INativeBorder;
            if (nativeObject == null)
            {
                throw new TypeResolutionException(string.Format(CultureInfo.CurrentCulture, Strings.TypeMustResolveToType,
                    ObjectRetriever.GetNativeObject(this).GetType().FullName, typeof(INativeBorder).FullName));
            }

            BorderThickness = new Thickness(1);
        }

        /// <summary>
        /// Called when this instance is ready to arrange its children and returns the final rendering size of the object.
        /// </summary>
        /// <param name="constraints">The width and height that this instance should not exceed.</param>
        /// <returns>The final rendering size of the object as a <see cref="Size"/> instance.</returns>
        protected override Size ArrangeOverride(Size constraints)
        {
            constraints = base.ArrangeOverride(constraints);

            var thickness = BorderThickness;
            var padding = Padding;
            var currentChild = Child ?? VisualTreeHelper.GetChild<Element>(this);
            if (currentChild != null)
            {
                currentChild.Arrange(new Rectangle(thickness.Left + padding.Left, thickness.Top + padding.Top,
                    Math.Max(constraints.Width - (thickness.Left + thickness.Right + padding.Left + padding.Right), 0),
                    Math.Max(constraints.Height - (thickness.Top + thickness.Bottom + padding.Top + padding.Bottom), 0)));
            }

            return constraints;
        }

        /// <summary>
        /// Called when this instance is ready to be measured and returns the desired size of the object.
        /// </summary>
        /// <param name="constraints">The width and height that this instance should not exceed.</param>
        /// <returns>The desired size of the object as a <see cref="Size"/> instance.</returns>
        protected override Size MeasureOverride(Size constraints)
        {
            constraints = base.MeasureOverride(constraints);

            var thickness = BorderThickness;
            var padding = Padding;
            var desiredSize = new Size(thickness.Left + thickness.Right + padding.Left + padding.Right,
                thickness.Top + thickness.Bottom + padding.Top + padding.Bottom);

            var currentChild = Child ?? VisualTreeHelper.GetChild<Element>(this);
            if (currentChild == null || currentChild.Visibility == Visibility.Collapsed)
            {
                return desiredSize;
            }

            constraints.Width -= desiredSize.Width;
            constraints.Height -= desiredSize.Height;
            currentChild.Measure(new Size(constraints.Width, constraints.Height));
            
            return currentChild.DesiredSize + desiredSize;
        }
    }
}
