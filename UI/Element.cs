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
using Prism.Input;
using Prism.Native;
using Prism.Resources;

namespace Prism.UI
{
    /// <summary>
    /// Represents the base class for UI elements.  This class is abstract.
    /// </summary>
    public abstract class Element : Visual
    {
        #region Event Descriptors
        /// <summary>
        /// Gets an <see cref="EventDescriptor"/> describing the <see cref="E:PointerCanceled"/> event.
        /// </summary>
        public static EventDescriptor PointerCanceledEvent { get; } = EventDescriptor.Create(nameof(PointerCanceled), typeof(TypedEventHandler<Element, PointerEventArgs>), typeof(Element));

        /// <summary>
        /// Gets an <see cref="EventDescriptor"/> describing the <see cref="E:PointerMoved"/> event.
        /// </summary>
        public static EventDescriptor PointerMovedEvent { get; } = EventDescriptor.Create(nameof(PointerMoved), typeof(TypedEventHandler<Element, PointerEventArgs>), typeof(Element));

        /// <summary>
        /// Gets an <see cref="EventDescriptor"/> describing the <see cref="E:PointerPressed"/> event.
        /// </summary>
        public static EventDescriptor PointerPressedEvent { get; } = EventDescriptor.Create(nameof(PointerPressed), typeof(TypedEventHandler<Element, PointerEventArgs>), typeof(Element));

        /// <summary>
        /// Gets an <see cref="EventDescriptor"/> describing the <see cref="E:PointerReleased"/> event.
        /// </summary>
        public static EventDescriptor PointerReleasedEvent { get; } = EventDescriptor.Create(nameof(PointerReleased), typeof(TypedEventHandler<Element, PointerEventArgs>), typeof(Element));
        #endregion

