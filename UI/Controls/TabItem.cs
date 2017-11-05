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
using Prism.UI.Media;
using Prism.UI.Media.Imaging;

namespace Prism.UI.Controls
{
    /// <summary>
    /// Represents a selectable item in a <see cref="TabView"/> instance.
    /// </summary>
    [Resolve(typeof(INativeTabItem))]
    public class TabItem : Visual
    {
        #region Property Descriptors
        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:Content"/> property.
        /// </summary>
        public static PropertyDescriptor ContentProperty { get; } = PropertyDescriptor.Create(nameof(Content), typeof(object), typeof(TabItem));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:FontFamily"/> property.
        /// </summary>
        public static PropertyDescriptor FontFamilyProperty { get; } = PropertyDescriptor.Create(nameof(FontFamily), typeof(FontFamily), typeof(TabItem));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:FontSize"/> property.
        /// </summary>
        public static PropertyDescriptor FontSizeProperty { get; } = PropertyDescriptor.Create(nameof(FontSize), typeof(double), typeof(TabItem));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:FontStyle"/> property.
        /// </summary>
        public static PropertyDescriptor FontStyleProperty { get; } = PropertyDescriptor.Create(nameof(FontStyle), typeof(FontStyle), typeof(TabItem));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:Foreground"/> property.
        /// </summary>
        public static PropertyDescriptor ForegroundProperty { get; } = PropertyDescriptor.Create(nameof(Foreground), typeof(Brush), typeof(TabItem));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:Image"/> property.
        /// </summary>
        public static PropertyDescriptor ImageProperty { get; } = PropertyDescriptor.Create(nameof(Image), typeof(ImageSource), typeof(TabItem));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:IsEnabled"/> property.
        /// </summary>
        public static PropertyDescriptor IsEnabledProperty { get; } = PropertyDescriptor.Create(nameof(IsEnabled), typeof(bool), typeof(TabItem));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:Title"/> property.
        /// </summary>
        public static PropertyDescriptor TitleProperty { get; } = PropertyDescriptor.Create(nameof(Title), typeof(string), typeof(TabItem));
        #endregion

        /// <summary>
        /// Gets or sets the object that acts as the content of the item.
        /// This is typically an <see cref="IView"/> or <see cref="ViewStack"/> instance.
        /// </summary>
        public object Content
        {
            get { return content; }
            set
            {
                if (value != content)
                {
                    var tabView = VisualTreeHelper.GetParent<TabView>(this, tv => tv.SelectedTabItem == this);
                    
                    content = value;
                    if (content is IView || content is INativeViewStack)
                    {
                        nativeObject.Content = ObjectRetriever.GetNativeObject(content);
                    }
                    else
                    {
                        object contentObj = content as ViewStack;
                        if (contentObj == null && content != null)
                        {
                            contentObj = new ContentView()
                            {
                                Content = content
                            };
                        }

                        nativeObject.Content = ObjectRetriever.GetNativeObject(contentObj);
                    }

                    if (tabView != null)
                    {
                        VisualTreeHelper.GetParent<SplitView>(tabView, sv => sv.MasterContent == tabView)?.OnMasterContentChanged();
                    }

                    OnPropertyChanged(ContentProperty);
                }
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private object content;

        /// <summary>
        /// Gets or sets the font to use for displaying the title text.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", Justification = "Exception parameter refers to property name for easier understanding of invalid value.")]
        public FontFamily FontFamily
        {
            get { return ObjectRetriever.GetAgnosticObject(nativeObject.FontFamily) as FontFamily; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(FontFamily));
                }

                nativeObject.FontFamily = ObjectRetriever.GetNativeObject(value);
            }
        }

        /// <summary>
        /// Gets or sets the size of the title text.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", Justification = "Exception parameter refers to property name for easier understanding of invalid value.")]
        public double FontSize
        {
            get { return nativeObject.FontSize; }
            set
            {
                if (double.IsNaN(value) || double.IsInfinity(value))
                {
                    throw new ArgumentException(Strings.ValueCannotBeNaNOrInfinity, nameof(FontSize));
                }

                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(FontSize), Strings.ValueCannotBeLessThanZero);
                }

