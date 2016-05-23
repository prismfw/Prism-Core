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
using Prism.UI.Media;

namespace Prism.UI.Controls
{
    /// <summary>
    /// Represents a menu control for a <see cref="ContentView"/> that provides the user with a set of selectable actions.
    /// </summary>
    public class ActionMenu : FrameworkObject
    {
        #region Property Descriptors
        /// <summary>
        /// Describes the <see cref="P:Background"/> property.  This field is read-only.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "PropertyDescriptor is immutable.")]
        public static readonly PropertyDescriptor BackgroundProperty = PropertyDescriptor.Create(nameof(Background), typeof(Brush), typeof(ActionMenu));

        /// <summary>
        /// Describes the <see cref="P:CancelButtonTitle"/> property.  This field is read-only.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "PropertyDescriptor is immutable.")]
        public static readonly PropertyDescriptor CancelButtonTitleProperty = PropertyDescriptor.Create(nameof(CancelButtonTitle), typeof(string), typeof(ActionMenu));

        /// <summary>
        /// Describes the <see cref="P:Foreground"/> property.  This field is read-only.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "PropertyDescriptor is immutable.")]
        public static readonly PropertyDescriptor ForegroundProperty = PropertyDescriptor.Create(nameof(Foreground), typeof(Brush), typeof(ActionMenu));

        /// <summary>
        /// Describes the <see cref="P:MaxDisplayItems"/> property.  This field is read-only.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "PropertyDescriptor is immutable.")]
        public static readonly PropertyDescriptor MaxDisplayItemsProperty = PropertyDescriptor.Create(nameof(MaxDisplayItems), typeof(int), typeof(ActionMenu));

        /// <summary>
        /// Describes the <see cref="P:OverflowImageUri"/> property.  This field is read-only.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "PropertyDescriptor is immutable.")]
        public static readonly PropertyDescriptor OverflowImageUriProperty = PropertyDescriptor.Create(nameof(OverflowImageUri), typeof(Uri), typeof(ActionMenu));
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
                    throw new ArgumentOutOfRangeException(nameof(MaxDisplayItems), Resources.Strings.ValueCannotBeLessThanZero);
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
            set { nativeObject.OverflowImageUri = value; }
        }

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private readonly INativeActionMenu nativeObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionMenu"/> class.
        /// </summary>
        public ActionMenu()
            : this(typeof(INativeActionMenu), null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionMenu"/> class.
        /// </summary>
        /// <param name="resolveType">The type to pass to the IoC container in order to resolve the native object.</param>
        /// <param name="resolveName">An optional name to use when resolving the native object.</param>
        /// <param name="resolveParameters">Any parameters to pass along to the constructor of the resolve type.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="resolveType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="resolveType"/> does not resolve to an <see cref="INativeActionMenu"/> instance.</exception>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", Justification = "resolveType is validated in base constructor.")]
        protected ActionMenu(Type resolveType, string resolveName, params ResolveParameter[] resolveParameters)
            : base(resolveType, resolveName, resolveParameters)
        {
            nativeObject = ObjectRetriever.GetNativeObject(this) as INativeActionMenu;
            if (nativeObject == null)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.Strings.TypeMustResolveToType, resolveType.FullName, typeof(INativeActionMenu).FullName), nameof(resolveType));
            }

            CancelButtonTitle = "Cancel";
            Items = new MenuItemCollection(nativeObject);
            MaxDisplayItems = SystemParameters.ActionMenuMaxDisplayItems;
        }
    }
}