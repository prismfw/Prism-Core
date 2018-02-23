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
    /// Represents a section header in a <see cref="ListBox"/>.
    /// </summary>
    [Resolve(typeof(INativeListBoxSectionHeader))]
    public class ListBoxSectionHeader : Element
    {
        #region Property Descriptors
        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:Background"/> property.
        /// </summary>
        public static PropertyDescriptor BackgroundProperty { get; } = PropertyDescriptor.Create(nameof(Background), typeof(Brush), typeof(ListBoxSectionHeader));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:ContentPanel"/> property.
        /// </summary>
        public static PropertyDescriptor ContentPanelProperty { get; } = PropertyDescriptor.Create(nameof(ContentPanel), typeof(Panel), typeof(ListBoxSectionHeader), new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));
        #endregion

        /// <summary>
        /// Gets or sets the background of the section header.
        /// </summary>
        public Brush Background
        {
            get { return nativeObject.Background; }
            set { nativeObject.Background = value; }
        }

        /// <summary>
        /// Gets or sets the <see cref="Panel"/> containing the content to be displayed by the section header.
        /// </summary>
        public Panel ContentPanel
        {
            get { return (Panel)ObjectRetriever.GetAgnosticObject(nativeObject.ContentPanel); }
            set { nativeObject.ContentPanel = (INativePanel)ObjectRetriever.GetNativeObject(value); }
        }

        /// <summary>
        /// Gets the text label for the section header.
        /// </summary>
        public Label TextLabel { get; }

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        // this field is to avoid casting
        private readonly INativeListBoxSectionHeader nativeObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListBoxSectionHeader"/> class.
        /// </summary>
        public ListBoxSectionHeader()
            : this(ResolveParameter.EmptyParameters)
        {
            ContentPanel = new Grid()
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Center
            };

            TextLabel = new Label()
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                Lines = 1,
                Margin = new Thickness(15, 0, 15, 0),
                VerticalAlignment = VerticalAlignment.Center,
            };

            ContentPanel.Children.Add(TextLabel);

            SetResourceReference(BackgroundProperty, SystemResources.SectionHeaderBackgroundBrushKey);
            TextLabel.SetResourceReference(Label.FontSizeProperty, SystemResources.SectionHeaderFontSizeKey);
            TextLabel.SetResourceReference(Label.FontStyleProperty, SystemResources.SectionHeaderFontStyleKey);
            TextLabel.SetResourceReference(Label.ForegroundProperty, SystemResources.SectionHeaderForegroundBrushKey);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListBoxSectionHeader"/> class and pairs it with the specified native object.
        /// </summary>
        /// <param name="nativeObject">The native object with which to pair this instance.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="nativeObject"/> doesn't match the type specified by the topmost <see cref="ResolveAttribute"/> in the inheritance chain.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="nativeObject"/> is <c>null</c>.</exception>
        protected ListBoxSectionHeader(INativeListBoxSectionHeader nativeObject)
            : base(nativeObject)
        {
            this.nativeObject = nativeObject;

            HorizontalAlignment = HorizontalAlignment.Stretch;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListBoxSectionHeader"/> class and pairs it with a native object that is resolved from the IoC container.
        /// </summary>
        /// <param name="resolveParameters">Any parameters to pass along to the constructor of the native type.</param>
        /// <exception cref="TypeResolutionException">Thrown when the native object does not resolve to an <see cref="INativeListBoxSectionHeader"/> instance.</exception>
        protected ListBoxSectionHeader(ResolveParameter[] resolveParameters)
            : base(resolveParameters)
        {
            nativeObject = ObjectRetriever.GetNativeObject(this) as INativeListBoxSectionHeader;
            if (nativeObject == null)
            {
                throw new TypeResolutionException(string.Format(CultureInfo.CurrentCulture, Strings.TypeMustResolveToType,
                    ObjectRetriever.GetNativeObject(this).GetType().FullName, typeof(INativeListBoxSectionHeader).FullName));
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
                content.Measure(constraints);
                return content.DesiredSize;
            }

            return Size.Empty;
        }

        /// <summary>
        /// Called when this instance is being recycled for use in a different location within a <see cref="ListBox"/>.
        /// </summary>
        protected internal virtual void OnReusing() { }
    }
}
