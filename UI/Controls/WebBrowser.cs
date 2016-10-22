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
using Prism.Resources;

#if !DEBUG
using System.Diagnostics;
#endif

namespace Prism.UI.Controls
{
    /// <summary>
    /// Represents a UI element that hosts HTML content within an application.
    /// </summary>
    public class WebBrowser : Element
    {
        #region Event Descriptors
        /// <summary>
        /// Gets an <see cref="EventDescriptor"/> describing the <see cref="E:NavigationCompleted"/> event.
        /// </summary>
        public static EventDescriptor NavigationCompletedEvent { get; } = EventDescriptor.Create(nameof(NavigationCompleted), typeof(TypedEventHandler<WebBrowser, WebNavigationCompletedEventArgs>), typeof(WebBrowser));

        /// <summary>
        /// Gets an <see cref="EventDescriptor"/> describing the <see cref="E:NavigationStarting"/> event.
        /// </summary>
        public static EventDescriptor NavigationStartingEvent { get; } = EventDescriptor.Create(nameof(NavigationStarting), typeof(TypedEventHandler<WebBrowser, WebNavigationStartingEventArgs>), typeof(WebBrowser));

        /// <summary>
        /// Gets an <see cref="EventDescriptor"/> describing the <see cref="E:ScriptCompleted"/> event.
        /// </summary>
        public static EventDescriptor ScriptCompletedEvent { get; } = EventDescriptor.Create(nameof(ScriptCompleted), typeof(TypedEventHandler<WebBrowser, WebScriptCompletedEventArgs>), typeof(WebBrowser));
        #endregion

        #region Property Descriptors
        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:CanGoBack"/> property.
        /// </summary>
        public static PropertyDescriptor CanGoBackProperty { get; } = PropertyDescriptor.Create(nameof(CanGoBack), typeof(bool), typeof(WebBrowser), true);

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:CanGoForward"/> property.
        /// </summary>
        public static PropertyDescriptor CanGoForwardProperty { get; } = PropertyDescriptor.Create(nameof(CanGoForward), typeof(bool), typeof(WebBrowser), true);

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:Title"/> property.
        /// </summary>
        public static PropertyDescriptor TitleProperty { get; } = PropertyDescriptor.Create(nameof(Title), typeof(string), typeof(WebBrowser), true);

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:Uri"/> property.
        /// </summary>
        public static PropertyDescriptor UriProperty { get; } = PropertyDescriptor.Create(nameof(Uri), typeof(Uri), typeof(WebBrowser), true);
        #endregion

        /// <summary>
        /// Occurs when the web browser has finished loading the document.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification = "Event handler provides a strongly-typed sender for easier use.")]
        public event TypedEventHandler<WebBrowser, WebNavigationCompletedEventArgs> NavigationCompleted;

        /// <summary>
        /// Occurs when the web browser has begun navigating to a document.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification = "Event handler provides a strongly-typed sender for easier use.")]
        public event TypedEventHandler<WebBrowser, WebNavigationStartingEventArgs> NavigationStarting;

        /// <summary>
        /// Occurs when a script invocation has completed.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification = "Event handler provides a strongly-typed sender for easier use.")]
        public event TypedEventHandler<WebBrowser, WebScriptCompletedEventArgs> ScriptCompleted;

        /// <summary>
        /// Gets a value indicating whether the web browser has at least one document in its back navigation history.
        /// </summary>
        public bool CanGoBack
        {
            get { return nativeObject.CanGoBack; }
        }

        /// <summary>
        /// Gets a value indicating whether the web browser has at least one document in its forward navigation history.
        /// </summary>
        public bool CanGoForward
        {
            get { return nativeObject.CanGoForward; }
        }

        /// <summary>
        /// Gets the title of the current document.
        /// </summary>
        public string Title
        {
            get { return nativeObject.Title; }
        }

