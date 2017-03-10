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


using System.Threading.Tasks;
using Prism.UI;

namespace Prism.Native
{
    /// <summary>
    /// Defines a status bar that is native to a particular platform.
    /// These objects are meant to be paired with platform-agnostic <see cref="StatusBar"/> objects.
    /// </summary>
    public interface INativeStatusBar
    {
        /// <summary>
        /// Gets or sets the background color for the status bar.
        /// </summary>
        Color BackgroundColor { get; set; }

        /// <summary>
        /// Gets a rectangle describing the area that the status bar is consuming.
        /// </summary>
        Rectangle Frame { get; }

        /// <summary>
        /// Gets a value indicating whether the status bar is visible.
        /// </summary>
        bool IsVisible { get; }

        /// <summary>
        /// Gets or sets the style of the status bar.
        /// </summary>
        StatusBarStyle Style { get; set; }

        /// <summary>
        /// Hides the status bar from view.
        /// </summary>
        Task HideAsync();

        /// <summary>
        /// Shows the status bar if it is not visible.
        /// </summary>
        Task ShowAsync();
    }
}
