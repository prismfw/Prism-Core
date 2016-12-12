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
    /// Represents a text entry control for entering passwords and other sensitive information.
    /// </summary>
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
            : this(typeof(INativePasswordBox), null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PasswordBox"/> class.
        /// </summary>
        /// <param name="resolveType">The type to pass to the IoC container in order to resolve the native object.</param>
        /// <param name="resolveName">An optional name to use when resolving the native object.</param>
        /// <param name="resolveParameters">Any parameters to pass along to the constructor of the resolve type.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="resolveType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="resolveType"/> does not resolve to an <see cref="INativePasswordBox"/> instance.</exception>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", Justification = "resolveType is validated in base constructor.")]
        protected PasswordBox(Type resolveType, string resolveName, params ResolveParameter[] resolveParameters)
            : base(resolveType, resolveName, resolveParameters)
        {
            nativeObject = ObjectRetriever.GetNativeObject(this) as INativePasswordBox;
            if (nativeObject == null)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Strings.TypeMustResolveToType, resolveType.FullName, typeof(INativePasswordBox).FullName), nameof(resolveType));
            }

            nativeObject.ActionKeyPressed += (o, e) => OnActionKeyPressed(e);
            nativeObject.PasswordChanged += (o, e) => OnPasswordChanged(e);

            BorderWidth = (double)Application.Current.Resources[SystemResources.TextBoxBorderWidthKey];
            FontSize = (double)Application.Current.Resources[SystemResources.TextBoxFontSizeKey];
            FontStyle = (FontStyle)Application.Current.Resources[SystemResources.TextBoxFontStyleKey];
            HorizontalAlignment = HorizontalAlignment.Stretch;

            SetResourceReference(BackgroundProperty, SystemResources.TextBoxBackgroundBrushKey);
            SetResourceReference(BorderBrushProperty, SystemResources.TextBoxBorderBrushKey);
            SetResourceReference(ForegroundProperty, SystemResources.TextBoxForegroundBrushKey);
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
    }
}
