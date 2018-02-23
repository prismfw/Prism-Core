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
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Prism.Input;
using Prism.Native;
using Prism.Resources;

#if !DEBUG
using System.Diagnostics;
#endif

namespace Prism.UI.Controls
{
    /// <summary>
    /// Represents a UI element that can accept multiple lines of text entry from the user.
    /// </summary>
    [Resolve(typeof(INativeTextArea))]
    public class TextArea : Control
    {
        #region Event Descriptors
        /// <summary>
        /// Gets an <see cref="EventDescriptor"/> describing the <see cref="E:ActionKeyPressed"/> event.
        /// </summary>
        public static EventDescriptor ActionKeyPressedEvent { get; } = EventDescriptor.Create(nameof(ActionKeyPressed), typeof(TypedEventHandler<TextArea, HandledEventArgs>), typeof(TextArea));

        /// <summary>
        /// Gets an <see cref="EventDescriptor"/> describing the <see cref="E:TextChanged"/> event.
        /// </summary>
        public static EventDescriptor TextChangedEvent { get; } = EventDescriptor.Create(nameof(TextChanged), typeof(TypedEventHandler<TextArea, TextChangedEventArgs>), typeof(TextArea));
        #endregion

        #region Property Descriptors
        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:ActionKeyType"/> property.
        /// </summary>
        public static PropertyDescriptor ActionKeyTypeProperty { get; } = PropertyDescriptor.Create(nameof(ActionKeyType), typeof(ActionKeyType), typeof(TextArea));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:InputType"/> property.
        /// </summary>
        public static PropertyDescriptor InputTypeProperty { get; } = PropertyDescriptor.Create(nameof(InputType), typeof(InputType), typeof(TextArea));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:MaxLength"/> property.
        /// </summary>
        public static PropertyDescriptor MaxLengthProperty { get; } = PropertyDescriptor.Create(nameof(MaxLength), typeof(int), typeof(TextArea));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:MaxLines"/> property.
        /// </summary>
        public static PropertyDescriptor MaxLinesProperty { get; } = PropertyDescriptor.Create(nameof(MaxLines), typeof(int), typeof(TextArea), new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:MinLines"/> property.
        /// </summary>
        public static PropertyDescriptor MinLinesProperty { get; } = PropertyDescriptor.Create(nameof(MinLines), typeof(int), typeof(TextArea), new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:Placeholder"/> property.
        /// </summary>
        public static PropertyDescriptor PlaceholderProperty { get; } = PropertyDescriptor.Create(nameof(Placeholder), typeof(string), typeof(TextArea));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:Text"/> property.
        /// </summary>
        public static PropertyDescriptor TextProperty { get; } = PropertyDescriptor.Create(nameof(Text), typeof(string), typeof(TextArea), new PropertyMetadata(PropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:TextAlignment"/> property.
        /// </summary>
        public static PropertyDescriptor TextAlignmentProperty { get; } = PropertyDescriptor.Create(nameof(TextAlignment), typeof(TextAlignment), typeof(TextArea));
        #endregion

        /// <summary>
        /// Occurs when the action key, most commonly mapped to the "Return" key, is pressed while the control has focus.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification = "Event handler provides a strongly-typed sender for easier use.")]
        public event TypedEventHandler<TextArea, HandledEventArgs> ActionKeyPressed;

        /// <summary>
        /// Occurs when the value of the <see cref="P:Text"/> property has changed.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification = "Event handler provides a strongly-typed sender for easier use.")]
        public event TypedEventHandler<TextArea, TextChangedEventArgs> TextChanged;

        /// <summary>
        /// Gets or sets the type of action key to use for the soft keyboard when the control has focus.
        /// </summary>
        public ActionKeyType ActionKeyType
        {
            get { return nativeObject.ActionKeyType; }
            set { nativeObject.ActionKeyType = value; }
        }

        /// <summary>
        /// Gets or sets the type of text that the user is expected to input.
        /// </summary>
        public InputType InputType
        {
            get { return nativeObject.InputType; }
            set { nativeObject.InputType = value; }
        }

        /// <summary>
        /// Gets or sets the maximum number of characters that are allowed to be entered into the control.
        /// A value of 0 means there is no limit.
        /// </summary>
        public int MaxLength
        {
            get { return nativeObject.MaxLength; }
            set { nativeObject.MaxLength = Math.Max(value, 0); }
        }

        /// <summary>
        /// Gets or sets the maximum number of lines of text that should be shown.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", Justification = "Exception parameter refers to property name for easier understanding of invalid value.")]
        public int MaxLines
        {
            get { return nativeObject.MaxLines; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(MaxLines), Strings.ValueCannotBeLessThanZero);
                }

                nativeObject.MaxLines = value;
            }
        }

        /// <summary>
        /// Gets or sets the minimum number of lines of text that should be shown.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", Justification = "Exception parameter refers to property name for easier understanding of invalid value.")]
        public int MinLines
        {
            get { return nativeObject.MinLines; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(MinLines), Strings.ValueCannotBeLessThanZero);
                }

                nativeObject.MinLines = value;
            }
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
        private readonly INativeTextArea nativeObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextArea"/> class.
        /// </summary>
        public TextArea()
            : this(ResolveParameter.EmptyParameters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextArea"/> class and pairs it with the specified native object.
        /// </summary>
        /// <param name="nativeObject">The native object with which to pair this instance.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="nativeObject"/> doesn't match the type specified by the topmost <see cref="ResolveAttribute"/> in the inheritance chain.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="nativeObject"/> is <c>null</c>.</exception>
        protected TextArea(INativeTextArea nativeObject)
            : base(nativeObject)
        {
            this.nativeObject = nativeObject;

            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextArea"/> class and pairs it with a native object that is resolved from the IoC container.
        /// </summary>
        /// <param name="resolveParameters">Any parameters to pass along to the constructor of the native type.</param>
        /// <exception cref="TypeResolutionException">Thrown when the native object does not resolve to an <see cref="INativeTextArea"/> instance.</exception>
        protected TextArea(ResolveParameter[] resolveParameters)
            : base(resolveParameters)
        {
            nativeObject = ObjectRetriever.GetNativeObject(this) as INativeTextArea;
            if (nativeObject == null)
            {
                throw new TypeResolutionException(string.Format(CultureInfo.CurrentCulture, Strings.TypeMustResolveToType,
                    ObjectRetriever.GetNativeObject(this).GetType().FullName, typeof(INativeTextArea).FullName));
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
        /// Called when the value of <see cref="P:Text"/> is changed and raises the <see cref="TextChanged"/> event.
        /// </summary>
        /// <param name="e">The event arguments containing details about the change.</param>
        protected virtual void OnTextChanged(TextChangedEventArgs e)
        {
            TextChanged?.Invoke(this, e);
        }

        private void Initialize()
        {
            nativeObject.ActionKeyPressed += (o, e) => OnActionKeyPressed(e);
            nativeObject.TextChanged += (o, e) => OnTextChanged(e);

            HorizontalAlignment = HorizontalAlignment.Stretch;
            InputType = InputType.Alphanumeric;
            MaxLines = int.MaxValue;
            MinLines = 0;

            SetParameterValueOverride(TextProperty);
            SetResourceReference(BackgroundProperty, SystemResources.TextBoxBackgroundBrushKey);
            SetResourceReference(BorderBrushProperty, SystemResources.TextBoxBorderBrushKey);
            SetResourceReference(BorderWidthProperty, SystemResources.TextBoxBorderWidthKey);
            SetResourceReference(FontSizeProperty, SystemResources.TextBoxFontSizeKey);
            SetResourceReference(FontStyleProperty, SystemResources.TextBoxFontStyleKey);
            SetResourceReference(ForegroundProperty, SystemResources.TextBoxForegroundBrushKey);
        }
    }
}
