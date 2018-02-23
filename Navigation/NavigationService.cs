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
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Prism.Resources;
using Prism.Systems;
using Prism.UI;
using Prism.UI.Controls;
using Prism.Utilities;

namespace Prism
{
    internal static class NavigationService
    {
        private static bool isNavigating;
        private static ConditionalWeakTable<IView, Dictionary<string, object>> navParameters = new ConditionalWeakTable<IView, Dictionary<string, object>>();

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
                Application.Current.OnControllerNavigationFailed(uri);
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

            if (fromView == null)
            {
                LoadController(controller, options, context, fromView);
            }
            else
            {
                var syncContext = SynchronizationContext.Current;
                Application.Current.BeginInvokeOnMainThread(() =>
                {
                    context.OriginatingPanes |= GetViewPane(fromView);
                    ApplyControlParameters(context, fromView);
                    syncContext.Post((state) =>
                    {
                        var array = (object[])state;
                        LoadController(array[0] as IController, (NavigationOptions)array[1], (NavigationContext)array[2], (IView)array[3]);
                    }, new object[] { controller, options, context, fromView });
                });
            }
        }

        public static void Navigate(Type controllerType, NavigationOptions options, IView fromView)
        {
            if (controllerType == null)
            {
                throw new ArgumentNullException(nameof(controllerType));
            }

            Logger.Trace(CultureInfo.CurrentCulture, Strings.NavigatingToControllerType, controllerType.FullName);

            var context = new NavigationContext(controllerType.FullName);

            string nullValue;
            if (fromView == null)
            {
                LoadController(ControllerManager.Default.Resolve(controllerType, null, out nullValue), options, context, fromView);
            }
            else
            {
                var syncContext = SynchronizationContext.Current;
                Application.Current.BeginInvokeOnMainThread(() =>
                {
                    context.OriginatingPanes |= GetViewPane(fromView);
                    ApplyControlParameters(context, fromView);
                    syncContext.Post((state) =>
                    {
                        var array = (object[])state;
                        LoadController(array[0] as IController, (NavigationOptions)array[1], (NavigationContext)array[2], (IView)array[3]);
                    }, new object[] { ControllerManager.Default.Resolve(controllerType, null, out nullValue), options, context, fromView });
                });
            }
        }

        internal static void SetControlParameter(IView parentView, Control control)
        {
            navParameters.GetOrCreateValue(parentView)[control.ParameterId] = control.ParameterValueProperty.GetValue(control, null);
        }

        private static void ApplyControlParameters(NavigationContext context, IView view)
        {
            Dictionary<string, object> parameters;
            if (navParameters.TryGetValue(view, out parameters))
            {
                foreach (var kvp in parameters)
                {
                    context.Parameters[kvp.Key] = kvp.Value;
                }
            }

            var parents = new List<object>() { view };
            while (parents.Count > 0)
            {
                var parent = parents[0];
                parents.RemoveAt(0);

                int count = VisualTreeHelper.GetChildrenCount(parent);
                for (int i = 0; i < count; i++)
                {
                    var child = VisualTreeHelper.GetChild(parent, i);
                    var control = child as Control;
                    if (control?.ParameterId != null)
                    {
                        context.Parameters[control.ParameterId] = control.ParameterValueProperty.GetValue(control, null);
                    }

                    if (VisualTreeHelper.GetChildrenCount(child) > 0)
                    {
                        parents.Add(child);
                    }
                }
            }
        }

        private static Panes GetViewPane(IView view)
        {
            var parent = VisualTreeHelper.GetParent(view, (o) => o is Window || o is SplitView || o is Popup);
            if (parent != null)
            {
                if (parent is Window)
                {
                    return Panes.Master;
                }

                if (parent is Popup || VisualTreeHelper.GetParent<Popup>(parent) != null)
                {
                    return Panes.Popup;
                }

                var splitView = parent as SplitView;
                if (splitView != null)
                {
                    return view == splitView.DetailContent || VisualTreeHelper.GetParent(view, (o) => o == splitView.DetailContent) != null ?
                        Panes.Detail : Panes.Master;
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

            var current = Application.Current;
            lock (current)
            {
                if (options != null && options.Parameters != null)
                {
                    foreach (var kvp in options.Parameters)
                    {
                        context.Parameters[kvp.Key] = kvp.Value;
                    }
                }

                if (isNavigating)
                {
                    context.OriginatingPanes |= current.LastNavigationContext.OriginatingPanes;
                }

                isNavigating = true;
                current.LastNavigationContext = context;

                var args = new CancelEventArgs();
                current.OnControllerLoading(controller, context, fromView, args);
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

                        Logger.Warn(CultureInfo.CurrentCulture, Strings.ControllerLoadFailedWithMessage, controller.GetType().FullName, ex.Message);
                        current.OnControllerLoadFailed(controller, ex);
                        return;
                    }

                    loadTimer?.Stop();
                    loadTimer = null;

                    Logger.Trace(CultureInfo.CurrentCulture, Strings.ControllerLoadedAndReturnedPerspective, controller.GetType().FullName, perspective);
                    current.OnControllerLoaded(controller, context, fromView, perspective);

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

                        navParameters.Remove(view);

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
            // Navigating to a tab view or a split view is interpretted as wanting to reset the view hierarchy.
            // If the view is desired elsewhere, it will have to be placed manually.
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
                    masterStack = GetSplitContent(splitView, true) as ViewStack;
                    detailStack = GetSplitContent(splitView, false) as ViewStack;
                }

                var tabView = Window.Current.Content as TabView;
                if (tabView != null)
                {
                    masterStack = GetTabContent(tabView, true) as ViewStack;
                    detailStack = GetTabContent(tabView, false) as ViewStack;
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

            if ((detailStack != masterStack && PopToView(detailStack, view)) || PopToView(masterStack, view))
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

            if (masterStack != null && (!masterStack.Views.Any() || detailStack == null || detailStack == masterStack ||
                (preferredPanes.HasFlag(Panes.Master) && (!preferredPanes.HasFlag(Panes.Detail) || detailStack.Views.Count() <= 1))))
            {
                if (detailStack != masterStack)
                {
                    detailStack?.PopToRoot(Animate.Off);
                }

                masterStack.PushView(view, Animate.Default);
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

        private static object GetSplitContent(SplitView splitView, bool forMasterPane)
        {
            if (splitView == null)
            {
                return null;
            }

            if (forMasterPane)
            {
                if (splitView.MasterContent == null)
                {
                    // If no master content has been set, create a view stack to act as the content.
                    return (splitView.MasterContent = new ViewStack());
                }

                return GetTabContent(splitView.MasterContent as TabView, forMasterPane) ?? splitView.MasterContent;
            }

            if (splitView.DetailContent == null)
            {
                // If no detail content has been set, create a view stack to act as the content.
                var stack = new ViewStack();
                stack.PushView(new ContentView() { IsValidBackTarget = false });
                return (splitView.DetailContent = stack);
            }

            return GetTabContent(splitView.DetailContent as TabView, forMasterPane) ?? splitView.DetailContent;
        }

        private static object GetTabContent(TabView tabView, bool forMasterPane)
        {
            if (tabView == null || tabView.SelectedIndex < 0 || tabView.SelectedIndex >= tabView.TabItems.Count)
            {
                return null;
            }

            var tabItem = tabView.TabItems[tabView.SelectedIndex];
            if (tabItem.Content == null)
            {
                // If the current tab item does not have content, create a view stack to act as the content.
                return (tabItem.Content = new ViewStack());
            }

            return GetSplitContent(tabItem.Content as SplitView, forMasterPane) ?? tabItem.Content;
        }
    }
}
