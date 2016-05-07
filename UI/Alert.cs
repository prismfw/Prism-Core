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
using System.Globalization;
using Prism.Native;

#if !DEBUG
using System.Diagnostics;
#endif

namespace Prism.UI
{
    /// <summary>
    /// Represents an object that modally presents an alert or confirmation dialog.
    /// </summary>
    public class Alert : FrameworkObject
    {
        /// <summary>
        /// Gets the number of buttons that have been added to the alert.
        /// </summary>
        public int ButtonCount
        {
            get { return nativeObject.ButtonCount; }
        }

        /// <summary>
        /// Gets or sets the zero-based index of the button that is mapped to the ESC key on desktop platforms.
        /// </summary>
        public int CancelButtonIndex
        {
            get { return nativeObject.CancelButtonIndex; }
            set { nativeObject.CancelButtonIndex = value; }
        }

        /// <summary>
        /// Gets or sets the zero-based index of the button that is mapped to the Enter key on desktop platforms.
        /// </summary>
        public int DefaultButtonIndex
        {
            get { return nativeObject.DefaultButtonIndex; }
            set { nativeObject.DefaultButtonIndex = value; }
        }

        /// <summary>
        /// Gets the message text for the alert.
        /// </summary>
        public string Message
        {
            get { return nativeObject.Message; }
        }

        /// <summary>
        /// Gets the title text for the alert.
        /// </summary>
        public string Title
        {
            get { return nativeObject.Title; }
        }

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        // this field is to avoid casting
        private readonly INativeAlert nativeObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="Alert"/> class.
        /// </summary>
        /// <param name="message">The message text for the alert.</param>
        /// <param name="title">The title text for the alert.</param>
        public Alert(string message, string title)
            : this(typeof(INativeAlert), null, new ResolveParameter(nameof(message), message), new ResolveParameter(nameof(title), title))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Alert"/> class.
        /// </summary>
        /// <param name="resolveType">The type to pass to the IoC container in order to resolve the native object.</param>
        /// <param name="resolveName">An optional name to use when resolving the native object.</param>
        /// <param name="resolveParameters">Any parameters to pass along to the constructor of the resolve type.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="resolveType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="resolveType"/> does not resolve to an <see cref="INativeAlert"/> instance.</exception>
        protected Alert(Type resolveType, string resolveName, params ResolveParameter[] resolveParameters)
            : base(resolveType, resolveName, resolveParameters)
        {
            nativeObject = ObjectRetriever.GetNativeObject(this) as INativeAlert;
            if (nativeObject == null)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.Strings.TypeMustResolveToType, resolveType.FullName, typeof(INativeAlert).FullName), nameof(resolveType));
            }
        }

        /// <summary>
        /// Adds the specified <see cref="AlertButton"/> to the alert.
        /// </summary>
        /// <param name="button">The button to add.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="button"/> is <c>null</c>.</exception>
        public void AddButton(AlertButton button)
        {
            if (button == null)
            {
                throw new ArgumentNullException(nameof(button));
            }

            nativeObject.AddButton(button);
        }

        /// <summary>
        /// Gets the button at the specified zero-based index.
        /// </summary>
        /// <param name="index">The zero-based index of the button to retrieve.</param>
        /// <returns>The <see cref="AlertButton"/> at the specified index -or- <c>null</c> if there is no button.</returns>
        public AlertButton GetButton(int index)
        {
            return nativeObject.GetButton(index);
        }

        /// <summary>
        /// Modally presents the alert.
        /// </summary>
        public void Show()
        {
            nativeObject.Show();
        }
    }
}
