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


using Prism.UI;

namespace Prism.Native
{
    /// <summary>
    /// Defines a split view that is native to a particular platform.
    /// These objects are meant to be paired with platform-agnostic <see cref="SplitView"/> objects.
    /// </summary>
    [CoreBehavior(CoreBehaviors.MeasuresByContent)]
    public interface INativeSplitView : INativeVisual
    {
        /// <summary>
        /// Gets the actual width of the detail pane.
        /// </summary>
        double ActualDetailWidth { get; }

        /// <summary>
        /// Gets the actual width of the master pane.
        /// </summary>
        double ActualMasterWidth { get; }

        /// <summary>
        /// Gets or sets the object that acts as the content for the detail pane.
        /// </summary>
        [CoreBehavior(CoreBehaviors.ChecksInequality | CoreBehaviors.TriggersChangeNotification)]
        object DetailContent { get; set; }

        /// <summary>
        /// Gets or sets the object that acts as the content for the master pane.
        /// </summary>
        [CoreBehavior(CoreBehaviors.ChecksInequality | CoreBehaviors.TriggersChangeNotification)]
        object MasterContent { get; set; }

        /// <summary>
        /// Gets or sets the maximum width of the master pane.
        /// </summary>
        [CoreBehavior(CoreBehaviors.ChecksRange | CoreBehaviors.ExpectsEarlyChangeNotification)]
        double MaxMasterWidth { get; set; }

        /// <summary>
        /// Gets or sets the minimum width of the master pane.
        /// </summary>
        [CoreBehavior(CoreBehaviors.ChecksRange | CoreBehaviors.ExpectsEarlyChangeNotification)]
        double MinMasterWidth { get; set; }

        /// <summary>
        /// Gets or sets the preferred width of the master pane as a percentage of the width of the split view.
        /// Valid values are between 0.0 and 1.0.
        /// </summary>
        [CoreBehavior(CoreBehaviors.ChecksRange | CoreBehaviors.ExpectsEarlyChangeNotification)]
        double PreferredMasterWidthRatio { get; set; }
    }
}
