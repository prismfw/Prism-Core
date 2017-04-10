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
using Prism.Native;
using Prism.Resources;
using Prism.UI.Media;

#if !DEBUG
using System.Diagnostics;
#endif

namespace Prism.UI.Controls
{
    /// <summary>
    /// Represents a selectable item in a <see cref="ListBox"/>.
    /// </summary>
    public class ListBoxItem : Element
    {
        #region Property Descriptors
        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:Accessory"/> property.
        /// </summary>
        public static PropertyDescriptor AccessoryProperty { get; } = PropertyDescriptor.Create(nameof(Accessory), typeof(ListBoxItemAccessory), typeof(ListBoxItem), new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:Background"/> property.
        /// </summary>
        public static PropertyDescriptor BackgroundProperty { get; } = PropertyDescriptor.Create(nameof(Background), typeof(Brush), typeof(ListBoxItem));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:ContentPanel"/> property.
        /// </summary>
        public static PropertyDescriptor ContentPanelProperty { get; } = PropertyDescriptor.Create(nameof(ContentPanel), typeof(Panel), typeof(ListBoxItem), new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:IsSelected"/> property.
        /// </summary>
        public static PropertyDescriptor IsSelectedProperty { get; } = PropertyDescriptor.Create(nameof(IsSelected), typeof(bool), typeof(ListBoxItem), true);

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:SelectedBackground"/> property.
        /// </summary>
        public static PropertyDescriptor SelectedBackgroundProperty { get; } = PropertyDescriptor.Create(nameof(SelectedBackground), typeof(Brush), typeof(ListBoxItem));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:SeparatorIndentation"/> property.
        /// </summary>
        public static PropertyDescriptor SeparatorIndentationProperty { get; } = PropertyDescriptor.Create(nameof(SeparatorIndentation), typeof(Thickness), typeof(ListBoxItem));
        #endregion

        /// <summary>
        /// Gets or sets the accessory for the item.
        /// </summary>
        public ListBoxItemAccessory Accessory
        {
            get { return nativeObject.Accessory; }
            set { nativeObject.Accessory = value; }
        }

        /// <summary>
        /// Gets or sets the background of the item.
        /// </summary>
        public Brush Background
        {
            get { return nativeObject.Background; }
            set { nativeObject.Background = value; }
        }

        /// <summary>
        /// Gets or sets the <see cref="Panel"/> containing the content to be displayed by the item.
        /// </summary>
        public Panel ContentPanel
        {
            get { return (Panel)ObjectRetriever.GetAgnosticObject(nativeObject.ContentPanel); }
            set { nativeObject.ContentPanel = (INativePanel)ObjectRetriever.GetNativeObject(value); }
        }

        /// <summary>
        /// Gets the secondary text label that is provided when the item is initialized with
        /// <see cref="ListBoxItemStyle.Detail"/> or <see cref="ListBoxItemStyle.Full"/>.
        /// </summary>
        public Label DetailTextLabel { get; }

        /// <summary>
        /// Gets the image element for the item.
        /// </summary>
        public Image Image { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is selected.
        /// </summary>
        public bool IsSelected
        {
            get { return nativeObject.IsSelected; }
        }

        /// <summary>
        /// Gets or sets the background of the item when it is selected.
        /// </summary>
        public Brush SelectedBackground
        {
            get { return nativeObject.SelectedBackground; }
            set { nativeObject.SelectedBackground = value; }
        }

        /// <summary>
        /// Gets or sets the amount to indent the separator.
        /// </summary>
        public Thickness SeparatorIndentation
        {
            get { return nativeObject.SeparatorIndentation; }
            set
            {
                setSeparatorIndentation = false;
                nativeObject.SeparatorIndentation = value;
            }
        }

        /// <summary>
        /// Gets the primary text label for the item.
        /// </summary>
        public Label TextLabel { get; }

        /// <summary>
        /// Gets the secondary text label that is provided when the item is initialized with
        /// <see cref="ListBoxItemStyle.Value"/> or <see cref="ListBoxItemStyle.Full"/>.
        /// </summary>
        public Label ValueTextLabel { get; }

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        // this field is to avoid casting
        private readonly INativeListBoxItem nativeObject;

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private bool setSeparatorIndentation = (bool)Application.Current.Resources[SystemResources.ShouldAutomaticallyIndentSeparatorsKey];

        /// <summary>
        /// Initializes a new instance of the <see cref="ListBoxItem"/> class.
        /// </summary>
        public ListBoxItem()
            : this(ListBoxItemStyle.Default)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListBoxItem"/> class.
        /// </summary>
        /// <param name="style">The style with which to initialize the item.</param>
        public ListBoxItem(ListBoxItemStyle style)
            : this(typeof(INativeListBoxItem), null)
        {
            if (style == ListBoxItemStyle.Empty)
            {
                MinHeight = (double)Application.Current.Resources[SystemResources.ListBoxItemStandardHeightKey];
                return;
            }

            var grid = new Grid()
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Center
            };

            ContentPanel = grid;

            if ((style & ListBoxItemStyle.Default) == ListBoxItemStyle.Default)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition());
                grid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));
                grid.RowDefinitions.Add(new RowDefinition());

                Image = new Image()
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Margin = new Thickness(0, 5, 0, 5),
                    VerticalAlignment = VerticalAlignment.Center
                };

