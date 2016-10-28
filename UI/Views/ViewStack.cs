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
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using Prism.Native;
using Prism.Resources;
using Prism.UI.Controls;

namespace Prism.UI
{
    /// <summary>
    /// Represents a navigable stack of <see cref="IView"/> instances.
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix", Justification = "Class behavior is consistent with that of a stack.")]
    public class ViewStack : Visual
    {
        #region Event Descriptors
        /// <summary>
        /// Gets an <see cref="EventDescriptor"/> describing the <see cref="E:Popping"/> event.
        /// </summary>
        public static EventDescriptor PoppingEvent { get; } = EventDescriptor.Create(nameof(Popping), typeof(TypedEventHandler<ViewStack, ViewStackPoppingEventArgs>), typeof(ViewStack));

        /// <summary>
        /// Gets an <see cref="EventDescriptor"/> describing the <see cref="E:ViewChanged"/> event.
        /// </summary>
        public static EventDescriptor ViewChangedEvent { get; } = EventDescriptor.Create(nameof(ViewChanged), typeof(TypedEventHandler<ViewStack>), typeof(ViewStack));

        /// <summary>
        /// Gets an <see cref="EventDescriptor"/> describing the <see cref="E:ViewChanging"/> event.
        /// </summary>
        public static EventDescriptor ViewChangingEvent { get; } = EventDescriptor.Create(nameof(ViewChanging), typeof(TypedEventHandler<ViewStack, ViewStackViewChangingEventArgs>), typeof(ViewStack));
        #endregion

        #region Property Descriptors
        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:BackButtonState"/> property.
        /// </summary>
        public static PropertyDescriptor BackButtonStateProperty { get; } = PropertyDescriptor.Create(nameof(BackButtonState), typeof(BackButtonState), typeof(ViewStack));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:IsHeaderHidden"/> property.
        /// </summary>
        public static PropertyDescriptor IsHeaderHiddenProperty { get; } = PropertyDescriptor.Create(nameof(IsHeaderHidden), typeof(bool), typeof(ViewStack), new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.AffectsMeasure));
        #endregion

        /// <summary>
        /// Occurs when a view is being popped off of the view stack.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification = "Event handler provides a strongly-typed sender for easier use.")]
        public event TypedEventHandler<ViewStack, ViewStackPoppingEventArgs> Popping;

        /// <summary>
        /// Occurs when the current view of the view stack has changed.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification = "Event handler provides a strongly-typed sender for easier use.")]
        public event TypedEventHandler<ViewStack> ViewChanged;

        /// <summary>
        /// Occurs when the current view of the view stack is being replaced by a different view.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification = "Event handler provides a strongly-typed sender for easier use.")]
        public event TypedEventHandler<ViewStack, ViewStackViewChangingEventArgs> ViewChanging;

