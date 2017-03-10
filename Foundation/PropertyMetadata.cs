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
using System.Diagnostics;

namespace Prism
{
    /// <summary>
    /// Represents metadata that is to be applied to a property via a <see cref="PropertyDescriptor"/>.
    /// </summary>
    public class PropertyMetadata
    {
        /// <summary>
        /// Gets or sets a value indicating whether the property defaults to a two-way binding when it is the target of a data binding.
        /// </summary>
        public bool BindsTwoWayByDefault
        {
            get { return bindsTwoWayByDefault ?? false; }
            set
            {
                if (IsSealed)
                {
                    throw new InvalidOperationException(Resources.Strings.PropertyMetadataHasBeenSealed);
                }

                bindsTwoWayByDefault = value;
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool? bindsTwoWayByDefault;

        /// <summary>
        /// Gets a value indicating whether this instance has been sealed and can no longer be modified.
        /// </summary>
        protected internal bool IsSealed { get; internal set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyMetadata"/> class.
        /// </summary>
        /// <param name="options">The options that specify the behavioral characteristics of the property.</param>
        public PropertyMetadata(PropertyMetadataOptions options)
        {
            if ((options & PropertyMetadataOptions.BindsTwoWayByDefault) != 0)
            {
                bindsTwoWayByDefault = true;
            }
        }

        /// <summary>
        /// Merges this metadata with the base metadata.
        /// </summary>
        /// <param name="baseMetadata">The base metadata to merge with the values of this instance.</param>
        /// <param name="descriptor">A <see cref="PropertyDescriptor"/> describing the property to which the metadata will be applied.</param>
        protected internal virtual void Merge(PropertyMetadata baseMetadata, PropertyDescriptor descriptor)
        {
            if (IsSealed || baseMetadata == null)
            {
                return;
            }

            if (!bindsTwoWayByDefault.HasValue)
            {
                bindsTwoWayByDefault = baseMetadata.bindsTwoWayByDefault;
            }
        }

        /// <summary>
        /// Called when this instance is being applied to a property and is about to be sealed.
        /// </summary>
        /// <param name="descriptor">A <see cref="PropertyDescriptor"/> describing the property to which the metadata is being applied.</param>
        /// <param name="targetType">The type associated with this metadata if this is type-specific metadata. If this is default metadata, this value is <c>null</c>.</param>
        protected internal virtual void OnApply(PropertyDescriptor descriptor, Type targetType)
        {
        }
    }
}
