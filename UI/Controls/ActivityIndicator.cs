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
    /// Represents a UI element that indicates to the user an activity is being performed that will take an indeterminate amount of time.
    /// </summary>
    [Resolve(typeof(INativeActivityIndicator))]
    public class ActivityIndicator : Element
    {
        #region Property Descriptors
        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:Foreground"/> property.
        /// </summary>
        public static PropertyDescriptor ForegroundProperty { get; } = PropertyDescriptor.Create(nameof(Foreground), typeof(Brush), typeof(ActivityIndicator));
        #endregion

        /// <summary>
        /// Gets or sets the <see cref="Brush"/> to apply to the foreground content of the indicator.
        /// </summary>
        public Brush Foreground
        {
            get { return nativeObject.Foreground; }
            set { nativeObject.Foreground = value; }
        }

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private readonly INativeActivityIndicator nativeObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityIndicator"/> class.
        /// </summary>
        public ActivityIndicator()
            : this(ResolveParameter.EmptyParameters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityIndicator"/> class and pairs it with the specified native object.
        /// </summary>
        /// <param name="nativeObject">The native object with which to pair this instance.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="nativeObject"/> doesn't match the type specified by the topmost <see cref="ResolveAttribute"/> in the inheritance chain.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="nativeObject"/> is <c>null</c>.</exception>
        protected ActivityIndicator(INativeActivityIndicator nativeObject)
            : base(nativeObject)
        {
            this.nativeObject = nativeObject;

            SetResourceReference(ForegroundProperty, SystemResources.ActivityIndicatorForegroundBrushKey);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityIndicator"/> class and pairs it with a native object that is resolved from the IoC container.
        /// </summary>
        /// <param name="resolveParameters">Any parameters to pass along to the constructor of the native type.</param>
        /// <exception cref="TypeResolutionException">Thrown when the native object does not resolve to an <see cref="INativeActivityIndicator"/> instance.</exception>
        protected ActivityIndicator(ResolveParameter[] resolveParameters)
            : base(resolveParameters)
        {
            nativeObject = ObjectRetriever.GetNativeObject(this) as INativeActivityIndicator;
            if (nativeObject == null)
            {
                throw new TypeResolutionException(string.Format(CultureInfo.CurrentCulture, Strings.TypeMustResolveToType,
                    ObjectRetriever.GetNativeObject(this).GetType().FullName, typeof(INativeActivityIndicator).FullName));
            }

            SetResourceReference(ForegroundProperty, SystemResources.ActivityIndicatorForegroundBrushKey);
        }
    }
}
