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
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using Prism.Native;
using Prism.UI.Controls;

#if !DEBUG
using System.Diagnostics;
#endif

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
        /// Describes the <see cref="E:Popping"/> event.  This field is read-only.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "EventDescriptor is immutable.")]
        public static readonly EventDescriptor PoppingEvent = EventDescriptor.Create(nameof(Popping), typeof(TypedEventHandler<ViewStack, ViewStackPoppingEventArgs>), typeof(ViewStack));
        #endregion

        #region Property Descriptors
        /// <summary>
        /// Describes the <see cref="P:IsHeaderHidden"/> property.  This field is read-only.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "PropertyDescriptor is immutable.")]
        public static readonly PropertyDescriptor IsHeaderHiddenProperty = PropertyDescriptor.Create(nameof(IsHeaderHidden), typeof(bool), typeof(ViewStack), new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.AffectsMeasure));
        #endregion

        /// <summary>
        /// Occurs when a view is being popped off of the view stack.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification = "Event handler provides a strongly-typed sender for easier use.")]
        public event TypedEventHandler<ViewStack, ViewStackPoppingEventArgs> Popping;

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
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.Strings.TypeMustResolveToType, resolveType.FullName, typeof(INativeViewStack).FullName), nameof(resolveType));
            }

            nativeObject.Popping += (o, e) =>
            {
                var args = new ViewStackPoppingEventArgs(ObjectRetriever.GetAgnosticObject(e.View) as IView);
                OnPopping(args);
                e.Cancel = args.Cancel;
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
                throw new ArgumentOutOfRangeException(nameof(index), Resources.Strings.ValueCannotBeLessThanZero);
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
                throw new ArgumentException(Resources.Strings.ViewIsNotPartOfStack, nameof(view));
            }
            
            return nativeObject.PopToView(nativeView, animate)?.Select(v => ObjectRetriever.GetAgnosticObject(v)).OfType<IView>().ToArray();
        }

        /// <summary>
        /// Pushes the specified view onto the top of the stack.
        /// </summary>
        /// <param name="view">The view to push to the top of the stack.</param>
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
        /// <exception cref="ArgumentException">Thrown when <paramref name="view"/> is already on the stack.</exception>
        public void PushView(IView view, Animate animate)
        {
            var nativeView = ObjectRetriever.GetNativeObject(view);
            if (nativeObject.Views.Contains(nativeView))
            {
                throw new ArgumentException(Resources.Strings.CannotPushExistingView, nameof(view));
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
                throw new ArgumentException(Resources.Strings.ViewIsNotPartOfStack, nameof(oldView));
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

            var currentView = CurrentView as Visual;
            if (currentView != null)
            {
                var offset = nativeObject.IsHeaderHidden ? new Thickness() : Window.Current.Width > Window.Current.Height ?
                    SystemParameters.ContentViewHeaderOffsetLandscape : SystemParameters.ContentViewHeaderOffsetPortrait;

                frame.X += offset.Left;
                frame.Y += offset.Top;
                frame.Width -= (offset.Left + offset.Right);
                frame.Height -= (offset.Top + offset.Bottom);
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
            var currentView = CurrentView as Visual;
            if (currentView != null)
            {
                var offset = nativeObject.IsHeaderHidden ? new Thickness() : Window.Current.Width > Window.Current.Height ?
                    SystemParameters.ContentViewHeaderOffsetLandscape : SystemParameters.ContentViewHeaderOffsetPortrait;

                currentView.Measure(new Size(constraints.Width - (offset.Left + offset.Right), constraints.Height - (offset.Top + offset.Bottom)));
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
    }
}
