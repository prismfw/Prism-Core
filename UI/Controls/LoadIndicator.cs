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

namespace Prism.UI.Controls
{
    /// <summary>
    /// Represents a UI object that is presented to the user when an activity takes significant time to complete.
    /// </summary>
    public class LoadIndicator : Visual
    {
        #region Property Descriptors
        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:Background"/> property.
        /// </summary>
        public static PropertyDescriptor BackgroundProperty { get; } = PropertyDescriptor.Create(nameof(Background), typeof(Brush), typeof(LoadIndicator));
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
        /// Gets or sets the title text to use on an indicator that does not have any title text set.
        /// </summary>
        public static string DefaultTitle { get; set; } = "Loading...";

        /// <summary>
        /// Gets the <see cref="Controls.ActivityIndicator"/> that is displayed by this instance.
        /// </summary>
        public ActivityIndicator ActivityIndicator { get; }

        /// <summary>
        /// Gets or sets the background of the indicator.
        /// </summary>
        public Brush Background
        {
            get { return nativeObject.Background; }
            set { nativeObject.Background = value; }
        }

        /// <summary>
        /// Gets the <see cref="Label"/> that displays the title text for this instance.
        /// </summary>
        public Label TextLabel { get; }

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
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", Justification = "resolveType is validated in base constructor.")]
        protected LoadIndicator(Type resolveType, string resolveName, params ResolveParameter[] resolveParameters)
            : base(resolveType, resolveName, resolveParameters)
        {
            nativeObject = ObjectRetriever.GetNativeObject(this) as INativeLoadIndicator;
            if (nativeObject == null)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Strings.TypeMustResolveToType, resolveType.FullName, typeof(INativeLoadIndicator).FullName), nameof(resolveType));
            }

            var panel = new StackPanel()
            {
                Children =
                {
                    (ActivityIndicator = new ActivityIndicator()
                    {
                        Height = 36,
                        Width = 36,
                        VerticalAlignment = VerticalAlignment.Center,
                    }),
                    (TextLabel = new Label()
                    {
                        FontSize = (double)Application.Current.Resources[SystemResources.LoadIndicatorFontSizeKey],
                        FontStyle = (FontStyle)Application.Current.Resources[SystemResources.LoadIndicatorFontStyleKey],
                        Margin = new Thickness(12, 0, 0, 0),
                        Text = DefaultTitle,
                        VerticalAlignment = VerticalAlignment.Center
                    })
                },
                Margin = new Thickness(8),
                HorizontalAlignment = Application.Current.Platform == Platform.Android ? HorizontalAlignment.Left : HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center 
            };

            nativeObject.Content = ObjectRetriever.GetNativeObject(panel);

            SetResourceReference(BackgroundProperty, SystemResources.LoadIndicatorBackgroundBrushKey);
            TextLabel.SetResourceReference(Label.ForegroundProperty, SystemResources.LoadIndicatorForegroundBrushKey);
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

        /// <summary>
        /// Called when this instance is ready to arrange its children.
        /// </summary>
        /// <param name="frame">The final rendering frame in which this instance should arrange its children.</param>
        protected sealed override void ArrangeCore(Rectangle frame)
        {
            frame.Width = Math.Min(DesiredSize.Width, frame.Width);
            frame.Height = Math.Min(DesiredSize.Height, frame.Height);
            frame.X = Math.Max(0, (Window.Current.Width - frame.Width) / 2);
            frame.Y = Math.Max(0, (Window.Current.Height - frame.Height) / 2);

            nativeObject.Frame = frame;
            var content = ObjectRetriever.GetAgnosticObject(nativeObject.Content) as Visual ?? VisualTreeHelper.GetChild<Visual>(this);
            if (content != null)
            {
                content.Arrange(new Rectangle(new Point(), frame.Size));
            }
        }

        /// <summary>
        /// Called when this instance is ready to be measured and returns the desired size of the object.
        /// </summary>
        /// <param name="constraints">The width and height that this instance should not exceed.</param>
        /// <returns>The desired size of the object as a <see cref="Size"/> instance.</returns>
        protected sealed override Size MeasureCore(Size constraints)
        {
            var content = ObjectRetriever.GetAgnosticObject(nativeObject.Content) as Visual ?? VisualTreeHelper.GetChild<Visual>(this);
            if (content != null)
            {
                content.Measure(constraints);
            }

            return nativeObject.Measure(constraints);
        }
    }
}
