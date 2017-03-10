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
using Prism.UI.Media.Imaging;

#if !DEBUG
using System.Diagnostics;
#endif

namespace Prism.UI.Controls
{
    /// <summary>
    /// Represents a UI element that reacts to click or tap interactions.
    /// </summary>
    public class Button : Control
    {
        #region Event Descriptors
        /// <summary>
        /// Gets an <see cref="EventDescriptor"/> describing the <see cref="E:Clicked"/> event.
        /// </summary>
        public static EventDescriptor ClickedEvent { get; } = EventDescriptor.Create(nameof(Clicked), typeof(TypedEventHandler<Button>), typeof(Button));
        #endregion

        #region Property Descriptors
        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:ContentDirection"/> property.
        /// </summary>
        public static PropertyDescriptor ContentDirectionProperty { get; } = PropertyDescriptor.Create(nameof(ContentDirection), typeof(ContentDirection), typeof(Button), new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:Image"/> property.
        /// </summary>
        public static PropertyDescriptor ImageProperty { get; } = PropertyDescriptor.Create(nameof(Image), typeof(ImageSource), typeof(Button), new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:Padding"/> property.
        /// </summary>
        public static PropertyDescriptor PaddingProperty { get; } = PropertyDescriptor.Create(nameof(Padding), typeof(Thickness), typeof(Button), new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:Title"/> property.
        /// </summary>
        public static PropertyDescriptor TitleProperty { get; } = PropertyDescriptor.Create(nameof(Title), typeof(string), typeof(Button), new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.AffectsMeasure));
        #endregion

        /// <summary>
        /// Occurs when the button is clicked or tapped.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification = "Event handler provides a strongly-typed sender for easier use.")]
        public event TypedEventHandler<Button> Clicked;

        /// <summary>
        /// Gets or sets the direction in which the button image should be placed in relation to the button title.
        /// </summary>
        public ContentDirection ContentDirection
        {
            get { return nativeObject.ContentDirection; }
            set { nativeObject.ContentDirection = value; }
        }

        /// <summary>
        /// Gets or sets an image to display within the button.
        /// </summary>
        public ImageSource Image
        {
            get { return (ImageSource)ObjectRetriever.GetAgnosticObject(nativeObject.Image); }
            set { nativeObject.Image = (INativeImageSource)ObjectRetriever.GetNativeObject(value); }
        }

        /// <summary>
        /// Gets or sets the inner padding of the element.
        /// </summary>
        public Thickness Padding
        {
            get { return nativeObject.Padding; }
            set { nativeObject.Padding = value; }
        }

        /// <summary>
        /// Gets or sets the title of the button.
        /// </summary>
        public string Title
        {
            get { return nativeObject.Title; }
            set { nativeObject.Title = value; }
        }

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        // this field is to avoid casting
        private readonly INativeButton nativeObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="Button"/> class.
        /// </summary>
        public Button()
            : this(typeof(INativeButton), null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Button"/> class.
        /// </summary>
        /// <param name="resolveType">The type to pass to the IoC container in order to resolve the native object.</param>
        /// <param name="resolveName">An optional name to use when resolving the native object.</param>
        /// <param name="resolveParameters">Any parameters to pass along to the constructor of the resolve type.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="resolveType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="resolveType"/> does not resolve to an <see cref="INativeButton"/> instance.</exception>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", Justification = "resolveType is validated in base constructor.")]
        protected Button(Type resolveType, string resolveName, params ResolveParameter[] resolveParameters)
            : base(resolveType, resolveName, resolveParameters)
        {
            nativeObject = ObjectRetriever.GetNativeObject(this) as INativeButton;
            if (nativeObject == null)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Strings.TypeMustResolveToType, resolveType.FullName, typeof(INativeButton).FullName), nameof(resolveType));
            }

            nativeObject.Clicked += (o, e) => OnClicked(e);

            BorderWidth = (double)Application.Current.Resources[SystemResources.ButtonBorderWidthKey];
            Padding = (Thickness)Application.Current.Resources[SystemResources.ButtonPaddingKey];
            FontSize = (double)Application.Current.Resources[SystemResources.ButtonFontSizeKey];
            FontStyle = (FontStyle)Application.Current.Resources[SystemResources.ButtonFontStyleKey];

            SetResourceReference(BackgroundProperty, SystemResources.ButtonBackgroundBrushKey);
            SetResourceReference(BorderBrushProperty, SystemResources.ButtonBorderBrushKey);
            SetResourceReference(ForegroundProperty, SystemResources.ButtonForegroundBrushKey);
        }

        /// <summary>
        /// Called when the button is clicked or tapped and raises the <see cref="Clicked"/> event.
        /// </summary>
        /// <param name="e">The event arguments for the event.</param>
        protected virtual void OnClicked(EventArgs e)
        {
            Clicked?.Invoke(this, e);
        }
    }
}
