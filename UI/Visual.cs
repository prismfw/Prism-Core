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
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Prism.Data;
using Prism.Native;
using Prism.Resources;
using Prism.UI.Media;

namespace Prism.UI
{
    /// <summary>
    /// Represents the base class for platform-agnostic objects that are attached to a visual tree and rendered to a screen.
    /// This class is abstract.
    /// </summary>
    public abstract class Visual : FrameworkObject, IDataContext
    {
        #region Event Descriptors
        /// <summary>
        /// Gets an <see cref="EventDescriptor"/> describing the <see cref="E:DataContextChanged"/> event.
        /// </summary>
        public static EventDescriptor DataContextChangedEvent { get; } = EventDescriptor.Create(nameof(DataContextChanged), typeof(TypedEventHandler<IDataContext, DataContextChangedEventArgs>), typeof(Visual));

        /// <summary>
        /// Gets an <see cref="EventDescriptor"/> describing the <see cref="E:Loaded"/> event.
        /// </summary>
        public static EventDescriptor LoadedEvent { get; } = EventDescriptor.Create(nameof(Loaded), typeof(TypedEventHandler<Visual>), typeof(Visual));

        /// <summary>
        /// Gets an <see cref="EventDescriptor"/> describing the <see cref="E:Unloaded"/> event.
        /// </summary>
        public static EventDescriptor UnloadedEvent { get; } = EventDescriptor.Create(nameof(Unloaded), typeof(TypedEventHandler<Visual>), typeof(Visual));
        #endregion

        #region Property Descriptors
        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:AreAnimationsEnabled"/> property.
        /// </summary>
        public static PropertyDescriptor AreAnimationsEnabledProperty { get; } = PropertyDescriptor.Create(nameof(AreAnimationsEnabled), typeof(bool), typeof(Visual));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:DataContext"/> property.
        /// </summary>
        public static PropertyDescriptor DataContextProperty { get; } = PropertyDescriptor.Create(nameof(DataContext), typeof(object), typeof(Visual));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:DesiredSize"/> property.
        /// </summary>
        public static PropertyDescriptor DesiredSizeProperty { get; } = PropertyDescriptor.Create(nameof(DesiredSize), typeof(Size), typeof(Visual), true);

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:IsHitTestVisible"/> property.
        /// </summary>
        public static PropertyDescriptor IsHitTestVisibleProperty { get; } = PropertyDescriptor.Create(nameof(IsHitTestVisible), typeof(bool), typeof(Visual));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:IsLoaded"/> property.
        /// </summary>
        public static PropertyDescriptor IsLoadedProperty { get; } = PropertyDescriptor.Create(nameof(IsLoaded), typeof(bool), typeof(Visual), true);

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:RenderSize"/> property.
        /// </summary>
        public static PropertyDescriptor RenderSizeProperty { get; } = PropertyDescriptor.Create(nameof(RenderSize), typeof(Size), typeof(Visual), true);

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:RenderTransform"/> property.
        /// </summary>
        public static PropertyDescriptor RenderTransformProperty { get; } = PropertyDescriptor.Create(nameof(RenderTransform), typeof(Transform), typeof(Visual));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:RequestedTheme"/> property.
        /// </summary>
        public static PropertyDescriptor RequestedThemeProperty { get; } = PropertyDescriptor.Create(nameof(RequestedTheme), typeof(Theme), typeof(Visual));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:Tag"/> property.
        /// </summary>
        public static PropertyDescriptor TagProperty { get; } = PropertyDescriptor.Create(nameof(Tag), typeof(object), typeof(Visual));
        #endregion

        /// <summary>
        /// Occurs when the value of the <see cref="P:DataContext"/> property has changed.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification = "Event handler provides a strongly-typed sender for easier use.")]
        public event TypedEventHandler<IDataContext, DataContextChangedEventArgs> DataContextChanged;

