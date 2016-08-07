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
using System.Collections;
using Prism.UI;
using Prism.UI.Media;

namespace Prism.Native
{
    /// <summary>
    /// Defines a tab view that is native to a particular platform.
    /// These objects are meant to be paired with platform-agnostic <see cref="TabView"/> objects.
    /// </summary>
    public interface INativeTabView : INativeVisual
    {
        /// <summary>
        /// Occurs when a tab item is selected.
        /// </summary>
        event EventHandler<NativeItemSelectedEventArgs> TabItemSelected;

        /// <summary>
        /// Gets or sets the background for the view.
        /// </summary>
        Brush Background { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Brush"/> to apply to the selected tab item.
        /// </summary>
        Brush Foreground { get; set; }

        /// <summary>
        /// Gets or sets the zero-based index of the selected tab item.
        /// </summary>
        int SelectedIndex { get; set; }

        /// <summary>
        /// Gets the size and location of the bar that contains the tab items.
        /// </summary>
        Rectangle TabBarFrame { get; }

        /// <summary>
        /// Gets a list of the tab items that are a part of the view.
        /// </summary>
        IList TabItems { get; }
    }
}
