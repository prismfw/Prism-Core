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
    /// Represents a UI element that allows a user to select from a range of time values.
    /// </summary>
    [Resolve(typeof(INativeTimePicker))]
    public class TimePicker : Control
    {
        #region Event Descriptors
        /// <summary>
        /// Gets an <see cref="EventDescriptor"/> describing the <see cref="E:TimeChanged"/> event.
        /// </summary>
        public static EventDescriptor TimeChangedEvent { get; } = EventDescriptor.Create(nameof(TimeChanged), typeof(TypedEventHandler<TimePicker, TimeChangedEventArgs>), typeof(TimePicker));
        #endregion

        #region Property Descriptors
        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:IsOpen"/> property.
        /// </summary>
        public static PropertyDescriptor IsOpenProperty { get; } = PropertyDescriptor.Create(nameof(IsOpen), typeof(bool), typeof(TimePicker));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:SelectedTime"/> property.
        /// </summary>
        public static PropertyDescriptor SelectedTimeProperty { get; } = PropertyDescriptor.Create(nameof(SelectedTime), typeof(TimeSpan?), typeof(TimePicker), new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:TimeStringFormat"/> property.
        /// </summary>
        public static PropertyDescriptor TimeStringFormatProperty { get; } = PropertyDescriptor.Create(nameof(TimeStringFormat), typeof(string), typeof(TimePicker));
        #endregion

        /// <summary>
        /// Occurs when the selected time has changed.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification = "Event handler provides a strongly-typed sender for easier use.")]
        public event TypedEventHandler<TimePicker, TimeChangedEventArgs> TimeChanged;

        /// <summary>
        /// Gets or sets a value indicating whether the picker is open.
        /// </summary>
        public bool IsOpen
        {
            get { return nativeObject.IsOpen; }
            set { nativeObject.IsOpen = value; }
        }

        /// <summary>
        /// Gets or sets the selected time.
        /// </summary>
        public TimeSpan? SelectedTime
        {
            get { return nativeObject.SelectedTime; }
            set { nativeObject.SelectedTime = value; }
        }

        /// <summary>
        /// Gets or sets the format in which to display the string value of the selected time.
        /// </summary>
        public string TimeStringFormat
        {
            get { return nativeObject.TimeStringFormat; }
            set { nativeObject.TimeStringFormat = value; }
        }

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        // this field is to avoid casting
        private readonly INativeTimePicker nativeObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimePicker"/> class.
        /// </summary>
        public TimePicker()
            : this(ResolveParameter.EmptyParameters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimePicker"/> class and pairs it with the specified native object.
        /// </summary>
        /// <param name="nativeObject">The native object with which to pair this instance.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="nativeObject"/> doesn't match the type specified by the topmost <see cref="ResolveAttribute"/> in the inheritance chain.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="nativeObject"/> is <c>null</c>.</exception>
        protected TimePicker(INativeTimePicker nativeObject)
            : base(nativeObject)
        {
            this.nativeObject = nativeObject;

            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimePicker"/> class and pairs it with a native object that is resolved from the IoC container.
        /// </summary>
        /// <param name="resolveParameters">Any parameters to pass along to the constructor of the native type.</param>
        /// <exception cref="TypeResolutionException">Thrown when the native object does not resolve to an <see cref="INativeTimePicker"/> instance.</exception>
        protected TimePicker(ResolveParameter[] resolveParameters)
            : base(resolveParameters)
        {
            nativeObject = ObjectRetriever.GetNativeObject(this) as INativeTimePicker;
            if (nativeObject == null)
            {
                throw new TypeResolutionException(string.Format(CultureInfo.CurrentCulture, Strings.TypeMustResolveToType,
                    ObjectRetriever.GetNativeObject(this).GetType().FullName, typeof(INativeTimePicker).FullName));
            }

            Initialize();
        }

        /// <summary>
        /// Called when the selected time is changed and raises the <see cref="TimeChanged"/> event.
        /// </summary>
        /// <param name="e">The event arguments containing details about the change.</param>
        protected virtual void OnTimeChanged(TimeChangedEventArgs e)
        {
            TimeChanged?.Invoke(this, e);
        }

        private void Initialize()
        {
            nativeObject.TimeChanged += (o, e) => OnTimeChanged(e);

            SetParameterValueOverride(SelectedTimeProperty);
            SetResourceReference(BackgroundProperty, SystemResources.DateTimePickerBackgroundBrushKey);
            SetResourceReference(BorderBrushProperty, SystemResources.DateTimePickerBorderBrushKey);
            SetResourceReference(BorderWidthProperty, SystemResources.DateTimePickerBorderWidthKey);
            SetResourceReference(FontSizeProperty, SystemResources.DateTimePickerFontSizeKey);
            SetResourceReference(FontStyleProperty, SystemResources.DateTimePickerFontStyleKey);
            SetResourceReference(ForegroundProperty, SystemResources.DateTimePickerForegroundBrushKey);
        }
    }
}
