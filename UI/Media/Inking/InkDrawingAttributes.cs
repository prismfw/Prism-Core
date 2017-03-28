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
using System.Diagnostics.CodeAnalysis;

namespace Prism.UI.Media.Inking
{
    /// <summary>
    /// Represents the rendering characteristics of an ink stroke.
    /// </summary>
    public sealed class InkDrawingAttributes : FrameworkObject
    {
        #region Property Descriptors
        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:Color"/> property.
        /// </summary>
        public static PropertyDescriptor ColorProperty { get; } = PropertyDescriptor.Create(nameof(Color), typeof(Color), typeof(InkDrawingAttributes));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:PenTip"/> property.
        /// </summary>
        public static PropertyDescriptor PenTipProperty { get; } = PropertyDescriptor.Create(nameof(PenTip), typeof(PenTipShape), typeof(InkDrawingAttributes));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:Size"/> property.
        /// </summary>
        public static PropertyDescriptor SizeProperty { get; } = PropertyDescriptor.Create(nameof(Size), typeof(double), typeof(InkDrawingAttributes));
        #endregion

        /// <summary>
        /// Gets or sets the color of the ink stroke.
        /// </summary>
        public Color Color
        {
            get { return color; }
            set
            {
                if (value != color)
                {
                    color = value;
                    OnPropertyChanged(ColorProperty);
                }
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Color color;

        /// <summary>
        /// Gets or sets the shape of the pen tip when drawing the ink stroke.
        /// </summary>
        public PenTipShape PenTip
        {
            get { return penTip; }
            set
            {
                if (value != penTip)
                {
                    penTip = value;
                    OnPropertyChanged(PenTipProperty);
                }
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private PenTipShape penTip;

        /// <summary>
        /// Gets or sets the size of the ink stroke.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", Justification = "Exception parameter refers to property name for easier understanding of invalid value.")]
        public double Size
        {
            get { return size; }
            set
            {
                if (value != size)
                {
                    if (double.IsNaN(value) || double.IsInfinity(value))
                    {
                        throw new ArgumentException(Resources.Strings.ValueCannotBeNaNOrInfinity, nameof(Size));
                    }

                    if (value < 0)
                    {
                        throw new ArgumentOutOfRangeException(nameof(Size), Resources.Strings.ValueCannotBeLessThanZero);
                    }

                    size = value;
                    OnPropertyChanged(SizeProperty);
                }
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private double size;

        /// <summary>
        /// Initializes a new instance of the <see cref="InkDrawingAttributes"/> class.
        /// </summary>
        public InkDrawingAttributes()
        {
            Color = (Color)Application.Current.FindResource(SystemResources.AltColorHighKey);
            PenTip = PenTipShape.Circle;
            Size = 2;
        }
    }
}
