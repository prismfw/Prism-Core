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

namespace Prism.UI
{
    /// <summary>
    /// Represents a window for housing an application's renderable content.
    /// </summary>
    public sealed class Window : FrameworkObject
    {
        #region Event Descriptors
        /// <summary>
        /// Gets an <see cref="EventDescriptor"/> describing the <see cref="E:Activated"/> event.
        /// </summary>
        public static EventDescriptor ActivatedEvent { get; } = EventDescriptor.Create(nameof(Activated), typeof(TypedEventHandler<Window>), typeof(Window));

        /// <summary>
        /// Gets an <see cref="EventDescriptor"/> describing the <see cref="E:Closing"/> event.
        /// </summary>
        public static EventDescriptor ClosingEvent { get; } = EventDescriptor.Create(nameof(Closing), typeof(TypedEventHandler<Window, CancelEventArgs>), typeof(Window));

        /// <summary>
        /// Gets an <see cref="EventDescriptor"/> describing the <see cref="E:Deactivated"/> event.
        /// </summary>
        public static EventDescriptor DeactivatedEvent { get; } = EventDescriptor.Create(nameof(Deactivated), typeof(TypedEventHandler<Window>), typeof(Window));

        /// <summary>
        /// Gets an <see cref="EventDescriptor"/> describing the <see cref="E:OrientationChanged"/> event.
        /// </summary>
        public static EventDescriptor OrientationChangedEvent { get; } = EventDescriptor.Create(nameof(OrientationChanged), typeof(TypedEventHandler<Window, DisplayOrientationChangedEventArgs>), typeof(Window));

        /// <summary>
        /// Gets an <see cref="EventDescriptor"/> describing the <see cref="E:SizeChanged"/> event.
        /// </summary>
        public static EventDescriptor SizeChangedEvent { get; } = EventDescriptor.Create(nameof(SizeChanged), typeof(TypedEventHandler<Window, WindowSizeChangedEventArgs>), typeof(Window));
        #endregion

