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
    [Resolve(typeof(INativeImage))]
    public class Image : Element
    {
        #region Property Descriptors
        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:Source"/> property.
        /// </summary>
        public static PropertyDescriptor SourceProperty { get; } = PropertyDescriptor.Create(nameof(Source), typeof(ImageSource), typeof(Image));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:Stretch"/> property.
        /// </summary>
        public static PropertyDescriptor StretchProperty { get; } = PropertyDescriptor.Create(nameof(Stretch), typeof(Stretch), typeof(Image));
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
                if (oldSource is BitmapImage)
                {
                    sourceLoadedEventManager.RemoveHandler(oldSource, sourceLoadedEventHandler);
                }

                if (value is BitmapImage)
                {
                    sourceLoadedEventManager.AddHandler(value, sourceLoadedEventHandler);
                }

                nativeObject.Source = (INativeImageSource)ObjectRetriever.GetNativeObject(value);
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
        private readonly static WeakEventManager sourceLoadedEventManager = new WeakEventManager(BitmapImage.ImageLoadedEvent);

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        // this field is to avoid casting
        private readonly INativeImage nativeObject;
#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private readonly EventHandler sourceLoadedEventHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class.
        /// </summary>
        public Image()
            : this(ResolveParameter.EmptyParameters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class.
        /// </summary>
        /// <param name="sourceUri">The URI of the source file containing the image data.</param>
        public Image(Uri sourceUri)
            : this(ResolveParameter.EmptyParameters)
        {
            if (sourceUri != null)
            {
                Source = new BitmapImage(sourceUri);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class.
        /// </summary>
        /// <param name="source">The <see cref="ImageSource"/> instance containing the image data.</param>
        public Image(ImageSource source)
            : this(ResolveParameter.EmptyParameters)
        {
            Source = source;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class and pairs it with the specified native object.
        /// </summary>
        /// <param name="nativeObject">The native object with which to pair this instance.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="nativeObject"/> doesn't match the type specified by the topmost <see cref="ResolveAttribute"/> in the inheritance chain.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="nativeObject"/> is <c>null</c>.</exception>
        protected Image(INativeImage nativeObject)
            : base(nativeObject)
        {
            this.nativeObject = nativeObject;

            sourceLoadedEventHandler = OnImageSourceLoaded;
            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class and pairs it with a native object that is resolved from the IoC container.
        /// </summary>
        /// <param name="resolveParameters">Any parameters to pass along to the constructor of the native type.</param>
        /// <exception cref="TypeResolutionException">Thrown when the native object does not resolve to an <see cref="INativeImage"/> instance.</exception>
        protected Image(ResolveParameter[] resolveParameters)
            : base(resolveParameters)
        {
            nativeObject = ObjectRetriever.GetNativeObject(this) as INativeImage;
            if (nativeObject == null)
            {
                throw new TypeResolutionException(string.Format(CultureInfo.CurrentCulture, Strings.TypeMustResolveToType,
                    ObjectRetriever.GetNativeObject(this).GetType().FullName, typeof(INativeImage).FullName));
            }

            sourceLoadedEventHandler = OnImageSourceLoaded;
            Initialize();
        }

        private void Initialize()
        {
            IsHitTestVisible = false;
            Stretch = Stretch.None;
        }

        private void OnImageSourceLoaded(object sender, EventArgs args)
        {
            if (nativeObject.Source != null && (Math.Ceiling(RenderSize.Width) != Math.Ceiling(nativeObject.Source.PixelWidth / nativeObject.Source.Scale) ||
                    Math.Ceiling(RenderSize.Height) != Math.Ceiling(nativeObject.Source.PixelHeight / nativeObject.Source.Scale)))
            {
                InvalidateMeasure();
                InvalidateArrange();
            }
        }
    }
}
