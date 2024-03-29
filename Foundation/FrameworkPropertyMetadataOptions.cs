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
using Prism.Data;

namespace Prism
{
    /// <summary>
    /// Describes the behavioral characteristics that pertain to a particular property on a type deriving from the <see cref="FrameworkObject"/> class.
    /// </summary>
    [Flags]
    public enum FrameworkPropertyMetadataOptions
    {
        /// <summary>
        /// No options are specified; the default behavior is used.
        /// </summary>
        None = 0,
        /// <summary>
        /// The property uses a two-way binding when it is the target of a data binding whose mode is set to <see cref="BindingMode.Default"/>.
        /// </summary>
        BindsTwoWayByDefault = 1,
        /// <summary>
        /// Changes to the value of the property can affect the arrangement of the property's owner.
        /// </summary>
        AffectsArrange = 1024,
        /// <summary>
        /// Changes to the value of the property can affect the measurement of the property's owner.
        /// </summary>
        AffectsMeasure = 2048,
        /// <summary>
        /// Changes to the value of the property can affect the arrangement of the parent of the property's owner.
        /// </summary>
        AffectsParentArrange = 4096,
        /// <summary>
        /// Changes to the value of the property can affect the measurement of the parent of the property's owner.
        /// </summary>
        AffectsParentMeasure = 8192
    }
}
