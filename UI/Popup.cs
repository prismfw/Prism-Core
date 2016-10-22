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
using Prism.Resources;
using Prism.Systems;

namespace Prism.UI
{
    /// <summary>
    /// Represents a UI container that presents its content above the application window.
    /// </summary>
    public class Popup : Visual
    {
        #region Event Descriptors
        /// <summary>
        /// Gets an <see cref="EventDescriptor"/> describing the <see cref="E:Closed"/> event.
        /// </summary>
        public static EventDescriptor ClosedEvent { get; } = EventDescriptor.Create(nameof(Closed), typeof(TypedEventHandler<Popup>), typeof(Popup));

        /// <summary>
        /// Gets an <see cref="EventDescriptor"/> describing the <see cref="E:Opened"/> event.
        /// </summary>
        public static EventDescriptor OpenedEvent { get; } = EventDescriptor.Create(nameof(Opened), typeof(TypedEventHandler<Popup>), typeof(Popup));
        #endregion

        #region Property Descriptors
        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:Content"/> property.
        /// </summary>
        public static PropertyDescriptor ContentProperty { get; } = PropertyDescriptor.Create(nameof(Content), typeof(object), typeof(Popup), new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:Height"/> property.
        /// </summary>
        public static PropertyDescriptor HeightProperty { get; } = PropertyDescriptor.Create(nameof(Height), typeof(double), typeof(Popup), new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:IsLightDismissEnabled"/> property.
        /// </summary>
        public static PropertyDescriptor IsLightDismissEnabledProperty { get; } = PropertyDescriptor.Create(nameof(IsLightDismissEnabled), typeof(bool), typeof(Popup));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:IsOpen"/> property.
        /// </summary>
        public static PropertyDescriptor IsOpenProperty { get; } = PropertyDescriptor.Create(nameof(IsOpen), typeof(bool), typeof(Popup), true);

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:PresentationStyle"/> property.
        /// </summary>
        public static PropertyDescriptor PresentationStyleProperty { get; } = PropertyDescriptor.Create(nameof(PresentationStyle), typeof(PopupPresentationStyle), typeof(Popup));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:Width"/> property.
        /// </summary>
        public static PropertyDescriptor WidthProperty { get; } = PropertyDescriptor.Create(nameof(Width), typeof(double), typeof(Popup), new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.AffectsMeasure));
        #endregion

        /// <summary>
        /// Occurs when the popup is closed.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification = "Event handler provides a strongly-typed sender for easier use.")]
        public event TypedEventHandler<Popup> Closed;

        /// <summary>
        /// Occurs when the popup is opened.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification = "Event handler provides a strongly-typed sender for easier use.")]
        public event TypedEventHandler<Popup> Opened;

        /// <summary>
        /// Gets or sets the object that acts as the content of the popup.
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
        /// Gets or sets the suggested height for the popup.
        /// The popup must have a presentation style of <see cref="PopupPresentationStyle.Custom"/> for this to have any effect.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", Justification = "Exception parameter refers to property name for easier understanding of invalid value.")]
        public double Height
        {
            get { return height; }
            set
            {
                if (value != height)
                {
                    if (double.IsInfinity(value))
                    {
                        throw new ArgumentException(Strings.ValueCannotBeInfinity, nameof(Height));
                    }

                    if (value < 0)
                    {
                        throw new ArgumentOutOfRangeException(nameof(Height), Strings.ValueCannotBeLessThanZero);
                    }

                    height = value;
                    OnPropertyChanged(HeightProperty);
                }
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private double height = double.NaN;

        /// <summary>
        /// Gets or sets a value indicating whether the popup can be dismissed by pressing outside of its bounds.
        /// </summary>
        public bool IsLightDismissEnabled
        {
            get { return nativeObject.IsLightDismissEnabled; }
            set { nativeObject.IsLightDismissEnabled = value; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is open and visible.
        /// </summary>
        public bool IsOpen { get; private set; }

        /// <summary>
        /// Gets or sets the style in which the popup is presented.
        /// </summary>
        public PopupPresentationStyle PresentationStyle
        {
            get { return presentationStyle; }
            set
            {
                if (value != presentationStyle)
                {
                    presentationStyle = value;
                    OnPropertyChanged(PresentationStyleProperty);
                }
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private PopupPresentationStyle presentationStyle;

        /// <summary>
        /// Gets the popup that is currently being presented by this instance.
        /// </summary>
        public Popup PresentedPopup { get; private set; }

        /// <summary>
        /// Gets or sets the suggested width for the popup.
        /// The popup must have a presentation style of <see cref="PopupPresentationStyle.Custom"/> for this to have any effect.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", Justification = "Exception parameter refers to property name for easier understanding of invalid value.")]
        public double Width
        {
            get { return width; }
            set
            {
                if (value != width)
                {
                    if (double.IsInfinity(value))
                    {
                        throw new ArgumentException(Strings.ValueCannotBeInfinity, nameof(Width));
                    }

                    if (value < 0)
                    {
                        throw new ArgumentOutOfRangeException(nameof(Width), Strings.ValueCannotBeLessThanZero);
                    }

                    width = value;
                    OnPropertyChanged(WidthProperty);
                }
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private double width = double.NaN;

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        // this field is to avoid casting
        private readonly INativePopup nativeObject;

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private object presentingObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="Popup"/> class.
        /// </summary>
        public Popup()
            : this(typeof(INativePopup), null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Popup"/> class.
        /// </summary>
        /// <param name="resolveType">The type to pass to the IoC container in order to resolve the native object.</param>
        /// <param name="resolveName">An optional name to use when resolving the native object.</param>
        /// <param name="resolveParameters">Any parameters to pass along to the constructor of the resolve type.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="resolveType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="resolveType"/> does not resolve to an <see cref="INativePopup"/> instance.</exception>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", Justification = "resolveType is validated in base constructor.")]
        protected Popup(Type resolveType, string resolveName, params ResolveParameter[] resolveParameters)
            : base(resolveType, resolveName, resolveParameters)
        {
            nativeObject = ObjectRetriever.GetNativeObject(this) as INativePopup;
            if (nativeObject == null)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Strings.TypeMustResolveToType, resolveType.FullName, typeof(INativePopup).FullName), nameof(resolveType));
            }

            nativeObject.Closed += (o, e) =>
            {
                var window = presentingObject as Window;
                if (window != null && window.PresentedPopup == this)
                {
                    window.PresentedPopup = null;
                }
                else
                {
                    var popup = presentingObject as Popup;
                    if (popup != null && popup.PresentedPopup == this)
                    {
                        popup.PresentedPopup = null;
                    }
                }

                PresentedPopup?.Close();

                IsOpen = false;
                OnPropertyChanged(IsOpenProperty);
                OnClosed(e);
            };

            nativeObject.Opened += (o, e) =>
            {
                IsOpen = true;
                OnPropertyChanged(IsOpenProperty);
                OnOpened(e);
            };
        }

        /// <summary>
        /// Closes the popup.
        /// </summary>
        public void Close()
        {
            nativeObject.Close();
        }

        /// <summary>
        /// Opens the popup as a child of the specified <see cref="Popup"/>.
        /// </summary>
        /// <param name="presenter">The <see cref="Popup"/> through which to present this instance.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="presenter"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="presenter"/> is the same as this instance -or- when <paramref name="presenter"/> is already presenting another popup.</exception>
        public void Open(Popup presenter)
        {
            if (presenter == null)
            {
                throw new ArgumentNullException(nameof(presenter));
            }

            if (presenter == this)
            {
                throw new ArgumentException(Strings.PopupCannotPresentSelf, nameof(presenter));
            }

            if (presenter.PresentedPopup == null)
            {
                presenter.PresentedPopup = this;
                presentingObject = presenter;
                nativeObject.Open(ObjectRetriever.GetNativeObject(presenter),
                    Device.Current.FormFactor == FormFactor.Phone ? PopupPresentationStyle.FullScreen : presentationStyle);
            }
            else if (presenter.PresentedPopup != this)
            {
                throw new ArgumentException(Strings.PresenterAlreadyPresentingPopup, nameof(presenter));
            }
        }

        /// <summary>
        /// Opens the popup as a child of the specified <see cref="Window"/>.
        /// </summary>
        /// <param name="presenter">The <see cref="Window"/> through which to present this instance.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="presenter"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="presenter"/> is already presenting another popup.</exception>
        public void Open(Window presenter)
        {
            if (presenter == null)
            {
                throw new ArgumentNullException(nameof(presenter));
            }

            if (presenter.PresentedPopup == null)
            {
                presenter.PresentedPopup = this;
                presentingObject = presenter;
                nativeObject.Open(ObjectRetriever.GetNativeObject(presenter),
                    Device.Current.FormFactor == FormFactor.Phone ? PopupPresentationStyle.FullScreen : presentationStyle);
            }
            else if (presenter.PresentedPopup != this)
            {
                throw new ArgumentException(Strings.PresenterAlreadyPresentingPopup, nameof(presenter));
            }
        }

        /// <summary>
        /// Called when this instance is ready to arrange its children.
        /// </summary>
        /// <param name="frame">The final rendering frame in which this instance should arrange its children.</param>
        protected sealed override void ArrangeCore(Rectangle frame)
        {
            frame.Width = DesiredSize.Width;
            frame.Height = DesiredSize.Height;

            VisualTreeHelper.GetChild<Visual>(this)?.Arrange(new Rectangle(0, 0, frame.Width, frame.Height));

            frame.X = (Window.Current.Width - frame.Width) / 2;
            frame.Y = (Window.Current.Height - frame.Height) / 2;
            nativeObject.Frame = frame;

            PresentedPopup?.Arrange(new Rectangle(0, 0, Window.Current.Width, Window.Current.Height));
        }

        /// <summary>
        /// Called when this instance is ready to be measured and returns the desired size of the object.
        /// </summary>
        /// <param name="constraints">The width and height that this instance should not exceed.</param>
        /// <returns>The desired size of the object as a <see cref="Size"/> instance.</returns>
        protected sealed override Size MeasureCore(Size constraints)
        {
            constraints = base.MeasureCore(constraints);
            
            switch (Device.Current.FormFactor == FormFactor.Phone ? PopupPresentationStyle.FullScreen : presentationStyle)
            {
                case PopupPresentationStyle.Default:
                    constraints.Width = SystemParameters.PopupSize.Width;
                    constraints.Height = SystemParameters.PopupSize.Height;
                    break;
                case PopupPresentationStyle.FullScreen:
                    constraints.Width = Window.Current.Width;
                    constraints.Height = Window.Current.Height;
                    break;
                case PopupPresentationStyle.Custom:
                    constraints.Width = Math.Min(constraints.Width, double.IsNaN(Width) ? SystemParameters.PopupSize.Width : Width);
                    constraints.Height = Math.Min(constraints.Height, double.IsNaN(Height) ? SystemParameters.PopupSize.Height : Height);
                    break;
            }

            constraints.Width = Math.Min(constraints.Width, Window.Current.Width);
            constraints.Height = Math.Min(constraints.Height, Window.Current.Height);

            VisualTreeHelper.GetChild<Visual>(this)?.Measure(constraints);
            PresentedPopup?.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            return constraints;
        }

        /// <summary>
        /// Called when the popup is closed and raises the <see cref="Closed"/> event.
        /// </summary>
        /// <param name="e">The event arguments for the event.</param>
        protected virtual void OnClosed(EventArgs e)
        {
            Closed?.Invoke(this, e);
        }

        /// <summary>
        /// Called when the popup is opened and raises the <see cref="Opened"/> event.
        /// </summary>
        /// <param name="e">The event arguments for the event.</param>
        protected virtual void OnOpened(EventArgs e)
        {   
            Opened?.Invoke(this, e);
        }
    }
}
