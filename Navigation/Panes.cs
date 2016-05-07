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
using System.Diagnostics.CodeAnalysis;
using Prism.UI;

namespace Prism
{
    /// <summary>
    /// Describes the available built-in panes that views can be rendered within.
    /// </summary>
    [Flags]
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue", Justification = "'Unknown' is preferred over 'None' in the context that this is used.")]
    public enum Panes
    {
        /// <summary>
        /// The pane is unknown.
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// The master pane of a <see cref="SplitView"/>, or the full-screen pane when a <see cref="SplitView"/> is not used.
        /// </summary>
        Master = 1,
        /// <summary>
        /// The detail pane of a <see cref="SplitView"/>.
        /// </summary>
        Detail = 2,
        /// <summary>
        /// A pane that presents its contents modally.
        /// </summary>
        Modal = 4
    }
}
