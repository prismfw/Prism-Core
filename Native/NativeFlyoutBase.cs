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
using Prism.UI.Controls;
using Prism.UI.Media;

namespace Prism.Native
{
    /// <summary>
    /// Defines common functionality between different types of flyout objects that are native to a particular platform.
    /// These objects are meant to be paired with platform-agnostic <see cref="FlyoutBase"/> objects.
    /// </summary>
    public interface INativeFlyoutBase : INativeVisual
    {
        /// <summary>
        /// Occurs when the flyout has been closed.
        /// </summary>
        event EventHandler Closed;

        /// <summary>
        /// Occurs when the flyout has been opened.
        /// </summary>
        event EventHandler Opened;

        /// <summary>
        /// Gets or sets the background for the flyout.
        /// </summary>
        Brush Background { get; set; }

        /// <summary>
        /// Gets or sets the placement of the flyout in relation to its placement target.
        /// </summary>
        FlyoutPlacement Placement { get; set; }

        /// <summary>
        /// Dismisses the flyout.
        /// </summary>
        void Hide();

        /// <summary>
        /// Presents the flyout and positions it relative to the specified placement target.
        /// </summary>
        /// <param name="placementTarget">The object to use as the flyout's placement target.</param>
        void ShowAt([CoreBehavior(CoreBehaviors.ChecksNullity)]object placementTarget);
    }
}
