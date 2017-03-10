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
    /// Represents a panel that arranges its children sequentially in a row or column
    /// and breaks to the next row or column once the edge of the panel is reached.
    /// </summary>
    public class WrapPanel : Panel
    {
        #region Property Descriptors
        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:HorizontalContentAlignment"/> property.
        /// </summary>
        public static PropertyDescriptor HorizontalContentAlignmentProperty { get; } = PropertyDescriptor.Create(nameof(HorizontalContentAlignment), typeof(HorizontalAlignment), typeof(WrapPanel), new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.AffectsArrange));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:Orientation"/> property.
        /// </summary>
        public static PropertyDescriptor OrientationProperty { get; } = PropertyDescriptor.Create(nameof(Orientation), typeof(Orientation), typeof(WrapPanel), new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:VerticalContentAlignment"/> property.
        /// </summary>
        public static PropertyDescriptor VerticalContentAlignmentProperty { get; } = PropertyDescriptor.Create(nameof(VerticalContentAlignment), typeof(VerticalAlignment), typeof(WrapPanel), new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.AffectsArrange));
        #endregion

        /// <summary>
        /// Gets or sets the manner in which the children of the panel are horizontally aligned within their respective rows.
        /// </summary>
        public HorizontalAlignment HorizontalContentAlignment
        {
            get { return horizontalContentAlignment; }
            set
            {
                if (value != horizontalContentAlignment)
                {
                    horizontalContentAlignment = value;
                    OnPropertyChanged(HorizontalContentAlignmentProperty);
                }
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private HorizontalAlignment horizontalContentAlignment;

        /// <summary>
        /// Gets or sets the direction in which the children are arranged.
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
        /// Gets or sets the manner in which the children of the panel are vertically aligned within their respective columns.
        /// </summary>
        public VerticalAlignment VerticalContentAlignment
        {
            get { return verticalContentAlignment; }
            set
            {
                if (value != verticalContentAlignment)
                {
                    verticalContentAlignment = value;
                    OnPropertyChanged(VerticalContentAlignmentProperty);
                }
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private VerticalAlignment verticalContentAlignment;

        /// <summary>
        /// Initializes a new instance of the <see cref="WrapPanel"/> class.
        /// </summary>
        public WrapPanel()
        {
        }

        /// <summary>
        /// Called when this instance is ready to arrange its children and returns the final rendering size of the object.
        /// </summary>
        /// <param name="constraints">The width and height that this instance should not exceed.</param>
        /// <returns>The final rendering size of the object as a <see cref="Size"/> instance.</returns>
        protected override Size ArrangeOverride(Size constraints)
        {
            var renderSize = base.ArrangeOverride(constraints);

            Rectangle frame = new Rectangle();
            int currentIndex = 0;

            for (int i = 0; i < Children.Count; i++)
            {
                var child = Children[i];
                if (orientation == Orientation.Vertical)
                {
                    if (frame.Height > 0 && frame.Height + child.DesiredSize.Height > renderSize.Height)
                    {
                        ArrangeVertical(currentIndex, i, frame, renderSize.Height);
                        frame.X += frame.Width;
                        frame.Width = child.DesiredSize.Width;
                        frame.Height = child.DesiredSize.Height;

                        currentIndex = i;
                    }
                    else
                    {
                        frame.Width = Math.Max(frame.Width, child.DesiredSize.Width);
                        frame.Height += child.DesiredSize.Height;
                    }
                }
                else
                {
                    if (frame.Width > 0 && frame.Width + child.DesiredSize.Width > renderSize.Width)
                    {
                        ArrangeHorizontal(currentIndex, i, frame, renderSize.Width);
                        frame.Y += frame.Height;
                        frame.Width = child.DesiredSize.Width;
                        frame.Height = child.DesiredSize.Height;

                        currentIndex = i;
                    }
                    else
                    {
                        frame.Width += child.DesiredSize.Width;
                        frame.Height = Math.Max(frame.Height, child.DesiredSize.Height);
                    }
                }
            }

            if (orientation == Orientation.Vertical)
            {
                ArrangeVertical(currentIndex, Children.Count, frame, renderSize.Height);
            }
            else
            {
                ArrangeHorizontal(currentIndex, Children.Count, frame, renderSize.Width);
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

            double width = 0;
            double height = 0;
            var desiredSize = new Size();

            foreach (var child in Children)
            {
                child.Measure(constraints);

                if (orientation == Orientation.Vertical)
                {
                    if (height > 0 && height + child.DesiredSize.Height > constraints.Height)
                    {
                        desiredSize.Width += width;
                        desiredSize.Height = Math.Max(desiredSize.Height, height);

                        width = child.DesiredSize.Width;
                        height = child.DesiredSize.Height;
                    }
                    else
                    {
                        width = Math.Max(width, child.DesiredSize.Width);
                        height += child.DesiredSize.Height;
                    }
                }
                else
                {
                    if (width > 0 && width + child.DesiredSize.Width > constraints.Width)
                    {
                        desiredSize.Width = Math.Max(desiredSize.Width, width);
                        desiredSize.Height += height;

                        width = child.DesiredSize.Width;
                        height = child.DesiredSize.Height;
                    }
                    else
                    {
                        width += child.DesiredSize.Width;
                        height = Math.Max(height, child.DesiredSize.Height);
                    }
                }
            }

            if (orientation == Orientation.Vertical)
            {
                desiredSize.Width += width;
                desiredSize.Height = Math.Max(desiredSize.Height, height);
            }
            else
            {
                desiredSize.Width = Math.Max(desiredSize.Width, width);
                desiredSize.Height += height;
            }

            return desiredSize;
        }

        private void ArrangeHorizontal(int startIndex, int endIndex, Rectangle frame, double renderWidth)
        {
            switch (HorizontalContentAlignment)
            {
                case HorizontalAlignment.Center:
                    frame.X = (renderWidth - frame.Width) / 2;
                    break;
                case HorizontalAlignment.Right:
                    frame.X = renderWidth - frame.Width;
                    break;
                default:
                    frame.X = 0;
                    break;
            }

            for (; startIndex < endIndex; startIndex++)
            {
                var currentChild = Children[startIndex];
                currentChild.Arrange(new Rectangle(frame.X, frame.Y, currentChild.DesiredSize.Width, frame.Height));

                frame.X += currentChild.DesiredSize.Width;
            }
        }

        private void ArrangeVertical(int startIndex, int endIndex, Rectangle frame, double renderHeight)
        {
            switch (VerticalContentAlignment)
            {
                case VerticalAlignment.Center:
                    frame.Y = (renderHeight - frame.Height) / 2;
                    break;
                case VerticalAlignment.Bottom:
                    frame.Y = renderHeight - frame.Height;
                    break;
                default:
                    frame.Y = 0;
                    break;
            }
            
            for (; startIndex < endIndex; startIndex++)
            {
                var currentChild = Children[startIndex];
                currentChild.Arrange(new Rectangle(frame.X, frame.Y, frame.Width, currentChild.DesiredSize.Height));

                frame.Y += currentChild.DesiredSize.Height;
            }
        }
    }
}
