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
using System.Diagnostics;

namespace Prism.UI.Controls
{
    /// <summary>
    /// Represents a panel that arranges its children in a single line that can be oriented either vertically or horizontally.
    /// </summary>
    public class StackPanel : Panel
    {
        #region Property Descriptors
        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:Orientation"/> property.
        /// </summary>
        public static PropertyDescriptor OrientationProperty { get; } = PropertyDescriptor.Create(nameof(Orientation), typeof(Orientation), typeof(StackPanel), new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));
        #endregion

        /// <summary>
        /// Gets or sets the direction in which the children are stacked.
        /// </summary>
        public Orientation Orientation
        {
            get { return orientation; }
            set
            {
                if (value != orientation)
                {
                    orientation = value;
                    OnPropertyChanged(OrientationProperty);
                }
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Orientation orientation;

        /// <summary>
        /// Initializes a new instance of the <see cref="StackPanel"/> class.
        /// </summary>
        public StackPanel()
        {
        }

        /// <summary>
        /// Called when this instance is ready to arrange its children and returns the final rendering size of the object.
        /// </summary>
        /// <param name="constraints">The width and height that this instance should not exceed.</param>
        /// <returns>The final rendering size of the object as a <see cref="Size"/> instance.</returns>
        protected override Size ArrangeOverride(Size constraints)
        {
            var renderSize = constraints = base.ArrangeOverride(constraints);
            var location = new Point();

            foreach (var child in Children)
            {
                if (Orientation == Orientation.Vertical)
                {
                    child.Arrange(new Rectangle(location, new Size(constraints.Width, child.DesiredSize.Height)));
                    constraints.Height -= (child.RenderSize.Height + child.Margin.Top + child.Margin.Bottom);
                    location.Y += child.RenderSize.Height + child.Margin.Top + child.Margin.Bottom;
                }
                else
                {
                    child.Arrange(new Rectangle(location, new Size(child.DesiredSize.Width, constraints.Height)));
                    constraints.Width -= (child.RenderSize.Width + child.Margin.Left + child.Margin.Right);
                    location.X += child.RenderSize.Width + child.Margin.Left + child.Margin.Right;
                }
            }

            return renderSize;
        }

        /// <summary>
        /// Called when this instance is ready to be measured and returns the desired size of the object.
        /// </summary>
        /// <param name="constraints">The width and height that this instance should not exceed.</param>
        /// <returns>The desired size of the object as a <see cref="Size"/> instance.</returns>
        protected override Size MeasureOverride(Size constraints)
        {
            constraints = base.MeasureOverride(constraints);

            Size desiredSize = new Size();
            foreach (var child in Children)
            {
                child.Measure(constraints);

                if (Orientation == Orientation.Vertical)
                {
                    desiredSize.Width = Math.Min(Math.Max(desiredSize.Width, child.DesiredSize.Width), constraints.Width);
                    desiredSize.Height += child.DesiredSize.Height;

                    constraints.Height = Math.Max(constraints.Height - child.DesiredSize.Height, 0);
                }
                else
                {
                    desiredSize.Width += child.DesiredSize.Width;
                    desiredSize.Height = Math.Min(Math.Max(desiredSize.Height, child.DesiredSize.Height), constraints.Height);

                    constraints.Width = Math.Max(constraints.Width - child.DesiredSize.Width, 0);
                }
            }

            return desiredSize;
        }
    }
}
