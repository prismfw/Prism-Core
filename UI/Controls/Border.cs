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
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Prism.Native;
using Prism.UI.Media;

#if !DEBUG
using System.Diagnostics;
#endif

namespace Prism.UI.Controls
{
    /// <summary>
    /// Represents a UI element for drawing a border around another UI element.
    /// </summary>
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
        public static PropertyDescriptor BorderThicknessProperty { get; } = PropertyDescriptor.Create(nameof(BorderThickness), typeof(Thickness), typeof(Border), new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));

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
        public Thickness BorderThickness
        {
            get { return nativeObject.BorderThickness; }
            set { nativeObject.BorderThickness = value; }
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
        public Thickness Padding
        {
            get { return nativeObject.Padding; }
            set { nativeObject.Padding = value; }
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
            : this(typeof(INativeBorder), null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Border"/> class.
        /// </summary>
        /// <param name="resolveType">The type to pass to the IoC container in order to resolve the native object.</param>
        /// <param name="resolveName">An optional name to use when resolving the native object.</param>
        /// <param name="resolveParameters">Any parameters to pass along to the constructor of the resolve type.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="resolveType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="resolveType"/> does not resolve to an <see cref="INativeBorder"/> instance.</exception>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", Justification = "resolveType is validated in base constructor.")]
        protected Border(Type resolveType, string resolveName, params ResolveParameter[] resolveParameters)
            : base(resolveType, resolveName, resolveParameters)
        {
            nativeObject = ObjectRetriever.GetNativeObject(this) as INativeBorder;
            if (nativeObject == null)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.Strings.TypeMustResolveToType, resolveType.FullName, typeof(INativeBorder).FullName), nameof(resolveType));
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
                    constraints.Width - (thickness.Left + thickness.Right + padding.Left + padding.Right),
                    constraints.Height - (thickness.Top + thickness.Bottom + padding.Top + padding.Bottom)));
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
