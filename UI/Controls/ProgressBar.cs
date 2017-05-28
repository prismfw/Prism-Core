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
    /// Represents a UI element that indicates the progress of an operation.
    /// </summary>
    [Resolve(typeof(INativeProgressBar))]
    public class ProgressBar : Element
    {
        #region Property Descriptors
        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:Background"/> property.
        /// </summary>
        public static PropertyDescriptor BackgroundProperty { get; } = PropertyDescriptor.Create(nameof(Background), typeof(Brush), typeof(ProgressBar));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:Foreground"/> property.
        /// </summary>
        public static PropertyDescriptor ForegroundProperty { get; } = PropertyDescriptor.Create(nameof(Foreground), typeof(Brush), typeof(ProgressBar));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:Progress"/> property.
        /// </summary>
        public static PropertyDescriptor ProgressProperty { get; } = PropertyDescriptor.Create(nameof(Progress), typeof(double), typeof(ProgressBar));
        #endregion

        /// <summary>
        /// Gets or sets the background of the progress bar.
        /// </summary>
        public Brush Background
        {
            get { return nativeObject.Background; }
            set { nativeObject.Background = value; }
        }

        /// <summary>
        /// Gets or sets the <see cref="Brush"/> to apply to the foreground content of the progress bar.
        /// </summary>
        public Brush Foreground
        {
            get { return nativeObject.Foreground; }
            set { nativeObject.Foreground = value; }
        }

        /// <summary>
        /// Gets or sets the level of progress as a value between 0.0 and 1.0.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", Justification = "Exception parameter refers to property name for easier understanding of invalid value.")]
        public double Progress
        {
            get { return nativeObject.Progress; }
            set
            {
                if (double.IsNaN(value) || double.IsInfinity(value))
                {
                    throw new ArgumentException(Strings.ValueCannotBeNaNOrInfinity, nameof(Progress));
                }

                nativeObject.Progress = Math.Max(0, Math.Min(1, value));
            }
        }

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private readonly INativeProgressBar nativeObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgressBar"/> class.
        /// </summary>
        public ProgressBar()
            : this(ResolveParameter.EmptyParameters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgressBar"/> class and pairs it with the specified native object.
        /// </summary>
        /// <param name="nativeObject">The native object with which to pair this instance.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="nativeObject"/> doesn't match the type specified by the topmost <see cref="ResolveAttribute"/> in the inheritance chain.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="nativeObject"/> is <c>null</c>.</exception>
        protected ProgressBar(INativeProgressBar nativeObject)
            : base(nativeObject)
        {
            this.nativeObject = nativeObject;

            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgressBar"/> class and pairs it with a native object that is resolved from the IoC container.
        /// </summary>
        /// <param name="resolveParameters">Any parameters to pass along to the constructor of the native type.</param>
        /// <exception cref="TypeResolutionException">Thrown when the native object does not resolve to an <see cref="INativeProgressBar"/> instance.</exception>
        protected ProgressBar(ResolveParameter[] resolveParameters)
            : base(resolveParameters)
        {
            nativeObject = ObjectRetriever.GetNativeObject(this) as INativeProgressBar;
            if (nativeObject == null)
            {
                throw new TypeResolutionException(string.Format(CultureInfo.CurrentCulture, Strings.TypeMustResolveToType,
                    ObjectRetriever.GetNativeObject(this).GetType().FullName, typeof(INativeProgressBar).FullName));
            }

            Initialize();
        }

        private void Initialize()
        {
            SetResourceReference(BackgroundProperty, SystemResources.ProgressBarBackgroundBrushKey);
            SetResourceReference(ForegroundProperty, SystemResources.ProgressBarForegroundBrushKey);
        }
    }
}