        /// <summary>
        /// Gets the URI of the current document.
        /// </summary>
        public Uri Uri
        {
            get { return nativeObject.Uri; }
        }

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        // this field is to avoid casting
        private readonly INativeWebBrowser nativeObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebBrowser"/> class.
        /// </summary>
        public WebBrowser()
            : this(typeof(INativeWebBrowser), null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebBrowser"/> class.
        /// </summary>
        /// <param name="resolveType">The type to pass to the IoC container in order to resolve the native object.</param>
        /// <param name="resolveName">An optional name to use when resolving the native object.</param>
        /// <param name="resolveParameters">Any parameters to pass along to the constructor of the resolve type.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="resolveType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="resolveType"/> does not resolve to an <see cref="INativeWebBrowser"/> instance.</exception>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", Justification = "resolveType is validated in base constructor.")]
        protected WebBrowser(Type resolveType, string resolveName, params ResolveParameter[] resolveParameters)
            : base(resolveType, resolveName, resolveParameters)
        {
            nativeObject = ObjectRetriever.GetNativeObject(this) as INativeWebBrowser;
            if (nativeObject == null)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Strings.TypeMustResolveToType, resolveType.FullName, typeof(INativeWebBrowser).FullName), nameof(resolveType));
            }

            nativeObject.NavigationCompleted += (o, e) => OnNavigationCompleted(e);
            nativeObject.NavigationStarting += (o, e) => OnNavigationStarting(e);
            nativeObject.ScriptCompleted += (o, e) => OnScriptCompleted(e);

            HorizontalAlignment = HorizontalAlignment.Stretch;
            VerticalAlignment = VerticalAlignment.Stretch;
        }

        /// <summary>
        /// Navigates to the previous document in the navigation history.
        /// </summary>
        public void GoBack()
        {
            nativeObject.GoBack();
        }

        /// <summary>
        /// Navigates to the next document in the navigation history.
        /// </summary>
        public void GoForward()
        {
            nativeObject.GoForward();
        }

        /// <summary>
        /// Executes a script function that is implemented by the current document.
        /// </summary>
        /// <param name="scriptName">The name of the script function to execute.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="scriptName"/> is <c>null</c>.</exception>
        public void InvokeScript(string scriptName)
        {
            if (scriptName == null)
            {
                throw new ArgumentNullException(nameof(scriptName));
            }

            nativeObject.InvokeScript(scriptName);
        }

        /// <summary>
        /// Navigates to the specified <see cref="Uri"/>.
        /// </summary>
        /// <param name="uri">The URI to navigate to.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="uri"/> is <c>null</c>.</exception>
        public void Navigate(Uri uri)
        {
            if (uri == null)
            {
                throw new ArgumentNullException(nameof(uri));
            }
            
            nativeObject.Navigate(uri);
        }

        /// <summary>
        /// Navigates to the specified <see cref="string"/> containing the content for a document.
        /// </summary>
        /// <param name="html">The string containing the content for a document.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="html"/> is <c>null</c>.</exception>
        public void NavigateToString(string html)
        {
            if (html == null)
            {
                throw new ArgumentNullException(nameof(html));
            }
            
            nativeObject.NavigateToString(html);
        }

        /// <summary>
        /// Reloads the current document.
        /// </summary>
        public void Refresh()
        {
            nativeObject.Refresh();
        }

        /// <summary>
        /// Called when the web browser finishes loading a document and raises the <see cref="NavigationCompleted"/> event.
        /// </summary>
        /// <param name="e">The event arguments containing the event details.</param>
        protected virtual void OnNavigationCompleted(WebNavigationCompletedEventArgs e)
        {
            NavigationCompleted?.Invoke(this, e);
        }

        /// <summary>
        /// Called when the web browser begins navigating to a document and raises the <see cref="NavigationStarting"/> event.
        /// </summary>
        /// <param name="e">The event arguments containing the navigation details.</param>
        protected virtual void OnNavigationStarting(WebNavigationStartingEventArgs e)
        {
            NavigationStarting?.Invoke(this, e);
        }

        /// <summary>
        /// Called when a script invocation completes and raises the <see cref="ScriptCompleted"/> event.
        /// </summary>
        /// <param name="e">The event arguments containing the event details.</param>
        protected virtual void OnScriptCompleted(WebScriptCompletedEventArgs e)
        {
            ScriptCompleted?.Invoke(this, e);
        }
    }
}
