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
using System.Linq;
using Prism.Native;
using Prism.UI.Controls;
using Prism.UI.Media;

#if !DEBUG
using System.Diagnostics;
#endif

namespace Prism.UI
{
    /// <summary>
    /// Represents a platform-agnostic view that displays selectable tab items for switching between views.
    /// The generic version implements <see cref="IView&lt;T&gt;"/> for specifying a model type.
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="P:Model"/>.</typeparam>
    public class TabView<T> : TabView, IView<T>
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
        /// Initializes a new instance of the <see cref="TabView&lt;T&gt;"/> class.
        /// </summary>
        public TabView()
        {
        }
    }

    /// <summary>
    /// Represents a platform-agnostic view that displays selectable tab items for switching between views.
    /// </summary>
    public class TabView : View
    {
        #region Event Descriptors
        /// <summary>
        /// Describes the <see cref="E:TabItemSelected"/> event.  This field is read-only.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "EventDescriptor is immutable.")]
        public static readonly EventDescriptor TabItemSelectedEvent = EventDescriptor.Create(nameof(TabItemSelected), typeof(TypedEventHandler<TabView, TabItemSelectedEventArgs>), typeof(TabView));
        #endregion

        #region Property Descriptors
        /// <summary>
        /// Describes the <see cref="P:Background"/> property.  This field is read-only.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "PropertyDescriptor is immutable.")]
        public static readonly PropertyDescriptor BackgroundProperty = PropertyDescriptor.Create(nameof(Background), typeof(Brush), typeof(TabView));

        /// <summary>
        /// Describes the <see cref="P:Foreground"/> property.  This field is read-only.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "PropertyDescriptor is immutable.")]
        public static readonly PropertyDescriptor ForegroundProperty = PropertyDescriptor.Create(nameof(Foreground), typeof(Brush), typeof(TabView));

        /// <summary>
        /// Describes the <see cref="P:SelectedIndex"/> property.  This field is read-only.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "PropertyDescriptor is immutable.")]
        public static readonly PropertyDescriptor SelectedIndexProperty = PropertyDescriptor.Create(nameof(SelectedIndex), typeof(int), typeof(TabView), new PropertyMetadata(PropertyMetadataOptions.BindsTwoWayByDefault));
        #endregion

        /// <summary>
        /// Occurs when a tab item is selected.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification = "Event handler provides a strongly-typed sender for easier use.")]
        public event TypedEventHandler<TabView, TabItemSelectedEventArgs> TabItemSelected;

        /// <summary>
        /// Gets or sets the background for the view.
        /// </summary>
        public Brush Background
        {
            get { return nativeObject.Background; }
            set { nativeObject.Background = value; }
        }

        /// <summary>
        /// Gets or sets the <see cref="Brush"/> to apply to the selected tab item.
        /// </summary>
        public Brush Foreground
        {
            get { return nativeObject.Foreground; }
            set { nativeObject.Foreground = value; }
        }

        /// <summary>
        /// Gets or sets the zero-based index of the selected tab item.
        /// </summary>
        public int SelectedIndex
        {
            get { return nativeObject.SelectedIndex; }
            set { nativeObject.SelectedIndex = value; }
        }

        /// <summary>
        /// Gets a collection of the tab items that are a part of the view.
        /// </summary>
        public TabItemCollection TabItems { get; }

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        // this field is to avoid casting
        private readonly INativeTabView nativeObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="TabView"/> class.
        /// </summary>
        public TabView()
            : this(typeof(INativeTabView), null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TabView"/> class.
        /// </summary>
        /// <param name="resolveType">The type to pass to the IoC container in order to resolve the native object.</param>
        /// <param name="resolveName">An optional name to use when resolving the native object.</param>
        /// <param name="resolveParameters">Any parameters to pass along to the constructor of the resolve type.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="resolveType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="resolveType"/> does not resolve to an <see cref="INativeTabView"/> instance.</exception>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", Justification = "resolveType is validated in base constructor.")]
        protected TabView(Type resolveType, string resolveName, params ResolveParameter[] resolveParameters)
            : base(resolveType, resolveName, resolveParameters)
        {
            nativeObject = ObjectRetriever.GetNativeObject(this) as INativeTabView;
            if (nativeObject == null)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.Strings.TypeMustResolveToType, resolveType.FullName, typeof(INativeTabView).FullName), nameof(resolveType));
            }
            
            TabItems = new TabItemCollection(nativeObject);

            nativeObject.TabItemSelected += (o, e) =>
            {
                OnTabItemSelected(new TabItemSelectedEventArgs(
                    ObjectRetriever.GetAgnosticObject(e.PreviousItem) as TabItem,
                    ObjectRetriever.GetAgnosticObject(e.CurrentItem) as TabItem
                ));
            };
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
                var visual = tab.Content as Visual;
                if (visual != null && (tab == currentTab || visual.IsLoaded))
                {
                    visual.Arrange(new Rectangle(0, 0, constraints.Width, constraints.Height));
                }
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
                var visual = tab.Content as Visual;
                if (visual != null && (tab == currentTab || visual.IsLoaded))
                {
                    visual.Measure(constraints);
                }
            }

            return constraints;
        }

        /// <summary>
        /// Called when a tab item is selected and raises the <see cref="TabItemSelected"/> event.
        /// </summary>
        /// <param name="e">The event arguments containing the selection details.</param>
        protected virtual void OnTabItemSelected(TabItemSelectedEventArgs e)
        {
            TabItemSelected?.Invoke(this, e);
        }
    }
}