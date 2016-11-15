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
using Prism.Native;
using Prism.UI.Media;

namespace Prism.UI
{
    /// <summary>
    /// Represents a UI object that is presented to the user when an activity takes significant time to complete.
    /// </summary>
    public class LoadIndicator : FrameworkObject
    {
        #region Property Descriptors
        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:Background"/> property.
        /// </summary>
        public static PropertyDescriptor BackgroundProperty { get; } = PropertyDescriptor.Create(nameof(Background), typeof(Brush), typeof(LoadIndicator));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:FontFamily"/> property.
        /// </summary>
        public static PropertyDescriptor FontFamilyProperty { get; } = PropertyDescriptor.Create(nameof(FontFamily), typeof(FontFamily), typeof(LoadIndicator));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:FontSize"/> property.
        /// </summary>
        public static PropertyDescriptor FontSizeProperty { get; } = PropertyDescriptor.Create(nameof(FontSize), typeof(double), typeof(LoadIndicator));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:FontStyle"/> property.
        /// </summary>
        public static PropertyDescriptor FontStyleProperty { get; } = PropertyDescriptor.Create(nameof(FontStyle), typeof(FontStyle), typeof(LoadIndicator));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:Foreground"/> property.
        /// </summary>
        public static PropertyDescriptor ForegroundProperty { get; } = PropertyDescriptor.Create(nameof(Foreground), typeof(Brush), typeof(LoadIndicator));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:IsVisible"/> property.
        /// </summary>
        public static PropertyDescriptor IsVisibleProperty { get; } = PropertyDescriptor.Create(nameof(IsVisible), typeof(bool), typeof(LoadIndicator), true);

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:Title"/> property.
        /// </summary>
        public static PropertyDescriptor TitleProperty { get; } = PropertyDescriptor.Create(nameof(Title), typeof(string), typeof(LoadIndicator));
        #endregion

        /// <summary>
        /// Gets or sets the indicator that is automatically presented during navigations.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", Justification = "Exception parameter refers to property name for easier understanding of invalid value.")]
        public static LoadIndicator DefaultIndicator
        {
            get { return defaultIndicator ?? (defaultIndicator = new LoadIndicator()); }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(DefaultIndicator));
                }

                defaultIndicator = value;
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static LoadIndicator defaultIndicator;

        /// <summary>
        /// Gets or sets the default amount of time, in milliseconds, to wait before displaying the load indicator.
        /// A negative or infinity value will disable the indicator unless set through <see cref="NavigationOptions"/>.
        /// </summary>
        public static double DefaultDelay { get; set; } = 250;

        /// <summary>
        /// Gets or sets the title text to use on an indicator that does not have its <see cref="Title"/> set.
        /// </summary>
        public static string DefaultTitle { get; set; } = "Loading...";

        /// <summary>
        /// Gets or sets the background of the indicator.
        /// </summary>
        public Brush Background
        {
            get { return nativeObject.Background; }
            set { nativeObject.Background = value; }
        }

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
                    throw new ArgumentException(Resources.Strings.ValueCannotBeNaNOrInfinity, nameof(FontSize));
                }

                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(FontSize), Resources.Strings.ValueCannotBeLessThanZero);
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
        /// Gets or sets the <see cref="Brush"/> to apply to the title text.
        /// </summary>
        public Brush Foreground
        {
            get { return nativeObject.Foreground; }
            set { nativeObject.Foreground = value; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is currently visible.
        /// </summary>
        public bool IsVisible
        {
            get { return nativeObject.IsVisible; }
        }

        /// <summary>
        /// Gets or sets the title text of the indicator.
        /// </summary>
        public string Title
        {
            get { return nativeObject.Title; }
            set { nativeObject.Title = value; }
        }

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private readonly INativeLoadIndicator nativeObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoadIndicator"/> class.
        /// </summary>
        public LoadIndicator()
            : this(typeof(INativeLoadIndicator), null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoadIndicator"/> class.
        /// </summary>
        /// <param name="resolveType">The type to pass to the IoC container in order to resolve the native object.</param>
        /// <param name="resolveName">An optional name to use when resolving the native object.</param>
        /// <param name="resolveParameters">Any parameters to pass along to the constructor of the resolve type.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="resolveType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="resolveType"/> does not resolve to an <see cref="INativeLoadIndicator"/> instance.</exception>
        protected LoadIndicator(Type resolveType, string resolveName, params ResolveParameter[] resolveParameters)
            : base(resolveType, resolveName, resolveParameters)
        {
            nativeObject = ObjectRetriever.GetNativeObject(this) as INativeLoadIndicator;
            if (nativeObject == null)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.Strings.TypeMustResolveToType, resolveType.FullName, typeof(INativeLoadIndicator).FullName), nameof(resolveType));
            }

            FontFamily = (FontFamily)Application.Current.Resources[SystemResources.BaseFontFamilyKey];
            FontSize = (double)Application.Current.Resources[SystemResources.LoadIndicatorFontSizeKey];
            FontStyle = (FontStyle)Application.Current.Resources[SystemResources.LoadIndicatorFontStyleKey];
            Title = DefaultTitle;
        }

        /// <summary>
        /// Removes the indicator from view.
        /// </summary>
        public void Hide()
        {
            nativeObject.Hide();
        }

        /// <summary>
        /// Displays the indicator.
        /// </summary>
        public void Show()
        {
            nativeObject.Show();
        }
    }
}
