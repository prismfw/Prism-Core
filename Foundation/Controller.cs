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
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Prism
{
    /// <summary>
    /// Defines a navigation controller for creating and populating data models.
    /// </summary>
    public interface IController
    {
        /// <summary>
        /// Gets the type of the model for the controller.
        /// </summary>
        Type ModelType { get; }

        /// <summary>
        /// Gets the model for the controller.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Property could conflict with property of generic type.")]
        object GetModel();

        /// <summary>
        /// Loads the controller with the specified <see cref="NavigationContext"/>.
        /// </summary>
        /// <param name="context">The context describing the navigation that took place.</param>
        /// <returns>
        /// A <see cref="string"/> representing the view perspective of the view to render.
        /// Be sure to apply a <see cref="NavigationViewAttribute"/> to each view to ensure that the framework can find it.
        /// </returns>
        Task<string> LoadAsync(NavigationContext context);
    }

    /// <summary>
    /// Represents a navigation controller for creating and populating data models. This class is abstract.
    /// </summary>
    public abstract class Controller<T> : IController
    {
        /// <summary>
        /// Gets or sets the model containing the information that is set by the controller.
        /// </summary>
        public T Model { get; set; }

        /// <summary>
        /// Gets the type of the model used by the controller.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Accessible via Model property.")]
        Type IController.ModelType { get { return typeof(T); } }

        /// <summary>
        /// Loads the controller with the specified <see cref="NavigationContext"/>.
        /// </summary>
        /// <param name="context">The context describing the navigation that took place.</param>
        /// <returns>
        /// A <see cref="string"/> representing the view perspective of the view to render.
        /// Be sure to apply a <see cref="NavigationViewAttribute"/> to each view to ensure that the framework can find it.
        /// </returns>
        public abstract Task<string> LoadAsync(NavigationContext context);

        /// <summary>
        /// Gets the model for the controller.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Accessible via Model property.")]
        object IController.GetModel() { return Model; }
    }
}