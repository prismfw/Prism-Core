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
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Prism.Native;
using Prism.Resources;
using Prism.UI;
using Prism.UI.Controls;
using Prism.Utilities;

namespace Prism
{
    /// <summary>
    /// Represents a cross-platform application.  This class is abstract.
    /// </summary>
    public abstract class Application : FrameworkObject
    {
        #region Property Descriptors
        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:LastNavigationContext"/> property.
        /// </summary>
        public static PropertyDescriptor LastNavigationContextProperty { get; } = PropertyDescriptor.Create(nameof(LastNavigationContext), typeof(NavigationContext), typeof(Application), true);
        #endregion

        /// <summary>
        /// Gets the <see cref="Application"/> instance for the current session.
        /// </summary>
        public static Application Current
        {
            get { return SessionManager.GetCurrentApplication(); }
        }

        /// <summary>
        /// Gets the context of the last navigation that occurred.
        /// </summary>
        public NavigationContext LastNavigationContext
        {
            get { return lastNavigatedContext; }
            internal set
            {
                if (value != lastNavigatedContext)
                {
                    lastNavigatedContext = value;
                    OnPropertyChanged(LastNavigationContextProperty);
                }
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private NavigationContext lastNavigatedContext;

        /// <summary>
        /// Gets the platform on which the application is running.
        /// </summary>
        public Platform Platform
        {
            get { return nativeObject == null ? Platform.Unknown : nativeObject.Platform; }
        }

        /// <summary>
        /// Gets or sets a collection of application-scoped resources.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Consuming developers should be able to use their own dictionaries.")]
        public ResourceDictionary Resources
        {
            get
            {
                if (resources == null)
                {
                    resources = new ResourceDictionary();
                    resources.ResourceChanged += OnResourceChanged;
                    resources.ResourceCollectionChanged += OnResourceCollectionChanged;
                }

                return resources;
            }
            set
            {
                if (value != resources)
                {
                    if (resources != null)
                    {
                        resources.ResourceChanged -= OnResourceChanged;
                        resources.ResourceCollectionChanged -= OnResourceCollectionChanged;
                    }

                    resources = value;
                    if (resources != null)
                    {
                        resources.ResourceChanged -= OnResourceChanged;
                        resources.ResourceChanged += OnResourceChanged;

                        resources.ResourceCollectionChanged -= OnResourceCollectionChanged;
                        resources.ResourceCollectionChanged += OnResourceCollectionChanged;
                    }

                    OnResourceCollectionChanged(resources, EventArgs.Empty);
                }
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ResourceDictionary resources;

        /// <summary>
        /// Gets a collection of application settings that are scoped to the current user session.
        /// </summary>
        public SessionSettings Session { get; internal set; }

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private INativeApplication nativeObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="Application"/> class.
        /// </summary>
        protected Application()
        {
        }

        #region Navigate methods
        /// <summary>
        /// Initiates a navigation to the specified URI.
        /// Be sure to apply a <see cref="NavigationControllerAttribute"/> to the controller type that should be associated with the URI.
        /// </summary>
        /// <param name="uri">A <see cref="string"/> representing the URI to navigate to.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="uri"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when the matching controller does not implement the <see cref="IController"/> interface.</exception>
        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", Justification = "Value may contain formatting that is nonstandard to System.Uri.")]
        public static void Navigate(string uri)
        {
            NavigationService.Navigate(uri, null, null);
        }

        /// <summary>
        /// Initiates a navigation to the specified URI.
        /// Be sure to apply a <see cref="NavigationControllerAttribute"/> to the controller type that should be associated with the URI.
        /// </summary>
        /// <param name="uri">A <see cref="string"/> representing the URI to navigate to.</param>
        /// <param name="options">Additional options to customize the navigation.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="uri"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when the matching controller does not implement the <see cref="IController"/> interface.</exception>
        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", Justification = "Value may contain formatting that is nonstandard to System.Uri.")]
        public static void Navigate(string uri, NavigationOptions options)
        {
            NavigationService.Navigate(uri, options, null);
        }

        /// <summary>
        /// Initiates a navigation to the specified URI.
        /// Be sure to apply a <see cref="NavigationControllerAttribute"/> to the controller type that should be associated with the URI.
        /// </summary>
        /// <param name="uri">A <see cref="string"/> representing the URI to navigate to.</param>
        /// <param name="options">Additional options to customize the navigation.</param>
        /// <param name="fromView">The <see cref="IView"/> instance from which the navigation was initiated.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="uri"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when the matching controller does not implement the <see cref="IController"/> interface.</exception>
        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", Justification = "Value may contain formatting that is nonstandard to System.Uri.")]
        public static void Navigate(string uri, NavigationOptions options, IView fromView)
        {
            NavigationService.Navigate(uri, options, fromView);
        }

        /// <summary>
        /// Initiates a navigation to the specified controller type.
        /// </summary>
        /// <param name="controllerType">The type of the controller to navigate to.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="controllerType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="controllerType"/> does not implement the <see cref="IController"/> interface.</exception>
        public static void Navigate(Type controllerType)
        {
            NavigationService.Navigate(controllerType, null, null);
        }

        /// <summary>
        /// Initiates a navigation to the specified controller type.
        /// </summary>
        /// <param name="controllerType">The type of the controller to navigate to.</param>
        /// <param name="options">Additional options to customize the navigation.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="controllerType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="controllerType"/> does not implement the <see cref="IController"/> interface.</exception>
        public static void Navigate(Type controllerType, NavigationOptions options)
        {
            NavigationService.Navigate(controllerType, options, null);
        }

        /// <summary>
        /// Initiates a navigation to the specified controller type.
        /// </summary>
        /// <param name="controllerType">The type of the controller to navigate to.</param>
        /// <param name="options">Additional options to customize the navigation.</param>
        /// <param name="fromView">The <see cref="IView"/> instance from which the navigation was initiated.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="controllerType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="controllerType"/> does not implement the <see cref="IController"/> interface.</exception>
        public static void Navigate(Type controllerType, NavigationOptions options, IView fromView)
        {
            NavigationService.Navigate(controllerType, options, fromView);
        }
        #endregion

        /// <summary>
        /// Signals the system to begin ignoring any user interactions within the application.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when the application has not been fully initialized.</exception>
        public void BeginIgnoringUserInput()
        {
            if (nativeObject == null)
            {
                throw new InvalidOperationException(Strings.ApplicationIsNotInitialized);
            }

            nativeObject.BeginIgnoringUserInput();
        }

        /// <summary>
        /// Asynchronously invokes the specified delegate on the platform's main thread.
        /// </summary>
        /// <param name="action">The action to invoke on the main thread.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="action"/> is <c>null</c>.</exception>
        public void BeginInvokeOnMainThread(Action action)
        {
            if (nativeObject == null)
            {
                throw new InvalidOperationException(Strings.ApplicationIsNotInitialized);
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            nativeObject.BeginInvokeOnMainThread(action);
        }

        /// <summary>
        /// Asynchronously invokes the specified delegate on the platform's main thread.
        /// </summary>
        /// <param name="del">A delegate to a method that takes multiple parameters.</param>
        /// <param name="parameters">The parameters for the delegate method.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="del"/> is <c>null</c>.</exception>
        public void BeginInvokeOnMainThread(Delegate del, params object[] parameters)
        {
            if (nativeObject == null)
            {
                throw new InvalidOperationException(Strings.ApplicationIsNotInitialized);
            }

            if (del == null)
            {
                throw new ArgumentNullException(nameof(del));
            }

            nativeObject.BeginInvokeOnMainThreadWithParameters(del, parameters);
        }

        /// <summary>
        /// Signals the system to stop ignoring user interactions within the application.
        /// </summary>
        public void EndIgnoringUserInput()
        {
            if (nativeObject == null)
            {
                throw new InvalidOperationException(Strings.ApplicationIsNotInitialized);
            }

            nativeObject.EndIgnoringUserInput();
        }

        /// <summary>
        /// Searches for a resource with the specified key and throws an exception if the resource cannot be found.
        /// </summary>
        /// <param name="resourceKey">The key of the resource to find.</param>
        /// <returns>The requested resource object.</returns>
        /// <exception cref="ArgumentException">Thrown when a resource for the specified <paramref name="resourceKey"/> cannot be found.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="resourceKey"/> is <c>null</c>.</exception>
        public object FindResource(object resourceKey)
        {
            if (resourceKey == null)
            {
                throw new ArgumentNullException(nameof(resourceKey));
            }

            object retval;
            if (Resources.TryGetValue(resourceKey, out retval))
            {
                return retval;
            }

            throw new ArgumentException(Strings.ResourceCouldNotBeFound, nameof(resourceKey));
        }

        /// <summary>
        /// Launches the specified URL in an external application, most commonly a web browser.
        /// </summary>
        /// <param name="url">The URL to launch to.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="url"/> is <c>null</c>.</exception>
        public void LaunchUrl(Uri url)
        {
            if (nativeObject == null)
            {
                throw new InvalidOperationException(Strings.ApplicationIsNotInitialized);
            }

            if (url == null)
            {
                throw new ArgumentNullException(nameof(url));
            }

            nativeObject.LaunchUrl(url);
        }

        /// <summary>
        /// Searches for a resource with the specified key.
        /// </summary>
        /// <param name="resourceKey">The key of the resource to find.</param>
        /// <returns>The requested resource object, or <c>null</c> if no resource is found.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="resourceKey"/> is <c>null</c>.</exception>
        public object TryFindResource(object resourceKey)
        {
            if (resourceKey == null)
            {
                throw new ArgumentNullException(nameof(resourceKey));
            }

            object retval;
            Resources.TryGetValue(resourceKey, out retval);
            return retval;
        }

        /// <summary>
        /// Called when a controller has successfully completed a load.
        /// </summary>
        /// <param name="controller">The controller that was loaded.</param>
        /// <param name="context">The context describing the navigation that resulted in the controller being loaded.</param>
        /// <param name="fromView">The view that initiated the navigation that resulted in the controller being loaded.</param>
        /// <param name="perspective">The perspective that was returned from the controller.</param>
        protected internal virtual void OnControllerLoaded(IController controller, NavigationContext context, IView fromView, string perspective)
        {
            if (perspective == null && context == LastNavigationContext)
            {
                Logger.Warn(CultureInfo.CurrentCulture, Strings.NullViewPerspectiveReturned, controller?.GetType().FullName);
            }
        }

        /// <summary>
        /// Called when a controller is about to be loaded.
        /// </summary>
        /// <param name="controller">The controller being loaded.</param>
        /// <param name="context">The context describing the navigation that resulted in the controller being loaded.</param>
        /// <param name="fromView">The view that initiated the navigation that resulted in the controller being loaded.</param>
        /// <param name="e">The <see cref="CancelEventArgs"/> that contain the event data.</param>
        protected internal virtual void OnControllerLoading(IController controller, NavigationContext context, IView fromView, CancelEventArgs e)
        {
        }

        /// <summary>
        /// Called when a controller fails to complete a load.
        /// </summary>
        /// <param name="controller">The controller that failed to load.</param>
        /// <param name="ex">The exception that caused the load to fail.</param>
        protected internal virtual void OnControllerLoadFailed(IController controller, Exception ex)
        {
            BeginInvokeOnMainThread(() => { throw new AggregateException(ex); });
        }

        /// <summary>
        /// Called when a navigated URI fails to resolve to an <see cref="IController"/> instance.
        /// </summary>
        /// <param name="uri">The URI that failed to resolve.</param>
        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", Justification = "Value may contain formatting that is nonstandard to System.Uri.")]
        protected internal virtual void OnControllerNavigationFailed(string uri)
        {
            Logger.Warn(CultureInfo.CurrentCulture, Strings.NoControllerFoundForURI, uri);
        }

        /// <summary>
        /// Called when the application instance has completed initialization and is ready for use.
        /// This method is meant to be overridden in consuming applications for application-level initialization code.
        /// </summary>
        protected virtual void OnInitialized()
        {
        }

        /// <summary>
        /// Called when the application resumes from a suspended state.
        /// </summary>
        protected virtual void OnResume()
        {
        }

        /// <summary>
        /// Called when the application is about to terminate.
        /// </summary>
        protected virtual void OnShutdown()
        {
        }

        /// <summary>
        /// Called when the application is suspended and pushed to the background.
        /// </summary>
        protected virtual void OnSuspend()
        {
        }

        /// <summary>
        /// Called when the application encounters an unhandled exception.
        /// The base implementation will log the exception as a Fatal message.
        /// </summary>
        /// <param name="ex">The exception that was unhandled.</param>
        protected virtual void OnUnhandledException(Exception ex)
        {
            Logger.Fatal(CultureInfo.CurrentCulture, Strings.ApplicationEncounterUnhandledException, ex);
        }

        /// <summary>
        /// Called when a view has rendered and been presented on the screen.
        /// </summary>
        /// <param name="view">The view that was rendered and presented.</param>
        /// <param name="fromController">The controller that returned the perspective for the view.</param>
        protected internal virtual void OnViewPresented(IView view, IController fromController)
        {
        }

        /// <summary>
        /// Called when a view is about to be rendered and presented on the screen.
        /// </summary>
        /// <param name="view">The view being rendered and presented.</param>
        /// <param name="fromController">The controller that returned the perspective for the view.</param>
        /// <param name="e">The <see cref="CancelEventArgs"/> that contain the event data.</param>
        protected internal virtual void OnViewPresenting(IView view, IController fromController, CancelEventArgs e)
        {
        }

        internal static void Initialize(Application appInstance)
        {
            if (appInstance.nativeObject == null)
            {
                appInstance.nativeObject = TypeManager.Default.Resolve<INativeApplication>();
                if (appInstance.nativeObject == null)
                {
                    throw new InvalidOperationException(Strings.NativeApplicationCouldNotBeResolved);
                }

                ObjectRetriever.SetPair(appInstance, appInstance.nativeObject);
                appInstance.nativeObject.Exiting += (o, e) => appInstance.OnShutdown();
                appInstance.nativeObject.Resuming += (o, e) => appInstance.OnResume();
                appInstance.nativeObject.Suspending += (o, e) => appInstance.OnSuspend();
                appInstance.nativeObject.UnhandledException += (o, e) => appInstance.OnUnhandledException(e.Exception);
            }

            SessionManager.SetApplication(appInstance);
            appInstance.OnInitialized();
        }

        private void OnResourceChanged(object sender, object key)
        {
            Visual.PropagateResourceChange(Window.Current.Content, key);

            var popup = Window.Current.PresentedPopup;
            while (popup != null)
            {
                Visual.PropagateResourceChange(popup, key);
                popup = popup.PresentedPopup;
            }

            Visual.PropagateResourceChange(LoadIndicator.DefaultIndicator, key);
        }

        private void OnResourceCollectionChanged(object sender, EventArgs e)
        {
            Visual.PropagateResourceCollectionChange(Window.Current.Content);

            var popup = Window.Current.PresentedPopup;
            while (popup != null)
            {
                Visual.PropagateResourceCollectionChange(popup);
                popup = popup.PresentedPopup;
            }

            Visual.PropagateResourceCollectionChange(LoadIndicator.DefaultIndicator);
        }
    }
}