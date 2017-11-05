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
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Prism.Native;
using Prism.Resources;

namespace Prism.UI
{
    /// <summary>
    /// Represents a platform-agnostic view that displays two child views together in separate panes.
    /// The generic version implements <see cref="IView&lt;T&gt;"/> for specifying a model type.
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="P:Model"/>.</typeparam>
    public class SplitView<T> : SplitView, IView<T>
    {
        /// <summary>
        /// Gets or sets the model containing the information that is displayed by the view.
        /// </summary>
        public new T Model
        {
            get { return (T)base.Model; }
            set { base.Model = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SplitView&lt;T&gt;"/> class.
        /// </summary>
        public SplitView()
        {
        }
    }

    /// <summary>
    /// Represents a platform-agnostic view that displays two child views together in separate panes.
    /// </summary>
    [Resolve(typeof(INativeSplitView))]
    public class SplitView : View
    {
        #region Property Descriptors
        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:ActualDetailWidth"/> property.
        /// </summary>
        public static PropertyDescriptor ActualDetailWidthProperty { get; } = PropertyDescriptor.Create(nameof(ActualDetailWidth), typeof(double), typeof(SplitView), true, new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:ActualMasterWidth"/> property.
        /// </summary>
        public static PropertyDescriptor ActualMasterWidthProperty { get; } = PropertyDescriptor.Create(nameof(ActualMasterWidth), typeof(double), typeof(SplitView), true, new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:DetailContent"/> property.
        /// </summary>
        public static PropertyDescriptor DetailContentProperty { get; } = PropertyDescriptor.Create(nameof(DetailContent), typeof(object), typeof(SplitView), new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.AffectsArrange));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:IsDetailAutoResetEnabled"/> property.
        /// </summary>
        public static PropertyDescriptor IsDetailAutoResetEnabledProperty { get; } = PropertyDescriptor.Create(nameof(IsDetailAutoResetEnabled), typeof(bool), typeof(SplitView));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:MasterContent"/> property.
        /// </summary>
        public static PropertyDescriptor MasterContentProperty { get; } = PropertyDescriptor.Create(nameof(MasterContent), typeof(object), typeof(SplitView), new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.AffectsArrange));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:MaxMasterWidth"/> property.
        /// </summary>
        public static PropertyDescriptor MaxMasterWidthProperty { get; } = PropertyDescriptor.Create(nameof(MaxMasterWidth), typeof(double), typeof(SplitView));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:MinMasterWidth"/> property.
        /// </summary>
        public static PropertyDescriptor MinMasterWidthProperty { get; } = PropertyDescriptor.Create(nameof(MinMasterWidth), typeof(double), typeof(SplitView));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:PreferredMasterWidthRatio"/> property.
        /// </summary>
        public static PropertyDescriptor PreferredMasterWidthRatioProperty { get; } = PropertyDescriptor.Create(nameof(PreferredMasterWidthRatio), typeof(double), typeof(SplitView));
        #endregion

        /// <summary>
        /// Gets the actual width of the detail pane.
        /// </summary>
        public double ActualDetailWidth
        {
            get { return nativeObject.ActualDetailWidth; }
        }

        /// <summary>
        /// Gets the actual width of the master pane.
        /// </summary>
        public double ActualMasterWidth
        {
            get { return nativeObject.ActualMasterWidth; }
        }

        /// <summary>
        /// Gets or sets the object that acts as the content for the detail pane.
        /// This is typically an <see cref="IView"/> or <see cref="ViewStack"/> instance.
        /// </summary>
        public object DetailContent
        {
            get { return detailContent; }
            set
            {
                if (value != detailContent)
                {
                    detailContent = value;
                    if (detailContent is IView || detailContent is INativeViewStack)
                    {
                        nativeObject.DetailContent = ObjectRetriever.GetNativeObject(detailContent);
                    }
                    else
                    {
                        object content = detailContent as ViewStack;
                        if (content == null && detailContent != null)
                        {
                            content = new ContentView()
                            {
                                Content = detailContent
                            };
                        }

                        nativeObject.DetailContent = ObjectRetriever.GetNativeObject(content);
                    }

                    OnPropertyChanged(DetailContentProperty);
                }
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private object detailContent;

        /// <summary>
        /// Gets or sets a value indicating whether the content of the detail pane should automatically reset upon changes to the content of the master pane.
        /// In most cases, this means popping a view stack in the detail pane to root when the current view of a view stack in the master pane is changed.
        /// </summary>
        public bool IsDetailAutoResetEnabled
        {
            get { return isDetailAutoResetEnabled; }
            set
            {
                if (value != isDetailAutoResetEnabled)
                {
                    isDetailAutoResetEnabled = value;
                    OnPropertyChanged(IsDetailAutoResetEnabledProperty);
                }
            }
        }
        private bool isDetailAutoResetEnabled = true;

        /// <summary>
        /// Gets or sets the object that acts as the content for the master pane.
        /// This is typically an <see cref="IView"/> or <see cref="ViewStack"/> instance.
        /// </summary>
        public object MasterContent
        {
            get { return masterContent; }
            set
            {
                if (value != masterContent)
                {
                    masterContent = value;
                    if (masterContent is IView || masterContent is INativeViewStack)
                    {
                        nativeObject.MasterContent = ObjectRetriever.GetNativeObject(masterContent);
                    }
                    else
                    {
                        object content = masterContent as ViewStack;
                        if (content == null && masterContent != null)
                        {
                            content = new ContentView()
                            {
                                Content = masterContent
                            };
                        }

                        nativeObject.MasterContent = ObjectRetriever.GetNativeObject(content);
                    }

                    OnPropertyChanged(MasterContentProperty);
                    OnMasterContentChanged();
                }
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private object masterContent;

        /// <summary>
        /// Gets or sets the maximum width of the master pane.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", Justification = "Exception parameter refers to property name for easier understanding of invalid value.")]
        public double MaxMasterWidth
        {
            get { return nativeObject.MaxMasterWidth; }
            set
            {
                if (double.IsNaN(value) || double.IsInfinity(value))
                {
                    throw new ArgumentException(Strings.ValueCannotBeNaNOrInfinity, nameof(MaxMasterWidth));
                }

                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(MaxMasterWidth), Strings.ValueCannotBeLessThanZero);
                }

                nativeObject.MaxMasterWidth = value;
            }
        }

        /// <summary>
        /// Gets or sets the minimum width of the master pane.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", Justification = "Exception parameter refers to property name for easier understanding of invalid value.")]
        public double MinMasterWidth
        {
            get { return nativeObject.MinMasterWidth; }
            set
            {
                if (double.IsNaN(value) || double.IsInfinity(value))
                {
                    throw new ArgumentException(Strings.ValueCannotBeNaNOrInfinity, nameof(MinMasterWidth));
                }

                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(MinMasterWidth), Strings.ValueCannotBeLessThanZero);
                }

                nativeObject.MinMasterWidth = value;
            }
        }

        /// <summary>
        /// Gets or sets the preferred width of the master pane as a percentage of the width of the split view.
        /// Valid values are between 0.0 and 1.0.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", Justification = "Exception parameter refers to property name for easier understanding of invalid value.")]
        public double PreferredMasterWidthRatio
        {
            get { return nativeObject.PreferredMasterWidthRatio; }
            set
            {
                if (double.IsNaN(value) || double.IsInfinity(value))
                {
                    throw new ArgumentException(Strings.ValueCannotBeNaNOrInfinity, nameof(PreferredMasterWidthRatio));
                }

                nativeObject.PreferredMasterWidthRatio = Math.Max(0, Math.Min(1, value));
            }
        }

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        // this field is to avoid casting
        private readonly INativeSplitView nativeObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="SplitView"/> class.
        /// </summary>
        public SplitView()
            : this(ResolveParameter.EmptyParameters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SplitView"/> class and pairs it with the specified native object.
        /// </summary>
        /// <param name="nativeObject">The native object with which to pair this instance.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="nativeObject"/> doesn't match the type specified by the topmost <see cref="ResolveAttribute"/> in the inheritance chain.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="nativeObject"/> is <c>null</c>.</exception>
        protected SplitView(INativeSplitView nativeObject)
            : base(nativeObject)
        {
            this.nativeObject = nativeObject;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SplitView"/> class and pairs it with a native object that is resolved from the IoC container.
        /// </summary>
        /// <param name="resolveParameters">Any parameters to pass along to the constructor of the native type.</param>
        /// <exception cref="TypeResolutionException">Thrown when the native object does not resolve to an <see cref="INativeSplitView"/> instance.</exception>
        protected SplitView(ResolveParameter[] resolveParameters)
            : base(resolveParameters)
        {
            nativeObject = ObjectRetriever.GetNativeObject(this) as INativeSplitView;
            if (nativeObject == null)
            {
                throw new TypeResolutionException(string.Format(CultureInfo.CurrentCulture, Strings.TypeMustResolveToType,
                    ObjectRetriever.GetNativeObject(this).GetType().FullName, typeof(INativeSplitView).FullName));
            }
        }

        /// <summary>
        /// Called when this instance is ready to arrange its children and returns the final rendering size of the object.
        /// </summary>
        /// <param name="constraints">The width and height that this instance should not exceed.</param>
        /// <returns>The final rendering size of the object as a <see cref="Size"/> instance.</returns>
        protected override Size ArrangeOverride(Size constraints)
        {
            var masterVisual = (MasterContent as Visual) ?? VisualTreeHelper.GetChild<ContentView>(this, c => c.Content == MasterContent);
            if (masterVisual != null)
            {
                masterVisual.Arrange(new Rectangle(0, 0, ActualMasterWidth, constraints.Height));
            }

            var detailVisual = (DetailContent as Visual) ?? VisualTreeHelper.GetChild<ContentView>(this, c => c.Content == DetailContent);
            if (detailVisual != null)
            {
                detailVisual.Arrange(new Rectangle(0, 0, ActualDetailWidth, constraints.Height));
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
            var masterVisual = (MasterContent as Visual) ?? VisualTreeHelper.GetChild<ContentView>(this, c => c.Content == MasterContent);
            if (masterVisual != null)
            {
                masterVisual.Measure(new Size(ActualMasterWidth, constraints.Height));
            }

            var detailVisual = (DetailContent as Visual) ?? VisualTreeHelper.GetChild<ContentView>(this, c => c.Content == DetailContent);
            if (detailVisual != null)
            {
                detailVisual.Measure(new Size(ActualDetailWidth, constraints.Height));
            }

            return nativeObject.Measure(constraints);
        }

        internal override Size GetChildConstraints(Visual child)
        {
            return new Size(child == MasterContent ? ActualMasterWidth : ActualDetailWidth, RenderSize.Height);
        }

        internal void OnMasterContentChanged()
        {
            if (isDetailAutoResetEnabled)
            {
                (detailContent as ViewStack)?.PopToRoot(Animate.Off);
            }
        }
    }
}
