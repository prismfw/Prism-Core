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
using System.Diagnostics;

namespace Prism
{
    /// <summary>
    /// Represents the method that is invoked when the value of a property on a <see cref="FrameworkObject"/> is changed.
    /// </summary>
    /// <param name="obj">The <see cref="FrameworkObject"/> whose property has changed.</param>
    /// <param name="property">A <see cref="PropertyDescriptor"/> describing the property that has changed.</param>
    public delegate void PropertyValueChangedCallback(FrameworkObject obj, PropertyDescriptor property);

    /// <summary>
    /// Represents metadata that is to be applied to a property that exists on a type deriving from the <see cref="FrameworkObject"/> class. 
    /// </summary>
    public class FrameworkPropertyMetadata : PropertyMetadata
    {
        /// <summary>
        /// Gets or sets a value indicating whether changes to the value of the property can affect the arrangement of the property's owner.
        /// </summary>
        public bool AffectsArrange
        {
            get { return affectsArrange ?? false; }
            set
            {
                if (IsSealed)
                {
                    throw new InvalidOperationException(Resources.Strings.PropertyMetadataHasBeenSealed);
                }

                affectsArrange = value;
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool? affectsArrange;

        /// <summary>
        /// Gets or sets a value indicating whether changes to the value of the property can affect the measurement of the property's owner.
        /// </summary>
        public bool AffectsMeasure
        {
            get { return affectsMeasure ?? false; }
            set
            {
                if (IsSealed)
                {
                    throw new InvalidOperationException(Resources.Strings.PropertyMetadataHasBeenSealed);
                }

                affectsMeasure = value;
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool? affectsMeasure;

        /// <summary>
        /// Gets or sets a method to invoke whenever the value of the property changes.
        /// </summary>
        public PropertyValueChangedCallback ValueChangedCallback { get; set; }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameworkPropertyMetadata"/> class.
        /// </summary>
        /// <param name="options">The options that specify the behavioral characteristics of the property.</param>
        public FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions options)
            : base((PropertyMetadataOptions)options)
        {
            if ((options & FrameworkPropertyMetadataOptions.AffectsArrange) != 0)
            {
                affectsArrange = true;
            }

            if ((options & FrameworkPropertyMetadataOptions.AffectsMeasure) != 0)
            {
                affectsMeasure = true;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FrameworkPropertyMetadata"/> class.
        /// </summary>
        /// <param name="valueChangedCallback">A method to invoke whenever the value of the property changes.</param>
        public FrameworkPropertyMetadata(PropertyValueChangedCallback valueChangedCallback)
            : base(PropertyMetadataOptions.None)
        {
            ValueChangedCallback = valueChangedCallback;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FrameworkPropertyMetadata"/> class.
        /// </summary>
        /// <param name="options">The options that specify the behavioral characteristics of the property.</param>
        /// <param name="valueChangedCallback">A method to invoke whenever the value of the property changes.</param>
        public FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions options, PropertyValueChangedCallback valueChangedCallback)
            : this(options)
        {
            ValueChangedCallback = valueChangedCallback;
        }

        /// <summary>
        /// Merges this metadata with the base metadata.
        /// </summary>
        /// <param name="baseMetadata">The base metadata to merge with the values of this instance.</param>
        /// <param name="descriptor">A <see cref="PropertyDescriptor"/> describing the property to which the metadata will be applied.</param>
        protected internal override void Merge(PropertyMetadata baseMetadata, PropertyDescriptor descriptor)
        {
            base.Merge(baseMetadata, descriptor);
            if (IsSealed || baseMetadata == null)
            {
                return;
            }

            var fwMetadata = baseMetadata as FrameworkPropertyMetadata;
            if (fwMetadata != null)
            {
                if (!affectsArrange.HasValue)
                {
                    affectsArrange = fwMetadata.affectsArrange;
                }

                if (!affectsMeasure.HasValue)
                {
                    affectsMeasure = fwMetadata.affectsMeasure;
                }
                
                ValueChangedCallback += fwMetadata.ValueChangedCallback;
            }
        }
    }
}
