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
using Prism.Systems;

namespace Prism
{
    /// <summary>
    /// Indicates that a class is a renderable view that can be looked up by a controller through the navigation system.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public sealed class NavigationViewAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the form factor that a device should be using for the view.
        /// If more than one view is matched to the perspective that is returned by a controller,
        /// this property will be compared to the current form factor in order to decide which view to render.
        /// </summary>
        public FormFactor FormFactor { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to reuse the same instance of the view for every output and render.
        /// </summary>
        public bool IsSingleton { get; set; }

        /// <summary>
        /// Gets the model type of the controller that will use this view.
        /// </summary>
        public Type ModelType { get; internal set; }

        /// <summary>
        /// Gets the view perspective that a controller will return when this view should be rendered.
        /// </summary>
        public string Perspective { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationViewAttribute"/> class.
        /// </summary>
        public NavigationViewAttribute()
            : this(string.Empty, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationViewAttribute"/> class.
        /// </summary>
        /// <param name="perspective">The view perspective that a controller will return when this view should be rendered.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="perspective"/> is <c>null</c>.</exception>
        public NavigationViewAttribute(string perspective)
            : this(perspective, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationViewAttribute"/> class.
        /// </summary>
        /// <param name="perspective">The view perspective that a controller will return when this view should be rendered.</param>
        /// <param name="modelType">The model type of the controller that will use this view.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="perspective"/> is <c>null</c>.</exception>
        public NavigationViewAttribute(string perspective, Type modelType)
        {
            if (perspective == null)
            {
                throw new ArgumentNullException(nameof(perspective));
            }

            Perspective = perspective;
            ModelType = modelType;
        }
    }
}
