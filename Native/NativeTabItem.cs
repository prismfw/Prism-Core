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


using Prism.UI.Controls;
using Prism.UI.Media;

namespace Prism.Native
{
    /// <summary>
    /// Defines a tab item that is native to a particular platform.
    /// These objects are meant to be paired with platform-agnostic <see cref="TabItem"/> objects.
    /// </summary>
    public interface INativeTabItem : INativeVisual
    {
        /// <summary>
        /// Gets or sets the object that acts as the content of the item.
        /// </summary>
        [CoreBehavior(CoreBehaviors.ChecksInequality | CoreBehaviors.TriggersChangeNotification)]
        object Content { get; set; }

        /// <summary>
        /// Gets or sets the font to use for displaying the title text.
        /// </summary>
        [CoreBehavior(CoreBehaviors.ChecksNullity)]
        object FontFamily { get; set; }

        /// <summary>
        /// Gets or sets the size of the title text.
        /// </summary>
        [CoreBehavior(CoreBehaviors.ChecksRange)]
        double FontSize { get; set; }

        /// <summary>
        /// Gets or sets the style with which to render the title text.
        /// </summary>
        FontStyle FontStyle { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Brush"/> to apply to the title.
        /// </summary>
        Brush Foreground { get; set; }

        /// <summary>
        /// Gets or sets an <see cref="INativeImageSource"/> for an image to display with the item.
        /// </summary>
        INativeImageSource Image { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user can interact with the item.
        /// </summary>
        bool IsEnabled { get; set; }

        /// <summary>
        /// Gets or sets the title for the item.
        /// </summary>
        string Title { get; set; }
    }
}
