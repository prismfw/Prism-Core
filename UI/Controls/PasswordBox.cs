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

#if !DEBUG
using System.Diagnostics;
#endif

namespace Prism.UI.Controls
{
    /// <summary>
    /// Represents a text entry control for entering passwords and other sensitive information.
    /// </summary>
    [Resolve(typeof(INativePasswordBox))]
    public class PasswordBox : Control
    {
        #region Event Descriptors
        /// <summary>
        /// Gets an <see cref="EventDescriptor"/> describing the <see cref="E:ActionKeyPressed"/> event.
        /// </summary>
        public static EventDescriptor ActionKeyPressedEvent { get; } = EventDescriptor.Create(nameof(ActionKeyPressed), typeof(TypedEventHandler<PasswordBox, HandledEventArgs>), typeof(PasswordBox));

        /// <summary>
        /// Gets an <see cref="EventDescriptor"/> describing the <see cref="E:PasswordChanged"/> event.
        /// </summary>
        public static EventDescriptor PasswordChangedEvent { get; } = EventDescriptor.Create(nameof(PasswordChanged), typeof(TypedEventHandler<PasswordBox>), typeof(PasswordBox));
        #endregion

        #region Property Descriptors
        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:ActionKeyType"/> property.
        /// </summary>
        public static PropertyDescriptor ActionKeyTypeProperty { get; } = PropertyDescriptor.Create(nameof(ActionKeyType), typeof(ActionKeyType), typeof(PasswordBox));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:Password"/> property.
        /// </summary>
        public static PropertyDescriptor PasswordProperty { get; } = PropertyDescriptor.Create(nameof(Password), typeof(string), typeof(PasswordBox));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:Placeholder"/> property.
        /// </summary>
        public static PropertyDescriptor PlaceholderProperty { get; } = PropertyDescriptor.Create(nameof(Placeholder), typeof(string), typeof(PasswordBox));
        #endregion

        /// <summary>
        /// Occurs when the action key, most commonly mapped to the "Return" key, is pressed while the control has focus.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification = "Event handler provides a strongly-typed sender for easier use.")]
        public event TypedEventHandler<PasswordBox, HandledEventArgs> ActionKeyPressed;

        /// <summary>
        /// Occurs when the value of the <see cref="P:Password"/> property has changed.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification = "Event handler provides a strongly-typed sender for easier use.")]
        public event TypedEventHandler<PasswordBox> PasswordChanged;

        /// <summary>
        /// Gets or sets the type of action key to use for the soft keyboard when the control has focus.
        /// </summary>
        public ActionKeyType ActionKeyType
        {
            get { return nativeObject.ActionKeyType; }
            set { nativeObject.ActionKeyType = value; }
        }

        /// <summary>
        /// Gets or sets the password value of the control.
        /// </summary>
        public string Password
        {
            get { return nativeObject.Password; }
            set { nativeObject.Password = value; }
        }

        /// <summary>
        /// Gets or sets the text to display when the control does not have a value.
        /// </summary>
        public string Placeholder
        {
            get { return nativeObject.Placeholder; }
            set { nativeObject.Placeholder = value; }
        }

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        // this field is to avoid casting
        private readonly INativePasswordBox nativeObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="PasswordBox"/> class.
        /// </summary>
        public PasswordBox()
            : this(ResolveParameter.EmptyParameters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PasswordBox"/> class and pairs it with the specified native object.
        /// </summary>
        /// <param name="nativeObject">The native object with which to pair this instance.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="nativeObject"/> doesn't match the type specified by the topmost <see cref="ResolveAttribute"/> in the inheritance chain.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="nativeObject"/> is <c>null</c>.</exception>
        protected PasswordBox(INativePasswordBox nativeObject)
            : base(nativeObject)
        {
            this.nativeObject = nativeObject;

            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PasswordBox"/> class and pairs it with a native object that is resolved from the IoC container.
        /// </summary>
        /// <param name="resolveParameters">Any parameters to pass along to the constructor of the native type.</param>
        /// <exception cref="TypeResolutionException">Thrown when the native object does not resolve to an <see cref="INativePasswordBox"/> instance.</exception>
        protected PasswordBox(ResolveParameter[] resolveParameters)
            : base(resolveParameters)
        {
            nativeObject = ObjectRetriever.GetNativeObject(this) as INativePasswordBox;
            if (nativeObject == null)
            {
                throw new TypeResolutionException(string.Format(CultureInfo.CurrentCulture, Strings.TypeMustResolveToType,
                    ObjectRetriever.GetNativeObject(this).GetType().FullName, typeof(INativePasswordBox).FullName));
            }

            Initialize();
        }

        /// <summary>
        /// Called when the action key is pressed while the control has focus and raises the <see cref="ActionKeyPressed"/> event.
        /// </summary>
        /// <param name="e">The event arguments for the event.</param>
        protected virtual void OnActionKeyPressed(HandledEventArgs e)
        {
            ActionKeyPressed?.Invoke(this, e);
        }

        /// <summary>
        /// Called when the value of <see cref="P:Password"/> is changed and raises the <see cref="PasswordChanged"/> event.
        /// </summary>
        /// <param name="e">The event arguments for the event.</param>
        protected virtual void OnPasswordChanged(EventArgs e)
        {
            PasswordChanged?.Invoke(this, e);
        }

        private void Initialize()
        {
            nativeObject.ActionKeyPressed += (o, e) => OnActionKeyPressed(e);
            nativeObject.PasswordChanged += (o, e) => OnPasswordChanged(e);

            HorizontalAlignment = HorizontalAlignment.Stretch;

            SetResourceReference(BackgroundProperty, SystemResources.TextBoxBackgroundBrushKey);
            SetResourceReference(BorderBrushProperty, SystemResources.TextBoxBorderBrushKey);
            SetResourceReference(BorderWidthProperty, SystemResources.TextBoxBorderWidthKey);
            SetResourceReference(FontSizeProperty, SystemResources.TextBoxFontSizeKey);
            SetResourceReference(FontStyleProperty, SystemResources.TextBoxFontStyleKey);
            SetResourceReference(ForegroundProperty, SystemResources.TextBoxForegroundBrushKey);
        }
    }
}
