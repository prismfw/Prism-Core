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
using Prism.Systems;
using Prism.UI;
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
        /// Be sure to apply a <see cref="NavigationAttribute"/> to the controller type that should be associated with the URI.
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
        /// Be sure to apply a <see cref="NavigationAttribute"/> to the controller type that should be associated with the URI.
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
        /// Be sure to apply a <see cref="NavigationAttribute"/> to the controller type that should be associated with the URI.
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

            Logger.Trace(CultureInfo.CurrentCulture, Resources.Strings.NavigatingToURI, uri);

            var context = new NavigationContext(uri);
            string[] uriParts = uri.Split(new[] { '/' });
            string uriPattern;

            var controller = ControllerManager.Default.Resolve(null, uriParts, out uriPattern) as IController;
            if (controller == null)
            {
                Current.OnControllerNavigationFailed(uri);
                return;
            }

            Logger.Trace(CultureInfo.CurrentCulture, Resources.Strings.MatchedURIWithPattern, uri, uriPattern);

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

            Logger.Trace(CultureInfo.CurrentCulture, Resources.Strings.NavigatingToControllerType, controllerType.FullName);

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
                throw new InvalidOperationException(Resources.Strings.ApplicationIsNotInitialized);
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
                throw new InvalidOperationException(Resources.Strings.ApplicationIsNotInitialized);
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
                throw new InvalidOperationException(Resources.Strings.ApplicationIsNotInitialized);
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
                throw new InvalidOperationException(Resources.Strings.ApplicationIsNotInitialized);
            }

            nativeObject.EndIgnoringUserInput();
        }

        /// <summary>
        /// Launches the specified URL in an external application, most commonly a web browser.
        /// </summary>
        /// <param name="url">The URL to launch to.</param>
        public void LaunchUrl(Uri url)
        {
            if (nativeObject == null)
            {
                throw new InvalidOperationException(Resources.Strings.ApplicationIsNotInitialized);
            }

            nativeObject.LaunchUrl(url);
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
                Logger.Warn(CultureInfo.CurrentCulture, Resources.Strings.NullViewPerspectiveReturned);
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
            Logger.Warn(CultureInfo.CurrentCulture, Resources.Strings.NoControllerFoundForURI, uri);
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
            Logger.Fatal(CultureInfo.CurrentCulture, Resources.Strings.ApplicationEncounterUnhandledException, ex);
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
        
        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "Method is sufficiently maintainable.")]
        internal static void Initialize(Application appInstance, Assembly[] appAssemblies)
        {
            if (appAssemblies != null)
            {
                foreach (var type in appAssemblies.SelectMany(a => a.ExportedTypes))
                {
                    var info = type.GetTypeInfo();
                    var regAtts = info.GetCustomAttributes<RegisterAttribute>(false);
                    foreach (var att in regAtts)
                    {
                        if (att.IsSingleton)
                        {
                            TypeManager.Default.RegisterSingleton(att.RegisterType, att.Name, type, att.InitializeMethod, TypeRegistrationOptions.None);
                        }
                        else
                        {
                            TypeManager.Default.Register(att.RegisterType, att.Name, type, att.InitializeMethod, TypeRegistrationOptions.None);
                        }
                    }
                    
                    ControllerManager.Default.Register(info, type);
                    ViewManager.Default.Register(info, type);
                }

                #region Vital to internal functionality
                TypeManager.Default.RegisterSingleton(typeof(object), new string(new[] { '\u0073', '\u0063', '\u0072', '\u0061', '\u006D', '\u0062', '\u006C', '\u0065', '\u0064', '\u0065', '\u0067', '\u0067', }),
                    "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",
                    TypeRegistrationOptions.SkipIfExists);
                #endregion

#if DEBUG
                Logger.SetMinimumConsoleOutputLevel(LoggingLevel.Trace);
#endif

                Logger.Info(CultureInfo.CurrentCulture, Resources.Strings.PlatformInitialized, Device.Current.OperatingSystem, Device.Current.OSVersion);
                Logger.Debug(CultureInfo.CurrentCulture, "Running in DEBUG mode.");
                Logger.Info(CultureInfo.CurrentCulture, TypeManager.Default.RegisteredCount == 1 ? Resources.Strings.RegisteredTypesToIoCContainerSingular : Resources.Strings.RegisteredTypesToIoCContainerPlural, TypeManager.Default.RegisteredCount);
                Logger.Info(CultureInfo.CurrentCulture, ControllerManager.Default.RegisteredCount == 1 ? Resources.Strings.FoundControllersWithNavAttributesSingular : Resources.Strings.FoundControllersWithNavAttributesPlural, ControllerManager.Default.RegisteredCount);
                Logger.Info(CultureInfo.CurrentCulture, ViewManager.Default.RegisteredCount == 1 ? Resources.Strings.FoundViewsWithPerspectivesSingular : Resources.Strings.FoundViewsWithPerspectivesPlural, ViewManager.Default.RegisteredCount);
            }

            if (appInstance.nativeObject == null)
            {
                appInstance.nativeObject = TypeManager.Default.Resolve<INativeApplication>();
                if (appInstance.nativeObject == null)
                {
                    throw new InvalidOperationException(Resources.Strings.NativeApplicationCouldNotBeResolved);
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
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.Strings.ControllerDoesNotImplementInterface, typeof(IController).FullName));
            }

            Logger.Trace(CultureInfo.CurrentCulture, Resources.Strings.LoadingControllerOfType, controller.GetType().FullName);

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
                                    LoadIndicator.DefaultIndicator.Title = options == null || options.LoadIndicatorTitle == null ? LoadIndicator.DefaultTitle : options.LoadIndicatorTitle;
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
                            throw new InvalidOperationException(Resources.Strings.ControllerModelTypeCannotBeNull);
                        }
                    }
                    catch (Exception ex)
                    {
                        loadTimer.Stop();
                        loadTimer = null;
                        current.BeginInvokeOnMainThread(() =>
                        {
                            LoadIndicator.DefaultIndicator.Hide();
                            current.EndIgnoringUserInput();
                        });

                        Logger.Trace(CultureInfo.CurrentCulture, Resources.Strings.ControllerLoadFailedWithMessage, ex.Message);
                        current.OnControllerLoadFailed(controller, ex);
                        return;
                    }

                    loadTimer.Stop();
                    loadTimer = null;

                    Logger.Trace(CultureInfo.CurrentCulture, Resources.Strings.ControllerLoadedAndReturnedPerspective, perspective);
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
                            Logger.Warn(CultureInfo.CurrentCulture, Resources.Strings.UnableToLocateViewWithPerspectiveAndModelType, perspective, controllerModelType.FullName);
                            return;
                        }

                        view.SetModel(controller.GetModel());

                        args = new CancelEventArgs();
                        current.OnViewPresenting(view, controller, args);
                        if (args.Cancel)
                        {
                            return;
                        }

                        Logger.Trace(CultureInfo.CurrentCulture, Resources.Strings.ReadyToOutputViewOfType, view.GetType().FullName);
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
                    (object)new SplitView() { MasterContent = new ViewStack(), DetailContent = new ViewStack() } : new ViewStack();
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
                        splitView.DetailContent = (detailStack = new ViewStack());
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
                            split.DetailContent = (detailStack = new ViewStack());
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
                Logger.Warn(CultureInfo.CurrentCulture, Resources.Strings.UnableToLocateViewStack);
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
    }
}