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
using Prism.UI;
using Prism.UI.Controls;

namespace Prism.Native
{
    /// <summary>
    /// Defines a scrollable area for visual content that is native to a particular platform.
    /// These objects are meant to be paired with platform-agnostic <see cref="ScrollViewer"/> objects.
    /// </summary>
    public interface INativeScrollViewer : INativeElement, IScrollable
    {
        /// <summary>
        /// Occurs when the contents of the scroll viewer has been scrolled.
        /// </summary>
        event EventHandler Scrolled;

        /// <summary>
        /// Gets or sets the content of the scroll viewer.
        /// </summary>
        [CoreBehavior(CoreBehaviors.ChecksInequality | CoreBehaviors.TriggersChangeNotification)]
        object Content { get; set; }
    }
}
