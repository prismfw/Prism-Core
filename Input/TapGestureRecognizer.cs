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

namespace Prism.Input
{
    /// <summary>
    /// Represents a gesture recognizer that listens for various tap gestures.
    /// </summary>
    [Resolve(typeof(INativeTapGestureRecognizer))]
    public class TapGestureRecognizer : GestureRecognizer
    {
        #region Event Descriptors
        /// <summary>
        /// Gets an <see cref="EventDescriptor"/> describing the <see cref="E:DoubleTapped"/> event.
        /// </summary>
        public static EventDescriptor DoubleTappedEvent { get; } = EventDescriptor.Create(nameof(DoubleTapped), typeof(TypedEventHandler<TapGestureRecognizer, TappedEventArgs>), typeof(TapGestureRecognizer));

        /// <summary>
        /// Gets an <see cref="EventDescriptor"/> describing the <see cref="E:RightTapped"/> event.
        /// </summary>
        public static EventDescriptor RightTappedEvent { get; } = EventDescriptor.Create(nameof(RightTapped), typeof(TypedEventHandler<TapGestureRecognizer, TappedEventArgs>), typeof(TapGestureRecognizer));

        /// <summary>
        /// Gets an <see cref="EventDescriptor"/> describing the <see cref="E:Tapped"/> event.
        /// </summary>
        public static EventDescriptor TappedEvent { get; } = EventDescriptor.Create(nameof(Tapped), typeof(TypedEventHandler<TapGestureRecognizer, TappedEventArgs>), typeof(TapGestureRecognizer));
        #endregion

        #region Property Descriptors
        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:IsDoubleTapEnabled"/> property.
        /// </summary>
        public static PropertyDescriptor IsDoubleTapEnabledProperty { get; } = PropertyDescriptor.Create(nameof(IsDoubleTapEnabled), typeof(bool), typeof(TapGestureRecognizer));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:IsRightTapEnabled"/> property.
        /// </summary>
        public static PropertyDescriptor IsRightTapEnabledProperty { get; } = PropertyDescriptor.Create(nameof(IsRightTapEnabled), typeof(bool), typeof(TapGestureRecognizer));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:IsTapEnabled"/> property.
        /// </summary>
        public static PropertyDescriptor IsTapEnabledProperty { get; } = PropertyDescriptor.Create(nameof(IsTapEnabled), typeof(bool), typeof(TapGestureRecognizer));
        #endregion

        /// <summary>
        /// Occurs when a double tap gesture is recognized.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification = "Event handler provides a strongly-typed sender for easier use.")]
        public event TypedEventHandler<TapGestureRecognizer, TappedEventArgs> DoubleTapped;

        /// <summary>
        /// Occurs when a right tap gesture (or long press gesture for touch input) is recognized.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification = "Event handler provides a strongly-typed sender for easier use.")]
        public event TypedEventHandler<TapGestureRecognizer, TappedEventArgs> RightTapped;

        /// <summary>
        /// Occurs when a single tap gesture is recognized.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification = "Event handler provides a strongly-typed sender for easier use.")]
        public event TypedEventHandler<TapGestureRecognizer, TappedEventArgs> Tapped;

        /// <summary>
        /// Gets or sets a value indicating whether double tap gestures should be recognized.
        /// </summary>
        public bool IsDoubleTapEnabled
        {
            get { return nativeObject.IsDoubleTapEnabled; }
            set { nativeObject.IsDoubleTapEnabled = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether right tap gestures (long press gestures for touch input) should be recognized.
        /// </summary>
        public bool IsRightTapEnabled
        {
            get { return nativeObject.IsRightTapEnabled; }
            set { nativeObject.IsRightTapEnabled = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether single tap gestures should be recognized.
        /// </summary>
        public bool IsTapEnabled
        {
            get { return nativeObject.IsTapEnabled; }
            set { nativeObject.IsTapEnabled = value; }
        }

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        // this field is to avoid casting
        private readonly INativeTapGestureRecognizer nativeObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="TapGestureRecognizer"/> class.
        /// </summary>
        public TapGestureRecognizer()
            : this(ResolveParameter.EmptyParameters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TapGestureRecognizer"/> class and pairs it with the specified native object.
        /// </summary>
        /// <param name="nativeObject">The native object with which to pair this instance.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="nativeObject"/> doesn't match the type specified by the topmost <see cref="ResolveAttribute"/> in the inheritance chain.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="nativeObject"/> is <c>null</c>.</exception>
        protected TapGestureRecognizer(INativeTapGestureRecognizer nativeObject)
            : base(nativeObject)
        {
            this.nativeObject = nativeObject;

            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TapGestureRecognizer"/> class and pairs it with a native object that is resolved from the IoC container.
        /// </summary>
        /// <param name="resolveParameters">Any parameters to pass along to the constructor of the native type.</param>
        /// <exception cref="TypeResolutionException">Thrown when the native object does not resolve to an <see cref="INativeTapGestureRecognizer"/> instance.</exception>
        protected TapGestureRecognizer(ResolveParameter[] resolveParameters)
            : base(resolveParameters)
        {
            nativeObject = ObjectRetriever.GetNativeObject(this) as INativeTapGestureRecognizer;
            if (nativeObject == null)
            {
                throw new TypeResolutionException(string.Format(CultureInfo.CurrentCulture, Strings.TypeMustResolveToType,
                    ObjectRetriever.GetNativeObject(this).GetType().FullName, typeof(INativeTapGestureRecognizer).FullName));
            }

            Initialize();
        }

        /// <summary>
        /// Called when a double tap gesture is recognized and raises the <see cref="DoubleTapped"/> event.
        /// </summary>
        /// <param name="e">The event arguments containing details about the gesture.</param>
        protected virtual void OnDoubleTapped(TappedEventArgs e)
        {
            DoubleTapped?.Invoke(this, e);
        }

        /// <summary>
        /// Called when a right tap gesture (or long press gesture for touch input) is recognized and raises the <see cref="RightTapped"/> event.
        /// </summary>
        /// <param name="e">The event arguments containing details about the gesture.</param>
        protected virtual void OnRightTapped(TappedEventArgs e)
        {
            RightTapped?.Invoke(this, e);
        }

        /// <summary>
        /// Called when a single tap gesture is recognized and raises the <see cref="Tapped"/> event.
        /// </summary>
        /// <param name="e">The event arguments containing details about the gesture.</param>
        protected virtual void OnTapped(TappedEventArgs e)
        {
            Tapped?.Invoke(this, e);
        }

        private void Initialize()
        {
            nativeObject.DoubleTapped += (o, e) => OnDoubleTapped(e);
            nativeObject.RightTapped += (o, e) => OnRightTapped(e);
            nativeObject.Tapped += (o, e) => OnTapped(e);

            nativeObject.IsDoubleTapEnabled = false;
            nativeObject.IsRightTapEnabled = false;
            nativeObject.IsTapEnabled = true;
        }
    }
}