        /// <summary>
        /// Gets the current application window.
        /// </summary>
        public static Window Current
        {
            get { return current; }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly static Window current = new Window(typeof(INativeWindow), null);

        /// <summary>
        /// Occurs when the window is brought to the foreground.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification = "Event handler provides a strongly-typed sender for easier use.")]
        public event TypedEventHandler<Window> Activated;

        /// <summary>
        /// Occurs when the window is about to be closed.
        /// This may not fire for the main window on certain platforms.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification = "Event handler provides a strongly-typed sender for easier use.")]
        public event TypedEventHandler<Window, CancelEventArgs> Closing;

        /// <summary>
        /// Occurs when the window is pushed to the background.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification = "Event handler provides a strongly-typed sender for easier use.")]
        public event TypedEventHandler<Window> Deactivated;

        /// <summary>
        /// Occurs when the orientation of the rendered content has changed.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification = "Event handler provides a strongly-typed sender for easier use.")]
        public event TypedEventHandler<Window, DisplayOrientationChangedEventArgs> OrientationChanged;

        /// <summary>
        /// Occurs when the size of the window has changed.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification = "Event handler provides a strongly-typed sender for easier use.")]
        public event TypedEventHandler<Window, WindowSizeChangedEventArgs> SizeChanged;

        /// <summary>
        /// Gets or sets the preferred orientations in which to automatically rotate the window in response to orientation changes of the physical device.
        /// </summary>
        public DisplayOrientations AutorotationPreferences
        {
            get { return nativeObject.AutorotationPreferences; }
            set { nativeObject.AutorotationPreferences = value; }
        }

        /// <summary>
        /// Gets or sets the object that acts as the content of the window.
        /// This is typically an <see cref="IView"/> or <see cref="ViewStack"/> instance.
        /// </summary>
        public object Content
        {
            get { return content; }
            set
            {
                if (value != content)
                {
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
                }
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private object content;

        /// <summary>
        /// Gets the height of the window.
        /// </summary>
        public double Height
        {
            get { return nativeObject.Height; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is currently visible.
        /// </summary>
        public bool IsVisible
        {
            get { return nativeObject.IsVisible; }
        }

        /// <summary>
        /// Gets the current orientation of the rendered content within the window.
        /// </summary>
        public DisplayOrientations Orientation
        {
            get { return nativeObject.Orientation; }
        }

        /// <summary>
        /// Gets the popup that is currently being presented by the window.
        /// </summary>
        public Popup PresentedPopup { get; internal set; }

        /// <summary>
        /// Gets or sets the style for the window.
        /// </summary>
        public WindowStyle Style
        {
            get { return nativeObject.Style; }
            set { nativeObject.Style = value; }
        }

        /// <summary>
        /// Gets the width of the window.
        /// </summary>
        public double Width
        {
            get { return nativeObject.Width; }
        }

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        // this field is to avoid casting
        private readonly INativeWindow nativeObject;
        
        private Window(Type resolveType, string resolveName, params ResolveParameter[] resolveParams)
            : base(resolveType, resolveName, resolveParams)
        {
            nativeObject = ObjectRetriever.GetNativeObject(this) as INativeWindow;
            if (nativeObject == null)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.Strings.TypeMustResolveToType, resolveType.FullName, typeof(INativeWindow).FullName), nameof(resolveType));
            }

            nativeObject.Activated += (o, e) => OnActivated(e);
            nativeObject.Closing += (o, e) => OnClosing(e);
            nativeObject.Deactivated += (o, e) => OnDeactivated(e);
            nativeObject.OrientationChanged += (o, e) => OnOrientationChanged(e);
            nativeObject.SizeChanged += (o, e) => OnSizeChanged(e);

            AutorotationPreferences = DisplayOrientations.Portrait | DisplayOrientations.Landscape;
        }

        /// <summary>
        /// Attempts to close the window. If this is the main window, attempts to shut down the application.
        /// </summary>
        public void Close()
        {
            nativeObject.Close();
        }

        /// <summary>
        /// Sets the preferred minimum size of the window.
        /// </summary>
        /// <param name="minSize">The preferred minimum size.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="minSize"/> contains a negative, NaN, or infinite value.</exception>
        public void SetPreferredMinSize(Size minSize)
        {
            if (double.IsNaN(minSize.Width) || double.IsInfinity(minSize.Width) || double.IsNaN(minSize.Height) || double.IsInfinity(minSize.Height))
            {
                throw new ArgumentException(Resources.Strings.SizeContainsNaNOrInfiniteValue, nameof(minSize));
            }

            if (minSize.Width < 0 || minSize.Height < 0)
            {
                throw new ArgumentException(Resources.Strings.SizeContainsNegativeValue, nameof(minSize));
            }

            nativeObject.SetPreferredMinSize(minSize);
        }

        /// <summary>
        /// Displays the window if it is not already visible.
        /// </summary>
        public void Show()
        {
            nativeObject.Show();
        }

        /// <summary>
        /// Attempts to resize the window to the specified <see cref="Size"/>.
        /// </summary>
        /// <param name="newSize">The width and height at which to size the window.</param>
        /// <returns><c>true</c> if the window was successfully resized; otherwise, <c>false</c>.</returns>
        public bool TryResize(Size newSize)
        {
            return nativeObject.TryResize(newSize);
        }

        private void OnActivated(EventArgs e)
        {
            Activated?.Invoke(this, e);
        }

        private void OnClosing(CancelEventArgs e)
        {
            Closing?.Invoke(this, e);
        }

        private void OnDeactivated(EventArgs e)
        {
            Deactivated?.Invoke(this, e);
        }

        private void OnOrientationChanged(DisplayOrientationChangedEventArgs e)
        {
            OrientationChanged?.Invoke(this, e);
        }

        private void OnSizeChanged(WindowSizeChangedEventArgs e)
        {
            SizeChanged?.Invoke(this, e);

            if (!e.IsHandled)
            {
                UI.Controls.LoadIndicator.DefaultIndicator.InvalidateMeasure();
                (VisualTreeHelper.GetChild<Visual>(this))?.InvalidateMeasure();

                var popup = PresentedPopup;
                while (popup != null)
                {
                    popup.InvalidateMeasure();
                    popup.InvalidateArrange();
                    popup = popup.PresentedPopup;
                }
            }
        }
    }
}
