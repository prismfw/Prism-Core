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

namespace Prism.Input
{
    /// <summary>
    /// Represents a gesture recognizer that listens for holding (long press) gestures.
    /// </summary>
    [Resolve(typeof(INativeHoldingGestureRecognizer))]
    public class HoldingGestureRecognizer : GestureRecognizer
    {
        #region Event Descriptors
        /// <summary>
        /// Gets an <see cref="EventDescriptor"/> describing the <see cref="E:Holding"/> event.
        /// </summary>
        public static EventDescriptor HoldingEvent { get; } = EventDescriptor.Create(nameof(Holding), typeof(TypedEventHandler<HoldingGestureRecognizer, HoldingEventArgs>), typeof(HoldingGestureRecognizer));
        #endregion

        /// <summary>
        /// Occurs when a holding gesture is started, completed, or canceled.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification = "Event handler provides a strongly-typed sender for easier use.")]
        public event TypedEventHandler<HoldingGestureRecognizer, HoldingEventArgs> Holding;

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        // this field is to avoid casting
        private readonly INativeHoldingGestureRecognizer nativeObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="HoldingGestureRecognizer"/> class.
        /// </summary>
        public HoldingGestureRecognizer()
            : this(ResolveParameter.EmptyParameters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HoldingGestureRecognizer"/> class and pairs it with the specified native object.
        /// </summary>
        /// <param name="nativeObject">The native object with which to pair this instance.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="nativeObject"/> doesn't match the type specified by the topmost <see cref="ResolveAttribute"/> in the inheritance chain.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="nativeObject"/> is <c>null</c>.</exception>
        protected HoldingGestureRecognizer(INativeHoldingGestureRecognizer nativeObject)
            : base(nativeObject)
        {
            this.nativeObject = nativeObject;

            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HoldingGestureRecognizer"/> class and pairs it with a native object that is resolved from the IoC container.
        /// </summary>
        /// <param name="resolveParameters">Any parameters to pass along to the constructor of the native type.</param>
        /// <exception cref="TypeResolutionException">Thrown when the native object does not resolve to an <see cref="INativeHoldingGestureRecognizer"/> instance.</exception>
        protected HoldingGestureRecognizer(ResolveParameter[] resolveParameters)
            : base(resolveParameters)
        {
            nativeObject = ObjectRetriever.GetNativeObject(this) as INativeHoldingGestureRecognizer;
            if (nativeObject == null)
            {
                throw new TypeResolutionException(string.Format(CultureInfo.CurrentCulture, Strings.TypeMustResolveToType,
                    ObjectRetriever.GetNativeObject(this).GetType().FullName, typeof(INativeHoldingGestureRecognizer).FullName));
            }

            Initialize();
        }

        /// <summary>
        /// Called when a holding gesture is started, completed, or canceled and raises the <see cref="Holding"/> event.
        /// </summary>
        /// <param name="e">The event arguments containing details about the gesture.</param>
        protected virtual void OnHolding(HoldingEventArgs e)
        {
            Holding?.Invoke(this, e);
        }

        private void Initialize()
        {
            nativeObject.Holding += (o, e) => OnHolding(e);
        }
    }
}
