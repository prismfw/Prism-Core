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
using System.Globalization;
using Prism.Native;
using Prism.Resources;

#if !DEBUG
using System.Diagnostics;
#endif

namespace Prism.Input
{
    /// <summary>
    /// Represents the base class for gesture recognizers.  This class is abstract.
    /// </summary>
    public abstract class GestureRecognizer : FrameworkObject
    {
        #region Property Descriptors
        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:Target"/> property.
        /// </summary>
        public static PropertyDescriptor TargetProperty { get; } = PropertyDescriptor.Create(nameof(Target), typeof(object), typeof(GestureRecognizer), true);
        #endregion

        /// <summary>
        /// Gets the current target of the gesture recognizer.
        /// </summary>
        public object Target { get; private set; }

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        // this field is to avoid casting
        private readonly INativeGestureRecognizer nativeObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="GestureRecognizer"/> class and pairs it with the specified native object.
        /// </summary>
        /// <param name="nativeObject">The native object with which to pair this instance.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="nativeObject"/> doesn't match the type specified by the topmost <see cref="ResolveAttribute"/> in the inheritance chain.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="nativeObject"/> is <c>null</c>.</exception>
        protected GestureRecognizer(INativeGestureRecognizer nativeObject)
            : base(nativeObject)
        {
            this.nativeObject = nativeObject;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GestureRecognizer"/> class and pairs it with a native object that is resolved from the IoC container.
        /// </summary>
        /// <param name="resolveParameters">Any parameters to pass along to the constructor of the native type.</param>
        /// <exception cref="TypeResolutionException">Thrown when the native object does not resolve to an <see cref="INativeGestureRecognizer"/> instance.</exception>
        protected GestureRecognizer(ResolveParameter[] resolveParameters)
            : base(resolveParameters)
        {
            nativeObject = ObjectRetriever.GetNativeObject(this) as INativeGestureRecognizer;
            if (nativeObject == null)
            {
                throw new TypeResolutionException(string.Format(CultureInfo.CurrentCulture, Strings.TypeMustResolveToType,
                    ObjectRetriever.GetNativeObject(this).GetType().FullName, typeof(INativeGestureRecognizer).FullName));
            }
        }

        internal void ClearTarget()
        {
            if (Target != null)
            {
                nativeObject.ClearTarget(ObjectRetriever.GetNativeObject(Target));

                Target = null;
                OnPropertyChanged(TargetProperty);
            }
        }

        internal void SetTarget(object target)
        {
            if (target != Target)
            {
                nativeObject.SetTarget(ObjectRetriever.GetNativeObject(target));

                Target = target;
                OnPropertyChanged(TargetProperty);
            }
        }
    }
}
