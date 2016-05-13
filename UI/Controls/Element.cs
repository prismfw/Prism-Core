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
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Prism.Input;
using Prism.Native;

namespace Prism.UI.Controls
{
    /// <summary>
    /// Represents the base class for UI elements.  This class is abstract.
    /// </summary>
    public abstract class Element : Visual
    {
        #region Event Descriptors
        /// <summary>
        /// Describes the <see cref="E:PointerCanceled"/> event.  This field is read-only.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "EventDescriptor is immutable.")]
        public static readonly EventDescriptor PointerCanceledEvent = EventDescriptor.Create(nameof(PointerCanceled), typeof(TypedEventHandler<Element, PointerEventArgs>), typeof(Element));

        /// <summary>
        /// Describes the <see cref="E:PointerMoved"/> event.  This field is read-only.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "EventDescriptor is immutable.")]
        public static readonly EventDescriptor PointerMovedEvent = EventDescriptor.Create(nameof(PointerMoved), typeof(TypedEventHandler<Element, PointerEventArgs>), typeof(Element));

        /// <summary>
        /// Describes the <see cref="E:PointerPressed"/> event.  This field is read-only.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "EventDescriptor is immutable.")]
        public static readonly EventDescriptor PointerPressedEvent = EventDescriptor.Create(nameof(PointerPressed), typeof(TypedEventHandler<Element, PointerEventArgs>), typeof(Element));

        /// <summary>
        /// Describes the <see cref="E:PointerReleased"/> event.  This field is read-only.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "EventDescriptor is immutable.")]
        public static readonly EventDescriptor PointerReleasedEvent = EventDescriptor.Create(nameof(PointerReleased), typeof(TypedEventHandler<Element, PointerEventArgs>), typeof(Element));
        #endregion

        #region Property Descriptors
        /// <summary>
        /// Describes the <see cref="P:Height"/> property.  This field is read-only.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "PropertyDescriptor is immutable.")]
        public static readonly PropertyDescriptor HeightProperty = PropertyDescriptor.Create(nameof(Height), typeof(double), typeof(Element), new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// Describes the <see cref="P:HorizontalAlignment"/> property.  This field is read-only.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "PropertyDescriptor is immutable.")]
        public static readonly PropertyDescriptor HorizontalAlignmentProperty = PropertyDescriptor.Create(nameof(HorizontalAlignment), typeof(HorizontalAlignment), typeof(Element), new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.AffectsArrange));

        /// <summary>
        /// Describes the <see cref="P:Margin"/> property.  This field is read-only.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "PropertyDescriptor is immutable.")]
        public static readonly PropertyDescriptor MarginProperty = PropertyDescriptor.Create(nameof(Margin), typeof(Thickness), typeof(Element), new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));

