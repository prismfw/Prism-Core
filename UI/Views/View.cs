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
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Prism.Native;

namespace Prism.UI
{
    /// <summary>
    /// Represents a base class for platform-agnostic views.  This class is abstract.
    /// </summary>
    public abstract class View : Visual, IView
    {
        /// <summary>
        /// Gets or sets the model containing the information that is displayed by the view.
        /// </summary>
        public object Model { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="View"/> class.
        /// </summary>
        /// <param name="resolveType">The type to pass to the IoC container in order to resolve the native object.</param>
        /// <param name="resolveName">An optional name to use when resolving the native object.</param>
        /// <param name="resolveParameters">Any parameters to pass along to the constructor of the resolve type.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="resolveType"/> is <c>null</c>.</exception>
        protected View(Type resolveType, string resolveName, params ResolveParameter[] resolveParameters)
            : base(resolveType, resolveName, resolveParameters)
        {
        }

        /// <summary>
        /// Configures the UI for the view in preparation for presentation.
        /// This is automatically called when the view is loaded from a controller.
        /// Override this method in a derived class for customized behavior.
        /// </summary>
#pragma warning disable 1998
        public virtual async Task ConfigureUIAsync() { }
#pragma warning restore 1998

        /// <summary>
        /// Initiates a navigation from this view to the specified URI.
        /// Be sure to apply a <see cref="NavigationControllerAttribute"/> to the controller type that should be associated with the URI.
        /// </summary>
        /// <param name="uri">A <see cref="string"/> representing the URI to navigate to.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="uri"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when the matching controller does not implement the <see cref="IController"/> interface.</exception>
        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", Justification = "Value may contain formatting that is nonstandard to System.Uri.")]
        public void Navigate(string uri)
        {
            Application.Navigate(uri, null, this);
        }

        /// <summary>
        /// Initiates a navigation from this view to the specified URI.
        /// Be sure to apply a <see cref="NavigationControllerAttribute"/> to the controller type that should be associated with the URI.
        /// </summary>
        /// <param name="uri">A <see cref="string"/> representing the URI to navigate to.</param>
        /// <param name="options">Additional options to customize the navigation.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="uri"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when the matching controller does not implement the <see cref="IController"/> interface.</exception>
        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", Justification = "Value may contain formatting that is nonstandard to System.Uri.")]
        public void Navigate(string uri, NavigationOptions options)
        {
            Application.Navigate(uri, options, this);
        }

        /// <summary>
        /// Initiates a navigation from this view to the specified controller type.
        /// </summary>
        /// <param name="controllerType">The type of the controller to navigate to.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="controllerType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="controllerType"/> does not implement the <see cref="IController"/> interface.</exception>
        public void Navigate(Type controllerType)
        {
            Application.Navigate(controllerType, null, this);
        }

        /// <summary>
        /// Initiates a navigation from this view to the specified controller type.
        /// </summary>
        /// <param name="controllerType">The type of the controller to navigate to.</param>
        /// <param name="options">Additional options to customize the navigation.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="controllerType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="controllerType"/> does not implement the <see cref="IController"/> interface.</exception>
        public void Navigate(Type controllerType, NavigationOptions options)
        {
            Application.Navigate(controllerType, options, this);
        }

        /// <summary>
        /// Called when this instance is ready to arrange its children.
        /// Derived classes wishing to define their own arrangement behavior should override <see cref="M:ArrangeOverride"/>.
        /// </summary>
        /// <param name="frame">The final rendering frame in which this instance should arrange its children.</param>
        protected sealed override void ArrangeCore(Rectangle frame)
        {
            var size = ArrangeOverride(frame.Size);
            var visual = ObjectRetriever.GetNativeObject(this) as INativeVisual;
            if (visual != null)
            {
                visual.Frame = new Rectangle(frame.TopLeft, size);
            }
        }

        /// <summary>
        /// Called when this instance is ready to arrange its children and returns the final rendering size of the object.
        /// </summary>
        /// <param name="constraints">The width and height that this instance should not exceed.</param>
        /// <returns>The final rendering size of the object as a <see cref="Size"/> instance.</returns>
        protected virtual Size ArrangeOverride(Size constraints)
        {
            return constraints;
        }

        /// <summary>
        /// Called when this instance is ready to be measured and returns the desired size of the object.
        /// Derived classes wishing to define their own measurement behavior should override <see cref="M:MeasureOverride"/>.
        /// </summary>
        /// <param name="constraints">The width and height that this instance should not exceed.</param>
        /// <returns>The desired size of the object as a <see cref="Size"/> instance.</returns>
        protected sealed override Size MeasureCore(Size constraints)
        {
            constraints = MeasureOverride(constraints);

            Popup parent = null;
            if (double.IsInfinity(constraints.Width))
            {
                parent = VisualTreeHelper.GetParent<Popup>(this);
                if (parent == null || parent.PresentationStyle == PopupPresentationStyle.FullScreen)
                {
                    constraints.Width = Window.Current.Width;
                }
                else
                {
                    constraints.Width = parent.PresentationStyle == PopupPresentationStyle.Custom && !double.IsNaN(parent.Width) ?
                        parent.Width : ((Size)parent.FindResource(SystemResources.PopupSizeKey)).Width;
                }
            }

            if (double.IsInfinity(constraints.Height))
            {
                if (parent == null)
                {
                    parent = VisualTreeHelper.GetParent<Popup>(this);
                }

                if (parent == null || parent.PresentationStyle == PopupPresentationStyle.FullScreen)
                {
                    constraints.Height = Window.Current.Height;
                }
                else
                {
                    constraints.Height = parent.PresentationStyle == PopupPresentationStyle.Custom && !double.IsNaN(parent.Height) ?
                        parent.Height : ((Size)parent.FindResource(SystemResources.PopupSizeKey)).Height;
                }
            }

            return constraints;
        }

        /// <summary>
        /// Called when this instance is ready to be measured and returns the desired size of the object.
        /// </summary>
        /// <param name="constraints">The width and height that this instance should not exceed.</param>
        /// <returns>The desired size of the object as a <see cref="Size"/> instance.</returns>
        protected virtual Size MeasureOverride(Size constraints)
        {
            return constraints;
        }

        [SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Accessible via Model property.")]
        object IView.GetModel()
        {
            return Model;
        }

        [SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Accessible via Model property.")]
        void IView.SetModel(object model)
        {
            Model = model;
        }
    }
}
