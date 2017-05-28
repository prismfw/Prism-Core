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
using Prism.UI.Media;

#if !DEBUG
using System.Diagnostics;
#endif

namespace Prism.UI.Controls
{
    /// <summary>
    /// Represents a UI element that can be toggled between a 'true' state and a 'false' state.
    /// </summary>
    [Resolve(typeof(INativeToggleSwitch))]
    public class ToggleSwitch : Control
    {
        #region Event Descriptors
        /// <summary>
        /// Gets an <see cref="EventDescriptor"/> describing the <see cref="E:ValueChanged"/> event.
        /// </summary>
        public static EventDescriptor ValueChangedEvent { get; } = EventDescriptor.Create(nameof(ValueChanged), typeof(TypedEventHandler<ToggleSwitch>), typeof(ToggleSwitch));
        #endregion

        #region Property Descriptors
        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:ThumbOffBrush"/> property.
        /// </summary>
        public static PropertyDescriptor ThumbOffBrushProperty { get; } = PropertyDescriptor.Create(nameof(ThumbOffBrush), typeof(Brush), typeof(ToggleSwitch));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:ThumbOnBrush"/> property.
        /// </summary>
        public static PropertyDescriptor ThumbOnBrushProperty { get; } = PropertyDescriptor.Create(nameof(ThumbOnBrush), typeof(Brush), typeof(ToggleSwitch));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:Value"/> property.
        /// </summary>
        public static PropertyDescriptor ValueProperty { get; } = PropertyDescriptor.Create(nameof(Value), typeof(bool), typeof(ToggleSwitch), new PropertyMetadata(PropertyMetadataOptions.BindsTwoWayByDefault));
        #endregion

        /// <summary>
        /// Occurs when the value of the toggle switch is changed.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification = "Event handler provides a strongly-typed sender for easier use.")]
        public event TypedEventHandler<ToggleSwitch> ValueChanged;

        /// <summary>
        /// Gets or sets the <see cref="Brush"/> to apply to the thumb of the control when the control's <see cref="P:Value"/> is <c>false</c>.
        /// </summary>
        public Brush ThumbOffBrush
        {
            get { return nativeObject.ThumbOffBrush; }
            set { nativeObject.ThumbOffBrush = value; }
        }

        /// <summary>
        /// Gets or sets the <see cref="Brush"/> to apply to the thumb of the control when the control's <see cref="P:Value"/> is <c>true</c>.
        /// </summary>
        public Brush ThumbOnBrush
        {
            get { return nativeObject.ThumbOnBrush; }
            set { nativeObject.ThumbOnBrush = value; }
        }

        /// <summary>
        /// Gets or sets the value of the toggle switch.
        /// </summary>
        public bool Value
        {
            get { return nativeObject.Value; }
            set { nativeObject.Value = value; }
        }

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        // this field is to avoid casting
        private readonly INativeToggleSwitch nativeObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToggleSwitch"/> class.
        /// </summary>
        public ToggleSwitch()
            : this(ResolveParameter.EmptyParameters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ToggleSwitch"/> class and pairs it with the specified native object.
        /// </summary>
        /// <param name="nativeObject">The native object with which to pair this instance.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="nativeObject"/> doesn't match the type specified by the topmost <see cref="ResolveAttribute"/> in the inheritance chain.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="nativeObject"/> is <c>null</c>.</exception>
        protected ToggleSwitch(INativeToggleSwitch nativeObject)
            : base(nativeObject)
        {
            this.nativeObject = nativeObject;

            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ToggleSwitch"/> class and pairs it with a native object that is resolved from the IoC container.
        /// </summary>
        /// <param name="resolveParameters">Any parameters to pass along to the constructor of the native type.</param>
        /// <exception cref="TypeResolutionException">Thrown when the native object does not resolve to an <see cref="INativeToggleSwitch"/> instance.</exception>
        protected ToggleSwitch(ResolveParameter[] resolveParameters)
            : base(resolveParameters)
        {
            nativeObject = ObjectRetriever.GetNativeObject(this) as INativeToggleSwitch;
            if (nativeObject == null)
            {
                throw new TypeResolutionException(string.Format(CultureInfo.CurrentCulture, Strings.TypeMustResolveToType,
                    ObjectRetriever.GetNativeObject(this).GetType().FullName, typeof(INativeToggleSwitch).FullName));
            }

            Initialize();
        }

        /// <summary>
        /// Called when the value of the toggle switch is changed and raises the <see cref="ValueChanged"/> event.
        /// </summary>
        /// <param name="e">The event arguments for the event.</param>
        protected virtual void OnValueChanged(EventArgs e)
        {
            ValueChanged?.Invoke(this, e);
        }

        private void Initialize()
        {
            nativeObject.ValueChanged += (o, e) => OnValueChanged(e);

            SetResourceReference(BackgroundProperty, SystemResources.ToggleSwitchBackgroundBrushKey);
            SetResourceReference(BorderBrushProperty, SystemResources.ToggleSwitchBorderBrushKey);
            SetResourceReference(ForegroundProperty, SystemResources.ToggleSwitchForegroundBrushKey);
            SetResourceReference(ThumbOffBrushProperty, SystemResources.ToggleSwitchThumbOffBrushKey);
            SetResourceReference(ThumbOnBrushProperty, SystemResources.ToggleSwitchThumbOnBrushKey);
        }
    }
}

