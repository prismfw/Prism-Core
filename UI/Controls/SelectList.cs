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
using System.Collections;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Prism.Native;
using Prism.UI.Media;

namespace Prism.UI.Controls
{
    /// <summary>
    /// Represents a UI element that allows for a single selection from a list of items.
    /// </summary>
    public class SelectList : Control
    {
        #region Event Descriptors
        /// <summary>
        /// Gets an <see cref="EventDescriptor"/> describing the <see cref="E:SelectionChanged"/> event.
        /// </summary>
        public static EventDescriptor SelectionChangedEvent { get; } = EventDescriptor.Create(nameof(SelectionChanged), typeof(TypedEventHandler<SelectList, SelectionChangedEventArgs>), typeof(SelectList));
        #endregion

        #region Property Descriptors
        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:Adapter"/> property.
        /// </summary>
        public static PropertyDescriptor AdapterProperty { get; } = PropertyDescriptor.Create(nameof(Adapter), typeof(SelectListAdapter), typeof(SelectList), new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:IsOpen"/> property.
        /// </summary>
        public static PropertyDescriptor IsOpenProperty { get; } = PropertyDescriptor.Create(nameof(IsOpen), typeof(bool), typeof(SelectList));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:Items"/> property.
        /// </summary>
        public static PropertyDescriptor ItemsProperty { get; } = PropertyDescriptor.Create(nameof(Items), typeof(IList), typeof(SelectList), new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:SelectedIndex"/> property.
        /// </summary>
        public static PropertyDescriptor SelectedIndexProperty { get; } = PropertyDescriptor.Create(nameof(SelectedIndex), typeof(int), typeof(SelectList), new PropertyMetadata(PropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:SelectedItem"/> property.
        /// </summary>
        public static PropertyDescriptor SelectedItemProperty { get; } = PropertyDescriptor.Create(nameof(SelectedItem), typeof(object), typeof(SelectList), new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));
        #endregion

        /// <summary>
        /// Occurs when the selection of the select list is changed.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification = "Event handler provides a strongly-typed sender for easier use.")]
        public event TypedEventHandler<SelectList, SelectionChangedEventArgs> SelectionChanged;

        /// <summary>
        /// Gets or sets the object responsible for translating data objects into UI elements for display within the select list.
        /// </summary>
        public SelectListAdapter Adapter
        {
            get { return adapter; }
            set
            {
                if (value != adapter)
                {
                    adapter = value;
                    OnPropertyChanged(AdapterProperty);

                    nativeObject.RefreshDisplayItem();
                    if (nativeObject.Items != null && nativeObject.Items.Count > 0)
                    {
                        nativeObject.RefreshListItems();
                    }
                }
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private SelectListAdapter adapter;

        /// <summary>
        /// Gets or sets a value indicating whether the list is open for selection.
        /// </summary>
        public bool IsOpen
        {
            get { return nativeObject.IsOpen; }
            set { nativeObject.IsOpen = value; }
        }

        /// <summary>
        /// Gets or sets a list of the items that make up the selection list.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Consuming developers should be able to specify the type of collection they wish to use.")]
        public IList Items
        {
            get { return nativeObject.Items; }
            set { nativeObject.Items = value; }
        }

        /// <summary>
        /// Gets or sets the zero-based index of the selected item.
        /// </summary>
        public int SelectedIndex
        {
            get { return nativeObject.SelectedIndex; }
            set { nativeObject.SelectedIndex = value; }
        }

        /// <summary>
        /// Gets or sets the selected item.
        /// </summary>
        public object SelectedItem
        {
            get { return Items == null || Items.Count <= SelectedIndex || SelectedIndex < 0 ? null : Items[SelectedIndex]; }
            set { SelectedIndex = Items == null ? 0 : Items.IndexOf(value); }
        }

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        // this field is to avoid casting
        private readonly INativeSelectList nativeObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectList"/> class.
        /// </summary>
        public SelectList()
            : this(typeof(INativeSelectList), null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectList"/> class.
        /// </summary>
        /// <param name="resolveType">The type to pass to the IoC container in order to resolve the native object.</param>
        /// <param name="resolveName">An optional name to use when resolving the native object.</param>
        /// <param name="resolveParameters">Any parameters to pass along to the constructor of the resolve type.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="resolveType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="resolveType"/> does not resolve to an <see cref="INativeSelectList"/> instance.</exception>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", Justification = "resolveType is validated in base constructor.")]
        protected SelectList(Type resolveType, string resolveName, params ResolveParameter[] resolveParameters)
            : base(resolveType, resolveName, resolveParameters)
        {
            nativeObject = ObjectRetriever.GetNativeObject(this) as INativeSelectList;
            if (nativeObject == null)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.Strings.TypeMustResolveToType, resolveType.FullName, typeof(INativeSelectList).FullName), nameof(resolveType));
            }

            nativeObject.PropertyChanged += (o, e) =>
            {
                if ((e.Property == SelectedIndexProperty || e.Property == ItemsProperty))
                {
                    OnPropertyChanged(SelectedItemProperty);
                }
            };

            nativeObject.SelectionChanged += (o, e) => OnSelectionChanged(e);

            nativeObject.DisplayItemRequest = () =>
            {
                var displayItem = nativeObject.Items == null || nativeObject.Items.Count <= nativeObject.SelectedIndex || nativeObject.SelectedIndex < 0 ?
                    null : nativeObject.Items[nativeObject.SelectedIndex];

                if (adapter != null)
                {
                    displayItem = adapter.GetDisplayItem(displayItem);
                }

                var element = displayItem as Element;
                if (element == null)
                {
                    var displayLabel = VisualTreeHelper.GetChild<Label>(this);
                    if (displayLabel == null)
                    {
                        displayLabel = new Label()
                        {
                            FontFamily = FontFamily,
                            FontSize = FontSize,
                            FontStyle = FontStyle,
                            Foreground = Foreground,
                            Margin = SystemParameters.SelectListDisplayItemPadding,
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center
                        };
                    }

                    displayLabel.Text = displayItem?.ToString();
                    element = displayLabel;
                }

                var grid = VisualTreeHelper.GetChild<Grid>(this) ?? new Grid()
                {
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    IsHitTestVisible = false,
                    VerticalAlignment = VerticalAlignment.Stretch
                };
                grid.Children.Clear();

                if (element.Parent != grid)
                {
                    (element.Parent as Panel)?.Children.Remove(element);
                    grid.Children.Add(element);
                }

                return ObjectRetriever.GetNativeObject(grid);
            };

            nativeObject.ListItemRequest = (value) =>
            {
                if (adapter != null)
                {
                    value = adapter.GetListItem(value);
                }

                var element = value as Element ?? new Label()
                {
                    FontFamily = FontFamily,
                    FontSize = FontSize,
                    FontStyle = FontStyle,
                    Margin = SystemParameters.SelectListListItemPadding,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Center,
                    Text = value?.ToString()
                };

                (element.Parent as Panel)?.Children.Remove(element);
                
                return ObjectRetriever.GetNativeObject(new Grid()
                {
                    Children = { element },
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    IsHitTestVisible = false
                });
            };

            BorderWidth = SystemParameters.SelectListBorderWidth;
            FontSize = Fonts.SelectListFontSize;
            FontStyle = Fonts.SelectListFontStyle;
        }

        /// <summary>
        /// Called when this instance is ready to arrange its children and returns the final rendering size of the object.
        /// </summary>
        /// <param name="constraints">The width and height that this instance should not exceed.</param>
        /// <returns>The final rendering size of the object as a <see cref="Size"/> instance.</returns>
        protected override Size ArrangeOverride(Size constraints)
        {
            constraints = base.ArrangeOverride(constraints);

            var content = VisualTreeHelper.GetChild<Visual>(this);
            if (content != null)
            {
                double borderWidth = nativeObject.BorderWidth;
                content.Arrange(new Rectangle(borderWidth, borderWidth, constraints.Width - borderWidth * 2, constraints.Height - borderWidth * 2));
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

            double borderWidth = nativeObject.BorderWidth;
            var content = VisualTreeHelper.GetChild<Visual>(this);
            if (content != null)
            {
                
                content.Measure(new Size(constraints.Width - borderWidth * 2, constraints.Height - borderWidth * 2));
                return new Size(content.DesiredSize.Width + borderWidth * 2, content.DesiredSize.Height + borderWidth * 2);
            }

            return new Size(borderWidth * 2, borderWidth * 2);
        }

        /// <summary>
        /// Called when the selection of the select list is changed and raises the <see cref="SelectionChanged"/> event.
        /// </summary>
        /// <param name="e">The event arguments containing the selection details.</param>
        protected virtual void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            SelectionChanged?.Invoke(this, e);
        }
    }
}