        /// <summary>
        /// Describes the <see cref="P:MaxHeight"/> property.  This field is read-only.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "PropertyDescriptor is immutable.")]
        public static readonly PropertyDescriptor MaxHeightProperty = PropertyDescriptor.Create(nameof(MaxHeight), typeof(double), typeof(Element), new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// Describes the <see cref="P:MaxWidth"/> property.  This field is read-only.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "PropertyDescriptor is immutable.")]
        public static readonly PropertyDescriptor MaxWidthProperty = PropertyDescriptor.Create(nameof(MaxWidth), typeof(double), typeof(Element), new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// Describes the <see cref="P:MinHeight"/> property.  This field is read-only.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "PropertyDescriptor is immutable.")]
        public static readonly PropertyDescriptor MinHeightProperty = PropertyDescriptor.Create(nameof(MinHeight), typeof(double), typeof(Element), new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// Describes the <see cref="P:MinWidth"/> property.  This field is read-only.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "PropertyDescriptor is immutable.")]
        public static readonly PropertyDescriptor MinWidthProperty = PropertyDescriptor.Create(nameof(MinWidth), typeof(double), typeof(Element), new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// Describes the <see cref="P:Name"/> property.  This field is read-only.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "PropertyDescriptor is immutable.")]
        public static readonly PropertyDescriptor NameProperty = PropertyDescriptor.Create(nameof(Name), typeof(string), typeof(Element));

        /// <summary>
        /// Describes the <see cref="P:VerticalAlignment"/> property.  This field is read-only.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "PropertyDescriptor is immutable.")]
        public static readonly PropertyDescriptor VerticalAlignmentProperty = PropertyDescriptor.Create(nameof(VerticalAlignment), typeof(VerticalAlignment), typeof(Element), new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.AffectsArrange));

        /// <summary>
        /// Describes the <see cref="P:Visibility"/> property.  This field is read-only.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "PropertyDescriptor is immutable.")]
        public static readonly PropertyDescriptor VisibilityProperty = PropertyDescriptor.Create(nameof(Visibility), typeof(Visibility), typeof(Element));

        /// <summary>
        /// Describes the <see cref="P:Width"/> property.  This field is read-only.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "PropertyDescriptor is immutable.")]
        public static readonly PropertyDescriptor WidthProperty = PropertyDescriptor.Create(nameof(Width), typeof(double), typeof(Element), new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.AffectsMeasure));
        #endregion

        /// <summary>
        /// Occurs when the system loses track of the pointer for some reason.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification = "Event handler provides a strongly-typed sender for easier use.")]
        public event TypedEventHandler<Element, PointerEventArgs> PointerCanceled;

        /// <summary>
        /// Occurs when the pointer has moved while over the element.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification = "Event handler provides a strongly-typed sender for easier use.")]
        public event TypedEventHandler<Element, PointerEventArgs> PointerMoved;

        /// <summary>
        /// Occurs when the pointer has been pressed while over the element.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification = "Event handler provides a strongly-typed sender for easier use.")]
        public event TypedEventHandler<Element, PointerEventArgs> PointerPressed;

        /// <summary>
        /// Occurs when the pointer has been released while over the element.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification = "Event handler provides a strongly-typed sender for easier use.")]
        public event TypedEventHandler<Element, PointerEventArgs> PointerReleased;

        /// <summary>
        /// Gets or sets the suggested height for the element.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", Justification = "Exception parameter refers to property name for easier understanding of invalid value.")]
        public double Height
        {
            get { return height; }
            set
            {
                if (value != height)
                {
                    if (double.IsInfinity(value))
                    {
                        throw new ArgumentException(Resources.Strings.ValueCannotBeInfinity, nameof(Height));
                    }

                    if (value < 0)
                    {
                        throw new ArgumentOutOfRangeException(nameof(Height), Resources.Strings.ValueCannotBeLessThanZero);
                    }

                    height = value;
                    OnPropertyChanged(HeightProperty);
                }
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private double height = double.NaN;

        /// <summary>
        /// Gets or sets the manner in which the element is horizontally aligned within its allocated space.
        /// </summary>
        public HorizontalAlignment HorizontalAlignment
        {
            get { return horizontalAlignment; }
            set
            {
                if (value != horizontalAlignment)
                {
                    horizontalAlignment = value;
                    OnPropertyChanged(HorizontalAlignmentProperty);
                }
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private HorizontalAlignment horizontalAlignment;

        /// <summary>
        /// Gets or sets the outer margin of the element.
        /// </summary>
        public Thickness Margin
        {
            get { return margin; }
            set
            {
                if (value != margin)
                {
                    margin = value;
                    OnPropertyChanged(MarginProperty);
                }
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Thickness margin;

        /// <summary>
        /// Gets or sets the maximum height for the element.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", Justification = "Exception parameter refers to property name for easier understanding of invalid value.")]
        public double MaxHeight
        {
            get { return maxHeight; }
            set
            {
                if (value != maxHeight)
                {
                    if (double.IsNaN(value) || double.IsNegativeInfinity(value))
                    {
                        throw new ArgumentException(Resources.Strings.ValueCannotBeNaNOrNegativeInfinity, nameof(MaxHeight));
                    }

                    if (value < 0)
                    {
                        throw new ArgumentOutOfRangeException(nameof(MaxHeight), Resources.Strings.ValueCannotBeLessThanZero);
                    }

                    maxHeight = value;
                    OnPropertyChanged(MaxHeightProperty);
                }
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private double maxHeight = double.PositiveInfinity;

        /// <summary>
        /// Gets or sets the maximum width for the element.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", Justification = "Exception parameter refers to property name for easier understanding of invalid value.")]
        public double MaxWidth
        {
            get { return maxWidth; }
            set
            {
                if (value != maxWidth)
                {
                    if (double.IsNaN(value) || double.IsNegativeInfinity(value))
                    {
                        throw new ArgumentException(Resources.Strings.ValueCannotBeNaNOrNegativeInfinity, nameof(MaxWidth));
                    }

                    if (value < 0)
                    {
                        throw new ArgumentOutOfRangeException(nameof(MaxWidth), Resources.Strings.ValueCannotBeLessThanZero);
                    }

                    maxWidth = value;
                    OnPropertyChanged(MaxWidthProperty);
                }
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private double maxWidth = double.PositiveInfinity;

        /// <summary>
        /// Gets or sets the minimum height for the element.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", Justification = "Exception parameter refers to property name for easier understanding of invalid value.")]
        public double MinHeight
        {
            get { return minHeight; }
            set
            {
                if (value != minHeight)
                {
                    if (double.IsNaN(value) || double.IsInfinity(value))
                    {
                        throw new ArgumentException(Resources.Strings.ValueCannotBeNaNOrInfinity, nameof(MinHeight));
                    }

                    if (value < 0)
                    {
                        throw new ArgumentOutOfRangeException(nameof(MinHeight), Resources.Strings.ValueCannotBeLessThanZero);
                    }

                    minHeight = value;
                    OnPropertyChanged(MinHeightProperty);
                }
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private double minHeight = 0;

        /// <summary>
        /// Gets or sets the minimum width for the element.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", Justification = "Exception parameter refers to property name for easier understanding of invalid value.")]
        public double MinWidth
        {
            get { return minWidth; }
            set
            {
                if (value != minWidth)
                {
                    if (double.IsNaN(value) || double.IsInfinity(value))
                    {
                        throw new ArgumentException(Resources.Strings.ValueCannotBeNaNOrInfinity, nameof(MinWidth));
                    }

                    if (value < 0)
                    {
                        throw new ArgumentOutOfRangeException(nameof(MinWidth), Resources.Strings.ValueCannotBeLessThanZero);
                    }

                    minWidth = value;
                    OnPropertyChanged(MinWidthProperty);
                }
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private double minWidth = 0;

        /// <summary>
        /// Gets or sets the name of the element, which can be used to identify it within a visual hierarchy or collection.
        /// </summary>
        public string Name
        {
            get { return name; }
            set
            {
                if (value != name)
                {
                    name = value;
                    OnPropertyChanged(NameProperty);
                }
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string name;

        /// <summary>
        /// Gets or sets the manner in which the element is vertically aligned within its allocated space.
        /// </summary>
        public VerticalAlignment VerticalAlignment
        {
            get { return verticalAlignment; }
            set
            {
                if (value != verticalAlignment)
                {
                    verticalAlignment = value;
                    OnPropertyChanged(VerticalAlignmentProperty);
                }
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private VerticalAlignment verticalAlignment;

        /// <summary>
        /// Gets or sets the display state of the element.
        /// </summary>
        public Visibility Visibility
        {
            get { return nativeObject.Visibility; }
            set
            {
                var oldValue = nativeObject.Visibility;
                nativeObject.Visibility = value;

                if ((oldValue == Visibility.Collapsed || nativeObject.Visibility == Visibility.Collapsed) && oldValue != nativeObject.Visibility)
                {
                    InvalidateMeasure();
                }
            }
        }

        /// <summary>
        /// Gets or sets the suggested width for the element.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", Justification = "Exception parameter refers to property name for easier understanding of invalid value.")]
        public double Width
        {
            get { return width; }
            set
            {
                if (value != width)
                {
                    if (double.IsInfinity(value))
                    {
                        throw new ArgumentException(Resources.Strings.ValueCannotBeInfinity, nameof(Width));
                    }

                    if (value < 0)
                    {
                        throw new ArgumentOutOfRangeException(nameof(Width), Resources.Strings.ValueCannotBeLessThanZero);
                    }

                    width = value;
                    OnPropertyChanged(WidthProperty);
                }
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private double width = double.NaN;

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        // this field is to avoid casting
        private readonly INativeElement nativeObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="Element"/> class.
        /// </summary>
        /// <param name="resolveType">The type to pass to the IoC container in order to resolve the native object.</param>
        /// <param name="resolveName">An optional name to use when resolving the native object.</param>
        /// <param name="resolveParameters">Any parameters to pass along to the constructor of the resolve type.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="resolveType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="resolveType"/> does not resolve to an <see cref="INativeElement"/> instance.</exception>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", Justification = "resolveType is validated in base constructor.")]
        protected Element(Type resolveType, string resolveName, params ResolveParameter[] resolveParameters)
            : base(resolveType, resolveName, resolveParameters)
        {
            nativeObject = ObjectRetriever.GetNativeObject(this) as INativeElement;
            if (nativeObject == null)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.Strings.TypeMustResolveToType, resolveType.FullName, typeof(INativeElement).FullName), nameof(resolveType));
            }

            nativeObject.PointerCanceled += (o, e) => OnPointerCanceled(e);
            nativeObject.PointerMoved += (o, e) => OnPointerMoved(e);
            nativeObject.PointerPressed += (o, e) => OnPointerPressed(e);
            nativeObject.PointerReleased += (o, e) => OnPointerReleased(e);

            Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Called when this instance is ready to arrange its children.
        /// Derived classes wishing to define their own arrangement behavior should override <see cref="M:ArrangeOverride"/>.
        /// </summary>
        /// <param name="frame">The final rendering frame in which this instance should arrange its children.</param>
        protected sealed override void ArrangeCore(Rectangle frame)
        {
            frame.X += Margin.Left;
            frame.Y += Margin.Top;
            frame.Width -= (Margin.Left + Margin.Right);
            frame.Height -= (Margin.Top + Margin.Bottom);

            if (Visibility == Visibility.Collapsed)
            {
                nativeObject.Frame = new Rectangle(frame.TopLeft, Size.Empty);
                return;
            }

            Size constraints = new Size(Math.Max(MinWidth, Math.Min(MaxWidth, double.IsNaN(Width) ? frame.Width : Width)),
                Math.Max(MinHeight, Math.Min(MaxHeight, double.IsNaN(Height) ? frame.Height : Height)));
            
            var renderSize = ArrangeOverride(constraints);

            if (double.IsNaN(renderSize.Width) || double.IsInfinity(renderSize.Width))
            {
                throw new InvalidOperationException(Resources.Strings.InvalidWidthReturnedFromArrangement);
            }

            if (double.IsNaN(renderSize.Height) || double.IsInfinity(renderSize.Height))
            {
                throw new InvalidOperationException(Resources.Strings.InvalidHeightReturnedFromArrangement);
            }

            renderSize.Width = Math.Max(MinWidth, Math.Min(MaxWidth, renderSize.Width));
            renderSize.Height = Math.Max(MinHeight, Math.Min(MaxHeight, renderSize.Height));

            switch (HorizontalAlignment)
            {
                case HorizontalAlignment.Center:
                    frame.X += (frame.Width - renderSize.Width) / 2;
                    break;
                case HorizontalAlignment.Right:
                    frame.X += frame.Width - renderSize.Width;
                    break;
                case HorizontalAlignment.Stretch:
                    renderSize.Width = constraints.Width;
                    frame.X += (frame.Width - renderSize.Width) / 2;
                    break;
            }

            switch (VerticalAlignment)
            {
                case VerticalAlignment.Bottom:
                    frame.Y += frame.Height - renderSize.Height;
                    break;
                case VerticalAlignment.Center:
                    frame.Y += (frame.Height - renderSize.Height) / 2;
                    break;
                case VerticalAlignment.Stretch:
                    renderSize.Height = constraints.Height;
                    frame.Y += frame.Height - renderSize.Height;
                    break;
            }

            nativeObject.Frame = new Rectangle(frame.X, frame.Y, Math.Max(renderSize.Width, 0), Math.Max(renderSize.Height, 0));
        }

        /// <summary>
        /// Called when this instance is ready to arrange its children and returns the final rendering size of the object.
        /// </summary>
        /// <param name="constraints">The width and height that this instance should not exceed.</param>
        /// <returns>The final rendering size of the object as a <see cref="Size"/> instance.</returns>
        protected virtual Size ArrangeOverride(Size constraints)
        {
            Size desiredSize = DesiredSize;
            desiredSize.Width -= (Margin.Left + Margin.Right);
            desiredSize.Height -= (Margin.Top + Margin.Bottom);

            Size renderSize = Size.Empty;
            renderSize.Width = Math.Max(MinWidth, Math.Min(MaxWidth, HorizontalAlignment == HorizontalAlignment.Stretch ?
                Math.Max(desiredSize.Width, constraints.Width) : Math.Min(desiredSize.Width, constraints.Width)));

            renderSize.Height = Math.Max(MinHeight, Math.Min(MaxHeight, VerticalAlignment == VerticalAlignment.Stretch ?
                Math.Max(desiredSize.Height, constraints.Height) : Math.Min(desiredSize.Height, constraints.Height)));

            return renderSize;
        }

        /// <summary>
        /// Called when this instance is ready to be measured and returns the desired size of the object.
        /// Derived classes wishing to define their own measurement behavior should override <see cref="M:MeasureOverride"/>.
        /// </summary>
        /// <param name="constraints">The width and height that this instance should not exceed.</param>
        /// <returns>The desired size of the object as a <see cref="Size"/> instance.</returns>
        protected sealed override Size MeasureCore(Size constraints)
        {
            if (Visibility == Visibility.Collapsed)
            {
                return Size.Empty;
            }

            constraints.Width = Math.Max(MinWidth, Math.Min(MaxWidth, double.IsNaN(Width) ? constraints.Width - Margin.Left - Margin.Right : Width));
            constraints.Height = Math.Max(MinHeight, Math.Min(MaxHeight, double.IsNaN(Height) ? constraints.Height - Margin.Top - Margin.Bottom : Height));

            var desiredSize = MeasureOverride(constraints);
            desiredSize.Width = Math.Max(MinWidth, Math.Min(MaxWidth, double.IsNaN(Width) ? desiredSize.Width : Width)) + Margin.Left + Margin.Right;
            desiredSize.Height = Math.Max(MinHeight, Math.Min(MaxHeight, double.IsNaN(Height) ? desiredSize.Height : Height)) + Margin.Top + Margin.Bottom;

            return desiredSize;
        }

        /// <summary>
        /// Called when this instance is ready to be measured and returns the desired size of the object.
        /// </summary>
        /// <param name="constraints">The width and height that this instance should not exceed.</param>
        /// <returns>The desired size of the object as a <see cref="Size"/> instance.</returns>
        protected virtual Size MeasureOverride(Size constraints)
        {
            var parent = Parent as IScrollable;
            if (parent != null)
            {
                if (parent.CanScrollHorizontally)
                {
                    constraints.Width = int.MaxValue;
                }

                if (parent.CanScrollVertically)
                {
                    constraints.Height = int.MaxValue;
                }
            }

            if (double.IsInfinity(constraints.Width))
            {
                constraints.Width = int.MaxValue;
            }

            if (double.IsInfinity(constraints.Height))
            {
                constraints.Height = int.MaxValue;
            }

            if (constraints.Width < 0)
            {
                constraints.Width = 0;
            }

            if (constraints.Height < 0)
            {
                constraints.Height = 0;
            }

            if (!double.IsNaN(Width) && Width < constraints.Width)
            {
                constraints.Width = Width;
            }

            if (!double.IsNaN(Height) && Height < constraints.Height)
            {
                constraints.Height = Height;
            }

            constraints.Width = Math.Max(MinWidth, Math.Min(MaxWidth, constraints.Width));
            constraints.Height = Math.Max(MinHeight, Math.Min(MaxHeight, constraints.Height));

            return nativeObject.Measure(constraints);
        }

        /// <summary>
        /// Called when the system loses track of the pointer and raises the <see cref="PointerCanceled"/> event.
        /// </summary>
        /// <param name="e">The event arguments containing details about the pointer.</param>
        protected virtual void OnPointerCanceled(PointerEventArgs e)
        {
            PointerCanceled?.Invoke(this, e);
            if (e == null || !e.IsHandled)
            {
                var parent = VisualTreeHelper.GetParent<Element>(this);
                parent?.OnPointerCanceled(e == null ? null : new PointerEventArgs(e.Source, e.PointerType,
                    TranslatePointToAncestor(e.Position, parent), e.Pressure, e.Timestamp));
            }
        }

        /// <summary>
        /// Called when the pointer has moved while over the element and raises the <see cref="PointerMoved"/> event.
        /// </summary>
        /// <param name="e">The event arguments containing details about the pointer.</param>
        protected virtual void OnPointerMoved(PointerEventArgs e)
        {
            PointerMoved?.Invoke(this, e);
            if (e == null || !e.IsHandled)
            {
                var parent = VisualTreeHelper.GetParent<Element>(this);
                parent?.OnPointerMoved(e == null ? null : new PointerEventArgs(e.Source, e.PointerType,
                    TranslatePointToAncestor(e.Position, parent), e.Pressure, e.Timestamp));
            }
        }

        /// <summary>
        /// Called when the pointer has been pressed while over the element and raises the <see cref="PointerPressed"/> event.
        /// </summary>
        /// <param name="e">The event arguments containing details about the pointer.</param>
        protected virtual void OnPointerPressed(PointerEventArgs e)
        {
            PointerPressed?.Invoke(this, e);
            if (e == null || !e.IsHandled)
            {
                var parent = VisualTreeHelper.GetParent<Element>(this);
                parent?.OnPointerPressed(e == null ? null : new PointerEventArgs(e.Source, e.PointerType,
                    TranslatePointToAncestor(e.Position, parent), e.Pressure, e.Timestamp));
            }
        }

        /// <summary>
        /// Called when the pointer has been released while over the element and raises the <see cref="PointerReleased"/> event.
        /// </summary>
        /// <param name="e">The event arguments containing details about the pointer.</param>
        protected virtual void OnPointerReleased(PointerEventArgs e)
        {
            PointerReleased?.Invoke(this, e);
            if (e == null || !e.IsHandled)
            {
                var parent = VisualTreeHelper.GetParent<Element>(this);
                parent?.OnPointerReleased(e == null ? null : new PointerEventArgs(e.Source, e.PointerType,
                    TranslatePointToAncestor(e.Position, parent), e.Pressure, e.Timestamp));
            }
        }
    }
}
