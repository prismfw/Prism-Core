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
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using Prism.Native;
using Prism.Resources;
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
    [Resolve(typeof(INativeTabView))]
    public class TabView : View
    {
        #region Event Descriptors
        /// <summary>
        /// Gets an <see cref="EventDescriptor"/> describing the <see cref="E:TabItemSelected"/> event.
        /// </summary>
        public static EventDescriptor TabItemSelectedEvent { get; } = EventDescriptor.Create(nameof(TabItemSelected), typeof(TypedEventHandler<TabView, TabItemSelectedEventArgs>), typeof(TabView));
        #endregion

        #region Property Descriptors
        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:Background"/> property.
        /// </summary>
        public static PropertyDescriptor BackgroundProperty { get; } = PropertyDescriptor.Create(nameof(Background), typeof(Brush), typeof(TabView));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:Foreground"/> property.
        /// </summary>
        public static PropertyDescriptor ForegroundProperty { get; } = PropertyDescriptor.Create(nameof(Foreground), typeof(Brush), typeof(TabView));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:SelectedIndex"/> property.
        /// </summary>
        public static PropertyDescriptor SelectedIndexProperty { get; } = PropertyDescriptor.Create(nameof(SelectedIndex), typeof(int), typeof(TabView), new PropertyMetadata(PropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:SelectedTabItem"/> property.
        /// </summary>
        public static PropertyDescriptor SelectedTabItemProperty { get; } = PropertyDescriptor.Create(nameof(SelectedTabItem), typeof(TabItem), typeof(TabView));
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
        /// Gets or sets the selected tab item.
        /// </summary>
        public TabItem SelectedTabItem
        {
            get { return SelectedIndex >= 0 && SelectedIndex < TabItems.Count ? TabItems[SelectedIndex] : null; }
            set { SelectedIndex = TabItems.IndexOf(value); }
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
            : this(ResolveParameter.EmptyParameters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TabView"/> class and pairs it with the specified native object.
        /// </summary>
        /// <param name="nativeObject">The native object with which to pair this instance.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="nativeObject"/> doesn't match the type specified by the topmost <see cref="ResolveAttribute"/> in the inheritance chain.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="nativeObject"/> is <c>null</c>.</exception>
        protected TabView(INativeTabView nativeObject)
            : base(nativeObject)
        {
            this.nativeObject = nativeObject;

            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TabView"/> class and pairs it with a native object that is resolved from the IoC container.
        /// </summary>
        /// <param name="resolveParameters">Any parameters to pass along to the constructor of the native type.</param>
        /// <exception cref="TypeResolutionException">Thrown when the native object does not resolve to an <see cref="INativeTabView"/> instance.</exception>
        protected TabView(ResolveParameter[] resolveParameters)
            : base(resolveParameters)
        {
            nativeObject = ObjectRetriever.GetNativeObject(this) as INativeTabView;
            if (nativeObject == null)
            {
                throw new TypeResolutionException(string.Format(CultureInfo.CurrentCulture, Strings.TypeMustResolveToType,
                    ObjectRetriever.GetNativeObject(this).GetType().FullName, typeof(INativeTabView).FullName));
            }

            TabItems = new TabItemCollection(nativeObject);
            Initialize();
        }

        /// <summary>
        /// Called when this instance is ready to arrange its children and returns the final rendering size of the object.
        /// </summary>
        /// <param name="constraints">The width and height that this instance should not exceed.</param>
        /// <returns>The final rendering size of the object as a <see cref="Size"/> instance.</returns>
        protected override Size ArrangeOverride(Size constraints)
        {
            var retVal = constraints;
            var tabBarFrame = nativeObject.TabBarFrame;
            if (tabBarFrame.Height > tabBarFrame.Width)
            {
                constraints.Width -= tabBarFrame.Width;
            }
            else
            {
                constraints.Height -= tabBarFrame.Height;
            }

            var currentTab = TabItems.ElementAtOrDefault(SelectedIndex);
            foreach (var tab in TabItems)
            {
                tab.Arrange(new Rectangle(0, 0, tab.DesiredSize.Width, tab.DesiredSize.Height));
                
                var visual = tab.Content as Visual;
                if (visual != null && (tab == currentTab || visual.IsLoaded))
                {
                    visual.Arrange(new Rectangle(0, 0, constraints.Width, constraints.Height));
                }
            }

            return retVal;
        }

        /// <summary>
        /// Called when this instance is ready to be measured and returns the desired size of the object.
        /// </summary>
        /// <param name="constraints">The width and height that this instance should not exceed.</param>
        /// <returns>The desired size of the object as a <see cref="Size"/> instance.</returns>
        protected override Size MeasureOverride(Size constraints)
        {
            var retVal = constraints;
            var tabBarFrame = nativeObject.TabBarFrame;
            if (tabBarFrame.Height > tabBarFrame.Width)
            {
                constraints.Width -= tabBarFrame.Width;
            }
            else
            {
                constraints.Height -= tabBarFrame.Height;
            }

            var currentTab = TabItems.ElementAtOrDefault(SelectedIndex);
            foreach (var tab in TabItems)
            {
                tab.Measure(tabBarFrame.Size);
                
                var visual = tab.Content as Visual;
                if (visual != null && (tab == currentTab || visual.IsLoaded))
                {
                    visual.Measure(constraints);
                }
            }

            return retVal;
        }

        /// <summary>
        /// Called when a tab item is selected and raises the <see cref="TabItemSelected"/> event.
        /// </summary>
        /// <param name="e">The event arguments containing the selection details.</param>
        protected virtual void OnTabItemSelected(TabItemSelectedEventArgs e)
        {
            TabItemSelected?.Invoke(this, e);
        }

        private void Initialize()
        {
            nativeObject.PropertyChanged += (o, e) =>
            {
                if (e.Property == SelectedIndexProperty)
                {
                    OnPropertyChanged(SelectedTabItemProperty);
                    VisualTreeHelper.GetParent<SplitView>(this, sv => sv.MasterContent == this)?.OnMasterContentChanged();
                }
            };

            nativeObject.TabItemSelected += (o, e) =>
            {
                OnTabItemSelected(new TabItemSelectedEventArgs(
                    ObjectRetriever.GetAgnosticObject(e.OldItem) as TabItem,
                    ObjectRetriever.GetAgnosticObject(e.NewItem) as TabItem
                ));
            };

            SetResourceReference(BackgroundProperty, SystemResources.TabViewBackgroundBrushKey);
            SetResourceReference(ForegroundProperty, SystemResources.TabViewForegroundBrushKey);
        }
    }
}