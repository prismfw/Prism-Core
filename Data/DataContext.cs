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


using System.Diagnostics.CodeAnalysis;

namespace Prism.Data
{
    /// <summary>
    /// Defines an object that describes a context for data binding.
    /// The context is used as the default Source value when binding to this object or any of its children.
    /// </summary>
    public interface IDataContext
    {
        /// <summary>
        /// Occurs when the value of the <see cref="P:DataContext"/> property has changed.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification = "Event handler provides a strongly-typed sender for easier use.")]
        event TypedEventHandler<IDataContext, DataContextChangedEventArgs> DataContextChanged;

        /// <summary>
        /// Gets or sets the object to use as the default Source value when binding to this instance or any of its children.
        /// </summary>
        object DataContext { get; set; }
    }
}
