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
    /// Represents a UI element that allows a user to select from a range of date values.
    /// </summary>
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
            : this(typeof(INativeDatePicker), null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatePicker"/> class.
        /// </summary>
        /// <param name="resolveType">The type to pass to the IoC container in order to resolve the native object.</param>
        /// <param name="resolveName">An optional name to use when resolving the native object.</param>
        /// <param name="resolveParameters">Any parameters to pass along to the constructor of the resolve type.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="resolveType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="resolveType"/> does not resolve to an <see cref="INativeDatePicker"/> instance.</exception>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", Justification = "resolveType is validated in base constructor.")]
        protected DatePicker(Type resolveType, string resolveName, params ResolveParameter[] resolveParameters)
            : base(resolveType, resolveName, resolveParameters)
        {
            nativeObject = ObjectRetriever.GetNativeObject(this) as INativeDatePicker;
            if (nativeObject == null)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Strings.TypeMustResolveToType, resolveType.FullName, typeof(INativeDatePicker).FullName), nameof(resolveType));
            }

            nativeObject.DateChanged += (o, e) => OnDateChanged(e);

            BorderWidth = (double)Application.Current.Resources[SystemResources.DateTimePickerBorderWidthKey];
            FontSize = (double)Application.Current.Resources[SystemResources.DateTimePickerFontSizeKey];
            FontStyle = (FontStyle)Application.Current.Resources[SystemResources.DateTimePickerFontStyleKey];

            SetResourceReference(BackgroundProperty, SystemResources.DateTimePickerBackgroundBrushKey);
            SetResourceReference(BorderBrushProperty, SystemResources.DateTimePickerBorderBrushKey);
            SetResourceReference(ForegroundProperty, SystemResources.DateTimePickerForegroundBrushKey);
        }

        /// <summary>
        /// Called when the selected date is changed and raises the <see cref="DateChanged"/> event.
        /// </summary>
        /// <param name="e">The event arguments containing details about the change.</param>
        protected virtual void OnDateChanged(DateChangedEventArgs e)
        {
            DateChanged?.Invoke(this, e);
        }
    }
}