        #region Property Descriptors
        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:Height"/> property.
        /// </summary>
        public static PropertyDescriptor HeightProperty { get; } = PropertyDescriptor.Create(nameof(Height), typeof(double), typeof(Element), new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:HorizontalAlignment"/> property.
        /// </summary>
        public static PropertyDescriptor HorizontalAlignmentProperty { get; } = PropertyDescriptor.Create(nameof(HorizontalAlignment), typeof(HorizontalAlignment), typeof(Element), new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.AffectsParentArrange));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:Margin"/> property.
        /// </summary>
        public static PropertyDescriptor MarginProperty { get; } = PropertyDescriptor.Create(nameof(Margin), typeof(Thickness), typeof(Element), new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:MaxHeight"/> property.
        /// </summary>
        public static PropertyDescriptor MaxHeightProperty { get; } = PropertyDescriptor.Create(nameof(MaxHeight), typeof(double), typeof(Element), new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:MaxWidth"/> property.
        /// </summary>
        public static PropertyDescriptor MaxWidthProperty { get; } = PropertyDescriptor.Create(nameof(MaxWidth), typeof(double), typeof(Element), new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:MinHeight"/> property.
        /// </summary>
        public static PropertyDescriptor MinHeightProperty { get; } = PropertyDescriptor.Create(nameof(MinHeight), typeof(double), typeof(Element), new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:MinWidth"/> property.
        /// </summary>
        public static PropertyDescriptor MinWidthProperty { get; } = PropertyDescriptor.Create(nameof(MinWidth), typeof(double), typeof(Element), new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:Name"/> property.
        /// </summary>
        public static PropertyDescriptor NameProperty { get; } = PropertyDescriptor.Create(nameof(Name), typeof(string), typeof(Element));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:Opacity"/> property.
        /// </summary>
        public static PropertyDescriptor OpacityProperty { get; } = PropertyDescriptor.Create(nameof(Opacity), typeof(double), typeof(Element));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:VerticalAlignment"/> property.
        /// </summary>
        public static PropertyDescriptor VerticalAlignmentProperty { get; } = PropertyDescriptor.Create(nameof(VerticalAlignment), typeof(VerticalAlignment), typeof(Element), new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.AffectsParentArrange));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:Visibility"/> property.
        /// </summary>
        public static PropertyDescriptor VisibilityProperty { get; } = PropertyDescriptor.Create(nameof(Visibility), typeof(Visibility), typeof(Element));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:Width"/> property.
        /// </summary>
        public static PropertyDescriptor WidthProperty { get; } = PropertyDescriptor.Create(nameof(Width), typeof(double), typeof(Element), new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.AffectsMeasure));
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
        /// Gets a collection of the gesture recognizers that are attached to the element.
        /// </summary>
        public GestureRecognizerCollection GestureRecognizers { get; }

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
                        throw new ArgumentException(Strings.ValueCannotBeInfinity, nameof(Height));
                    }

                    if (value < 0)
                    {
                        throw new ArgumentOutOfRangeException(nameof(Height), Strings.ValueCannotBeLessThanZero);
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
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", Justification = "Exception parameter refers to property name for easier understanding of invalid value.")]
        public Thickness Margin
        {
            get { return margin; }
            set
            {
                if (value != margin)
                {
                    if (double.IsNaN(value.Left) || double.IsInfinity(value.Left) || double.IsNaN(value.Top) || double.IsInfinity(value.Top) ||
                        double.IsNaN(value.Right) || double.IsInfinity(value.Right) || double.IsNaN(value.Bottom) || double.IsInfinity(value.Bottom))
                    {
                        throw new ArgumentException(Strings.ThicknessContainsNaNOrInfiniteValue, nameof(Margin));
                    }

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
                        throw new ArgumentException(Strings.ValueCannotBeNaNOrNegativeInfinity, nameof(MaxHeight));
                    }

                    if (value < 0)
                    {
                        throw new ArgumentOutOfRangeException(nameof(MaxHeight), Strings.ValueCannotBeLessThanZero);
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
                        throw new ArgumentException(Strings.ValueCannotBeNaNOrNegativeInfinity, nameof(MaxWidth));
                    }

                    if (value < 0)
                    {
                        throw new ArgumentOutOfRangeException(nameof(MaxWidth), Strings.ValueCannotBeLessThanZero);
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
                        throw new ArgumentException(Strings.ValueCannotBeNaNOrInfinity, nameof(MinHeight));
                    }

                    if (value < 0)
                    {
                        throw new ArgumentOutOfRangeException(nameof(MinHeight), Strings.ValueCannotBeLessThanZero);
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
                        throw new ArgumentException(Strings.ValueCannotBeNaNOrInfinity, nameof(MinWidth));
                    }

                    if (value < 0)
                    {
                        throw new ArgumentOutOfRangeException(nameof(MinWidth), Strings.ValueCannotBeLessThanZero);
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
        /// Gets or sets the level of opacity for the element.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", Justification = "Exception parameter refers to property name for easier understanding of invalid value.")]
        public double Opacity
        {
            get { return nativeObject.Opacity; }
            set
            {
                if (double.IsNaN(value) || double.IsInfinity(value))
                {
                    throw new ArgumentException(Strings.ValueCannotBeNaNOrInfinity, nameof(Opacity));
                }

                nativeObject.Opacity = Math.Max(0, Math.Min(1, value));
            }
        }

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
                    var parent = Parent as Visual;
                    if (parent == null)
                    {
                        InvalidateMeasure();
                    }
                    else
                    {
                        parent.InvalidateMeasure();
                    }
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
                        throw new ArgumentException(Strings.ValueCannotBeInfinity, nameof(Width));
                    }

                    if (value < 0)
                    {
                        throw new ArgumentOutOfRangeException(nameof(Width), Strings.ValueCannotBeLessThanZero);
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
        /// Initializes a new instance of the <see cref="Element"/> class and pairs it with the specified native object.
        /// </summary>
        /// <param name="nativeObject">The native object with which to pair this instance.</param>
        /// <exception cref="ArgumentException">Thrown when a <see cref="ResolveAttribute"/> is located in the inheritance chain and <paramref name="nativeObject"/> doesn't match the type specified by the attribute.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="nativeObject"/> is <c>null</c>.</exception>
        protected Element(INativeElement nativeObject)
            : base(nativeObject)
        {
            this.nativeObject = nativeObject;

            GestureRecognizers = new GestureRecognizerCollection(this);
            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Element"/> class and pairs it with a native object that is resolved from the IoC container.
        /// At least one class in the inheritance chain must be decorated with a <see cref="ResolveAttribute"/> or an exception will be thrown.
        /// </summary>
        /// <param name="resolveParameters">Any parameters to pass along to the constructor of the native type.</param>
        /// <exception cref="TypeResolutionException">Thrown when the native object does not resolve to an <see cref="INativeElement"/> instance.</exception>
        protected Element(ResolveParameter[] resolveParameters)
            : base(resolveParameters)
        {
            nativeObject = ObjectRetriever.GetNativeObject(this) as INativeElement;
            if (nativeObject == null)
            {
                throw new TypeResolutionException(string.Format(CultureInfo.CurrentCulture, Strings.TypeMustResolveToType,
                    ObjectRetriever.GetNativeObject(this).GetType().FullName, typeof(INativeElement).FullName));
            }

            GestureRecognizers = new GestureRecognizerCollection(this);
            Initialize();
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
            frame.Width = Math.Max(frame.Width - (Margin.Left + Margin.Right), 0);
            frame.Height = Math.Max(frame.Height - (Margin.Top + Margin.Bottom), 0);

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
                throw new InvalidOperationException(Strings.InvalidWidthReturnedFromArrangement);
            }

            if (double.IsNaN(renderSize.Height) || double.IsInfinity(renderSize.Height))
            {
                throw new InvalidOperationException(Strings.InvalidHeightReturnedFromArrangement);
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
            desiredSize.Width = Math.Max(desiredSize.Width - (Margin.Left + Margin.Right), 0);
            desiredSize.Height = Math.Max(desiredSize.Height - (Margin.Top + Margin.Bottom), 0);

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
                parent?.OnPointerCanceled(e == null ? null : new PointerEventArgs(e.Source, e.PointerId, e.PointerType,
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
                parent?.OnPointerMoved(e == null ? null : new PointerEventArgs(e.Source, e.PointerId, e.PointerType,
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
                parent?.OnPointerPressed(e == null ? null : new PointerEventArgs(e.Source, e.PointerId, e.PointerType,
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
                parent?.OnPointerReleased(e == null ? null : new PointerEventArgs(e.Source, e.PointerId, e.PointerType,
                    TranslatePointToAncestor(e.Position, parent), e.Pressure, e.Timestamp));
            }
        }

        private void Initialize()
        {
            nativeObject.PointerCanceled += (o, e) => OnPointerCanceled(e);
            nativeObject.PointerMoved += (o, e) => OnPointerMoved(e);
            nativeObject.PointerPressed += (o, e) => OnPointerPressed(e);
            nativeObject.PointerReleased += (o, e) => OnPointerReleased(e);

            Visibility = Visibility.Visible;
        }
    }
}
