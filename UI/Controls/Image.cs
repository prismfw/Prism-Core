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
using Prism.UI.Media.Imaging;
using Prism.Utilities;

#if !DEBUG
using System.Diagnostics;
#endif

namespace Prism.UI.Controls
{
    /// <summary>
    /// Represents a UI element that displays an image.
    /// </summary>
    public class Image : Element
    {
        #region Property Descriptors
        /// <summary>
        /// Describes the <see cref="P:Source"/> property.  This field is read-only.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "PropertyDescriptor is immutable.")]
        public static readonly PropertyDescriptor SourceProperty = PropertyDescriptor.Create(nameof(Source), typeof(ImageSource), typeof(Image));

        /// <summary>
        /// Describes the <see cref="P:Stretch"/> property.  This field is read-only.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "PropertyDescriptor is immutable.")]
        public static readonly PropertyDescriptor StretchProperty = PropertyDescriptor.Create(nameof(Stretch), typeof(Stretch), typeof(Image));
        #endregion

        /// <summary>
        /// Gets or sets the <see cref="ImageSource"/> object that contains the image data for the element.
        /// </summary>
        public ImageSource Source
        {
            get { return (ImageSource)ObjectRetriever.GetAgnosticObject(nativeObject.Source); }
            set
            {
                var oldSource = Source;
                if (oldSource != null)
                {
                    sourceLoadedEventManager.RemoveHandler(oldSource, sourceLoadedEventHandler);
                }

                nativeObject.Source = (INativeImageSource)ObjectRetriever.GetNativeObject(value);

                if (value != null)
                {
                    sourceLoadedEventManager.AddHandler(value, sourceLoadedEventHandler);
                }
            }
        }

        /// <summary>
        /// Gets or sets the manner in which the image will be stretched to fit its allocated space.
        /// </summary>
        public Stretch Stretch
        {
            get { return nativeObject.Stretch; }
            set { nativeObject.Stretch = value; }
        }

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private static WeakEventManager sourceLoadedEventManager = new WeakEventManager(ImageSource.ImageLoadedEvent);

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        // this field is to avoid casting
        private readonly INativeImage nativeObject;
#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private EventHandler sourceLoadedEventHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class.
        /// </summary>
        public Image()
            : this(typeof(INativeImage), null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class.
        /// </summary>
        /// <param name="sourceUri">The URI of the source file containing the image data.</param>
        public Image(Uri sourceUri)
            : this(typeof(INativeImage), null)
        {
            if (sourceUri != null)
            {
                Source = new ImageSource(sourceUri);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class.
        /// </summary>
        /// <param name="source">The <see cref="ImageSource"/> instance containing the image data.</param>
        public Image(ImageSource source)
            : this(typeof(INativeImage), null)
        {
            Source = source;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class.
        /// </summary>
        /// <param name="resolveType">The type to pass to the IoC container in order to resolve the native object.</param>
        /// <param name="resolveName">An optional name to use when resolving the native object.</param>
        /// <param name="resolveParameters">Any parameters to pass along to the constructor of the resolve type.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="resolveType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="resolveType"/> does not resolve to an <see cref="INativeImage"/> instance.</exception>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", Justification = "resolveType is validated in base constructor.")]
        protected Image(Type resolveType, string resolveName, params ResolveParameter[] resolveParameters)
            : base(resolveType, resolveName, resolveParameters)
        {
            nativeObject = ObjectRetriever.GetNativeObject(this) as INativeImage;
            if (nativeObject == null)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.Strings.TypeMustResolveToType, resolveType.FullName, typeof(INativeImage).FullName), nameof(resolveType));
            }

            sourceLoadedEventHandler = (o, e) =>
            {
                if (nativeObject.Source != null && (RenderSize.Width != nativeObject.Source.PixelWidth || RenderSize.Height != nativeObject.Source.PixelHeight))
                {
                    InvalidateMeasure();
                    (Parent as Visual)?.InvalidateArrange();
                }
            };

            IsHitTestVisible = false;
            Stretch = Stretch.None;
        }
    }
}