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

#if !DEBUG
using System.Diagnostics;
#endif

namespace Prism.UI.Controls
{
    /// <summary>
    /// Represents the method that is invoked when a menu button is pressed.
    /// </summary>
    /// <param name="button">The button that was pressed.</param>
    public delegate void MenuButtonPressedHandler(MenuButton button);

    /// <summary>
    /// Represents a pressable button within an <see cref="ActionMenu"/>.
    /// </summary>
    [Resolve(typeof(INativeMenuButton))]
    public class MenuButton : MenuItem
    {
        #region Property Descriptors
        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:ImageUri"/> property.
        /// </summary>
        public static PropertyDescriptor ImageUriProperty { get; } = PropertyDescriptor.Create(nameof(ImageUri), typeof(Uri), typeof(MenuButton));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:IsEnabled"/> property.
        /// </summary>
        public static PropertyDescriptor IsEnabledProperty { get; } = PropertyDescriptor.Create(nameof(IsEnabled), typeof(bool), typeof(MenuButton));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:Title"/> property.
        /// </summary>
        public static PropertyDescriptor TitleProperty { get; } = PropertyDescriptor.Create(nameof(Title), typeof(string), typeof(MenuButton));
        #endregion

        /// <summary>
        /// Gets or sets the action to perform when the button is pressed by the user.
        /// </summary>
        public MenuButtonPressedHandler Action { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Uri"/> of the image to display within the button.
        /// </summary>
        public Uri ImageUri
        {
            get { return nativeObject.ImageUri; }
            set { nativeObject.ImageUri = IO.Directory.ValidateUri(value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the button is enabled and should respond to user interaction.
        /// </summary>
        public bool IsEnabled
        {
            get { return nativeObject.IsEnabled; }
            set { nativeObject.IsEnabled = value; }
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
        private readonly INativeMenuButton nativeObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="MenuButton"/> class.
        /// </summary>
        public MenuButton()
            : this(ResolveParameter.EmptyParameters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MenuButton"/> class and pairs it with the specified native object.
        /// </summary>
        /// <param name="nativeObject">The native object with which to pair this instance.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="nativeObject"/> doesn't match the type specified by the topmost <see cref="ResolveAttribute"/> in the inheritance chain.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="nativeObject"/> is <c>null</c>.</exception>
        protected MenuButton(INativeMenuButton nativeObject)
            : base(nativeObject)
        {
            this.nativeObject = nativeObject;

            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MenuButton"/> class and pairs it with a native object that is resolved from the IoC container.
        /// </summary>
        /// <param name="resolveParameters">Any parameters to pass along to the constructor of the native type.</param>
        /// <exception cref="TypeResolutionException">Thrown when the native object does not resolve to an <see cref="INativeMenuButton"/> instance.</exception>
        protected MenuButton(ResolveParameter[] resolveParameters)
            : base(resolveParameters)
        {
            nativeObject = ObjectRetriever.GetNativeObject(this) as INativeMenuButton;
            if (nativeObject == null)
            {
                throw new TypeResolutionException(string.Format(CultureInfo.CurrentCulture, Resources.Strings.TypeMustResolveToType,
                    ObjectRetriever.GetNativeObject(this).GetType().FullName, typeof(INativeMenuButton).FullName));
            }

            Initialize();
        }

        private void Initialize()
        {
            nativeObject.Action = () => Action?.Invoke(this);
        }
    }
}