        /// <summary>
        /// Gets or sets the state of the view stack's back button.
        /// </summary>
        public BackButtonState BackButtonState
        {
            get { return backButtonState; }
            set
            {
                if (value != backButtonState)
                {
                    backButtonState = value;
                    UpdateBackButtonState();
                    OnPropertyChanged(BackButtonStateProperty);
                }
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private BackButtonState backButtonState;

        /// <summary>
        /// Gets the view that is currently on top of the stack.
        /// </summary>
        public IView CurrentView
        {
            get { return ObjectRetriever.GetAgnosticObject(nativeObject.CurrentView) as IView; }
        }

        /// <summary>
        /// Gets the header for the view stack.
        /// </summary>
        public ViewStackHeader Header { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the header is hidden.
        /// </summary>
        public bool IsHeaderHidden
        {
            get { return nativeObject.IsHeaderHidden; }
            set { nativeObject.IsHeaderHidden = value; }
        }

        /// <summary>
        /// Gets a collection of the views that are currently a part of the stack.
        /// </summary>
        public IEnumerable<IView> Views
        {
            get
            {
                var views = nativeObject.Views;
                return views == null ? null : views.Select(v => ObjectRetriever.GetAgnosticObject(v)).OfType<IView>();
            }
        }

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private readonly INativeViewStack nativeObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewStack"/> class.
        /// </summary>
        public ViewStack()
            : this(typeof(INativeViewStack), null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewStack"/> class.
        /// </summary>
        /// <param name="resolveType">The type to pass to the IoC container in order to resolve the native object.</param>
        /// <param name="resolveName">An optional name to use when resolving the native object.</param>
        /// <param name="resolveParameters">Any parameters to pass along to the constructor of the resolve type.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="resolveType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="resolveType"/> does not resolve to an <see cref="INativeViewStack"/> instance.</exception>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", Justification = "resolveType is validated in base constructor.")]
        protected ViewStack(Type resolveType, string resolveName, params ResolveParameter[] resolveParameters)
            : base(resolveType, resolveName, resolveParameters)
        {
            nativeObject = ObjectRetriever.GetNativeObject(this) as INativeViewStack;
            if (nativeObject == null)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Strings.TypeMustResolveToType, resolveType.FullName, typeof(INativeViewStack).FullName), nameof(resolveType));
            }

            nativeObject.Popping += (o, e) =>
            {
                var args = new ViewStackPoppingEventArgs(ObjectRetriever.GetAgnosticObject(e.View) as IView);
                OnPopping(args);
                e.Cancel = args.Cancel;
            };

            nativeObject.ViewChanged += (o, e) =>
            {
                OnViewChanged(e);
            };

            nativeObject.ViewChanging += (o, e) =>
            {
                OnViewChanging(new ViewStackViewChangingEventArgs(ObjectRetriever.GetAgnosticObject(e.OldView) as IView, ObjectRetriever.GetAgnosticObject(e.NewView) as IView));

                var splitView = Parent as SplitView;
                if (splitView != null && splitView.MasterContent == this)
                {
                    splitView.OnMasterContentChanged();
                }
                else
                {
                    var tabView = Parent as TabbedSplitView;
                    if (tabView != null && tabView.SelectedTabItem?.Content == this)
                    {
                        tabView.OnMasterContentChanged();
                    }
                }

                UpdateBackButtonState();
            };

            Header = new ViewStackHeader(nativeObject.Header);
            IsHeaderHidden = false;
        }

        /// <summary>
        /// Inserts the specified <see cref="IView"/> instance into the stack at the specified index.
        /// </summary>
        /// <param name="view">The view to be inserted.</param>
        /// <param name="index">The zero-based index of the location in the stack in which to insert the view.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="view"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="index"/> is less than zero.</exception>
        public void InsertView(IView view, int index)
        {
            InsertView(view, index, Animate.Default);
        }

        /// <summary>
        /// Inserts the specified <see cref="IView"/> instance into the stack at the specified index.
        /// </summary>
        /// <param name="view">The view to be inserted.</param>
        /// <param name="index">The zero-based index of the location in the stack in which to insert the view.</param>
        /// <param name="animate">Whether to use any system-defined transition animation.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="view"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="index"/> is less than zero.</exception>
        public void InsertView(IView view, int index, Animate animate)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            if (index < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index), Strings.ValueCannotBeLessThanZero);
            }

            nativeObject.InsertView(ObjectRetriever.GetNativeObject(view), index, animate);
        }

        /// <summary>
        /// Removes the top view from the stack.
        /// </summary>
        /// <returns>The view that was removed from the stack as an <see cref="IView"/> instance.</returns>
        public IView PopView()
        {
            return ObjectRetriever.GetAgnosticObject(nativeObject.PopView(Animate.Default)) as IView;
        }

        /// <summary>
        /// Removes the top view from the stack.
        /// </summary>
        /// <param name="animate">Whether to use any system-defined transition animation.</param>
        /// <returns>The view that was removed from the stack as an <see cref="IView"/> instance.</returns>
        public IView PopView(Animate animate)
        {
            return ObjectRetriever.GetAgnosticObject(nativeObject.PopView(animate)) as IView;
        }

        /// <summary>
        /// Removes every view from the stack except for the root view.
        /// </summary>
        /// <returns>An <see cref="Array"/> containing the views that were removed from the stack.</returns>
        public IView[] PopToRoot()
        {
            return PopToRoot(Animate.Default);
        }

        /// <summary>
        /// Removes every view from the stack except for the root view.
        /// </summary>
        /// <param name="animate">Whether to use any system-defined transition animation.</param>
        /// <returns>An <see cref="Array"/> containing the views that were removed from the stack.</returns>
        public IView[] PopToRoot(Animate animate)
        {
            return nativeObject.PopToRoot(animate)?.Select(v => ObjectRetriever.GetAgnosticObject(v)).OfType<IView>().ToArray();
        }

        /// <summary>
        /// Removes from the stack every view on top of the specified view.
        /// </summary>
        /// <param name="view">The view to pop to.</param>
        /// <returns>An <see cref="Array"/> containing the views that were removed from the stack.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="view"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="view"/> is not a part of the stack.</exception>
        public IView[] PopToView(IView view)
        {
            return PopToView(view, Animate.Default);
        }

        /// <summary>
        /// Removes from the stack every view on top of the specified view.
        /// </summary>
        /// <param name="view">The view to pop to.</param>
        /// <param name="animate">Whether to use any system-defined transition animation.</param>
        /// <returns>An <see cref="Array"/> containing the views that were removed from the stack.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="view"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="view"/> is not a part of the stack.</exception>
        public IView[] PopToView(IView view, Animate animate)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            var nativeView = ObjectRetriever.GetNativeObject(view);
            if (!nativeObject.Views.Contains(nativeView))
            {
                throw new ArgumentException(Strings.ViewIsNotPartOfStack, nameof(view));
            }
            
            return nativeObject.PopToView(nativeView, animate)?.Select(v => ObjectRetriever.GetAgnosticObject(v)).OfType<IView>().ToArray();
        }

        /// <summary>
        /// Pushes the specified view onto the top of the stack.
        /// </summary>
        /// <param name="view">The view to push to the top of the stack.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="view"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="view"/> is already on the stack.</exception>
        public void PushView(IView view)
        {
            PushView(view, Animate.Default);
        }

        /// <summary>
        /// Pushes the specified view onto the top of the stack.
        /// </summary>
        /// <param name="view">The view to push to the top of the stack.</param>
        /// <param name="animate">Whether to use any system-defined transition animation.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="view"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="view"/> is already on the stack.</exception>
        public void PushView(IView view, Animate animate)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            var nativeView = ObjectRetriever.GetNativeObject(view);
            if (nativeObject.Views.Contains(nativeView))
            {
                throw new ArgumentException(Strings.CannotPushExistingView, nameof(view));
            }

            nativeObject.PushView(nativeView, animate);
        }

        /// <summary>
        /// Replaces a view that is currently on the stack with the specified view.
        /// </summary>
        /// <param name="oldView">The view to be replaced.</param>
        /// <param name="newView">The view with which to replace the old view.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="oldView"/> is <c>null</c> -or- when <paramref name="newView"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="oldView"/> is not a part of the stack.</exception>
        public void ReplaceView(IView oldView, IView newView)
        {
            ReplaceView(oldView, newView, Animate.Default);
        }

        /// <summary>
        /// Replaces a view that is currently on the stack with the specified view.
        /// </summary>
        /// <param name="oldView">The view to be replaced.</param>
        /// <param name="newView">The view with which to replace the old view.</param>
        /// <param name="animate">Whether to use any system-defined transition animation.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="oldView"/> is <c>null</c> -or- when <paramref name="newView"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="oldView"/> is not a part of the stack.</exception>
        public void ReplaceView(IView oldView, IView newView, Animate animate)
        {
            if (oldView == null)
            {
                throw new ArgumentNullException(nameof(oldView));
            }

            if (newView == null)
            {
                throw new ArgumentNullException(nameof(newView));
            }

            var nativeView = ObjectRetriever.GetNativeObject(oldView);
            if (!nativeObject.Views.Contains(nativeView))
            {
                throw new ArgumentException(Strings.ViewIsNotPartOfStack, nameof(oldView));
            }

            nativeObject.ReplaceView(nativeView, ObjectRetriever.GetNativeObject(newView), animate);
        }

        /// <summary>
        /// Called when this instance is ready to arrange its children.
        /// </summary>
        /// <param name="frame">The final rendering frame in which this instance should arrange its children.</param>
        protected sealed override void ArrangeCore(Rectangle frame)
        {
            nativeObject.Frame = frame;
            Header.Arrange(new Rectangle(new Point(), Header.DesiredSize));

            var currentView = CurrentView as Visual;
            if (currentView != null)
            {
                var offset = nativeObject.IsHeaderHidden || Header.IsInset ? Size.Empty : Header.RenderSize;
                
                frame.Width = Math.Max(frame.Width, 0);
                frame.Height = Math.Max(frame.Height - offset.Height, 0);
                currentView.Arrange(frame);
            }
        }

        /// <summary>
        /// Called when this instance is ready to be measured and returns the desired size of the object.
        /// </summary>
        /// <param name="constraints">The width and height that this instance should not exceed.</param>
        /// <returns>The desired size of the object as a <see cref="Size"/> instance.</returns>
        protected sealed override Size MeasureCore(Size constraints)
        {
            Header.Measure(constraints);

            var currentView = CurrentView as Visual;
            if (currentView != null)
            {
                var offset = nativeObject.IsHeaderHidden || Header.IsInset ? Size.Empty : Header.DesiredSize;

                currentView.Measure(new Size(Math.Max(constraints.Width, 0), Math.Max(constraints.Height - offset.Height, 0)));
                return new Size(Math.Max(currentView.DesiredSize.Width, offset.Height), currentView.DesiredSize.Height + offset.Height);
            }

            if (double.IsInfinity(constraints.Width))
            {
                constraints.Width = int.MaxValue;
            }

            if (double.IsInfinity(constraints.Height))
            {
                constraints.Height = int.MaxValue;
            }

            return constraints;
        }

        /// <summary>
        /// Called when a view is being popped off of the view stack and raises the <see cref="Popping"/> event.
        /// </summary>
        /// <param name="e">The event arguments containing the event details.</param>
        protected virtual void OnPopping(ViewStackPoppingEventArgs e)
        {
            Popping?.Invoke(this, e);
        }

        /// <summary>
        /// Called when the current view of the view stack has changed and raises the <see cref="ViewChanged"/> event.
        /// </summary>
        /// <param name="e">The event arguments containing the event details.</param>
        protected virtual void OnViewChanged(EventArgs e)
        {
            ViewChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Called when the current view of the view stack is being replaced by a different view and raises the <see cref="ViewChanging"/> event.
        /// </summary>
        /// <param name="e">The event arguments containing the event details.</param>
        protected virtual void OnViewChanging(ViewStackViewChangingEventArgs e)
        {
            ViewChanging?.Invoke(this, e);
        }

        internal void UpdateBackButtonState()
        {
            if (backButtonState == BackButtonState.Auto)
            {
                var currentView = CurrentView as IViewStackChild;
                var nextView = Views?.LastOrDefault(v => v != currentView) as IViewStackChild;
                nativeObject.IsBackButtonEnabled = (currentView == null || currentView.IsBackButtonEnabled) && (nextView != null && nextView.IsValidBackTarget);
            }
            else
            {
                nativeObject.IsBackButtonEnabled = backButtonState == BackButtonState.Enabled;
            }
        }
    }
}
