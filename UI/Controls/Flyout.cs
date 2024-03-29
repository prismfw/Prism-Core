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

namespace Prism.UI.Controls
{
    /// <summary>
    /// Represents a popup that contains a UI element and is positioned relative to a specified target object.
    /// </summary>
    [Resolve(typeof(INativeFlyout))]
    public class Flyout : FlyoutBase
    {
        #region Property Descriptors
        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:Content"/> property.
        /// </summary>
        public static PropertyDescriptor ContentProperty { get; } = PropertyDescriptor.Create(nameof(Content), typeof(Element), typeof(Flyout));
        #endregion

        /// <summary>
        /// Gets or sets the element that serves as the content of the flyout.
        /// </summary>
        public Element Content
        {
            get { return (Element)ObjectRetriever.GetAgnosticObject(nativeObject.Content); }
            set { nativeObject.Content = ObjectRetriever.GetNativeObject(value); }
        }

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        // this field is to avoid casting
        private readonly INativeFlyout nativeObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="Flyout"/> class.
        /// </summary>
        public Flyout()
            : this(ResolveParameter.EmptyParameters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Flyout"/> class and pairs it with the specified native object.
        /// </summary>
        /// <param name="nativeObject">The native object with which to pair this instance.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="nativeObject"/> doesn't match the type specified by the topmost <see cref="ResolveAttribute"/> in the inheritance chain.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="nativeObject"/> is <c>null</c>.</exception>
        protected Flyout(INativeFlyout nativeObject)
            : base(nativeObject)
        {
            this.nativeObject = nativeObject;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Flyout"/> class and pairs it with a native object that is resolved from the IoC container.
        /// </summary>
        /// <param name="resolveParameters">Any parameters to pass along to the constructor of the native type.</param>
        /// <exception cref="TypeResolutionException">Thrown when the native object does not resolve to an <see cref="INativeFlyout"/> instance.</exception>
        protected Flyout(ResolveParameter[] resolveParameters)
            : base(resolveParameters)
        {
            nativeObject = ObjectRetriever.GetNativeObject(this) as INativeFlyout;
            if (nativeObject == null)
            {
                throw new TypeResolutionException(string.Format(CultureInfo.CurrentCulture, Strings.TypeMustResolveToType,
                    ObjectRetriever.GetNativeObject(this).GetType().FullName, typeof(INativeFlyout).FullName));
            }
        }

        /// <summary>
        /// Called when this instance is ready to arrange its children.
        /// </summary>
        /// <param name="frame">The final rendering frame in which this instance should arrange its children.</param>
        protected sealed override void ArrangeCore(Rectangle frame)
        {
            var content = Content;
            if (content == null)
            {
                base.ArrangeCore(new Rectangle());
            }
            else
            {
                frame = new Rectangle(new Point(), content.DesiredSize);
                if (!content.IsArrangeValid || content.RenderSize != frame.Size)
                {
                    content.Arrange(frame);
                }

                base.ArrangeCore(frame);
            }
        }

        /// <summary>
        /// Called when this instance is ready to be measured and returns the desired size of the object.
        /// </summary>
        /// <param name="constraints">The width and height that this instance should not exceed.</param>
        /// <returns>The desired size of the object as a <see cref="Size"/> instance.</returns>
        protected sealed override Size MeasureCore(Size constraints)
        {
            var content = Content;
            if (content == null)
            {
                return Size.Empty;
            }
            else
            {
                if (!content.IsMeasureValid)
                {
                    content.Measure(constraints);
                }

                return content.DesiredSize;
            }
        }
    }
}
