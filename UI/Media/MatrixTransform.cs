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


namespace Prism.UI.Media
{
    /// <summary>
    /// Represents a transformation that uses an arbitrary affine transformation matrix to manipulate an object in two-dimensional space.
    /// </summary>
    public class MatrixTransform : Transform
    {
        #region Property Descriptors
        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:Matrix"/> property.
        /// </summary>
        public static PropertyDescriptor MatrixProperty { get; } = PropertyDescriptor.Create(nameof(Matrix), typeof(Matrix), typeof(MatrixTransform));
        #endregion

        /// <summary>
        /// Gets or sets the affine transformation matrix.
        /// </summary>
        public Matrix Matrix
        {
            get { return nativeObject.Value; }
            set
            {
                if (value != nativeObject.Value)
                {
                    nativeObject.Value = value;
                    OnPropertyChanged(MatrixProperty);
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MatrixTransform"/> class.
        /// </summary>
        public MatrixTransform()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MatrixTransform"/> class.
        /// </summary>
        /// <param name="matrix">The affine transformation matrix.</param>
        public MatrixTransform(Matrix matrix)
        {
            Matrix = matrix;
        }
    }
}
