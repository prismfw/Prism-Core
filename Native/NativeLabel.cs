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


using Prism.UI;
using Prism.UI.Controls;
using Prism.UI.Media;

namespace Prism.Native
{
    /// <summary>
    /// Defines a label that is native to a particular platform.
    /// These objects are meant to be paired with platform-agnostic <see cref="Label"/> objects.
    /// </summary>
    public interface INativeLabel : INativeElement
    {
        /// <summary>
        /// Gets or sets the font to use for displaying the text in the label.
        /// </summary>
        [CoreBehavior(CoreBehaviors.ChecksNullity)]
        object FontFamily { get; set; }

        /// <summary>
        /// Gets or sets the size of the text in the label.
        /// </summary>
        [CoreBehavior(CoreBehaviors.ChecksInequality | CoreBehaviors.ChecksRange | CoreBehaviors.TriggersChangeNotification)]
        double FontSize { get; set; }

        /// <summary>
        /// Gets or sets the style with which to render the text in the label.
        /// </summary>
        FontStyle FontStyle { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Brush"/> to apply to the text contents of the label.
        /// </summary>
        Brush Foreground { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Brush"/> to apply to the text contents when the label resides within a highlighted element.
        /// </summary>
        Brush HighlightBrush { get; set; }

        /// <summary>
        /// Gets or sets the text of the label.
        /// </summary>
        string Text { get; set; }

        /// <summary>
        /// Gets or sets the alignment of the text within the label.
        /// </summary>
        TextAlignment TextAlignment { get; set; }

        /// <summary>
        /// Sets the maximum number of lines of text that the label can show.  A value of 0 means there is no limit.
        /// </summary>
        /// <param name="maxLines"></param>
        void SetMaxLines([CoreBehavior(CoreBehaviors.ChecksRange)]int maxLines);
    }
}
