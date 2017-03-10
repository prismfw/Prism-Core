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
using System.Threading.Tasks;
using Prism.Native;

#if !DEBUG
using System.Diagnostics;
#endif

namespace Prism.UI
{
    /// <summary>
    /// Represents a system-provided UI element that displays system status information to the user.
    /// </summary>
    public sealed class StatusBar : FrameworkObject
    {
        /// <summary>
        /// Gets the status bar for the current application.
        /// </summary>
        public static StatusBar Current { get; } = new StatusBar(typeof(INativeStatusBar), null);

        /// <summary>
        /// Gets or sets the background color for the status bar.
        /// </summary>
        public Color BackgroundColor
        {
            get { return nativeObject.BackgroundColor; }
            set { nativeObject.BackgroundColor = value; }
        }

        /// <summary>
        /// Gets a rectangle describing the area that the status bar is consuming.
        /// </summary>
        public Rectangle Frame
        {
            get { return nativeObject.Frame; }
        }

        /// <summary>
        /// Gets a value indicating whether the status bar is visible.
        /// </summary>
        public bool IsVisible
        {
            get { return nativeObject.IsVisible; }
        }

        /// <summary>
        /// Gets or sets the style of the status bar.
        /// </summary>
        public StatusBarStyle Style
        {
            get { return nativeObject.Style; }
            set { nativeObject.Style = value; }
        }

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private readonly INativeStatusBar nativeObject;

        private StatusBar(Type resolveType, string resolveName, params ResolveParameter[] resolveParams)
            : base(resolveType, resolveName, resolveParams)
        {
            nativeObject = ObjectRetriever.GetNativeObject(this) as INativeStatusBar;
            if (nativeObject == null)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.Strings.TypeMustResolveToType, resolveType.FullName, typeof(INativeStatusBar).FullName), nameof(resolveType));
            }
        }

        /// <summary>
        /// Hides the status bar from view.
        /// </summary>
        public Task HideAsync()
        {
            return nativeObject.HideAsync();
        }

        /// <summary>
        /// Shows the status bar if it is not visible.
        /// </summary>
        public Task ShowAsync()
        {
            return nativeObject.ShowAsync();
        }
    }
}
