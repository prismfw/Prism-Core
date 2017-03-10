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

namespace Prism.Data
{
    /// <summary>
    /// Provides data for the <see cref="MultiBinding.BindingFailed"/> event.
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Multi", Justification = "Valid prefix meaning 'multiple'.")]
    public class MultiBindingFailedEventArgs : ErrorEventArgs
    {
        /// <summary>
        /// Gets the <see cref="Binding"/> instance within the multi-binding that encountered the error.
        /// </summary>
        public Binding Binding { get; }

        /// <summary>
        /// Gets or sets a value indicating whether to ignore the error.
        /// If set to <c>true</c>, the data binding will remain active and continue to perform updates; otherwise, the binding will be deactivated.
        /// Only errors that occur during updates can be ignored; errors that occur during path resolution will always cause deactivation.
        /// </summary>
        public bool Ignore { get; set; }

        /// <summary>
        /// Gets the target object of the data binding.
        /// </summary>
        public object Target { get; }

        /// <summary>
        /// Gets the <see cref="PropertyPath"/> describing the target property of the data binding.
        /// </summary>
        public PropertyPath TargetProperty { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiBindingFailedEventArgs"/> class.
        /// </summary>
        /// <param name="target">The target object of the data binding.</param>
        /// <param name="targetProperty">The <see cref="PropertyPath"/> describing the target property of the data binding.</param>
        /// <param name="binding">The <see cref="Binding"/> instance within the multi-binding that encountered the error.</param>
        /// <param name="exception">The exception that caused the event.</param>
        public MultiBindingFailedEventArgs(object target, PropertyPath targetProperty, Binding binding, Exception exception)
            : base(exception)
        {
            Target = target;
            TargetProperty = targetProperty;
            Binding = binding;
        }
    }
}
