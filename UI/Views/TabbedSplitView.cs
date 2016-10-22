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
using System.Linq;
using Prism.Native;
using Prism.Resources;

namespace Prism.UI
{
    /// <summary>
    /// Represents a platform-agnostic view with the combined functionality of a <see cref="SplitView"/> and a <see cref="TabView"/>.
    /// The generic version implements <see cref="IView&lt;T&gt;"/> for specifying a model type.
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="P:Model"/>.</typeparam>
    [SuppressMessage("Microsoft.Maintainability", "CA1501:AvoidExcessiveInheritance", Justification = "Changing inheritance would lead to duplication of code and a decrease in maintainability.")]
    public class TabbedSplitView<T> : TabbedSplitView, IView<T>
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
        /// Initializes a new instance of the <see cref="TabbedSplitView&lt;T&gt;"/> class.
        /// </summary>
        public TabbedSplitView()
        {
        }
    }

    /// <summary>
    /// Represents a platform-agnostic view with the combined functionality of a <see cref="SplitView"/> and a <see cref="TabView"/>.
    /// </summary>
    public class TabbedSplitView : TabView
    {
        #region Property Descriptors
        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:ActualDetailWidth"/> property.
        /// </summary>
        public static PropertyDescriptor ActualDetailWidthProperty { get; } = PropertyDescriptor.Create(nameof(ActualDetailWidth), typeof(double), typeof(TabbedSplitView), true, new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:ActualMasterWidth"/> property.
        /// </summary>
        public static PropertyDescriptor ActualMasterWidthProperty { get; } = PropertyDescriptor.Create(nameof(ActualMasterWidth), typeof(double), typeof(TabbedSplitView), true, new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:DetailContent"/> property.
        /// </summary>
        public static PropertyDescriptor DetailContentProperty { get; } = PropertyDescriptor.Create(nameof(DetailContent), typeof(object), typeof(TabbedSplitView), new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.AffectsArrange));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:IsDetailAutoResetEnabled"/> property.
        /// </summary>
        public static PropertyDescriptor IsDetailAutoResetEnabledProperty { get; } = PropertyDescriptor.Create(nameof(IsDetailAutoResetEnabled), typeof(bool), typeof(TabbedSplitView));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:MaxMasterWidth"/> property.
        /// </summary>
        public static PropertyDescriptor MaxMasterWidthProperty { get; } = PropertyDescriptor.Create(nameof(MaxMasterWidth), typeof(double), typeof(TabbedSplitView));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:MinMasterWidth"/> property.
        /// </summary>
        public static PropertyDescriptor MinMasterWidthProperty { get; } = PropertyDescriptor.Create(nameof(MinMasterWidth), typeof(double), typeof(TabbedSplitView));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:PreferredMasterWidthRatio"/> property.
        /// </summary>
        public static PropertyDescriptor PreferredMasterWidthRatioProperty { get; } = PropertyDescriptor.Create(nameof(PreferredMasterWidthRatio), typeof(double), typeof(TabbedSplitView));
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
        private readonly INativeTabbedSplitView nativeObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="TabbedSplitView"/> class.
        /// </summary>
        public TabbedSplitView()
            : this(typeof(INativeTabbedSplitView), null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TabbedSplitView"/> class.
        /// </summary>
        /// <param name="resolveType">The type to pass to the IoC container in order to resolve the native object.</param>
        /// <param name="resolveName">An optional name to use when resolving the native object.</param>
        /// <param name="resolveParameters">Any parameters to pass along to the constructor of the resolve type.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="resolveType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="resolveType"/> does not resolve to an <see cref="INativeTabbedSplitView"/> instance.</exception>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", Justification = "resolveType is validated in base constructor.")]
        protected TabbedSplitView(Type resolveType, string resolveName, params ResolveParameter[] resolveParameters)
            : base(resolveType, resolveName, resolveParameters)
        {
            nativeObject = ObjectRetriever.GetNativeObject(this) as INativeTabbedSplitView;
            if (nativeObject == null)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Strings.TypeMustResolveToType, resolveType.FullName, typeof(INativeTabbedSplitView).FullName), nameof(resolveType));
            }

            nativeObject.TabItemSelected += (o, e) =>
            {
                if (e.CurrentItem != e.PreviousItem)
                {
                    OnMasterContentChanged();
                }
            };
        }

        /// <summary>
        /// Called when this instance is ready to arrange its children and returns the final rendering size of the object.
        /// </summary>
        /// <param name="constraints">The width and height that this instance should not exceed.</param>
        /// <returns>The final rendering size of the object as a <see cref="Size"/> instance.</returns>
        protected override Size ArrangeOverride(Size constraints)
        {
            Size masterSize = new Size(ActualMasterWidth, constraints.Height);
            Size detailSize = new Size(ActualDetailWidth, constraints.Height);

            var tabBarFrame = nativeObject.TabBarFrame;
            if (tabBarFrame.Height < tabBarFrame.Width)
            {
                if (tabBarFrame.X <= ActualMasterWidth)
                {
                    masterSize.Height -= tabBarFrame.Height;
                }
                if (tabBarFrame.X > ActualMasterWidth || tabBarFrame.Width > ActualMasterWidth)
                {
                    detailSize.Height -= tabBarFrame.Height;
                }
            }

            var currentTab = TabItems.ElementAtOrDefault(SelectedIndex);
            foreach (var tab in TabItems)
            {
                var masterVisual = tab.Content as Visual;
                if (masterVisual != null && (tab == currentTab || masterVisual.IsLoaded))
                {
                    masterVisual.Arrange(new Rectangle(new Point(), masterSize));
                }
            }

            var detailVisual = (DetailContent as Visual);
            if (detailVisual != null)
            {
                detailVisual.Arrange(new Rectangle(new Point(), detailSize));
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
            Size masterConstraints = new Size(ActualMasterWidth, constraints.Height);
            Size detailConstraints = new Size(ActualDetailWidth, constraints.Height);

            var tabBarFrame = nativeObject.TabBarFrame;
            if (tabBarFrame.Height < tabBarFrame.Width)
            {
                if (tabBarFrame.X <= ActualMasterWidth)
                {
                    masterConstraints.Height -= tabBarFrame.Height;
                }
                if (tabBarFrame.X > ActualMasterWidth || tabBarFrame.Width > ActualMasterWidth)
                {
                    detailConstraints.Height -= tabBarFrame.Height;
                }
            }

            var currentTab = TabItems.ElementAtOrDefault(SelectedIndex);
            foreach (var tab in TabItems)
            {
                var masterVisual = tab.Content as Visual;
                if (masterVisual != null && (tab == currentTab || masterVisual.IsLoaded))
                {
                    masterVisual.Measure(masterConstraints);
                }
            }

            var detailVisual = (DetailContent as Visual);
            if (detailVisual != null)
            {
                detailVisual.Measure(detailConstraints);
            }

            return constraints;
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