                nativeObject.FontSize = value;
            }
        }

        /// <summary>
        /// Gets or sets the style with which to render the title text.
        /// </summary>
        public FontStyle FontStyle
        {
            get { return nativeObject.FontStyle; }
            set { nativeObject.FontStyle = value; }
        }

        /// <summary>
        /// Gets or sets the <see cref="Brush"/> to apply to the title.
        /// </summary>
        public Brush Foreground
        {
            get { return nativeObject.Foreground; }
            set { nativeObject.Foreground = value; }
        }

        /// <summary>
        /// Gets or sets an <see cref="ImageSource"/> for an image to display with the item.
        /// </summary>
        public ImageSource Image
        {
            get { return (ImageSource)ObjectRetriever.GetAgnosticObject(nativeObject.Image); }
            set { nativeObject.Image = (INativeImageSource)ObjectRetriever.GetNativeObject(value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the user can interact with the item.
        /// </summary>
        public bool IsEnabled
        {
            get { return nativeObject.IsEnabled; }
            set { nativeObject.IsEnabled = value; }
        }

        /// <summary>
        /// Gets or sets the title for the item.
        /// </summary>
        public string Title
        {
            get { return nativeObject.Title; }
            set { nativeObject.Title = value; }
        }

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private readonly INativeTabItem nativeObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="TabItem"/> class.
        /// </summary>
        public TabItem()
            : this(ResolveParameter.EmptyParameters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TabItem"/> class and pairs it with the specified native object.
        /// </summary>
        /// <param name="nativeObject">The native object with which to pair this instance.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="nativeObject"/> doesn't match the type specified by the topmost <see cref="ResolveAttribute"/> in the inheritance chain.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="nativeObject"/> is <c>null</c>.</exception>
        protected TabItem(INativeTabItem nativeObject)
            : base(nativeObject)
        {
            this.nativeObject = nativeObject;

            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TabItem"/> class and pairs it with a native object that is resolved from the IoC container.
        /// </summary>
        /// <param name="resolveParameters">Any parameters to pass along to the constructor of the native type.</param>
        /// <exception cref="TypeResolutionException">Thrown when the native object does not resolve to an <see cref="INativeTabItem"/> instance.</exception>
        protected TabItem(ResolveParameter[] resolveParameters)
            : base(resolveParameters)
        {
            nativeObject = ObjectRetriever.GetNativeObject(this) as INativeTabItem;
            if (nativeObject == null)
            {
                throw new TypeResolutionException(string.Format(CultureInfo.CurrentCulture, Strings.TypeMustResolveToType,
                    ObjectRetriever.GetNativeObject(this).GetType().FullName, typeof(INativeTabItem).FullName));
            }

            Initialize();
        }

        /// <summary>
        /// Called when this instance is ready to arrange its children.
        /// </summary>
        /// <param name="frame">The final rendering frame in which this instance should arrange its children.</param>
        protected sealed override void ArrangeCore(Rectangle frame)
        {
            nativeObject.Frame = new Rectangle(frame.TopLeft, DesiredSize);
        }

        /// <summary>
        /// Called when this instance is ready to be measured and returns the desired size of the object.
        /// </summary>
        /// <param name="constraints">The width and height that this instance should not exceed.</param>
        /// <returns>The desired size of the object as a <see cref="Size"/> instance.</returns>
        protected sealed override Size MeasureCore(Size constraints)
        {
            return base.MeasureCore(constraints);
        }

        internal override Size GetChildConstraints(Visual child)
        {
            return (Parent as TabView)?.GetChildConstraints(this) ?? base.GetChildConstraints(child);
        }

        private void Initialize()
        {
            SetResourceReference(FontFamilyProperty, SystemResources.BaseFontFamilyKey);
            SetResourceReference(FontSizeProperty, SystemResources.TabItemFontSizeKey);
            SetResourceReference(FontStyleProperty, SystemResources.TabItemFontStyleKey);
            SetResourceReference(ForegroundProperty, SystemResources.TabItemForegroundBrushKey);
        }
    }
}
