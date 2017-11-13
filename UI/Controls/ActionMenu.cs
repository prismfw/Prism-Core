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
    /// Represents a menu control for a <see cref="ContentView"/> that provides the user with a set of selectable actions.
    /// </summary>
    [Resolve(typeof(INativeActionMenu))]
    public class ActionMenu : Visual
    {
        #region Property Descriptors
        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:Background"/> property.
        /// </summary>
        public static PropertyDescriptor BackgroundProperty { get; } = PropertyDescriptor.Create(nameof(Background), typeof(Brush), typeof(ActionMenu));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:CancelButtonTitle"/> property.
        /// </summary>
        public static PropertyDescriptor CancelButtonTitleProperty { get; } = PropertyDescriptor.Create(nameof(CancelButtonTitle), typeof(string), typeof(ActionMenu));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:Foreground"/> property.
        /// </summary>
        public static PropertyDescriptor ForegroundProperty { get; } = PropertyDescriptor.Create(nameof(Foreground), typeof(Brush), typeof(ActionMenu));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:MaxDisplayItems"/> property.
        /// </summary>
        public static PropertyDescriptor MaxDisplayItemsProperty { get; } = PropertyDescriptor.Create(nameof(MaxDisplayItems), typeof(int), typeof(ActionMenu), new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:OverflowImageUri"/> property.
        /// </summary>
        public static PropertyDescriptor OverflowImageUriProperty { get; } = PropertyDescriptor.Create(nameof(OverflowImageUri), typeof(Uri), typeof(ActionMenu), new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.AffectsMeasure));
        #endregion

        /// <summary>
        /// Gets or sets the background for the menu.
        /// </summary>
        public Brush Background
        {
            get { return nativeObject.Background; }
            set { nativeObject.Background = value; }
        }

        /// <summary>
        /// Gets or sets the title of the menu's Cancel button, if one exists.
        /// </summary>
        public string CancelButtonTitle
        {
            get { return nativeObject.CancelButtonTitle; }
            set { nativeObject.CancelButtonTitle = value; }
        }

        /// <summary>
        /// Gets or sets the <see cref="Brush"/> to apply to the foreground content of the menu.
        /// </summary>
        public Brush Foreground
        {
            get { return nativeObject.Foreground; }
            set { nativeObject.Foreground = value; }
        }

        /// <summary>
        /// Gets a collection of the items within the menu.
        /// </summary>
        public MenuItemCollection Items { get; }

        /// <summary>
        /// Gets or sets the maximum number of menu items that can be displayed before they are placed into an overflow menu.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", Justification = "Exception parameter refers to property name for easier understanding of invalid value.")]
        public int MaxDisplayItems
        {
            get { return nativeObject.MaxDisplayItems; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(MaxDisplayItems), Strings.ValueCannotBeLessThanZero);
                }

                nativeObject.MaxDisplayItems = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="Uri"/> of the image to use for representing the overflow menu when one is present.
        /// </summary>
        public Uri OverflowImageUri
        {
            get { return nativeObject.OverflowImageUri; }
            set { nativeObject.OverflowImageUri = IO.Directory.ValidateUri(value); }
        }

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private readonly INativeActionMenu nativeObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionMenu"/> class.
        /// </summary>
        public ActionMenu()
            : this(ResolveParameter.EmptyParameters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionMenu"/> class and pairs it with the specified native object.
        /// </summary>
        /// <param name="nativeObject">The native object with which to pair this instance.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="nativeObject"/> doesn't match the type specified by the topmost <see cref="ResolveAttribute"/> in the inheritance chain.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="nativeObject"/> is <c>null</c>.</exception>
        protected ActionMenu(INativeActionMenu nativeObject)
            : base(nativeObject)
        {
            this.nativeObject = nativeObject;

            Items = new MenuItemCollection(nativeObject);
            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionMenu"/> class and pairs it with a native object that is resolved from the IoC container.
        /// </summary>
        /// <param name="resolveParameters">Any parameters to pass along to the constructor of the native type.</param>
        /// <exception cref="TypeResolutionException">Thrown when the native object does not resolve to an <see cref="INativeActionMenu"/> instance.</exception>
        protected ActionMenu(ResolveParameter[] resolveParameters)
            : base(resolveParameters)
        {
            nativeObject = ObjectRetriever.GetNativeObject(this) as INativeActionMenu;
            if (nativeObject == null)
            {
                throw new TypeResolutionException(string.Format(CultureInfo.CurrentCulture, Strings.TypeMustResolveToType,
                    ObjectRetriever.GetNativeObject(this).GetType().FullName, typeof(INativeActionMenu).FullName));
            }

            Items = new MenuItemCollection(nativeObject);
            Initialize();
        }

        /// <summary>
        /// Called when this instance is ready to arrange its children.
        /// </summary>
        /// <param name="frame">The final rendering frame in which this instance should arrange its children.</param>
        protected sealed override void ArrangeCore(Rectangle frame)
        {
            nativeObject.Frame = new Rectangle(frame.TopLeft, DesiredSize);
        }

        /// <summary>
        /// Called when this instance is ready to be measured and returns the desired size of the object.
        /// </summary>
        /// <param name="constraints">The width and height that this instance should not exceed.</param>
        /// <returns>The desired size of the object as a <see cref="Size"/> instance.</returns>
        protected sealed override Size MeasureCore(Size constraints)
        {
            return base.MeasureCore(constraints);
        }

        private void Initialize()
        {
            CancelButtonTitle = "Cancel";

            SetResourceReference(BackgroundProperty, SystemResources.ActionMenuBackgroundBrushKey);
            SetResourceReference(ForegroundProperty, SystemResources.ActionMenuForegroundBrushKey);
            SetResourceReference(MaxDisplayItemsProperty, SystemResources.ActionMenuMaxDisplayItemsKey);
        }
    }
}
