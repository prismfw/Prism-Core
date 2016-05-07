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


namespace Prism.UI.Controls
{
    /// <summary>
    /// Defines a UI container that is able to scroll its contents.
    /// </summary>
    public interface IScrollable
    {
        /// <summary>
        /// Gets or sets a value indicating whether the contents can be scrolled horizontally.
        /// </summary>
        bool CanScrollHorizontally { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the contents can be scrolled vertically.
        /// </summary>
        bool CanScrollVertically { get; set; }

        /// <summary>
        /// Gets the distance that the contents has been scrolled.
        /// </summary>
        Point ContentOffset { get; }

        /// <summary>
        /// Gets the size of the scrollable area.
        /// </summary>
        Size ContentSize { get; }

        /// <summary>
        /// Scrolls the contents to the specified offset.
        /// </summary>
        /// <param name="offset">The position to which to scroll the contents.</param>
        /// <param name="animate">Whether to animate the scrolling.</param>
        void ScrollTo(Point offset, Animate animate);
    }
}
