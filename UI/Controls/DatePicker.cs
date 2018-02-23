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
using Prism.Native;
using Prism.Resources;

#if !DEBUG
using System.Diagnostics;
#endif

namespace Prism.UI.Controls
{
    /// <summary>
    /// Represents a UI element that allows a user to select from a range of date values.
    /// </summary>
    [Resolve(typeof(INativeDatePicker))]
    public class DatePicker : Control
    {
        #region Event Descriptors
        /// <summary>
        /// Gets an <see cref="EventDescriptor"/> describing the <see cref="E:DateChanged"/> event.
        /// </summary>
        public static EventDescriptor DateChangedEvent { get; } = EventDescriptor.Create(nameof(DateChanged), typeof(TypedEventHandler<DatePicker, DateChangedEventArgs>), typeof(DatePicker));
        #endregion

        #region Property Descriptors
        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:DateStringFormat"/> property.
        /// </summary>
        public static PropertyDescriptor DateStringFormatProperty { get; } = PropertyDescriptor.Create(nameof(DateStringFormat), typeof(string), typeof(DatePicker));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:IsOpen"/> property.
        /// </summary>
        public static PropertyDescriptor IsOpenProperty { get; } = PropertyDescriptor.Create(nameof(IsOpen), typeof(bool), typeof(DatePicker));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:SelectedDate"/> property.
        /// </summary>
        public static PropertyDescriptor SelectedDateProperty { get; } = PropertyDescriptor.Create(nameof(SelectedDate), typeof(DateTime?), typeof(DatePicker), new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.AffectsMeasure));
        #endregion

        /// <summary>
        /// Occurs when the selected date has changed.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification = "Event handler provides a strongly-typed sender for easier use.")]
        public event TypedEventHandler<DatePicker, DateChangedEventArgs> DateChanged;

        /// <summary>
        /// Gets or sets the format in which to display the string value of the selected date.
        /// </summary>
        public string DateStringFormat
        {
            get { return nativeObject.DateStringFormat; }
            set { nativeObject.DateStringFormat = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the picker is open.
        /// </summary>
        public bool IsOpen
        {
            get { return nativeObject.IsOpen; }
            set { nativeObject.IsOpen = value; }
        }

        /// <summary>
        /// Gets or sets the selected date.
        /// </summary>
        public DateTime? SelectedDate
        {
            get { return nativeObject.SelectedDate; }
            set { nativeObject.SelectedDate = value; }
        }

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        // this field is to avoid casting
        private readonly INativeDatePicker nativeObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatePicker"/> class.
        /// </summary>
        public DatePicker()
            : this(ResolveParameter.EmptyParameters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatePicker"/> class and pairs it with the specified native object.
        /// </summary>
        /// <param name="nativeObject">The native object with which to pair this instance.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="nativeObject"/> doesn't match the type specified by the topmost <see cref="ResolveAttribute"/> in the inheritance chain.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="nativeObject"/> is <c>null</c>.</exception>
        protected DatePicker(INativeDatePicker nativeObject)
            : base(nativeObject)
        {
            this.nativeObject = nativeObject;

            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatePicker"/> class and pairs it with a native object that is resolved from the IoC container.
        /// </summary>
        /// <param name="resolveParameters">Any parameters to pass along to the constructor of the native type.</param>
        /// <exception cref="TypeResolutionException">Thrown when the native object does not resolve to an <see cref="INativeDatePicker"/> instance.</exception>
        protected DatePicker(ResolveParameter[] resolveParameters)
            : base(resolveParameters)
        {
            nativeObject = ObjectRetriever.GetNativeObject(this) as INativeDatePicker;
            if (nativeObject == null)
            {
                throw new TypeResolutionException(string.Format(CultureInfo.CurrentCulture, Strings.TypeMustResolveToType,
                    ObjectRetriever.GetNativeObject(this).GetType().FullName, typeof(INativeDatePicker).FullName));
            }

            Initialize();
        }

        /// <summary>
        /// Called when the selected date is changed and raises the <see cref="DateChanged"/> event.
        /// </summary>
        /// <param name="e">The event arguments containing details about the change.</param>
        protected virtual void OnDateChanged(DateChangedEventArgs e)
        {
            DateChanged?.Invoke(this, e);
        }

        private void Initialize()
        {
            nativeObject.DateChanged += (o, e) => OnDateChanged(e);

            SetParameterValueOverride(SelectedDateProperty);
            SetResourceReference(BackgroundProperty, SystemResources.DateTimePickerBackgroundBrushKey);
            SetResourceReference(BorderBrushProperty, SystemResources.DateTimePickerBorderBrushKey);
            SetResourceReference(BorderWidthProperty, SystemResources.DateTimePickerBorderWidthKey);
            SetResourceReference(FontSizeProperty, SystemResources.DateTimePickerFontSizeKey);
            SetResourceReference(FontStyleProperty, SystemResources.DateTimePickerFontStyleKey);
            SetResourceReference(ForegroundProperty, SystemResources.DateTimePickerForegroundBrushKey);
        }
    }
}
