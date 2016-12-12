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
    /// Represents a UI element that can accept a single line of text entry from the user.
    /// </summary>
    public class TextBox : Control
    {
        #region Event Descriptors
        /// <summary>
        /// Gets an <see cref="EventDescriptor"/> describing the <see cref="E:ActionKeyPressed"/> event.
        /// </summary>
        public static EventDescriptor ActionKeyPressedEvent { get; } = EventDescriptor.Create(nameof(ActionKeyPressed), typeof(TypedEventHandler<TextBox, HandledEventArgs>), typeof(TextBox));

        /// <summary>
        /// Gets an <see cref="EventDescriptor"/> describing the <see cref="E:TextChanged"/> event.
        /// </summary>
        public static EventDescriptor TextChangedEvent { get; } = EventDescriptor.Create(nameof(TextChanged), typeof(TypedEventHandler<TextBox, TextChangedEventArgs>), typeof(TextBox));
        #endregion

        #region Property Descriptors
        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:ActionKeyType"/> property.
        /// </summary>
        public static PropertyDescriptor ActionKeyTypeProperty { get; } = PropertyDescriptor.Create(nameof(ActionKeyType), typeof(ActionKeyType), typeof(TextBox));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:Placeholder"/> property.
        /// </summary>
        public static PropertyDescriptor PlaceholderProperty { get; } = PropertyDescriptor.Create(nameof(Placeholder), typeof(string), typeof(TextBox));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:Text"/> property.
        /// </summary>
        public static PropertyDescriptor TextProperty { get; } = PropertyDescriptor.Create(nameof(Text), typeof(string), typeof(TextBox), new PropertyMetadata(PropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:TextAlignment"/> property.
        /// </summary>
        public static PropertyDescriptor TextAlignmentProperty { get; } = PropertyDescriptor.Create(nameof(TextAlignment), typeof(TextAlignment), typeof(TextBox));
        #endregion

        /// <summary>
        /// Occurs when the action key, most commonly mapped to the "Return" key, is pressed while the control has focus.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification = "Event handler provides a strongly-typed sender for easier use.")]
        public event TypedEventHandler<TextBox, HandledEventArgs> ActionKeyPressed;

        /// <summary>
        /// Occurs when the value of the <see cref="P:Text"/> property has changed.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification = "Event handler provides a strongly-typed sender for easier use.")]
        public event TypedEventHandler<TextBox, TextChangedEventArgs> TextChanged;

        /// <summary>
        /// Gets or sets the type of action key to use for the soft keyboard when the control has focus.
        /// </summary>
        public ActionKeyType ActionKeyType
        {
            get { return nativeObject.ActionKeyType; }
            set { nativeObject.ActionKeyType = value; }
        }

        /// <summary>
        /// Gets or sets the text to display when the control does not have a value.
        /// </summary>
        public string Placeholder
        {
            get { return nativeObject.Placeholder; }
            set { nativeObject.Placeholder = value; }
        }

        /// <summary>
        /// Gets or sets the text value of the control.
        /// </summary>
        public string Text
        {
            get { return nativeObject.Text; }
            set { nativeObject.Text = value; }
        }

        /// <summary>
        /// Gets or sets the manner in which the text is aligned within the control.
        /// </summary>
        public TextAlignment TextAlignment
        {
            get { return nativeObject.TextAlignment; }
            set { nativeObject.TextAlignment = value; }
        }

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        // this field is to avoid casting
        private readonly INativeTextBox nativeObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextBox"/> class.
        /// </summary>
        public TextBox()
            : this(typeof(INativeTextBox), null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextBox"/> class.
        /// </summary>
        /// <param name="resolveType">The type to pass to the IoC container in order to resolve the native object.</param>
        /// <param name="resolveName">An optional name to use when resolving the native object.</param>
        /// <param name="resolveParameters">Any parameters to pass along to the constructor of the resolve type.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="resolveType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="resolveType"/> does not resolve to an <see cref="INativeTextBox"/> instance.</exception>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", Justification = "resolveType is validated in base constructor.")]
        protected TextBox(Type resolveType, string resolveName, params ResolveParameter[] resolveParameters)
            : base(resolveType, resolveName, resolveParameters)
        {
            nativeObject = ObjectRetriever.GetNativeObject(this) as INativeTextBox;
            if (nativeObject == null)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Strings.TypeMustResolveToType, resolveType.FullName, typeof(INativeTextBox).FullName), nameof(resolveType));
            }

            nativeObject.ActionKeyPressed += (o, e) => OnActionKeyPressed(e);
            nativeObject.TextChanged += (o, e) => OnTextChanged(e);

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
        /// Called when the value of <see cref="P:Text"/> is changed and raises the <see cref="TextChanged"/> event.
        /// </summary>
        /// <param name="e">The event arguments containing details about the change.</param>
        public virtual void OnTextChanged(TextChangedEventArgs e)
        {
            TextChanged?.Invoke(this, e);
        }
    }
}
