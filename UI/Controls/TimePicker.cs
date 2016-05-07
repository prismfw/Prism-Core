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
using Prism.UI.Media;

#if !DEBUG
using System.Diagnostics;
#endif

namespace Prism.UI.Controls
{
    /// <summary>
    /// Represents a UI element that allows a user to select from a range of time values.
    /// </summary>
    public class TimePicker : Control
    {
        #region Event Descriptors
        /// <summary>
        /// Describes the <see cref="E:TimeChanged"/> event.  This field is read-only.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "EventDescriptor is immutable.")]
        public static readonly EventDescriptor TimeChangedEvent = EventDescriptor.Create(nameof(TimeChanged), typeof(TypedEventHandler<TimePicker, TimeChangedEventArgs>), typeof(TimePicker));
        #endregion

        #region Property Descriptors
        /// <summary>
        /// Describes the <see cref="P:IsOpen"/> property.  This field is read-only.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "PropertyDescriptor is immutable.")]
        public static readonly PropertyDescriptor IsOpenProperty = PropertyDescriptor.Create(nameof(IsOpen), typeof(bool), typeof(TimePicker));

        /// <summary>
        /// Describes the <see cref="P:SelectedTime"/> property.  This field is read-only.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "PropertyDescriptor is immutable.")]
        public static readonly PropertyDescriptor SelectedTimeProperty = PropertyDescriptor.Create(nameof(SelectedTime), typeof(TimeSpan?), typeof(TimePicker), new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// Describes the <see cref="P:TimeStringFormat"/> property.  This field is read-only.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "PropertyDescriptor is immutable.")]
        public static readonly PropertyDescriptor TimeStringFormatProperty = PropertyDescriptor.Create(nameof(TimeStringFormat), typeof(string), typeof(TimePicker));
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
            : this(typeof(INativeTimePicker), null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimePicker"/> class.
        /// </summary>
        /// <param name="resolveType">The type to pass to the IoC container in order to resolve the native object.</param>
        /// <param name="resolveName">An optional name to use when resolving the native object.</param>
        /// <param name="resolveParameters">Any parameters to pass along to the constructor of the resolve type.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="resolveType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="resolveType"/> does not resolve to an <see cref="INativeTimePicker"/> instance.</exception>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", Justification = "resolveType is validated in base constructor.")]
        protected TimePicker(Type resolveType, string resolveName, params ResolveParameter[] resolveParameters)
            : base(resolveType, resolveName, resolveParameters)
        {
            nativeObject = ObjectRetriever.GetNativeObject(this) as INativeTimePicker;
            if (nativeObject == null)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.Strings.TypeMustResolveToType, resolveType.FullName, typeof(INativeTimePicker).FullName), nameof(resolveType));
            }

            nativeObject.TimeChanged += (o, e) => { OnTimeChanged(e); };

            BorderWidth = SystemParameters.TimePickerBorderWidth;
            FontSize = Fonts.TimePickerFontSize;
            FontStyle = Fonts.TimePickerFontStyle;
        }

        /// <summary>
        /// Called when the selected time is changed and raises the <see cref="TimeChanged"/> event.
        /// </summary>
        /// <param name="e">The event arguments containing details about the change.</param>
        protected virtual void OnTimeChanged(TimeChangedEventArgs e)
        {
            TimeChanged?.Invoke(this, e);
        }
    }
}
