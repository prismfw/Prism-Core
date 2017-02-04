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
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Prism.Native;
using Prism.Resources;
using Prism.Systems;
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
            private set
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

                    Visual.PropagateResourceCollectionChange(Window.Current.Content);
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
        private static bool isNavigating;
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
            Navigate(uri, null, null);
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
            Navigate(uri, options, null);
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
            if (uri == null)
            {
                throw new ArgumentNullException(nameof(uri));
            }

            Logger.Trace(CultureInfo.CurrentCulture, Strings.NavigatingToURI, uri);

            var context = new NavigationContext(uri);
            string[] uriParts = uri.Split(new[] { '/' });
            string uriPattern;

            var controller = ControllerManager.Default.Resolve(null, uriParts, out uriPattern) as IController;
            if (controller == null)
            {
                Current.OnControllerNavigationFailed(uri);
                return;
            }

            Logger.Trace(CultureInfo.CurrentCulture, Strings.MatchedURIWithPattern, uri, uriPattern);

            string[] patternParts = uriPattern.Split('/');
            for (int i = 0; i < uriParts.Length; i++)
            {
                string segment = patternParts[i].Trim();
                if (segment.Length > 1 && segment[0] == '{' && segment[segment.Length - 1] == '}')
                {
                    context.Parameters[segment.Substring(1, segment.Length - 2)] = uriParts[i];
                }
            }

            if (options != null && options.Parameters != null)
            {
                foreach (var key in options.Parameters.Keys)
                {
                    context.Parameters[key] = options.Parameters[key];
                }
            }

            if (fromView == null)
            {
                LoadController(controller, options, context, fromView);
            }
            else
            {
                var syncContext = SynchronizationContext.Current;
                Current.BeginInvokeOnMainThread(() =>
                {
                    context.OriginatingPanes |= GetViewPane(fromView);
                    syncContext.Post((state) =>
                    {
                        var array = (object[])state;
                        LoadController(array[0] as IController, (NavigationOptions)array[1], (NavigationContext)array[2], (IView)array[3]);
                    }, new object[] { controller, options, context, fromView });
                });
            }
        }

        /// <summary>
        /// Initiates a navigation to the specified controller type.
        /// </summary>
        /// <param name="controllerType">The type of the controller to navigate to.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="controllerType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="controllerType"/> does not implement the <see cref="IController"/> interface.</exception>
        public static void Navigate(Type controllerType)
        {
            Navigate(controllerType, null, null);
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
            Navigate(controllerType, options, null);
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
            if (controllerType == null)
            {
                throw new ArgumentNullException(nameof(controllerType));
            }

            Logger.Trace(CultureInfo.CurrentCulture, Strings.NavigatingToControllerType, controllerType.FullName);

            var context = new NavigationContext(controllerType.FullName);
            if (options != null)
            {
                foreach (var key in options.Parameters.Keys)
                {
                    context.Parameters[key] = options.Parameters[key];
                }
            }

            string nullValue;
            if (fromView == null)
            {
                LoadController(ControllerManager.Default.Resolve(controllerType, null, out nullValue), options, context, fromView);
            }
            else
            {
                var syncContext = SynchronizationContext.Current;
                Current.BeginInvokeOnMainThread(() =>
                {
                    context.OriginatingPanes |= GetViewPane(fromView);
                    syncContext.Post((state) =>
                    {
                        var array = (object[])state;
                        LoadController(array[0] as IController, (NavigationOptions)array[1], (NavigationContext)array[2], (IView)array[3]);
                    }, new object[] { ControllerManager.Default.Resolve(controllerType, null, out nullValue), options, context, fromView });
                });
            }
        }
        #endregion

        /// <summary>
        /// Signals the system to begin ignoring any user interactions within the application.
        /// </summary>
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
        public void BeginInvokeOnMainThread(Action action)
        {
            if (nativeObject == null)
            {
                throw new InvalidOperationException(Strings.ApplicationIsNotInitialized);
            }

            nativeObject.BeginInvokeOnMainThread(action);
        }

        /// <summary>
        /// Asynchronously invokes the specified delegate on the platform's main thread.
        /// </summary>
        /// <param name="del">A delegate to a method that takes multiple parameters.</param>
        /// <param name="parameters">The parameters for the delegate method.</param>
        public void BeginInvokeOnMainThread(Delegate del, params object[] parameters)
        {
            if (nativeObject == null)
            {
                throw new InvalidOperationException(Strings.ApplicationIsNotInitialized);
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
        public void LaunchUrl(Uri url)
        {
            if (nativeObject == null)
            {
                throw new InvalidOperationException(Strings.ApplicationIsNotInitialized);
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
        /// <param name="fromView">The view that initiated the navigation that resulted in the controller being loaded.</param>
        /// <param name="perspective">The perspective that was returned from the controller.</param>
        protected virtual void OnControllerLoaded(IController controller, IView fromView, string perspective)
        {
            if (perspective == null)
            {
                Logger.Warn(CultureInfo.CurrentCulture, Strings.NullViewPerspectiveReturned);
            }
        }

        /// <summary>
        /// Called when a controller is about to be loaded.
        /// </summary>
        /// <param name="controller">The controller being loaded.</param>
        /// <param name="fromView">The view that initiated the navigation that resulted in the controller being loaded.</param>
        /// <param name="e">The <see cref="CancelEventArgs"/> that contain the event data.</param>
        protected virtual void OnControllerLoading(IController controller, IView fromView, CancelEventArgs e)
        {
        }

        /// <summary>
        /// Called when a controller fails to complete a load.
        /// </summary>
        /// <param name="controller">The controller that failed to load.</param>
        /// <param name="ex">The exception that caused the load to fail.</param>
        protected virtual void OnControllerLoadFailed(IController controller, Exception ex)
        {
            throw new AggregateException(ex);
        }

        /// <summary>
        /// Called when a navigated URI fails to resolve to an <see cref="IController"/> instance.
        /// </summary>
        /// <param name="uri">The URI that failed to resolve.</param>
        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", Justification = "Value may contain formatting that is nonstandard to System.Uri.")]
        protected virtual void OnControllerNavigationFailed(string uri)
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
        protected virtual void OnViewPresented(IView view, IController fromController)
        {
        }

        /// <summary>
        /// Called when a view is about to be rendered and presented on the screen.
        /// </summary>
        /// <param name="view">The view being rendered and presented.</param>
        /// <param name="fromController">The controller that returned the perspective for the view.</param>
        /// <param name="e">The <see cref="CancelEventArgs"/> that contain the event data.</param>
        protected virtual void OnViewPresenting(IView view, IController fromController, CancelEventArgs e)
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

                appInstance.nativeObject.Exiting += (o, e) => appInstance.OnShutdown();
                appInstance.nativeObject.Resuming += (o, e) => appInstance.OnResume();
                appInstance.nativeObject.Suspending += (o, e) => appInstance.OnSuspend();
                appInstance.nativeObject.UnhandledException += (o, e) => appInstance.OnUnhandledException(e.Exception);
            }

            SessionManager.SetApplication(appInstance);
            appInstance.OnInitialized();
        }

        private static bool CheckPaneForView(object paneContent, IView view)
        {
            return paneContent == view || ((paneContent as ViewStack)?.Views.Contains(view) ?? false);
        }

        private static Panes GetViewPane(IView view)
        {
            var parent = VisualTreeHelper.GetParent(view, (o) => o is Window || o is SplitView || o is TabbedSplitView || o is Popup);
            if (parent is Window)
            {
                return Panes.Master;
            }

            if (parent is Popup)
            {
                return Panes.Popup;
            }

            var splitView = parent as SplitView;
            if (splitView != null)
            {
                if (CheckPaneForView(splitView.MasterContent, view))
                {
                    return Panes.Master;
                }
                else if (CheckPaneForView(splitView.DetailContent, view))
                {
                    return Panes.Detail;
                }
            }

            var tabView = Window.Current.Content as TabbedSplitView;
            if (tabView != null)
            {
                if (CheckPaneForView(tabView.DetailContent, view))
                {
                    return Panes.Detail;
                }

                var tabItem = tabView.TabItems[tabView.SelectedIndex];
                if (CheckPaneForView(tabItem.Content, view))
                {
                    return Panes.Master;
                }

                for (int i = 0; i < tabView.TabItems.Count; i++)
                {
                    if (i != tabView.SelectedIndex)
                    {
                        tabItem = tabView.TabItems[i];
                        if (CheckPaneForView(tabItem.Content, view))
                        {
                            return Panes.Master;
                        }
                    }
                }
            }

            return Panes.Unknown;
        }

        private static void LoadController(IController controller, NavigationOptions options, NavigationContext context, IView fromView)
        {
            if (controller == null)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Strings.ControllerDoesNotImplementInterface, typeof(IController).FullName));
            }

            Logger.Trace(CultureInfo.CurrentCulture, Strings.LoadingControllerOfType, controller.GetType().FullName);

            var current = Current;
            lock (current)
            {
                if (isNavigating)
                {
                    context.OriginatingPanes |= current.lastNavigatedContext.OriginatingPanes;
                }

                isNavigating = true;
                current.LastNavigationContext = context;

                var args = new CancelEventArgs();
                current.OnControllerLoading(controller, fromView, args);
                if (args.Cancel)
                {
                    return;
                }

                current.BeginInvokeOnMainThread(() => current.BeginIgnoringUserInput());

                Task.Run(async () =>
                {
                    Timer loadTimer = null;
                    double delay = options == null ? LoadIndicator.DefaultDelay : options.LoadIndicatorDelay;
                    if (delay >= 0 && !double.IsInfinity(delay))
                    {
                        loadTimer = new Timer(delay, false);
                        loadTimer.Elapsed += (o, e) =>
                        {
                            current.BeginInvokeOnMainThread(() =>
                            {
                                if (loadTimer != null)
                                {
                                    LoadIndicator.DefaultIndicator.TextLabel.Text = options?.LoadIndicatorTitle == null ? LoadIndicator.DefaultTitle : options.LoadIndicatorTitle;
                                    LoadIndicator.DefaultIndicator.TextLabel.Visibility = string.IsNullOrEmpty(LoadIndicator.DefaultIndicator.TextLabel.Text) ?
                                        Visibility.Collapsed : Visibility.Visible;

                                    LoadIndicator.DefaultIndicator.Show();
                                }
                            });
                        };

                        loadTimer.Start();
                    }

                    string perspective = null;
                    try
                    {
                        perspective = await controller.LoadAsync(new NavigationContext(context));
                        if (controller.ModelType == null)
                        {
                            throw new InvalidOperationException(Strings.ControllerModelTypeCannotBeNull);
                        }
                    }
                    catch (Exception ex)
                    {
                        loadTimer?.Stop();
                        loadTimer = null;
                        current.BeginInvokeOnMainThread(() =>
                        {
                            LoadIndicator.DefaultIndicator.Hide();
                            current.EndIgnoringUserInput();
                        });

                        Logger.Trace(CultureInfo.CurrentCulture, Strings.ControllerLoadFailedWithMessage, ex.Message);
                        current.OnControllerLoadFailed(controller, ex);
                        return;
                    }

                    loadTimer?.Stop();
                    loadTimer = null;

                    Logger.Trace(CultureInfo.CurrentCulture, Strings.ControllerLoadedAndReturnedPerspective, perspective);
                    current.OnControllerLoaded(controller, fromView, perspective);

                    if (perspective == null)
                    {
                        return;
                    }

                    isNavigating = false;
                    current.BeginInvokeOnMainThread(async () =>
                    {
                        LoadIndicator.DefaultIndicator.Hide();
                        current.EndIgnoringUserInput();

                        var controllerModelType = controller.GetModel()?.GetType() ?? controller.ModelType;
                        
                        var view = ViewManager.Default.Resolve(controllerModelType, perspective) as IView;
                        if (view == null)
                        {
                            Logger.Warn(CultureInfo.CurrentCulture, Strings.UnableToLocateViewWithPerspectiveAndModelType, perspective, controllerModelType.FullName);
                            return;
                        }

                        view.SetModel(controller.GetModel());

                        args = new CancelEventArgs();
                        current.OnViewPresenting(view, controller, args);
                        if (args.Cancel)
                        {
                            return;
                        }

                        Logger.Trace(CultureInfo.CurrentCulture, Strings.ReadyToOutputViewOfType, view.GetType().FullName);
                        await view.ConfigureUIAsync();

                        PresentView(view, context);
                        current.OnViewPresented(view, controller);
                    });
                });
            }
        }

        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "Method is sufficiently maintainable.")]
        private static void PresentView(IView view, NavigationContext context)
        {
            // Tab views and split views can only be the content of the main window.  Having them elsewhere is unsupported!
            if (view is TabView || view is SplitView)
            {
                Window.Current.Content = view;
                return;
            }

            if (Window.Current.Content == null)
            {
                Window.Current.Content = Device.Current.FormFactor == FormFactor.Tablet || Device.Current.FormFactor == FormFactor.Desktop ?
                    (object)new SplitView() { MasterContent = new ViewStack() } : new ViewStack();
            }

            var masterStack = Window.Current.Content as ViewStack;
            ViewStack detailStack = null;

            // gather the master and detail stacks
            if (masterStack == null)
            {
                var splitView = Window.Current.Content as SplitView;
                if (splitView != null)
                {
                    if (splitView.MasterContent == null)
                    {
                        splitView.MasterContent = (masterStack = new ViewStack());
                    }
                    else
                    {
                        masterStack = splitView.MasterContent as ViewStack;
                    }

                    if (splitView.DetailContent == null)
                    {
                        detailStack = new ViewStack();
                        detailStack.PushView(new ContentView() { IsValidBackTarget = false });
                        splitView.DetailContent = detailStack;
                    }
                    else
                    {
                        detailStack = splitView.DetailContent as ViewStack;
                    }
                }

                var tabView = Window.Current.Content as TabView;
                if (tabView != null)
                {
                    var tabItem = tabView.TabItems[tabView.SelectedIndex];
                    if (tabItem.Content == null)
                    {
                        // If the current tab item does not have content, create a view stack to act as the content.
                        tabItem.Content = (masterStack = new ViewStack());
                    }
                    else
                    {
                        masterStack = tabItem.Content as ViewStack;
                    }

                    var split = tabView as TabbedSplitView;
                    if (split != null)
                    {
                        if (split.DetailContent == null)
                        {
                            detailStack = new ViewStack();
                            detailStack.PushView(new ContentView() { IsValidBackTarget = false });
                            split.DetailContent = detailStack;
                        }

                        detailStack = split.DetailContent as ViewStack;
                    }
                }
            }

            /*
             * Preferred Pane rules:
             * - If a view of the same type exists in any stack, replace and pop.
             * - If preferred pane is detail and a master or tab stack is empty, push to master.
             * - If full-screen and preferred pane is detail, push to master.
             * - If none of the above, honor the attribute.
            */

            var popupStack = Window.Current.PresentedPopup?.Content as ViewStack;
            if (PopToView(popupStack, view))
            {
                return;
            }

            if (PopToView(detailStack, view))
            {
                Window.Current.PresentedPopup?.Close();
                return;
            }

            if (PopToView(masterStack, view))
            {
                Window.Current.PresentedPopup?.Close();
                return;
            }

            var preferredAttribute = view.GetType().GetTypeInfo().GetCustomAttributes<PreferredPanesAttribute>(false);
            var preferredPanes = Panes.Unknown;
            foreach (var att in preferredAttribute)
            {
                preferredPanes |= att.PreferredPanes;
            }

            if (preferredPanes == Panes.Popup || ((preferredPanes == Panes.Unknown || preferredPanes.HasFlag(Panes.Popup)) && popupStack != null))
            {
                if (popupStack == null)
                {
                    popupStack = new ViewStack();
                    popupStack.PushView(view, Animate.Off);
                    new Popup() { Content = popupStack }.Open(Window.Current);
                }
                else
                {
                    popupStack.PushView(view, Animate.Default);
                }
                return;
            }

            if (masterStack == null && detailStack == null)
            {
                Logger.Warn(CultureInfo.CurrentCulture, Strings.UnableToLocateViewStack);
                return;
            }

            Window.Current.PresentedPopup?.Close();

            if (masterStack != null && (!masterStack.Views.Any() || detailStack == null || (preferredPanes.HasFlag(Panes.Master) &&
                (!preferredPanes.HasFlag(Panes.Detail) || detailStack.Views.Count() <= 1))))
            {
                masterStack.PushView(view, Animate.Default);
                detailStack?.PopToRoot(Animate.Off);
            }
            else if (detailStack != null)
            {
                if (context.OriginatingPanes.HasFlag(Panes.Master))
                {
                    if (detailStack.Views.Count() > 1)
                    {
                        detailStack.InsertView(view, 1, Animate.Off);
                        detailStack.PopToView(view, Animate.Off);
                    }
                    else
                    {
                        detailStack.PushView(view, Animate.Off);
                    }
                }
                else
                {
                    detailStack.PushView(view, Animate.Default);
                }
            }
        }

        private static bool PopToView(ViewStack stack, IView view)
        {
            if (stack != null)
            {
                var vsc = view as IViewStackChild;
                foreach (var currentView in stack.Views)
                {
                    if (currentView == view)
                    {
                        stack.PopToView(view, Animate.Default);
                        return true;
                    }
                    
                    if (vsc != null)
                    {
                        var currentVSC = currentView as IViewStackChild;
                        if (currentVSC == null)
                        {
                            continue;
                        }

                        if (currentVSC.StackId == vsc.StackId && (vsc.StackId != null || currentView.GetType() == view.GetType()))
                        {
                            stack.ReplaceView(currentView, view, Animate.Off);
                            stack.PopToView(view, Animate.Default);
                            return true;
                        }
                    }
                    else if (currentView.GetType() == view.GetType())
                    {
                        stack.ReplaceView(currentView, view, Animate.Off);
                        stack.PopToView(view, Animate.Default);
                        return true;
                    }
                }
            }

            return false;
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