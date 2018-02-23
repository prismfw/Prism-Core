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
using System.Reflection;
using Prism.Native;
using Prism.Resources;
using Prism.UI.Media;

namespace Prism.UI.Controls
{
    /// <summary>
    /// Represents the base class for UI controls.  This class is abstract.
    /// </summary>
    public abstract class Control : Element
    {
        #region Event Descriptors
        /// <summary>
        /// Gets an <see cref="EventDescriptor"/> describing the <see cref="E:GotFocus"/> event.
        /// </summary>
        public static EventDescriptor GotFocusEvent { get; } = EventDescriptor.Create(nameof(GotFocus), typeof(TypedEventHandler<Control>), typeof(Control));

        /// <summary>
        /// Gets an <see cref="EventDescriptor"/> describing the <see cref="E:LostFocus"/> event.
        /// </summary>
        public static EventDescriptor LostFocusEvent { get; } = EventDescriptor.Create(nameof(LostFocus), typeof(TypedEventHandler<Control>), typeof(Control));
        #endregion

        #region Property Descriptors
        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:Background"/> property.
        /// </summary>
        public static PropertyDescriptor BackgroundProperty { get; } = PropertyDescriptor.Create(nameof(Background), typeof(Brush), typeof(Control));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:BorderBrush"/> property.
        /// </summary>
        public static PropertyDescriptor BorderBrushProperty { get; } = PropertyDescriptor.Create(nameof(BorderBrush), typeof(Brush), typeof(Control));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:BorderWidth"/> property.
        /// </summary>
        public static PropertyDescriptor BorderWidthProperty { get; } = PropertyDescriptor.Create(nameof(BorderWidth), typeof(double), typeof(Control), new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:FontFamily"/> property.
        /// </summary>
        public static PropertyDescriptor FontFamilyProperty { get; } = PropertyDescriptor.Create(nameof(FontFamily), typeof(FontFamily), typeof(Control));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:FontSize"/> property.
        /// </summary>
        public static PropertyDescriptor FontSizeProperty { get; } = PropertyDescriptor.Create(nameof(FontSize), typeof(double), typeof(Control));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:FontStyle"/> property.
        /// </summary>
        public static PropertyDescriptor FontStyleProperty { get; } = PropertyDescriptor.Create(nameof(FontStyle), typeof(FontStyle), typeof(Control));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:Foreground"/> property.
        /// </summary>
        public static PropertyDescriptor ForegroundProperty { get; } = PropertyDescriptor.Create(nameof(Foreground), typeof(Brush), typeof(Control));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:IsEnabled"/> property.
        /// </summary>
        public static PropertyDescriptor IsEnabledProperty { get; } = PropertyDescriptor.Create(nameof(IsEnabled), typeof(bool), typeof(Control));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:IsFocused"/> property.
        /// </summary>
        public static PropertyDescriptor IsFocusedProperty { get; } = PropertyDescriptor.Create(nameof(IsFocused), typeof(bool), typeof(Control), true);

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:ParameterId"/> property.
        /// </summary>
        public static PropertyDescriptor ParameterIdProperty { get; } = PropertyDescriptor.Create(nameof(ParameterId), typeof(string), typeof(Control));
        #endregion

        /// <summary>
        /// Occurs when the control receives focus.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification = "Event handler provides a strongly-typed sender for easier use.")]
        public event TypedEventHandler<Control> GotFocus;

        /// <summary>
        /// Occurs when the control loses focus.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification = "Event handler provides a strongly-typed sender for easier use.")]
        public event TypedEventHandler<Control> LostFocus;

        /// <summary>
        /// Gets or sets the background for the control.
        /// </summary>
        public Brush Background
        {
            get { return nativeObject.Background; }
            set { nativeObject.Background = value; }
        }

        /// <summary>
        /// Gets or sets the <see cref="Brush"/> to apply to the border of the control.
        /// </summary>
        public Brush BorderBrush
        {
            get { return nativeObject.BorderBrush; }
            set { nativeObject.BorderBrush = value; }
        }

        /// <summary>
        /// Gets or sets the width of the border around the control.
        /// </summary>
        public double BorderWidth
        {
            get { return nativeObject.BorderWidth; }
            set { nativeObject.BorderWidth = value; }
        }

        /// <summary>
        /// Gets or sets the font to use for displaying the text in the control.
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
        /// Gets or sets the size of the text in the control.
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
        /// Gets or sets the style with which to render the text in the control.
        /// </summary>
        public FontStyle FontStyle
        {
            get { return nativeObject.FontStyle; }
            set { nativeObject.FontStyle = value; }
        }

        /// <summary>
        /// Gets or sets the <see cref="Brush"/> to apply to the foreground content of the control.
        /// </summary>
        public Brush Foreground
        {
            get { return nativeObject.Foreground; }
            set { nativeObject.Foreground = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the user can interact with the control.
        /// </summary>
        public bool IsEnabled
        {
            get { return nativeObject.IsEnabled; }
            set { nativeObject.IsEnabled = value; }
        }

        /// <summary>
        /// Gets a value indicating whether the control has focus.
        /// </summary>
        public bool IsFocused
        {
            get { return nativeObject.IsFocused; }
        }

        /// <summary>
        /// Gets or sets the identifier to use when gathering control values for navigation parameters.
        /// This is used in conjunction with the value of the control to create an entry in the parameters of the navigation context.
        /// </summary>
        public string ParameterId
        {
            get { return parameterId; }
            set
            {
                if (value != parameterId)
                {
                    if (parentView != null && parameterId != null)
                    {
                        NavigationService.SetControlParameter(parentView, this);
                    }

                    parameterId = value;
                    if (parameterId != null && parentView == null && IsLoaded)
                    {
                        parentView = VisualTreeHelper.GetParent<IView>(this);
                    }

                    OnPropertyChanged(ParameterIdProperty);
                }
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string parameterId;

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        internal PropertyDescriptor ParameterValueProperty { get; private set; }
#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        // this field is to avoid casting
        private readonly INativeControl nativeObject;
#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private IView parentView;

        /// <summary>
        /// Initializes a new instance of the <see cref="Control"/> class and pairs it with the specified native object.
        /// </summary>
        /// <param name="nativeObject">The native object with which to pair this instance.</param>
        /// <exception cref="ArgumentException">Thrown when a <see cref="ResolveAttribute"/> is located in the inheritance chain and <paramref name="nativeObject"/> doesn't match the type specified by the attribute.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="nativeObject"/> is <c>null</c>.</exception>
        protected Control(INativeControl nativeObject)
            : base(nativeObject)
        {
            nativeObject = ObjectRetriever.GetNativeObject(this) as INativeControl;
            if (nativeObject == null)
            {
                throw new TypeResolutionException(string.Format(CultureInfo.CurrentCulture, Strings.TypeMustResolveToType,
                    ObjectRetriever.GetNativeObject(this).GetType().FullName, typeof(INativeControl).FullName));
            }

            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Control"/> class and pairs it with a native object that is resolved from the IoC container.
        /// At least one class in the inheritance chain must be decorated with a <see cref="ResolveAttribute"/> or an exception will be thrown.
        /// </summary>
        /// <param name="resolveParameters">Any parameters to pass along to the constructor of the native type.</param>
        /// <exception cref="TypeResolutionException">Thrown when the native object does not resolve to an <see cref="INativeControl"/> instance.</exception>
        protected Control(ResolveParameter[] resolveParameters)
            : base(resolveParameters)
        {
            nativeObject = ObjectRetriever.GetNativeObject(this) as INativeControl;
            if (nativeObject == null)
            {
                throw new TypeResolutionException(string.Format(CultureInfo.CurrentCulture, Strings.TypeMustResolveToType,
                    ObjectRetriever.GetNativeObject(this).GetType().FullName, typeof(INativeControl).FullName));
            }

            Initialize();
        }

        /// <summary>
        /// Attempts to set focus to the control.
        /// </summary>
        public void Focus()
        {
            nativeObject.Focus();
        }

        /// <summary>
        /// Sets the property that contains the value of the control for when navigation parameters are gathered.
        /// This is used in conjunction with the <see cref="ParameterId"/> to create an entry in the parameters of the navigation context. 
        /// </summary>
        /// <param name="property">The <see cref="PropertyDescriptor"/> describing the property that contains the control's value.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="property"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="property"/> does not describe a valid property on this instance.</exception>
        public void SetParameterValueOverride(PropertyDescriptor property)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            if (!property.OwnerType.GetTypeInfo().IsAssignableFrom(GetType().GetTypeInfo()))
            {
                throw new ArgumentException(Strings.OwnerTypeDoesNotMatchCurrentType, nameof(property));
            }

            ParameterValueProperty = property;
        }

        /// <summary>
        /// Attempts to remove focus from the control.
        /// </summary>
        public void Unfocus()
        {
            nativeObject.Unfocus();
        }

        /// <summary>
        /// Called when the control receives focus and raises the <see cref="GotFocus"/> event.
        /// </summary>
        /// <param name="e">The event arguments for the event.</param>
        protected virtual void OnGotFocus(EventArgs e)
        {
            GotFocus?.Invoke(this, e);
        }

        /// <summary>
        /// Called when the control loses focus and raises the <see cref="LostFocus"/> event.
        /// </summary>
        /// <param name="e">The event arguments for the event.</param>
        protected virtual void OnLostFocus(EventArgs e)
        {
            LostFocus?.Invoke(this, e);
        }

        internal override void OnLoadedInternal(object sender, EventArgs e)
        {
            if (parameterId != null)
            {
                parentView = VisualTreeHelper.GetParent<IView>(this);
            }

            base.OnLoadedInternal(sender, e);
        }

        internal override void OnUnloadedInternal(object sender, EventArgs e)
        {
            if (parentView != null && parameterId != null)
            {
                NavigationService.SetControlParameter(parentView, this);
            }

            parentView = null;
            base.OnUnloadedInternal(sender, e);
        }

        private void Initialize()
        {
            nativeObject.GotFocus += (o, e) => OnGotFocus(e);
            nativeObject.LostFocus += (o, e) => OnLostFocus(e);

            IsEnabled = true;

            SetParameterValueOverride(TagProperty);
            SetResourceReference(FontFamilyProperty, SystemResources.BaseFontFamilyKey);
        }
    }
}