        /// <summary>
        /// Occurs when this instance has been attached to the visual tree and is ready to be rendered.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification = "Event handler provides a strongly-typed sender for easier use.")]
        public event TypedEventHandler<Visual> Loaded;

        /// <summary>
        /// Occurs when this instance has been detached from the visual tree.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification = "Event handler provides a strongly-typed sender for easier use.")]
        public event TypedEventHandler<Visual> Unloaded;

        /// <summary>
        /// Gets or sets a value indicating whether animations are enabled for this instance.
        /// </summary>
        public bool AreAnimationsEnabled
        {
            get { return nativeObject.AreAnimationsEnabled; }
            set { nativeObject.AreAnimationsEnabled = value; }
        }

        /// <summary>
        /// Gets or sets the object to use as the default Source value when binding to this instance or any of its children.
        /// </summary>
        public object DataContext
        {
            get { return dataContext; }
            set
            {
                if (value != dataContext)
                {
                    var oldContext = dataContext;

                    dataContext = value;
                    OnPropertyChanged(DataContextProperty);
                    DataContextChanged?.Invoke(this, new DataContextChangedEventArgs(oldContext, dataContext));
                }
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private object dataContext;

        /// <summary>
        /// Gets the size that this instance computed during the measure pass of the layout process.
        /// </summary>
        public Size DesiredSize { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the current positions and sizes of child elements within this instance are valid.
        /// </summary>
        public bool IsArrangeValid { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance can be considered a valid result for hit testing.
        /// </summary>
        public bool IsHitTestVisible
        {
            get { return nativeObject.IsHitTestVisible; }
            set { nativeObject.IsHitTestVisible = value; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has been loaded and is ready for rendering.
        /// </summary>
        public bool IsLoaded
        {
            get { return nativeObject.IsLoaded; }
        }

        /// <summary>
        /// Gets a value indicating whether the current desired size of this instance is valid.
        /// </summary>
        public bool IsMeasureValid { get; private set; }

        /// <summary>
        /// Gets the visual parent for this instance.
        /// </summary>
        public object Parent
        {
            get { return parent ?? VisualTreeHelper.GetParent(this, p => p is Visual || p is IView || p is Window); }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private object parent;

        /// <summary>
        /// Gets the rendering size of this instance.
        /// </summary>
        public Size RenderSize { get; private set; }

        /// <summary>
        /// Gets or sets transformation information that affects the rendering position of this instance.
        /// </summary>
        public Transform RenderTransform
        {
            get { return ObjectRetriever.GetAgnosticObject(nativeObject.RenderTransform) as Transform; }
            set { nativeObject.RenderTransform = ObjectRetriever.GetNativeObject(value) as INativeTransform; }
        }

        /// <summary>
        /// Gets or sets the visual theme that should be used by this instance.
        /// </summary>
        public Theme RequestedTheme
        {
            get { return nativeObject.RequestedTheme; }
            set
            {
                if (value != nativeObject.RequestedTheme)
                {
                    nativeObject.RequestedTheme = value;

                    OnPropertyChanged(RequestedThemeProperty);
                    PropagateResourceCollectionChange(this);
                }
            }
        }

        /// <summary>
        /// Gets or sets a collection of resources scoped to this instance and its children.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Consuming developers should be able to use their own dictionaries.")]
        public ResourceDictionary Resources
        {
            get
            {
                if (resources == null)
                {
                    resources = new ResourceDictionary();
                    resources.ResourceChanged += OnResourceChanged;
                    resources.ResourceCollectionChanged += OnResourceCollectionChanged;
                }

                return resources;
            }
            set
            {
                if (value != resources)
                {
                    if (resources != null)
                    {
                        resources.ResourceChanged -= OnResourceChanged;
                        resources.ResourceCollectionChanged -= OnResourceCollectionChanged;
                    }
                    
                    resources = value;
                    if (resources != null)
                    {
                        resources.ResourceChanged -= OnResourceChanged;
                        resources.ResourceChanged += OnResourceChanged;

                        resources.ResourceCollectionChanged -= OnResourceCollectionChanged;
                        resources.ResourceCollectionChanged += OnResourceCollectionChanged;
                    }

                    PropagateResourceCollectionChange(this);
                }
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ResourceDictionary resources;

        /// <summary>
        /// Gets or sets an arbitrary object that can be used for attaching custom information to this instance.
        /// </summary>
        public object Tag
        {
            get { return tag; }
            set
            {
                if (value != tag)
                {
                    tag = value;
                    OnPropertyChanged(TagProperty);
                }
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private object tag;

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        // this field is to avoid casting
        private readonly INativeVisual nativeObject;
        
#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private bool isArranging;
#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private bool isMeasuring;
#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private List<ResourceReference> resourceReferences;

#if METRICS
        private static int arrangeRequestCount, arrangePerformCount, arrangeSkipCount;
        private static int measureRequestCount, measurePerformCount, measureSkipCount;
#endif

        /// <summary>
        /// Initializes a new instance of the <see cref="Visual"/> class and pairs it with the specified native object.
        /// </summary>
        /// <param name="nativeObject">The native object with which to pair this instance.</param>
        /// <exception cref="ArgumentException">Thrown when a <see cref="ResolveAttribute"/> is located in the inheritance chain and <paramref name="nativeObject"/> doesn't match the type specified by the attribute.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="nativeObject"/> is <c>null</c>.</exception>
        protected Visual(INativeVisual nativeObject)
            : base(nativeObject)
        {
            this.nativeObject = nativeObject;

            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Visual"/> class and pairs it with a native object that is resolved from the IoC container.
        /// At least one class in the inheritance chain must be decorated with a <see cref="ResolveAttribute"/> or an exception will be thrown.
        /// </summary>
        /// <param name="resolveParameters">Any parameters to pass along to the constructor of the native type.</param>
        /// <exception cref="TypeResolutionException">Thrown when the native object does not resolve to an <see cref="INativeVisual"/> instance.</exception>
        protected Visual(ResolveParameter[] resolveParameters)
            : base(resolveParameters)
        {
            nativeObject = ObjectRetriever.GetNativeObject(this) as INativeVisual;
            if (nativeObject == null)
            {
                throw new TypeResolutionException(string.Format(CultureInfo.CurrentCulture, Strings.TypeMustResolveToType,
                    ObjectRetriever.GetNativeObject(this).GetType().FullName, typeof(INativeVisual).FullName));
            }

            Initialize();
        }

        /// <summary>
        /// Positions and sizes child elements within this instance.
        /// </summary>
        /// <param name="frame">The final rendering frame in which this instance should arrange its children.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="frame"/> contains a NaN or infinite value -or- when <paramref name="frame"/> size contains a negative value.</exception>
        public void Arrange(Rectangle frame)
        {
            isArranging = true;
            IsArrangeValid = false;

#if METRICS
            Utilities.Logger.Info(CultureInfo.CurrentCulture, "Layout Metrics: Arrangement performed {0} times (from {1} total requests)", ++arrangePerformCount, ++arrangeRequestCount);
#endif

            if (double.IsNaN(frame.X) || double.IsNaN(frame.Y) || double.IsNaN(frame.Width) || double.IsNaN(frame.Height) ||
                double.IsInfinity(frame.X) || double.IsInfinity(frame.Y) || double.IsInfinity(frame.Width) || double.IsInfinity(frame.Height))
            {
                throw new ArgumentException(Strings.RectangleContainsNaNOrInfiniteValue, nameof(frame));
            }

            if (frame.Width < 0 || frame.Height < 0)
            {
                throw new ArgumentException(Strings.RectangleSizeContainsNegativeValue, nameof(frame));
            }

            if (!IsMeasureValid)
            {
                Measure(frame.Size);
            }

            var oldFrame = nativeObject.Frame;
            ArrangeCore(frame);

            IsArrangeValid = true;
            isArranging = false;

            var newFrame = nativeObject.Frame;
            if (RenderSize != newFrame.Size)
            {
                RenderSize = newFrame.Size;
                OnPropertyChanged(RenderSizeProperty);
            }

            if (oldFrame != newFrame)
            {
                var parentVisual = Parent as Visual;
                if (parentVisual != null && !parentVisual.isArranging)
                {
                    VisualTreeHelper.GetParent<Visual>(this, (o) => !(o.Parent is Visual))?.InvalidateArrange();
                }
            }
        }

        /// <summary>
        /// Deactivates and removes from this instance the binding with the target property described by the specified <see cref="PropertyDescriptor"/>.
        /// </summary>
        /// <param name="property">The <see cref="PropertyDescriptor"/> describing the target property of the binding to remove.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="property"/> is <c>null</c>.</exception>
        public void ClearBinding(PropertyDescriptor property)
        {
            BindingOperations.ClearBinding(this, property);
        }

        /// <summary>
        /// Searches for a resource with the specified key and throws an exception if the resource cannot be found.
        /// </summary>
        /// <param name="resourceKey">The key of the resource to find.</param>
        /// <returns>The requested resource object.</returns>
        /// <exception cref="ArgumentException">Thrown when a resource for the specified <paramref name="resourceKey"/> cannot be found.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="resourceKey"/> is <c>null</c>.</exception>
        public object FindResource(object resourceKey)
        {
            if (resourceKey == null)
            {
                throw new ArgumentNullException(nameof(resourceKey));
            }

            object retval;
            if (resources != null && resources.TryGetResource(this, resourceKey, out retval, false))
            {
                return retval;
            }

            var parentVisual = this;
            do
            {
                parentVisual = VisualTreeHelper.GetParent<Visual>(parentVisual, p => p.resources != null);
                if (parentVisual != null && parentVisual.resources.TryGetResource(this, resourceKey, out retval, false))
                {
                    return retval;
                }
            }
            while (parentVisual != null);

            if (Application.Current.Resources.TryGetResource(this, resourceKey, out retval, true))
            {
                return retval;
            }

            throw new ArgumentException(Strings.ResourceCouldNotBeFound, nameof(resourceKey));
        }

        /// <summary>
        /// Gets from this instance the binding with the target property described by the specified <see cref="PropertyDescriptor"/>.
        /// </summary>
        /// <param name="property">The <see cref="PropertyDescriptor"/> describing the target property of the binding to retrieve.</param>
        /// <returns>The <see cref="BindingBase"/> instance, or <c>null</c> if no instance was found.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="property"/> is <c>null</c>.</exception>
        public BindingBase GetBinding(PropertyDescriptor property)
        {
            return BindingOperations.GetBinding(this, property);
        }

        /// <summary>
        /// Invalidates the arrangement of child elements within this instance.
        /// </summary>
        public void InvalidateArrange()
        {
            IsArrangeValid = false;
            nativeObject.InvalidateArrange();
        }

        /// <summary>
        /// Invalidates the measurement state of this instance.
        /// </summary>
        public void InvalidateMeasure()
        {
            IsMeasureValid = false;
            nativeObject.InvalidateMeasure();
        }

        /// <summary>
        /// Measures this instance and any child elements to determine a desired size for the object.
        /// </summary>
        /// <param name="constraints">The width and height that this instance should not exceed.</param>
        public void Measure(Size constraints)
        {
            isMeasuring = true;
            IsMeasureValid = false;

#if METRICS
            Utilities.Logger.Info(CultureInfo.CurrentCulture, "Layout Metrics: Measurement performed {0} times (from {1} total requests)", ++measurePerformCount, ++measureRequestCount);
#endif

            if (double.IsNaN(constraints.Width) || double.IsNegativeInfinity(constraints.Width))
            {
                constraints.Width = double.PositiveInfinity;
            }

            if (double.IsNaN(constraints.Height) || double.IsNegativeInfinity(constraints.Height))
            {
                constraints.Height = double.PositiveInfinity;
            }

            var size = MeasureCore(constraints);

            if (double.IsNaN(size.Width) || double.IsInfinity(size.Width))
            {
                throw new InvalidOperationException(Strings.InvalidWidthReturnedFromMeasurement);
            }

            if (double.IsNaN(size.Height) || double.IsInfinity(size.Height))
            {
                throw new InvalidOperationException(Strings.InvalidHeightReturnedFromMeasurement);
            }

            if (size.Width < 0)
            {
                size.Width = 0;
            }

            if (size.Height < 0)
            {
                size.Height = 0;
            }

            IsMeasureValid = true;
            isMeasuring = false;

            if (size != DesiredSize)
            {
                DesiredSize = size;
                OnPropertyChanged(DesiredSizeProperty);

                var parentVisual = Parent as Visual;
                if (parentVisual != null)
                {
                    if (!parentVisual.isMeasuring)
                    {
                        VisualTreeHelper.GetParent<Visual>(this, (o) => !(o.Parent is Visual))?.InvalidateMeasure();
                    }
                    if (!parentVisual.isArranging)
                    {
                        VisualTreeHelper.GetParent<Visual>(this, (o) => !(o.Parent is Visual))?.InvalidateArrange();
                    }
                }
                else if (IsArrangeValid)
                {
                    InvalidateArrange();
                }
            }
        }

        /// <summary>
        /// Sets the specified <see cref="BindingBase"/> with this instance as the target.
        /// </summary>
        /// <param name="property">The <see cref="PropertyDescriptor"/> describing the target property.</param>
        /// <param name="binding">The binding to be set.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="property"/> is <c>null</c> -or- when <paramref name="binding"/> is <c>null</c>.</exception>
        public void SetBinding(PropertyDescriptor property, BindingBase binding)
        {
            BindingOperations.SetBinding(this, property, binding);
        }

        /// <summary>
        /// Binds the property described by the specified <see cref="PropertyDescriptor"/> to the resource that is associated with the specified key.
        /// </summary>
        /// <param name="property">The <see cref="PropertyDescriptor"/> describing the property to bind to the resource.</param>
        /// <param name="resourceKey">The key of the resource that is to be bound to the property.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="property"/> does not describe a valid property on this instance.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="property"/> is <c>null</c>.</exception>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Exact error types can be unpredictable but should not interfere with execution of the program.  The error is logged to facilitate debugging.")]
        public void SetResourceReference(PropertyDescriptor property, object resourceKey)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }
            
            if (!property.OwnerType.GetTypeInfo().IsAssignableFrom(GetType().GetTypeInfo()))
            {
                throw new ArgumentException(Strings.OwnerTypeDoesNotMatchCurrentType);
            }

            if (resourceKey == null)
            {
                resourceReferences.RemoveAll(r => r.Property == property);
                return;
            }

            if (resourceReferences == null)
            {
                resourceReferences = new List<ResourceReference>();
            }

            var resourceRef = resourceReferences.FirstOrDefault(r => r.Property == property);
            if (resourceRef == null)
            {
                resourceRef = new ResourceReference(property);
                resourceReferences.Add(resourceRef);
            }

            resourceRef.Key = resourceKey;

            try
            {
                property.SetValue(this, TryFindResource(resourceKey), null);
                resourceRef.Value = property.GetValue(this, null);
            }
            catch (Exception e)
            {
                Utilities.Logger.Error(CultureInfo.CurrentCulture, Strings.FailedToSetResourceValueOnProperty, resourceRef.Value, resourceRef.Property.Name, e);
                resourceReferences.Remove(resourceRef);
            }
        }

        /// <summary>
        /// Translates a <see cref="Point"/> relative to this instance to coordinates relative to the specified ancestor of this instance.
        /// </summary>
        /// <param name="point">The point, relative to this instance, that is to be translated.</param>
        /// <param name="ancestor">The ancestor of this instance into which the point is to be translated.</param>
        /// <returns>A <see cref="Point"/> containing the translated values.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="ancestor"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="ancestor"/> is not an ancestor of this instance.</exception>
        public Point TranslatePointToAncestor(Point point, Visual ancestor)
        {
            if (ancestor == null)
            {
                throw new ArgumentNullException(nameof(ancestor));
            }

            var instance = this;
            do
            {
                point += instance.nativeObject.Frame.TopLeft;
                instance = VisualTreeHelper.GetParent<Visual>(instance);
            }
            while (instance != null && instance != ancestor);

            if (instance == null)
            {
                throw new ArgumentException(Strings.ObjectIsNotAnAncestor, nameof(ancestor));
            }

            return point;
        }

        /// <summary>
        /// Translates a <see cref="Point"/> relative to this instance to coordinates relative to the specified descendant of this instance.
        /// </summary>
        /// <param name="point">The point, relative to this instance, that is to be translated.</param>
        /// <param name="descendant">The descendant of this instance into which the point is to be translated.</param>
        /// <returns>A <see cref="Point"/> containing the translated values.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="descendant"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="descendant"/> is not a descendant of this instance.</exception>
        public Point TranslatePointToDescendant(Point point, Visual descendant)
        {
            if (descendant == null)
            {
                throw new ArgumentNullException(nameof(descendant));
            }

            do
            {
                point -= descendant.nativeObject.Frame.TopLeft;
                descendant = VisualTreeHelper.GetParent<Visual>(descendant);
            }
            while (descendant != null && descendant != this);

            if (descendant == null)
            {
                throw new ArgumentException(Strings.ObjectIsNotADescendant, nameof(descendant));
            }

            return point;
        }

        /// <summary>
        /// Searches for a resource with the specified key.
        /// </summary>
        /// <param name="resourceKey">The key of the resource to find.</param>
        /// <returns>The requested resource object, or <c>null</c> if no resource is found.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="resourceKey"/> is <c>null</c>.</exception>
        public object TryFindResource(object resourceKey)
        {
            if (resourceKey == null)
            {
                throw new ArgumentNullException(nameof(resourceKey));
            }

            object retval;
            if (resources != null && resources.TryGetResource(this, resourceKey, out retval, false))
            {
                return retval;
            }

            var parentVisual = this;
            do
            {
                parentVisual = VisualTreeHelper.GetParent<Visual>(parentVisual, p => p.resources != null);
                if (parentVisual != null && parentVisual.resources.TryGetResource(this, resourceKey, out retval, false))
                {
                    return retval;
                }
            }
            while (parentVisual != null);

            Application.Current.Resources.TryGetResource(this, resourceKey, out retval, true);
            return retval;
        }

        /// <summary>
        /// Called when this instance is ready to arrange its children.
        /// </summary>
        /// <param name="frame">The final rendering frame in which this instance should arrange its children.</param>
        protected virtual void ArrangeCore(Rectangle frame)
        {
            nativeObject.Frame = new Rectangle(frame.TopLeft,
                new Size(Math.Min(DesiredSize.Width, frame.Width), Math.Min(DesiredSize.Height, frame.Height)));
        }

        /// <summary>
        /// Called when this instance is ready to be measured and returns the desired size of the object.
        /// </summary>
        /// <param name="constraints">The width and height that this instance should not exceed.</param>
        /// <returns>The desired size of the object as a <see cref="Size"/> instance.</returns>
        protected virtual Size MeasureCore(Size constraints)
        {
            var parentVisual = Parent as IScrollable;
            if (parentVisual != null)
            {
                if (parentVisual.CanScrollHorizontally)
                {
                    constraints.Width = int.MaxValue;
                }

                if (parentVisual.CanScrollVertically)
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

            return nativeObject.Measure(constraints);
        }

        /// <summary>
        /// Called when this instance has been attached to the visual tree and raises the <see cref="Loaded"/> event.
        /// </summary>
        /// <param name="e">The event arguments for the event.</param>
        protected virtual void OnLoaded(EventArgs e)
        {
            Loaded?.Invoke(this, e);
        }

        /// <summary>
        /// Called when this instance has been detached from the visual tree and raises the <see cref="Unloaded"/> event.
        /// </summary>
        /// <param name="e">The event arguments for the event.</param>
        protected virtual void OnUnloaded(EventArgs e)
        {
            Unloaded?.Invoke(this, e);
        }

        internal static void PropagateResourceChange(object obj, object key)
        {
            if (obj == null)
            {
                return;
            }

            var visualObj = obj as Visual;
            if (visualObj?.resourceReferences != null)
            {
                for (int i = 0; i < visualObj.resourceReferences.Count; i++)
                {
                    var resourceRef = visualObj.resourceReferences[i];
                    if (resourceRef.IsKeyRelated(key))
                    {
                        if (!visualObj.UpdateResourceReference(resourceRef))
                        {
                            i--;
                        }
                    }
                }
            }

            int childCount = VisualTreeHelper.GetChildrenCount(obj);
            for (int i = 0; i < childCount; i++)
            {
                var child = VisualTreeHelper.GetChild(obj, i);
                if (!(child as Visual)?.resources?.ContainsKey(key) ?? true)
                {
                    PropagateResourceChange(child, key);
                }
            }
        }

        internal static void PropagateResourceCollectionChange(object obj)
        {
            if (obj == null)
            {
                return;
            }

            var visualObj = obj as Visual;
            if (visualObj?.resourceReferences != null)
            {
                for (int i = 0; i < visualObj.resourceReferences.Count; i++)
                {
                    if (!visualObj.UpdateResourceReference(visualObj.resourceReferences[i]))
                    {
                        i--;
                    }
                }
            }

            int childCount = VisualTreeHelper.GetChildrenCount(obj);
            for (int i = 0; i < childCount; i++)
            {
                PropagateResourceCollectionChange(VisualTreeHelper.GetChild(obj, i));
            }
        }

        internal virtual Size GetChildConstraints(Visual child)
        {
            return new Size(double.PositiveInfinity, double.PositiveInfinity);
        }

        private void Initialize()
        {
            nativeObject.ArrangeRequest = OnArrangeRequest;
            nativeObject.MeasureRequest = OnMeasureRequest;

            nativeObject.Loaded += OnLoad;
            nativeObject.Unloaded += OnUnload;

            AreAnimationsEnabled = true;
        }

        private void OnArrangeRequest(bool forceArrange, Rectangle? frameOverride)
        {
            if (!isArranging && !isMeasuring && (forceArrange || !IsArrangeValid))
            {
                Rectangle frame;
                if (frameOverride == null)
                {
                    frame = nativeObject.Frame;

                    var element = this as Element;
                    if (element != null)
                    {
                        frame.X = Math.Max(0, frame.X - element.Margin.Left);
                        frame.Y = Math.Max(0, frame.Y - element.Margin.Top);
                        frame.Width += element.Margin.Left + element.Margin.Right;
                        frame.Height += element.Margin.Top + element.Margin.Bottom;
                    }

                    if (Parent == null)
                    {
                        frame.Width = Math.Max(frame.Width, DesiredSize.Width);
                        frame.Height = Math.Max(frame.Height, DesiredSize.Height);
                    }
                    else
                    {
                        var window = Parent as Window;
                        if (window != null)
                        {
                            frame.Width = Math.Max(0, window.Width - frame.X);
                            frame.Height = Math.Max(0, window.Height - frame.Y);
                        }
                    }
                }
                else
                {
                    frame = frameOverride.Value;
                }

                Arrange(frame);
            }
#if METRICS
            else
            {
                Utilities.Logger.Info(CultureInfo.CurrentCulture, "Layout Metrics: Arrangement skipped {0} times (from {1} total requests)", ++arrangeSkipCount, ++arrangeRequestCount);
            }
#endif
        }

        private void OnLoad(object sender, EventArgs e)
        {
            parent = VisualTreeHelper.GetParent(this, p => p is Visual || p is IView || p is Window);

            if (resourceReferences != null)
            {
                for (int i = 0; i < resourceReferences.Count; i++)
                {
                    if (!UpdateResourceReference(resourceReferences[i]))
                    {
                        i--;
                    }
                }
            }

            BindingOperations.ActivateBindings(this);

            if (IsMeasureValid)
            {
                InvalidateMeasure();
            }

            var parentVisual = Parent as Visual;
            if (parentVisual != null && parentVisual.IsArrangeValid)
            {
                parentVisual.InvalidateArrange();
            }
            else if (IsArrangeValid)
            {
                InvalidateArrange();
            }

            OnLoaded(e);
        }

        private Size OnMeasureRequest(bool forceMeasure, Size? constraintsOverride)
        {
            if (!isMeasuring && (forceMeasure || !IsMeasureValid))
            {
                Size constraints = Size.Empty;
                if (constraintsOverride == null)
                {
                    var window = Parent as Window;
                    if (window == null)
                    {
                        constraints = (Parent as Visual)?.GetChildConstraints(this) ?? new Size(double.PositiveInfinity, double.PositiveInfinity);
                    }
                    else
                    {
                        constraints.Width = window.Width;
                        constraints.Height = window.Height;
                    }
                }
                else
                {
                    constraints = constraintsOverride.Value;
                }

                Measure(constraints);
            }
#if METRICS
            else
            {
                Utilities.Logger.Info(CultureInfo.CurrentCulture, "Layout Metrics: Measurement skipped {0} times (from {1} total requests)", ++measureSkipCount, ++measureRequestCount);
            }
#endif

            return DesiredSize;
        }

        private void OnResourceChanged(object sender, object key)
        {
            PropagateResourceChange(this, key);
        }

        private void OnResourceCollectionChanged(object sender, EventArgs e)
        {
            PropagateResourceCollectionChange(this);
        }

        private void OnUnload(object sender, EventArgs e)
        {
            parent = null;

            var bindings = BindingOperations.GetAllBindings(this);
            if (bindings != null)
            {
                foreach (var binding in bindings)
                {
                    binding.Deactivate();
                }
            }

            OnUnloaded(e);
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Exact error types can be unpredictable but should not interfere with execution of the program.  The error is logged to facilitate debugging.")]
        private bool UpdateResourceReference(ResourceReference resourceRef)
        {
            try
            {
                // If the property has been set to something else, the resource reference is no longer valid.
                var propValue = resourceRef.Property.GetValue(this, null);
                if (propValue == null ? resourceRef.Value == null : propValue.Equals(resourceRef.Value))
                {
                    resourceRef.Value = TryFindResource(resourceRef.Key);
                    if (propValue == null ? resourceRef.Value != null : !propValue.Equals(resourceRef.Value))
                    {
                        resourceRef.Property.SetValue(this, resourceRef.Value, null);
                        // The exact value of the property could be different (for instance, with floating point numbers), so we read it and store it.
                        resourceRef.Value = resourceRef.Property.GetValue(this, null);
                    }
                    return true;
                }
            }
            catch (Exception e)
            {
                Utilities.Logger.Error(CultureInfo.CurrentCulture, Strings.FailedToSetResourceValueOnProperty, resourceRef.Value, resourceRef.Property.Name, e);
            }

            resourceReferences.Remove(resourceRef);
            return false;
        }

        private class ResourceReference
        {
            public object Key;
            public readonly PropertyDescriptor Property;
            public object Value;

            public ResourceReference(PropertyDescriptor property)
            {
                Property = property;
            }

            public bool IsKeyRelated(object key)
            {
                if (Key.Equals(key))
                {
                    return true;
                }

                var resourceKey = (Key as ResourceKey)?.DependencyKey;
                while (resourceKey != null)
                {
                    if (resourceKey.Equals(key))
                    {
                        return true;
                    }

                    resourceKey = resourceKey.DependencyKey;
                }

                return false;
            }
        }
    }
}
