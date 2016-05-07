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
        /// Describes the <see cref="P:ActualDetailWidth"/> property.  This field is read-only.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "PropertyDescriptor is immutable.")]
        public static readonly PropertyDescriptor ActualDetailWidthProperty = PropertyDescriptor.Create(nameof(ActualDetailWidth), typeof(double), typeof(TabbedSplitView), true, new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// Describes the <see cref="P:ActualMasterWidth"/> property.  This field is read-only.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "PropertyDescriptor is immutable.")]
        public static readonly PropertyDescriptor ActualMasterWidthProperty = PropertyDescriptor.Create(nameof(ActualMasterWidth), typeof(double), typeof(TabbedSplitView), true, new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// Describes the <see cref="P:DetailContent"/> property.  This field is read-only.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "PropertyDescriptor is immutable.")]
        public static readonly PropertyDescriptor DetailContentProperty = PropertyDescriptor.Create(nameof(DetailContent), typeof(object), typeof(TabbedSplitView), new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.AffectsArrange));

        /// <summary>
        /// Describes the <see cref="P:MaxMasterWidth"/> property.  This field is read-only.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "PropertyDescriptor is immutable.")]
        public static readonly PropertyDescriptor MaxMasterWidthProperty = PropertyDescriptor.Create(nameof(MaxMasterWidth), typeof(double), typeof(TabbedSplitView));

        /// <summary>
        /// Describes the <see cref="P:MinMasterWidth"/> property.  This field is read-only.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "PropertyDescriptor is immutable.")]
        public static readonly PropertyDescriptor MinMasterWidthProperty = PropertyDescriptor.Create(nameof(MinMasterWidth), typeof(double), typeof(TabbedSplitView));

        /// <summary>
        /// Describes the <see cref="P:PreferredMasterWidthRatio"/> property.  This field is read-only.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "PropertyDescriptor is immutable.")]
        public static readonly PropertyDescriptor PreferredMasterWidthRatioProperty = PropertyDescriptor.Create(nameof(PreferredMasterWidthRatio), typeof(double), typeof(TabbedSplitView));
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
        /// Gets or sets the maximum width of the master pane.
        /// </summary>
        public double MaxMasterWidth
        {
            get { return nativeObject.MaxMasterWidth; }
            set { nativeObject.MaxMasterWidth = value; }
        }

        /// <summary>
        /// Gets or sets the minimum width of the master pane.
        /// </summary>
        public double MinMasterWidth
        {
            get { return nativeObject.MinMasterWidth; }
            set { nativeObject.MinMasterWidth = value; }
        }

        /// <summary>
        /// Gets or sets the preferred width of the master pane as a percentage of the width of the split view.
        /// Valid values are between 0.0 and 1.0.
        /// </summary>
        public double PreferredMasterWidthRatio
        {
            get { return nativeObject.PreferredMasterWidthRatio; }
            set { nativeObject.PreferredMasterWidthRatio = value; }
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
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.Strings.TypeMustResolveToType, resolveType.FullName, typeof(INativeTabbedSplitView).FullName), nameof(resolveType));
            }
        }

        /// <summary>
        /// Called when this instance is ready to arrange its children and returns the final rendering size of the object.
        /// </summary>
        /// <param name="constraints">The width and height that this instance should not exceed.</param>
        /// <returns>The final rendering size of the object as a <see cref="Size"/> instance.</returns>
        protected override Size ArrangeOverride(Size constraints)
        {
            var currentTab = TabItems.ElementAtOrDefault(SelectedIndex);
            foreach (var tab in TabItems)
            {
                var masterVisual = tab.Content as Visual;
                if (masterVisual != null && (tab == currentTab || masterVisual.IsLoaded))
                {
                    masterVisual.Arrange(new Rectangle(0, 0, ActualMasterWidth, constraints.Height));
                }
            }

            var detailVisual = (DetailContent as Visual);
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
            var currentTab = TabItems.ElementAtOrDefault(SelectedIndex);
            foreach (var tab in TabItems)
            {
                var masterVisual = tab.Content as Visual;
                if (masterVisual != null && (tab == currentTab || masterVisual.IsLoaded))
                {
                    masterVisual.Measure(new Size(ActualMasterWidth, constraints.Height));
                }
            }

            var detailVisual = (DetailContent as Visual);
            if (detailVisual != null)
            {
                detailVisual.Measure(new Size(ActualDetailWidth, constraints.Height));
            }

            return constraints;
        }
    }
}
