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
    /// Represents a UI element with a thumb that can be slid along a track.
    /// </summary>
    [Resolve(typeof(INativeSlider))]
    public class Slider : Control
    {
        #region Event Descriptors
        /// <summary>
        /// Gets an <see cref="EventDescriptor"/> describing the <see cref="E:ValueChanged"/> event.
        /// </summary>
        public static EventDescriptor ValueChangedEvent { get; } = EventDescriptor.Create(nameof(ValueChanged), typeof(TypedEventHandler<Slider, ValueChangedEventArgs<double>>), typeof(Slider));
        #endregion

        #region Property Descriptors
        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:IsSnapToStepEnabled"/> property.
        /// </summary>
        public static PropertyDescriptor IsSnapToStepEnabledProperty { get; } = PropertyDescriptor.Create(nameof(IsSnapToStepEnabled), typeof(bool), typeof(Slider));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:MaxValue"/> property.
        /// </summary>
        public static PropertyDescriptor MaxValueProperty { get; } = PropertyDescriptor.Create(nameof(MaxValue), typeof(double), typeof(Slider));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:MinValue"/> property.
        /// </summary>
        public static PropertyDescriptor MinValueProperty { get; } = PropertyDescriptor.Create(nameof(MinValue), typeof(double), typeof(Slider));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:StepFrequency"/> property.
        /// </summary>
        public static PropertyDescriptor StepFrequencyProperty { get; } = PropertyDescriptor.Create(nameof(StepFrequency), typeof(double), typeof(Slider));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:ThumbBrush"/> property.
        /// </summary>
        public static PropertyDescriptor ThumbBrushProperty { get; } = PropertyDescriptor.Create(nameof(ThumbBrush), typeof(Brush), typeof(Slider));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:Value"/> property.
        /// </summary>
        public static PropertyDescriptor ValueProperty { get; } = PropertyDescriptor.Create(nameof(Value), typeof(double), typeof(Slider), new PropertyMetadata(PropertyMetadataOptions.BindsTwoWayByDefault));
        #endregion

        /// <summary>
        /// Occurs when the <see cref="P:Value"/> property has changed.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification = "Event handler provides a strongly-typed sender for easier use.")]
        public event TypedEventHandler<Slider, ValueChangedEventArgs<double>> ValueChanged;

        /// <summary>
        /// Gets or sets a value indicating whether the thumb should automatically move to the closest step.
        /// </summary>
        public bool IsSnapToStepEnabled
        {
            get { return nativeObject.IsSnapToStepEnabled; }
            set { nativeObject.IsSnapToStepEnabled = value; }
        }

        /// <summary>
        /// Gets or sets the maximum value that the control is allowed to have.
        /// </summary>
        public double MaxValue
        {
            get { return nativeObject.MaxValue; }
            set { nativeObject.MaxValue = value; }
        }

        /// <summary>
        /// Gets or sets the minimum value that the control is allowed to have.
        /// </summary>
        public double MinValue
        {
            get { return nativeObject.MinValue; }
            set { nativeObject.MinValue = value; }
        }

        /// <summary>
        /// Gets or sets the interval between steps along the track.
        /// </summary>
        public double StepFrequency
        {
            get { return nativeObject.StepFrequency; }
            set { nativeObject.StepFrequency = value; }
        }

        /// <summary>
        /// Gets or sets the <see cref="Brush"/> to apply to the thumb of the control.
        /// </summary>
        public Brush ThumbBrush
        {
            get { return nativeObject.ThumbBrush; }
            set { nativeObject.ThumbBrush = value; }
        }

        /// <summary>
        /// Gets or sets the current value of the control.
        /// </summary>
        public double Value
        {
            get { return nativeObject.Value; }
            set { nativeObject.Value = value; }
        }

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        // this field is to avoid casting
        private readonly INativeSlider nativeObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="Slider"/> class.
        /// </summary>
        public Slider()
            : this(ResolveParameter.EmptyParameters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Slider"/> class and pairs it with the specified native object.
        /// </summary>
        /// <param name="nativeObject">The native object with which to pair this instance.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="nativeObject"/> doesn't match the type specified by the topmost <see cref="ResolveAttribute"/> in the inheritance chain.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="nativeObject"/> is <c>null</c>.</exception>
        protected Slider(INativeSlider nativeObject)
            : base(nativeObject)
        {
            this.nativeObject = nativeObject;

            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Slider"/> class and pairs it with a native object that is resolved from the IoC container.
        /// </summary>
        /// <param name="resolveParameters">Any parameters to pass along to the constructor of the native type.</param>
        /// <exception cref="TypeResolutionException">Thrown when the native object does not resolve to an <see cref="INativeSlider"/> instance.</exception>
        protected Slider(ResolveParameter[] resolveParameters)
            : base(resolveParameters)
        {
            nativeObject = ObjectRetriever.GetNativeObject(this) as INativeSlider;
            if (nativeObject == null)
            {
                throw new TypeResolutionException(string.Format(CultureInfo.CurrentCulture, Strings.TypeMustResolveToType,
                    ObjectRetriever.GetNativeObject(this).GetType().FullName, typeof(INativeSlider).FullName));
            }

            Initialize();
        }
        
        /// <summary>
        /// Called when the <see cref="P:Value"/> property is changed and raises the <see cref="ValueChanged"/> event.
        /// </summary>
        /// <param name="e">The event arguments containing the event details.</param>
        protected virtual void OnValueChanged(ValueChangedEventArgs<double> e)
        {
            ValueChanged?.Invoke(this, e);
        }

        private void Initialize()
        {
            nativeObject.ValueChanged += (o, e) => OnValueChanged(e);

            HorizontalAlignment = HorizontalAlignment.Stretch;
            MaxValue = 100;
            MinValue = 0;
            StepFrequency = 0;

            SetParameterValueOverride(ValueProperty);
            SetResourceReference(BackgroundProperty, SystemResources.SliderBackgroundBrushKey);
            SetResourceReference(ForegroundProperty, SystemResources.SliderForegroundBrushKey);
            SetResourceReference(ThumbBrushProperty, SystemResources.SliderThumbBrushKey);
        }
    }
}
