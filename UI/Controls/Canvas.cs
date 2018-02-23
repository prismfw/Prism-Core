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


using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Prism.Resources;

#if !DEBUG
using System.Diagnostics;
#endif

namespace Prism.UI.Controls
{
    /// <summary>
    /// Represents a panel that arranges its child elements using absolute coordinate values.
    /// </summary>
    public class Canvas : Panel
    {
#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private static readonly ConditionalWeakTable<Element, CanvasPosition> elements = new ConditionalWeakTable<Element, CanvasPosition>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Canvas"/> class.
        /// </summary>
        public Canvas()
        {
        }

        /// <summary>
        /// Gets the X-axis value of the left edge of the specified element.
        /// </summary>
        /// <param name="element">The element from which to get the left value.</param>
        /// <returns>The left value as a <see cref="double"/> instance.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="element"/> is <c>null</c>.</exception>
        public static double GetLeft(Element element)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            CanvasPosition position;
            return elements.TryGetValue(element, out position) ? position.Left : double.NaN;
        }

        /// <summary>
        /// Gets the Y-axis value of the top edge of the specified element.
        /// </summary>
        /// <param name="element">The element from which to get the top value.</param>
        /// <returns>The top value as a <see cref="double"/> instance.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="element"/> is <c>null</c>.</exception>
        public static double GetTop(Element element)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            CanvasPosition position;
            return elements.TryGetValue(element, out position) ? position.Top : double.NaN;
        }

        /// <summary>
        /// Sets the X-axis value of the left edge of the specified element.
        /// </summary>
        /// <param name="element">The element for which to set the left value.</param>
        /// <param name="value">The X-axis value of the left edge.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="element"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="value"/> is not a number or is infinite.</exception>
        public static void SetLeft(Element element, double value)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            if (double.IsNaN(value) || double.IsInfinity(value))
            {
                throw new ArgumentException(Strings.ValueCannotBeNaNOrInfinity, nameof(value));
            }

            var position = elements.GetOrCreateValue(element);
            if (position.Left != value)
            {
                position.Left = value;

                var parent = element.Parent as Canvas;
                if (parent != null)
                {
                    parent.InvalidateMeasure();
                    parent.InvalidateArrange();
                }
            }
        }

        /// <summary>
        /// Sets the X-axis value of the left edge of the specified element.
        /// </summary>
        /// <param name="element">The element for which to set the left value.</param>
        /// <param name="value">The X-axis value of the left edge.</param>
        /// <param name="platforms">The platforms on which the value should be set.  Platforms that are not specified will not attempt to set the value.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="element"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="value"/> is not a number or is infinite.</exception>
        public static void SetLeft(Element element, double value, PlatformMask platforms)
        {
            if (platforms.HasFlag(Application.Current.Platform))
            {
                SetLeft(element, value);
            }
        }

        /// <summary>
        /// Sets the Y-axis value of the top edge of the specified element.
        /// </summary>
        /// <param name="element">The element for which to set the top value.</param>
        /// <param name="value">The Y-axis value of the top edge.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="element"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="value"/> is not a number or is infinite.</exception>
        public static void SetTop(Element element, double value)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            if (double.IsNaN(value) || double.IsInfinity(value))
            {
                throw new ArgumentException(Strings.ValueCannotBeNaNOrInfinity, nameof(value));
            }

            var position = elements.GetOrCreateValue(element);
            if (position.Top != value)
            {
                position.Top = value;

                var parent = element.Parent as Canvas;
                if (parent != null)
                {
                    parent.InvalidateMeasure();
                    parent.InvalidateArrange();
                }
            }
        }

        /// <summary>
        /// Sets the Y-axis value of the top edge of the specified element.
        /// </summary>
        /// <param name="element">The element for which to set the top value.</param>
        /// <param name="value">The Y-axis value of the top edge.</param>
        /// <param name="platforms">The platforms on which the value should be set.  Platforms that are not specified will not attempt to set the value.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="element"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="value"/> is not a number or is infinite.</exception>
        public static void SetTop(Element element, double value, PlatformMask platforms)
        {
            if (platforms.HasFlag(Application.Current.Platform))
            {
                SetTop(element, value);
            }
        }

        /// <summary>
        /// Called when this instance is ready to arrange its children and returns the final rendering size of the object.
        /// </summary>
        /// <param name="constraints">The width and height that this instance should not exceed.</param>
        /// <returns>The final rendering size of the object as a <see cref="Size"/> instance.</returns>
        protected override Size ArrangeOverride(Size constraints)
        {
            constraints = base.ArrangeOverride(constraints);
            foreach (var child in Children)
            {
                CanvasPosition position;
                elements.TryGetValue(child, out position);

                var location = position == null ? new Point() :
                    new Point(double.IsNaN(position.Left) ? 0 : position.Left, double.IsNaN(position.Top) ? 0 : position.Top);

                child.Arrange(new Rectangle(location, child.DesiredSize));
            }

            return constraints;
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
                CanvasPosition position;
                elements.TryGetValue(child, out position);

                var location = position == null || child.Visibility == Visibility.Collapsed ? new Point() :
                    new Point(double.IsNaN(position.Left) ? 0 : position.Left,
                    double.IsNaN(position.Top) ? 0 : position.Top);

                child.Measure(new Size(constraints.Width - location.X, constraints.Height - location.Y));

                desiredSize.Width = Math.Min(Math.Max(Math.Max(location.X + child.DesiredSize.Width, 0), desiredSize.Width), constraints.Width);
                desiredSize.Height = Math.Min(Math.Max(Math.Max(location.Y + child.DesiredSize.Height, 0), desiredSize.Height), constraints.Height);
            }

            return desiredSize;
        }

        [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Class is instantiated through ConditionalWeakTable.GetOrCreateValue method.")]
        private class CanvasPosition
        {
            public double Left = double.NaN;
            public double Top = double.NaN;
        }
    }
}
