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
    [Resolve(typeof(INativeLoadIndicator))]
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
        public ActivityIndicator ActivityIndicator { get; } = new ActivityIndicator();

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
        public Label TextLabel { get; } = new Label();

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private readonly INativeLoadIndicator nativeObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoadIndicator"/> class.
        /// </summary>
        public LoadIndicator()
            : this(ResolveParameter.EmptyParameters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoadIndicator"/> class and pairs it with the specified native object.
        /// </summary>
        /// <param name="nativeObject">The native object with which to pair this instance.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="nativeObject"/> doesn't match the type specified by the topmost <see cref="ResolveAttribute"/> in the inheritance chain.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="nativeObject"/> is <c>null</c>.</exception>
        protected LoadIndicator(INativeLoadIndicator nativeObject)
            : base(nativeObject)
        {
            this.nativeObject = nativeObject;
            
            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoadIndicator"/> class and pairs it with a native object that is resolved from the IoC container.
        /// </summary>
        /// <param name="resolveParameters">Any parameters to pass along to the constructor of the native type.</param>
        /// <exception cref="TypeResolutionException">Thrown when the native object does not resolve to an <see cref="INativeLoadIndicator"/> instance.</exception>
        protected LoadIndicator(ResolveParameter[] resolveParameters)
            : base(resolveParameters)
        {
            nativeObject = ObjectRetriever.GetNativeObject(this) as INativeLoadIndicator;
            if (nativeObject == null)
            {
                throw new TypeResolutionException(string.Format(CultureInfo.CurrentCulture, Strings.TypeMustResolveToType,
                    ObjectRetriever.GetNativeObject(this).GetType().FullName, typeof(INativeLoadIndicator).FullName));
            }

            Initialize();
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

        private void Initialize()
        {
            ActivityIndicator.Height = 36;
            ActivityIndicator.Width = 36;
            ActivityIndicator.VerticalAlignment = VerticalAlignment.Center;

            TextLabel.Margin = new Thickness(12, 0, 0, 0);
            TextLabel.Text = DefaultTitle;
            TextLabel.VerticalAlignment = VerticalAlignment.Center;

            var panel = new StackPanel()
            {
                Children = { ActivityIndicator, TextLabel },
                Margin = new Thickness(8),
                HorizontalAlignment = Application.Current.Platform == Platform.Android ? HorizontalAlignment.Left : HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            nativeObject.Content = ObjectRetriever.GetNativeObject(panel);

            SetResourceReference(BackgroundProperty, SystemResources.LoadIndicatorBackgroundBrushKey);
            TextLabel.SetResourceReference(Label.FontSizeProperty, SystemResources.LoadIndicatorFontSizeKey);
            TextLabel.SetResourceReference(Label.FontStyleProperty, SystemResources.LoadIndicatorFontStyleKey);
            TextLabel.SetResourceReference(Label.ForegroundProperty, SystemResources.LoadIndicatorForegroundBrushKey);
        }
    }
}
