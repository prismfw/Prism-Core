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
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using Prism.Resources;

#if !DEBUG
using System.Diagnostics;
#endif

namespace Prism.UI.Controls
{
    /// <summary>
    /// Represents a panel that arranges its child elements into various rows and columns.
    /// </summary>
    public class Grid : Panel
    {
        /// <summary>
        /// Gets a collection of the columns that make up the grid.
        /// </summary>
        public ColumnDefinitionCollection ColumnDefinitions { get; }

        /// <summary>
        /// Gets a collection of the rows that make up the grid.
        /// </summary>
        public RowDefinitionCollection RowDefinitions { get; }

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private static readonly ConditionalWeakTable<Element, GridPosition> elements = new ConditionalWeakTable<Element, GridPosition>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Grid"/> class.
        /// </summary>
        public Grid()
        {
            ColumnDefinitions = new ColumnDefinitionCollection();
            RowDefinitions = new RowDefinitionCollection();
        }

        /// <summary>
        /// Gets the zero-based column index for the specified element.
        /// </summary>
        /// <param name="element">The element from which to get the column index.</param>
        /// <returns>The zero-based column index as an <see cref="int"/> instance.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="element"/> is <c>null</c>.</exception>
        public static int GetColumn(Element element)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            GridPosition position;
            return elements.TryGetValue(element, out position) ? position.Column : 0;
        }