                TextLabel = new Label()
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Lines = 0,
                    Margin = new Thickness(15, 5, 0, 5),
                    VerticalAlignment = VerticalAlignment.Center
                };
                Grid.SetColumn(TextLabel, 1);

                ContentPanel.Children.Add(Image);
                ContentPanel.Children.Add(TextLabel);
            }

            if ((style & ListBoxItemStyle.Detail) == ListBoxItemStyle.Detail)
            {
                grid.RowDefinitions.Add(new RowDefinition());
                Grid.SetRowSpan(Image, 2);

                TextLabel.Margin = new Thickness(15, 5, 0, 0);

                DetailTextLabel = new Label()
                {
                    FontSize = (double)Application.Current.Resources[SystemResources.DetailLabelFontSizeKey],
                    FontStyle = (FontStyle)Application.Current.Resources[SystemResources.DetailLabelFontStyleKey],
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Lines = 1,
                    Margin = new Thickness(15, 0, 0, 5),
                    VerticalAlignment = VerticalAlignment.Top
                };
                DetailTextLabel.SetResourceReference(Label.ForegroundProperty, SystemResources.DetailLabelForegroundBrushKey);
                Grid.SetColumn(DetailTextLabel, 1);
                Grid.SetRow(DetailTextLabel, 1);

                ContentPanel.Children.Add(DetailTextLabel);

                MinHeight = (double)Application.Current.Resources[SystemResources.ListBoxItemDetailHeightKey];
            }
            else
            {
                MinHeight = (double)Application.Current.Resources[SystemResources.ListBoxItemStandardHeightKey];
            }

            if ((style & ListBoxItemStyle.Value) == ListBoxItemStyle.Value)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition());

                ValueTextLabel = new Label()
                {
                    FontSize = (double)Application.Current.Resources[SystemResources.ValueLabelFontSizeKey],
                    FontStyle = (FontStyle)Application.Current.Resources[SystemResources.ValueLabelFontStyleKey],
                    HorizontalAlignment = HorizontalAlignment.Right,
                    Lines = 1,
                    Margin = new Thickness(0, 5, 0, 5),
                    VerticalAlignment = VerticalAlignment.Center
                };
                ValueTextLabel.SetResourceReference(Label.ForegroundProperty, SystemResources.ValueLabelForegroundBrushKey);
                Grid.SetColumn(ValueTextLabel, 2);

                if (DetailTextLabel != null)
                {
                    ValueTextLabel.Margin = new Thickness(0, 5, 0, 0);
                    Grid.SetColumnSpan(DetailTextLabel, 2);
                }

                ContentPanel.Children.Add(ValueTextLabel);
            }

            SetResourceReference(BackgroundProperty, SystemResources.ListBoxItemBackgroundBrushKey);
            SetResourceReference(SelectedBackgroundProperty, SystemResources.ListBoxItemSelectedBackgroundBrushKey);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListBoxItem"/> class.
        /// </summary>
        /// <param name="resolveType">The type to pass to the IoC container in order to resolve the native object.</param>
        /// <param name="resolveName">An optional name to use when resolving the native object.</param>
        /// <param name="resolveParameters">Any parameters to pass along to the constructor of the resolve type.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="resolveType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="resolveType"/> does not resolve to an <see cref="INativeListBoxItem"/> instance.</exception>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", Justification = "resolveType is validated in base constructor.")]
        protected ListBoxItem(Type resolveType, string resolveName, params ResolveParameter[] resolveParameters)
            : base(resolveType, resolveName, resolveParameters)
        {
            nativeObject = ObjectRetriever.GetNativeObject(this) as INativeListBoxItem;
            if (nativeObject == null)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Strings.TypeMustResolveToType, resolveType.FullName, typeof(INativeListBoxItem).FullName), nameof(resolveType));
            }

            HorizontalAlignment = HorizontalAlignment.Stretch;
        }

        /// <summary>
        /// Called when this instance is ready to arrange its children and returns the final rendering size of the object.
        /// </summary>
        /// <param name="constraints">The width and height that this instance should not exceed.</param>
        /// <returns>The final rendering size of the object as a <see cref="Size"/> instance.</returns>
        protected override Size ArrangeOverride(Size constraints)
        {
            constraints = base.ArrangeOverride(constraints);

            var content = ContentPanel ?? VisualTreeHelper.GetChild<Visual>(this);
            if (content != null)
            {
                content.Arrange(new Rectangle(new Point(), constraints));

                if (setSeparatorIndentation && TextLabel != null)
                {
                    var nativeVisual = ObjectRetriever.GetNativeObject(TextLabel) as INativeVisual;
                    if (nativeVisual != null)
                    {
                        nativeObject.SeparatorIndentation = new Thickness(TextLabel.TranslatePointToAncestor(new Point(), this).X, 0, 0, 0);
                    }
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
            constraints = base.MeasureOverride(constraints);

            var content = ContentPanel ?? VisualTreeHelper.GetChild<Visual>(this);
            if (content != null)
            {
                double rightMargin = 0;
                if (Accessory == ListBoxItemAccessory.Indicator)
                {
                    rightMargin = ((Size)Application.Current.Resources[SystemResources.ListBoxItemIndicatorSizeKey]).Width;
                }
                else if (Accessory == ListBoxItemAccessory.InfoButton)
                {
                    rightMargin = ((Size)Application.Current.Resources[SystemResources.ListBoxItemInfoButtonSizeKey]).Width;
                }
                else if (Accessory == ListBoxItemAccessory.InfoIndicator)
                {
                    rightMargin = ((Size)Application.Current.Resources[SystemResources.ListBoxItemInfoIndicatorSizeKey]).Width;
                }
                else
                {
                    rightMargin = 15;
                }

                constraints.Width -= rightMargin;
                content.Measure(constraints);
                return new Size(content.DesiredSize.Width + rightMargin, content.DesiredSize.Height);
            }

            return Size.Empty;
        }

        /// <summary>
        /// Called when this instance is being recycled for use in a different location within a <see cref="ListBox"/>.
        /// </summary>
        protected internal virtual void OnReusing() { }
    }
}
