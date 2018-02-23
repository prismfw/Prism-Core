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
    /// Defines a renderable view that can be constructed from either a platform-specific object or a platform-agnostic object (Prism.UI).
    /// </summary>
    public interface IView
    {
        /// <summary>
        /// Configures the UI for the view in preparation for presentation.
        /// This is automatically called when the view is loaded from a controller.
        /// </summary>
        Task ConfigureUIAsync();

        /// <summary>
        /// Gets the model for the view.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Property could conflict with property of generic type.")]
        object GetModel();

        /// <summary>
        /// Sets the model for the view.
        /// </summary>
        /// <param name="model">The model to associate with the view.</param>
        /// <exception cref="InvalidCastException">Thrown if a model of the wrong type is set.</exception>
        void SetModel(object model);
    }

    /// <summary>
    /// Defines a renderable view that displays models of type <typeparamref name="T"/> and can be constructed from either a platform-specific object or a platform-agnostic object (Prism.UI).
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="P:Model"/>.</typeparam>
    public interface IView<T> : IView
    {
        /// <summary>
        /// Gets or sets the model containing the information that is displayed by the view.
        /// </summary>
        T Model { get; set; }
    }
}