        /// <summary>
        /// Gets the number of columns that the specified element spans across.
        /// </summary>
        /// <param name="element">The element from which to get the column span.</param>
        /// <returns>The column span as an <see cref="int"/> instance.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="element"/> is <c>null</c>.</exception>
        public static int GetColumnSpan(Element element)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            GridPosition position;
            return elements.TryGetValue(element, out position) ? position.ColumnSpan : 1;
        }

        /// <summary>
        /// Gets the zero-based row index for the specified element.
        /// </summary>
        /// <param name="element">The element from which to get the row index.</param>
        /// <returns>The zero-based row index as an <see cref="int"/> instance.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="element"/> is <c>null</c>.</exception>
        public static int GetRow(Element element)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            GridPosition position;
            return elements.TryGetValue(element, out position) ? position.Row : 0;
        }

        /// <summary>
        /// Gets the number of rows that the specified element spans across.
        /// </summary>
        /// <param name="element">The element from which to get the row span.</param>
        /// <returns>The row span as an <see cref="int"/> instance.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="element"/> is <c>null</c>.</exception>
        public static int GetRowSpan(Element element)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            GridPosition position;
            return elements.TryGetValue(element, out position) ? position.RowSpan : 1;
        }

        /// <summary>
        /// Sets the zero-based column index for the specified element.
        /// </summary>
        /// <param name="element">The element for which to set the column index.</param>
        /// <param name="column">The zero-based index of the column.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="element"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="column"/> is less than zero.</exception>
        public static void SetColumn(Element element, int column)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            if (column < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(column), Strings.ValueCannotBeLessThanZero);
            }

            var position = elements.GetOrCreateValue(element);
            if (position.Column != column)
            {
                position.Column = column;

                var parent = element.Parent as Grid;
                if (parent != null)
                {
                    parent.InvalidateMeasure();
                    parent.InvalidateArrange();
                }
            }
        }

        /// <summary>
        /// Sets the zero-based column index for the specified element.
        /// </summary>
        /// <param name="element">The element for which to set the column index.</param>
        /// <param name="column">The zero-based index of the column.</param>
        /// <param name="platforms">The platforms on which the value should be set.  Platforms that are not specified will not attempt to set the value.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="element"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="column"/> is less than zero.</exception>
        public static void SetColumn(Element element, int column, PlatformMask platforms)
        {
            if (platforms.HasFlag(Application.Current.Platform))
            {
                SetColumn(element, column);
            }
        }

        /// <summary>
        /// Sets the number of columns that the specified element spans across.
        /// </summary>
        /// <param name="element">The element for which to set the column span.</param>
        /// <param name="columnSpan">The number of columns to span.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="element"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="columnSpan"/> is less than one.</exception>
        public static void SetColumnSpan(Element element, int columnSpan)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            if (columnSpan < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(columnSpan), Strings.ValueCannotBeLessThanOne);
            }

            var position = elements.GetOrCreateValue(element);
            if (position.ColumnSpan != columnSpan)
            {
                position.ColumnSpan = columnSpan;

                var parent = element.Parent as Grid;
                if (parent != null)
                {
                    parent.InvalidateMeasure();
                    parent.InvalidateArrange();
                }
            }
        }

        /// <summary>
        /// Sets the number of columns that the specified element spans across.
        /// </summary>
        /// <param name="element">The element for which to set the column span.</param>
        /// <param name="columnSpan">The number of columns to span.</param>
        /// <param name="platforms">The platforms on which the value should be set.  Platforms that are not specified will not attempt to set the value.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="element"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="columnSpan"/> is less than one.</exception>
        public static void SetColumnSpan(Element element, int columnSpan, PlatformMask platforms)
        {
            if (platforms.HasFlag(Application.Current.Platform))
            {
                SetColumnSpan(element, columnSpan);
            }
        }

        /// <summary>
        /// Sets the zero-based row index for the specified element.
        /// </summary>
        /// <param name="element">The element for which to set the row index.</param>
        /// <param name="row">The zero-based index of the row.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="element"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="row"/> is less than zero.</exception>
        public static void SetRow(Element element, int row)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            if (row < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(row), Strings.ValueCannotBeLessThanZero);
            }

            var position = elements.GetOrCreateValue(element);
            if (position.Row != row)
            {
                position.Row = row;

                var parent = element.Parent as Grid;
                if (parent != null)
                {
                    parent.InvalidateMeasure();
                    parent.InvalidateArrange();
                }
            }
        }

        /// <summary>
        /// Sets the zero-based row index for the specified element.
        /// </summary>
        /// <param name="element">The element for which to set the row index.</param>
        /// <param name="row">The zero-based index of the row.</param>
        /// <param name="platforms">The platforms on which the value should be set.  Platforms that are not specified will not attempt to set the value.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="element"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="row"/> is less than zero.</exception>
        public static void SetRow(Element element, int row, PlatformMask platforms)
        {
            if (platforms.HasFlag(Application.Current.Platform))
            {
                SetRow(element, row);
            }
        }

        /// <summary>
        /// Sets the number of rows that the specified element spans across.
        /// </summary>
        /// <param name="element">The element for which to set the row span.</param>
        /// <param name="rowSpan">The number of rows to span.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="element"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="rowSpan"/> is less than one.</exception>
        public static void SetRowSpan(Element element, int rowSpan)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            if (rowSpan < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(rowSpan), Strings.ValueCannotBeLessThanOne);
            }

            var position = elements.GetOrCreateValue(element);
            if (position.RowSpan != rowSpan)
            {
                position.RowSpan = rowSpan;

                var parent = element.Parent as Grid;
                if (parent != null)
                {
                    parent.InvalidateMeasure();
                    parent.InvalidateArrange();
                }
            }
        }

        /// <summary>
        /// Sets the number of rows that the specified element spans across.
        /// </summary>
        /// <param name="element">The element for which to set the row span.</param>
        /// <param name="rowSpan">The number of rows to span.</param>
        /// <param name="platforms">The platforms on which the value should be set.  Platforms that are not specified will not attempt to set the value.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="element"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="rowSpan"/> is less than one.</exception>
        public static void SetRowSpan(Element element, int rowSpan, PlatformMask platforms)
        {
            if (platforms.HasFlag(Application.Current.Platform))
            {
                SetRowSpan(element, rowSpan);
            }
        }

        /// <summary>
        /// Called when this instance is ready to arrange its children and returns the final rendering size of the object.
        /// </summary>
        /// <param name="constraints">The width and height that this instance should not exceed.</param>
        /// <returns>The final rendering size of the object as a <see cref="Size"/> instance.</returns>
        protected override Size ArrangeOverride(Size constraints)
        {
            var renderSize = base.ArrangeOverride(constraints);

            foreach (var child in Children)
            {
                int columnIndex = 0;
                int columnSpan = 1;
                int rowIndex = 0;
                int rowSpan = 1;

                GridPosition position;
                if (elements.TryGetValue(child, out position))
                {
                    columnIndex = Math.Max(0, Math.Min(position.Column, ColumnDefinitions.Count - 1));
                    columnSpan = Math.Max(1, Math.Min(position.Column + position.ColumnSpan, ColumnDefinitions.Count));
                    rowIndex = Math.Max(0, Math.Min(position.Row, RowDefinitions.Count - 1));
                    rowSpan = Math.Max(1, Math.Min(position.Row + position.RowSpan, RowDefinitions.Count));
                }

                Rectangle frame = new Rectangle();
                if (ColumnDefinitions.Count == 0)
                {
                    frame.Width = renderSize.Width;
                }
                else
                {
                    frame.X = ColumnDefinitions[columnIndex].Offset;
                    for (int i = columnIndex; i < columnSpan; i++)
                    {
                        frame.Width += ColumnDefinitions[i].ActualWidth;
                    }
                }

                if (RowDefinitions.Count == 0)
                {
                    frame.Height = renderSize.Height;
                }
                else
                {
                    frame.Y = RowDefinitions[rowIndex].Offset;
                    for (int i = rowIndex; i < rowSpan; i++)
                    {
                        frame.Height += RowDefinitions[i].ActualHeight;
                    }
                }

                child.Arrange(frame);
            }

            return renderSize;
        }

        /// <summary>
        /// Called when this instance is ready to be measured and returns the desired size of the object.
        /// </summary>
        /// <param name="constraints">The width and height that this instance should not exceed.</param>
        /// <returns>The desired size of the object as a <see cref="Size"/> instance.</returns>
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "Method cannot be easily reduced and any change may have unexpected consequences.")]
        [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = "Method cannot be easily reduced and any change may have unexpected consequences.")]
#if DEBUG
        [SuppressMessage("Microsoft.Performance", "CA1809:AvoidExcessiveLocals", Justification = "Compiling in release brings variable count beneath threshold.")]
#endif
        protected override Size MeasureOverride(Size constraints)
        {
            var availableSize = constraints = base.MeasureOverride(constraints);

            ColumnDefinition column;
            RowDefinition row;

            double[] columns;
            if (ColumnDefinitions.Count == 0)
            {
                columns = new double[] { 0 };
            }
            else
            {
                columns = new double[ColumnDefinitions.Count];
                for (int i = 0; i < columns.Length; i++)
                {
                    column = ColumnDefinitions[i];
                    double width = Math.Min(availableSize.Width, Math.Max(column.MinWidth, Math.Min(column.MaxWidth, column.Width.IsAbsolute ? column.Width.Value : 0)));
                    columns[i] = width;
                    availableSize.Width -= width;
                }
            }

            double[] rows;
            if (RowDefinitions.Count == 0)
            {
                rows = new double[] { 0 };
            }
            else
            {
                rows = new double[RowDefinitions.Count];
                for (int i = 0; i < rows.Length; i++)
                {
                    row = RowDefinitions[i];
                    double height = Math.Min(availableSize.Height, Math.Max(row.MinHeight, Math.Min(row.MaxHeight, row.Height.IsAbsolute ? row.Height.Value : 0)));
                    rows[i] = height;
                    availableSize.Height -= height;
                }
            }

            availableSize.Width = Math.Max(availableSize.Width, 0);
            availableSize.Height = Math.Max(availableSize.Height, 0);

            GridPosition position;
            int columnIndex;
            int columnSpan;
            int rowIndex;
            int rowSpan;

            List<Element> autoSingles = null;
            List<Element> autoSpanners = null;
            int autoCount = 0;
            foreach (var child in Children)
            {
                if (elements.TryGetValue(child, out position))
                {
                    columnIndex = Math.Min(position.Column, columns.Length - 1);
                    columnSpan = Math.Min(position.Column + position.ColumnSpan, columns.Length);
                    rowIndex = Math.Min(position.Row, rows.Length - 1);
                    rowSpan = Math.Min(position.Row + position.RowSpan, rows.Length);
                }
                else
                {
                    columnIndex = 0;
                    columnSpan = 1;
                    rowIndex = 0;
                    rowSpan = 1;
                }

                bool inAuto = false;
                bool inStar = false;
                for (int i = columnIndex; i < columnSpan; i++)
                {
                    column = i < ColumnDefinitions.Count ? ColumnDefinitions[i] : null;
                    if (column == null || !column.Width.IsAbsolute)
                    {
                        if (inAuto)
                        {
                            if (autoSpanners == null)
                            {
                                autoSpanners = new List<Element>();
                            }

                            autoSpanners.Add(child);
                            goto next; // suddenly, a wild goto appears!
                        }

                        inAuto = true;
                        if (column != null && column.Width.IsStar)
                        {
                            inStar = true;
                        }
                    }
                }

                inAuto = false;
                for (int i = rowIndex; i < rowSpan; i++)
                {
                    row = i < RowDefinitions.Count ? RowDefinitions[i] : null;
                    if (row == null || !row.Height.IsAbsolute)
                    {
                        if (inAuto)
                        {
                            if (autoSpanners == null)
                            {
                                autoSpanners = new List<Element>();
                            }

                            autoSpanners.Add(child);
                            goto next;
                        }

                        inAuto = true;
                        if (row != null && row.Height.IsStar)
                        {
                            inStar = true;
                        }
                    }
                }

                if (autoSingles == null)
                {
                    autoSingles = new List<Element>();
                }

                // children that do not reside within a star column or row should get measurement priority over children that do
                if (inStar)
                {
                    autoSingles.Add(child);
                }
                else
                {
                    autoSingles.Insert(autoCount++, child);
                }

                next:
                continue;
            }

            Size childConstraints;
            Size desiredSize;

            // measure the children that only span one auto/star column or row first to get initial auto/star values
            if (autoSingles != null)
            {
                foreach (var child in autoSingles)
                {
                    if (elements.TryGetValue(child, out position))
                    {
                        columnIndex = Math.Min(position.Column, columns.Length - 1);
                        columnSpan = Math.Min(position.Column + position.ColumnSpan, columns.Length);
                        rowIndex = Math.Min(position.Row, rows.Length - 1);
                        rowSpan = Math.Min(position.Row + position.RowSpan, rows.Length);
                    }
                    else
                    {
                        columnIndex = 0;
                        columnSpan = 1;
                        rowIndex = 0;
                        rowSpan = 1;
                    }

                    childConstraints = new Size();
                    for (int i = columnIndex; i < columnSpan; i++)
                    {
                        childConstraints.Width += columns[i];
                        column = i < ColumnDefinitions.Count ? ColumnDefinitions[i] : null;
                        if (column == null || !column.Width.IsAbsolute)
                        {
                            childConstraints.Width += availableSize.Width;
                        }
                    }

                    for (int i = rowIndex; i < rowSpan; i++)
                    {
                        childConstraints.Height += rows[i];
                        row = i < RowDefinitions.Count ? RowDefinitions[i] : null;
                        if (row == null || !row.Height.IsAbsolute)
                        {
                            childConstraints.Height += availableSize.Height;
                        }
                    }

                    child.Measure(childConstraints);

                    desiredSize = child.DesiredSize;
                    int autoIndex = 0;
                    for (int i = columnIndex; i < columnSpan; i++)
                    {
                        column = i < ColumnDefinitions.Count ? ColumnDefinitions[i] : null;
                        if (column != null && column.Width.IsAbsolute)
                        {
                            desiredSize.Width -= column.Width.Value;
                        }
                        else
                        {
                            autoIndex = i;
                        }
                    }

                    double autoLength = 0;
                    double currentLength = columns[autoIndex];

                    column = autoIndex < ColumnDefinitions.Count ? ColumnDefinitions[autoIndex] : null;
                    if (column == null)
                    {
                        autoLength = Math.Max(currentLength, desiredSize.Width);
                    }
                    else
                    {
                        autoLength = Math.Max(column.MinWidth, Math.Min(column.MaxWidth, Math.Max(currentLength, desiredSize.Width)));
                    }

                    if (autoLength > currentLength)
                    {
                        columns[autoIndex] = autoLength;
                        availableSize.Width -= (autoLength - currentLength);
                    }

                    for (int i = rowIndex; i < rowSpan; i++)
                    {
                        row = i < RowDefinitions.Count ? RowDefinitions[i] : null;
                        if (row != null && row.Height.IsAbsolute)
                        {
                            desiredSize.Height -= row.Height.Value;
                        }
                        else
                        {
                            autoIndex = i;
                        }
                    }

                    currentLength = rows[autoIndex];

                    row = autoIndex < RowDefinitions.Count ? RowDefinitions[autoIndex] : null;
                    if (row == null)
                    {
                        autoLength = Math.Max(currentLength, desiredSize.Height);
                    }
                    else
                    {
                        autoLength = Math.Max(row.MinHeight, Math.Min(row.MaxHeight, Math.Max(currentLength, desiredSize.Height)));
                    }

                    if (autoLength > currentLength)
                    {
                        rows[autoIndex] = autoLength;
                        availableSize.Height -= (autoLength - currentLength);
                    }
                }
            }

            // if any children span multiple autos/stars, measure them now and expand any autos that need it
            if (autoSpanners != null)
            {
                foreach (var child in autoSpanners)
                {
                    if (elements.TryGetValue(child, out position))
                    {
                        columnIndex = Math.Min(position.Column, columns.Length - 1);
                        columnSpan = Math.Min(position.Column + position.ColumnSpan, columns.Length);
                        rowIndex = Math.Min(position.Row, rows.Length - 1);
                        rowSpan = Math.Min(position.Row + position.RowSpan, rows.Length);
                    }
                    else
                    {
                        columnIndex = 0;
                        columnSpan = 1;
                        rowIndex = 0;
                        rowSpan = 1;
                    }

                    childConstraints = availableSize;
                    for (int i = columnIndex; i < columnSpan; i++)
                    {
                        childConstraints.Width += columns[i];
                    }

                    for (int i = rowIndex; i < rowSpan; i++)
                    {
                        childConstraints.Height += rows[i];
                    }

                    child.Measure(childConstraints);

                    desiredSize = child.DesiredSize;
                    int trueSpan = columnSpan - columnIndex;
                    for (int i = columnIndex; i < columnSpan; i++)
                    {
                        if (ColumnDefinitions[i].Width.IsAbsolute)
                        {
                            trueSpan--;
                        }

                        desiredSize.Width -= columns[i];
                    }

                    double oldValue;
                    double newValue;
                    double difference;
                    while (desiredSize.Width / trueSpan > 0)
                    {
                        for (int i = columnIndex; i < columnSpan; i++)
                        {
                            column = ColumnDefinitions[i];
                            if (!column.Width.IsAbsolute)
                            {
                                oldValue = columns[i];
                                newValue = Math.Min(availableSize.Width + oldValue, Math.Max(column.MinWidth, Math.Min(column.MaxWidth, oldValue + (desiredSize.Width / trueSpan))));
                                difference = newValue - oldValue;

                                if (difference > 0)
                                {
                                    availableSize.Width -= difference;
                                    desiredSize.Width -= difference;

                                    columns[i] = newValue;
                                }
                                else
                                {
                                    trueSpan--;
                                }
                            }
                        }

                        if (trueSpan == 0)
                        {
                            break;
                        }
                    }

                    trueSpan = rowSpan - rowIndex;
                    for (int i = rowIndex; i < rowSpan; i++)
                    {
                        if (RowDefinitions[i].Height.IsAbsolute)
                        {
                            trueSpan--;
                        }

                        desiredSize.Height -= rows[i];
                    }
                    
                    while (desiredSize.Height / trueSpan > 0)
                    {
                        for (int i = rowIndex; i < rowSpan; i++)
                        {
                            row = RowDefinitions[i];
                            if (!row.Height.IsAbsolute)
                            {
                                oldValue = rows[i];
                                newValue = Math.Min(availableSize.Height + oldValue, Math.Max(row.MinHeight, Math.Min(row.MaxHeight, oldValue + (desiredSize.Height / trueSpan))));
                                difference = newValue - oldValue;

                                if (difference > 0)
                                {
                                    availableSize.Height -= difference;
                                    desiredSize.Height -= difference;

                                    rows[i] = newValue;
                                }
                                else
                                {
                                    trueSpan--;
                                }
                            }
                        }

                        if (trueSpan == 0)
                        {
                            break;
                        }
                    }
                }
            }

            // divvy up star space and re-measure any children within them since their constraints may have changed
            int starIndex;
            double starTotal = availableSize.Width;
            double starWeight = 0;
            double starValue = 0;
            double finalValue = 0;
            List<int> starIndices = new List<int>(Math.Max(ColumnDefinitions.Count, RowDefinitions.Count));
            for (int i = 0; i < ColumnDefinitions.Count; i++)
            {
                column = ColumnDefinitions[i];
                if (column.Width.IsStar)
                {
                    starTotal += columns[i];
                    starWeight += column.Width.Value;
                    starIndices.Add(i);
                }
            }

            bool again;
            do
            {
                again = false;
                for (int i = 0; i < starIndices.Count; i++)
                {
                    starIndex = starIndices[i];
                    column = ColumnDefinitions[starIndex];

                    starValue = starTotal * (column.Width.Value / starWeight);
                    finalValue = Math.Max(column.MinWidth, Math.Min(column.MaxWidth, starValue));
                    columns[starIndex] = finalValue;

                    if (Math.Abs(finalValue - starValue) > 0.01)
                    {
                        starIndices.RemoveAt(i);
                        starTotal -= finalValue;
                        starWeight -= column.Width.Value;
                        again = true;
                        break;
                    }
                }
            }
            while (again);

            starTotal = availableSize.Height;
            starWeight = 0;
            starIndices.Clear();
            for (int i = 0; i < RowDefinitions.Count; i++)
            {
                row = RowDefinitions[i];
                if (row.Height.IsStar)
                {
                    starTotal += rows[i];
                    starWeight += row.Height.Value;
                    starIndices.Add(i);
                }
            }

            do
            {
                again = false;
                for (int i = 0; i < starIndices.Count; i++)
                {
                    starIndex = starIndices[i];
                    row = RowDefinitions[starIndex];

                    starValue = starTotal * (row.Height.Value / starWeight);
                    finalValue = Math.Max(row.MinHeight, Math.Min(row.MaxHeight, starValue));
                    rows[starIndex] = finalValue;

                    if (Math.Abs(finalValue - starValue) > 0.01)
                    {
                        starIndices.RemoveAt(i);
                        starTotal -= finalValue;
                        starWeight -= row.Height.Value;
                        again = true;
                        break;
                    }
                }
            }
            while (again);

            foreach (var child in Children)
            {
                if (elements.TryGetValue(child, out position))
                {
                    columnIndex = Math.Min(position.Column, columns.Length - 1);
                    columnSpan = Math.Min(position.Column + position.ColumnSpan, columns.Length);
                    rowIndex = Math.Min(position.Row, rows.Length - 1);
                    rowSpan = Math.Min(position.Row + position.RowSpan, rows.Length);
                }
                else
                {
                    columnIndex = 0;
                    columnSpan = 1;
                    rowIndex = 0;
                    rowSpan = 1;
                }

                bool inStar = false;
                childConstraints = new Size();
                for (int i = columnIndex; i < columnSpan; i++)
                {
                    childConstraints.Width += columns[i];
                    column = i < ColumnDefinitions.Count ? ColumnDefinitions[i] : null;
                    if (column != null && column.Width.IsStar)
                    {
                        inStar = true;
                    }
                }

                for (int i = rowIndex; i < rowSpan; i++)
                {
                    childConstraints.Height += rows[i];
                    row = i < RowDefinitions.Count ? RowDefinitions[i] : null;
                    if (row != null && row.Height.IsStar)
                    {
                        inStar = true;
                    }
                }

                if (inStar)
                {
                    child.Measure(childConstraints);
                }
            }

            double offset = 0;
            for (int i = 0; i < ColumnDefinitions.Count; i++)
            {
                column = ColumnDefinitions[i];
                column.Offset = offset;
                column.ActualWidth = Math.Max(column.MinWidth, Math.Min(column.MaxWidth, columns[i]));

                offset += column.ActualWidth;
            }

            offset = 0;
            for (int i = 0; i < RowDefinitions.Count; i++)
            {
                row = RowDefinitions[i];
                row.Offset = offset;
                row.ActualHeight = Math.Max(row.MinHeight, Math.Min(row.MaxHeight, rows[i]));

                offset += row.ActualHeight;
            }

            return new Size(Math.Min(columns.Sum(), constraints.Width), Math.Min(rows.Sum(), constraints.Height));
        }

        [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Class is instantiated through ConditionalWeakTable.GetOrCreateValue method.")]
        private class GridPosition
        {
            public int Column;
            public int ColumnSpan = 1;
            public int Row;
            public int RowSpan = 1;
        }
    }
}
